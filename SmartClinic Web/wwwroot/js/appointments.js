$(function () {

    loadAppointments();

    // Open Generate Token Modal
    $(document).on('click', '#btnGenerateToken',
        function () {

            $.get(
                '/Appointments/GenerateTokenModal',

                function (response) {

                    $('#tokenModalContainer')
                        .html(response);

                    $('#generateTokenModal')
                        .modal('show');


                });
        });

    // Modal Open
    $(document).on(
        'shown.bs.modal',
        '#generateTokenModal',

        function () {

            resetTokenForm();

            loadDepartments();

            loadPriorities();

            loadSymptoms();

            initializePatientSearch();
        });
});


// Patient Search
function initializePatientSearch() {

    if ($('#patientSearch').hasClass('select2-hidden-accessible')) {

        $('#patientSearch').select2('destroy');
    }


    $('#patientSearch')
        .select2({

            dropdownParent:
                $('#generateTokenModal'),

            placeholder:
                'Search UHID / Name / Mobile',

            minimumInputLength: 0,

            width: '100%',

            ajax: {

                url: '/Appointments/SearchPatients',

                dataType: 'json',

                delay: 300,

                data:
                    function (params) {
                        console.log('Search Term:', params.term);
                        return {
                            search: params.term || ''
                        };
                    },

                processResults:
                    function (data) {
                        console.log(data);
                        return {

                            results:
                                $.map(
                                    data,
                                    function (item) {

                                        return {

                                            id:
                                                item.id,

                                            text:
                                                item.uhid
                                                + ' - '
                                                + item.name
                                                + ' - '
                                                + item.mobileNumber
                                        };
                                    })
                        };
                    }
            }
        });

    // Force focus when dropdown opens
    $('#patientSearch')
        .on(
            'select2:open',

            function () {

                setTimeout(function () {

                    document
                        .querySelector(
                            '.select2-search__field'
                        )
                        ?.focus();

                }, 50);
            });

    // Patient Selected
    $('#patientSearch')
        .on(
            'select2:select',

            function (e) {

                var patientId = e.params.data.id;

                // Save patient id
                $('#patientId')
                    .val(patientId);

                $.get(
                    '/Appointments/GetPatientDetails',

                    {
                        patientId: patientId
                    },

                    function (response) {

                        $('#patientInfoSection')
                            .show();

                        $('#patientName')
                            .val(response.name);

                        $('#mobileNumber')
                            .val(response.mobileNumber);

                        $('#age')
                            .val(response.age);
                    });
            });
}

