$(function () {

    // Load doctor queue
    loadDoctorQueue();

    // Auto refresh every 10 seconds
    setInterval(function () {

        loadDoctorQueue();

    }, 10000);

});

let doctorQueueData = null;

let previousReviewCount = 0;

let reviewSoundPlayedCount = 0;

let isPlayingReviewAlert = false;

/* ==========================================
   Load Doctor Queue
========================================== */
function loadDoctorQueue() {

    $.ajax({

        url: '/Doctors/GetDoctorQueue',

        type: 'GET',

        beforeSend: function () {

            showLoading();
        },

        success:
            function (response) {

                if (
                    !response.success
                ) {

                    Swal.fire({

                        icon: 'error',

                        title: 'Error',

                        text: 'Unable to load doctor queue'
                    });

                    return;
                }

                let data = response.data;

                doctorQueueData = data;

                // ===========================
                // Header
                // ===========================
                $('#doctorName').text(`👨‍⚕️ ${data.doctorName}`);

                $('#terminalName').text(`Terminal: ${data.terminalName}`);

                // ===========================
                // Statistics
                // ===========================       

                $('#calledCount').text(data.calledCount ?? 0);

                $('#labCount').text(data.sentToLabCount ?? 0);

                $('#scanCount').text(data.sentToScanCount ?? 0);

                $('#billingCount').text(data.sentToBillingCount ?? 0);

                $('#paidCount').text(data.paidCount ?? 0);

                $('#pharmacyCount').text(data.sentToPharmacyCount ?? 0);

                $('#MedicineDispensedCount').text(data.medicineDispensed ?? 0);

                $('#SendToAdmitCount').text(data.sendToAdmissionCount ?? 0);

                $('#AdmittedCount').text(data.admittedCount ?? 0);

                $('#totalCount').text(data.totalCount);

                $('#servedCount').text(data.servedCount);
                
                $('#CompletedCount').text(data.completedCount);

                $('#waitingCount').text(data.waitingCount);

                $('#missedCount').text(data.missedCount);

                $('#holdCount').text(data.holdQueue.length);

                let reviewCount = data.waitingForReviewQueue?.length ?? 0;

                $('#reviewCount').text(reviewCount);

                if (reviewCount > 0) {

                    $('#btnWaitingReview').addClass('review-alert');

                    // Play only if count increased
                    if (reviewCount > previousReviewCount) {

                        playReviewAlertSound();
                    }
                }
                else {

                    $('#btnWaitingReview').removeClass('review-alert');

                    reviewSoundPlayedCount = 0;

                    isPlayingReviewAlert = false;
                }

                previousReviewCount = reviewCount;


                // ===========================
                // Current Ticket
                // ===========================
                bindCurrentTicket(data.currentTicket);

                // ===========================
                // Queue Lists
                // ===========================
                bindNextQueue(data.nextQueue);

                bindWaitingQueue(data.waitingQueue);

                bindSkippedQueue(data.skippedQueue);

                bindHoldQueue(data.holdQueue);
            },

        error:
            function () {

                Swal.fire({

                    icon:
                        'error',

                    title:
                        'Error',

                    text:
                        'Failed to load doctor queue'
                });
            }
    });
}


/* ==========================================
   Current Ticket
========================================== */
function bindCurrentTicket(ticket) {

    if (!ticket) {
        $('#currentTicketId')
            .text('—')
            .removeAttr('data-id');

        $('#currentTicketName')
            .text('No Active Call');

        return;
    }

    $('#currentTicketId').text(ticket.tokenNumber).attr('data-id', ticket.id);

    $('#currentTicketName')
        .html(`
            ${ticket.patientName}
            <br/>
            <small>
                ${ticket.priorityName}
            </small>
        `);
}


/* ==========================================
   Next Queue
========================================== */
function bindNextQueue(queue) {

    let container = $('#nextQueueList');

    container.empty();

    if (
        !queue ||
        queue.length === 0
    ) {

        container.html(`

            <div class="empty-state">

                No upcoming patients

            </div>
        `);

        return;
    }

    $.each(
        queue,

        function (
            index,
            item
        ) {

            container.append(`

                <div class="queue-item">

                    <div>

                        <strong>
                            ${item.patientName}
                        </strong>

                        <br/>

                        <small>
                            ${item.priorityName}
                        </small>

                    </div>

                    <span class="token-badge">

                        ${item.tokenNumber}

                    </span>

                </div>
            `);
        });
}


/* ==========================================
   Waiting Queue
========================================== */
function bindWaitingQueue(
    queue
) {

    let container =
        $('#waitingQueueList');

    container.empty();

    if (
        !queue ||
        queue.length === 0
    ) {

        container.html(`

            <div class="empty-state">

                No waiting patients

            </div>
        `);

        return;
    }

    $.each(
        queue,

        function (
            index,
            item
        ) {

            container.append(`

                <div class="queue-item">

                    <div>

                        <strong>
                            ${item.patientName}
                        </strong>

                        <br/>

                        <small>
                            ${item.priorityName}
                        </small>

                    </div>

                    <span class="token-badge">

                        ${item.tokenNumber}

                    </span>

                </div>
            `);
        });
}


/* ==========================================
   Skipped Queue
========================================== */
function bindSkippedQueue(
    queue
) {

    let container =
        $('#skippedQueueList');

    container.empty();

    if (
        !queue ||
        queue.length === 0
    ) {

        container.html(`

            <div class="empty-state">

                No skipped patients

            </div>
        `);

        return;
    }

    $.each(
        queue,

        function (
            index,
            item
        ) {

            container.append(`

                <div class="queue-item">

                    <div>

                        <strong>
                            ${item.patientName}
                        </strong>

                    </div>

                    <span class="token-badge">

                        ${item.tokenNumber}

                    </span>

                </div>
            `);
        });
}


