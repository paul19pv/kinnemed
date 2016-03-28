$(document).ready(function () {
    $("#pac_provincia").change(function () {
        fillCombo("pac_canton", $("#pac_provincia").val());
    });
});

function fillCombo(updateId, value) {
    $.ajax({
        type: "POST",
        //url: "/kinnemed01/Paciente/GetCantonesPorProvincia",
        url: "/Paciente/GetCantonesPorProvincia",
        //url: '@Url.Action("GetCantonesPorProvincia","Paciente")',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "id": value }),
        success: function (resultado) {
            $("#" + updateId).empty();
            $.each(resultado, function (i, item) {
                $("#" + updateId).append("<option value='"
                   + item.Value + "'>" + item.Text
                   + "</option>");
            });

        },
        error: function () {
            alert('Ocurrió un error al acceder a la información');
        }
    });
}