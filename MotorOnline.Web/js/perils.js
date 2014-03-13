var limitsivalidationcontrols = ['thefttextbox', 'striketextbox', 'actsofnaturetextbox'];
//limitsivalidationcontrols[1] = [];
//limitsivalidationcontrols[2] = ;
//limitsivalidationcontrols[3] = ['thefttextbox', 'striketextbox', 'actsofnaturetextbox'];

var computationmatrix = [];
computationmatrix[1] = { "ids": [187] };
computationmatrix[2] = { "ids": [274, 182, 191, 194, 195, 184, 187] };
computationmatrix[3] = { "ids": [274, 182, 191, 194, 195, 184] };

var associatedcontrols = [];
associatedcontrols[182] = { "limitsi": "actsofnaturetextbox",
    "rate": "actsofnatureratetextbox",
    "premium": "actsofnaturepremiumtextbox",
    "policyrate": "actsofnaturepolicyratetextbox",
    "policypremium": "actsofnaturepolicypremiumtextbox",
    "data": "actsofnaturehidden"
};

associatedcontrols[184] = { "limitsi": "autopatextbox",
    "rate": "autoparatetextbox",
    "premium": "autopapremiumtextbox",
    "policyrate": "autopapolicyratetextbox",
    "policypremium": "autopapolicypremiumtextbox",
    "data": "autopahidden"
};

associatedcontrols[187] = { "limitsi": "ctpltextbox",
    "rate": "ctplratetextbox",
    "premium": "ctplpremiumtextbox",
    "policyrate": "ctplpolicyratetextbox",
    "policypremium": "ctplpolicypremiumtextbox",
    "data": "ctplhidden"
};

associatedcontrols[191] = { "limitsi": "striketextbox",
    "rate": "strikeratetextbox",
    "premium": "strikepremiumtextbox",
    "policyrate": "strikepolicyratetextbox",
    "policypremium": "strikepolicypremiumtextbox",
    "data": "strikehidden"
};

associatedcontrols[194] = { "limitsi": "vtplbodilydropdown",
    "rate": "vtplbodilyratetextbox",
    "premium": "vtplbodilypremiumtextbox",
    "policyrate": "vtplbodilypolicyratetextbox",
    "policypremium": "vtplbodilypolicypremiumtextbox",
    "data": "vtplbodilyhidden"
};

associatedcontrols[195] = { "limitsi": "vtplpropertydropdown",
    "rate": "vtplpropertyratetextbox",
    "premium": "vtplpropertypremiumtextbox",
    "policyrate": "vtplpropertypolicyratetextbox",
    "policypremium": "vtplpropertypolicypremiumtextbox",
    "data": "vtplpropertyhidden"
};

associatedcontrols[274] = { "limitsi": "thefttextbox",
    "rate": "theftratetextbox",
    "premium": "theftpremiumtextbox",
    "policyrate": "theftpolicyratetextbox",
    "policypremium": "theftpolicypremiumtextbox",
    "data": "thefthidden"
};

