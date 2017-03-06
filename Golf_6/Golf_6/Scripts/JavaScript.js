
$(document).ready(function(){
    $('[data-toggle="popover"]').popover(); 
});
$(document).ready(function () {
    $('#table_id').DataTable();
    
});

$(document).ready(function () { //Datatablen som visar alla medlemmar i Admin/AllaMedlemmar
    $('#alla_medlemmar').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'colvis'
        ],
        pagingType: "simple",
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

