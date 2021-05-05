(function (window,$) {
    window.cart = {
        init: function () {
            cart.regisControl();
        },
        regisControl: function () {
            $('.btn-update').off('click').on('click', function (e) {
                e.preventDefault();
                var btn = $(this);
                var productId = btn.data("productid");
                var quantity = $('#txtQuantity_'+productId+'').val();
                if (quantity <= 0)
                    toastr["error"]("Số lượng phải từ 0 trở lên.");
                else
                    cart.updateQuantity(productId, quantity);
            });
            $('.btn-remove').off('click').on('click', function (e) {
                e.preventDefault();
                var btn = $(this);
                var productId = btn.data("productid");
                cart.deleteItem(productId);
            });
        },
        updateQuantity: function (productId,quantity) {
            $.ajax({
                url: '/Cart/updateQuantity',
                type: 'post',
                data: { productId: productId, quantity: quantity },
                success: function (res) {
                    if (res.status == 1) {
                        toastr["error"]("Có lỗi xảy ra, xin vui lòng thử lại.");
                        location.reload();
                    }
                    else if (res.status == 2) {
                        toastr["error"]("Số lượng phải từ 0 trở lên.");
                    }
                    else if (res.status == 3) {
                        toastr["error"]("Vượt quá số lượng trong kho.");
                    }
                    else {
                        localStorage.setItem("cart", res.data);
                        $('#content').html('');
                        $('#content').html(res.html);
                        cart.regisControl();
                    }
                }

            });
        },
        deleteItem: function (productId) {
            $.ajax({
                url: '/Cart/deleteItem',
                type: 'post',
                data: { productId: productId },
                success: function (res) {
                    if (!res.status)
                    {
                        toastr["error"]("Có lỗi xảy ra, xin vui lòng thử lại");
                        location.reload();
                    }
                    else
                    {
                        localStorage.setItem("cart", res.data);
                        $('#content').html('');
                        $('#content').html(res.html);
                        base.setNumberItemInCart(res.totalRow);
                        cart.regisControl();
                    }
                }
            });
        }
    }
})(window, jQuery);

$(document).ready(function () {
    cart.init();
});