//This is used for add mode
function displayperils() {
    var selectedType = $('#TypeOfCoverDropdown').val();
    var subline = $('#SublineDropdown').val();
    var motortype = $('#MotorTypeDropdown').val();
    var ids = [194, 195];
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            "action": 'getperils',
            "type": selectedType,
            "subline": subline,
            "motortype": motortype,
            ids: JSON.stringify(ids)
        },
        success: function (result) {
            var obj = JSON.parse(result);
            var html = '<table class="computation-table" cellpadding=4 WIDTH=70%>';
            html += '<tr>';
            html += '<th>Coverage/Perils</th>';
            html += '<th>Limit/SI</th>';
            html += '<th>Rate</th>';
            html += '<th>Premium</th>';
            html += '<th>Policy Rate</th>';
            html += '<th>Policy Premium</th>';
            html += '</tr>';
            if (obj != null) {

                $.each(obj.Perils, function (key, value) {
                    console.log(value);
                    html += '<tr>';
                    html += '<td>';
                    html += value.PerilName;
                    html += "<input id='" + associatedcontrols[value.PerilID].data + "' type='hidden' value='" + JSON.stringify(value.PerilID) + "' />";
                    html += '</td>';

                    html += '<td>';
                    html += handlelimitsicontrol(value.PerilID);
                    html += '</td>';

                    html += '<td>';
                    html += '<input class="fixed-width-100" id="' + associatedcontrols[value.PerilID].rate + '" type="text" />';
                    html += '</td>';

                    html += '<td>';
                    html += createpremiumtemplate(0, value.PerilID);
                    html += '</td>';

                    html += '<td>';
                    html += '<input class="fixed-width-100" id="' + associatedcontrols[value.PerilID].policyrate + '" type="text" />';
                    html += '</td>';

                    html += '<td>';
                    html += createpolicypremiumtemplate(0, value.PerilID);
                    html += '</td>';

                    html += '</tr>';
                });

            }
            html += '<tr><td colspan="2"><input id="computebutton" type="button" value="Compute" class="btn btn-default" /></td>';
            html += '<td align="right"><strong>Basic Premium Net:</strong></td><td><span id="basic-premiumnettext"></span></td>';
            html += '<td align="right"><strong>Basic Premium Gross:</strong></td><td><span id="basic-premiumgrosstext"></span></td></tr>';
            html += '</table>';
            html += '<br />';

            html += '<table cellpadding=4 WIDTH=70%>';
            html += '<tr><td colspan="4"><input id="netcomputationbutton" type="button" value="Net Computation" class="btn btn-default" />&nbsp;<input id="grosscomputationbutton" type="button" value="Gross Computation" class="btn btn-default" /></td></tr>';
            html += '</table>';

            html += '<table>';
            html += '<tr><td><strong>Remarks</strong></td><td colspan="3"><textarea id="remarks" cols="50" name="S1" rows="2"></textarea></td></tr>';
            html += '</table>';
            $('#perils-table').html(html);
            handledefaultvalue(obj.PerilDefaults);
            populatevtpls(obj.Tariff, ids);
            handlebindingofthefttextboxkeyup();
            initializeinputmasks();
            $('#computebutton').click(handlecompute);
            //disablevtplrates();
            $('#netcomputationbutton').click(handleshowcomputation);
            $('#grosscomputationbutton').click(handleshowcomputation);
        },
        error: function () {

        }
    });
}

function populatevtpls(tariffrates, ids) {
    $.each(ids, function (key, value) {
        var html = '';
        html += '<option value="0">-- SELECT --</option>';
        $.each(tariffrates.DropdownValues[value], function (resultKey, resultValue) {
            html += '<option value="' + resultValue.Value + '" >' + parseFloat(resultValue.Text).toFixed(2) + '</option>';
        });
        $('#' + associatedcontrols[value].limitsi).html(html);
    });

    insertctpldefault(tariffrates);

    $('#vtplbodilydropdown').unbind('change', vtplbodyinjurychange);
    $('#vtplbodilydropdown').change(vtplbodyinjurychange);
    $('#vtplpropertydropdown').unbind('change', vtplpropertychange);
    $('#vtplpropertydropdown').change(vtplpropertychange);
}

function insertctpldefault(obj) {
    var id = 187;
    var ctpldefault = parseFloat(obj.CTPLDefault);
    $('#' + associatedcontrols[id].premium).html(ctpldefault);
    $('#' + associatedcontrols[id].policypremium).html(ctpldefault);
}

function getperildefaultbykey(list, key) {
    var pd;
    $.each(list, function (k, v) {
        if (v.PerilID == key) {
            pd = v;
            return false;
        }
    });
    return pd;
}

