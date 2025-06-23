<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MFSAadharConfirmation.aspx.vb"
    Title="Aadhar Confirmation" Inherits="MFDOnline.MFSAadharConfirmation" EnableEventValidation="false"
    ClientIDMode="Static" %>

<%@ Implements Interface="Idunno.AntiCsrf.ISuppressCsrfCheck" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Javascript/AadhaarCommom.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js" type="text/javascript"></script>
    <script src="https://demo.sundarambnpparibasfs.in/miscellaneous/js/jquery.redirect.js"
        type="text/javascript"></script>
</head>
<body>
    <form runat="server" id="frmAadhar" name="frmAadhar">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">
        //            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(callPageload);
        //            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callPageload);
        function AadharGateway() {
            frmAadhar.method = 'POST';
            //document.getElementById('USER_ID').value = "AL_STERL";
            //document.getElementById('PASSWORD').value = "dF$8Bk9a";
            frmAadhar.enctype = 'multipart/form-data';
            //frmAadhar.action = 'https://uat2.camsonline.com/InvestorServices/AMCandCPAadhaar.aspx';
            frmAadhar.action = document.getElementById('URL').value;
            frmAadhar.submit();
            return false;
        }

        function GetAadhaarmsg(msg) {
            //window.close();
            //this.close();
            //alert(msg);
            //            window.setTimeout(function () {
            //                alert(msg)
            //            }, 100);
            //window.blur();
            //setTimeout(function () { self.close(); }, 5000);
            return false;
        }
        function XMLGateway() {
            var cbsulogin = $("#hdnXML").val();
            alert(cbsulogin);
            $.redirect("https://demo.sundarambnpparibasfs.in/miscellaneous/public/aadhaarChannel.xhtml", cbsulogin);
            //  $.redirect(document.getElementById('URL').value,{cbsulogin}); 
        }
    </script>
    <div>
        <input type="hidden" runat="server" id="USER_ID" name="USER_ID" />
        <input type="hidden" runat="server" id="PASSWORD" name="PASSWORD" />
        <input type="hidden" runat="server" id="SESSION_ID" name="SESSION_ID" />
        <input type="hidden" runat="server" id="PAN_PEKRN" name="PAN_PEKRN" />
        <input type="hidden" runat="server" id="RETN_URL" name="RETN_URL" />
        <input type="hidden" runat="server" id="REQUEST_TYPE" name="REQUEST_TYPE" />
        <input type="hidden" runat="server" id="AADHAAR_NAME" name="AADHAAR_NAME" />
        <input type="hidden" runat="server" id="DOB" name="DOB" />
        <input type="hidden" runat="server" id="GENDER" name="GENDER" />
        <input type="hidden" runat="server" id="MOBILE_NO" name="MOBILE_NO" />
        <input type="hidden" runat="server" id="EMAIL_ID" name="EMAIL_ID" />
        <input type="hidden" runat="server" id="URL" name="URL" />
        <input type="hidden" runat="server" id="reqObj" name="reqObj" />
        <input type="hidden" runat="server" value="" id="MOBILENO" name="MOBILENO" />
        <input type="hidden" runat="server" value="" id="hdnXML" name="hdnXML" />
    </div>
    </form>
    <div id="dverror" runat="server" visible="false" style="width: 100%; height: auto;
        margin: 0 auto; margin-top: 50px;">
        <center>
            <span id="spnloadmsg" runat="server" style="color: Red; text-align: center; font-size: 25px;">
            </span>
        </center>
    </div>
</body>
</html>
