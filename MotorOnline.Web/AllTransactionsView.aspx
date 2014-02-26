<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="AllTransactionsView.aspx.cs" Inherits="MotorOnline.Web.AllTransactionsView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/transactionsearch.js" type="text/javascript"></script>
    <script type="text/javascript">
        function secondinitialize() {
            initpaging();
            $('#rowsperpage').change(rowsperpagechange);
        }

        function rowsperpagechange() {
            initpaging();
            $('#rowcounthidden').val($('#rowsperpage').val());
            searchtransactions(null);
        }

        function initpaging() {
            $('#pagehidden').val('0');
        }

        function changepage(page) {
            $('#pagehidden').val(page);
            searchtransactions();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<table border="0" cellpadding="2">
    <tr>
        <td colspan="6"><strong style="font-size:large;">Search</strong></td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>Crediting Branch</td>
        <td><select id="creditingbranch" class="form-control input-sm"></select></td>
        <td>Par No:</td>
        <td><input id="parno" type="text" class="form-control input-sm" /></td>
        <td>Policy No:</td>
        <td><input id="policyno" type="text" class="form-control input-sm" /></td>
        <td>Subline</td>
        <td><select id="subline" name="subline" class="form-control input-sm"></select></td>
    </tr>
    <tr>
        <td>Date Created</td>
        <td>
            <input id="datecreated" type="text" class="form-control input-sm width-170 not-block" /></td>
        <td>
            Policy Period From</td>
        <td>
            <input id="policyperiodfrom" type="text" class="form-control input-sm width-170 not-block" /></td>
        <td>Policy Period To</td>
        <td>
            <input id="policyperiodto" type="text" class="form-control input-sm width-170 not-block" /></td>
        <td>Type of Cover</td>
        <td>
            <select id="typeofcover" name="typeofcover" class="form-control input-sm"></select></td>
    </tr>
    <tr>
        <td>Mortgagee</td>
        <td colspan="3">
            <select id="mortgagee" name="mortgagee" class="width-400 form-control input-sm"></select></td>
        <td>
            First Name</td>
        <td>
            <input id="firstname" type="text" class="form-control input-sm" /></td>
        <td>
            Last Name</td>
        <td>
            <input id="lastname" type="text" class="form-control input-sm" /></td>
    </tr>
    <tr>
        <td>Intermediary</td>
        <td colspan="3">
            <select id="intermediary" name="intermediary" class="width-400 form-control input-sm"></select></td>
        <td>
            Chassis No</td>
        <td>
            <input id="chassisno" type="text" class="form-control input-sm" /></td>
        <td>
            Engine No</td>
        <td>
            <input id="engineno" type="text" class="form-control input-sm" /></td>
    </tr>
    <tr>
        <td>Car Company</td>
        <td>
            <select id="carcompany" name="carcompany" class="form-control input-sm"></select></td>
        <td>Motor Type</td>
        <td>
            <select id="motortype" name="motortype" class="form-control input-sm">
                <option value="0">--SELECT--</option>
                <option value="LIGHT">LIGHT</option>
                <option value="MEDIUM">MEDIUM</option>
                <option value="HEAVY">HEAVY</option>
            </select></td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td></td>
        <td>
            <input id="searchbutton" class="btn btn-primary" type="button" value="Search" />&nbsp;<input id="resetbutton" 
                type="button" value="Reset" class="btn btn-default" /></td>
    </tr>
</table>
    <div id="searchresult" class="search-result">
    </div>
    <div id="progressbox">
        <h3 id="progress-message">
            Now Loading ...</h3>
    </div>
    <div id="search-footer clearfix">
    <div class="search-footer-a">
        <strong>Rows Per Page:</strong>
        <select id="rowsperpage">
            <option value="10">10</option>
            <option value="20">20</option>
            <option value="50">50</option>
            <option value="100">100</option>
            <option value="All">All</option>
        </select>
    </div>
    <div class="search-footer-b">
        <ul id="search-pager" class="pagination pagination-sm">

        </ul>
    </div>
    </div>
    
    <input id="pagehidden" type="hidden" />
    <input id="rowcounthidden" type="hidden" />
</asp:Content>