///This is used for the edit mode
function displayperilsedit(json, remarks, tariffrates) {
    var selectedType = $('#TypeOfCoverDropdown').val();
    var transactionId = $('#IdHiddenField').val();

    var html = '<table class="computation-table" cellpadding=4 WIDTH=70%>';
    html += '<tr>';
    html += '<th>Coverage/Perils</th>';
    html += '<th>Limit/SI</th>';
    html += '<th>Rate</th>';
    html += '<th>Premium</th>';
    html += '<th>Policy Rate</th>';
    html += '<th>Policy Premium</th>';
    html += '</tr>';
    if (json != null) {

        $.each(json, function (key, value) {
            html += '<tr>';
            html += '<td>';
            html += value.PerilName;
            html += "<input id='" + associatedcontrols[value.PerilID].data + "' type='hidden' value='" + JSON.stringify(value.PerilID) + "' />";
            html += '</td>';

            html += '<td>';
            html += handlelimitsicontrol(value.PerilID);
            html += '</td>';

            html += '<td>';
            html += '<input class="fixed-width-100" id="' + associatedcontrols[value.PerilID].rate + '" type="text" />';
            html += '</td>';

            html += '<td>';
            html += createpremiumtemplate(value.Premium, value.PerilID);
            html += '</td>';

            html += '<td>';
            html += '<input class="fixed-width-100" id="' + associatedcontrols[value.PerilID].policyrate + '" type="text" />';
            html += '</td>';

            html += '<td>';
            html += createpolicypremiumtemplate(value.PolicyPremium, value.PerilID);
            html += '</td>';

            html += '</tr>';
        });

    }
    html += '<tr><td colspan="2"><input id="computebutton" type="button" class="btn btn-default" value="Compute" /></td>';
    html += '<td align="right"><strong>Basic Premium Net:</strong></td><td><span id="basic-premiumnettext"></span></td>';
    html += '<td align="right"><strong>Basic Premium Gross:</strong></td><td><span id="basic-premiumgrosstext"></span></td></tr>';
    html += '</table>';
    html += '<br />';

    html += '<table cellpadding=4 WIDTH=70%>';
    html += '<tr><td colspan="4"><input id="netcomputationbutton" type="button" value="Net Computation" class="btn btn-default" />&nbsp;<input id="grosscomputationbutton" type="button" value="Gross Computation" class="btn btn-default" /></td></tr>';
    html += '</table>';

    html += '<table>';
    html += '<tr><td><strong>Remarks</strong></td><td colspan="3"><textarea id="remarks" cols="50" name="S1" rows="2">'+ remarks +'</textarea></td></tr>';
    html += '</table>';
    $('#perils-table').html(html);
    handlebindingofthefttextboxkeyup();
    initializeinputmasks();
    //handledefaultvalue();
//    populatevtplcontrolswithcallback(
//    function () {
//        if (json != null) {
//            $.each(json, function (key, value) {
//                if (parseInt(value.PerilID) === 194) {
//                    $('#' + associatedcontrols[value.PerilID].limitsi).val(parseInt(value.NewPremium));
//                    $('#' + associatedcontrols[value.PerilID].premium).html(parseFloat(value.NewPremium).toFixed(2));
//                    $('#' + associatedcontrols[value.PerilID].policypremium).html(parseFloat(value.NewPolicyPremium).toFixed(2));
//                }
//            });
//        }
//    },
//    function () {
//        if (json != null) {
//            $.each(json, function (key, value) {
//                if (parseInt(value.PerilID) !== 194 && parseInt(value.PerilID) !== 195) {
//                    //NOTE:  blur() is called to trigger the formatting to currency
//                    $('#' + associatedcontrols[value.PerilID].limitsi).val(value.NewLimitSI);
//                    $('#' + associatedcontrols[value.PerilID].limitsi).blur();
//                    $('#' + associatedcontrols[value.PerilID].rate).val(value.NewRate);
//                    $('#' + associatedcontrols[value.PerilID].rate).blur();
//                    $('#' + associatedcontrols[value.PerilID].premium).html(parseFloat(value.NewPremium).toFixed(2));
//                    $('#' + associatedcontrols[value.PerilID].policyrate).val(value.NewPolicyRate);
//                    $('#' + associatedcontrols[value.PerilID].policyrate).blur();
//                    $('#' + associatedcontrols[value.PerilID].policypremium).html(parseFloat(value.NewPolicyPremium).toFixed(2));
//                } else if (parseInt(value.PerilID) === 195) {

//                    $('#' + associatedcontrols[value.PerilID].limitsi).val(parseInt(value.NewPremium));
//                    $('#' + associatedcontrols[value.PerilID].premium).html(parseFloat(value.NewPremium).toFixed(2));
//                    $('#' + associatedcontrols[value.PerilID].policypremium).html(parseFloat(value.NewPolicyPremium).toFixed(2));
//                } else {
//                }
//            });
//            
//        }
//    });


    renderdropdown(tariffrates.DropdownValues[194], associatedcontrols[194].limitsi);
    renderdropdown(tariffrates.DropdownValues[195], associatedcontrols[195].limitsi);
    if (json != null && json != undefined) {
        $.each(json, function (key, value) {
            //            if (parseInt(value.PerilID) === 194) {
            //                $('#' + associatedcontrols[value.PerilID].limitsi).val(parseInt(value.NewPremium));
            //                $('#' + associatedcontrols[value.PerilID].premium).html(parseFloat(value.NewPremium).toFixed(2));
            //                $('#' + associatedcontrols[value.PerilID].policypremium).html(parseFloat(value.NewPolicyPremium).toFixed(2));
            //            }
            if (parseInt(value.PerilID) !== 194 && parseInt(value.PerilID) !== 195) {
                //NOTE:  blur() is called to trigger the formatting to currency
                $('#' + associatedcontrols[value.PerilID].limitsi).val(value.NewLimitSI);
                $('#' + associatedcontrols[value.PerilID].limitsi).blur();
                $('#' + associatedcontrols[value.PerilID].rate).val(value.NewRate);
                $('#' + associatedcontrols[value.PerilID].rate).blur();
                $('#' + associatedcontrols[value.PerilID].premium).html(parseFloat(value.NewPremium).toFixed(2));
                $('#' + associatedcontrols[value.PerilID].policyrate).val(value.NewPolicyRate);
                $('#' + associatedcontrols[value.PerilID].policyrate).blur();
                $('#' + associatedcontrols[value.PerilID].policypremium).html(parseFloat(value.NewPolicyPremium).toFixed(2));
            } else {

                $('#' + associatedcontrols[value.PerilID].limitsi).val(parseInt(value.NewPremium));
                $('#' + associatedcontrols[value.PerilID].premium).html(parseFloat(value.NewPremium).toFixed(2));
                $('#' + associatedcontrols[value.PerilID].policypremium).html(parseFloat(value.NewPolicyPremium).toFixed(2));
            }

            $('#' + associatedcontrols[value.PerilID].limitsi).prop('disabled', !value.LimitSIEditable);
            $('#' + associatedcontrols[value.PerilID].rate).prop('disabled', !value.RateEditable);
            if (value.RateShowTariffText) {
                $('#' + associatedcontrols[value.PerilID].rate).val('tariff');
            }
            $('#' + associatedcontrols[value.PerilID].policyrate).prop('disabled', !value.PolicyRateEditable);

            if (value.PolicyRateShowTariffText) {
                $('#' + associatedcontrols[value.PerilID].policyrate).val('tariff');
            }

        });
        handlecompute();
    }
    
    
    $('#computebutton').click(handlecompute);
    $('#netcomputationbutton').click(handleshowcomputation);
    $('#grosscomputationbutton').click(handleshowcomputation);
}

