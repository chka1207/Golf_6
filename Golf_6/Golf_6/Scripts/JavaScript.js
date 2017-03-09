
$(document).ready(function(){
    $('[data-toggle="popover"]').popover(); 
});
$(document).ready(function () {
    $('#table_id').DataTable();
    
});
//$(document).ready(function () {
//    $('#hämtaMedlemmar').DataTable();

//});

$(document).ready(function () {
    $('#hamtaMedlemmar').DataTable({
        "pagingType": "simple",
        "language": {
            "search": "Sök:",
            "info": "",
            "lengthMenu": "Visa _MENU_ åt gången",
            "infoFiltered": "",
            "zeroRecords": "Din sökning gav inga träffar",
            "infoEmpty": "",
            "paginate": {
                "previous": "Föregående",
                "next": "Nästa"
            }
        }
    });
});
    //$(document).ready(function () {
    //    $('#btnSök').click(function () {
    //        $('#divSök').load('@Url.Action("Tidsbokning", "Search")');
    //    });
    //});

$(document).ready(function () { //Datatablen som visar alla medlemmar i Admin/AllaMedlemmar
    $('#alla_medlemmar').DataTable({
        dom: 'Bfrtip',
        info: 'false',
            buttons: [
                //'colvis' //Dropdown för att välja kolumner
                'columnsToggle' //Varje kolumn får en egen knapp att välja (show/hide)
            ],
            pagingType: "simple", //Går att ändra för att se pagenumbers mm
            columnDefs: [
                {
                    targets: [2], //Gömmer adress
                    visible: false
                },
                {
                    targets: [3], //Gömmer postnummer
                    visible: false
                },
                {
                    targets: [5], //Gömmer e-mail   
                    visible: false
                },
                {
                    targets: [10], //Gömmer tele
                    visible: false
                }
            ]
    });
});

$(document).ready(function () { //Denna laddar in värdena från varje cell till modal
    var table = $('#alla_medlemmar').DataTable();

    $('#alla_medlemmar').on('click', 'tr', function () {
        $("#modal-hantera-medlem").modal();
        var data = table.row(this).data();
        $('#modal-namn-medlem').html("Hantera " + data[0] + " " + data[1]);
        $('#fornamn').val(data[0]);
        $('#efternamn').val(data[1]);
        $('#adress').val(data[2]);
        $('#postnummer').val(data[3]);
        $('#ort').val(data[4]);
        $('#email').val(data[5]);
        $('#kon').val(data[6]);
        $('#handikapp').val(data[7]);
        $('#medlemskategori').val(data[8]);
        $('#golfid').val(data[9]);
        $('#telefonnummer').val(data[10]);
        $('#btn-radera').show();
        $('#btn-redigera').show();
        $('#btn-acceptera-ny-medlem').hide();
    });
});

$(document).ready(function() { //Öppnar upp modal
    $('#btn-ny-medlem-modal').on('click', function() {
        $('#modal-hantera-medlem').modal();
        $('#modal-namn-medlem').html("Lägga till ny medlem");
        $('#fornamn').val("");
        $('#efternamn').val("");
        $('#adress').val("");
        $('#postnummer').val("");
        $('#ort').val("");
        $('#email').val("");
        $('#kon').val("");
        $('#handikapp').val("");
        $('#medlemskategori').val("");
        $('#golfid').val("");
        $('#telefonnummer').val("");
        $('#btn-radera').hide();
        $('#btn-redigera').hide();
        $('#btn-acceptera-ny-medlem').show();
    });
});

// Raderaknappen i Admin/AllaMedlemmar -> Modal
$('#btn-radera').click(function () {
    $('#modal-form').attr('action', '/Admin/RaderaMedlem');
    var form = $("#modal-form");
    var url = form.attr("action");
    var formData = form.serialize();
    $.post(url,
        formData,
        function (data) {
            $("#feedback-radera-medlem").html(data + "är nu raderad");
        });
});

// Redigeraknappen i Admin/AllaMedlemmar -> Modal
$('#btn-redigera').click(function () {
    $('#modal-form').attr('action', '/Admin/RedigeraMedlem');
    var form = $("#modal-form");
    var url = form.attr("action");
    var formData = form.serialize();
    $.post(url,
        formData,
        function (data) {
            $("#feedback-radera-medlem").html(data + "är nu redigerad");
        });
});

// NyMedlemKnappen i Admin/AllaMedlemmar -> Modal
$('#btn-acceptera-ny-medlem').click(function () {
    $('#modal-form').attr('action', '/Admin/RegistreraNyMedlem');
    var form = $("#modal-form");
    var url = form.attr("action");
    var formData = form.serialize();
    $.post(url,
        formData,
        function (data) {
            $("#feedback-radera-medlem").html(data + "är nu inlagd i systemet");
        });
});

function whichDay(dateString) { //Funktion för att hämta dag på ett speciellt datum
    return ['Söndag', 'Måndag', 'Tisdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lördag']
        [new Date(dateString).getDay()];
}
window.onload = function () { //Hämtar dag och visar upp detta i Admin/HanteraSasong
    document.getElementById("startVeckodag").innerHTML = whichDay(document.getElementById("startVeckodag").innerHTML);
    document.getElementById("slutVeckodag").innerHTML = whichDay(document.getElementById("slutVeckodag").innerHTML);
};

//$("#btnSök").on('click', function () {
//    $.ajax({
//        async: false,
//        url: '/Tidsbokning/Search'
//    }).success(function (partialView) {
//        $('#partialViewSök').append(partialView);
//    });
//});
