$(document).ready(function () {

    setTimeout(function () {
        initializePatientTable();
    }, 50);

    function initializePatientTable() {
        // Check if table exists
        if ($('#patientsTable').length === 0) {
            console.log("Table not found, retrying...");
            setTimeout(initializePatientTable, 100);
            return;
        }

        // Check if DataTables is available
        if ($.fn.DataTable === undefined) {
            console.log("DataTables not loaded, retrying...");
            setTimeout(initializePatientTable, 100);
            return;
        }

        // Check if hasData variable exists
        var hasData = typeof window.hasData !== 'undefined' ? window.hasData : ($('#patientsTable tbody tr').length > 0);

        if (hasData) {
            // Initialize using global standardized helper
            var table = smartClinicDataTable('#patientsTable', {
                order: [[1, 'desc']],
                columnDefs: [
                    // Sl.No — orderable/searchable handled by global drawCallback
                    { targets: 0, orderable: false, searchable: false },
                    // Action column
                    { targets: 6, orderable: false, searchable: false }
                ],
                language: {
                    searchPlaceholder: 'Search patients...',
                    emptyTable: 'No patients found'
                }
            });

            console.log("DataTable initialized successfully");
        } else {
            console.log("No data found in table");
        }
    }


    //Add Patient button click
    $("#btnAddPatient").click(function () {
        $.get(
            "/Patients/AddPatientModal",
            function (response) {
                $("#patientModalContainer").html(response);
                $("#addPatientModal").modal("show");
            }
        );
    });

    // Save Patient
    $(document).on("click","#btnSavePatient",

        function () {

            savePatient(false);
        });

    // Save and Book Appointment
    $(document).on(
        "click",
        "#btnSavePatientAndBookApp",

        function () {

            savePatient(true);
        });



    function savePatient(
        bookAppointment = false
    ) {

        let patient = {

            Name:
                $("#patientName")
                    .val(),

            Age:
                $("#age")
                    .val(),

            Gender:
                $("#gender")
                    .val(),

            MobileNumber:
                $("#mobile")
                    .val(),

            Email:
                $("#email")
                    .val(),

            Dob:
                $("#dob")
                    .val()
        };

        // Validation
        if (
            patient.Name === ""
        ) {

            Swal.fire({
                icon:
                    'warning',

                title:
                    'Validation Error',

                text:
                    'Please enter patient name'
            });

            return;
        }

        if (
            patient.Gender === ""
        ) {

            Swal.fire({
                icon:
                    'warning',

                title:
                    'Validation Error',

                text:
                    'Please select gender'
            });

            return;
        }

        $.ajax({

            url:
                "/Patients/SavePatient",

            type:
                "POST",

            data:
                patient,

            beforeSend:
                function () {

                    $('#btnSavePatient, #btnSaveAndBookAppointment')
                        .prop(
                            'disabled',
                            true);
                },

            success:
                function (response) {                    
                    if (
                        response.success
                    ) {

                        // CASE 1:
                        // Save Only
                        if (!bookAppointment)
                        {

                            Swal.fire({
                                icon:
                                    'success',

                                title:
                                    'Success',

                                text:
                                    response.message,

                                timer:
                                    2000,

                                timerProgressBar:
                                    true
                            })
                                .then(() => {

                                    $("#addPatientModal")
                                        .modal(
                                            "hide"
                                        );

                                    $("#patientForm")[0].reset();

                                    location.reload();
                                });

                            return;
                        }

                        
                        //openGenerateTokenModal(response.data);

                        // CASE 2:
                        // Save + Book Appointment

                        $("#addPatientModal")
                            .modal("hide");

                        $("#patientForm")[0]
                            .reset();

                        openGenerateTokenModal(response.data);                       
                        
                    }
                    else {

                        Swal.fire({
                            icon:
                                'error',

                            title:
                                'Oops...',

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
                            'Something went wrong while saving patient'
                    });
                },

            complete:
                function () {

                    $('#btnSavePatient, #btnSaveAndBookAppointment')
                        .prop(
                            'disabled',
                            false);
                }
        });
    }
       

    function openGenerateTokenModal(patient) {
        
        // Close Add Patient modal
        $('#addPatientModal').modal('hide');

        setTimeout(function () {

            $.get('/Appointments/GenerateTokenModal',

                function (response) {

                    // Inject modal html
                    $('#tokenModalContainer').html(response);

                    // Load dropdowns
                    loadDepartments();
                    loadPriorities();
                    loadSymptoms();

                    // Open modal
                    const modalElement = document.getElementById('generateTokenModal');

                    const modal = new bootstrap.Modal(modalElement);

                    modal.show();

                    // Modal fully loaded
                    modalElement.addEventListener('shown.bs.modal',

                            function () {

                                // Hide Search Patient section
                                $('#patientSearch').closest('.col-md-12').hide();

                                // Show patient details section
                                $('#generateTokenModal #patientInfoSection').show();

                                $('#generateTokenModal #patientId').val(patient.id);

                                // Prefill patient data
                                $('#generateTokenModal #patientName').val(patient.name);

                                $('#generateTokenModal #mobileNumber').val(patient.mobileNumber);

                                $('#generateTokenModal #age').val(patient.age);                                
                            },
                            {
                                once: true
                            }             
                    );                   
                });

            

        }, 300);
    }


    function initializePatientSearch() {

        $('#patientSearch').select2({dropdownParent:$('#generateTokenModal'),placeholder:'Search UHID / Name / Mobile',minimumInputLength:1,width:'100%',

                ajax: {

                    url:'/Appointments/SearchPatients',

                    dataType:'json',

                    delay:300,

                    data:function (params) {

                            return {
                                search:
                                    params.term
                            };
                        },

                    processResults:function (data) {

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

        $('#patientSearch')
            .on(
                'select2:select',

                function (e) {

                    var patientId =
                        e.params.data.id;

                    $('#patientId')
                        .val(patientId);

                    $.get(
                        '/Appointments/GetPatientDetails',

                        {
                            patientId:
                                patientId
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
                            </option>`
                            );
                    });
            });
    }

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
                            </option>`
                            );
                    });
            });
    }


    function loadSymptoms() {

        $.get(
            '/Appointments/GetSymptoms',

            function (response) {

                let html = '';

                $.each(
                    response,

                    function (
                        i,
                        item
                    ) {

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


    $(document).on('click','.btn-generate-token',

        function () {

            let patient = {

                id: $(this).data('patient-id'),

                name: $(this).data('patient-name'),

                mobileNumber:$(this).data('mobile'),

                age:$(this).data('age'),

                uhid: $(this).data('uhid')
            };

            openGenerateTokenModal(patient);
        });
    
    

    // Cancel Button
    $(document).on("click", "#btnCancelPatient", function () {
        $("#patientForm")[0].reset();
        $("#addPatientModal").modal("hide");
    });

    // View Patient button click
    $(document).on("click", ".btn-view", function () {
        const patientId = $(this).data("patient-id");

        $.ajax({
            url: `/Patients/ViewPatient/${patientId}`,
            type: "GET",
            dataType: "json",
            success: function (response) {
                if (response.success && response.data) {
                    const patient = response.data;

                    // Load the modal
                    $.get(
                        `/Patients/ViewPatientModal/${patientId}`,
                        function (modalResponse) {
                            $("#patientModalContainer").html(modalResponse);

                            // Populate modal fields with patient data
                            $("#viewUhid").text(patient.uhid || 'N/A');
                            $("#viewName").text(patient.name || 'N/A');
                            $("#viewAge").text(patient.age || 'N/A');
                            $("#viewGender").text(patient.gender || 'N/A');
                            $("#viewDob").text(patient.dob ? new Date(patient.dob).toLocaleDateString() : 'N/A');
                            $("#viewMobile").text(patient.mobileNumber || 'N/A');
                            $("#viewSymptoms").text(patient.symptoms || 'N/A');
                            $("#viewCreatedDate").text(patient.createdDateTime ? new Date(patient.createdDateTime).toLocaleDateString() : 'N/A');
                            $("#viewCreatedBy").text(patient.createdBy || 'N/A');
                            $("#viewUpdatedDate").text(patient.updatedDateTime ? new Date(patient.updatedDateTime).toLocaleDateString() : 'N/A');
                            $("#viewUpdatedBy").text(patient.updatedBy || 'N/A');

                            // Show the modal
                            $("#viewPatientModal").modal("show");
                        }
                    );
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message || 'Failed to fetch patient details',
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Something went wrong while fetching patient details',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    // Edit Patient button click
    $(document).on("click", ".btn-edit", function () {
        const patientId = $(this).data("patient-id");

        // Fetch the edit modal
        $.get(
            `/Patients/EditPatientModal/${patientId}`,
            function (response) {
                $("#patientModalContainer").html(response);

                // Fetch patient details and populate the form
                $.ajax({
                    url: `/Patients/GetPatientDetails/${patientId}`,
                    type: "GET",
                    dataType: "json",
                    success: function (response) {
                        if (response.success && response.data) {
                            const patient = response.data;

                            // Populate the form fields
                            $("#patientId").val(patient.id);
                            $("#patientName").val(patient.name);
                            $("#age").val(patient.age);
                            $("#gender").val(patient.gender);
                            $("#dob").val(patient.dob);
                            $("#mobile").val(patient.mobileNumber);
                            $("#symptoms").val(patient.symptoms);

                            // Show the modal
                            $("#editPatientModal").modal("show");
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: response.message || 'Failed to load patient data',
                                confirmButtonText: 'OK'
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Something went wrong while loading patient data',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            }
        );
    });

    // Update Patient
    $(document).on("click", "#btnUpdatePatient", function () {
        let patient = {
            Id: $("#patientId").val(),
            Name: $("#patientName").val(),
            Age: $("#age").val(),
            Gender: $("#gender").val(),
            Dob: $("#dob").val(),
            MobileNumber: $("#mobile").val(),
            Symptoms: $("#symptoms").val()
        };

        // Validation
        if (patient.Name === "") {
            Swal.fire({
                icon: 'warning',
                title: 'Validation Error',
                text: 'Please enter patient name',
                confirmButtonText: 'OK'
            });
            return;
        }

        if (patient.Gender === "") {
            Swal.fire({
                icon: 'warning',
                title: 'Validation Error',
                text: 'Please select gender',
                confirmButtonText: 'OK'
            });
            return;
        }

        $.ajax({
            url: "/Patients/UpdatePatient",
            type: "POST",
            data: patient,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: response.message,
                        confirmButtonText: 'OK',
                        timer: 2000,
                        timerProgressBar: true
                    }).then(() => {
                        $("#editPatientModal").modal("hide");
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: response.message,
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Something went wrong while updating patient',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    // Delete Patient button click
    $(document).on("click", ".btn-delete", function () {
        const patientId = $(this).data("patient-id");

        // Confirmation dialog
        Swal.fire({
            title: 'Confirm Delete',
            text: 'Are you sure you want to delete this patient? This action cannot be undone.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Yes, Delete!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // Proceed with deletion
                $.ajax({
                    url: `/Patients/DeletePatient/${patientId}`,
                    type: "DELETE",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Deleted!',
                                text: response.message,
                                confirmButtonText: 'OK',
                                timer: 2000,
                                timerProgressBar: true
                            }).then(() => {
                                location.reload();
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: response.message || 'Failed to delete patient',
                                confirmButtonText: 'OK'
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Something went wrong while deleting patient',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            }
        });
    });

    // Cancel Button for Edit
    $(document).on("click", "#btnCancelEditPatient", function () {
        $("#editPatientModal").modal("hide");
    });
});

// View All Appointments
$(document).on(
    'click',
    '#btnViewAppointments',

    function () {

        window.location.href =
            '/Appointments/GetAllAppointments';
    }); 

// Add this to your page to show what breakpoint is active
function logScaleInfo() {
    const width = window.innerWidth;
    let breakpoint = '';

    if (width > 1550) breakpoint = '100% scale (normal)';
    else if (width > 1300) breakpoint = '125% scale (your current)';
    else if (width > 1100) breakpoint = '150% scale';
    else if (width > 768) breakpoint = '175%+ scale';
    else breakpoint = 'Mobile view';

    console.log(`Viewport: ${width}px | ${breakpoint}`);

    // Show on screen temporarily for debugging
    const debug = document.createElement('div');
    debug.style.cssText = 'position:fixed; top:5px; right:5px; background:#007bff; color:white; padding:2px 8px; border-radius:3px; font-size:11px; z-index:9999; font-family:monospace;';
    debug.textContent = `${breakpoint}`;
    document.body.appendChild(debug);
    setTimeout(() => debug.remove(), 3000);
}

window.addEventListener('load', logScaleInfo);
window.addEventListener('resize', () => {
    clearTimeout(window.resizeTimer);
    window.resizeTimer = setTimeout(logScaleInfo, 500);
});


// Auto-fix for any Windows scaling
function fixLayoutForScaling() {
    const viewportWidth = window.innerWidth;
    const tableWrapper = document.querySelector('.table-responsive');
    const table = document.getElementById('patientsTable');

    if (!table) return;

    // Log current state for debugging
    console.log(`Viewport width: ${viewportWidth}px`);

    // If viewport is too narrow, ensure horizontal scroll is enabled
    if (viewportWidth < 1200) {
        if (tableWrapper) {
            tableWrapper.style.overflowX = 'auto';
            tableWrapper.style.display = 'block';
        }
        table.style.minWidth = '900px';
    } else {
        if (tableWrapper) {
            tableWrapper.style.overflowX = 'auto';
        }
        table.style.minWidth = '100%';
    }

    // Fix for 150% scale specifically
    if (viewportWidth <= 1300 && viewportWidth >= 1100) {
        console.log('150% scale detected - enabling horizontal scroll');
        if (tableWrapper) {
            tableWrapper.style.overflowX = 'auto';
            tableWrapper.style.WebkitOverflowScrolling = 'touch';
        }
    }
}

// Also check for DataTables initialization
$(document).ready(function () {
    // Run after DataTables is initialized
    setTimeout(fixLayoutForScaling, 100);

    // Run on window resize
    $(window).on('resize', function () {
        setTimeout(fixLayoutForScaling, 50);
    });
});

// Run on load
window.addEventListener('load', fixLayoutForScaling);
window.addEventListener('resize', function () {
    setTimeout(fixLayoutForScaling, 100);
});


$(document).on(
    'hidden.bs.modal',
    '#generateTokenModal',

    function () {

        location.reload();
    });
