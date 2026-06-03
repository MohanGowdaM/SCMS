$(document).ready(function () {

    loadLabQueue();

});

function loadLabQueue() {

    $.ajax({

        url: '/Lab/GetLabQueue',

        type: 'GET',

        dataType: 'json',        

        success: function (response) {     

            if (!response.success) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.message
                });
                return;
            }

            var tbody = $('#tblLabQueue tbody');
            tbody.empty();

            $.each(response.data, function (index, item) {
                tbody.append('<tr>' +
                    '<td>' + (item.tokenNumber || '') + '</td>' +
                    '<td>' + (item.patientName || '') + '</td>' +
                    '<td>' + (item.labTests || '') + '</td>' +
                    '<td><button class="btn btn-success btnCompleteLab" data-id="' + item.id + '" data-tokenid="' + item.tokenId + '">Complete Test</button></td>' +
                    '</tr>');
            });

            // Initialize DataTable using global standardized helper
            smartClinicDataTable('#tblLabQueue', {
                language: {
                    searchPlaceholder: 'Search lab queue...',
                    emptyTable: 'No lab requests'
                }
            });
        },

        error: function (xhr) {
            console.log(xhr);
            Swal.fire({
                icon: 'error',
                title: 'Ajax Error',
                text: xhr.responseText
            });
        }
    });
}

$(document).on(
    'click',
    '.btnCompleteLab',

    function () {

        let id =
            $(this)
                .data(
                    'id'
                );

        let tokenId =
            $(this)
                .data(
                    'tokenid'
                );

        Swal.fire({

            title:
                'Complete Lab Test?',

            text:
                'Patient will return to doctor.',

            icon:
                'question',

            showCancelButton:
                true,

            confirmButtonText:
                'Yes, Complete'
        })

            .then(

                function (result) {

                    if (
                        result.isConfirmed
                    ) {

                        $.ajax({

                            url:
                                '/Lab/CompleteLabTest',

                            type:
                                'POST',

                            contentType:
                                'application/json',

                            data:
                                JSON.stringify({

                                    id:
                                        id,

                                    tokenId:
                                        tokenId
                                }),

                            success:
                                function (
                                    response
                                ) {

                                    if (
                                        response.success
                                    ) {

                                        Swal.fire({

                                            icon:
                                                'success',

                                            title:
                                                'Completed',

                                            text:
                                                response.message
                                        });

                                        loadLabQueue();
                                    }
                                    else {

                                        Swal.fire({

                                            icon:
                                                'error',

                                            title:
                                                'Error',

                                            text:
                                                response.message
                                        });
                                    }
                                }
                        });
                    }
                });
    });