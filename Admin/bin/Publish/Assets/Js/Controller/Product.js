var config = {
    pageIndex: 1,
    pageSize: 10
};

(function (window, $) {
    window.product = {
        init: function () {
            if ($('#actionName').val() != "Statistic") {
                product.loadProducts();
            }
            else {
                product.regisControl();
            }
        },
        regisControl: function () {
            $('.cbStatus').off('click').on('click', function (e) {
                var Id = $(this).data("id");
                $.ajax({
                    url: _mainConfig.relativePath + '/Product/ChangeStatus',
                    data: { Id: Id },
                    type: 'post',
                    success: function (res) {
                        if (res.status) {
                            toastr["success"]("Cập nhật thành công.");
                        }
                        else {
                            toastr["error"]("Có lỗi xảy ra, vui lòng thử lại.");
                            setTimeout(function () { location.reload() }, 2500);
                        }
                    }
                });
            });
            $('.cbIsHot').off('click').on('click', function (e) {
                var Id = $(this).data("id");
                $.ajax({
                    url: _mainConfig.relativePath + '/Product/ChangeIsHot',
                    data: { Id: Id },
                    type: 'post',
                    success: function (res) {
                        if (res.status) {
                            toastr["success"]("Cập nhật thành công.");
                        }
                        else {
                            toastr["error"]("Có lỗi xảy ra, vui lòng thử lại.");
                            setTimeout(function () { location.reload() }, 2500);
                        }
                    }
                });
            });
            $('#btnSearchProduct').off('click').on('click', function (e) {
                product.loadProducts(true);
            });
            $('#btnStatisticProduct').off('click').on('click', function (e) {
                e.preventDefault();
                product.loadStatistic(true);
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
        loadProducts: function (isPageChanged) {
            var code = $('input#code').val();
            var name = $('input#productName').val();
            $.ajax({
                url: _mainConfig.relativePath + '/Product/Get_ProductListAll',
                type: 'get',
                contentType: 'application/json',
                data: { pageIndex: config.pageIndex, pageSize: config.pageSize, keyword: name, code: code },
                success: function (res) {
                    if (res.status) {
                        $("#tbody").html('');
                        $("#tbody").html(res.data);
                        product.paging(res.totalRow, function () { product.loadProducts(); }, isPageChanged);
                        product.regisControl();
                    }
                }
            })
        },
        loadStatistic: function (isPageChanged) {
            var type = $('#typeOfStatistic').val();
            if (type == "")
                toastr["error"]("Vui lòng chọn loại thống kê.");
            else {
                $.ajax({
                    url: _mainConfig.relativePath + '/Product/Get_Statistic_Product',
                    type: 'get',
                    contentType: 'application/json',
                    data: { pageIndex: config.pageIndex, pageSize: config.pageSize, type: type },
                    success: function (res) {
                        if (res.status) {
                            if (type == "inventory")
                                $('#colType').empty().html(" Số lượng tồn");
                            else
                                $('#colType').empty().html(" Số lượng bán");
                            $("#tbody").html('');
                            $("#tbody").html(res.data);
                            product.paging(res.totalRow, function () { product.loadStatistic(); }, isPageChanged);
                        }
                        else {
                            toastr["error"]("Vui lòng chọn loại thống kê.");
                        }
                    }
                })
            }

        }
    }
})(window, jQuery);

$(function () {
    product.init();
});
