var config = {
    pageIndex: 1,
    pageSize: 10
};

(function (window, $) {
    window.news = {
        init: function () {
            news.loadNews();
        },
        regisControl: function () {
            $('.cbStatus').off('click').on('click', function (e) {
                var Id = $(this).data("id");
                $.ajax({
                    url: _mainConfig.relativePath + '/News/ChangePublished',
                    data: { Id: Id },
                    type: 'post',
                    success: function (res) {
                        if (res.status) {
                            toastr["success"]("Cập nhật thành công.");
                        }
                        else {
                            toastr["error"]("Có lỗi xảy ra, vui lòng thử lại.");
                            setTimeout(function(){ location.reload() }, 2500);
                        }
                    }
                });
            });
            $('#btnSearchCustomer').off('click').on('click', function () {
                news.loadNews(true);
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
        loadNews: function (isPageChanged) {
            $.ajax({
                url: _mainConfig.relativePath + '/News/Get_NewsListAll',
                type: 'get',
                contentType: 'application/json',
                data: { pageIndex: config.pageIndex, pageSize: config.pageSize},
                success: function (res) {
                    if (res.status) {
                        $("#tbody").html('');
                        $("#tbody").html(res.data);
                        news.paging(res.totalRow, function () { news.loadNews(); }, isPageChanged);
                        news.regisControl();
                    }
                }
            })
        }
    }
})(window, jQuery);

$(function () {
    news.init();
});
