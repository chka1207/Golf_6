
$(document).ready(function(){
    $('[data-toggle="popover"]').popover(); 
});
$(document).ready(function () {
    $('#table_id').DataTable();
    
});

$(document).ready(function () { //Datatablen som visar alla medlemmar i Admin/RedigeraMedlem
    $('#alla_medlemmar').DataTable({
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.modal({
                    header: function (row) {
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

//$(document).ready(function () {
//    $('#alla_medlemmar').DataTable();
//});



