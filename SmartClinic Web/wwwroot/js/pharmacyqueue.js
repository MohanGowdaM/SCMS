$(document).ready(function () {

    loadPharmacyQueue();
});

function loadPharmacyQueue() {

    $.ajax({

        url:
            '/Pharmacy/GetPharmacyQueue',

        type:
            'GET',

        success:
            function (response) {

                var rows = '';

                response.data.forEach(function (item) {
                    rows += '<tr>' +
                        '<td>' + (item.tokenNumber || '') + '</td>' +
                        '<td>' + (item.patientName || '') + '</td>' +
                        '<td>' + (item.prescriptionNotes ?? '') + '</td>' +
                        '<td><button class="btn btn-primary btnDispense" data-id="' + item.id + '" data-tokenid="' + item.tokenId + '" data-notes="' + (item.prescriptionNotes || '') + '">Dispense</button></td>' +
                        '</tr>';
                });

                $('#tblPharmacyQueue tbody').html(rows);

                // Initialize DataTable using global standardized helper
                smartClinicDataTable('#tblPharmacyQueue', {
                    language: {
                        searchPlaceholder: 'Search pharmacy queue...',
                        emptyTable: 'No pharmacy requests'
                    }
                });
            }
    });
}


$(document).on('click', '.btnDispense',

    function () {

        let tokenId =
            $(this)
                .data(
                    'tokenid'
                );

        let notes =
            $(this)
                .data(
                    'notes'
                );

        $('#hdnTokenId')
            .val(
                tokenId
            );

        $('#txtPrescription')
            .val(
                notes
            );

        $('#dispenseMedicineModal')
            .modal(
                'show'
            );
    });

$('#btnDispenseMedicine')
    .click(function () {

        $.ajax({

            url:
                '/Pharmacy/DispenseMedicine',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId:
                        $('#hdnTokenId')
                            .val(),

                    medicineFee:
                        $('#txtMedicineFee')
                            .val()
                }),

            success:
                function (response) {

                    Swal.fire({

                        icon:
                            'success',

                        title:
                            'Success',

                        text:
                            response.message
                    });

                    $('#dispenseMedicineModal')
                        .modal(
                            'hide'
                        );

                    loadPharmacyQueue();
                }
        });
    });