function loadAppointments() {

    $.ajax({

        url:
            '/Appointments/GetAppointments',

        type:
            'GET',

        beforeSend:
            function () {

                $('#appointmentsTable tbody')
                    .html(`
                        <tr>
                            <td colspan="10"
                                class="text-center py-4">

                                <div class="spinner-border text-primary">
                                </div>

                                <p class="mt-2 text-muted">
                                    Loading appointments...
                                </p>

                            </td>
                        </tr>
                    `);
            },

        success:
            function (response) {

                // Destroy existing DataTable
                if (
                    $.fn.DataTable
                        .isDataTable(
                            '#appointmentsTable'
                        )
                ) {

                    $('#appointmentsTable')
                        .DataTable()
                        .destroy();
                }

                let tbody =
                    $('#appointmentsTable tbody');

                tbody.empty();

                // No data
                if (
                    !response.success ||
                    !response.data ||
                    response.data.length === 0
                ) {

                    tbody.html(`
                        <tr>
                            <td colspan="10"
                                class="text-center py-5">

                                <i class="bi bi-calendar-x fs-1 text-muted">
                                </i>

                                <p class="mt-3 text-muted">
                                    No appointments found
                                </p>

                            </td>
                        </tr>
                    `);

                    return;
                }

                // Append rows
                $.each(
                    response.data,

                    function (
                        index,
                        item
                    ) {

                        tbody.append(`
                            <tr>

                                <!-- Sl.No -->
                                <td></td>

                                <!-- Token -->
                                <td>
                                    ${item.tokenNumber ?? '-'}
                                </td>

                                <!-- UHID -->
                                <td>
                                    ${item.uhid ?? '-'}
                                </td>

                                <!-- Patient -->
                                <td>
                                    ${item.patientName ?? '-'}
                                </td>

                                <!-- Doctor -->
                                <td>
                                    ${item.doctorName ?? '-'}
                                </td>

                                <!-- Department -->
                                <td>
                                    ${item.departmentName ?? '-'}
                                </td>

                                <!-- Priority -->
                                <td>
                                    ${getPriorityBadge(
                            item.priorityName
                        )}
                                </td>

                                <!-- Status -->
                                <td>
                                    ${getStatusBadge(
                            item.statusName
                        )}
                                </td>

                                <!-- Created Time -->
                                <td>
                                    ${formatDate(
                            item.createdTime
                        )}
                                </td>

                                <!-- Action -->
                               <td>
                               ${getActionButtons(item)}
                               </td>

                            </tr>
                        `);
                    });

                // Initialize DataTable
                var table = $('#appointmentsTable')
                    .DataTable({

                        responsive:
                            true,

                        destroy:
                            true,

                        pageLength:
                            10,

                        lengthMenu:
                            [
                                [10, 25, 50, 100],
                                [10, 25, 50, 100]
                            ],

                        searching:
                            true,

                        ordering:
                            true,

                        paging:
                            true,

                        info:
                            true,

                        autoWidth:
                            false,

                        // Sort by token no
                        order:
                            [[8, 'desc']],

                        columnDefs: [

                            // Sl.No
                            {
                                targets: 0,
                                orderable: false,
                                searchable: false
                            },

                            // Action
                            {
                                targets: 9,
                                orderable: false,
                                searchable: false
                            }
                        ],

                        language: {

                            search:
                                '',

                            searchPlaceholder:
                                'Search appointments...',

                            emptyTable:
                                'No appointments found'
                        }
                    });

                // Dynamic Serial Number
                table.on(
                    'draw.dt',

                    function () {

                        let pageInfo =
                            table.page.info();

                        table
                            .column(
                                0,
                                {
                                    page:
                                        'current'
                                })
                            .nodes()
                            .each(function (
                                cell,
                                index
                            ) {

                                cell.innerHTML =
                                    pageInfo.start +
                                    index +
                                    1;
                            });
                    });

                table.draw();
            },

        error:
            function () {

                $('#appointmentsTable tbody')
                    .html(`
                        <tr>
                            <td colspan="10"
                                class="text-center text-danger py-5">

                                Unable to load appointments

                            </td>
                        </tr>
                    `);

                Swal.fire({
                    icon:
                        'error',

                    title:
                        'Error',

                    text:
                        'Unable to load appointments'
                });
            }
    });
}


function getPriorityBadge(priority) {

    switch (priority?.trim()
        ?.toLowerCase()
    ) {

        case 'normal':

            return `
                <span class="badge bg-primary">
                    Normal
                </span>
            `;

        case 'medium':

            return `
                <span class="badge bg-warning text-dark">
                    Medium
                </span>
            `;

        case 'high':

            return `
                <span class="badge bg-dark">
                    High
                </span>
            `;

        case 'emergency':

            return `  
                <span class="badge bg-danger">
                    Emergency
                </span>
            `;

        default:

            return `
                <span class="badge bg-secondary">
                    -
                </span>
            `;
    }
}

function getStatusBadge(status) {

    switch (
    status?.trim()
        ?.toLowerCase()
    ) {

        case 'pending':

            return `
                <span class="badge bg-warning text-dark">
                    Pending
                </span>
            `;

        case 'completed':

            return `
                <span class="badge bg-success">
                    Completed
                </span>
            `;

        case 'cancelled':

            return `
                <span class="badge bg-danger">
                    Cancelled
                </span>
            `;

        default:

            return `
                <span class="badge bg-secondary">
                    ${status ?? '-'}
                </span>
            `;
    }
}

