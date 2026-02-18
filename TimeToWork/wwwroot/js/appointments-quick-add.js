// Quick Add functionality for Appointments
(function ($) {
    'use strict';

    var DEBUG = false; // Set to true for debugging

    function log() {
        if (DEBUG && console && console.log) {
            console.log.apply(console, arguments);
        }
    }

    // Quick Add Client
    function initQuickAddClient() {
        $('#quickAddClientForm').on('submit', function (e) {
            e.preventDefault();

            var formData = {
                LastName: $('#quickClient_LastName').val(),
                FirstName: $('#quickClient_FirstName').val(),
                PhoneNumber: $('#quickClient_PhoneNumber').val()
            };

            log('Sending client data:', formData);

            $.ajax({
                url: '/Clients/QuickCreate',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function (response) {
                    log('Client response:', response);
                    if (response.success) {
                        var newOption = new Option(response.text, response.id, true, true);
                        $('#ClientId').append(newOption).trigger('change');
                        $('#quickAddClientModal').modal('hide');
                        $('#quickAddClientForm')[0].reset();
                        $('#quickAddClientErrors').addClass('d-none');
                        showSuccessToast('Client "' + response.text + '" created successfully!');
                    } else {
                        $('#quickAddClientErrors').removeClass('d-none').text(response.message);
                    }
                },
                error: function (xhr) {
                    log('Client error:', xhr.responseText);
                    var errorMsg = 'Error adding client. ';
                    if (xhr.responseText) {
                        try {
                            var errorData = JSON.parse(xhr.responseText);
                            errorMsg += errorData.message || xhr.responseText;
                        } catch (e) {
                            errorMsg += xhr.responseText;
                        }
                    }
                    $('#quickAddClientErrors').removeClass('d-none').text(errorMsg);
                }
            });
        });

        $('#quickAddClientModal').on('hidden.bs.modal', function () {
            $('#quickAddClientForm')[0].reset();
            $('#quickAddClientErrors').addClass('d-none').text('');
        });
    }

    // Quick Add Service
    function initQuickAddService() {
        $('#quickAddServiceForm').on('submit', function (e) {
            e.preventDefault();

            var formData = {
                ServiceName: $('#quickService_ServiceName').val(),
                ShortDescription: $('#quickService_ShortDescription').val(),
                Description: $('#quickService_Description').val(),
                Price: parseInt($('#quickService_Price').val()),
                ЕxecutionTimeHours: parseInt($('#quickService_Hours').val()),
                ЕxecutionTimeMinutes: parseInt($('#quickService_Minutes').val())
            };

            log('Sending service data:', formData);

            $.ajax({
                url: '/Services/QuickCreate',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function (response) {
                    log('Service response:', response);
                    if (response.success) {
                        var newOption = new Option(response.text, response.id, true, true);
                        $('#lstServiceId').append(newOption).trigger('change');
                        $('#quickAddServiceModal').modal('hide');
                        $('#quickAddServiceForm')[0].reset();
                        $('#quickAddServiceErrors').addClass('d-none');
                        showSuccessToast('Service "' + response.text + '" created successfully!');
                    } else {
                        $('#quickAddServiceErrors').removeClass('d-none').text(response.message);
                    }
                },
                error: function (xhr) {
                    log('Service error:', xhr.responseText);
                    var errorMsg = 'Error adding service. ';
                    if (xhr.responseText) {
                        try {
                            var errorData = JSON.parse(xhr.responseText);
                            errorMsg += errorData.message || xhr.responseText;
                        } catch (e) {
                            errorMsg += xhr.responseText;
                        }
                    }
                    $('#quickAddServiceErrors').removeClass('d-none').text(errorMsg);
                }
            });
        });

        $('#quickAddServiceModal').on('hidden.bs.modal', function () {
            $('#quickAddServiceForm')[0].reset();
            $('#quickAddServiceErrors').addClass('d-none').text('');
        });
    }

    // Quick Add ServiceProvider
    function initQuickAddServiceProvider(fillServiceProviderCallback) {
        // Load workplaces and services when modal opens
        $('#quickAddServiceProviderModal').on('show.bs.modal', function () {
            // Load workplaces
            $.ajax({
                url: '/ServiceProviders/GetPlaceOfWorks',
                type: 'GET',
                success: function (data) {
                    log('Workplaces loaded:', data);
                    var select = $('#quickServiceProvider_PlaceOfWorkID');
                    select.empty();
                    select.append('<option value="" selected disabled>Select Workplace</option>');
                    $.each(data, function (index, item) {
                        select.append($('<option></option>').val(item.value).text(item.text));
                    });
                }
            });

            // Load services
            $.ajax({
                url: '/ServiceProviders/GetServices',
                type: 'GET',
                success: function (data) {
                    log('Services loaded:', data);
                    var container = $('#quickServiceProvider_Services');
                    container.empty();

                    if (data && data.length > 0) {
                        $.each(data, function (index, service) {
                            var checkboxHtml =
                                '<div class="form-check">' +
                                '<input class="form-check-input service-checkbox" type="checkbox" value="' + service.id + '" id="service_' + service.id + '">' +
                                '<label class="form-check-label" for="service_' + service.id + '">' +
                                service.name +
                                '</label>' +
                                '</div>';
                            container.append(checkboxHtml);
                        });
                    } else {
                        container.html('<div class="text-muted">No services available</div>');
                    }
                },
                error: function () {
                    $('#quickServiceProvider_Services').html('<div class="text-danger">Error loading services</div>');
                }
            });
        });

        $('#quickAddServiceProviderForm').on('submit', function (e) {
            e.preventDefault();

            // Collect selected services
            var selectedServices = [];
            $('.service-checkbox:checked').each(function () {
                selectedServices.push(parseInt($(this).val()));
            });

            var formData = {
                LastName: $('#quickServiceProvider_LastName').val(),
                FirstName: $('#quickServiceProvider_FirstName').val(),
                HireDate: $('#quickServiceProvider_HireDate').val(),
                PlaceOfWorkID: parseInt($('#quickServiceProvider_PlaceOfWorkID').val()),
                SelectedServices: selectedServices
            };

            log('Sending service provider data:', formData);

            $.ajax({
                url: '/ServiceProviders/QuickCreate',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function (response) {
                    log('ServiceProvider response:', response);
                    if (response.success) {
                        $('#quickAddServiceProviderModal').modal('hide');
                        $('#quickAddServiceProviderForm')[0].reset();
                        $('#quickAddServiceProviderErrors').addClass('d-none');
                        showSuccessToast('Executor "' + response.text + '" created successfully!');

                        // Refresh service provider list if service is selected
                        var selectedServiceId = $('#lstServiceId').val();
                        if (selectedServiceId && fillServiceProviderCallback) {
                            log('Refreshing service provider list for service:', selectedServiceId);
                            fillServiceProviderCallback(document.getElementById('lstServiceId'), 'lstServiceProvider');
                        }
                    } else {
                        $('#quickAddServiceProviderErrors').removeClass('d-none').text(response.message);
                    }
                },
                error: function (xhr) {
                    log('ServiceProvider error:', xhr.responseText);
                    var errorMsg = 'Error adding executor. ';
                    if (xhr.responseText) {
                        try {
                            var errorData = JSON.parse(xhr.responseText);
                            errorMsg += errorData.message || xhr.responseText;
                        } catch (e) {
                            errorMsg += xhr.responseText;
                        }
                    }
                    $('#quickAddServiceProviderErrors').removeClass('d-none').text(errorMsg);
                }
            });
        });

        $('#quickAddServiceProviderModal').on('hidden.bs.modal', function () {
            $('#quickAddServiceProviderForm')[0].reset();
            $('#quickAddServiceProviderErrors').addClass('d-none').text('');
            $('#quickServiceProvider_Services').html('<div class="text-muted">Loading services...</div>');
        });
    }

    // Public API
    window.AppointmentsQuickAdd = {
        init: function (fillServiceProviderCallback) {
            initQuickAddClient();
            initQuickAddService();
            initQuickAddServiceProvider(fillServiceProviderCallback);
        },
        setDebug: function (enabled) {
            DEBUG = enabled;
        }
    };

})(jQuery);
