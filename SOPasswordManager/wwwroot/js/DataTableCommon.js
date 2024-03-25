var settings;
var self;
(function ($) {
    $.fn.createGrid = function (options) {
        var defaults = {
            Url: '/CommonGrid/GetGridData',
            PagerInfo: true,
            Mode: '',
            SearchParams: {},
            RecordPerPage: 10,
            DataType: 'POST',
            paging: true,
            Columns: [],
            FixClause: '',
            SortColumn: '0',
            SortOrder: 'asc',
            ExportIcon: true,
            ColumnSelection: true,
            IsAddShow: true,
            OnAdd: fnAdd,
            GrdLabels: JSON.stringify({ show: "Showing", to: "to", of: "of", entries: "entries", search: "Search", first: "first", last: "last", next: "next", previous: "previous", sortAsc: "activate to sort column ascending", sortDesc: "activate to sort column descending", add: "Add", exportTo: "Export To ", excel: "Excel", pdf: "Pdf", csv: "Csv", word: "Word" }),
            DrawCallback: fn_drawCallback,
            createdRow: fn_RowCallback
        };
        settings = $.extend({}, defaults, options);
        self = this;
        var labels = { show: "Showing", to: "to", of: "of", entries: "entries", search: "Search", first: "first", last: "last", next: "next", previous: "previous", sortAsc: "activate to sort column ascending", sortDesc: "activate to sort column descending", add: "Add", exportTo: "Export To ", excel: "Excel", pdf: "Pdf", csv: "Csv", word: "Word", showHideColumn: "Show/Hide collumns", columns: "Columns" };
        $(this).DataTable({
            "rowCallback": settings.createdRow,
            "processing": false,
            //"scrollX": true,
            "serverSide": true,
            "responsive": true,
            "ajax": {
                "type": settings.DataType,
                "url": settings.Url,
                "data": { 'SearchParams': settings.SearchParams, 'mode': settings.Mode, 'FixClause': settings.FixClause },
                'dataType': 'json'
            },
            "columns": settings.Columns,
            "oSearch": settings.SearchParams,
            "pagingType": "simple_numbers",
            "order": [[settings.SortColumn, '' + settings.SortOrder + '']],
            dom: 'lfrti<"toolbar">p',
            initComplete: function () {
                var table = $('#' + $(this).attr("id") + '').DataTable();
                var s = '';
                var s1 = '';
                if (settings.ExportIcon) {
                    s1 += "  <a onclick=\"Export(1,'" + $(this).attr("id") + "')\" data-toggle=\"tooltip\" title=\"" + labels.exportTo.replace(/\"/g, "") + " " + labels.excel.replace(/\"/g, "") + "\" class=\"btn btn-info btn-sm btn\"><i class=\"la la-file-excel-o \"></i><span class=\"d-none d-sm-block\">" + labels.excel.replace(/\"/g, "") + "</span></a>\
                            <a onclick=\"Export(2,'" + $(this).attr("id") + "')\" data-toggle=\"tooltip\" title=\"" + labels.exportTo.replace(/\"/g, "") + " " + labels.pdf.replace(/\"/g, "") + "\" class=\"btn btn-info btn-sm btn\"><i class=\"la la-file-pdf-o\"></i><span class=\"d-none d-sm-block\"> " + labels.pdf.replace(/\"/g, "") + "</span></a>\
                            <a onclick=\"Export(3,'" + $(this).attr("id") + "')\" data-toggle=\"tooltip\" title=\"" + labels.exportTo.replace(/\"/g, "") + " " + labels.csv.replace(/\"/g, "") + "\" class=\"btn btn-info btn-sm btn\"><i class=\"la la-file-text\"></i><span class=\"d-none d-sm-block\"> " + labels.csv.replace(/\"/g, "") + "</span></a>";
                }
                if (settings.IsAddShow) {
                    s += "<a href=\"javascript:void(0);\" data-toggle=\"tooltip\" title=\"" + labels.add.replace(/\"/g, "") + "\" id=\"btnAddNew\" class=\"btn btn-info btn-sm btn clsbtnAddNew\" style=\"margin-right: 4px;margin-left:4px;\"><i class=\"la la-plus sm-mr-5\"></i><span class=\"d-none d-sm-block\">" + labels.add.replace(/\"/g, "") + "</span></a>"
                }
                if (settings.ColumnSelection) {
                    s += '<iframe id=\"exportFram\" name=\"exportFram\" width=\"0\" height=\"0\"  style=\"visibility: hidden;\"></iframe>';
                    s += '&nbsp;<div class="dropdown" style="display: inline-block;" data-toggle=\"tooltip\" title=\"' + labels.showHideColumn.replace(/\"/g, "") + '\"><button id="dLabel1" onclick=\"my_fn(this)\" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" class="btn btn-info btn-sm btn"><i class="la la-columns"></i> <span class=\"d-none d-sm-block\">' + labels.columns.replace(/\"/g, "") + '</span></button><ul style="max-height:520%;overflow-y:auto;z-index:999;position: absolute; list-style:none;margin:0;padding:0" class="dropdown-menu dropdown-menu-right column_drop m-checkbox-list" aria-labelledby="dLabel1">';
                    for (var i = 0; i < table.columns().count(); i++) {

                        var c = table.column(i).visible() == false ? "" : "checked";
                        if (!($(table.column(i).header()).text().match("Id"))) {
                            s += "<li class=\"xs-plr-10 xs-ptb-5\">";
                            s += "   <label class=\"m-checkbox m-checkbox--bold\">";
                            s += "    <input class=\"form-check-input\" type=\"checkbox\" " + c + " onchange=\"ShowHideColumn(this,'" + $(this).attr("id") + "');\" coldata='" + $(table.column(i).header()).text() + "' /><span class=\"form-check-sign\"></span>";
                            s += "" + $(table.column(i).header()).text() + "";
                            s += "    </label>";
                            s += "     </li>";

                        }
                    }
                    s += "</ul></div>";
                }
                $("#" + $(this).attr("id") + "").parent().find("div.dataTables_filter").append(s);
                $("#" + $(this).attr("id") + "").parent().find("div.toolbar").html(s1);

                $(".clsbtnAddNew").on("click", function () {
                    settings.OnAdd();
                });
                $('[data-toggle="tooltip"]').tooltip();
                if (settings.DrawCallback)
                    settings.DrawCallback(settings);

                MakeDataTableResponsive($(this).attr("id"));
            }
        });
        $('div.dataTables_filter input').addClass('form-control form-control-md input-inline');
        $('div.dataTables_length select').addClass('form-control input-inline ');
        $('div.dataTables_length').addClass('col-xs-12 col-md-3 xs-pt-10 sm-text-center md-text-left');
        $('div.dataTables_filter').addClass('col-xs-12 col-md-9 xs-pt-15 sm-text-center md-text-right');
        $('div.toolbar').addClass('col-xs-12 col-md-5 text-center');
        //$('div#example_wrapper').addClass('row xs-mlr-0');
        //$('div.dataTables_wrapper').addClass('row xs-mlr-0');
        $('div#example').addClass('col-xs-12 col-md-8 xs-pb-10');
        $('div.dataTables_info').addClass('col-xs-12 col-md-3');
        $('div.dataTables_paginate').addClass('col-xs-12 col-md-4 xs-text-center sm-text-right');
    };
}(jQuery));

function HideLoading() {
    $(".dataTables_processing").hide();
}

function fn_drawCallback() {

}

function fn_RowCallback(row, data, rowIndex) {

    var tablid = $(this).attr("id");

    var i = 0;
    $.each($('td', row), function () {
        // For example, adding data-* attributes to the cell

        var abc = $("#" + tablid + "").find('th').eq(i).text()
        $(this).attr('title', abc);
        i = i + 1;
    });
}

function my_fn(e) {
    $('body').toggleClass('fix-col');
    // $(this).parent('.dropdown').parents('.wrapper').addClass('asd');
}
function ShowHideColumn(obj, tbl) {
    var table = $('#' + tbl.trim() + '').DataTable();
    for (var i = 0; i < table.columns().count(); i++) {
        if ($(table.column(i).header()).text() == $(obj).attr("coldata")) {
            //table.column(i).visible = obj.checked;
            table.column(i).visible(obj.checked)
        }
    }
}

function fnAdd() {

}

function Export(obj, tbl) {
    var table = $('#' + tbl.trim() + '').DataTable();
    //$(".dataTables_processing").show();
    var type = 'excel';
    if (obj === 2)
        type = 'pdf'
    else if (obj === 3)
        type = 'csv';
    else if (obj === 4)
        type = 'word';
    var winName = 'MyWindow';
    var winURL = '/CommonGrid/ExportData';
    var windowoption = 'resizable=yes,height=600,width=800,location=0,menubar=0,scrollbars=1';
    //  var params = { 'param1': '1', 'param2': '2' };
    var params1 = jQuery('#' + tbl + '').DataTable().ajax.params();
    var params = {
        'search[value]': params1.search.value,
        'order[0][column]': params1.order[0].column,
        'order[0][dir]': params1.order[0].dir,
        'start': "-2",
        'mode': params1.mode,
        'FixClause': params1.FixClause,
        'type': type,
        'columns': params1.columns
    };
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", winURL);
    form.setAttribute("target", winName);
    for (var i in params) {
        if (params.hasOwnProperty(i)) {
            var input;
            if (i === "columns") {
                input = document.createElement('input');
                input.type = 'hidden';
                input.name = 'Columns';
                var cols = "";
                for (c = 0; c < table.columns().count(); c++) {
                    if (table.column(c).visible() != false)
                        cols += cols == "" ? table.column(c).dataSrc() : "," + table.column(c).dataSrc();
                }
                input.value = cols;
                form.appendChild(input);
            }
            else {
                input = document.createElement('input');
                input.type = 'hidden';
                input.name = i;
                input.value = params[i];
                form.appendChild(input);
            }
        }
    }
    document.body.appendChild(form);
    // window.open('', winName, windowoption);
    form.target = "exportFram";//winName;
    form.submit();
    document.body.removeChild(form);
}

function ConvertDateddmmyyyy(data) {
    if (data == null) return '1/1/1950';
    var r = /\/Date\(([0-9]+)\)\//gi
    var matches = data.match(r);
    if (matches == null) return '1/1/1950';
    var result = matches.toString().substring(6, 19);
    var epochMilliseconds = result.replace(
        /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
        '$1');
    var b = new Date(parseInt(epochMilliseconds));
    var c = new Date(b.toString());
    var curr_date = c.getDate();
    var curr_month = c.getMonth() + 1;
    var curr_year = c.getFullYear();
    var curr_h = c.getHours();
    var curr_m = c.getMinutes();
    var curr_s = c.getSeconds();
    var curr_offset = c.getTimezoneOffset() / 60
    var d = curr_date + '/' + curr_month.toString() + '/' + curr_year;
    return d;
}

function ConvertDateMMddyyyy(data) {
    if (data == null) return '1/1/1950';
    var r = /\/Date\(([0-9]+)\)\//gi;
    var matches = data.match(r);
    if (matches == null) return '1/1/1950';
    var result = matches.toString().substring(6, 19);
    var epochMilliseconds = result.replace(
        /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
        '$1');
    var b = new Date(parseInt(epochMilliseconds));
    var c = new Date(b.toString());
    var curr_date = c.getDate();
    var curr_month = c.getMonth() + 1;
    var curr_year = c.getFullYear();
    var curr_h = c.getHours();
    var curr_m = c.getMinutes();
    var curr_s = c.getSeconds();
    var curr_offset = c.getTimezoneOffset() / 60
    var d = curr_month.toString() + '/' + curr_date + '/' + curr_year;
    return d;
}

function Converttohijri(Case_Date) {
    $.ajax({
        url: "/Admin/Cases/ConvertToHijriDate", data: { 'date': Case_Date }, success: function (result) {
            return result;
        }
    });
}

function MakeDataTableResponsive(tblId) {
    $("#" + tblId + " tr th").each(function (e, index) {
        $("#" + tblId + " tr td:nth-child(" + (e + 1) + ")").attr('title', $(this).text());;
    });
}