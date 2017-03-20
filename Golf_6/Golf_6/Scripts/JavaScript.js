
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
        if (data[8] === "Junior 0-12") {
            $('select#medlemskategori').val('1');
        }
        else if (data[8] === "Junior 13-18") {
            $('select#medlemskategori').val('2');
        }
        else if (data[8] === "Studerande") {
            $('select#medlemskategori').val('3');
        }
        else if (data[8] === "Senior") {
            $('select#medlemskategori').val('4');
        }
        $('#medlemskategori').val();
        $('#golfid').val(data[9]);
        $('#telefonnummer').val(data[10]);
        $('#btn-radera').show();
        $('#btn-redigera').show();
        $('#btn-acceptera-ny-medlem').hide();
        $('#välja-födelsedatum').hide();
        $('#se-golfid').show();
        $("#golfid").prop("readonly", true);
    });
});

$(document).ready(function() { //Öppnar upp modal
    $('#btn-ny-medlem-modal').on('click', function () {
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
        $('#välja-födelsedatum').show();
        $('#se-golfid').hide();
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
    var inputFornamn = $("#fornamn").val();
    var inputEfternamn = $("#efternamn").val();
    var inputAdress= $("#adress").val();
    var inputPostnummer= $("#postnummer").val();
    var inputOrt = $("#ort").val();
    var inputEmail= $("#email").val();
    var inputKon = $("#kon").val();
    var inputHandikapp = $("#handikapp").val();
    var inputMedlemskategori = $("#medlemskategori").val();
    var inputGolfid = $("#golfid").val();
    var inputTelefonnummer = $("#telefonnummer").val();

    if ((jQuery.trim(inputFornamn).length > 0) &&
        (jQuery.trim(inputEfternamn).length > 0) &&
        (jQuery.trim(inputAdress).length > 0) &&
        (jQuery.trim(inputPostnummer).length > 0) &&
        (jQuery.trim(inputOrt).length > 0) &&
        (jQuery.trim(inputEmail).length > 0) &&
        (jQuery.trim(inputKon).length > 0) &&
        (jQuery.trim(inputHandikapp).length > 0) &&
        (jQuery.trim(inputMedlemskategori).length > 0) &&
        (jQuery.trim(inputGolfid).length > 0) &&
        (jQuery.trim(inputTelefonnummer).length > 0)) {
        $('#modal-form').attr('action', '/Admin/RedigeraMedlem');
        var form = $("#modal-form");
        var url = form.attr("action");
        var formData = form.serialize();
        $.post(url,
            formData,
            function(data) {
                $("#feedback-radera-medlem").html(data + "är nu redigerad");
            });
    } else {
        alert("Du måste fylla i alla fält!");
    }
});

// NyMedlemKnappen i Admin/AllaMedlemmar -> Modal
$('#btn-acceptera-ny-medlem').click(function () {
    var inputFornamn = $("#fornamn").val();
    var inputEfternamn = $("#efternamn").val();
    var inputAdress= $("#adress").val();
    var inputPostnummer= $("#postnummer").val();
    var inputOrt = $("#ort").val();
    var inputEmail= $("#email").val();
    var inputKon = $("#kon").val();
    var inputHandikapp = $("#handikapp").val();
    var inputMedlemskategori = $("#medlemskategori").val();
    var inputGolfid = $("#golfid").val();
    var inputTelefonnummer = $("#telefonnummer").val();

    if ((jQuery.trim(inputFornamn).length > 0) && (jQuery.trim(inputEfternamn).length > 0) && (jQuery.trim(inputAdress).length > 0) &&
        (jQuery.trim(inputPostnummer).length > 0) && (jQuery.trim(inputOrt).length > 0) && (jQuery.trim(inputEmail).length > 0) &&
        (jQuery.trim(inputKon).length > 0) && (jQuery.trim(inputHandikapp).length > 0) && (jQuery.trim(inputMedlemskategori).length > 0) &&
        (jQuery.trim(inputGolfid).length > 0) && (jQuery.trim(inputTelefonnummer).length > 0)) {
        $('#modal-form').attr('action', '/Admin/RegistreraNyMedlem');
        var form = $("#modal-form");
        var url = form.attr("action");
        var formData = form.serialize();
        $.post(url,
            formData,
            function(data) {
                $("#feedback-radera-medlem").html(data + "är nu inlagd i systemet");
            });
    } else {
        alert("Du måste fylla i alla fält!");
    }
});

$(function () {
    $("#datepicker").datepicker();
});

$(function () {
    $('#datepickerFödelsedatum').datepicker({
        onSelect: function (date) {
            $("#golfid").val(date);
            var number =  Math.floor(Math.random() * 899) + 100 ;
            $('#golfid').val($('#golfid').val() + number);
        },
        changeYear: true,
        yearRange: "1900:2017",
        dateFormat: 'ymmdd-'
    });

    var currentDate = $(".selector").datepicker("getDate");
$.datepicker.regional['sv'] = {
    closeText: 'Stäng',
    prevText: '< Föregående',
    nextText: 'Nästa >',
    currentText: 'Idag',
    monthNames: [
        'Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli', 'Augusti', 'September', 'Oktober',
        'November', 'December'
    ],
    monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dec'],
    dayNamesShort: ['Sön', 'Mån', 'Tis', 'Ons', 'Tor', 'Fre', 'Lör'],
    dayNames: ['Söndag', 'Måndag', 'Tisdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lördag'],
    dayNamesMin: ['Sö', 'Må', 'Ti', 'On', 'To', 'Fr', 'Lö'],
    weekHeader: 'Не',
    dateFormat: 'yy-mm-dd',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: '',
    onSelect: function (dateText, inst) {
        //    //$('form').submit();
        //    $('form:first').submit();
    }
};
$.datepicker.setDefaults($.datepicker.regional['sv']);
});

function whichDay(dateString) { //Funktion för att hämta dag på ett speciellt datum
    return ['Söndag', 'Måndag', 'Tisdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lördag']
        [new Date(dateString).getDay()];
}
window.onload = function () { //Hämtar dag och visar upp detta i Admin/HanteraSasong
    document.getElementById("startVeckodag").innerHTML = whichDay(document.getElementById("startVeckodag").innerHTML);
    document.getElementById("slutVeckodag").innerHTML = whichDay(document.getElementById("slutVeckodag").innerHTML);
};

function showDiv() {
    document.getElementById('sokfunktion').style.display = "block";
}
function hideDiv() {
    document.getElementById('sokfunktion').style.display = "none";
}


//$("#btnSök").on('click', function () {
//    $.ajax({
//        async: false,
//        url: '/Tidsbokning/Search'
//    }).success(function (partialView) {
//        $('#partialViewSök').append(partialView);
//    });
//});
