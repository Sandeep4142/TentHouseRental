var products = {};
var customers = {};

$(document).ready(function () {
    var productsPromise = fetchProducts();
    var customersPromise = fetchCustomers();

    // Use Promise.all() to wait for both promises to resolve
    Promise.all([productsPromise, customersPromise]).then(function() {
        populateTransactionTable();
    });

    $('#addTransactionForm').submit(function (event) {
        event.preventDefault();
        addTransaction();
    });

    $('#removeTransaction').click(function() {
        removeAllTransactions();
    });

});

function populateTransactionTable() {
    $.ajax({
        url: 'http://localhost:5140/api/Transaction',
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (data) {
            var transactionTable = $('#transactionTable').DataTable().clear().draw();
            $.each(data, function (index, transaction) {
                var productName = products[transaction.productId];
                var customerName = customers[transaction.customerId];
                var transactionType = transaction.transactionType == true ? 'IN' : 'OUT';
                var transactionDateTime = new Date(transaction.transactionDateTime).toLocaleString();

                transactionTable.row.add([
                    //transaction.transactionId,
                    index+1,
                    transactionDateTime, 
                    productName,
                    customerName,
                    transactionType,
                    transaction.quantity,
                ]).draw(false);
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    }).done(function(){
        // Initialize DataTable after populating data
        $('#transactionTable').DataTable();
    });
}


function addTransaction() {
    var formData = {
        transactionDateTime: new Date().toISOString(),
        productId: $('#productId').val(),
        customerId: $('#customerId').val(),
        transactionType: $('#transactionType').val() === 'true',
        quantity: $('#quantity').val()
    };

    $.ajax({
        url: 'http://localhost:5140/api/Transaction',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (response) {
            // Handle success response
            var success = response.success;
            var remainingQuantity = response.transactionId;

            if(success == true){
                $('#addTransactionModal').modal('hide');
                $('#addTransactionForm')[0].reset();
                $('#availableQuantity').text("");
                populateTransactionTable();
                alert("Transaction Successfull  !!");
                return;
            }else{
                if(formData.transactionType == true){
                    alert("Transaction Failed : IN Quantity is " + formData.quantity +", but remaining quantity to be return is " + remainingQuantity);
                    return;
                }else{
                    alert("Transaction Failed : First Return the remaing quantity - " + remainingQuantity +" of "+ $('#productId option:selected').text() +" before making new transaction .");
                    return;
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', xhr);
        }
    });
}

function fetchProducts() {
    return new Promise(function(resolve, reject) {
        $.ajax({
            url: 'http://localhost:5140/api/Product',
            type: 'GET',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
            },
            success: function(data) {
                console.log(data)
                // Populate product dropdown
                var dropdown = $('#productId');
                dropdown.empty();
                dropdown.append('<option value="">-- Select Product --</option>');
                $.each(data, function(key, product) {
                    products[product.productId] = product.productTitle;
                    dropdown.append($('<option></option>').attr('value', product.productId).text(product.productTitle));
                });

                // Add event listener to update available quantity
                dropdown.on('change', function() {
                    var selectedProductId = $(this).val();
                    if (selectedProductId) {
                        var selectedProduct = data.find(function(product) {
                            return product.productId == selectedProductId;
                        });
                        if (selectedProduct) {
                            var availableQuantity = selectedProduct.quantityTotal;
                            $('#availableQuantity').text('Available Quantity: ' + availableQuantity);
                            $('#quantity').attr('max', availableQuantity);
                        }
                    } else {
                        $('#availableQuantity').text(''); 
                    }
                });

                resolve(); 
            },
            error: function(xhr, textStatus, errorThrown) {
                console.log('Error fetching products:', errorThrown);
                reject(errorThrown); 
            }
        });
    });
}

function fetchCustomers() {
    return new Promise(function(resolve, reject) {
        $.ajax({
            url: 'http://localhost:5140/api/Customer',
            type: 'GET',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
            },
            success: function(data) {
                console.log(data)
                var dropdown = $('#customerId');
                dropdown.empty();
                dropdown.append('<option value="">-- Select Customer --</option>');
                $.each(data, function(key, customer) {
                    customers[customer.customerId] = customer.customerName;
                    dropdown.append($('<option></option>').attr('value', customer.customerId).text(customer.customerName));
                });
                resolve(); 
            },
            error: function(xhr, textStatus, errorThrown) {
                console.log('Error fetching products:', errorThrown);
                reject(errorThrown); 
            }
        });
    });
}

function removeAllTransactions() {
    $.ajax({
        url: 'http://localhost:5140/api/Transaction',
        type: 'DELETE', 
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function(response) {
            populateTransactionTable();
            alert('All transactions removed successfully.');
        },
        error: function(xhr, status, error) {
            console.error('Error occurred while removing transactions:', error);
        }
    });
}
