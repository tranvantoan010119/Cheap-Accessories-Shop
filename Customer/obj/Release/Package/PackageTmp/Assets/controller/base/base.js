(function (window,$) {
    var common = {
        customerId: $('input#customerId').val()
    }
    window.base = {
        init: function () {
            base.regisControl();
            base.getCartItemNo();
        },
        regisControl: function () {
            $('.btn-cart').off('click').on('click', function (e) {
                e.preventDefault();
                if (parseInt($(this).data("stock")) <= 0)
                {
                    toastr["error"]("Tạm thời hết hàng.");
                }
                else
                {
                    var productId = $(this).data('productid');
                    base.addToCart(productId, null);
                }
            });
            $('div#cart').off('click').on('click', function () {
                location.href = '/Cart/Index'
            });
            $('.btn-search').off('click').on('click', function (e) {
                e.preventDefault();
                var keyword = $('#txtSearch').val();
                var url = '/Product/Search?keyword=' + keyword + '';
                location.href = url;
            })
            $('#btn-logout').off('click').on('click', function (e) {
                e.preventDefault();
                location.href = '/Account/Logout';
            });
            $('#btn-transaction-history').off('click').on('click', function (e) {
                e.preventDefault();
                location.href = '/Account/TransactionHistory';
            });
            $('#button-cart').off('click').on('click', function (e) {
                e.preventDefault();
                var quantityStock = parseInt($(this).data("stock"));
                var quantity = $('#input-quantity').val();
                if (quantityStock <= 0) {
                    toastr["error"]("Tạm thời hết hàng.");
                }
                else if(quantityStock < quantity)
                {
                    toastr["error"]("Vượt quá số lượng trong kho.");
                }
                else {
                    var productId = $(this).data('productid');
                    if (quantity <= 0) {
                        toastr["error"]("Số lượng phải lớn hơn 0.");
                        $('#input-quantity').val(1);
                        $('#input-quantity').focus();
                    }
                    else {
                        base.addToCart(productId, quantity);
                    }
                }
                
            });
        },
        addToCart: function (productId,quantity) {
            if (quantity == null)
                quantity = 1;
            $.ajax({
                url: '/Cart/addToCart',
                type: 'post',
                data: { productId: productId, quantity: quantity },
                success: function (res) {
                    if(res.status)
                    {
                        toastr["success"]('Thêm vào giỏ hàng thành công.');
                        $('span#cart-total').text('Giỏ hàng (' + res.totalItem + ')');
                        localStorage.setItem("cart", res.data);
                    }
                }
            });
        },
        getCartItemNo: function () {
            var localCart = localStorage.getItem("cart");
            if (localCart != null)
            {
                $.ajax({
                    url: '/Cart/getCartItemNo',
                    data: { localCart: localCart },
                    type: 'post',
                    success: function (res) {
                        if(res.status)
                        {
                            base.setNumberItemInCart(res.totalItem);
                        }
                    }
                });
            }
        },
        setNumberItemInCart: function (count) {
            if(count > 0)
            {
                $('span#cart-total').text('Giỏ hàng (' + count + ')');
            }
            else
            {
                $('span#cart-total').text('Giỏ hàng');
            }
        },
        startLoading: function () {
            $('#imgLoading').removeClass("hide");
            $('#bt_container').css("opacity", "0.4");
            $('#bt_container').css("pointer-events", "none");
        },
        stopLoading: function(){
            $('#imgLoading').addClass("hide");
            $('#bt_container').css("opacity", "unset");
            $('#bt_container').css("pointer-events", "unset");
        },
        formatTextboxCurrency: function (nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1))
                x1 = x1.replace(rgx, '$1' + '.' + '$2');
            return x1 + x2;
        }
    }
})(window, jQuery);

$(document).ready(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "300",
        "timeOut": "2500",
        "extendedTimeOut": "300",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    base.init();
});