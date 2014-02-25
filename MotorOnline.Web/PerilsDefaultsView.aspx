﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="PerilsDefaultsView.aspx.cs" Inherits="MotorOnline.Web.PerilsDefaultsView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    $(document).ready(initialize);
    function initialize() {
        $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            data: { action: "getallperilsdefaults" },
            success: function (result) {
                var obj = JSON.parse(result);
                var html = '<table class="table table-striped table-bordered"><tr><th></th><th>PerilID</th><th>LimitSI</th><th>Editable</th><th>Rate</th><th>Editable</th><th>Show Tariff Text</th><th>Premium</th><th>Policy Rate</th><th>Editable</th><th>Show Tariff Text</th><th>Policy Premium</th></tr>';
                if (obj != null) {
                    $.each(obj, function (key, value) {
                        html += '<tr>';
                        html += '<td><a href="#" onclick="showmodaledit(' + value.PerilID + ');" class="btn btn-default btn-xs">Edit</td>';
                        html += '<td>' + value.PerilID + '</td>';
                        html += '<td>' + value.LimitSIDefault + '</td>';
                        html += '<td>' + value.LimitSIEditable + '</td>';
                        html += '<td>' + value.RateDefault + '</td>';
                        html += '<td>' + value.RateEditable + '</td>';
                        html += '<td>' + value.RateShowTariffText + '</td>';
                        html += '<td>' + value.PremiumDefault + '</td>';
                        html += '<td>' + value.PolicyRateDefault + '</td>';
                        html += '<td>' + value.PolicyRateEditable + '</td>';
                        html += '<td>' + value.PolicyRateShowTariffText + '</td>';
                        html += '<td>' + value.PolicyPremiumDefault + '</td>';
                        html += "<input id='Hidden" + value.PerilID + "' type='hidden' value='" + JSON.stringify(value) + "' />";
                        html += '</tr>';
                    });
                }
                html += '</table>';
                $('#perils-defaults').html(html);
            },
            error: function () {

            }
        });
    }

    function showmodaledit(id) {
        var p = $('#Hidden' + id).val();
        var obj = JSON.parse(p);
        $('#limitsitext').val(obj.LimitSIDefault);
        $('#perildefault-modal').modal();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="perils-defaults">
</div>

<div class="modal fade" id="perildefault-modal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h4 class="modal-title">Edit Peril Default</h4>
      </div>
      <div class="modal-body">
        <table class="table table-striped table-bordered" cellpadding="8">
            <tr>
                <td><strong>Limit/SI</strong></td>
                <td><input id="limitsitext" type="text" /></td>
                <td colspan="2"><input id="limitsieditable" type="checkbox" /> Editable</td>
            </tr>
            <tr>
                <td><strong>Rate</strong></td>
                <td><input id="ratetext" type="text" /></td>
                <td><input id="rateeditable" type="checkbox" /> Editable</td>
                <td><input id="rateshowtariff" type="checkbox" /> Show as Tariff</td>
            </tr>
            <tr>
                <td><strong>Premium</strong></td>
                <td colspan="3"><input id="premiumtext" type="text" /></td>
            </tr>
            <tr>
                <td><strong>Policy Rate</strong></td>
                <td><input id="policyratetext" type="text" /></td>
                <td><input id="policyrateeditable" type="checkbox" /> Editable</td>
                <td><input id="policyrateshowtariff" type="checkbox" /> Show as Tariff</td>
            </tr>
            <tr>
                <td><strong>Policy Premium</strong></td>
                <td colspan="3"><input id="policypremiumtext" type="text" /></td>
            </tr>
        </table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary">Save changes</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->

</asp:Content>
