﻿$(document).ready(initialize);

function initialize() {
    $('#datecreatedfrom,#datecreated,#policyperiodfrom,#policyperiodto').datepicker({
        showOn: "button",
        buttonImage: "images/Calendar-icon.png",
        buttonImageOnly: true
    });

    showloader();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: { action: "loadsearchfilters" },
        success: function (result) {
            var obj = JSON.parse(result);
            var creditingBranch = obj["creditingbranches"];
            buildfilter('creditingbranch', creditingBranch);
            var sublines = obj["sublines"];
            buildfilter('subline', sublines);
            var mortgagee = obj["mortgagee"];
            buildfilter('mortgagee', mortgagee);
            var intermediary = obj["intermediary"];
            buildfilter('intermediary', intermediary);
            var typeofcovers = obj["typeofcovers"];
            buildfilter('typeofcover', typeofcovers);
            var carcompany = obj["carcompanies"];
            buildfilter('carcompany', carcompany);
            hideloader();
            searchtransactions(function(){
                secondinitialize();
            });
        },
        error: function () {
            hideloader();
        }
    });

    $('#searchbutton').click(function () {
        initpaging();
        searchtransactions();
    });
    $('#resetbutton').click(resetsearch);
    
}

function buildfilter(element, list) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(list, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#' + element).html(html);
}

function searchtransactions(callback) {
    showloader();
    var creditingbranch = $('#creditingbranch').val();
    var parno = $('#parno').val();
    var policyno = $('#policyno').val();
    var subline = $('#subline').val();
    var datecreated = $('#datecreated').val();
    var policyperiodfrom = $('#policyperiodfrom').val();
    var policyperiodto = $('#policyperiodto').val();
    var typeofcover = $('#typeofcover').val();
    var mortgagee = $('#mortgagee').val();
    var intermediary = $('#intermediary').val();
    var carcompany = $('#carcompany').val();
    var motortype = $('#motortype').val();
    var chassisno = $('#chassisno').val();
    var engineno = $('#engineno').val();
    var firstname = $('#firstname').val();
    var lastname = $('#lastname').val();

    var page = $('#pagehidden').val();
    var rowcount = $('#rowcounthidden').val();

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "searchtransactions",
            creditingbranch: creditingbranch,
            parno: parno,
            policyno: policyno,
            subline: subline,
            datecreated: datecreated,
            policyperiodfrom: policyperiodfrom,
            policyperiodto: policyperiodto,
            typeofcover: typeofcover,
            mortgagee: mortgagee,
            intermediary: intermediary,
            carcompany: carcompany,
            motortype: motortype,
            chassisno: chassisno,
            engineno: engineno,
            firstname: firstname,
            lastname: lastname,
            page: page,
            rowcount: rowcount
        },
        success: function (result) {
            var obj = JSON.parse(result);
            html = '<table class="table table-bordered table-striped"><tr>';
            html += '<th></th>';
            //            html += '<th></th>';
            html += '<th>Branch</th>';
            html += '<th>Par No</th>';
            html += '<th>Policy No</th>';
            html += '<th>Subline</th>';
            html += '<th>Date Created</th>';
            html += '<th>Period From</th>';
            html += '<th>Period To</th>';
            html += '<th>Type Of Cover</th>';
            html += '<th>Car Company</th>';
            html += '<th>Motor Type</th>';
            html += '<th>Chassis No</th>';
            html += '<th>Engine No</th>';
            html += '<th>Plate No</th>';
            html += '<th>LastName</th>';
            html += '<th>FirstName</th>';
            html += '<th>MiddleName</th>';
            html += '</tr>';
            if (obj != null) {
                $.each(obj.Data, function (key, value) {
                    html += '<tr>';
                    html += '<td><a href="TransactionDetailsView.aspx?id=' + value.TransactionID + '">Details</a></td>';
                    //                    html += '<td><a href="TransactionView.aspx?id=' + value.TransactionID + '">Edit</a></td>';
                    html += '<td>' + value.CreditingBranch + '</td>';
                    html += '<td>' + value.ParNo + '</td>';
                    html += '<td>' + value.PolicyNo + '</td>';
                    html += '<td>' + value.Subline + '</td>';
                    html += '<td>' + value.DateCreatedText + '</td>';
                    html += '<td>' + value.PolicyPeriodFromText + '</td>';
                    html += '<td>' + value.PolicyPeriodToText + '</td>';
                    html += '<td>' + value.TypeOfCover + '</td>';
                    html += '<td>' + value.CarCompany + '</td>';
                    html += '<td>' + value.MotorType + '</td>';
                    html += '<td>' + value.ChassisNo + '</td>';
                    html += '<td>' + value.EngineNo + '</td>';
                    html += '<td>' + value.PlateNo + '</td>';
                    html += '<td>' + value.LastName + '</td>';
                    html += '<td>' + value.FirstName + '</td>';
                    html += '<td>' + value.MiddleName + '</td>';
                    html += '</tr>';
                });

                if (obj.Data.length <= 0) {
                    $('#rowsperpage').prop('disabled', true);
                } else {
                    $('#rowsperpage').prop('disabled', false);
                }
            }


            $('#searchresult').html(html);

            var pager = '';
            for (var i = 1; i <= obj.PageCount; i++) {
                var cssclass = '';
                if (i === obj.CurrentPage) {
                    cssclass = 'active';
                }
                pager += '<li class="clickable ' + cssclass + '"><a onclick="changepage(' + i + ')">' + (i) + '</a></li>';
            }
            $('#search-pager').html(pager);
            $('#search-pager').show();
            hideloader();

            if (callback != null && (callback instanceof Function)) {
                callback();
            }
        },
        error: function () {
            hideloader();
        }
    });
}

function resetsearch() {
    $('#creditingbranch').val('0');
    $('#parno').val('');
    $('#policyno').val('');
    $('#subline').val('0');
    $('#datecreated').val('');
    $('#policyperiodfrom').val('');
    $('#policyperiodto').val('');
    $('#typeofcover').val('0');
    $('#mortgagee').val('0');
    $('#intermediary').val('0');
    $('#carcompany').val('0');
    $('#motortype').val('0');
    $('#chassisno').val('');
    $('#engineno').val('');
    $('#firstname').val('');
    $('#lastname').val('');
    $('#searchresult').html('');

    $('#search-pager').hide();
    $('#rowsperpage').prop('disabled', true);
}


function showloader() {
    $('#progressbox').css('display', 'block');
    $('#progressbox').dialog({ modal: true, resizable: false, draggable: false, dialogClass: 'notitledialog', width: 300, height: 80 });
}

function hideloader() {
    $('#progressbox').dialog('close');
    $('#progressbox').css('display', 'none');
}