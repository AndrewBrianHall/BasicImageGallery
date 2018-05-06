$(document).ready(function () {
    $("#file").on('change', function () {
        $("#form").submit();
    });
    //$('.lazy').Lazy({
    //    scrollDirection: 'vertical',
    //    effect: 'fadeIn',
    //    visibleOnly: true,
    //    onError: function (element) {
    //        console.log('error loading ' + element.data('src'));
    //    }
    //});
});