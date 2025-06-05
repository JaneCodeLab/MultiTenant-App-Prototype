$(function () {
    $('.select2').select2();
    $('.summernote').summernote({ height: 500, focus: true });
});
(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();


$(function () {
    let tableName = "#CustomDataTable";
    let api = $(tableName).data("api");
    const isServerSide = api != undefined;
    let columnsObj = {}, buttons = {};
    let colvis = { extend: 'colvis', columns: ':not(.noVis)' };
    if (typeof columns !== "undefined")
        columnsObj = { columns: columns };
    if (!isServerSide)
        buttons = {
            "buttons": [
                "copy",
                "excel",
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    exportOptions: {
                        columns: ':not(.no-export)'
                    }
                },
                "print",
                colvis
            ]
        };
    else {
        buttons = {
            "buttons": [
                {
                    text: '<em title="Excel" class="fa-solid fa-file-excel"></em>',
                    action: function (e, dt, node, config) {
                        var fileName = `${window.tenantName}_${window.pageTitle}_${formatedDate()}`;
                        var params = dt.ajax.params();
                        var inner = node[0].innerHTML;
                        node[0].innerHTML = "<em class='fas fa-spinner fa-spin'></em>";
                        const url = `${dt.ajax.url().replace(/getdata/gi, 'excel')}?fileName=${fileName}`;

                        $.ajax({
                            url: url,
                            method: 'POST',
                            data: params,
                            cache: false,
                            xhr: function () {
                                var xhr = new XMLHttpRequest();
                                xhr.onreadystatechange = function () {
                                    if (xhr.readyState == 2) {
                                        if (xhr.status == 200) {
                                            xhr.responseType = "blob";
                                        } else {
                                            xhr.responseType = "text";
                                        }
                                    }
                                };
                                return xhr;
                            },
                            success: response => {
                                node[0].innerHTML = inner;
                                var blob = new Blob([response], { type: "application/octetstream" });

                                var isIE = false || !!document.documentMode;
                                if (isIE) {
                                    window.navigator.msSaveBlob(blob, fileName);
                                } else {
                                    var url = window.URL || window.webkitURL;
                                    link = url.createObjectURL(blob);
                                    var a = $("<a />");
                                    a.attr("download", `${fileName}.xlsx`);
                                    a.attr("href", link);
                                    $("body").append(a);
                                    a[0].click();
                                    $("body").remove(a);
                                }
                            }
                        });
                    }
                },
                colvis
            ]
        };
    }
    let formatedDate = () => {
        var m = new Date();
        return `${m.getFullYear()}-${(m.getMonth() + 1)}-${m.getDate()}_${m.getHours()}-${m.getMinutes()}-${m.getSeconds()}`;
    }
    var onInit = (settings, json) => {
        var table = $(tableName).DataTable();
        table.buttons().container().appendTo(`${tableName}_wrapper  .col-md-6:eq(0)`);
        table.buttons('.buttons-copy').text('<em title="Kopayala" class="fa-solid fa-copy"></em>');
        table.buttons('.buttons-excel').text('<em title="Excel" class="fa-solid fa-file-excel"></em>');
        table.buttons('.buttons-pdf').text('<em title="PDF" class="fa-solid fa-file-pdf"></em>');
        table.buttons('.buttons-print').text('<em title="Print" class="fa-solid fa-print"></em>');
        table.buttons('.buttons-collection').text('');
        $(`${tableName}_length`).addClass("float-right");
        $(`${tableName}_paginate`).addClass("float-right");
        $(`${tableName}_info`).addClass("float-left");
        $(`${tableName}_filter > label > input[type=search]`).each(function () { $(this).before('<em class="fas fa-search fa-fw" style="margin-left: -20px;"></em>') });

        if (table.rows().count() === 0) {
            $(`${tableName}_empty`).show();
            $(`${tableName}_wrapper`).hide();
        }
    }
    $(tableName).DataTable({
        "oLanguage": { "sSearch": "" },
        "bInfo": false,
        "language": {
            'paginate': {
                'previous': '<em class="fa-solid fa-angles-left"></em>',
                'next': '<em class="fa-solid fa-angles-right"></em>'
            },
            "lengthMenu": '_MENU_',
            "processing": `<span class='loaging'>${loadingText}</span>`,
            "info": datatablesInfoText
        },
        processing: true,
        serverSide: isServerSide,
        filter: !isServerSide,
        ajax: isServerSide ? {
            url: api,
            type: "POST"
        } : undefined,
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "order": [1, 'desc'],
        "columnDefs": [
            { "targets": 'no-sort', "orderable": false, },
            { "lengthMenu": [10, 25, 50, 75, 100] },
            { "targets": 'no-vis', "className": 'noVis' }],
        "initComplete": onInit,
        ...columnsObj,
        ...buttons,
        orderCellsTop: true,
        "sDom": 'RBfrtipl',
        'info': true,
    })
});
function clear_all_filter() {
    event.stopPropagation();
    var filters = document.querySelectorAll('*[id^="Filter_"]');
    filters.forEach(function (filter) {
        filter.value = null;
    });
    document.getElementById("Filter_ClearAll").value = true;
    document.getElementById("searchButton").click();
};

function clean_filter(event, fieldName) {
    event.stopPropagation();
    document.getElementById("Filter_ItemRemoved").value = true;
    document.getElementById("Filter_" + fieldName).value = null;
    document.getElementById("searchButton").click();
};
$('[data-mask]').inputmask();