function renderdropdown(data, key) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(data, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#' + key).html(html);
}

function displayperilsdetails(json, remarks, computations, customer) {
    var selectedType = $('#TypeOfCoverDropdown').val();
    var transactionId = $('#IdHiddenField').val();
    var html = '<table class="computation-table" cellpadding=4 WIDTH=70%>';
    html += '<tr>';
    html += '<th>Coverage/Perils</th>';
    html += '<th>Limit/SI</th>';
    html += '<th>Rate</th>';
    html += '<th>Premium</th>';
    html += '<th>Policy Rate</th>';
    html += '<th>Policy Premium</th>';
    html += '</tr>';
    if (json != null) {

        $.each(json, function (key, value) {
            html += '<tr>';
            html += '<td>';
            html += value.PerilName;
            html += "";
            html += '</td>';

            html += '<td>';
            html += parseFloat(value.NewLimitSI).toFixed(2);
            html += '</td>';

            html += '<td>';
            html += parseFloat(value.NewRate).toFixed(2);
            html += '</td>';

            html += '<td>';
            html += parseFloat(value.NewPremium).toFixed(2);
            html += '</td>';

            html += '<td>';
            html += parseFloat(value.NewPolicyRate).toFixed(2);
            html += '</td>';

            html += '<td>';
            html += parseFloat(value.NewPolicyPremium).toFixed(2);
            html += '</td>';

            html += '</tr>';
        });

    }
    html += '<tr><td colspan="2"></td>';
    html += '<td align="right"><strong>Basic Premium Net:</strong></td><td><span id="basic-premiumnettext">' + computations.NetComputationDetails.BasicPremium + '</span></td>';
    html += '<td align="right"><strong>Basic Premium Gross:</strong></td><td><span id="basic-premiumgrosstext">' + computations.GrossComputationDetails.BasicPremium + '</span></td></tr>';
    html += '</table>';

    html += '<table cellpadding=4 WIDTH=70%>';
    html += '<tr><td colspan="4"><input id="netcomputationbutton" type="button" value="Net Computation" class="btn btn-default" />&nbsp;<input id="grosscomputationbutton" type="button" value="Gross Computation" class="btn btn-default" /></td></tr>';
    html += '</table>';

    html += '<strong>Remarks</strong>';
    html += '<div class="remarks-text">';
    html += remarks;
    html += '</div>';
    $('#perils-table').html(html);
    //$('#netcomputationbutton,#grosscomputationbutton').button();
    $('#netcomputationbutton').click(
                { computation: computations.NetComputationDetails,
                    covertype: customer.TypeOfCover
                },
                    handleshowcomputationdetails);
    $('#grosscomputationbutton').click(
                { computation: computations.GrossComputationDetails,
                    covertype: customer.TypeOfCover
                },
                    handleshowcomputationdetails);
}


