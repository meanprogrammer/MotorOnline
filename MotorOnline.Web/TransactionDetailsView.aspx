<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true"
    CodeBehind="TransactionDetailsView.aspx.cs" Inherits="MotorOnline.Web.TransactionDetailsView" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="head" runat="server">
    <script src="js/autoNumeric.js" type="text/javascript"></script>
    <script src="js/cardetails.js" type="text/javascript"></script>
    <script src="js/datehelper.js" type="text/javascript"></script>
    <script src="js/perils.js" type="text/javascript"></script>
    <script src="js/transaction.js" type="text/javascript"></script>
    <script src="js/endorsement.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(
        function () {
            $("#car-details").css('display', 'none');

            //$(':button,:submit').button();

            $('#PrintButton').click(handleprint);

            var id = $('#IdHiddenField').val();
            if (parseInt(id) > 0) {
//                $("#SaveButton").unbind("click");
//                $('#SaveButton').click(updatetransaction);

//                $("#SaveButton").prop('value', 'Update');

                //Showing a loader
                showloader('Loading Transaction ...');

                $.ajax({
                    url: "ajax/TransactionAjax.aspx",
                    type: "post",
                    data: { "action": 'gettransactionbyid', "transactionid": id },
                    success: function (result) {
                        var obj = JSON.parse(result);
                        if (obj != null) {
                            loadtransactiondetails(obj, id);

                        }
                    },
                    error: function () {
                    }
                });
            } else {
//                $("#SaveButton").unbind("click");
//                $('#SaveButton').click(savetransaction);
//                $("#SaveButton").prop('value', 'Save');
            }

            $('#PostButton').click(posttransaction);

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

    function posttransaction(e) { 
            e.preventDefault();
            var id = $('#IdHiddenField').val();
            $.ajax({
                url: "ajax/TransactionAjax.aspx",
                type: "post",
                data: { "action": 'posttransaction', "transactionid": id },
                success: function (result) {
                    if(result == '1') {
                        window.location.href = 'TransactionDetailsView.aspx?id=' + id;
                    }
                },
                error: function () {
                }
            });
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

        function toggleaddtionaltextbox(show) {
            var value = show ? 'block' : 'none';
            $('#CorporateMultipleLabel').css('display', value);
            $('#corporatemultiple').css('display', value);
        }
    </script>
</asp:Content>
<asp:Content ID="maincontent" ContentPlaceHolderID="Content" runat="server">
    <div>
        <table cellpadding="6">
            <tr>
                <td>
                    <strong>Crediting Branch:</strong>
                </td>
                <td>
                    <span id="creditingbranchlabel"></span></td>
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
                    &nbsp;</td>
                <td>
                    <strong>Policy Period From:</strong>
                </td>
                <td colspan="2" valign="middle">
                    <span id="policyperiodfrom"></span></td>
                <td colspan="2" valign="middle">
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Policy No.:</strong>
                </td>
                <td>
                    <asp:Label ID="lblPolicyNo" runat="server" Text="Label" ClientIDMode="Static"></asp:Label>
                </td>
                <td>
                    <strong>Policy Period To:</strong>
                </td>
                <td class="style1" colspan="2">
                    <span id="policyperiodto"></span>
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
                    <span id="businesstype"></span></td>
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
                    <span id="subline"></span></td>
            </tr>
            <tr>
                <td>
                    <strong>Mortgagee:</strong>
                </td>
                <td>
                    <span id="mortgagee"></span></td>
            </tr>
            <tr>
                <td>
                    <strong>Intermediary:</strong>
                </td>
                <td>
                    <span id="intermediary"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Type of insurance</strong>
                </td>
                <td>
                    <span id="typeofinsurance"></span>
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
                    <span id="designation"></span>
                </td>
                <td>
                    <strong>Last Name:</strong>
                </td>
                <td>
                    <span id="lastname"></span>
                </td>
                <td>
                    <strong>First Name:</strong>
                </td>
                <td>
                    <span id="firstname"></span></td>
                <td>
                    <strong>M.I.:</strong>
                </td>
                <td>
                    <span id="middlename"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Address:</strong>
                </td>
                <td>
                    <span id="address"></span></td>
                <td>
                    <strong>Telephone:</strong>
                </td>
                <td>
                    <span id="telephone"></span></td>
                <td>
                    <strong>Mobile No.:</strong>
                </td>
                <td>
                    <span id="mobileno"></span></td>
                <td>
                    <strong>Email Address:</strong>
                </td>
                <td>
                    <span id="emailaddress"></span></td>
            </tr>
            <tr>
                <td>
                    <strong>
                        <asp:Label ID="CorporateMultipleLabel" ClientIDMode="Static" runat="server" Text="Label"></asp:Label></strong>
                </td>
                <td>
                    <span id="corporatemultiple"></span></td>
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
 
        <%--<div id="car-details">
            <table class="table-vertical-align-td" border="0">
                <tr>
                    <td>
                        Type of Body
                    </td>
                    <td>
                        <asp:DropDownList ID="TypeOfBodyDropdown" ClientIDMode="Static" runat="server" Width="120px">
                        </asp:DropDownList>
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
                        Authentication No.</td>
                    <td>
                        <asp:TextBox ID="txtAuthenticationNo" ClientIDMode="Static" runat="server"></asp:TextBox>
                        </td>
                    <td>
                        COC No.</td>
                    <td>
                        <asp:TextBox ID="txtCOCNo" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" colspan="4">
                        <asp:Button ID="SaveCarDetailsButton" ClientIDMode="Static" runat="server" Text="Save" />
                    </td>
                </tr>
            </table>
            <span id="validation-message"></span>
        </div>--%>
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
                    <strong>Authentication No:</strong></td>
                <td class="label-value">
                    <asp:Label ID="lblAuthenticationNo" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                <td class="label-text">
                    <strong>COC No:</strong></td>
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
         <div id="endorsement-dialog" style="display:none;">
            <table cellpadding="8" width="100%">
                <tr>
                    <td>
                        Type of Endorsement</td>
                    <td>
                        <asp:DropDownList ID="EndorsementDropdown" ClientIDMode="Static" runat="server">
                            <asp:ListItem Value="0" Text="-- SELECT --"></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    <td><span class="required-field">*</span></td>
                </tr>
                <tr>
                    <td>
                        Effectivity date</td>
                    <td>
                        <input id="effectivitydate" type="text" /></td>
                    <td><span class="required-field">*</span></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <textarea id="endorsementtext" cols="60" 
                            name="endorsementtext" rows="10"></textarea></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="endorsement-controls">
                        </div>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <input id="endsavebutton" type="button" value="Save" />&nbsp;
                        <input id="endcancelbutton" type="button" value="Cancel" />
                        </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
        <hr />
        <table style="width: 500px">
            <tr>
                <td>
                    <asp:Button ID="CancelButton" runat="server" Text="Cancel" class="btn btn-default" />
                    <%--<asp:Button ID="SaveButton" ClientIDMode="Static" runat="server" Text="Save" class="btn btn-primary" />&nbsp;--%>
                    <asp:Button ID="PrintButton" ClientIDMode="Static" runat="server" Text="Print" class="btn btn-success" />
                    <asp:Button ID="PostButton" runat="server" ClientIDMode="Static" Text="Post" class="btn btn-default" />

                    <% if (this.CurrentUser.UserRole.CanEndorse) { %>
                        <input id="endorsebutton" type="button" value="Endorse" class="btn btn-default" />
                    <% } %>
                </td>
            </tr>
        </table>
    </div>
    <input id="CarDetailsHidden" type="hidden" />
    <input id="TransactionInformationHidden" type="hidden" />
    <input id="CustomerInfo" type="hidden" />
    <input id="pagetypehidden" type="hidden" value="detail" />
    <asp:HiddenField ID="IdHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="NamesAutocompleteHiddenField" ClientIDMode="Static" runat="server" />
    <div id="validation-messages">
    </div>
    <div id="progressbox">
        <h3 id="progress-message">
            Now Loading ...</h3>
    </div>
</asp:Content>