function formatDate(date) {

    if (!date)
        return '-';

    let d =
        new Date(date);

    return d.toLocaleString(
        'en-IN',
        {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
}

//Action Button Function
function getActionButtons(item) {

    let status =
        item.statusName
            ?.toLowerCase();

    // Pending
    if (status === 'pending') {

        return `

            <button class="btn btn-sm btn-success btn-call"
                    data-id="${item.id}"
                    data-token="${item.tokenNumber}"
                    title="Call Patient">

                <i class="bi bi-megaphone-fill">
                </i>

                Call

            </button>
        `;
    }

    // Called
    if (
        status === 'called'
    ) {

        return `

            <div class="btn-group">

                <button class="btn btn-sm btn-warning btn-recall"
                        data-id="${item.id}"
                        title="Recall">

                    <i class="bi bi-arrow-repeat">
                    </i>

                </button>

                <button class="btn btn-sm btn-danger btn-skip"
                        data-id="${item.id}"
                        title="Skip">

                    <i class="bi bi-skip-forward-fill">
                    </i>

                </button>

                <button class="btn btn-sm btn-primary btn-complete"
                        data-id="${item.id}"
                        title="Complete">

                    <i class="bi bi-check-circle-fill">
                    </i>

                </button>

            </div>
        `;
    }

    // Skipped
    if (
        status === 'skipped'
    ) {

        return `

            <button class="btn btn-sm btn-warning btn-recall"
                    data-id="${item.id}"
                    title="Recall Patient">

                <i class="bi bi-arrow-repeat">
                </i>

                Recall

            </button>
        `;
    }

    // Completed
    if (
        status === 'completed'
    ) {

        return `

            <span class="badge bg-success">

                Consultation Completed

            </span>
        `;
    }

    return '-';
}

//Call Patient
$(document).on('click', '.btn-call',

    function () {

        let appointmentId = $(this).data('id');

        let tokenNumber = $(this).data('token');

        Swal.fire({

            icon:
                'info',

            title:
                'Calling Patient',

            text:
                `Token ${tokenNumber}`,

            timer:
                1500,

            showConfirmButton:
                false
        });

        updateAppointmentStatus(
            appointmentId,
            'Called'
        );
    });

//Skip Patient
$(document).on('click', '.btn-skip',

    function () {

        let appointmentId =
            $(this).data('id');

        updateAppointmentStatus(
            appointmentId,
            'Skipped'
        );
    });

//Recall Patient
$(document).on('click', '.btn-recall',

    function () {

        let appointmentId =
            $(this).data('id');

        updateAppointmentStatus(
            appointmentId,
            'Called'
        );
    });


//Complete Consultation
$(document).on(
    'click',
    '.btn-complete',

    function () {

        let appointmentId =
            $(this).data('id');

        updateAppointmentStatus(
            appointmentId,
            'Completed'
        );
    });

//Status Update Method
function updateAppointmentStatus(
    appointmentId,
    status
) {

    $.ajax({

        url:
            '/Appointments/UpdateStatus',

        type:
            'POST',

        data: {

            appointmentId: appointmentId,

            status: status
        },

        success:
            function (response) {

                if (
                    response.success
                ) {

                    loadAppointments();
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
                        'Unable to update status'
                });
            }
    });
}


let priorityTimer;

$(document).on('change', '.symptom-checkbox',

    function () {

        let selectedSymptoms =
            $('.symptom-checkbox:checked');

        if (
            selectedSymptoms.length === 0
        ) {

            resetSymptomsUI();
            return;
        }

        $('#clearSymptoms')
            .removeClass('d-none');

        clearTimeout(priorityTimer);

        priorityTimer =
            setTimeout(function () {

                $('#otherSymptomsContainer')
                    .fadeOut(200);

                calculatePriority();

            }, 1000); // wait for multiple clicks
    });


$(document).on('click', '#clearSymptoms',

    function () {

        $('.symptom-checkbox')
            .prop('checked', false);

        $(this)
            .addClass('d-none');

        $('#otherSymptomsContainer')
            .fadeIn(200);

        resetSymptomsUI();
    });

// Other Symptoms typing
$(document).on('keyup', '#otherSymptoms',

    function () {

        let otherSymptoms = $(this).val().trim();

        if (otherSymptoms !== '') {

            // Hide Symptoms
            $('#symptomsContainer')
                .fadeOut(200);

            // Show reset button
            $('#clearOtherSymptoms')
                .removeClass('d-none');

            calculatePriority();
        }
        else {

            $('#symptomsContainer')
                .fadeIn(200);

            $('#clearOtherSymptoms')
                .addClass('d-none');

            $('#priorityContainer')
                .addClass('d-none');
        }
    });


// Reset function
function resetSymptomsUI() {

    // Show Other Symptoms again
    $('#otherSymptomsContainer')
        .fadeIn(200);

    // Hide clear button
    $('#clearSymptoms')
        .addClass('d-none');

    // Reset priority
    $('#priorityContainer')
        .addClass('d-none');

    $('#priorityId')
        .val('');
}

// Reset Other Symptoms
$(document).on(
    'click',
    '#clearOtherSymptoms',

    function () {

        // Clear textarea
        $('#otherSymptoms')
            .val('');

        // Hide reset button
        $(this)
            .addClass('d-none');

        // Show symptoms again
        $('#symptomsContainer')
            .fadeIn(200);

        // Reset priority
        $('#priorityId')
            .val('');

        $('#priorityContainer')
            .addClass('d-none');

        $('#priorityWarning')
            .addClass('d-none');
    });

// Load Departments
function loadDepartments() {

    $.get(
        '/Appointments/GetDepartments',

        function (response) {

            $('#departmentId')
                .empty()
                .append(
                    '<option value="">Select Department</option>'
                );

            $.each(
                response,

                function (i, item) {

                    $('#departmentId')
                        .append(
                            `<option value="${item.id}">
                                ${item.name}
                            </option>`);
                });
        });
}

// Load Priorities
function loadPriorities() {

    $.get(
        '/Appointments/GetPriorities',

        function (response) {

            $('#priorityId')
                .empty()
                .append(
                    '<option value="">Select Priority</option>'
                );

            $.each(
                response,

                function (i, item) {

                    $('#priorityId')
                        .append(
                            `<option value="${item.id}">
                                ${item.name}
                            </option>`);
                });
        });
}

// Department Changed
$(document).on('change', '#departmentId',

    function () {

        var departmentId =
            $(this).val();

        $('#doctorId')
            .empty()
            .append(
                '<option value="">Select Doctor</option>'
            );

        if (!departmentId)
            return;

        $.get(
            '/Appointments/GetDoctorsByDepartment',

            {
                departmentId:
                    departmentId
            },

            function (response) {

                $.each(
                    response,

                    function (i, item) {

                        $('#doctorId')
                            .append(
                                `<option value="${item.id}">
                                    ${item.name}
                                </option>`);
                    });
            });
    });


function loadSymptoms() {

    $.get(
        '/Appointments/GetSymptoms',

        function (response) {

            let html = '';

            $.each(response, function (i, item) {

                html += `
                    <label class="symptom-card">

                        <input type="checkbox"
                               class="symptom-checkbox"
                               value="${item.id}">

                        <span class="symptom-text">
                            ${item.name}
                        </span>

                    </label>
                `;
            });

            $('#symptomsList')
                .html(html);
        });
}

// Hide warning after priority selection
$(document).on('change', '#priorityId',

    function () {

        applyPriorityColor();

        let priority =
            $(this).val();

        if (priority !== '') {

            $('#priorityWarning')
                .fadeOut(200);
        }
    });


// $(document).on('click','#btnSaveToken',

//     function () {
//         generateToken();
//     });

$(document).on('click', '#btnSaveToken',

    function () {

        if (!validateTokenForm()) {
            return;
        }

        // Load fee dynamically if needed
        $('#txtConsultationFee')
            .val('300');

        $('#paymentModal')
            .modal('show');
    });


function validateTokenForm() {

    let symptomIds = [];

    $('.symptom-checkbox:checked')
        .each(function () {

            symptomIds.push(
                $(this).val()
            );
        });

    let patientId =
        $('#patientId').val();

    let otherSymptoms =
        $('#otherSymptoms')
            .val()
            .trim();

    let priorityId =
        $('#priorityId').val();

    let departmentId =
        $('#departmentId').val();

    let doctorId =
        $('#doctorId').val();

    let visitType =
        $('#visitType').val();

    if (!patientId || patientId === '0') {

        Swal.fire({
            icon: 'warning',
            title: 'Validation',
            text: 'Please select a patient'
        });

        return false;
    }

    if (
        symptomIds.length === 0 &&
        otherSymptoms === ''
    ) {

        Swal.fire({
            icon: 'warning',
            title: 'Validation',
            text: 'Please select or enter symptoms'
        });

        return false;
    }

    if (!priorityId) {

        Swal.fire({
            icon: 'warning',
            title: 'Validation',
            text: 'Please select priority'
        });

        return false;
    }

    if (!departmentId) {

        Swal.fire({
            icon: 'warning',
            title: 'Validation',
            text: 'Please select department'
        });

        return false;
    }

    if (!doctorId) {

        Swal.fire({
            icon: 'warning',
            title: 'Validation',
            text: 'Please select doctor'
        });

        return false;
    }

    if (!visitType) {

        Swal.fire({
            icon: 'warning',
            title: 'Validation',
            text: 'Please select visit type'
        });

        return false;
    }

    return true;
}


$(document).on('click', '#btnConfirmPayment',

    function () {

        let paymentMode =
            $('#ddlPaymentMode').val();

        if (!paymentMode) {

            Swal.fire({

                icon: 'warning',

                title: 'Payment',

                text: 'Please select payment mode'
            });

            return;
        }

        $.ajax({

            url:
                '/Appointments/CollectPayment',

            type:
                'POST',

            contentType:
                'application/json',

            data:
                JSON.stringify({

                    patientId:
                        $('#patientId').val(),

                    doctorId:
                        $('#doctorId').val(),

                    consultationFee:
                        $('#txtConsultationFee').val(),

                    paymentMode:
                        paymentMode,

                    notes:
                        'Consultation Payment'
                }),

            success:
                function (response) {

                    if (response.success) {

                        $('#paymentModal')
                            .modal('hide');

                        // Generate appointment token
                        generateToken();
                    }
                }
        });
    });




function generateToken() {

    // ======================
    // Request Object
    // ======================

    let symptomIds = [];
    
    $('.symptom-checkbox:checked')
        .each(function () {

            symptomIds.push(
                $(this).val()
            );
        });

    let patientId = $('#patientId').val();

    let priorityId = $('#priorityId').val();

    let departmentId = $('#departmentId').val();

    let doctorId = $('#doctorId').val();

    let visitType = $('#visitType').val();

    let otherSymptoms = $('#otherSymptoms').val().trim();

    let appointmentDateTime = $('#appointmentDateTime').val();

    let request = {

        patientId: patientId,

        symptoms: symptomIds,

        otherSymptoms: otherSymptoms,

        priorityId: priorityId,

        departmentId: departmentId,

        doctorId: doctorId,

        visitType: visitType,

        AppointmentDateTime: appointmentDateTime
    };


    $.ajax({

        url: '/Appointments/CheckAppointmentConflict',

        type: 'POST',

        contentType: 'application/json',

        data: JSON.stringify({

            patientId: parseInt(patientId),

            appointmentDateTime: appointmentDateTime
        }),

        success: function (response) {

            let conflict = response;

            // =========================
            // BLOCK
            // =========================
            if (conflict.type === 'BLOCK') {

                Swal.fire({

                    icon: 'error',

                    title: 'Duplicate Appointment',

                    text: conflict.message,

                    confirmButtonText: 'OK'
                });

                return;
            }


            // =========================
            // WARNING
            // =========================
            if (conflict.type === 'WARNING_30_MIN' || conflict.type === 'WARNING_SAME_DAY') {

                Swal.fire({
                    icon: 'warning',

                    title: 'Appointment Exists',

                    text: conflict.message,

                    showCancelButton: true,

                    confirmButtonText: 'Yes, Continue',

                    cancelButtonText: 'Cancel'
                })

                    .then(function (result) {

                        if (result.isConfirmed) {

                            request.isReschedule = true;

                            request.existingTokenId = conflict.conflictDetails.tokenId;

                            saveToken(request);
                        }
                    });

                return;
            }

            // =========================
            // No conflict
            // =========================
            saveToken(request);
        },

        error:
            function () {

                Swal.fire({

                    icon: 'error',

                    title: 'Error',

                    text: 'Unable to verify appointment'
                });
            }
    });
}


function saveToken(request) {

    $.ajax({

        url: '/Appointments/GenerateToken',

        type: 'POST',

        contentType: 'application/json',

        data: JSON.stringify(request),

        beforeSend:
            function () {

                $('#btnSaveToken').prop('disabled', true).html('<span class="spinner-border spinner-border-sm"></span> Generating...');
            },

        success:
            function (response) {

                if (response.success) {

                    Swal.fire({

                        icon: 'success',

                        title: 'Token Generated',

                        text: `Token No: ${response.data.tokenNumber}`
                    });

                    $('#generateTokenModal').modal('hide');

                    loadAppointments();                    
                }
                else {

                    Swal.fire({

                        icon: 'error',

                        title: 'Failed',

                        text: response.message
                    });
                }
            },

        error:
            function () {

                Swal.fire({

                    icon: 'error',

                    title: 'Error',

                    text: 'Unable to generate token'
                });
            },

        complete:
            function () {
                $('#btnSaveToken').prop('disabled', false).html('<i class="bi bi-check-circle me-1"></i> Generate Token');
            }
    });
}





function calculatePriority() {
    let symptomNames = [];

    // Get checked checkbox symptom names
    $('.symptom-checkbox:checked')
        .each(function () {

            symptomNames.push(
                $(this)
                    .siblings('.symptom-text')
                    .text()
                    .trim()
            );
        });

    let otherSymptoms =
        $('#otherSymptoms')
            .val()
            .trim();

    // Reset warning
    $('#priorityWarning')
        .addClass('d-none');

    // Nothing entered
    if (
        symptomNames.length === 0 &&
        otherSymptoms === ''
    ) {

        $('#priorityContainer')
            .addClass('d-none');

        $('#priorityId')
            .val('')
            .prop('disabled', true);

        return;
    }

    // Show priority
    $('#priorityContainer')
        .removeClass('d-none');

    // CASE 1:
    // Only custom symptom
    if (
        symptomNames.length === 0 &&
        otherSymptoms !== ''
    ) {

        $('#priorityId')
            .val('')
            .prop('disabled', false);

        $('#priorityWarning')
            .removeClass('d-none')
            .text(
                'Symptom not found. Select priority.'
            );

        return;
    }

    // CASE 2:
    // Checkbox symptoms selected
    $.ajax({

        url:
            '/Appointments/CalculatePriority',

        type:
            'POST',

        data: {

            symptoms:
                symptomNames.join(',')
        },

        success:
            function (response) {

                if (response.success) {

                    // Auto set priority
                    $('#priorityId')
                        .val(response.data.id)
                        .trigger('change');

                    // Apply color
                    applyPriorityColor();

                    // CASE 3:
                    // Symptoms + custom symptom
                    if (otherSymptoms !== '') {
                        $('#priorityId').prop('disabled', false);

                        $('#priorityWarning')
                            .removeClass(
                                'd-none')
                            .text(
                                'Custom symptom added. Review priority.'
                            );
                    }
                    else {

                        // Lock auto priority
                        $('#priorityId')
                            .prop(
                                'disabled',
                                true);
                    }
                }
            },

        error:
            function () {

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text:
                        'Unable to calculate priority'
                });
            }
    });
}