/* ==========================================
   Hold Queue
========================================== */
function bindHoldQueue(
    queue
) {

    let container =
        $('#holdQueueList');

    container.empty();

    if (
        !queue ||
        queue.length === 0
    ) {

        container.html(`

            <div class="empty-state">

                No hold patients

            </div>
        `);

        return;
    }

    $.each(
        queue,

        function (
            index,
            item
        ) {

            container.append(`

                <div class="queue-item">

                    <div>

                        <strong>
                            ${item.patientName}
                        </strong>

                    </div>

                    <span class="token-badge">

                        ${item.tokenNumber}

                    </span>

                </div>
            `);
        });
}


/* ==========================================
   Action Buttons
========================================== */

/* ==========================================
   CALL
========================================== */
$('#callBtn').click(function () {

    updateQueueStatus('CALL', null);
});


/* ==========================================
   END
========================================== */
$('#endBtn').click(function () {

    let tokenId =
        doctorQueueData.currentTicket?.id;

    if (!tokenId) {
        Swal.fire({

            icon:
                'warning',

            title:
                'No Active Consultation',

            text:
                'No patient is currently in consultation'
        });

        return;
    }

    updateQueueStatus(
        'END',
        tokenId
    );
});


/* ==========================================
   RECALL
========================================== */
$('#recallBtn').click(function () {

    let tokenId =
        doctorQueueData.currentTicket?.id;

    if (!tokenId) {
        Swal.fire({

            icon:
                'warning',

            title:
                'No Active Call',

            text:
                'No patient available to recall'
        });

        return;
    }

    updateQueueStatus(
        'RECALL',
        tokenId
    );
});


/* ==========================================
   HOLD
========================================== */
$('#holdBtn').click(function () {

    let tokenId =
        doctorQueueData.currentTicket?.id;

    if (!tokenId) {
        Swal.fire({

            icon:
                'warning',

            title:
                'No Active Consultation',

            text:
                'No patient available to hold'
        });

        return;
    }

    updateQueueStatus(
        'HOLD',
        tokenId
    );
});


/* ==========================================
   SKIP
========================================== */
$('#skipBtn').click(function () {

    let tokenId = doctorQueueData.currentTicket?.id;

    if (!tokenId) {
        Swal.fire({

            icon:
                'warning',

            title:
                'No Active Consultation',

            text:
                'No patient available to skip'
        });

        return;
    }

    updateQueueStatus(
        'SKIP',
        tokenId
    );
});


/* ==========================================
   NEXT
========================================== */
$('#nextBtn').click(function () {

    let tokenId = doctorQueueData.currentTicket?.id;
    let WaitingTokenId = doctorQueueData.waitingCount?.id;

    if (!tokenId) {
        Swal.fire({

            icon: 'warning',

            title: 'No Active Consultation',

            text: 'No patient available to move next'
        });

        return;
    }

    if (!WaitingTokenId) {
        Swal.fire({

            icon: 'warning',

            title: 'No Waiting Patient',

            text: 'No patient waiting to move next'
        });

        return;
    }

    updateQueueStatus(
        'NEXT',
        tokenId
    );
});


/* ==========================================
   Update Queue Status
========================================== */
function updateQueueStatus(action, tokenId) {

    $.ajax({

        url: '/Doctors/UpdateQueueStatus',

        type: 'POST',

        contentType: 'application/json',

        data:
            JSON.stringify({

                action: action,

                tokenId: tokenId
            }),

        success:
            function (response) {
                if (response.success) {
                    Swal.fire({

                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    });

                    loadDoctorQueue();
                }
                else {
                    Swal.fire({

                        icon: 'warning',
                        title: 'Warning',
                        text: response.message
                    });
                }
            }
    });
}


/* ==========================================
   Loading
========================================== */
function showLoading() {

    $('#nextQueueList')
        .html(
            '<div class="empty-state">Loading...</div>'
        );

    $('#waitingQueueList')
        .html(
            '<div class="empty-state">Loading...</div>'
        );

    $('#skippedQueueList')
        .html(
            '<div class="empty-state">Loading...</div>'
        );

    $('#holdQueueList')
        .html(
            '<div class="empty-state">Loading...</div>'
        );
}

/* ==========================================
   Open Consultation Modal
========================================== */
$(document).on('click', '#currentlyCalledCard',
    function () {
        let ticket = doctorQueueData?.currentTicket;

        if (!ticket) {

            Swal.fire({
                icon: 'warning',

                title: 'No Active Patient',

                text:
                    'No patient is under consultation.'
            });

            return;
        }

        $.get(
            '/Doctors/ConsultationModal',

            function (response) {

                $('#consultationModalContainer')
                    .html(response);

                $('#modalTokenNumber')
                    .text(
                        ticket.tokenNumber
                    );

                $('#modalTokenNo')
                    .text(
                        ticket.tokenNumber
                    );

                $('#modalPatientName')
                    .text(
                        ticket.patientName
                    );

                $('#modalPriority')
                    .text(
                        ticket.priorityName
                    );

                $('#consultationModal')
                    .modal('show');
            }
        );
    }
);

$(document).on('click', '.action-tile',

    function () {

        $('.action-tile').removeClass('active');

        $(this).addClass('active');

        let tab = $(this).data('tab');

        // Hide all tabs
        $('.consult-tab-content').addClass('d-none');

        // Show selected tab
        $('#' + tab).removeClass('d-none');
    }
);

/* ==========================================
   SAVE PRESCRIPTION
========================================== */