function initializeinputmasks() {
    var selectedCovertype = $('#TypeOfCoverDropdown').val();
    $.each(computationmatrix[selectedCovertype].ids, function (key, value) {
        if (value !== 194 && value !== 195) {
            $('#' + associatedcontrols[value].limitsi).autoNumeric('init');
        }
        $('#' + associatedcontrols[value].rate).autoNumeric('init');
        $('#' + associatedcontrols[value].policyrate).autoNumeric('init');
    });
}

function handleshowcomputation() {
    //force recompute
    if (!handlecompute()) {
        return;
    }

    //compute the details
    var type = (this.id === "netcomputationbutton" ? "Net" : "Gross");

    populatecomputationdetails(type);
}

function handleshowcomputationdetails(event) {
    var computation = event.data.computation;
    var covertype = event.data.covertype;

                $('#lblBasicPremium').html(computation.BasicPremium);

                //Documentary stamps
                $('#lblDocStamps').html(computation.DocumentaryStamps.toFixed(2));

                //valueaddedtax
                $('#lblVat').html(computation.ValueAddedTax.toFixed(2));

                //local goverment tax
                $('#lblLgt').html(computation.LocalGovernmentTax.toFixed(2));

                //dst on coc - constant
                $('#lblDST').html(computation.DSTonCOC.toFixed(2));

                //lto interconnect - constant
                $('#lblLtoInter').html(computation.LTOInterconnectivity.toFixed(2));

                //total
                $('#lblTotalAmmountDue').html(computation.GrandTotal.toFixed(2));

                if (covertype == '1' || covertype == '2') {
                    $('#lto-row').css('display', 'table-row');
                    $('#dst-row').css('display', 'table-row');
                } else {
                    $('#lto-row').css('display', 'none');
                    $('#dst-row').css('display', 'none');
                }

                $("#computation-dialog").css('display', 'block');
                $("#computation-dialog").dialog({
                    modal: true,
                    height: 430,
                    width: 450,
                    resizable: false,
                    title: " Computation",
                    beforeClose: function (event, ui) {
                        //clearcomputationdetails();
                    }
                });

}

function clearcomputationdetails() {
    $('#lblBasicPremium').val('');
    $('#lblDocStamps').val('');
    $('#lblVat').val('');
    $('#lblLgt').val('');
    $('#lblDST').val('');
    $('#lblLtoInter').val('');
    $('#lblTotalAmmountDue').val('');
}

function disablevtplrates() {
    //rate
    $('#' + associatedcontrols[194].rate).val('tariff');
    $('#' + associatedcontrols[195].rate).val('tariff');
    $('#' + associatedcontrols[194].rate).attr('disabled', true);
    $('#' + associatedcontrols[195].rate).attr('disabled', true);
    //policy rate
    $('#' + associatedcontrols[194].policyrate).val('tariff');
    $('#' + associatedcontrols[195].policyrate).val('tariff');
    $('#' + associatedcontrols[194].policyrate).attr('disabled', true);
    $('#' + associatedcontrols[195].policyrate).attr('disabled', true);

}


function fixemptyinputs(covertype) {
    $.each(computationmatrix[covertype].ids, function (key, value) {
        if ($('#' + associatedcontrols[value].limitsi).val() == '') {
            $('#' + associatedcontrols[value].limitsi).val('0')
        }

        if ($('#' + associatedcontrols[value].rate).val() == '') {
            $('#' + associatedcontrols[value].rate).val('0')
        }

        if ($('#' + associatedcontrols[value].policyrate).val() == '') {
            $('#' + associatedcontrols[value].policyrate).val('0')
        }
    });
}

