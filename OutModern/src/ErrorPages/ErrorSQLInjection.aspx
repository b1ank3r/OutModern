<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorSQLInjection.aspx.cs" Inherits="OutModern.src.ErrorPages.ErrorSQLInjection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error SQL Injection</title>
    <style>
        @import url("https://fonts.googleapis.com/css?family=Share+Tech+Mono|Montserrat:700");

        * {
            margin: 0;
            padding: 0;
            border: 0;
            font-size: 100%;
            font: inherit;
            vertical-align: baseline;
            box-sizing: border-box;
            color: inherit;
        }

        body {
            background-color: black;
            height: 100vh;
        }

        div {
            background: rgba(0, 0, 0, 0);
            width: 70vw;
            position: relative;
            top: 50%;
            transform: translateY(-50%);
            margin: 0 auto;
            padding: 30px 30px 10px;
            box-shadow: 0 0 150px -20px rgba(0, 0, 0, 0.5);
            z-index: 3;
        }

        p {
            font-family: "Share Tech Mono", monospace;
            color: #f5f5f5;
            margin: 0 0 20px;
            font-size: 17px;
            line-height: 1.2;
        }

        span {
            color: white;
            font-size:24px;
        }

        i {
            color: red;
            font-size:24px;
        }

        b {
            color: red;
            font-size:24px;
        }

        .homepage {
            color: white !important;
        }
    </style>
</head>
<body>
    <div id="messageContainer">
        <p><span>ERROR CODE</span>: <i>SQL Injection</i></p>
        <p>
            <span>ERROR MESSAGE</span>: <i>Access Denied. Your account has been locked due to suspicion of a SQL injection attack.
            </i>
        </p>
        <p>
            <span>ERROR DESCRIPTION</span>: <b>SQL injection attack detected, suspicious activity triggering security measures, unauthorized database access attempts, exploitation of vulnerabilities, malicious SQL code injection, improper user input handling, inadequate security controls.
            </b>
        </p>
        <p>
            [<asp:HyperLink runat="server" CssClass="homepage" NavigateUrl="~/src/Client/Home/Home.aspx">Return to Home Page</asp:HyperLink>...]
        </p>
    </div>
    <script>
        const errorMessage = document.getElementById("messageContainer").innerHTML.toString()
        const messageContainer = document.getElementById("messageContainer");
        document.getElementById("messageContainer").innerHTML = "";
        let index = 0;

        function displayMessage() {
            messageContainer.innerHTML = errorMessage.slice(0, index) + "|";
            index++;

            if (index > errorMessage.length) {
                messageContainer.innerHTML = errorMessage;
                return;
            }

            setTimeout(displayMessage, 15);
        }
        displayMessage();
    </script>
</body>
</html>