$(document).on('click','#btnSavePrescription',

    function () {

        let tokenId =
            doctorQueueData
                .currentTicket
                ?.id;

        if (!tokenId) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'No Patient',

                text:
                    'No active patient found.'
            });

            return;
        }

        if (
            medicines.length === 0
        ) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'Warning',

                text:
                    'Please add at least one medicine.'
            });

            return;
        }

        let diagnosis =
            $('#txtDiagnosis')
                .val();

        let prescriptionNotes =
            $('#txtPrescriptionNotes')
                .val()
            || '';

        $.ajax({

            url:
                '/Doctors/SaveConsultation',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId:
                        tokenId,

                    diagnosis:
                        diagnosis,

                    prescriptionNotes:
                        prescriptionNotes,

                    medicines:
                        medicines
                }),

            success:
                function (response) {

                    if (
                        response.success
                    ) {

                        Swal.fire({

                            icon:
                                'success',

                            title:
                                'Saved',

                            text:
                                response.message,

                            timer:
                                1500,

                            showConfirmButton:
                                false
                        });

                        clearPrescriptionForm();
                    }
                    else {

                        Swal.fire({

                            icon:
                                'warning',

                            title:
                                'Warning',

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
                            'Failed to save consultation.'
                    });
                }
        });
    });

/* ==========================================
   CLEAR PRESCRIPTION FORM
========================================== */

function clearPrescriptionForm() {

    $('#txtDiagnosis')
        .val('');

    $('#txtPrescriptionNotes')
        .val('');

    medicines = [];

    bindMedicines();

    $('#medicineTableContainer')
        .hide();

    clearMedicineFields();
}

/* ==========================================
   PRINT PRESCRIPTION
========================================== */

$(document).on(
    'click',
    '#btnPrintPrescription',

    function () {

        if (
            medicines.length === 0
        ) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'No Medicines',

                text:
                    'Please add medicines first.'
            });

            return;
        }

        printPrescription();
    });

function printPrescription() {

    let prescriptionHtml = '';

    medicines.forEach(function (m, index) {

        let schedule = [];

        if (m.timing.includes('Morning')) {

            schedule.push(
                `Morning (${m.food === 'Before Food'
                    ? 'BF'
                    : 'AF'})`
            );
        }

        if (m.timing.includes('Lunch')) {

            schedule.push(
                `Afternoon (${m.food === 'Before Food'
                    ? 'BF'
                    : 'AF'})`
            );
        }

        if (m.timing.includes('Dinner')) {

            schedule.push(
                `Night (${m.food === 'Before Food'
                    ? 'BF'
                    : 'AF'})`
            );
        }

        prescriptionHtml += `

<div class="medicine-card">

    <div class="medicine-header">

        ${index + 1}. ${m.medicineName}

    </div>

    <div class="timing-line">

        <span class="timing-item">

            <b>Morning</b>

            <input type="checkbox"
                   ${m.timing.includes('Morning')
                && m.food === 'Before Food'
                ? 'checked'
                : ''}>

            BF

            <input type="checkbox"
                   ${m.timing.includes('Morning')
                && m.food === 'After Food'
                ? 'checked'
                : ''}>

            AF
        </span>

        <span class="timing-item">

            <b>Afternoon</b>

            <input type="checkbox"
                   ${m.timing.includes('Lunch')
                && m.food === 'Before Food'
                ? 'checked'
                : ''}>

            BF

            <input type="checkbox"
                   ${m.timing.includes('Lunch')
                && m.food === 'After Food'
                ? 'checked'
                : ''}>

            AF
        </span>

        <span class="timing-item">

            <b>Night</b>

            <input type="checkbox"
                   ${m.timing.includes('Dinner')
                && m.food === 'Before Food'
                ? 'checked'
                : ''}>

            BF

            <input type="checkbox"
                   ${m.timing.includes('Dinner')
                && m.food === 'After Food'
                ? 'checked'
                : ''}>

            AF
        </span>

    </div>

    <div class="medicine-footer">

        <span>

            <b>Dose:</b>

            ${m.dosage}

        </span>

        <span>

            <b>Qty:</b>

            ${m.quantity}

        </span>

        <span>

            <b>Days:</b>

            ${m.days}

        </span>

    </div>

</div>
`;
    });

    let printWindow =
        window.open(
            '',
            '',
            'width=900,height=700'
        );

    printWindow.document.write(`

<html>

<head>

<title>
Prescription
</title>

<link rel="stylesheet"
      type="text/css"
      href="${window.location.origin}/css/doctor-terminal-print.css?v=${new Date().getTime()}">

</head>

<body>

<div class="clinic-header">

<h1>

SMART CLINIC

</h1>

<p>

Doctor Prescription

</p>

</div>

<div class="patient-row">

    <span>

        Patient:
        ${doctorQueueData.currentTicket.patientName}

    </span>

    <span>

        Token:
        ${doctorQueueData.currentTicket.tokenNumber}

    </span>

    <span>

        Date:
        ${new Date().toLocaleDateString()}

    </span>

</div>

<div class="diagnosis-row">

    <b>Diagnosis:</b>

    ${$('#txtDiagnosis').val() || '-'}

    ${$('#txtPrescriptionNotes').val()
            ? `<br><b>Instructions:</b> ${$('#txtPrescriptionNotes').val()}`
            : ''}

</div>

<div class="medicine-container">

${prescriptionHtml}

</div>

<div class="signature">

    <div class="signature-line">
    </div>

    Doctor Signature

</div>

</body>

</html>
`);

    printWindow.document.close();

    printWindow.onload = function () {

        setTimeout(function () {

            printWindow.print();

        }, 500);
    };

    printWindow.onafterprint = function () {

        loadDoctorQueue();

        printWindow.close();
    };
}


/* ==========================================
   VOICE PRESCRIPTION
========================================== */

let recognition = null;
let isRecording = false;

