$(document).ready(function () {

    loadBillingQueue();

});


/* ==========================================
   LOAD BILLING QUEUE
========================================== */
function loadBillingQueue() {

    $.ajax({

        url:
            '/Billing/GetBillingQueue',

        type:
            'GET',

        success:
            function (response) {

                if (typeof response === 'string') {
                    response = JSON.parse(response);
                }

                var tbody = $('#tblBillingQueue tbody');
                tbody.empty();

                $.each(response.data, function (index, item) {
                    tbody.append('<tr>' +
                        '<td>' + (item.tokenNumber || '') + '</td>' +
                        '<td>' + (item.patientName || '') + '</td>' +
                        '<td>₹ ' + (item.consultationFee ?? 0) + '</td>' +
                        '<td>₹ ' + (item.labFee ?? 0) + '</td>' +
                        '<td>₹ ' + (item.scanFee ?? 0) + '</td>' +
                        '<td>₹ ' + (item.medicineFee ?? 0) + '</td>' +
                        '<td>₹ ' + (item.totalAmount ?? 0) + '</td>' +
                        '<td>' +
                            '<button class="btn btn-success btn-sm btnCompletePayment" data-id="' + item.id + '" data-tokenid="' + item.tokenId + '" data-total="' + item.totalAmount + '">Make Payment</button> ' +
                            '<button class="btn btn-primary btn-sm btnSendToPharmacy mt-1" data-tokenid="' + item.tokenId + '">Send to Pharmacy</button> ' +
                            '<button class="btn btn-danger btn-sm btnCompleteVisit mt-1" data-tokenid="' + item.tokenId + '">Complete Visit</button>' +
                        '</td>' +
                        '</tr>');
                });

                // Initialize DataTable using global standardized helper
                smartClinicDataTable('#tblBillingQueue', {
                    language: {
                        searchPlaceholder: 'Search billing queue...',
                        emptyTable: 'No billing records'
                    }
                });
            }
    });
}

/* ==========================================
   OPEN PAYMMENT MODAL
========================================== */
$(document).on(
    'click',
    '.btnCompletePayment',

    function () {

        let billingId =
            $(this)
                .data(
                    'id'
                );

        let tokenId =
            $(this)
                .data(
                    'tokenid'
                );

        let total =
            $(this)
                .data(
                    'total'
                );

        $('#paymentBillingId')
            .val(
                billingId
            );

        $('#paymentTokenId')
            .val(
                tokenId
            );

        $('#txtPaymentAmount')
            .val(
                total
            );

        $('#paymentModal')
            .modal(
                'show'
            );
    });


/* ==========================================
   CONFIRM PAYMENT
========================================== */
$(document).on(
    'click',
    '#btnConfirmPayment',

    function () {

        $.ajax({

            url:
                '/Billing/CompletePayment',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    billingId:
                        $('#paymentBillingId')
                            .val(),

                    tokenId:
                        $('#paymentTokenId')
                            .val(),

                    paymentMethod:
                        $('#ddlPaymentMethod')
                            .val(),

                    remarks:
                        $('#txtPaymentRemarks')
                            .val()
                }),

            success:
                function (response) {

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
                                'Success',

                            text:
                                response.message
                        });

                        $('#paymentModal')
                            .modal(
                                'hide'
                            );

                        loadBillingQueue();
                    }
                }
        });
    });


/* ==========================================
   SEND TO PHARMACY
========================================== */
$(document).on(
    'click',
    '.btnSendToPharmacy',

    function () {

        let tokenId =
            $(this)
                .data(
                    'tokenid'
                );

        $.ajax({

            url:
                '/Billing/SendToPharmacy',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId:
                        tokenId
                }),

            success:
                function (response) {

                    if (
                        typeof response
                        === 'string'
                    ) {

                        response =
                            JSON.parse(
                                response
                            );
                    }

                    Swal.fire({

                        icon:
                            'success',

                        title:
                            'Success',

                        text:
                            response.message
                    });

                    loadBillingQueue();
                },

            error:
                function (xhr) {

                    Swal.fire({

                        icon:
                            'error',

                        title:
                            'Error',

                        text:
                            xhr.responseText
                    });
                }
        });
    });



/* ==========================================
   COMPLETE VISIT
========================================== */

$(document).on(
    'click',
    '.btnCompleteVisit',

    function () {
        debugger;

        let tokenId =
            $(this)
                .data(
                    'tokenid'
                );

        $.ajax({

            url:
                '/Billing/CompleteVisit',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId:
                        tokenId
                }),

            success:
                function (response) {

                    if (
                        typeof response
                        === 'string'
                    ) {

                        response =
                            JSON.parse(
                                response
                            );
                    }

                    Swal.fire({

                        icon:
                            'success',

                        title:
                            'Success',

                        text:
                            response.message
                    });

                    loadBillingQueue();
                },

            error:
                function (xhr) {

                    Swal.fire({

                        icon:
                            'error',

                        title:
                            'Error',

                        text:
                            xhr.responseText
                    });
                }
        });
    });