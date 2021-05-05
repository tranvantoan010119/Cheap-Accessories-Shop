(function(window, $){
    window.base = {
        load: function(){
            base.InputNumber();
        },
        formatJsonDate: function(datetime){
            if (datetime == '' || datetime == undefined) {
                return '';
            } else {
                var newdate = new Date(parseInt(datetime.substr(6)));
                var month = newdate.getMonth() + 1;
                var day = newdate.getDate();
                var year = newdate.getFullYear();
                if (month < 10)
                    month = "0" + month;
                if (day < 10)
                    day = "0" + day;
                return day + "/" + month + "/" + year;
            }
        },
        getCurrentDate: function(){
            var d = new Date();
            var strDate = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            return strDate;
        },
        dateToString: function (timestamp) {
            var x = new Date(timestamp);
            var dd = x.getDate();
            var mm = x.getMonth() + 1;
            var yy = x.getFullYear();
            return dd + "/" + mm;
            //return dd + "/" + mm + "/" + yy;
        },
        InputNumber: function () {
            $("input[inputNumber]").on('keyup', function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
            });
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
});