// Reset Form
function resetTokenForm() {

    $('#patientSearch')
        .val(null)
        .trigger('change');

    $('#patientInfoSection')
        .hide();

    $('#patientName').val('');
    $('#mobileNumber').val('');
    $('#age').val('');

    $('#symptoms')
        .val([]);

    $('#otherSymptoms')
        .val('');

    $('#departmentId')
        .val('');

    $('#doctorId')
        .empty()
        .append(
            '<option value="">Select Doctor</option>'
        );

    $('#priorityId')
        .prop('disabled', true)
        .val('');

    $('#priorityWarning')
        .addClass('d-none');
}

function applyPriorityColor() {

    let priority =
        $('#priorityId option:selected')
            .text().trim()
            .toLowerCase();

    $('#priorityId')
        .removeClass(
            'priority-low priority-normal priority-medium priority-high priority-emergency'
        );

    switch (priority) {

        case 'low':
            $('#priorityId')
                .addClass(
                    'priority-low');
            break;

        case 'normal':
            $('#priorityId')
                .addClass(
                    'priority-normal');
            break;

        case 'medium':
            $('#priorityId')
                .addClass(
                    'priority-medium');
            break;

        case 'high':
            $('#priorityId')
                .addClass(
                    'priority-high');
            break;

        case 'emergency':
            $('#priorityId')
                .addClass(
                    'priority-emergency');
            break;
    }
}




