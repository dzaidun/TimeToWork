// Quick Add functionality for ServiceProviders
(function ($) {
    'use strict';

    // Quick Add PlaceOfWork
    function initQuickAddPlaceOfWork() {
        $('#quickAddPlaceOfWorkForm').on('submit', function (e) {
            e.preventDefault();

            var formData = {
                Location: $('#quickPlaceOfWork_Location').val()
            };

            $.ajax({
                url: '/PlaceOfWorks/QuickCreate',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function (response) {
                    if (response.success) {
                        var newOption = new Option(response.text, response.id, true, true);
                        $('#PlaceOfWorkID').append(newOption).trigger('change');
                        $('#quickAddPlaceOfWorkModal').modal('hide');
                        $('#quickAddPlaceOfWorkForm')[0].reset();
                        $('#quickAddPlaceOfWorkErrors').addClass('d-none');
                        showSuccessToast('Workplace "' + response.text + '" created successfully!');
                    } else {
                        $('#quickAddPlaceOfWorkErrors').removeClass('d-none').text(response.message);
                    }
                },
                error: function () {
                    $('#quickAddPlaceOfWorkErrors').removeClass('d-none').text('Error adding workplace. Please try again.');
                }
            });
        });

        $('#quickAddPlaceOfWorkModal').on('hidden.bs.modal', function () {
            $('#quickAddPlaceOfWorkForm')[0].reset();
            $('#quickAddPlaceOfWorkErrors').addClass('d-none').text('');
        });
    }

    // Public API
    window.ServiceProvidersQuickAdd = {
        init: function () {
            initQuickAddPlaceOfWork();
        }
    };

})(jQuery);
