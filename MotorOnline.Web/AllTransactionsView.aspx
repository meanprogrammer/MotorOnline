<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="AllTransactionsView.aspx.cs" Inherits="MotorOnline.Web.AllTransactionsView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/transactionsearch.js" type="text/javascript"></script>
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
    <div id="searchresult">
    </div>
    <div id="progressbox">
        <h3 id="progress-message">
            Now Loading ...</h3>
    </div>
</asp:Content>
