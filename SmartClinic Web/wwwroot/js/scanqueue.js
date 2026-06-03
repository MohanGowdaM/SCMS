$(document).ready(function () {

    loadScanQueue();

});

function loadScanQueue() {

    $.ajax({

        url:
            '/Scan/GetScanQueue',

        type:
            'GET',

        success:
            function (response) {

                if (typeof response === 'string') {
                    response = JSON.parse(response);
                }

                var tbody = $('#tblScanQueue tbody');
                tbody.empty();

                $.each(response.data, function (index, item) {
                    tbody.append('<tr>' +
                        '<td>' + (item.tokenNumber || '') + '</td>' +
                        '<td>' + (item.patientName || '') + '</td>' +
                        '<td>' + (item.scanTypes || '') + '</td>' +
                        '<td>' + (item.notes ?? '') + '</td>' +
                        '<td><button class="btn btn-success btnCompleteScan" data-id="' + item.id + '" data-tokenid="' + item.tokenId + '">Complete Scan</button></td>' +
                        '</tr>');
                });

                // Initialize DataTable using global standardized helper
                smartClinicDataTable('#tblScanQueue', {
                    language: {
                        searchPlaceholder: 'Search scan queue...',
                        emptyTable: 'No scan requests'
                    }
                });
            }
    });
}

$(document).on(
    'click',
    '.btnCompleteScan',

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
                'Complete Scan?',

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
                                '/Scan/CompleteScan',

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
                                        typeof response
                                        === 'string'
                                    ) {

                                        response =
                                            JSON.parse(
                                                response
                                            );
                                    }

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

                                        loadScanQueue();
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
                                },

                            error:
                                function () {

                                    Swal.fire({

                                        icon:
                                            'error',

                                        title:
                                            'Error',

                                        text:
                                            'Failed to complete scan'
                                    });
                                }
                        });
                    }
                });
    });