$(document).on(
    'click',
    '#btnVoicePrescription',

    function () {

        if (
            !(
                'webkitSpeechRecognition'
                in window
            )
        ) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'Not Supported',

                text:
                    'Speech recognition is not supported in this browser.'
            });

            return;
        }

        let button =
            $(this);

        // Initialize once
        if (!recognition) {

            recognition =
                new webkitSpeechRecognition();

            recognition.continuous =
                true;

            recognition.interimResults =
                false; // VERY IMPORTANT

            recognition.lang =
                'en-IN';

            recognition.onresult =
                function (event) {

                    let finalTranscript =
                        '';

                    for (
                        let i =
                            event.resultIndex;

                        i <
                        event.results.length;

                        i++
                    ) {

                        // Only final results
                        if (
                            event.results[i]
                                .isFinal
                        ) {

                            finalTranscript +=
                                event.results[i][0]
                                    .transcript;
                        }
                    }

                    if (
                        finalTranscript
                            .trim()
                    ) {

                        let currentText =
                            $('#txtPrescription')
                                .val();

                        $('#txtPrescription')
                            .val(

                                currentText
                                    ? currentText
                                    + '\n'
                                    + finalTranscript
                                    : finalTranscript
                            );
                    }
                };

            recognition.onerror =
                function () {

                    button.removeClass(
                        'recording'
                    );

                    isRecording =
                        false;
                };

            recognition.onend =
                function () {

                    button.removeClass(
                        'recording'
                    );

                    isRecording =
                        false;
                };
        }

        // Toggle Recording
        if (isRecording) {

            recognition.stop();

            button.removeClass(
                'recording'
            );

            isRecording =
                false;
        }
        else {

            recognition.start();

            button.addClass(
                'recording'
            );

            isRecording =
                true;
        }
    });

/* ==========================================
   CLEAR PRESCRIPTION
========================================== */

$(document).on(
    'click',
    '#btnClearPrescription',

    function () {

        Swal.fire({

            title:
                'Clear Prescription?',

            text:
                'Prescription notes will be removed.',

            icon:
                'warning',

            showCancelButton:
                true,

            confirmButtonText:
                'Yes, Clear',

            cancelButtonText:
                'Cancel'
        })

            .then(

                function (result) {

                    if (
                        result.isConfirmed
                    ) {

                        $('#txtPrescription')
                            .val('');

                        Swal.fire({

                            toast:
                                true,

                            position:
                                'top-end',

                            icon:
                                'success',

                            title:
                                'Prescription cleared',

                            timer:
                                1500,

                            showConfirmButton:
                                false
                        });
                    }
                });
    });

/* ==========================================
   SEND TO LAB
========================================== */

$(document).on('click', '#btnSendToLab',

    function () {

        let tokenId = doctorQueueData.currentTicket?.id;

        if (!tokenId) {

            Swal.fire({

                icon: 'warning',

                title: 'No Active Patient',

                text: 'No patient under consultation.'
            });

            return;
        }

        let selectedTests = [];

        $('.lab-checkbox:checked')
            .each(
                function () {

                    selectedTests.push($(this).data('name'));
                });

        // Validation
        if (selectedTests.length === 0) {

            Swal.fire({

                icon: 'warning',

                title: 'Select Test',

                text: 'Please select at least one lab test.'
            });

            return;
        }

        $.ajax({

            url: '/Doctors/SendToLab',

            type: 'POST',

            contentType: 'application/json',

            data:
                JSON.stringify({

                    tokenId: tokenId,

                    labTests: selectedTests,

                    notes: $('#txtLabNotes').val()
                }),

            success:
                function (response) {

                    if (response.success) {

                        Swal.fire({

                            icon: 'success',

                            title: 'Sent',

                            text: response.message,

                            timer: 1500,

                            showConfirmButton: false
                        });

                        // Clear selection
                        $('.lab-checkbox').prop('checked', false);

                        $('.lab-test-item').removeClass('selected');

                        $('.btn-clear-card').hide();

                        $('#txtLabNotes').val('');

                        $('#consultationModal').modal('hide');

                        loadDoctorQueue();
                    }
                },

            error:
                function () {

                    Swal.fire({

                        icon: 'error',

                        title: 'Error',

                        text: 'Failed to send to lab.'
                    });
                }
        });
    });



/* ==========================================
   LAB NOTES SPEECH RECOGNITION
========================================== */

let labRecognition = null;
let isLabRecording = false;

$(document).on(
    'click',
    '#btnVoiceLabNotes',

    function () {

        if (
            !(
                'webkitSpeechRecognition'
                in window
            )
        ) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'Not Supported',

                text:
                    'Speech recognition is not supported in this browser.'
            });

            return;
        }

        let button =
            $(this);

        // Initialize once
        if (!labRecognition) {

            labRecognition =
                new webkitSpeechRecognition();

            labRecognition.continuous =
                true;

            labRecognition.interimResults =
                false;

            labRecognition.lang =
                'en-IN';

            labRecognition.onresult =
                function (event) {

                    let finalTranscript =
                        '';

                    for (
                        let i =
                            event.resultIndex;

                        i <
                        event.results.length;

                        i++
                    ) {

                        if (
                            event.results[i]
                                .isFinal
                        ) {

                            finalTranscript +=
                                event.results[i][0]
                                    .transcript;
                        }
                    }

                    if (
                        finalTranscript
                            .trim()
                    ) {

                        let currentText =
                            $('#txtLabNotes')
                                .val();

                        $('#txtLabNotes')
                            .val(

                                currentText
                                    ? currentText
                                    + '\n'
                                    + finalTranscript
                                    : finalTranscript
                            );
                    }
                };

            labRecognition.onerror =
                function () {

                    button.removeClass(
                        'recording'
                    );

                    isLabRecording =
                        false;
                };

            labRecognition.onend =
                function () {

                    button.removeClass(
                        'recording'
                    );

                    isLabRecording =
                        false;
                };
        }

        // Toggle Recording
        if (
            isLabRecording
        ) {

            labRecognition.stop();

            button.removeClass(
                'recording'
            );

            isLabRecording =
                false;
        }
        else {

            labRecognition.start();

            button.addClass(
                'recording'
            );

            isLabRecording =
                true;
        }
    });


/* ==========================================
   CLEAR LAB NOTES
========================================== */

