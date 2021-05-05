var config = {
    pageIndex: 1,
    pageSize: 12
};

(function (window, $) {
    window.order = {
        init: function () {
            order.loadOrders();
        },
        regisControl: function () {
            $('.optStatus').off('change').on('change', function (e) {
                var btn = $(this);
                var orderId = btn.data("id");
                var statusId = btn.val();
                
                $.ajax({
                    url: _mainConfig.relativePath + '/Order/ChangeStatus',
                    data: { orderId: orderId, statusId: statusId },
                    type: 'post',
                    success: function (res) {
                        if (res.status) {
                            toastr["success"]("Cập nhật thành công.");
                            if (statusId > 3)
                                btn.attr("disabled", true);
                        }
                        else {
                            toastr["error"]("Có lỗi xảy ra, vui lòng thử lại.");
                            setTimeout(function(){ location.reload() }, 2500);
                        }
                    }
                });
            });
            $('.linkOrderDetail').off('click').on('click', function (e) {
                e.preventDefault();
                var orderId = $(this).data("id");
                var email = $(this).data("email");
                $.ajax({
                    url: _mainConfig.relativePath + '/Order/ViewOrder',
                    type: 'get',
                    data: { orderId: orderId , email: email},
                    success: function (res) {
                        $('.modal-content').html('');
                        $('.modal-content').html(res);
                        setTimeout(function(){$('#myModal').modal('show')}, 300);
                    }
                });
            });
            $('#btnSearchOrder').off('click').on('click', function (e) {
                order.loadOrders(true);
            });
        },
        paging: function (totalRow, callBack, changePageSize) {
            var totalPage = Math.ceil(totalRow / config.pageSize);

            if (changePageSize === true) {
                $('#paginationUL').empty();
                $('#paginationUL').removeData("twbs-pagination");
                $('#paginationUL').unbind("page");
            }

            $('#paginationUL').twbsPagination({
                totalPages: totalPage,
                visiblePages: 5,
                first: '<<',
                last: '>>',
                prev: '',
                next: '',
                onPageClick: function (event, p) {
                    config.pageIndex = p;
                    callBack();
                }
            });
        },
        loadOrders: function (isPageChanged) {
            var orderId = $('input#orderId').val();
            var email = $('input#email').val();
            $.ajax({
                url: _mainConfig.relativePath + '/Order/ListAll_Paging',
                type: 'get',
                contentType: 'application/json',
                data: { pageIndex: config.pageIndex, pageSize: config.pageSize, orderId: orderId, email: email },
                success: function (res) {
                    if (res.status) {
                        $("#tbody").html('');
                        $("#tbody").html(res.data);
                        order.paging(res.totalRow, function () { order.loadOrders(); }, isPageChanged);
                        order.regisControl();
                    }
                }
            })
        }
    }
})(window, jQuery);

$(function () {
    order.init();
});
