<%@ Page Title="AADHAAR Mapping" Language="vb" AutoEventWireup="false" EnableEventValidation="false"
    MasterPageFile="~/MasterPage/MFDAfterLogin.Master" ClientIDMode="Static" CodeBehind="MFSAadharMapping.aspx.vb"
    Inherits="MFDOnline.MFSAadharmapping" %>

<%@ Implements Interface="Idunno.AntiCsrf.ISuppressCsrfCheck" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Javascript/CommonFunction.js" type="text/javascript"></script>
    <script src="../Javascript/AadhaarCommom.js" type="text/javascript"></script>
    <link href="../Css/CalenderTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Css/ModelPopup.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
    //testing purpose
        function Open_PopUp_Digio(url) {
            var id, file_name;
            window.open(url, "popup_window", "width=350,height=150,left=350,top=300,resizable=yes");
            var options = {
                "environment": "production",
                "callback": function (t) {
                    if (t.hasOwnProperty('txn_id') && t.hasOwnProperty('digio_doc_id') && t.hasOwnProperty('message') && t.message == "Signing Success") {
                        document.getElementById("result").innerHTML = "eMandate Registered Successfully";
                        document.getElementById('btneMandate_Sub').disabled = true;
                        success();
                        try {
                            writelog("Sucess: " + "|" + t.txn_id + "|" + t.digio_doc_id + "|" + t.message);
                        } catch (err) {
                        }
                    } else {
                        document.getElementById("loading").style.display = 'none';
                        document.getElementById("result").innerHTML = "failed to sign with error : " + t.message;
                        try {
                            if (t.hasOwnProperty('error_code') && t.hasOwnProperty('digio_doc_id') && t.hasOwnProperty('message')) {
                                writelog("Failure: " + "|" + t.error_code + "|" + t.digio_doc_id + "|" + t.message);
                            }
                        } catch (err) {
                        }
                    }
                }, "logo": "https://www.nsenmf.com/Images/MF_Logo.png"
            };

            var digio = new Digio_OPEN(options);
            digio.init123();
            //digio.esign(document.getElementById('hdnResp_ID').value, document.getElementById('hdnEmail').value);
        }
        //testing end

        function ChkValidation() {
            var Pan = trim(document.getElementById("txtPAN").value.toUpperCase());
            var Aadhar = trim(document.getElementById("txtAadharName").value);
            var dob = trim(document.getElementById("txtDOB").value);
            var mobile = trim(document.getElementById("txtmobileno").value);
            var rbgender = document.getElementById("<%=rdlgender.ClientID%>");
            var radio = rbgender.getElementsByTagName("input");
            var isSelected = false;

            var chkBox = document.getElementById('<%=chkBoxList.ClientID%>');
            var options = chkBox.getElementsByTagName('input');
            var isChecked = false;


            if ((trim(Pan) == '')) {
                alert("Please enter the PAN.");
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
            if (trim(dob) != '') {
                if (ChkDateFormat(dob) == 1) {
                    alert("Invalid From date.")
                    document.getElementById("txtDOB").focus();
                    return false;

                }
            }
            //            for (var i = 0; i < radio.length; i++) {
            //                if (radio[i].checked) {
            //                    isSelected = true;
            //                    break;
            //                }
            //            }
            //            if (!isSelected) {
            //                alert("Please select a Gender!");
            //                return false;
            //            }
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
            if (document.getElementById("txtemail").value != "") {
                if (Validate_Email(document.getElementById("txtemail")) == false) { return false; }
            }

            var listOfSpans = chkBox.getElementsByTagName('span');
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    isChecked = true;
                    break;
                }
            }
            if (!isChecked) {
                alert("Please check anyone registrar ID!");
                return false;
            }
        }

        function Validate_Email11(em) {
            var emailval = trim(em.value);
            if (emailval != "") {
                var regexp = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
                if (!regexp.test(emailval)) {
                    alert("Invalid Email");
                    em.focus();
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function ClearPANBoxes() {
            if (document.getElementById('chkPANExempt').checked == true) {
                $('#txtPAN').val('');
                $('#txtPAN').focus();
            }
        }
        function ChkAvaliablityValidate() {
            var ddlRegistrar = trim(document.getElementById("ddlRegistrar").value);
            if (trim(ddlRegistrar) == '') {
                alert("Please select Registrar ID.");
                document.getElementById("ddlRegistrar").focus();
                return false;
            }
            var Pan = trim(document.getElementById("txtPAN1").value.toUpperCase());
            if ((trim(Pan) == '')) {
                alert("Please enter the PAN.");
                document.getElementById("txtPAN1").focus();
                return false;
            }
            if ($("#chkPANExempt1").is(':checked')) {
            }
            else {
                if (panvalidate(document.getElementById("txtPAN1")) == false) {
                    return false;
                }
            }
        }
        function ClearTextboxes() {
            document.getElementById('chkPANExempt').checked = false
            document.getElementById('txtPAN').value = '';
            document.getElementById('txtAadharName').value = '';
            document.getElementById('txtDOB').value = '';
            document.getElementById('txtmobileno').value = '';
            document.getElementById('txtemail').value = '';
            document.getElementById('txtCustomerID').value = '';

            document.getElementById('chkPANExempt1').checked = false
            document.getElementById('txtPAN1').value = '';

            document.getElementById('dvgrid').style.display = "none";
            document.getElementById('divAadharmsg').style.display = "none";
            document.getElementById('SpnAadharmsg').innerHTML = "";

            document.getElementById('dvlink').style.display = "none";

            $("#<%= rdlgender.ClientID %> input[type=radio]").prop('checked', false);
            return false;
        }
        function iframGateway() {
            $find("popupDetails").show();
            $("popupDetails").show();
            return false;
        }
        function radbtnValue(chk) {
            var checked_radio = $("[id*=radaadhaar] input:checked");
            var radiovalue = $('[id*=radaadhaar]').find('input:checked').val();
            var radiotext = checked_radio.closest("td").find("label").html();
            $('#dvspace').hide();
            document.getElementById('btnSubmit').disabled = false;
            if (radiovalue == "IIN") {
                $('#dvlocate').show();
                $('#dvSubmis').hide();
                $('#dvAvail').hide();
                $("#txtCustomerID").focus();
            }
            else if (radiovalue == "SAVE") {
                $('#dvlocate').hide();
                $('#dvSubmis').show();
                $('#dvAvail').hide();
                $("#txtPAN").focus();
            }
            else if (radiovalue == "CHECK") {
                $('#dvlocate').hide();
                $('#dvSubmis').hide();
                $('#dvAvail').show();
                $("#txtPAN1").focus();
            }
            if (chk != "ACTION" && chk != "CHECK") {
                ClearTextboxes();
            }
            if (chk == "ACTION") {
                document.getElementById('btnSubmit').disabled = true;
                if (radiovalue == "IIN") {
                    $('#dvspace').show();
                    $('#dvSubmis').show();
                }
            }
        }
        function ValidateInput() {
            var Custid = $("#txtCustomerID").val();
            if (Custid == "") {
                alert("Please Enter CustomerID.");
                $("#txtCustomerID").focus();
                return false;
            }
        }
        function OpenAadhaarPopUp(rta) {
            var aCAMS = document.getElementById('aCAMS');
            var aFTI = document.getElementById('aFTI');
            var aKARVY = document.getElementById('aKARVY');
            var aSUN = document.getElementById('aSUN');
            var url;
            if (rta == 'CAMS') {
                url = aCAMS.href;
            }
            else if (rta == 'FTI') {
                url = aFTI.href;
            }
            else if (rta == 'KARVY') {
                url = aKARVY.href;
            }
            else if (rta == 'SUN') {
                url = aSUN.href;
            }
            popup = window.open(url, "popup_window", "width=750,height=700,left=250,top=200,resizable=yes");
            popup.focus();
            return false;
        }


        function OpenCheckAadhaarPopUp(url) {
            var self;
            //window.open(url, "popup_window", "width=350,height=350,left=350,top=300,resizable=yes");
            window.open(url, 'null', 'width=350,height=350,toolbar=no,scrollbars=no,location=no,resizable =yes');
            window.moveTo(0, 0);
            window.blur();
            return false;
        }

        function ChangeTab(lnk) {
            var checked_radio = $("[id*=radaadhaar] input:checked");
            var radiovalue = $('[id*=radaadhaar]').find('input:checked').val();

            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            document.getElementById('txtPAN').value = trim(row.cells[1].innerText);
            document.getElementById('txtAadharName').value = trim(row.cells[2].innerText);
            document.getElementById('txtemail').value = trim(row.cells[4].innerText);
            document.getElementById('txtmobileno').value = trim(row.cells[5].innerText);
            document.getElementById('txtDOB').value = trim(row.cells[6].innerText);

            var text = "Submission";
            var radio = $("[id*=radaadhaar] label:contains('" + text + "')").closest("td").find("input");
            //radio.attr("checked", "checked");
            document.getElementById('btnSubmit').disabled = false;
            $('#dvlocate').show();
            $('#dvspace').show();
            $('#dvSubmis').show();
            $('#dvAvail').hide();
            return false;
        }
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
    <div style="width: 100%;" align="center">
        <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" align="center" style="margin-top: 5px; width: 50%">
                    <tr>
                        <td style="height: 15px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="height: 35px;">
                                <div id="Div1" runat="server" class="page_header" style="width: 140px;">
                                    <span id="Span1" runat="server" class="page_headertext">AADHAAR Mapping</span>
                                </div>
                                <div class="page_headerline" style="width: 100%;">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tblAltCol" align="center" style="height: 35px;">
                            <asp:RadioButtonList ID="radaadhaar" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                onclick="radbtnValue();" Height="16px" Width="100%">
                                <asp:ListItem Value="IIN" Text="Locate by Customer ID" Selected="True">Locate by Customer ID</asp:ListItem>
                                <asp:ListItem Value="SAVE" Text="Submission">Submission</asp:ListItem>
                                <asp:ListItem Value="CHECK" Text="Avalability">Avalability</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <div id="dvlocate" runat="server">
                    <table id="tbllocate" runat="server" cellpadding="0" cellspacing="0" align="center"
                        style="width: 50%;">
                        <tr id="trFolio" runat="server">
                            <td class="tblColMan" style="width: 150px;">
                                Customer ID
                            </td>
                            <td class="tblAltCol">
                                <asp:TextBox ID="txtCustomerID" runat="server" CssClass="textinput" TabIndex="3"
                                    MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tblAltCol" style="text-align: center; padding-top: 5px; padding-bottom: 5px;">
                                <asp:Button ID="BtnView" runat="server" Text="View" TabIndex="9" CssClass="but_ord1"
                                    OnClientClick="return ValidateInput();" />
                                <asp:Button ID="Button2" runat="server" Text="Reset" TabIndex="10" CssClass="but_ord1"
                                    OnClientClick="javascript:return ClearTextboxes();" />
                            </td>
                        </tr>
                    </table>
                    <div id="dvgrid" runat="server" style="margin-top: 15px; display: none;">
                        <asp:GridView ID="grdview" CssClass="GridSummary" runat="server" AutoGenerateColumns="False"
                            Width="60%" AllowPaging="true" PageSize="20">
                            <HeaderStyle Height="35px" />
                            <Columns>
                                <asp:BoundField DataField="PAN_TYPE" HeaderText="PAN Type">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PAN_PEKRN" HeaderText="PAN">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="INVESTOR_NAME" HeaderText="Invesor Name">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TAX_STATUS_DESC" HeaderText="Tax Status">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EMAIL" HeaderText="EMAIL">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="HideGridColumn" />
                                    <HeaderStyle CssClass="HideGridColumn" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MOBILE_NO" HeaderText="MOBILE_NO">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="HideGridColumn" />
                                    <HeaderStyle CssClass="HideGridColumn" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DATE_OF_BIRTH" HeaderText="DATE_OF_BIRTH">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="HideGridColumn" />
                                    <HeaderStyle CssClass="HideGridColumn" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Submission" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSub" Text="Submission" runat="server" CommandName="Select"
                                            OnClientClick="javascript:return ChangeTab(this);" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div id="dvspace" style="height: 40px; display: none;">
                </div>
                <div id="dvSubmis" runat="server" style="display: none;">
                    <table width="50%" border="0" cellpadding="0" cellspacing="0">
                        <tr class="tblRow">
                            <td class="tblColMan">
                                PAN/PEKRN
                            </td>
                            <td class="tblAltCol">
                                <asp:TextBox ID="txtPAN" runat="server" CssClass="textinput" TabIndex="2" MaxLength="10"
                                    Style="text-transform: uppercase;"></asp:TextBox>
                                <asp:CheckBox ID="chkPANExempt" Text="Exemption" onclick="ClearPANBoxes();" runat="server" />
                            </td>
                        </tr>
                        <tr class="tblRow">
                            <td class="tblCol">
                                Name in AADHAAR
                            </td>
                            <td class="tblAltCol">
                                <asp:TextBox ID="txtAadharName" runat="server" CssClass="textinput" TabIndex="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tblColMan">
                                DOB
                            </td>
                            <td class="tblAltCol">
                                <asp:TextBox ID="txtDOB" CssClass="dateinput" runat="server" MaxLength="11" Style="text-transform: uppercase;"></asp:TextBox>
                                <asp:ImageButton runat="Server" CssClass="CalenderCss" ID="imgDOB" ImageUrl="../images/Calender.png"
                                    AlternateText="" TabIndex="4" />
                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDOB"
                                    CssClass="Cal_Theme" Format="dd-MMM-yyyy" PopupButtonID="imgDOB" />
                                <font size="1">(DD-MMM-YYYY)</font>
                            </td>
                        </tr>
                        <tr>
                            <td class="tblCol">
                                Gender
                            </td>
                            <td class="tblAltCol">
                                <asp:RadioButtonList ID="rdlgender" runat="server" RepeatDirection="Horizontal" TabIndex="5">
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
                                <asp:TextBox ID="txtmobileno" runat="server" CssClass="textinput" TabIndex="6" MaxLength="10"></asp:TextBox>
                                <br />
                                <font size="1">(Only 10 digit number without prefix of 0 or +91.)</font>
                            </td>
                        </tr>
                        <tr>
                            <td class="tblCol">
                                Email ID
                            </td>
                            <td class="tblAltCol">
                                <asp:TextBox ID="txtemail" runat="server" CssClass="textinput" TabIndex="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tblCol">
                                Registrar ID
                            </td>
                            <td class="tblAltCol">
                                <asp:CheckBoxList ID="chkBoxList" runat="server" RepeatLayout="OrderedList">
                                    <asp:ListItem Text="Computer Age Management Services Pvt Ltd" Value="CAMS"></asp:ListItem>
                                    <asp:ListItem Text="FRANKLIN TEMPLETON" Value="FTI"></asp:ListItem>
                                    <asp:ListItem Text="KARVY CONSULTING PVT LTD" Value="KARVY"></asp:ListItem>
                                    <asp:ListItem Text="Sundaram BNP Paribas Fund Services" Value="SUN"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="tblAltCol" style="height: 40px;">
                                <center>
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="but_ord1" TabIndex="8" Text="Submit"
                                        ClientIDMode="Static" OnClientClick="return ChkValidation();" />
                                    &nbsp;
                                    <asp:Button ID="btnreset" runat="server" CssClass="but_ord1" TabIndex="10" Text="Reset"
                                        OnClientClick="return ClearTextboxes();" />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <center>
                                    <font face='Verdana, Arial, Helvetica' size='-1' color='red'>
                                        <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="False"></asp:Label></font></center>
                                <span id="spnblockmsg" runat="server" style="color: Red; font-size: 14px;"></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="dvAvail" runat="server" style="display: none;">
                    <table width="50%" border="0" cellpadding="0" cellspacing="0">
                        <tr class="tblRow">
                            <td class="tblColMan">
                                Registrar Name
                            </td>
                            <td class="tblAltCol">
                                <asp:DropDownList ID="ddlRegistrar" CssClass="dropDownList" runat="server">
                                    <asp:ListItem Text="-- Choose Registrar ID --" Value="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Computer Age Management Services Pvt Ltd" Value="CAMS"></asp:ListItem>
                                    <asp:ListItem Text="FRANKLIN TEMPLETON" Value="FTI"></asp:ListItem>
                                    <asp:ListItem Text="KARVY CONSULTING PVT LTD" Value="KARVY"></asp:ListItem>
                                    <asp:ListItem Text="Sundaram BNP Paribas Fund Services" Value="SUN"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tblRow">
                            <td class="tblColMan">
                                PAN/PEKRN
                            </td>
                            <td class="tblAltCol">
                                <asp:TextBox ID="txtPAN1" runat="server" CssClass="textinput" TabIndex="2" MaxLength="10"
                                    Style="text-transform: uppercase;"></asp:TextBox>
                                <asp:CheckBox ID="chkPANExempt1" Text="Exemption" onclick="ClearPANBoxes();" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="tblAltCol" style="height: 40px;">
                                <center>
                                    <asp:Button ID="btnCheck" runat="server" CssClass="but_ord1" TabIndex="9" Text="Check"
                                        ClientIDMode="Static" OnClientClick="return ChkAvaliablityValidate();" />
                                    &nbsp;
                                    <asp:Button ID="btnAvlReset" runat="server" CssClass="but_ord1" TabIndex="10" Text="Reset"
                                        OnClientClick="return ClearTextboxes();" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divAadharmsg" runat="server" class="FormErrorSummary FormMessage" style="margin: 10px auto 0 auto;
                    width: 45%; display: none;">
                    <span id="SpnAadharmsg" runat="server"></span>
                </div>
                <div id="dvlink" runat="server" style="width: 50%; height: 300px; text-align: left;
                    display: none;">
                    <asp:LinkButton ID="lnkCAMS" runat="server" OnClientClick="return OpenAadhaarPopUp('CAMS')"
                        Visible="false" Text="AADHAAR Mapping
                            - CAMS"></asp:LinkButton>
                    <div id="dvCAMS" runat="server" style="height: 2px; display: block;">
                    </div>
                    <asp:LinkButton ID="lnkFTI" runat="server" OnClientClick="return OpenAadhaarPopUp('FTI')"
                        Visible="false" Text="AADHAAR Mapping
                            - FTI"></asp:LinkButton>
                    <div id="dvFTI" runat="server" style="height: 2px; display: block;">
                    </div>
                    <asp:LinkButton ID="lnkKARVY" runat="server" OnClientClick="return OpenAadhaarPopUp('KARVY')"
                        Visible="false" Text="AADHAAR Mapping
                            - KARVY"></asp:LinkButton>
                    <div id="dvKARVY" runat="server" style="height: 2px; display: block;">
                    </div>
                    <asp:LinkButton ID="lnkSUN" runat="server" OnClientClick="return OpenAadhaarPopUp('SUN')"
                        Visible="false" Text="AADHAAR Mapping
                            - SUN"></asp:LinkButton>
                    <div style="display: none;">
                        <a id="aCAMS" runat="server" href=""></a><a id="aFTI" runat="server" href=""></a>
                        <a id="aKARVY" runat="server" href=""></a><a id="aSUN" runat="server" href=""></a>
                    </div>
                </div>
                <ajax:ModalPopupExtender ID="popupDetails" BehaviorID="popupDetails" runat="server"
                    PopupControlID="dvmodal" TargetControlID="HiddenField1" BackgroundCssClass="modalBackground">
                </ajax:ModalPopupExtender>
                <div id="dvmodal" style="width: 750px; height: 650px; display: none;">
                    <div style="height: 35px; border-top-left-radius: 6px; border-top-right-radius: 6px;
                        background-color: #f68b1f;">
                        <div style="width: 500px; text-align: left; margin-left: 10px; float: left;">
                            <font id="Font1" style="color: White; text-align: left; font-size: large;">Mapping</font>
                        </div>
                        <div style="width: 40px; padding-top: 6px; float: right;">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </div>
                    </div>
                    <div align="center" style="margin-top: 10px; text-align: justify; width: 750px; height: 650px;
                        padding-left: 10px;">
                       <iframe name="myIframe" id="myIframe" width="700px" height="600px">
                        </iframe>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
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
