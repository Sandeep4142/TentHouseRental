$(document).ready(function() {
    loadDetailedReport();

    $('#reportDate').on('change', function() {
        var selectedDate = $(this).val();
        getInventoryReportByDate(selectedDate);
    });

    $('#reportMonth').on('change', function() {
        var selectedMonth = $(this).val();
        getInventoryReportByMonth(selectedMonth);
    });
     $("#downloadPDFButton").click(exportToPDF);
    
});

function loadDetailedReport() {
    $.ajax({
        url: 'http://localhost:5140/api/Reports/InventoryDetailedReport', 
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function(response) {
            displayReport(response);
        },
        error: function(xhr, status, error) {
            if (xhr.status === 401) {
                // Redirect to unauthorized page
                window.location.href = '/unauthorized.html';
            }
            console.error('Error occurred while fetching the detailed report:', error);
        }
    });
} 

function displayReport(reportData) {
    var reportContainer = $('#reportContainer');
    reportContainer.empty();
    reportData.forEach(function(inventoryItem) {
        var i = inventoryItem;
        var j = inventoryItem.Product
        console.log('Inventory - '+ inventoryItem)
        var itemHtml = '<div class="inventory-item">';

        itemHtml += '<div class="card mb-3 product-section inventory-item">';
        itemHtml += '<div class="card-header">';
        itemHtml += 'Item Name: ' + inventoryItem.itemName;
        itemHtml += '<br />';
        itemHtml += 'Available Quantity: ' + (inventoryItem.availableQuantity)
        itemHtml += '</div>';

        if (inventoryItem.transactions.length > 0) {
            itemHtml += '<div class="card-body">';
            itemHtml += '<table class="table table-bordered">';
            itemHtml += '<thead class="thead-dark">';
            itemHtml += '<tr>';
            itemHtml += '<th>Transaction ID</th>';
            itemHtml += '<th>Date</th>';
            itemHtml += '<th>Type</th>';
            itemHtml += '<th>Quantity</th>';
            itemHtml += '</tr>';
            itemHtml += '</thead>';
            itemHtml += '<tbody>';

            inventoryItem.transactions.forEach(function(transaction) {
                var transactionType = transaction.transactionType == true ? 'IN' : 'OUT';
                var transactionDateTime = new Date(transaction.transactionDateTime).toLocaleString();

                itemHtml += '<tr>';
                itemHtml += '<td>' + transaction.transactionId + '</td>';
                itemHtml += '<td>' + transactionDateTime + '</td>';
                itemHtml += '<td>' + transactionType + '</td>';
                itemHtml += '<td>' + transaction.quantity + '</td>';
                itemHtml += '</tr>';
            });
            itemHtml += '</tbody>';
            itemHtml += '</table>';
            itemHtml += '</div>'; // Close card-body div
        }
        itemHtml += '</div>'; 
        itemHtml += '</div>'; 

        reportContainer.append(itemHtml);
    });
}

function getInventoryReportByDate(date) {
    $.ajax({
        url: 'http://localhost:5140/api/Reports/InventoryDetailedReportByDate/'+date,
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function(response) {
            displayReport(response);
        },
        error: function(xhr, status, error) {
            if (xhr.status === 401) {
                window.location.href = '/unauthorized.html';
            }
            console.error('Error occurred while fetching the detailed report:', error);
        }
    });
}

function getInventoryReportByMonth(month) {
    $.ajax({
        url: 'http://localhost:5140/api/Reports/InventoryDetailedReportByMonth/'+month,
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function(response) {
            displayReport(response);
        },
        error: function(xhr, status, error) {
            if (xhr.status === 401) {
                window.location.href = '/unauthorized.html';
            }
            console.error('Error occurred while fetching the detailed report:', error);
        }
    });
}

function exportToPDF() {
    var content = $('#reportContainer').html();
    const pdf = new jsPDF();
    pdf.fromHTML(content, 15, 20);

    const currentDate = new Date();
    const formattedDate = currentDate.toDateString();
    const formattedTime = currentDate.toLocaleTimeString();
    const formattedDateTime = formattedDate + " " + formattedTime;

    const fileName = `Inventory-Report-${formattedDateTime}.pdf`;
    pdf.save(fileName);
}