function validatepremiumrates(covertype) {
    var isvalid = true;
    $.each(computationmatrix[covertype].ids, function (key, value) {
        if (value != undefined && (value !== 194 || !value == 195)) {
            var premiumrate = $('#' + associatedcontrols[value].rate).val();
            if (parseFloat(premiumrate) > 0.4) {
                $('#' + associatedcontrols[value].rate).addClass('control-validation-error');
                $('#' + associatedcontrols[value].rate).attr('title', 'Value must not exceed 0.40.');
                isvalid = false;
            }

            var policyrate = $('#' + associatedcontrols[value].policyrate).val();
            if (parseFloat(policyrate) > 0.4) {
                $('#' + associatedcontrols[value].policyrate).addClass('control-validation-error');
                $('#' + associatedcontrols[value].policyrate).attr('title', 'Value must not exceed 0.40.');
                isvalid = false;
            }
        }
    });
    return isvalid;
}

function validatelimitsi(currentselectedcovertype) {

    var isvalid = true;
    //Ensure that this validation will only fire for the valid
    //cover types
    if (parseInt(currentselectedcovertype) === 1) {
        return isvalid;
    }

    //NOTE: Harcoding the values since this will only validate specific controls
    $.each(limitsivalidationcontrols, function (key, value) {
        if (parseFloat($('#' + value).autoNumeric('get')) > 2600000) {
            $('#' + value).attr('title', 'Value must not exceed 2,600,000.');
            $('#'+value).addClass('control-validation-error');
            isvalid = false;
        }
    });

    return isvalid;
}

function resetlimitsivalidations() {
    $.each(limitsivalidationcontrols, function (key, value) {
        $('#'+value).removeClass('control-validation-error');
        $('#'+value).attr('title', '');
    });
}

function resetcomputationvalidations(covertype) {
    $.each(computationmatrix[covertype].ids, function (key, value) {
        if (value != undefined && (value !== 194 || !value == 195)) {
            $('#' + associatedcontrols[value].rate).removeClass('control-validation-error');
            $('#' + associatedcontrols[value].rate).attr('title','');
            $('#' + associatedcontrols[value].policyrate).removeClass('control-validation-error');
            $('#' + associatedcontrols[value].policyrate).attr('title','');
        }
    });
}

function handlecompute() {
    var tooltip = null;
    var currentselectedcovertype = $('#TypeOfCoverDropdown').val();
    resetcomputationvalidations(currentselectedcovertype);
    resetlimitsivalidations();
    fixemptyinputs(currentselectedcovertype);
    var ispremiumratesvalid = validatepremiumrates(currentselectedcovertype);
    var islimitsivalid = validatelimitsi(currentselectedcovertype);

    if (!ispremiumratesvalid || !islimitsivalid) {
        tooltip = $(document).tooltip();
        return false;
    }

    if (tooltip !== null) {
        $(document).tooltip('destroy');
    }
    var totalpremium = 0;
    var totalpolicypremium = 0;
    $.each(computationmatrix[currentselectedcovertype].ids, function (key, value) {
        if (value != undefined) {
            
            //for premium column
            if (value !== 194 && value !== 195 && value !== 187) {
                
                var plimitsi = $('#' + associatedcontrols[value].limitsi).autoNumeric('get');
                var prate = $('#' + associatedcontrols[value].rate).autoNumeric('get');
                if (plimitsi == null || plimitsi == undefined) {
                    plimitsi = 0;
                }

                if (prate == null || prate == undefined) {
                    prate = 0;
                }

                var p = parseFloat(plimitsi) * parseFloat(prate) / 100;
                $('#' + associatedcontrols[value].premium).html(parseFloat(p).toFixed(2));


                //for policy premium column
                var pclimitsi = (value === 194 || value === 195) ? $('#' + associatedcontrols[value].limitsi).val() : $('#' + associatedcontrols[value].limitsi).autoNumeric('get');
                var pcpolicyrate = $('#' + associatedcontrols[value].policyrate).autoNumeric('get');

                if (pclimitsi == null || pclimitsi == undefined) {
                    pclimitsi = 0;
                }

                if (pcpolicyrate == null || pcpolicyrate == undefined) {
                    pcpolicyrate = 0;
                }

                var x = parseFloat(pclimitsi) * parseFloat(pcpolicyrate) / 100;
                $('#' + associatedcontrols[value].policypremium).html(parseFloat(x).toFixed(2));

            }

            //total the premium
            var premium = $('#' + associatedcontrols[value].premium).html();
            totalpremium += parseFloat(premium);

            //total the policypremium
            var policypremium = $('#' + associatedcontrols[value].policypremium).html();
            totalpolicypremium += parseFloat(policypremium);
        }
    });

    $('#basic-premiumnettext').html(parseFloat(totalpremium).toFixed(2));
    $('#basic-premiumgrosstext').html(parseFloat(totalpolicypremium).toFixed(2));
    return true;
}

