$(document).ready(function () {
    $('.iconb').tooltip()
    $('.btn-img').tooltip()
    $('.form-group select').addClass('form-control');
    //$("#pac_fechanacimiento").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat:"dd/mm/yy",
    //    onClose: function (selectedDate) {
    //        var f = new Date();
    //        $("#pac_edad").val(restaFechas(selectedDate, f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear()));
    //    }
    //});
    var progressbar = $("#div_loading"), progressLabel = $(".progress-bar");

    progressbar.progressbar({
        value: false,
        change: function () {
            progressLabel.css("width", progressbar.progressbar("value")+"%");
            //progressLabel.text(progressbar.progressbar("value") + "%");
        },
        complete: function () {
            //progressLabel.text("Completo");
        }
    });

    function progress() {
        var val = progressbar.progressbar("value") || 0;

        progressbar.progressbar("value", val + 2);

        if (val < 99) {
            setTimeout(progress, 80);
        }
    }


    setTimeout(progress, 2000);

});

$(document).ajaxStart(function () {
    $('#div_loading').show();
    $('#div_form').hide();
});
$(document).ajaxStop(function () {
    $('#div_loading').hide();
    $('#div_form').show();
});

restaFechas = function (f1, f2) {
    var aFecha1 = f1.split('/');
    var aFecha2 = f2.split('/');
    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
    var dif = fFecha2 - fFecha1;
    var dias = Math.floor(dif / (1000 * 60 * 60 * 24*365));
    return dias;
}

function error(result, status, xhr) {
    alert("Ocurrio un problema con la variable de sesión, empiece el proceso nuevamente, si el problema persiste contacte con el administrador");
    $(location).attr('href', $("#link_error").attr("href"));
}