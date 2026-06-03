var sa = (function () {
    function get(url) { return fetch(url).then(r => r.json()); }
    function postJson(url, data) { return fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) }).then(r => r.json()); }
    function putJson(url, data) { return fetch(url, { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) }).then(r => r.json()); }
    function del(url) { return fetch(url, { method: 'DELETE' }).then(r => r.json()); }

    async function loadRoles() {
        var items = await get('/SystemAdmin/RolesList');
        var html = '<table class="table" id="tblRoles"><thead><tr><th>Name</th><th>Description</th><th></th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr>
            <td>${i.name}</td>
            <td>${i.discription}</td>
            <td>
            <button class="btn btn-sm btn-secondary" onclick="sa.openRoleEdit(${i.id})">Edit</button>
            <button class="btn btn-sm btn-danger" onclick="sa.deleteRole(${i.id})">Delete</button>
            </td></tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('rolesList').innerHTML = html;
        smartClinicDataTable('#tblRoles', {
            language: { searchPlaceholder: 'Search roles...', emptyTable: 'No roles found' }
        });
    }
    async function loadUsers() {
        var items = await get('/SystemAdmin/GetUsers');
        var html = '<table class="table" id="tblUsers"><thead><tr><th>Username</th><th>Email</th><th>Role</th><th></th></tr></thead><tbody>';
        items.forEach(i => {
            html += `
            <tr>
                <td>${i.userName}</td>
                <td>${i.email}</td>
                <td>${i.roleName || ''}</td>
                <td><button class="btn btn-sm btn-secondary" onclick="sa.openUserEdit(${i.id})">Edit</button>
                <button class="btn btn-sm btn-danger" onclick="sa.deleteUser(${i.id})">Delete</button>
                </td>
            </tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('usersList').innerHTML = html;
        smartClinicDataTable('#tblUsers', {
            language: { searchPlaceholder: 'Search users...', emptyTable: 'No users found' }
        });
    }
    async function loadDepartments() {
        var items = await get('/SystemAdmin/GetDepartments');
        var html = '<table class="table" id="tblDepartments"><thead><tr><th>Name</th><th>Description</th><th></th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr>
            <td>${i.name}</td>
            <td>${i.description}</td>
            <td>
            <button class="btn btn-sm btn-secondary" onclick="sa.openDepartmentEdit(${i.id})">Edit</button> 
            <button class="btn btn-sm btn-danger" onclick="sa.deleteDepartment(${i.id})">Delete</button>
            </td>
            </tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('departmentsList').innerHTML = html;
        smartClinicDataTable('#tblDepartments', {
            language: { searchPlaceholder: 'Search departments...', emptyTable: 'No departments found' }
        });
    }
    async function loadSymptoms() {
        var items = await get('/SystemAdmin/GetSymptoms');
        var html = '<table class="table" id="tblSymptoms"><thead><tr><th>Name</th><th>Weight</th><th></th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.name}</td><td>${i.weight}</td><td><button class="btn btn-sm btn-secondary" onclick="sa.openSymptomEdit(${i.id})">Edit</button> <button class="btn btn-sm btn-danger" onclick="sa.deleteSymptom(${i.id})">Delete</button></td></tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('symptomsList').innerHTML = html;
        smartClinicDataTable('#tblSymptoms', {
            language: { searchPlaceholder: 'Search symptoms...', emptyTable: 'No symptoms found' }
        });
    }
    async function loadPriorities() {
        var items = await get('/SystemAdmin/GetPriorities');
        var html = '<table class="table" id="tblPriorities"><thead><tr><th>Name</th><th>Level</th><th>Color</th><th></th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.name}</td><td>${i.priorityLevel}</td><td>${i.color || ''}</td><td><button class="btn btn-sm btn-secondary" onclick="sa.openPriorityEdit(${i.id})">Edit</button> <button class="btn btn-sm btn-danger" onclick="sa.deletePriority(${i.id})">Delete</button></td></tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('prioritiesList').innerHTML = html;
        smartClinicDataTable('#tblPriorities', {
            language: { searchPlaceholder: 'Search priorities...', emptyTable: 'No priorities found' }
        });
    }
    async function loadTokenStatuses() {
        var items = await get('/SystemAdmin/GetTokenStatuses');
        var html = '<table class="table" id="tblTokenStatuses"><thead><tr><th>Name</th><th>Description</th><th></th></tr></thead><tbody>';
        items.forEach(i => {
            html += `<tr><td>${i.name}</td><td>${i.description}</td><td><button class="btn btn-sm btn-secondary" onclick="sa.openTokenStatusEdit(${i.id})">Edit</button> <button class="btn btn-sm btn-danger" onclick="sa.deleteTokenStatus(${i.id})">Delete</button></td></tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('tokenStatusesList').innerHTML = html;
        smartClinicDataTable('#tblTokenStatuses', {
            language: { searchPlaceholder: 'Search token statuses...', emptyTable: 'No token statuses found' }
        });
    }



    // Roles
    function openRoleCreate() {
        $('#Role_Id').val('');
        $('#Role_Name').val('');
        $('#Role_Description').val('');
        var modal = new bootstrap.Modal(document.getElementById('roleModal'));
        modal.show();
        $('#roleSaveBtn').off('click').on('click', async function () {
            var model = { Name: $('#Role_Name').val(), discription: $('#Role_Description').val() };
            try {
                var response = await postJson('/SystemAdmin/CreateRole', model);
                if (response.success) {
                    modal.hide();
                    loadRoles();
                    showSuccess('Role created successfully');
                }
                else {
                    showError('Failed to create role');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function openRoleEdit(id) {
        var items = await get('/SystemAdmin/RolesList');
        var item = items.find(x => x.id === id);
        if (!item) return;
        $('#Role_Id').val(item.id);
        $('#Role_Name').val(item.name);
        $('#Role_Description').val(item.discription);
        var modal = new bootstrap.Modal(document.getElementById('roleModal'));
        modal.show();
        $('#roleSaveBtn').off('click').on('click', async function () {
            var model = { Id: item.id, Name: $('#Role_Name').val(), discription: $('#Role_Description').val() };
            try {
                var response = await postJson('/SystemAdmin/UpdateRole', model);
                if (response.success) {
                    modal.hide();
                    loadRoles();
                    showSuccess('Role updated successfully');
                }
                else {
                    showError('Failed to update role');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deleteRole(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {
                var response = await postJson('/SystemAdmin/DeleteRole?id=' + id, {});
                if (response.success) {
                    loadRoles();
                    showSuccess('Role deleted successfully');
                }
                else {
                    showError('Failed to delete role');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        }
    }
    // Users
    function openUserCreate() {
        $('#User_Id').val('');
        $('#User_UserName').val('');
        $('#User_Email').val('');
        $('#User_ContactNo').val('');
        $('#User_Password').val('');
        $('#User_RoleId').empty();
        // load roles into select
        get('/SystemAdmin/RolesList').then(r => { r.forEach(x => $('#User_RoleId').append(`<option value="${x.id}">${x.name}</option>`)); });
        get('/SystemAdmin/GetDepartments').then(r => { r.forEach(x => $('#User_DepartmentId').append(`<option value="${x.id}">${x.name}</option>`)); });
        var modal = new bootstrap.Modal(document.getElementById('userModal'));
        modal.show();
        $('#userSaveBtn').off('click').on('click', async function () {
            var model = {
                UserName: $('#User_UserName').val(),
                Email: $('#User_Email').val(),
                ContactNumber: $('#User_ContactNo').val(),
                PasswordHash: $('#User_Password').val(),
                RoleId: parseInt($('#User_RoleId').val() || '0'),
                DepartmentId: parseInt($('#User_DepartmentId').val() || '0')
            };
            try {
                var response = await postJson('/SystemAdmin/CreateUser', model);

                if (response.success) {
                    modal.hide();
                    loadUsers();
                    showSuccess('User created successfully');
                }
                else {
                    showError('Failed to create user');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
            modal.hide();
            loadUsers();
        });
    }
    async function openUserEdit(id) {
        const items = await get('/SystemAdmin/GetUsers');
        const item = items.find(x => x.id === id);
        if (!item) return;
        $('#User_Id').val(item.id);
        $('#User_UserName').val(item.userName);
        $('#User_Email').val(item.email);
        $('#User_Password').val(item.passwordHash);
        $('#User_ContactNo').val(item.contactNumber || '');
        $('#User_RoleId').empty();
        get('/SystemAdmin/RolesList').then(r => { r.forEach(x => $('#User_RoleId').append(`<option value="${x.id}" ${x.id == item.roleId ? 'selected' : ''}>${x.name}</option>`)); });
        get('/SystemAdmin/GetDepartments').then(r => { r.forEach(x => $('#User_DepartmentId').append(`<option value="${x.id}">${x.name}</option>`)); });
        var modal = new bootstrap.Modal(document.getElementById('userModal'));
        modal.show();
        $('#userSaveBtn').off('click').on('click', async function () {
            var model =
            {
                Id: item.id,
                UserName: $('#User_UserName').val(),
                Email: $('#User_Email').val(),
                PasswordHash: $('#User_Password').val(),
                RoleId: parseInt($('#User_RoleId').val() || '0'),
                ContactNumber: $('#User_ContactNo').val(),
                DepartmentId: parseInt($('#User_DepartmentId').val() || '0')
            };
            try {
                var response = await postJson('/SystemAdmin/UpdateUser', model);
                if (response.success) {
                    modal.hide();
                    loadUsers();
                    showSuccess('User updated successfully');
                }
                else {
                    showError('Failed to update user');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deleteUser(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {

                var response = await postJson('/SystemAdmin/DeleteUser?id=' + id, {});

                if (response.success) {

                    loadUsers();

                    Swal.fire({
                        icon: 'success',
                        title: 'Deleted!',
                        text: 'User deleted successfully',
                        timer: 2000,
                        showConfirmButton: false
                    });

                } else {

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to delete user'
                    });

                }

            } catch (e) {

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Something went wrong'
                });

            }
        }
    }
    // Departments
    function openDepartmentCreate() {
        $('#Department_Id').val('');
        $('#Department_Name').val('');
        $('#Department_Description').val('');
        var modal = new bootstrap.Modal(document.getElementById('departmentModal'));
        modal.show();
        $('#departmentSaveBtn').off('click').on('click', async function () {
            var model = { Name: $('#Department_Name').val(), Description: $('#Department_Description').val() };
            try {
                var response = await postJson('/SystemAdmin/CreateDepartment', model);
                if (response.success) {
                    modal.hide();
                    loadDepartments();
                    showSuccess('Department created successfully');
                }
                else {
                    showError('Failed to create department');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function openDepartmentEdit(id) {
        var items = await get('/SystemAdmin/GetDepartments');
        var item = items.find(x => x.id === id);
        if (!item) return;
        $('#Department_Id').val(item.id);
        $('#Department_Name').val(item.name);
        $('#Department_Description').val(item.description);
        var modal = new bootstrap.Modal(document.getElementById('departmentModal'));
        modal.show();
        $('#departmentSaveBtn').off('click').on('click', async function () {
            var model = { Id: item.id, Name: $('#Department_Name').val(), Description: $('#Department_Description').val() };
            try {
                var response = await postJson('/SystemAdmin/UpdateDepartment', model);
                if (response.success) {
                    modal.hide();
                    loadDepartments();
                    showSuccess('Department updated successfully');
                }
                else {
                    showError('Failed to update department');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deleteDepartment(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {
                var response = await postJson('/SystemAdmin/DeleteDepartment?id=' + id, {});
                if (response.success) {
                    loadDepartments();
                    showSuccess('Department deleted successfully');
                }
                else {
                    showError('Failed to delete department');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        }
    }
    // Symptoms
    function openSymptomCreate() {
        $('#Symptom_Id').val('');
        $('#Symptom_Name').val('');
        $('#Symptom_Weight').val('');
        var modal = new bootstrap.Modal(document.getElementById('symptomModal'));
        modal.show();
        $('#symptomSaveBtn').off('click').on('click', async function () {
            var model = { Name: $('#Symptom_Name').val(), Weight: $('#Symptom_Weight').val() };
            try {
                var response = await postJson('/SystemAdmin/CreateSymptom', model);
                if (response.success) {
                    modal.hide();
                    loadSymptoms();
                    showSuccess('Symptom created successfully');
                }
                else {
                    showError('Failed to create symptom');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function openSymptomEdit(id) {
        var items = await get('/SystemAdmin/GetSymptoms');
        var item = items.find(x => x.id === id);
        if (!item) return;
        $('#Symptom_Id').val(item.id);
        $('#Symptom_Name').val(item.name);
        $('#Symptom_Weight').val(item.weight);
        var modal = new bootstrap.Modal(document.getElementById('symptomModal'));
        modal.show();
        $('#symptomSaveBtn').off('click').on('click', async function () {
            var model = { Id: item.id, Name: $('#Symptom_Name').val(), Weight: $('#Symptom_Weight').val() };
            try {
                var response = await postJson('/SystemAdmin/UpdateSymptom', model);
                if (response.success) {
                    modal.hide();
                    loadSymptoms();
                    showSuccess('Symptom updated successfully');
                }
                else {
                    showError('Failed to update symptom');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deleteSymptom(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {
                var response = await postJson('/SystemAdmin/DeleteSymptom?id=' + id, {});
                if (response.success) {
                    loadSymptoms();
                    showSuccess('Symptom deleted successfully');
                }
                else {
                    showError('Failed to delete symptom');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        }
    }
    // Priorities
    function openPriorityCreate() {
        $('#Priority_Id').val('');
        $('#Priority_Name').val('');
        $('#Priority_PriorityLevel').val(0);
        $('#Priority_Color').val('');
        var modal = new bootstrap.Modal(document.getElementById('priorityModal'));
        modal.show();
        $('#prioritySaveBtn').off('click').on('click', async function () {
            var model = { Name: $('#Priority_Name').val(), PriorityLevel: parseInt($('#Priority_PriorityLevel').val() || '0'), Color: $('#Priority_Color').val() };
            try {
                var response = await postJson('/SystemAdmin/CreatePriority', model);
                if (response.success) {
                    modal.hide();
                    loadPriorities();
                    showSuccess('Priority created successfully');
                }
                else {
                    showError('Failed to create priority');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function openPriorityEdit(id) {
        var items = await get('/SystemAdmin/GetPriorities');
        var item = items.find(x => x.id === id);
        if (!item) return;
        $('#Priority_Id').val(item.id);
        $('#Priority_Name').val(item.name);
        $('#Priority_PriorityLevel').val(item.priorityLevel);
        $('#Priority_Color').val(item.color || '');
        var modal = new bootstrap.Modal(document.getElementById('priorityModal'));
        modal.show();
        $('#prioritySaveBtn').off('click').on('click', async function () {
            var model = { Id: item.id, Name: $('#Priority_Name').val(), PriorityLevel: parseInt($('#Priority_PriorityLevel').val() || '0'), Color: $('#Priority_Color').val() };
            try {
                var response = await postJson('/SystemAdmin/UpdatePriority', model);
                if (response.success) {
                    modal.hide();
                    loadPriorities();
                    showSuccess('Priority updated successfully');
                }
                else {
                    showError('Failed to update priority');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deletePriority(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {
                var response = await postJson('/SystemAdmin/DeletePriority?id=' + id, {});
                if (response.success) {
                    loadPriorities();
                    showSuccess('Priority deleted successfully');
                }
                else {
                    showError('Failed to delete priority');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        }
    }
    // Token statuses
    function openTokenStatusCreate() {
        $('#TokenStatus_Id').val('');
        $('#TokenStatus_Name').val('');
        $('#TokenStatus_Description').val('');
        var modal = new bootstrap.Modal(document.getElementById('tokenStatusModal'));
        modal.show();
        $('#tokenStatusSaveBtn').off('click').on('click', async function () {
            var model = { Name: $('#TokenStatus_Name').val(), Description: $('#TokenStatus_Description').val() };
            try {
                var response = await postJson('/SystemAdmin/CreateTokenStatus', model);
                if (response.success) {
                    modal.hide();
                    loadTokenStatuses();
                    showSuccess('Token status created successfully');
                }
                else {
                    showError('Failed to create token status');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function openTokenStatusEdit(id) {
        var items = await get('/SystemAdmin/GetTokenStatuses');
        var item = items.find(x => x.id === id);
        if (!item) return;
        $('#TokenStatus_Id').val(item.id);
        $('#TokenStatus_Name').val(item.name);
        $('#TokenStatus_Description').val(item.description);
        var modal = new bootstrap.Modal(document.getElementById('tokenStatusModal'));
        modal.show();
        $('#tokenStatusSaveBtn').off('click').on('click', async function () {
            var model = { Id: item.id, Name: $('#TokenStatus_Name').val(), Description: $('#TokenStatus_Description').val() };
            try {
                var response = await postJson('/SystemAdmin/UpdateTokenStatus', model);
                if (response.success) {
                    modal.hide();
                    loadTokenStatuses();
                    showSuccess('Token status updated successfully');
                }
                else {
                    showError('Failed to update token status');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deleteTokenStatus(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {
                var response = await postJson('/SystemAdmin/DeleteTokenStatus?id=' + id, {});
                if (response.success) {
                    loadTokenStatuses();
                    showSuccess('Token status deleted successfully');
                }
                else {
                    showError('Failed to delete token status');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        }
    }


    

    function showSuccess(message) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: message,
            timer: 2000,
            showConfirmButton: false
        });
    }
    function showError(message) {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: message
        });
    }
    async function confirmDelete() {
        return await Swal.fire({
            title: 'Are you sure?',
            text: 'You will not be able to recover this record!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!'
        });
    }

    // Doctors
    async function loadDoctors() {
        var items = await get('/SystemAdmin/GetDoctors');

        var html = `
    <table class="table" id="tblDoctors">
        <thead>
            <tr>
                <th>Name</th>
                <th>Department</th>
                <th>Contact</th>
                <th></th>
            </tr>
        </thead>
        <tbody>`;

        items.forEach(i => {
            html += `
        <tr>
            <td>${i.name}</td>
            <td>${i.departmentName || ''}</td>
            <td>${i.contactNumber || ''}</td>
            <td>
                <button class="btn btn-sm btn-secondary"
                        onclick="sa.openDoctorEdit(${i.id})">
                    Edit
                </button>

                <button class="btn btn-sm btn-danger"
                        onclick="sa.deleteDoctor(${i.id})">
                    Delete
                </button>
            </td>
        </tr>`;
        });

        html += '</tbody></table>';

        document.getElementById('doctorsList').innerHTML = html;
        smartClinicDataTable('#tblDoctors', {
            language: { searchPlaceholder: 'Search doctors...', emptyTable: 'No doctors found' }
        });
    }
    function openDoctorCreate() {
        $('#Doctor_Id').val('');
        $('#Doctor_FullName').val('');
        $('#Doctor_DepartmentId').empty();
        $('#Doctor_ContactNumber').val('');
        get('/SystemAdmin/GetDepartments').then(r => { r.forEach(x => $('#Doctor_DepartmentId').append(`<option value="${x.id}">${x.name}</option>`)); });
        var modal = new bootstrap.Modal(document.getElementById('doctorModal'));
        modal.show();
        $('#doctorSaveBtn').off('click').on('click', async function () {
            var model = {
                Name: $('#Doctor_FullName').val(),
                DepartmentId: parseInt($('#Doctor_DepartmentId').val() || '0'),
                ContactNumber: $('#Doctor_ContactNumber').val()
            };
            try {
                var response = await postJson('/SystemAdmin/CreateDoctor', model);
                if (response.success) {
                    modal.hide();
                    loadDoctors();
                    showSuccess('Doctor created successfully');
                }
                else {
                    showError('Failed to create doctor');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function openDoctorEdit(id) {
        var items = await get('/SystemAdmin/GetDoctors');
        var item = items.find(x => x.id === id);
        if (!item) return;
        $('#Doctor_Id').val(item.id);
        $('#Doctor_FullName').val(item.name);
        $('#Doctor_DepartmentId').empty();
        get('/SystemAdmin/GetDepartments').then(r => { r.forEach(x => $('#Doctor_DepartmentId').append(`<option value="${x.id}" ${x.id == item.departmentId ? 'selected' : ''}>${x.name}</option>`)); });
        $('#Doctor_ContactNumber').val(item.contactNumber || '');
        var modal = new bootstrap.Modal(document.getElementById('doctorModal'));
        modal.show();
        $('#doctorSaveBtn').off('click').on('click', async function () {
            var model = {
                Id: item.id,
                Name: $('#Doctor_FullName').val(),
                DepartmentId: parseInt($('#Doctor_DepartmentId').val() || '0'),
                ContactNumber: $('#Doctor_ContactNumber').val()
            };
            try {
                var response = await postJson('/SystemAdmin/UpdateDoctor', model);
                if (response.success) {
                    modal.hide();
                    loadDoctors();
                    showSuccess('Doctor updated successfully');
                }
                else {
                    showError('Failed to update doctor');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        });
    }
    async function deleteDoctor(id) {
        const result = await confirmDelete();
        if (result.isConfirmed) {
            try {
                var response = await postJson('/SystemAdmin/DeleteDoctor?id=' + id, {});
                if (response.success) {
                    loadDoctors();
                    showSuccess('Doctor deleted successfully');
                }
                else {
                    showError('Failed to delete doctor');
                }
            }
            catch (e) {
                showError('Something went wrong');
            }
        }
    }
    return {
        loadAll: function () { loadRoles(); loadUsers(); loadDepartments(); loadSymptoms(); loadPriorities(); loadTokenStatuses(); },
        // roles
        loadRoles: loadRoles,
        openRoleCreate: openRoleCreate,
        openRoleEdit: openRoleEdit,
        deleteRole: deleteRole,
        // users
        loadUsers: loadUsers,
        openUserCreate: openUserCreate,
        openUserEdit: openUserEdit,
        deleteUser: deleteUser,
        // departments
        loadDepartments: loadDepartments,
        openDepartmentCreate: openDepartmentCreate,
        openDepartmentEdit: openDepartmentEdit,
        deleteDepartment: deleteDepartment,
        // doctors
        // loadDoctors: loadDoctors,
        // openDoctorCreate: openDoctorCreate,
        // openDoctorEdit: openDoctorEdit,
        // deleteDoctor: deleteDoctor,
        // symptoms
        loadSymptoms: loadSymptoms,
        openSymptomCreate: openSymptomCreate,
        openSymptomEdit: openSymptomEdit,
        deleteSymptom: deleteSymptom,
        // priorities
        loadPriorities: loadPriorities,
        openPriorityCreate: openPriorityCreate,
        openPriorityEdit: openPriorityEdit,
        deletePriority: deletePriority,
        // token statuses
        loadTokenStatuses: loadTokenStatuses,
        openTokenStatusCreate: openTokenStatusCreate,
        openTokenStatusEdit: openTokenStatusEdit,
        deleteTokenStatus: deleteTokenStatus
    };
})();
