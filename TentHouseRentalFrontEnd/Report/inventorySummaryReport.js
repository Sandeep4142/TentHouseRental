$(document).ready(function () {
    populateSummaryTable();
    $("#downloadPDFButton").click(exportToPDF);
});

function populateSummaryTable() {
    $.ajax({
        url: 'http://localhost:5140/api/Reports/InventorySummaryReport',
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (data) {
            $('#summaryReportTable').DataTable().clear().draw();
            $.each(data, function (index, product) {
                $('#summaryReportTable').DataTable().row.add([
                    product.productId,
                    product.productTitle,
                    product.quantityTotal,                   
                ]).draw(false);
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 401) {
                // Redirect to unauthorized page
                window.location.href = '/unauthorized.html';
            }
            console.log('Error:', errorThrown);
        }
    });
}

function exportToPDF() {
    var doc = new jsPDF();
    var table = document.getElementById("summaryReportTable");
    var currentDate = new Date().toLocaleDateString();

    var heading = "<div style='font-size: 18px; font-weight: bold;'>Appointment Detailed Report</div><br>"+
    "<div>Date  : " + currentDate + "</div><br>";
    doc.fromHTML(heading, 15, 10);

    doc.autoTable({
        html: table,
        startY: 30
    });
    doc.save("inventory_Summary_report.pdf");
}

