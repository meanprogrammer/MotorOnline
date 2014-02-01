<%@ Page Title="" Language="C#" MasterPageFile="~/Empty.Master" AutoEventWireup="true"
    CodeBehind="LoginView.aspx.cs" Inherits="MotorOnline.LoginView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/login.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div>
        <h3>
            Login</h3>
        <table cellpadding="5" align="center">
            <tr>
                <td align="center" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="UserName" runat="server" Style="padding: 10px;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="MotorOnlineLogin">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="padded-textbox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="MotorOnlineLogin">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="color: Red;">
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="MotorOnlineLogin"
                        ClientIDMode="Static" Font-Size="Medium" onclick="LoginButton_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
