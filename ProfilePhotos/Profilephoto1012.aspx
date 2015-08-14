<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2checkout.aspx.cs" Inherits="_2checkout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
<script src="https://www.2checkout.com/checkout/api/2co.min.js"></script>

<script>
    // Called when token created successfully.
    var successCallback = function (data) {
        var myForm = document.getElementById('myCCForm');

        // Set the token as the value for the token input
        myForm.token.value = data.response.token.token;

        // IMPORTANT: Here we call `submit()` on the form element directly instead of using jQuery to prevent and infinite token request loop.
        myForm.submit();
    };

    // Called when token creation fails.
    var errorCallback = function (data) {
        if (data.errorCode === 200) {
            tokenRequest();
        } else {
            alert(data.errorMsg);
        }
    };

    var tokenRequest = function () {
        // Setup token request arguments
        var args = {
            sellerId: "901250391",
            publishableKey: "89988EBA-4422-46AD-B687-BEC8315CD85F",
            ccNo: $("#ccNo").val(),
            cvv: $("#cvv").val(),
            expMonth: $("#expMonth").val(),
            expYear: $("#expYear").val()
        };

        // Make the token request
        TCO.requestToken(successCallback, errorCallback, args);
    };

    $(function () {
        // Pull in the public encryption key for our environment
        TCO.loadPubKey('sandbox');

        $("#myCCForm").submit(function (e) {
            // Call our token request function
            tokenRequest();

            // Prevent form from submitting
            return false;
        });
    });
    function expMonth_onclick() {

    }

</script>
  <script type="text/javascript">
      function isNumber1(evt) {
          evt = (evt) ? evt : window.event;
          var charCode = (evt.which) ? evt.which : evt.keyCode;
          if (charCode > 31 && (charCode < 48 || charCode > 57)) {

              return false;
          }
          return true;
      }
  </script>
</head>
<body>

    <form id="myCCForm" action="process.aspx" method="post">
    
    <input id="token" name="token" type="hidden" value="">
    
    <div id=div1 runat="server">
    <div>
        <label>
            <span>Card Number</span>
        </label>
        <input id="ccNo" type="text" size="20" maxlength="16" onkeypress="return isNumber1(event)" value="" autocomplete="off" required />
    </div>
    <div>
        <label>
            <span>Expiration Date (MM/YYYY)</span>
        </label>
        <input type="text" size="2" id="expMonth" required maxlength="2" onkeypress="return isNumber1(event)" onclick="return expMonth_onclick()" />
        

        <span> / </span>
        <input type="text" size="2" maxlength="4" onkeypress="return isNumber1(event)" id="expYear" required />
    </div>
    <div>
        <label>
            <span>CVV</span>
        </label>
        <input id="cvv" size="4" type="text" value="" maxlength="3" onkeypress="return isNumber1(event)" autocomplete="off" required />
    </div>
    <input type="submit" value="Submit Payment">
    </div>
   &nbsp;<%-- <input type="hidden" name="return" id="return" value="<%= str_Success %>" />--%></form>
</body>
</html>
