// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

 
        $(document).ready(function () {
            // Load products via AJAX
            loadProducts();

            // Add product button click
            $('#btnAddProduct').click(function () {
                $('#productForm')[0].reset();
                $('#nameError').hide();
                $('#priceError').hide();
                $('#productModalLabel').text('Add Product');
                $('#saveProductBtn').show();
                $('#confirmDeleteBtn').hide();
            });

            // Edit product button click
            $(document).on('click', '.editProductBtn', function () {
                var row = $(this).closest('tr');
                var productId = row.data('id');
                var productName = row.find('td').eq(0).text();
                var productPrice = row.find('td').eq(1).text();

                $('#ProductName').val(productName);
                $('#ProductPrice').val(productPrice);
                $('#productModalLabel').text('Edit Product');
                $('#saveProductBtn').data('id', productId);
                $('#saveProductBtn').show();
                $('#confirmDeleteBtn').hide();
                $('#productModal').modal('show');
            });

            // Save product (Add/Update)
            $('#saveProductBtn').click(function () {
                var productData = {
                    Name: $('#ProductName').val(),
                    Price: $('#ProductPrice').val()
                };

                var isValid = true;

                // Validation
                if (!productData.Name) {
                    $('#nameError').text('Product name is required').show();
                    isValid = false;
                }

                if (!productData.Price || productData.Price <= 0) {
                    $('#priceError').text('Price must be a positive number').show();
                    isValid = false;
                }

                if (!isValid) {
                    return;
                }

                var url = '/Products/AddProduct';
                if ($(this).data('id')) {
                    productData.ProductId = $(this).data('id');
                    url = '/Products/UpdateProduct';
                }

                $.ajax({
                    url: url,
                    method: 'POST',
                    data: productData,
                    success: function (response) {
                        if (response.success) {
                            loadProducts();
                            $('#productModal').modal('hide');
                        } else {
                            alert('Error: ' + response.errors.join(', '));
                        }
                    }
                });
            });

            // Delete product button click
            $(document).on('click', '.deleteProductBtn', function () {
                var productId = $(this).closest('tr').data('id');
                $('#confirmDeleteBtn').data('id', productId);
                $('#confirmDeleteModal').modal('show');
            });

            // Confirm Delete action
            $('#confirmDeleteBtn').click(function () {
                var productId = $(this).data('id');
                $.ajax({
                    url: '/Products/DeleteProduct',
                    method: 'POST',
                    data: { id: productId },
                    success: function () {
                        loadProducts();
                        $('#confirmDeleteModal').modal('hide');
                    }
                });
            });

            // Load products via AJAX
            function loadProducts() {
                $.ajax({
                    url: '/Products/GetProducts',
                    method: 'GET',
                    success: function (data) {
                        $('#productTable').empty();
                        $.each(data, function (index, product) {
                            var row = `<tr data-id="${product.productId}">
                                           <td>${product.name}</td>
                                           <td>${product.price}</td>
                                           <td>
                                               <button class="btn btn-warning editProductBtn">Edit</button>
                                               <button class="btn btn-danger deleteProductBtn">Delete</button>
                                           </td>
                                       </tr>`;
                            $('#productTable').append(row);
                        });
                    }
                });
            }
        });
  
 
