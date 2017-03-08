
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
            buttons: [
                //'colvis' //Dropdown för att välja kolumner
                'columnsToggle' //Varje kolumn får en egen knapp att välja (show/hide)
            ],
            pagingType: "simple", //Går attt ändra för att se pagenumbers mm
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
            ],
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.modal({
                        header: function(row) {
                            var data = row.data();
                            return 'Utökade detaljer för ' + data[0] + ' ' + data[1];
                        }
                    }),
                renderer: $.fn.dataTable.Responsive.renderer.tableAll({
                    tableClass: 'table'
                })
            }
        }
    });
});

function whichDay(dateString) {
    return ['Söndag', 'Måndag', 'Tisdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lördag']
        [new Date(dateString).getDay()];
}
window.onload = function () {
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
