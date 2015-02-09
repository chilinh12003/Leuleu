<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="avatar.aspx.cs" Inherits="LeuLeu.Test.avatar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <a href="@Url.Gravatar("MyEmailAddress@example.com", 80, GravatarHelper.DefaultImageIdenticon)">Your Gravatar</a>

    </div>
    </form>
</body>
</html>
