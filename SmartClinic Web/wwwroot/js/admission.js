$(document).ready(function () {

   

    console.log(
        'admission.js loaded'
    );

    loadAdmitQueue();
});
function loadAdmitQueue() {
   
    $.ajax({
       
        url:
            '/Admission/GetAdmitQueue',

        type:
            'GET',

        success:
            function (response) {

                var rows = '';

                response.data.forEach(function (item) {
                    rows += '<tr>' +
                        '<td>' + (item.tokenNumber || '') + '</td>' +
                        '<td>' + (item.patientName || '') + '</td>' +
                        '<td>' + (item.wardType || '') + '</td>' +
                        '<td>' + (item.admissionReason ?? '') + '</td>' +
                        '<td><button class="btn btn-primary btnAssignBed" data-id="' + item.id + '" data-tokenid="' + item.tokenId + '">Assign Bed</button></td>' +
                        '</tr>';
                });

                $('#tblAdmitQueue tbody').html(rows);

                // Initialize DataTable using global standardized helper
                smartClinicDataTable('#tblAdmitQueue', {
                    language: {
                        searchPlaceholder: 'Search admission queue...',
                        emptyTable: 'No admission requests'
                    }
                });
            }
    });
}


$(document).on(
    'click',
    '.btnAssignBed',

    function () {

        $('#admissionId')
            .val(
                $(this).data('id')
            );

        $('#admissionTokenId')
            .val(
                $(this).data('tokenid')
            );

        $('#assignBedModal')
            .modal('show');
    });


$(document).on(
    'click',
    '#btnCompleteAdmission',

    function () {

        $.ajax({

            url:
                '/Admission/CompleteAdmission',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    id:
                        $('#admissionId')
                            .val(),

                    tokenId:
                        $('#admissionTokenId')
                            .val(),

                    bedNumber:
                        $('#txtBedNumber')
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

                    $('#assignBedModal')
                        .modal('hide');

                    loadAdmitQueue();
                }
        });
    });