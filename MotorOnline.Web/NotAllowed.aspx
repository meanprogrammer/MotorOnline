<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="NotAllowed.aspx.cs" Inherits="MotorOnline.Web.NotAllowed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<h1>You are not allowed here.</h1>
<span>Click </span><a href="DefaultView.aspx">here</a><span> to return home.</span>
</asp:Content>
