document.addEventListener('DOMContentLoaded', function () {
    var plusBtns = document.querySelectorAll('.plus');
    var minusBtns = document.querySelectorAll('.minus');
    var quantityInputs = document.querySelectorAll('.quantity-input');

    async function updateQuantity(url, id) {
        try {
            var response = await fetch(url, {
                method: 'PATCH'
            });

            if (response.ok) {
                var data = await response.json();

                if (data.success) {
                    var quantityElement = document.querySelector('.quantity-input[data-productId="' + id + '"]');
                    quantityElement.value = data.quantity;

                    var subTotalElement = document.querySelector('#subTotal[data-productId="' + id + '"]');
                    subTotalElement.innerText = '$' + data.subTotal.toFixed(2);

                    var cartTotalElement = document.querySelector('#cart-total');
                    cartTotalElement.innerText = '$' + data.total.toFixed(2);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message || 'Failed to update quantity'
                    });
                }
            } else {
                throw new Error('Network response was not ok');
            }

        } catch (error) {
            console.error('Error:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'An error occurred while updating the quantity'
            });
        }
    }

    plusBtns.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var id = btn.getAttribute('data-productId');
            updateQuantity(`/cart-item/plus/${id}`, id);
        });
    });

    minusBtns.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var id = btn.getAttribute('data-productId');
            var quantityElement = document.querySelector('.quantity-input[data-productId="' + id + '"]');
            var currentQuantity = parseInt(quantityElement.value);

            if (currentQuantity <= 1) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Invalid Quantity',
                    text: 'Quantity cannot be less than 1'
                });
                return;
            }

            updateQuantity(`/cart-item/minus/${id}`, id);
        });
    });

    // Add input validation for direct quantity changes
    quantityInputs.forEach(function (input) {
        input.addEventListener('change', function () {
            var quantity = parseInt(this.value);
            if (quantity < 1) {
                this.value = 1;
                Swal.fire({
                    icon: 'warning',
                    title: 'Invalid Quantity',
                    text: 'Quantity cannot be less than 1'
                });
            }
        });
    });
});