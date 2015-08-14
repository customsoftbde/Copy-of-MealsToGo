<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="MealsToGo.Views.GCheckout.WebForm1" %>

<%@ Register assembly="GCheckout" namespace="GCheckout.Checkout" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 332px">
    <form id="form1" runat="server">
    <div draggable="auto" style="height: 326px">
    
        <cc1:GCheckoutButton ID="GCheckoutButton1" runat="server" Height="42px" ImageUrl="~/Images/checkout.gif" OnClick="GCheckoutButton1_Click" Width="120px" />
    
    </div>
    </form>
</body>
</html>