$(document).on(
    'click',
    '#btnClearLabNotes',

    function () {

        Swal.fire({

            title:
                'Clear Lab Notes?',

            text:
                'Lab notes will be removed.',

            icon:
                'warning',

            showCancelButton:
                true,

            confirmButtonText:
                'Yes, Clear',

            cancelButtonText:
                'Cancel'
        })

            .then(

                function (result) {

                    if (
                        result.isConfirmed
                    ) {

                        $('#txtLabNotes')
                            .val('');

                        Swal.fire({

                            toast:
                                true,

                            position:
                                'top-end',

                            icon:
                                'success',

                            title:
                                'Lab notes cleared',

                            timer:
                                1500,

                            showConfirmButton:
                                false
                        });
                    }
                });
    });

/* ==========================================
   SEND TO SCAN
========================================== */

$(document).on('click','#btnSendToScan',

    function () {

        let tokenId =
            doctorQueueData
                .currentTicket
                ?.id;

        if (!tokenId) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'No Patient',

                text:
                    'No patient under consultation.'
            });

            return;
        }

        let selectedScans = [];

        $('.scan-checkbox:checked')
            .each(

                function () {

                    selectedScans.push(

                        $(this)
                            .data(
                                'name'
                            )
                    );
                });

        // Validation
        if (
            selectedScans.length === 0
        ) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'Select Scan',

                text:
                    'Please select at least one scan.'
            });

            return;
        }

        $.ajax({

            url:
                '/Doctors/SendToScan',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId:
                        tokenId,

                    scanTypes:
                        selectedScans,

                    notes:
                        $('#txtScanNotes')
                            .val()
                }),

            success:
                function (response) {

                    if (
                        response.success
                    ) {

                        Swal.fire({

                            icon:
                                'success',

                            title:
                                'Sent',

                            text:
                                response.message,

                            timer:
                                1500,

                            showConfirmButton:
                                false
                        });

                        // Clear selected scans
                        $('.scan-checkbox')
                            .prop(
                                'checked',
                                false
                            );

                        // Remove selected UI
                        $('.lab-test-item')
                            .removeClass(
                                'selected'
                            );

                        // Hide clear buttons
                        $('.btn-clear-card')
                            .hide();

                        // Clear notes
                        $('#txtScanNotes')
                            .val('');

                        // Hide modal
                        $('#consultationModal')
                            .modal(
                                'hide'
                            );

                        loadDoctorQueue();
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
                            'Failed to send scan request.'
                    });
                }
        });
    });



/* ==========================================
   SCAN NOTES SPEECH RECOGNITION
========================================== */

let scanRecognition = null;
let isScanRecording = false;

$(document).on(
    'click',
    '#btnVoiceScanNotes',

    function () {

        if (
            !(
                'webkitSpeechRecognition'
                in window
            )
        ) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'Not Supported',

                text:
                    'Speech recognition is not supported in this browser.'
            });

            return;
        }

        let button =
            $(this);

        // Initialize once
        if (!scanRecognition) {

            scanRecognition =
                new webkitSpeechRecognition();

            scanRecognition.continuous =
                true;

            scanRecognition.interimResults =
                false;

            scanRecognition.lang =
                'en-IN';

            scanRecognition.onresult =
                function (event) {

                    let finalTranscript =
                        '';

                    for (
                        let i =
                            event.resultIndex;

                        i <
                        event.results.length;

                        i++
                    ) {

                        if (
                            event.results[i]
                                .isFinal
                        ) {

                            finalTranscript +=
                                event.results[i][0]
                                    .transcript;
                        }
                    }

                    if (
                        finalTranscript
                            .trim()
                    ) {

                        let currentText =
                            $('#txtScanNotes')
                                .val();

                        $('#txtScanNotes')
                            .val(

                                currentText
                                    ? currentText
                                    + '\n'
                                    + finalTranscript
                                    : finalTranscript
                            );
                    }
                };

            scanRecognition.onerror =
                function () {

                    button.removeClass(
                        'recording'
                    );

                    isScanRecording =
                        false;
                };

            scanRecognition.onend =
                function () {

                    button.removeClass(
                        'recording'
                    );

                    isScanRecording =
                        false;
                };
        }

        // Toggle Recording
        if (
            isScanRecording
        ) {

            scanRecognition.stop();

            button.removeClass(
                'recording'
            );

            isScanRecording =
                false;
        }
        else {

            scanRecognition.start();

            button.addClass(
                'recording'
            );

            isScanRecording =
                true;
        }
    });


/* ==========================================
   CLEAR SCAN NOTES
========================================== */

$(document).on(
    'click',
    '#btnClearScanNotes',

    function () {

        Swal.fire({

            title:
                'Clear Scan Notes?',

            text:
                'Scan notes will be removed.',

            icon:
                'warning',

            showCancelButton:
                true,

            confirmButtonText:
                'Yes, Clear',

            cancelButtonText:
                'Cancel'
        })

            .then(

                function (result) {

                    if (
                        result.isConfirmed
                    ) {

                        $('#txtScanNotes')
                            .val('');

                        Swal.fire({

                            toast:
                                true,

                            position:
                                'top-end',

                            icon:
                                'success',

                            title:
                                'Scan notes cleared',

                            timer:
                                1500,

                            showConfirmButton:
                                false
                        });
                    }
                });
    });



/* ==========================================
   CALCULATE BILL
========================================== */
$(document).on(
    'keyup change',

    '#txtConsultationFee, #txtLabFee, #txtScanFee, #txtMedicineFee',

    function () {

        calculateBill();

    });

function calculateBill() {

    let consultation =
        parseFloat(
            $('#txtConsultationFee')
                .val()
        ) || 0;

    let lab =
        parseFloat(
            $('#txtLabFee')
                .val()
        ) || 0;

    let scan =
        parseFloat(
            $('#txtScanFee')
                .val()
        ) || 0;

    let medicine =
        parseFloat(
            $('#txtMedicineFee')
                .val()
        ) || 0;

    let total =
        consultation
        + lab
        + scan
        + medicine;

    $('#lblTotalAmount')
        .text(total.toFixed(2));
}



