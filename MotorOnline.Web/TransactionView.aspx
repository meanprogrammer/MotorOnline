<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true"
    CodeBehind="TransactionView.aspx.cs" Inherits="MotorOnline.Web.TransactionView" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="head" runat="server">
    <script src="js/autoNumeric.js" type="text/javascript"></script>
    <script src="js/cardetails.js" type="text/javascript"></script>
    <script src="js/datehelper.js" type="text/javascript"></script>
    <script src="js/perils.js" type="text/javascript"></script>
    <script src="js/transaction.js" type="text/javascript"></script>
    <script src="js/endorsement.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getexistingpolicy() {
            var engineNo = $('#txtEngine').val();
            if (engineNo.length <= 0) {
                return;
            }
            $.ajax({
                url: "ajax/TransactionAjax.aspx",
                type: "post",
                data: { "action": 'getexistingpolicy', "engineid": engineNo },
                success: function (result) {
                    var obj = JSON.parse(result);
                    if (obj != null) {
                        if (obj.length > 0) {
                            var msgs = 'The specified Engine # already has existing policy(ies) with status of ';
                            $.each(obj, function (key, value) {
                                msgs += value + ', ';
                            });
                            msgs += '.';
                            msgs += 'This transaction is no longer valid and will be canceled. Click OK';
                            alert(msgs);
                            document.location = 'TransactionView.aspx';
                        }
                    }
                },
                error: function () {

                }
            });
        }

        function cardetailsave() {
            if (!validatecardetails()) {
                return;
            }
            var cardetail = createcardetails();
            populatecardetaildisplay(cardetail);

            displayperils();

            $('#CarDetailsHidden').val(JSON.stringify(cardetail));
            $('#car-details').dialog('close');
        }

        function validatecardetails() {
            resetcardetailsvalidation();
            var isvalid = true;

            if ($('#TypeOfCoverDropdown').val() == "0") {
                isvalid = false;
                $('#TypeOfCoverDropdown').addClass('control-validation-error');
            }

            if ($('#CarCompaniesDropdown').val() == "0") {
                isvalid = false;
                $('#CarCompaniesDropdown').addClass('control-validation-error');
            }

            if ($('#CarMakeDropdown').val() == "0") {
                isvalid = false;
                $('#CarMakeDropdown').addClass('control-validation-error');
            }

            if ($('#CarSeriesDropdown').val() == "0") {
                isvalid = false;
                $('#CarSeriesDropdown').addClass('control-validation-error');
            }

            if ($('#EngineDropdown').val() == "0") {
                isvalid = false;
                $('#EngineDropdown').addClass('control-validation-error');
            }

            if ($('#MotorTypeDropdown').val() == "0") {
                isvalid = false;
                $('#MotorTypeDropdown').addClass('control-validation-error');
            }

            if ($('#YearDropdown').val() == "0") {
                isvalid = false;
                $('#YearDropdown').addClass('control-validation-error');
            }

            return isvalid;
        }

        function resetcardetailsvalidation() {
            $('#TypeOfCoverDropdown').removeClass('control-validation-error');
            $('#CarCompaniesDropdown').removeClass('control-validation-error');
            $('#CarMakeDropdown').removeClass('control-validation-error');
            $('#CarSeriesDropdown').removeClass('control-validation-error');
            $('#EngineDropdown').removeClass('control-validation-error');
            $('#MotorTypeDropdown').removeClass('control-validation-error');
            $('#YearDropdown').removeClass('control-validation-error');
        }

        function carcompanychange(event) {
            var carcompanydropdownid = event.data.carcompanydropdownid;
            var carmakedropdownid = event.data.carmakedropdownid;
            var enginedropdownid = event.data.enginedropdownid;
            
            var selectedCarCompany = $('#' + carcompanydropdownid).val();

            if (parseInt(selectedCarCompany) <= 0) {
                $('#' + carmakedropdownid).html('');
                $('#' + enginedropdownid).html('');
                return;
            }

            $.ajax({
                url: "ajax/TransactionAjax.aspx",
                type: "post",
                data: { "action": 'filtercarmake', "compid": selectedCarCompany },
                success: function (result) {
                    var obj = JSON.parse(result);
                    if (obj != null) {
                        var html = '<option value="0">-- SELECT --</option>';
                        $.each(obj, function (key, value) {
                            html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                        });
                        //
                        $('#' + carmakedropdownid).html(html);
                    }

                },
                error: function () {

                }
            });
        }

        function carcompanychangewithcallback(callback) {
            var selectedCarCompany = $('#CarCompaniesDropdown').val();

            if (parseInt(selectedCarCompany) <= 0) {
                $('#CarMakeDropdown').html('');
                $('#EngineDropdown').html('');
                return;
            }

            $.ajax({
                url: "ajax/TransactionAjax.aspx",
                type: "post",
                data: { "action": 'filtercarmake', "compid": selectedCarCompany },
                success: function (result) {
                    var obj = JSON.parse(result);
                    if (obj != null) {
                        var html = '<option value="0">-- SELECT --</option>';
                        $.each(obj, function (key, value) {
                            html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                        });
                        //
                        $('#CarMakeDropdown').html(html);
                        callback();
                    }

                },
                error: function () {

                }
            });
        }

        $(document).ready(
        function () {
            $('#PeriodFromTextbox').datepicker({
                showOn: "button",
                buttonImage: "images/Calendar-icon.png",
                buttonImageOnly: true,
                onSelect: function () {
                    handlecalendarselect('PeriodFromTextbox', 'PeriodToTextbox');
                }
            });
            $("#car-details").css('display', 'none');
            toggleaddtionaltextbox(false);
            $('#InputCarDetailsButton').click(
                inputcardetailsbuttonclick
            );

            $('#TypeOfInsuranceDropdown').change(function () {
                var selectedValue = $('#TypeOfInsuranceDropdown').val();
                //multiname
                if (selectedValue == 2) {
                    $('#CorporateMultipleLabel').html('Multiple Name');
                    toggleaddtionaltextbox(true);
                } else if
                //corporate
                (selectedValue == 3) {
                    $('#CorporateMultipleLabel').html('Corporate Name');
                    toggleaddtionaltextbox(true);
                } else {
                    toggleaddtionaltextbox(false);
                }
            });

            $('#CarCompaniesDropdown').change({
                carcompanydropdownid: 'CarCompaniesDropdown',
                carmakedropdownid: 'CarMakeDropdown',
                enginedropdownid: 'EngineDropdown'
            }, carcompanychange);
            $('#CarMakeDropdown').change({
                carcompanydropdownid: 'CarCompaniesDropdown',
                carmakedropdownid: 'CarMakeDropdown',
                enginedropdownid: 'EngineDropdown'
            }, carmakechange);

            $('#SaveCarDetailsButton').click(cardetailsave);
            $(':button,:submit').button();

            $('#PrintButton').click(handleprint);

            // $('#NamesAutocompleteHiddenField').val()

            var data = $('#NamesAutocompleteHiddenField').val();

            $('#txtLastName').autocomplete({
                source: JSON.parse(data),
                select: function (event, ui) {
                    $('#txtFirstName').val(ui.item.FirstName);
                    $('#txtMI').val(ui.item.MiddleName);
                    $('#txtMailAdress').val(ui.item.Address);
                    $('#txtTelephone').val(ui.item.Telephone);
                    $('#txtMobileNo').val(ui.item.MobileNo);
                    $('#txtEmailAdd').val(ui.item.Email);
                    $('#ddDesignation').val(ui.item.Designation);
                    $('#CustomerInfo').val(ui.item.CustomerID);
                }
            });

            $('#txtEngine').blur(getexistingpolicy);

            var id = $('#IdHiddenField').val();
            if (parseInt(id) > 0) {
                $("#SaveButton").unbind("click");
                $('#SaveButton').click(updatetransaction);

                $("#SaveButton").prop('value', 'Update');

                //Showing a loader
                showloader('Loading Transaction ...');

                $.ajax({
                    url: "ajax/TransactionAjax.aspx",
                    type: "post",
                    data: { "action": 'gettransactionbyidextended', "transactionid": id },
                    success: function (result) {
                        var obj = JSON.parse(result);
                        if (obj != null) {

                            loadtransaction(obj, id);
                            if (obj.Transaction.HasEndorsement == true) {
                                $('#printendorsement').css('display', 'inline');
                            }

                            if (obj.Transaction.IsEndorsed == true) {
                                $('#endorsebutton').css('display', 'none');
                            } else {
                                if (obj.Transaction.IsPosted == false) {
                                    $('#endorsebutton').css('display', 'none');
                                }
                            }
                        }
                    },
                    error: function () {
                    }
                });
            } else {
                $("#SaveButton").unbind("click");
                $('#SaveButton').click(savetransaction);
                $("#SaveButton").prop('value', 'Save');
                $('#PrintButton,#PostButton,#endorsebutton').css('display', 'none');
            }



            //Endorsement Section
            $('#endorsebutton').click(
                function () {
                    $("#endorsement-dialog").css('display', 'block');
                    $("#endorsement-dialog").dialog({
                        modal: true,
                        height: 600,
                        width: 600,
                        resizable: false,
                        closeOnEscape: false,
                        title: "Endorsement"
                    });
                }
            );

            $('#endcancelbutton').click(
                function () {
                    $('#endorsement-dialog').dialog('close');
                    $("#endorsement-dialog").css('display', 'none');
                }
            );

            $('#EndorsementDropdown').change(onendorsementselectchanged);
            $('#effectivitydate').datepicker();

            $('#printendorsement').click(
                function () {
                    var tid = $('#IdHiddenField').val();
                    var url = 'EndorsementPrintOut.aspx?id=' + tid;
                    window.open(url, "_blank", 'toolbar=0,location=0,menubar=0');
                }
            );

            //Endorsement Section

        }
    );

        function togglecardetailscontrols(enabled) {
            $('#TypeOfBodyDropdown').attr('disabled', enabled);
            $('#YearDropdown').attr('disabled', enabled);
            $('#TypeOfCoverDropdown').attr('disabled', enabled);
            $('#CarCompaniesDropdown').attr('disabled', enabled);
        }

        function carmakechange(event) {
            var carcompanydropdownid = event.data.carcompanydropdownid;
            var carmakedropdownid = event.data.carmakedropdownid;
            var enginedropdownid = event.data.enginedropdownid;

            var selectedMake = $('#'+carmakedropdownid).val();
            if (selectedMake === "0") {
                $('#'+enginedropdownid).html('');
                return;
            }

            var selectedCompany = $('#'+carcompanydropdownid).val();
            var ids = selectedMake.split("|");
            var carMakeId = ids[0];
            var carSeriedId = ids[1];
            $.ajax({
                url: "ajax/TransactionAjax.aspx",
                type: "post",
                data: { "action": 'filterengine', "makeid": carMakeId, "compid": selectedCompany, "seriesid": carSeriedId },
                success: function (result) {
                    var obj = JSON.parse(result);
                    if (obj != null) {
                        var html = '<option value="0">-- SELECT --</option>';
                        $.each(obj, function (key, value) {
                            html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                        });
                        //
                        $('#'+enginedropdownid).html(html);
                    }

                },
                error: function () {
                }
            });
        }

        function carmakechangewithcallback(callback) {

            var selectedMake = $('#CarMakeDropdown').val();
            if (selectedMake === "0") {
                $('#EngineDropdown').html('');
                return;
            }

            var selectedCompany = $('#CarCompaniesDropdown').val();
            var ids = selectedMake.split("|");
            var carMakeId = ids[0];
            var carSeriedId = ids[1];
            $.ajax({
                url: "ajax/TransactionAjax.aspx",
                type: "post",
                data: { "action": 'filterengine', "makeid": carMakeId, "compid": selectedCompany, "seriesid": carSeriedId },
                success: function (result) {
                    var obj = JSON.parse(result);
                    if (obj != null) {
                        var html = '<option value="0">-- SELECT --</option>';
                        $.each(obj, function (key, value) {
                            html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                        });
                        //
                        $('#EngineDropdown').html(html);
                        callback();
                    }

                },
                error: function () {
                }
            });
        }

        function toggleaddtionaltextbox(show) {
            var value = show ? 'block' : 'none';
            $('#CorporateMultipleLabel').css('display', value);
            $('#CorporateMultipleTextbox').css('display', value);
        }

        function inputcardetailsbuttonclick() {
            if (!validatebeforeaddingcardetails()) {
                alert('Select a Subline first.');
                return;
            }

            $("#car-details").dialog({
                modal: true,
                height: 300,
                width: 680,
                resizable: false,
                title: "Car Details"//,
                //close: function (event, ui) { alert('close'); }
            });
            //            var cardetailjson = $('#CarDetailsHidden').val();
            //            if (cardetailjson.length === 0) {
            //                loadcardetailsoptions();
            //            }
        }

        function handleprint() {
            var id = $('#IdHiddenField').val();
            if (id.length === 0) {
                return;
            } else {
                window.open('GeneratePrintOut.aspx?id=' + id, "_blank", 'toolbar=0,location=0,menubar=0');
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="maincontent" ContentPlaceHolderID="Content" runat="server">
    <form id="transactionform" runat="server">
    <div>
        <table cellpadding="6">
            <tr>
                <td>
                    <strong>Crediting Branch:</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCreditingBranch" ClientIDMode="Static" runat="server">
                    </asp:DropDownList>
                    <span class="required-field">*</span>
                </td>
                <td>
                    <strong>Date Created:</strong>
                </td>
                <td colspan="2">
                    <asp:Label ID="lblCurrentDate" runat="server" Text="Label" ClientIDMode="Static"></asp:Label>
                </td>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <strong>PAR NO.:</strong>
                </td>
                <td>
                    <asp:Label ID="lblParNo" runat="server" Text="Label" ClientIDMode="Static"></asp:Label>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td>
                    <strong>Policy Period From:</strong>
                </td>
                <td colspan="2" valign="middle">
                    <asp:TextBox ID="PeriodFromTextbox" ClientIDMode="Static" ReadOnly="true" runat="server"></asp:TextBox>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td colspan="2" valign="middle">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Policy No.:</strong>
                </td>
                <td>
                    <asp:Label ID="lblPolicyNo" runat="server" Text="Label" ClientIDMode="Static"></asp:Label>&nbsp;<span
                        class="required-field">*</span>
                </td>
                <td>
                    <strong>Policy Period To:</strong>
                </td>
                <td class="style1" colspan="2">
                    <asp:TextBox ID="PeriodToTextbox" runat="server" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                </td>
                <td class="style1" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Geniisys Policy #:</strong>
                </td>
                <td class="style15">
                    <asp:Label ID="lblGeniisysNo" runat="server" ClientIDMode="Static" Text="MC-PC-QC-13-00077745-03"></asp:Label>
                    &nbsp;
                </td>
                <td>
                    <strong>Businesss Type</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddBusinessType" runat="server" ClientIDMode="Static">
                        <asp:ListItem Value="NEW">NEW</asp:ListItem>
                        <asp:ListItem Value="RENEW">RENEW</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    <strong>Policy Status</strong>
                </td>
                <td>
                    <asp:Label ID="lblPolicyStatus" runat="server" Text="Label" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="6">
            <tr>
                <td>
                    <strong>SubLine:</strong>
                </td>
                <td>
                    <asp:DropDownList ID="SublineDropdown" runat="server" ClientIDMode="Static">
                    </asp:DropDownList>
                    &nbsp;<span class="required-field">*</span>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Mortgagee:</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMortgagee" runat="server" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Intermediary:</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddInterMediary" runat="server" ClientIDMode="Static">
                    </asp:DropDownList>
                    &nbsp;<span class="required-field">*</span>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Type of insurance</strong>
                </td>
                <td>
                    <asp:DropDownList ID="TypeOfInsuranceDropdown" ClientIDMode="Static" runat="server">
                    </asp:DropDownList>
                    &nbsp;<span class="required-field">*</span>
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="6">
            <tr>
                <td>
                    <strong>Designation:</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddDesignation" runat="server" ClientIDMode="Static">
                        <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                        <asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td>
                    <strong>Last Name:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" ClientIDMode="Static"></asp:TextBox>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td>
                    <strong>First Name:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" ClientIDMode="Static"></asp:TextBox>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td>
                    <strong>M.I.:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtMI" runat="server" ClientIDMode="Static"></asp:TextBox>
                    &nbsp;<span class="required-field">*</span>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Address:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtMailAdress" runat="server" ClientIDMode="Static"></asp:TextBox>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td>
                    <strong>Telephone:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtTelephone" runat="server" ClientIDMode="Static"></asp:TextBox>
                </td>
                <td>
                    <strong>Mobile No.:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtMobileNo" runat="server" ClientIDMode="Static"></asp:TextBox>
                    &nbsp;<span class="required-field">*</span>
                </td>
                <td>
                    <strong>Email Address:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtEmailAdd" runat="server" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>
                        <asp:Label ID="CorporateMultipleLabel" ClientIDMode="Static" runat="server" Text="Label"></asp:Label></strong>
                </td>
                <td>
                    <asp:TextBox ID="CorporateMultipleTextbox" runat="server" ClientIDMode="Static"></asp:TextBox>
                </td>
                <td style="text-align: right">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="text-align: right">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="text-align: right">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table border="0">
            <tr>
                <td>
                    <input id="InputCarDetailsButton" type="button" value="Input Car Details" class="btn btn-primary btn-md" />
                </td>
                <td>
                    <span id="cardetails-span">* Input car details.</span>
                </td>
            </tr>
        </table>
        <div id="car-details">
            <table class="table-vertical-align-td" border="0">
                <tr>
                    <td>
                        Type of Body
                    </td>
                    <td>
                        <asp:DropDownList ID="TypeOfBodyDropdown" ClientIDMode="Static" runat="server" Width="120px">
                        </asp:DropDownList>
                        <%--                    <asp:Button ID="NewButton" runat="server" Text="New" />&nbsp;--%>
                    </td>
                    <td>
                        Type Of Cover
                    </td>
                    <td>
                        <asp:DropDownList ID="TypeOfCoverDropdown" ClientIDMode="Static" runat="server" Width="200px">
                        </asp:DropDownList>
                        <span class="required-field">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Year
                    </td>
                    <td>
                        <asp:DropDownList ID="YearDropdown" ClientIDMode="Static" runat="server">
                        </asp:DropDownList>
                        <span class="required-field">*</span>
                    </td>
                    <td>
                        Car Company
                    </td>
                    <td>
                        <asp:DropDownList ID="CarCompaniesDropdown" ClientIDMode="Static" runat="server"
                            Width="200px">
                        </asp:DropDownList>
                        <span class="required-field">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Motor Type
                    </td>
                    <td>
                        <asp:DropDownList ID="MotorTypeDropdown" runat="server" ClientIDMode="Static">
                            <asp:ListItem Text="LIGHT" Value="LIGHT" />
                            <asp:ListItem Text="MEDIUM" Value="MEDIUM" />
                            <asp:ListItem Text="HEAVY" Value="HEAVY" />
                        </asp:DropDownList>
                        <span class="required-field">*</span>
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        <asp:DropDownList ID="CarMakeDropdown" Width="200px" ClientIDMode="Static" runat="server">
                        </asp:DropDownList>
                        <span class="required-field">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Engine No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtEngine" ClientIDMode="Static" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        Engine
                    </td>
                    <td>
                        <asp:DropDownList ID="EngineDropdown" Width="200px" ClientIDMode="Static" runat="server">
                        </asp:DropDownList>
                        <span class="required-field">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Color
                    </td>
                    <td>
                        <asp:TextBox ID="txtColor" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                        Chassis No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtChasis" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Conduction No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtConductionNo" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                        Plate No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlateNo" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        Accessories
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccesories" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Authentication No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtAuthenticationNo" ClientIDMode="Static" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        COC No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtCOCNo" ClientIDMode="Static" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="4">
                        <asp:Button ID="SaveCarDetailsButton" class="btn btn-primary" ClientIDMode="Static"
                            runat="server" Text="Save" />
                    </td>
                </tr>
            </table>
            <span id="validation-message"></span>
        </div>
        <br />
        <table width="70%" cellpadding="6" class="car-details-table">
            <tr>
                <td class="label-text fixed-width-150">
                    <strong>SubLine Type :</strong>
                </td>
                <td class="label-value fixed-width-250">
                    <asp:Label ID="lblSublineType" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text fixed-width-150">
                    <strong>Type Of Cover:</strong>
                </td>
                <td class="label-value fixed-width-250">
                    <asp:Label ID="lblTypeOfCover" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label-text">
                    <strong>Year :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblYear" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text">
                    <strong>Car Company:</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblCarCompany" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label-text">
                    <strong>Make :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblMake" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text">
                    <strong>Motor Type :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblMotorType" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label-text">
                    <strong>Engine No :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblEngineNo" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text">
                    <strong>Chassis No. :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblChasisNo" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label-text">
                    <strong>Color :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblColor" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text">
                    <strong>Plate No. :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblPlateNo" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label-text">
                    <strong>Conduction No.:</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblConductionNo" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text">
                    <strong>Accessories :</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblAccessories" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label-text">
                    <strong>Authentication No:</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblAuthenticationNo" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td class="label-text">
                    <strong>COC No:</strong>
                </td>
                <td class="label-value">
                    <asp:Label ID="lblCOCNo" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div id="perils-table">
        </div>
        <div id="computation-dialog" style="display: none;">
            <table cellpadding="4">
                <tr>
                    <td class="label-text" width="40%">
                        Basic Premium:
                    </td>
                    <td class="label-value" width="60%">
                        <asp:Label ID="lblBasicPremium" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label-text">
                        Documentary Stamps:
                    </td>
                    <td class="label-value">
                        <asp:Label ID="lblDocStamps" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label-text">
                        Value Added Tax
                    </td>
                    <td class="label-value">
                        <asp:Label ID="lblVat" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label-text">
                        Local Gov. Tax:
                    </td>
                    <td class="label-value">
                        <asp:Label ID="lblLgt" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr id="dst-row">
                    <td class="label-text">
                        DST on COC:
                    </td>
                    <td class="label-value">
                        <asp:Label ID="lblDST" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr id="lto-row">
                    <td class="label-text">
                        LTO interconnectivity:
                    </td>
                    <td class="label-value">
                        <asp:Label ID="lblLtoInter" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label-text">
                        Total amount due:
                    </td>
                    <td class="label-value">
                        <asp:Label ID="lblTotalAmmountDue" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="endorsement-dialog" style="display: none;">
            <table cellpadding="8" width="100%">
                <tr>
                    <td>
                        Type of Endorsement
                    </td>
                    <td>
                        <asp:DropDownList ID="EndorsementDropdown" ClientIDMode="Static" runat="server">
                            <asp:ListItem Value="0" Text="-- SELECT --"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="required-field">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Effectivity date
                    </td>
                    <td>
                        <input id="effectivitydate" type="text" />
                    </td>
                    <td>
                        <span class="required-field">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <textarea id="endorsementtext" cols="60" name="endorsementtext" rows="10"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="endorsement-controls">
                        </div>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input id="endsavebutton" type="button" value="Save" />&nbsp;
                        <input id="endcancelbutton" type="button" value="Cancel" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <table style="width: 550px">
            <tr>
                <td>
                    <asp:Button ID="CancelButton" class="btn btn-default" runat="server" Text="Cancel" />
                    <asp:Button ID="SaveButton" class="btn btn-primary" ClientIDMode="Static" runat="server"
                        Text="Save" />
                    <asp:Button ID="PrintButton" class="btn btn-success" ClientIDMode="Static" runat="server"
                        Text="Print" />
                    <asp:Button ID="PostButton" class="btn btn-default" ClientIDMode="Static" runat="server"
                        Text="Post" />
                    <% if (this.CurrentUser.UserRole.CanEndorse)
                       { %>
                    <input id="endorsebutton" type="button" class="btn btn-default" value="Endorse" />
                    <% } %>
                    <input id="printendorsement" type="button" class="btn btn-default" value="Print Endorsement"
                        style="display: none;" />
                </td>
            </tr>
        </table>
    </div>
    <input id="CarDetailsHidden" type="hidden" />
    <input id="TransactionInformationHidden" type="hidden" />
    <input id="CustomerInfo" type="hidden" />
    <asp:HiddenField ID="IdHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="NamesAutocompleteHiddenField" ClientIDMode="Static" runat="server" />
    <div id="validation-messages">
    </div>
    <div id="progressbox">
        <h3 id="progress-message">
            Now Loading ...</h3>
    </div>
    </form>
</asp:Content>
