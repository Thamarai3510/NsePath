<%@ Page Title="Aadhar mapping with Folio" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MFDAfterLogin.Master"
    ClientIDMode="Static" CodeBehind="MFSAadharMapping_test.aspx.vb" Inherits="MFDOnline.MFSAadharMapping_test" %>

<%@ Implements Interface="Idunno.AntiCsrf.ISuppressCsrfCheck" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/CalenderTheme.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function ChkValidation(mode) {
            var Pan = trim(document.getElementById("txtPAN").value.toUpperCase());
            var Aadhar = trim(document.getElementById("txtAadharName").value);
            var dob = trim(document.getElementById("txtDOB").value);
            var mobile = trim(document.getElementById("txtmobileno").value);
            var rbgender = document.getElementById("<%=rdlgender.ClientID%>");
            var radio = rbgender.getElementsByTagName("input");
            var isChecked = false;

            //return isChecked;

            if ((trim(Pan) == '')) {
                document.getElementById("spnalertmsg").innerHTML = "Please enter the PAN.";
                $("#dialog-message").dialog({
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                //alert("Please enter the PAN.");
                document.getElementById("txtPAN").focus();
                return false;
            }
            if ($("#chkPANExempt").is(':checked')) {
            }
            else {
                if (panvalidate(document.getElementById("txtPAN")) == false) {
                    return false;
                }
            }
            //dob
            if (trim(dob) == '') {
                alert("Please Select Date of Birth");
                document.getElementById("txtDOB").focus();
                return false;
            }
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    isChecked = true;
                    break;
                }
            }
            if (!isChecked) {
                alert("Please select a Gender!");
                return false;
            }
            if (trim(mobile) == '') {
                alert("Please enter Mobile number");
                document.getElementById("txtmobileno").focus();
                return false;

            }
            if (document.getElementById("txtmobileno").value != "") {
                if (!(onlyNumber(document.getElementById("txtmobileno")))) {
                    document.getElementById("txtmobileno").focus();
                    return false;
                }
                if (mobile.length < 10) {
                    alert("Invalid Mobile number.");
                    document.getElementById("txtmobileno").focus();
                    return false;
                }
            }
            if (mode == "test") { AadharGateway(); }
        }
        function ClearPANBoxes() {
            if (document.getElementById('chkPANExempt').checked == true) {
                $('#txtPAN').val('');
                $('#txtPAN').focus();
            }
        }
        function AadharGateway() {
            var ret_url = '<%=strReturn_URL%>';
            var sess_id = '<%=sess_id %>';
            frmMFDMaster.method = 'POST';
            document.getElementById('USER_ID').value = "AL_STERL";
            document.getElementById('PASSWORD').value = "dF$8Bk9a";
            document.getElementById('SESSION_ID').value = sess_id;
            document.getElementById('RETN_URL').value = ret_url;
            document.getElementById('PAN_PEKRN').value = $('#txtPAN').val().toUpperCase();
            document.getElementById('REQUEST_TYPE').value = "2";
            document.getElementById('AADHAAR_NAME').value = $('#txtAadharName').val();
            document.getElementById('DOB').value = $('#txtDOB').val().toUpperCase();
            document.getElementById('GENDER').value = $('#<%=rdlgender.ClientID %> input:checked').val();
            document.getElementById('MOBILE_NO').value = $('#txtmobileno').val();
            document.getElementById('EMAIL_ID').value = $('#txtemail').val();
            frmMFDMaster.enctype = 'multipart/form-data';
            frmMFDMaster.target = '_blank';
            frmMFDMaster.action = 'https://uat2.camsonline.com/InvestorServices/AMCandCPAadhaar.aspx';
            //frmMFDMaster.submit();

            $('#frmMFDMaster').submit(function (e) {
                e.preventDefault();
                $('#dialog-confirm').dialog('open');
            });
            $("#dialog-confirm").dialog({
                autoOpen: false,
                modal: true,
                buttons: {
                    "Continue": function () {
                        //$(this).dialog('close');
                        $("#frmMFDMaster").submit();
                        //e.preventDefault();
                        $('#dialog-confirm').load('https://www.google.co.in/?gfe_rd=cr&dcr=0&ei=I8mnWpLxO-aK8Qe-k5roCA');
                        $('#dialog-confirm').dialog('open');
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                        // close dialog and do nothing
                    }
                }
            });

            return false;
        }
        $('#frmMFDMaster').submit(function (e) {
            e.preventDefault();
            $('#dialog-confirm').dialog('open');
        });
        /*
        $('form#yourFormId').submit(function(e){
   e.preventDefault();
   $('#dialog-confirm').dialog('open');
});
$("#dialog-confirm").dialog({
    autoOpen: false,
    modal: true,
    buttons: {
           "Continue": function () {
                $(this).dialog('close');
                $("form#yourFormId").submit();
           },
           "Cancel": function () {
           // close dialog and do nothing
          }
    }
});
        */
    </script>
    <style type="text/css">
        .ui-widget
        {
            font-family: Verdana,Arial,sans-serif;
            font-size: 8pt; /*color: #ff7d30;*/ /*background-color:#ff7d30;*/
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Button ID="btnTest" runat="server" Text="Button" Visible="false" /></div>
    <div style="width: 100%;" align="center">
        <table cellpadding="0" cellspacing="0" align="center" style="margin-top: 5px; width: 40%">
            <tr>
                <td style="height: 15px;">
                </td>
            </tr>
            <tr>
                <td>
                    <div style="height: 35px;">
                        <div id="Div1" runat="server" class="page_header" style="width: 230px;">
                            <span id="Span1" runat="server" class="page_headertext">AADHAR SEEDING</span>
                        </div>
                        <div class="page_headerline" style="width: 100%;">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table width="40%" border="0" cellpadding="0" cellspacing="0">
            <tr class="tblRow">
                <td class="tblColMan">
                    PAN/PEKRN
                </td>
                <td class="tblAltCol">
                    <asp:TextBox ID="txtPAN" runat="server" CssClass="textinput" TabIndex="1" MaxLength="10"
                        Style="text-transform: uppercase;"></asp:TextBox>
                    <asp:CheckBox ID="chkPANExempt" Text="Exemption" onclick="ClearPANBoxes();" runat="server" />
                    <%--    onblur="return panvalidate(txtPAN);" --%>
                </td>
            </tr>
            <tr class="tblRow">
                <td class="tblCol">
                    Aadhar Name
                </td>
                <td class="tblAltCol">
                    <asp:TextBox ID="txtAadharName" runat="server" CssClass="textinput" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tblColMan">
                    DOB
                </td>
                <td class="tblAltCol">
                    <asp:TextBox ID="txtDOB" CssClass="dateinput" runat="server" MaxLength="11" Enabled="True"
                        TabIndex="3" Style="text-transform: uppercase;"></asp:TextBox>
                    <asp:ImageButton runat="Server" CssClass="CalenderCss" ID="imgDOB" ImageUrl="../images/Calender.png"
                        AlternateText="" TabIndex="3" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDOB"
                        CssClass="Cal_Theme" Format="dd-MMM-yyyy" PopupButtonID="imgDOB" />
                    <font size="1">(DD-MMM-YYYY)</font>
                </td>
            </tr>
            <tr>
                <td class="tblColMan">
                    Gender
                </td>
                <td class="tblAltCol">
                    <asp:RadioButtonList ID="rdlgender" runat="server" RepeatDirection="Horizontal" TabIndex="4">
                        <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                        <asp:ListItem Text="Transgender" Value="T"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tblColMan">
                    Mobile No
                </td>
                <td class="tblAltCol">
                    <asp:TextBox ID="txtmobileno" runat="server" CssClass="textinput" TabIndex="5" MaxLength="10"></asp:TextBox>
                    <br />
                    <font size="1">(Only 10 digit number without prefix of 0 or +91.)</font>
                </td>
            </tr>
            <tr>
                <td class="tblCol">
                    Email ID
                </td>
                <td class="tblAltCol">
                    <asp:TextBox ID="txtemail" runat="server" CssClass="textinput" TabIndex="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="">
                    <center>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="but_ord1" TabIndex="7" Text="Sumbit "
                            ClientIDMode="Static" OnClientClick="return ChkValidation('test');" />
                        &nbsp;
                        <asp:Button ID="btnreset" runat="server" CssClass="but_ord1" TabIndex="8" Text="Reset" />
                        &nbsp;
                        <asp:Button ID="btnSubmit1" runat="server" CssClass="but_ord1" TabIndex="7" Text="test"
                            OnClientClick="return ChkValidation();" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <center>
                        <font face='Verdana, Arial, Helvetica' size='-1' color='red'>
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="False"></asp:Label></font></center>
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog-message" title="Alert Box">
        <span id="spnalertmsg"></span>
    </div>
    <div id="dialog-confirm"" title="Aadhar Box">
    </div>
    <input type="hidden" id="USER_ID" name="USER_ID" />
    <input type="hidden" id="PASSWORD" name="PASSWORD" />
    <input type="hidden" id="SESSION_ID" name="SESSION_ID" />
    <input type="hidden" id="PAN_PEKRN" name="PAN_PEKRN" />
    <input type="hidden" id="RETN_URL" name="RETN_URL" />
    <input type="hidden" id="REQUEST_TYPE" name="REQUEST_TYPE" />
    <input type="hidden" id="AADHAAR_NAME" name="AADHAAR_NAME" />
    <input type="hidden" id="DOB" name="DOB" />
    <input type="hidden" id="GENDER" name="GENDER" />
    <input type="hidden" id="MOBILE_NO" name="MOBILE_NO" />
    <input type="hidden" id="EMAIL_ID" name="EMAIL_ID" />
</asp:Content>