/* ==========================================
   SEND TO BILLING
========================================== */
$(document).on('click', '#btnSendToBilling',

    function () {

        let tokenId = doctorQueueData.currentTicket?.id;

        if (!tokenId) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'No Active Patient',

                text:
                    'No patient under consultation.'
            });

            return;
        }

        $.ajax({

            url:
                '/Doctors/SendToBilling',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId:
                        tokenId,

                    consultationFee:
                        parseFloat(
                            $('#txtConsultationFee')
                                .val()
                        ) || 0,

                    labFee:
                        parseFloat(
                            $('#txtLabFee')
                                .val()
                        ) || 0,

                    scanFee:
                        parseFloat(
                            $('#txtScanFee')
                                .val()
                        ) || 0,

                    medicineFee:
                        parseFloat(
                            $('#txtMedicineFee')
                                .val()
                        ) || 0,

                    notes:
                        $('#txtBillingNotes')
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

                        $('#consultationModal')
                            .modal('hide');

                        loadDoctorQueue();
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
                            'Failed to send patient to billing'
                    });
                }
        });
    });


/* ==========================================
   OPEN WAITING FOR REVIEW MODAL
========================================== */
$(document).on('click', '#btnWaitingReview',

    function () {

        let rows = '';

        doctorQueueData.waitingForReviewQueue.forEach(function (item) {

            rows += `
                <tr>
                <td>${item.tokenNumber}</td>
                <td>${item.patientName}</td>
                <td><button class="btn btn-primary btnReviewCall" data-id="${item.id}">Call</button></td>
                </tr>`;
        });

        $('#tblWaitingReviewBody').html(rows);

        $('#waitingReviewModal').modal('show');
    });


/* ==========================================
   CALL FROM REVIEW MODULE
========================================== */
$(document).on('click', '.btnReviewCall',

    function () {

        let tokenId = $(this).data('id');

        updateQueueStatus('WAITINGCALL', tokenId);

        $('#waitingReviewModal').modal('hide');
    });


/* ==========================================
Function to Play Alert Sound 3 Times
========================================== */
function playReviewAlertSound() {

    if (isPlayingReviewAlert)
        return;

    isPlayingReviewAlert = true;

    reviewSoundPlayedCount = 0;

    function playAgain() {

        if (reviewSoundPlayedCount >= 1) {

            isPlayingReviewAlert = false;

            return;
        }

        $('#reviewAlertSound')[0]
            .currentTime = 0;

        $('#reviewAlertSound')[0]
            .play();

        reviewSoundPlayedCount++;

        setTimeout(
            playAgain,
            3000
        );
    }

    playAgain();
}


/* ==========================================
   SEND FOR ADMISSION
========================================== */
$(document).on('click', '#btnSendToAdmit',

    function () {

        let tokenId = doctorQueueData.currentTicket?.id;

        if (!tokenId) {

            Swal.fire({

                icon:
                    'warning',

                title:
                    'No Active Patient',

                text:
                    'No patient under consultation.'
            });

            return;
        }

        $.ajax({

            url:
                '/Doctors/SendToAdmit',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    tokenId: tokenId,


                    doctorId: $('#modalDoctorId').val(),

                    admissionReason: $('#txtAdmissionReason').val(),

                    wardType: $('#ddlWardType').val()
                }),

            success:
                function (response) {

                    Swal.fire({

                        icon: 'success',

                        title: 'Success',

                        text: response.message
                    });

                    $('#consultationModal')
                        .modal('hide');

                    loadDoctorQueue();
                },

            error:
                function (xhr) {

                    console.log(xhr);

                    Swal.fire({

                        icon: 'error',

                        title: 'Error',

                        text: 'Failed to send patient to admit'
                    });
                }
        });
    });



/* ==========================================
    LOAD MEDICINES FOR PRESCRIPTION PAGE
========================================== */
$(document).on('shown.bs.modal', '#consultationModal',

    function () {

        setTimeout(function () {

            loadMedicines();
            loadLabTests();
            loadScanTests();

        }, 300);
    });

function loadMedicines() {

    if ($('#ddlMedicine').length === 0) {
        return;
    }

    $.ajax({

        url: '/Doctors/GetMedicines',

        type: 'GET',

        success:
            function (response) {

                let options = '<option value="">Select Medicine</option>';

                response.data
                    .forEach(

                        function (item) {

                            options += `<option value="${item.id}">${item.medicineName}</option>`;
                        });

                $('#ddlMedicine').empty().append(options);
            },

        error:
            function (xhr) {

                console.log(xhr);
            }
    });
}


/* ==========================================
   ADD MEDICINE
========================================== */

var medicines = [];

$(document).on('click', '#btnAddMedicine',

    function () {

        let timing = [];

        if ($('#chkMorning').is(':checked')) timing.push('Morning');

        if ($('#chkLunch').is(':checked')) timing.push('Lunch');

        if ($('#chkDinner').is(':checked')) timing.push('Dinner');

        if (!$('#ddlMedicine').val()) {

            Swal.fire({

                icon: 'warning',
                title: 'Warning',
                text: 'Please select medicine.'
            });

            return;
        }

        let medicine = {

            medicineId: $('#ddlMedicine').val(),

            medicineName: $('#ddlMedicine option:selected').text(),

            quantity: $('#txtQuantity').val(),

            dosage: $('#txtDosage').val(),

            days: $('#txtDays').val(),

            food: $('#ddlFood').val(),

            timing: timing.join(', ')
        };

        // EDIT MODE
        if (editMedicineIndex > -1) {

            medicines[editMedicineIndex] = medicine;

            editMedicineIndex = -1;

            $('#btnAddMedicine').html('<i class="fa-solid fa-plus"></i> Add Medicine');
        }
        else {
            medicines.push(medicine);
        }

        bindMedicines();

        clearMedicineFields();
    });


