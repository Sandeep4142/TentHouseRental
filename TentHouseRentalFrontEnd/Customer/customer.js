$(document).ready(function () {
    populateCustomerTable();
    
    $('#addCustomerForm').submit(function (e) {
        e.preventDefault();
        addCustomer();
    });
});

function populateCustomerTable() {
    $.ajax({
        url: 'http://localhost:5140/api/Customer',
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (data) {
            var customerTable = $('#customerTable').DataTable({"ordering": false}).clear().draw();
            $.each(data, function (index, customer) {
                customerTable.row.add([
                    //customer.customerId,
                    index+1,
                    customer.customerName,
                ]);        
            });
            customerTable.destroy();
            $('#customerTable').DataTable();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}



function addCustomer() {
    var customer = {
        customerName: $('#customerName').val()
    };
    $.ajax({
        url: 'http://localhost:5140/api/Customer',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(customer),
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (response) {
            if(response == false){
                alert("Customer with same name already exist .")
            }else{
                populateCustomerTable();
                $('#addCustomerModal').modal('hide');
                $('#addCustomerForm')[0].reset();
            }          
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}

