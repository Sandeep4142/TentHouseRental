$(document).ready(function () {
    populateProductTable();

    $('#addProductForm').submit(function (e) {
        e.preventDefault();
        addProduct();
    });

    $('#updateProductForm').submit(function (e) {
        e.preventDefault();
        updateProduct();
    });
});

function populateProductTable() {
    $.ajax({
        url: 'http://localhost:5140/api/Product',
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (data) {
            console.log(data);
            var productTable = $('#productTable').DataTable().clear().draw();
            $.each(data, function (index, product) {
                productTable.row.add([
                    //product.productId,
                    index+1,
                    product.productTitle,
                    product.quantityTotal,
                    product.quantityBooked,
                    product.price,
                    '<button type="button" class="btn btn-primary btn-sm" onclick="openUpdateModal(' + product.productId + ')">Update</button>'
                ]).draw(false);
            });
            productTable.destroy();
            $('#productTable').DataTable();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}

function addProduct() {
    var product = {
        productTitle: $('#productTitle').val(),
        quantityTotal: $('#quantityTotal').val(),
        quantityBooked: $('#quantityBooked').val(),
        price: $('#price').val()
    };
    $.ajax({
        url: 'http://localhost:5140/api/Product',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(product),
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (response) {
            if(response == false){
                alert("Product already exist");
            }
            else{
                populateProductTable();
                $('#addProductModal').modal('hide');
                $('#addProductForm')[0].reset();
            }    
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}

function openUpdateModal(productId) {
    $.ajax({
        url: 'http://localhost:5140/api/Product/' + productId,
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (data) {
            $('#updateProductId').val(data.productId);
            $('#updateProductTitle').val(data.productTitle);
            $('#updateQuantityTotal').val(data.quantityTotal);
            $('#updateQuantityBooked').val(data.quantityBooked);
            $('#updatePrice').val(data.price);
            $('#updateProductModal').modal('show');
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}

function updateProduct() {
    var product = {
        productId: $('#updateProductId').val(),
        productTitle: $('#updateProductTitle').val(),
        quantityTotal: $('#updateQuantityTotal').val(),
        quantityBooked: $('#updateQuantityBooked').val(),
        price: $('#updatePrice').val()
    };
    $.ajax({
        url: 'http://localhost:5140/api/Product/' + product.productId,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(product),
        headers: {
            'Authorization': 'Bearer ' + (localStorage.getItem('jwtToken') || null)
        },
        success: function (data) {
            populateProductTable();
            $('#updateProductModal').modal('hide');
            $('#updateProductForm')[0].reset();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}