/* ==========================================
   BIND MEDICINES
========================================== */
function bindMedicines() {

    let rows = '';

    medicines.forEach(function (m, index) {

        rows += `<tr>
        <td>${m.medicineName || ''}</td>
        <td>${m.quantity || ''}</td>
        <td>${m.dosage || ''}</td>
        <td>${m.timing || ''}</td>
        <td>${m.food || ''}</td>
        <td>${m.days || ''}</td>
        <td><button type="button" class="btn btn-warning btn-sm btnEditMedicine" data-index="${index}"><i class="fa-solid fa-pen"></i></button>
        <button type="button" class="btn btn-danger btn-sm btnRemoveMedicine" data-index="${index}"><i class="fa-solid fa-trash"></i></button></td>
        </tr>`;
    });

    $('#tblPrescriptionBody').html(rows);

    if (medicines.length > 0) {

        $('#medicineTableContainer').show();

        $('#printContainer').show();
    }
    else {

        $('#medicineTableContainer').hide();

        $('#printContainer').hide();
    }
}


/* ==========================================
   EDIT MEDICINE
========================================== */

let editMedicineIndex = -1;

$(document).on('click', '.btnEditMedicine',

    function (e) {

        e.stopPropagation();

        let index = $(this).data('index');

        editMedicineIndex = index;

        let medicine = medicines[index];

        // Fill form
        $('#ddlMedicine').val(medicine.medicineId);

        $('#txtQuantity').val(medicine.quantity);

        $('#txtDosage').val(medicine.dosage);

        $('#txtDays').val(medicine.days);

        $('#ddlFood').val(medicine.food);

        // Clear checkboxes
        $('#chkMorning').prop('checked', false);

        $('#chkLunch').prop('checked', false);

        $('#chkDinner').prop('checked', false);

        // Restore timing
        if (medicine.timing?.includes('Morning')) {
            $('#chkMorning').prop('checked', true);
        }

        if (medicine.timing?.includes('Lunch')) {
            $('#chkLunch').prop('checked', true);
        }

        if (medicine.timing?.includes('Dinner')) {
            $('#chkDinner').prop('checked', true);
        }

        $('#btnAddMedicine').html('<i class="fa-solid fa-pen"></i> Update Medicine');
    });


/* ==========================================
   REMOVE MEDICINE
========================================== */

$(document).on('click', '.btnRemoveMedicine',

    function () {

        let index = $(this).data('index');

        medicines.splice(index, 1);

        bindMedicines();
    });

/* ==========================================
   CLEAR FIELDS
========================================== */

function clearMedicineFields() {

    $('#ddlMedicine').val('');

    $('#txtQuantity').val('');

    $('#txtDosage').val('');

    $('#txtDays').val('');

    $('#ddlFood').val('After Food');

    $('#chkMorning').prop('checked', false);

    $('#chkLunch').prop('checked', false);

    $('#chkDinner').prop('checked', false);
}

/* ==========================================
    OPEN ADD MEDICINE MODAL
========================================== */
$(document).on('click', '#btnAddNewMedicine',

    function () {
        $('#addMedicineModal').modal('show');
    });


/* ==========================================
   SAVE MEDICINES
========================================== */
$(document).on('click', '#btnSaveMedicine',

    function () {

        $.ajax({

            url: '/Doctors/AddMedicine',

            type: 'POST',

            contentType: 'application/json',

            data:
                JSON.stringify({

                    medicineName: $('#txtMedicineName').val(),

                    strength: $('#txtStrength').val(),

                    medicineType: $('#ddlMedicineType').val(),

                    price: $('#txtMedicinePrice').val()
                }),

            success:
                function (response) {

                    Swal.fire({

                        icon: 'success',

                        title: 'Success',

                        text: response.message
                    });

                    clearMedicineModal();

                    $('#addMedicineModal').modal('hide');

                    loadMedicines();
                }
        });
    });



/* ==========================================
   MODAL CLOSE AND CLEAR FIELDS
========================================== */
$('#addMedicineModal').on('hidden.bs.modal',

    function () {

        clearMedicineModal();
    });

function clearMedicineModal() {

    $('#txtMedicineName').val('');

    $('#txtStrength').val('');

    $('#ddlMedicineType').val('Tablet');

    $('#txtMedicinePrice').val('');
}

$(document).on('hidden.bs.modal', '#consultationModal',

    function () {
        resetConsultationModal();
    });


/* ==========================================
RESET CONSULTATION
========================================== */
function resetConsultationModal() {

    // Reset text fields
    $('#txtDiagnosis').val('');

    $('#txtPrescriptionNotes').val('');

    // Clear medicine form
    clearMedicineFields();

    // Empty medicine array
    medicines.length = 0;

    // Reset edit mode
    editMedicineIndex = -1;

    // Clear table rows
    $('#tblPrescriptionBody').empty();

    // Hide table + print
    $('#medicineTableContainer').hide();

    $('#printContainer').hide();

    // Reset button
    $('#btnAddMedicine').html('<i class="fa-solid fa-plus"></i> Add Medicine');
}

/* ==========================================
   LOAD LAB TESTS
========================================== */
function loadLabTests() {

    $.ajax({

        url: '/Doctors/GetLabTests',

        type: 'GET',

        success:
            function (response) {

                let tests = response.data;

                bindLabCategory(tests, 'Blood Test', '#bloodTestContainer');

                bindLabCategory(tests, 'Urine Test', '#urineTestContainer');

                bindLabCategory(tests, 'ECG', '#ecgTestContainer');

                bindLabCategory(tests, 'Sugar Test', '#sugarTestContainer');

                bindLabCategory(tests, 'CBC', '#cbcTestContainer');
            }
    });
}