function populatecomputationdetails(type) {
    var premium = parseFloat($('#basic-premiumnettext').html());
    var gross = parseFloat($('#basic-premiumgrosstext').html());
    var covertype = $('#TypeOfCoverDropdown').val();

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: { "action": 'computedetails', "basicpremiumnet": premium, "basicpremiumgross": gross, "covertype": covertype },
        success: function (result) {
            var obj = JSON.parse(result);
            if (obj != null) {

                $('#lblBasicPremium').html(type == "Net" ? premium : gross);

                //Documentary stamps
                var docStamps = (type == "Net" ? obj.NetComputationDetails.DocumentaryStamps : obj.GrossComputationDetails.DocumentaryStamps);
                $('#lblDocStamps').html(docStamps.toFixed(2));

                //valueaddedtax
                var vat = (type == "Net" ? obj.NetComputationDetails.ValueAddedTax : obj.GrossComputationDetails.ValueAddedTax);
                $('#lblVat').html(vat.toFixed(2));

                //local goverment tax
                var lgt = (type == "Net" ? obj.NetComputationDetails.LocalGovernmentTax : obj.GrossComputationDetails.LocalGovernmentTax);
                $('#lblLgt').html(lgt.toFixed(2));

                //dst on coc - constant
                var dst = (type == "Net" ? obj.NetComputationDetails.DSTonCOC : obj.GrossComputationDetails.DSTonCOC);
                $('#lblDST').html(dst.toFixed(2));

                //lto interconnect - constant
                var lto = (type == "Net" ? obj.NetComputationDetails.LTOInterconnectivity : obj.GrossComputationDetails.LTOInterconnectivity);
                $('#lblLtoInter').html(lto.toFixed(2));

                //total
                var total = (type == "Net" ? obj.NetComputationDetails.GrandTotal : obj.GrossComputationDetails.GrandTotal);
                $('#lblTotalAmmountDue').html(total.toFixed(2));

                if (covertype == '1' || covertype == '2') {
                    $('#lto-row').css('display', 'table-row');
                    $('#dst-row').css('display', 'table-row');
                } else {
                    $('#lto-row').css('display', 'none');
                    $('#dst-row').css('display', 'none');
                }

                $("#computation-dialog").css('display', 'block');
                $("#computation-dialog").dialog({
                    modal: true,
                    height: 430,
                    width: 450,
                    resizable: false,
                    title: type + " Computation",
                    beforeClose: function (event, ui) {
                        clearcomputationdetails();
                    }
                });
            }
        },
        error: function () {

        }
    });
}

function createpremiumtemplate(currentPremium, id) {
    return '<span id="' + associatedcontrols[id].premium + '">' + parseFloat(currentPremium).toFixed(2) + '</span>';
}

function createpolicypremiumtemplate(currentPremium, id) {
    return '<span id="' + associatedcontrols[id].policypremium + '">' + parseFloat(currentPremium).toFixed(2) + '</span>';
}


function vtplbodyinjurychange() {
    var premiumValue = $('#' + associatedcontrols[194].limitsi).val();
    $('#' + associatedcontrols[194].premium).html(parseFloat(premiumValue).toFixed(2));
    $('#' + associatedcontrols[194].policypremium).html(parseFloat(premiumValue).toFixed(2));
}

function vtplpropertychange() {
    var premiumValue = $('#' + associatedcontrols[195].limitsi).val();
    $('#' + associatedcontrols[195].premium).html(parseFloat(premiumValue).toFixed(2));
    $('#' + associatedcontrols[195].policypremium).html(parseFloat(premiumValue).toFixed(2));
}

