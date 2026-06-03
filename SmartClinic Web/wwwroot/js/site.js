// ==========================================
// SMART CLINIC - GLOBAL DATATABLE HELPER
// Single reusable configuration for all DataTables
// ==========================================

/**
 * Initializes a DataTable with standardized SmartClinic defaults.
 * 
 * @param {string|jQuery} tableSelector - CSS selector or jQuery object for the table
 * @param {object} customOptions - Optional overrides for this specific table (columnDefs, order, language, etc.)
 * @returns {object} The initialized DataTable instance
 * 
 * Standard defaults enforced:
 *   pageLength: 5
 *   responsive: true
 *   autoWidth: false
 *   ordering: true
 *   searching: true
 *   paging: true
 *   lengthChange: false
 */
function smartClinicDataTable(tableSelector, customOptions) {

    var defaults = {
        responsive: true,
        autoWidth: false,
        ordering: true,
        searching: true,
        paging: true,
        lengthChange: false,
        pageLength: 5,
        destroy: true,
        language: {
            search: '',
            searchPlaceholder: 'Search...',
            emptyTable: 'No data available',
            info: 'Showing _START_ to _END_ of _TOTAL_ entries',
            infoEmpty: 'Showing 0 to 0 of 0 entries',
            infoFiltered: '(filtered from _MAX_ total entries)',
            paginate: {
                first: 'First',
                last: 'Last',
                next: 'Next',
                previous: 'Previous'
            }
        },
        // dom with full-width search (no length selector since lengthChange: false)
        dom: '<"row"<"col-sm-12"f>>' +
             '<"row"<"col-sm-12"tr>>' +
             '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        // Standard draw callback — handles Sl.No (column 0) if present
        drawCallback: function () {
            // Handle serial numbering for column 0 if it's marked as a serial number column
            var api = this.api();
            if (api.columns(0).header().length) {
                var pageInfo = api.page.info();
                api.column(0, { page: 'current' }).nodes().each(function (cell, index) {
                    cell.innerHTML = pageInfo.start + index + 1;
                });
            }
        }
    };

    // Merge custom options into defaults
    var options = $.extend(true, {}, defaults, customOptions);

    // Destroy existing instance if present
    if ($.fn.DataTable.isDataTable(tableSelector)) {
        $(tableSelector).DataTable().destroy();
    }

    // Initialize and return
    return $(tableSelector).DataTable(options);
}