/* ==========================================
   BIND LAB TESTS TO CATEGORY
========================================== */

function bindLabCategory(tests, category, containerId) {

    let html = '';

    let filtered = tests.filter(
        x =>
            x.categoryName?.trim().toLowerCase()
            ===
            category.trim().toLowerCase()
    );

    filtered.forEach(

        function (item) {

            html += `<label class="lab-test-item">
            <input type="checkbox" class="lab-checkbox" value="${item.id}" data-name="${item.testName}" />${item.testName}</label>`;
        });

    $(containerId).html(html);
}

/* ==========================================
   LAB DROPDOWN TOGGLE
========================================== */

$(document).on('click', '.lab-dropdown-header',

    function (e) {

        // Ignore clear button click
        if ($(e.target).closest('.btn-clear-card').length) {
            return;
        }

        let $header = $(this);

        let target = $header.data('target');

        let $target = $(target);

        // Close others
        $('.lab-dropdown-body').not($target).stop(true, true).slideUp(200);

        // Reset arrows
        $('.lab-dropdown-header').not($header).find('i').removeClass('fa-chevron-up').addClass('fa-chevron-down');

        // Toggle current
        if ($target.is(':visible')) {

            $target.slideUp(200);

            $header.find('i').removeClass('fa-chevron-up').addClass('fa-chevron-down');
        }
        else {

            $target.slideDown(200);

            $header.find('i').removeClass('fa-chevron-down').addClass('fa-chevron-up');
        }
    });

/* ==========================================
    LAB TEST CHECKBOX CHANGE
========================================== */

$(document).on('change', '.lab-checkbox',

    function () {

        $(this).closest('.lab-test-item').toggleClass('selected', this.checked);
    });


$(document).on('change', '.lab-checkbox',

    function () {

        let $container = $(this).closest('.lab-dropdown-body');

        let checkedCount = $container.find('.lab-checkbox:checked').length;

        let $clearButton = $container.closest('.lab-dropdown-card').find('.btn-clear-card');

        // Show / Hide Clear button
        if (checkedCount > 0) {

            $clearButton.fadeIn(150);
        }
        else {

            $clearButton.fadeOut(150);
        }

        // Selected UI
        $(this).closest('.lab-test-item').toggleClass('selected', this.checked);
    });

/* ==========================================
CLEAR LAB TEST SELECTION
========================================== */
$(document).on('click', '.btn-clear-card',

    function () {

        let container = $(this).data('container');

        $(container).find('.lab-checkbox').prop('checked', false);

        $(container).find('.lab-test-item').removeClass('selected');

        $(this).fadeOut(150);
    });


$(document).on('change', '.scan-checkbox',

    function () {

        let $container =
            $(this)
                .closest(
                    '.lab-dropdown-body'
                );

        let checkedCount =
            $container
                .find(
                    '.scan-checkbox:checked'
                )
                .length;

        let $clearButton =
            $container
                .closest(
                    '.lab-dropdown-card'
                )
                .find(
                    '.btn-clear-card'
                );

        if (checkedCount > 0) {

            $clearButton.show();
        }
        else {

            $clearButton.hide();
        }

        $(this)
            .closest(
                '.lab-test-item'
            )
            .toggleClass(

                'selected',

                this.checked
            );
    });

/* ==========================================
 LOAD SCAN TESTS
========================================== */
function loadScanTests() {

    $.ajax({

        url: '/Doctors/GetScanTests',

        type: 'GET',

        success:
            function (tests) {
                renderScanCards(
                    tests
                );
            }
    });
}


/* ==========================================
 RENDER TESTS FOR SCANNING
========================================== */
function renderScanCards(tests) {

    let grouped = {};

    // Group by category
    tests.forEach(

        function (item) {

            if (!grouped[item.categoryName]) {
                grouped[item.categoryName] = [];
            }

            grouped[item.categoryName].push(item);
        });

    let html = '';

    Object.keys(grouped)
        .forEach(

            function (category) {
                let containerId = category.replaceAll(' ', '').replace('/', '') + 'Container';

                html += `
                <div class="lab-dropdown-card">
                <div class="lab-dropdown-header" data-target="#${containerId}"><span>${category}</span>
                <div class="lab-header-actions">
                <button type="button"
                    class="btn-clear-card"
                    data-container="#${containerId}"
                    style="display:none;">
                Clear
                </button><i class="fa-solid fa-chevron-down"></i>
                </div>
                </div>

    <div id="${containerId}" class="lab-dropdown-body">

        ${grouped[category].map(x => `<label class="lab-test-item">
        <input type="checkbox"
        class="scan-checkbox"
        value="${x.id}"
        data-name="${x.testName}"
        data-category="${x.categoryName}" />
        ${x.testName}
        </label>`).join('')}
    </div></div>`;});

    $('#scanCategoryGrid').html(html);
}


/* ==========================================
 CHECKBOX CHANGE FOR SCANNING 
========================================== */
$(document).on('change', '.scan-checkbox',

    function () {

        let $container = $(this).closest('.lab-dropdown-body');

        let checkedCount = $container.find('.scan-checkbox:checked').length;

        let $clearButton = $container.closest('.lab-dropdown-card').find('.btn-clear-scan-card');

        // Show / Hide
        if (checkedCount > 0) {

            $clearButton.fadeIn(150);
        }
        else {

            $clearButton.fadeOut(150);
        }

        // Selected UI
        $(this).closest('.lab-test-item').toggleClass('selected', this.checked);
    });

/* ==========================================
 CLEAR SCAN TEST CHECKBOX
========================================== */
$(document).on('click','.btn-clear-card',

    function (e) {

        e.stopPropagation();

        let container = $(this).data('container');

        // Uncheck only current card
        $(container).find('.scan-checkbox').prop('checked',false);

        // Remove selected style
        $(container).find('.lab-test-item').removeClass('selected');

        // Hide button
        $(this).fadeOut(150);
    });