//function populatevtplcontrols() {
//    buildvtplcontrols([194, 195]);
//    $('#vtplbodilydropdown').unbind('change', vtplbodyinjurychange);
//    $('#vtplbodilydropdown').change(vtplbodyinjurychange);
//    $('#vtplpropertydropdown').unbind('change', vtplpropertychange);
//    $('#vtplpropertydropdown').change(vtplpropertychange);
//}

//function populatevtplcontrolswithcallback(callback194, callback195) {
//    buildvtplcontrols([194, 195], function () {
//        if (callback194 != null && callback194 != undefined) { callback194(); }
//        if (callback195 != null && callback195 != undefined) { callback195(); }
//    });
//    $('#vtplbodilydropdown').unbind('change', vtplbodyinjurychange);
//    $('#vtplbodilydropdown').change(vtplbodyinjurychange);
////    buildvtplcontrols(195, associatedcontrols[195].limitsi, callback195);
//    $('#vtplpropertydropdown').unbind('change', vtplpropertychange);
//    $('#vtplpropertydropdown').change(vtplpropertychange);
//}

function handlelimitsicontrol(id) {
    if (id === 194 || id === 195) {
        return '<select id="' + associatedcontrols[id].limitsi + '" class="fixed-width-150" ></select>';
    } else {
        return '<input id="' + associatedcontrols[id].limitsi + '" type="text" class="fixed-width-150"  />';
    }
    return '';
}

//function buildvtplcontrols(ids,callback) {
//    var subline = $('#SublineDropdown').val();
//    var motortype = $('#MotorTypeDropdown').val();

//    $.ajax({
//        url: "ajax/TransactionAjax.aspx",
//        type: "post",
//        data: { "action": 'gettariffrates', "subline": subline, "motortype": motortype, "ids": JSON.stringify(ids) },
//        success: function (result) {
//            var obj = JSON.parse(result);
//            if (obj != null) {
//                //populating the two dropdown
//                $.each(ids, function (key, value) {
//                    var html = '';
//                    html += '<option value="0">-- SELECT --</option>';
//                    $.each(obj.DropdownValues[value], function (resultKey, resultValue) {
//                        html += '<option value="' + resultValue.Value + '" >' + parseFloat(resultValue.Text).toFixed(2) + '</option>';
//                    });
//                    $('#' + associatedcontrols[value].limitsi).html(html);
//                });               

//                if (callback != null && callback != undefined) {
//                    callback();
//                }
//            }
//        },
//        error: function () {

//        }
//    });
//    //return html;
//}

function handledefaultvalue(defaults) {
    var type = $('#TypeOfCoverDropdown').val();

    $.each(computationmatrix[type].ids, function (k, v) {
        var pd = getperildefaultbykey(defaults, v);
        if (pd != undefined || pd != null) {
            $('#' + associatedcontrols[v].limitsi).val(pd.LimitSIDefault);
            $('#' + associatedcontrols[v].limitsi).prop('disabled', !pd.LimitSIEditable);
            $('#' + associatedcontrols[v].rate).val(pd.RateDefault);
            $('#' + associatedcontrols[v].rate).prop('disabled', !pd.RateEditable);
            if (pd.RateShowTariffText) {
                $('#' + associatedcontrols[v].rate).val('tariff');
            }
            $('#' + associatedcontrols[v].premium).html(pd.PremiumDefault);
            $('#' + associatedcontrols[v].policyrate).val(pd.PolicyRateDefault);
            $('#' + associatedcontrols[v].policyrate).prop('disabled', !pd.PolicyRateEditable);
            if (pd.PolicyRateShowTariffText) {
                $('#' + associatedcontrols[v].policyrate).val('tariff');
            }
            $('#' + associatedcontrols[v].policypremium).html(pd.PolicyPremiumDefault);
        }
    });
}

function handlebindingofthefttextboxkeyup() {
    var value = $('#TypeOfCoverDropdown').val();
    if (value == '2' || value == '3') {
        $('#thefttextbox').keyup(
            function (e) {
                $('#actsofnaturetextbox').val($('#thefttextbox').val());
                $('#striketextbox').val($('#thefttextbox').val());
            }
        );
    }
}