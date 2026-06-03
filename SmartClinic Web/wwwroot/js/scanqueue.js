$(document).ready(function () {
    loadScanQueue();
});
var currentCalledPatient = null;
function loadScanQueue() {
    $.ajax({
        url: '/Scan/GetScanQueue',
        type: 'GET',
        success: function (response) {
            if (typeof response === 'string') {
                response = JSON.parse(response);
            }
            // Waiting Queue
            $('#waitingQueueList').empty();
            if (response.data.waitingScans && response.data.waitingScans.length > 0) {
                $.each(response.data.waitingScans, function (i, item) {
                    $('#waitingQueueList').append(`
                        <div class="queue-item">
                            <strong>${item.tokenNumber}</strong><br />
                            ${item.patientName}<br />
                            <small>${item.scanTypes}</small>
                        </div>
                    `);
                });
            }
            else {
                $('#waitingQueueList').html('<div class="text-muted">No waiting patients</div>');
            }
            // Hold Queue
            $('#holdQueueList').empty();
            if (response.data.holdScans && response.data.holdScans.length > 0) {
                $.each(response.data.holdScans, function (i, item) {
                    $('#holdQueueList').append(`
                        <div class="queue-item">
                            <strong>${item.tokenNumber}</strong><br />
                            ${item.patientName}<br />
                            <small>${item.scanTypes}</small>
                        </div>
                    `);
                });
            }
            else {
                $('#holdQueueList').html('<div class="text-muted">No hold patients</div>');
            }
            // Skipped Queue
            $('#skippedQueueList').empty();
            if (response.data.skippedScans && response.data.skippedScans.length > 0) {
                $.each(response.data.skippedScans, function (i, item) {
                    $('#skippedQueueList').append(`
                        <div class="queue-item">
                            <strong>${item.tokenNumber}</strong><br />
                            ${item.patientName}<br />
                            <small>${item.scanTypes}</small>
                        </div>
                    `);
                });
            }
            else {
                $('#skippedQueueList').html('<div class="text-muted">No skipped patients</div>');
            }
            // Next 3 Queue
            $('#nextQueueList').empty();
            if (response.data.next3Scans && response.data.next3Scans.length > 0) {
                $.each(response.data.next3Scans, function (i, item) {
                    $('#nextQueueList').append(`
                        <div class="queue-item">
                            <strong>${item.tokenNumber}</strong><br />
                            ${item.patientName}<br />
                            <small>${item.scanTypes}</small>
                        </div>
                    `);
                });
            }
            else {
                $('#nextQueueList').html('<div class="text-muted">No upcoming patients</div>');
            }
            if (currentCalledPatient != null) {

                $('#currentToken').text(currentCalledPatient.tokenNumber);
                $('#currentPatient').text(currentCalledPatient.patientName);
                $('#currentScan').text(currentCalledPatient.scanTypes);

                $('#currentTokenId').val(currentCalledPatient.tokenId);
            }
            else if (response.data.next3Scans?.length > 0) {

                var current = response.data.next3Scans[0];

                $('#currentToken').text(current.tokenNumber);
                $('#currentPatient').text(current.patientName);
                $('#currentScan').text(current.scanTypes);

                $('#currentTokenId').val(current.tokenId);
                $('#currentRequestId').val(current.id);
            }
            else {

                $('#currentToken').text('---');
                $('#currentPatient').text('No Patient');
                $('#currentScan').text('---');

                $('#currentTokenId').val('');
                $('#currentRequestId').val('');
            }
            // Counts
            $('#waitingCount').text(response.data.waitingScans?.length || 0);
            $('#holdCount').text(response.data.holdScans?.length || 0);
            $('#skipCount').text(response.data.skippedScans?.length || 0);

            $('#totalCount').text((response.data.waitingScans?.length || 0) + (response.data.holdScans?.length || 0) + (response.data.skippedScans?.length || 0));
        }
    });
}
function updateTokenStatus(action) {
    var tokenId = $('#currentTokenId').val();
    if (!tokenId) {
        Swal.fire('Info', 'No patient selected.', 'info');
        return;
    }
    $.ajax({
        url: '/Scan/UpdateScanTokenStatus',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            tokenId: parseInt(tokenId),
            action: action
        }),
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: response.message,
                    timer: 1200,
                    showConfirmButton: false
                });
                loadScanQueue();
            }
            else {
                Swal.fire('Error', response.message, 'error');
            }
        },
        error: function () {
            Swal.fire('Error', 'Failed to update status', 'error');
        }
    });
}
$('#btnCall').click(function () {
    currentCalledPatient = {
        tokenId: $('#currentTokenId').val(),
        tokenNumber: $('#currentToken').text(),
        patientName: $('#currentPatient').text(),
        scanTypes: $('#currentScan').text()
    };
    updateTokenStatus('CALL');
});
$('#btnEnd').click(function () {
    updateTokenStatus('END');
    currentCalledPatient = null;
});
$('#btnRecall').click(function () {
    updateTokenStatus('RECALL');
    currentCalledPatient = null;
});
$('#btnHold').click(function () {
    updateTokenStatus('HOLD');
    currentCalledPatient = null;
});
$('#btnSkip').click(function () {
    updateTokenStatus('SKIP');
    currentCalledPatient = null;
});
$('#btnNext').click(function () {
    updateTokenStatus('NEXT');
    currentCalledPatient = null;
});
$(document).on('click', '.btnCompleteScan',
    function () {
        var id = $(this).data('id');
        var tokenId = $(this).data('tokenid');
        Swal.fire({
            title: 'Complete Scan?',
            text: 'Patient will return to doctor.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes, Complete'
        }).then(
            function (result) {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Scan/CompleteScan',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            id: id,
                            tokenId: tokenId
                        }),
                        success: function (response) {
                            if (typeof response === 'string') {
                                response = JSON.parse(response);
                            }
                            if (response.success) {
                                Swal.fire({ icon: 'success', title: 'Completed', text: response.message });
                                loadScanQueue();
                            }
                            else {
                                Swal.fire({ icon: 'error', title: 'Error', text: response.message });
                            }
                        },
                        error: function () {
                            Swal.fire({ icon: 'error', title: 'Error', text: 'Failed to complete scan' });
                        }
                    });
                }
            });
    });
