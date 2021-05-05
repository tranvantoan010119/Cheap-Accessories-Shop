var config = {
    pageIndex: 1,
    pageSize: 10
};

(function (window, $) {
    window.customer = {
        init: function () {
            customer.loadCustomers();
        },
        regisControl: function () {
            $('.cbStatus').off('click').on('click', function (e) {
                var Id = $(this).data("id");
                $.ajax({
                    url: _mainConfig.relativePath + '/Customer/ChangeStatus',
                    data: { Id: Id },
                    type: 'post',
                    success: function (res) {
                        if(res.status)
                        {
                            toastr["success"]("Cập nhật thành công.");
                        }
                        else
                        {
                            toastr["error"]("Có lỗi xảy ra, vui lòng thử lại.");
                            setTimeout(function(){ location.reload() }, 2500);
                        }
                    }
                });
            });
            $('#btnSearchCustomer').off('click').on('click',function(){
                customer.loadCustomers(true);
            });
        },
        paging: function (totalRow,callBack,changePageSize) {
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
                onPageClick: function(event,p)
                {
                    config.pageIndex = p;
                    callBack();
                }
            });
        },
        loadCustomers: function (isPageChanged) {
            var fullname = $('input#fullname').val();
            var email = $('input#email').val();
            $.ajax({
                url: _mainConfig.relativePath + '/Customer/Get_CustomerListAll',
                type: 'get',
                contentType: 'application/json',
                data: {pageIndex: config.pageIndex,pageSize: config.pageSize, fullname: fullname, email: email},
                success: function (res) {
                    if (res.status)
                    {
                        $("#tbody").html('');
                        $("#tbody").html(res.data);
                        customer.paging(res.totalRow, function () { customer.loadCustomers(); }, isPageChanged);
                        customer.regisControl();
                    }
                }
            })
        }
    }
})(window, jQuery);

$(function () {
    customer.init();
});
