Imports System.Net
Imports System.Xml
Imports System.Web.UI.HtmlControls



Public Class MFSAadharmapping
    Inherits CommonFunctions


#Region "Variable Declaration"
    Dim strQueryString As String = String.Empty
    Dim strDecryptedData As String = String.Empty

    Dim AssignParam As New MFD_TRXN_Select_Property
    Dim InsertParam As New MFD_InsupdProperty
    Dim MFD_Bll As New MFD_BLL

    Dim smtEncrypt As New SmartEncript
    Dim nvCollection As NameValueCollection
    Public sess_id As String
    Dim allstr, allstrarr() As String
    Dim strPageName As String = "Aadhar Mapping"
    Dim StrUser_Code, strServerHost, strBrokName, strBrokCode, strUser_level, StrUser_type, strBrokType As String
    Public strReturn_URL As String
    Dim strQueryStringVal As String = String.Empty
    Dim strResult As String = String.Empty
    Dim strRef_No As String = String.Empty

    Dim strRetrun_SessID, strRetrun_PAN, strRetrun_RespCode, strRetrun_Name, strRetrun_DOB, strRetrun_Gender, strRetrun_mob As String
    Dim dtIIN As DataTable
    Dim a As HtmlAnchor
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            strRetrun_SessID = ModuleInputFilterlines(Request.Form("SESSION_ID"))
            strRetrun_PAN = ModuleInputFilterlines(Request.Form("PAN_PEKRN"))
            strRetrun_RespCode = ModuleInputFilterlines(Request.Form("RESPONSE_CODE"))
            strRetrun_Name = ModuleInputFilterlines(Request.Form("NAME"))
            strRetrun_DOB = ModuleInputFilterlines(Request.Form("DOB"))
            strRetrun_Gender = ModuleInputFilterlines(Request.Form("GENDER"))
            strRetrun_mob = ModuleInputFilterlines(Request.Form("MOBILE_NO"))
            '
            If strRetrun_RespCode <> String.Empty Then
                lblErrorMsg.Text = strRetrun_RespCode
                btnSubmit.Enabled = False
            Else
                'strQueryString = ModuleInputFilterlines(Request.QueryString.ToString())
                Dim hdnID As HiddenField = CType(Page.Master.FindControl("hidAllStr"), HiddenField)
                strQueryStringVal = Request.Form("hidAllStr")
                If strQueryStringVal = "" AndAlso hdnID IsNot Nothing Then
                    strQueryStringVal = hdnID.Value
                End If
                If strQueryStringVal = String.Empty And hdnID.Value = String.Empty Then
                Else
                    strDecryptedData = smtEncrypt.decrypt(Server.UrlDecode(strQueryStringVal))
                nvCollection = ParseQueryString(strDecryptedData, "")
                    sess_id = Trim(nvCollection("sess_id"))
                    allstr = Trim(nvCollection("allstr"))
                    Dim MasPage As MFDAfterLogin = CType(Page.Master, MasterPage)
                    If Not (strQueryStringVal Is Nothing And sess_id Is Nothing) Then
                        If strQueryStringVal.Length > 0 Then
                            MasPage.SessionId = Server.UrlEncode(smtEncrypt.encrypt(sess_id))
                            MasPage.AllString = Server.UrlEncode(smtEncrypt.encrypt(allstr))
                        End If
                    End If
                End If
                strReturn_URL = Request.Url.GetLeftPart(UriPartial.Authority) + IIf(Request.ApplicationPath.Length > 1, Request.ApplicationPath, "") & "/AadharMapping/MFSAadharConfirmation.aspx"
                If allstr.Length > 0 Then
                    allstrarr = Split(allstr, "|")
                    StrUser_Code = ModuleInputFilterlines(allstrarr(0).ToString)
                    strServerHost = ModuleInputFilterlines(allstrarr(1).ToString)
                    strBrokName = ModuleInputFilterlines(allstrarr(16).ToString)
                    strBrokCode = ModuleInputFilterlines(allstrarr(4).ToString)
                    strUser_level = ModuleInputFilterlines(allstrarr(6).ToString)
                    StrUser_type = ModuleInputFilterlines(allstrarr(7).ToString)
                    strBrokType = ModuleInputFilter(allstrarr(60).ToString)
                End If
                SessionValidation(allstr, sess_id, StrUser_Code, strPageName, strServerHost)
            End If
            txtPAN.Focus()
        Catch ex1 As Threading.ThreadAbortException
        Catch ex As Exception
            Call_ErrorPage(sess_id, "", "", ex.ToString, "Error Occured In AADHAR Mapping Page Load", strPageName)
        End Try

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Try
            If Validation() = False Then
                Exit Sub
            End If
            InsertDetail()
        Catch ex As Exception
            Call_ErrorPage(sess_id, "", "", ex.ToString, "Error Occured In AADHAR Mapping button Submit", strPageName)
        End Try
    End Sub
    Sub InsertDetail()
        InsertParam = New MFD_InsupdProperty
        MFD_Bll = New MFD_BLL
        Dim strsubmsg, stralready As String
        Try
            Dim selectedItems As String = String.Join(",", chkBoxList.Items.OfType(Of ListItem)().Where(Function(r) r.Selected).[Select](Function(r) r.Value))
            With InsertParam
                .Flag = "INS_AADHAR_MAPPING"
                .Usercode = StrUser_Code
                .broker_code = strBrokCode
                .Folio = txtPAN.Text.ToString().ToUpper()
                If chkPANExempt.Checked = True Then
                    .trxn_type = "Y"
                Else
                    .trxn_type = "N"
                End If
                .broker_name = txtAadharName.Text
                .instrm_date = txtDOB.Text
                .amc_code = rdlgender.SelectedValue
                .BRANCH_COUNTRY = txtmobileno.Text
                .instrm_branch = txtemail.Text
                .SOURCEPROGRAM = "MFSWEB"
                '.instrm_bank = selectedItems
            End With
            strResult = MFD_Bll.MFDInsertUpdate(InsertParam, StrUser_Code, strPageName)
            If strResult.Split("|")(0) = 0 Then
                stralready = ""
                SpnAadharmsg.InnerHtml = ""
                If strResult.Split("|")(1) = "CHECK STATUS" Then
                    'strResult.Split("|")(2) 
                    strRef_No = strResult.Split("|")(3)
                    For Each lst As ListItem In chkBoxList.Items
                        If lst.Selected = True Then
                            If strResult.Split("|")(2).Contains(lst.Value) = True Then
                                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                                SpnAadharmsg.InnerHtml += lst.Value + " Already Mapped with PAN. <br />"
                            Else
                                If stralready = String.Empty Then
                                    divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                                    If SpnAadharmsg.InnerHtml <> String.Empty Then
                                        SpnAadharmsg.InnerHtml += " And also Please click the following link for AADHAAR Mapping. <br />"
                                    Else
                                        SpnAadharmsg.InnerHtml += " Please click the following link for AADHAAR Mapping. <br />"
                                    End If
                                    stralready = "True"
                                    'Do Something
                                End If
                            End If
                        End If
                    Next
                    '    divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                    '    SpnAadharmsg.InnerText = strResult.Split("|")(1)
                    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GET", "javascript:radbtnValue('ACTION');", True)
                    '    Exit Sub
                    'ElseIf strResult.Split("|")(1) = "Not Verified" Then
                    '    divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                    '    SpnAadharmsg.InnerText = "Please click the following link for AADHAAR Mapping."
                Else
                    strRef_No = strResult.Split("|")(2)
                    divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                    SpnAadharmsg.InnerText += "Please click the following link for AADHAAR Mapping."
                End If
                'btnSubmit.Enabled = False
                btnSubmit.Attributes.Add("disabled", "true")
                GenerateURL()

                'strQueryStringVal = "PAN_PEKRN=" & txtPAN.Text.ToString().ToUpper() & "&SourceProgram=MFSWEB" & "&UserCode=" & StrUser_Code & "&SESS_ID=" & sess_id & "&REF_NO=" & strResult.Split("|")(2) & "&REQUEST_TYPE=2" & "&REGISTRAR_ID=CAMS"
                'Dim url As String = String.Format("../AadharMapping/MFSAadharConfirmation.aspx?" & Server.UrlEncode(smtEncrypt.encrypt(strQueryStringVal)), False)
                'Dim script As String = String.Format("window.open('{0}','_blank');", url)

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GET", "javascript:radbtnValue('ACTION');", True)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YourStartupScript", script, True)

                'myIframe.Attributes.Add("src", url)
                'popupDetails.Show()

                'Dim s As String = "window.open('" & url + "', 'popup_window', 'width=750,height=700,left=250,top=200,resizable=yes');"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", s, True)
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.ToString)
        Finally
            InsertParam = Nothing
            MFD_Bll.Dispose()
        End Try
    End Sub
    Sub GenerateURL()
        Dim strRTA As String() = {"CAMS", "FTI", "KARVY", "SUN"}
        Dim strRTAID As String() = {"aCAMS", "aFTI", "aKARVY", "aSUN"}
        a = New HtmlAnchor
        Try
            dvlink.Style.Add(HtmlTextWriterStyle.Display, "block")
            For i As Integer = 0 To strRTA.Length - 1
                'hypurl.Visible = True
                strQueryStringVal = "PAN_PEKRN=" & txtPAN.Text.ToString().ToUpper() & "&SourceProgram=MFSWEB" & "&UserCode=" & StrUser_Code & "&SESS_ID=" & sess_id & "&REF_NO=" & strRef_No & "&REQUEST_TYPE=2" & "&REGISTRAR_ID=" & strRTA(i)
                Dim url As String = String.Format("../AadharMapping/MFSAadharConfirmation.aspx?" & Server.UrlEncode(smtEncrypt.encrypt(strQueryStringVal)), False)
                Dim script As String = String.Format("window.open('{0}','_blank');", url)

                'a.ID = strRTAID(i)
                'a.Attributes.Add("href", url)
                'hypurl.NavigateUrl = url
                If strRTA(i) = "CAMS" Then
                    aCAMS.HRef = url
                ElseIf strRTA(i) = "FTI" Then
                    aFTI.HRef = url
                ElseIf strRTA(i) = "KARVY" Then
                    aKARVY.HRef = url
                ElseIf strRTA(i) = "SUN" Then
                    aSUN.HRef = url
                End If
            Next
            For j As Integer = 0 To chkBoxList.Items.Count - 1
                If chkBoxList.Items(j).Selected Then
                    If chkBoxList.Items(j).Value = "CAMS" AndAlso strResult.Split("|")(2).Contains(chkBoxList.Items(j).Value) = False Then
                        lnkCAMS.Visible = True
                        dvCAMS.Visible = True
                    ElseIf chkBoxList.Items(j).Value = "FTI" AndAlso strResult.Split("|")(2).Contains(chkBoxList.Items(j).Value) = False Then
                        lnkFTI.Visible = True
                        dvFTI.Visible = True
                    ElseIf chkBoxList.Items(j).Value = "KARVY" AndAlso strResult.Split("|")(2).Contains(chkBoxList.Items(j).Value) = False Then
                        lnkKARVY.Visible = True
                        dvKARVY.Visible = True
                    ElseIf chkBoxList.Items(j).Value = "SUN" AndAlso strResult.Split("|")(2).Contains(chkBoxList.Items(j).Value) = False Then
                        lnkSUN.Visible = True
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New ApplicationException(ex.ToString)
        End Try
    End Sub

    Function Validation() As Boolean
        Dim boolVal As Boolean = True
        Try
            If (txtPAN.Text = String.Empty) Then
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                SpnAadharmsg.InnerText = "Please enter PAN"
                boolVal = False
                Exit Function
            ElseIf (txtPAN.Text <> String.Empty) AndAlso (chkPANExempt.Checked = False) Then
                If Regex.Match(txtPAN.Text.ToUpper.Trim, "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$").Success() = False Then
                    divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                    SpnAadharmsg.InnerText = "Invalid PAN Format"
                    boolVal = False
                    Exit Function
                End If
            End If
            'If (txtDOB.Text = String.Empty) Then
            '    lblErrorMsg.Text = "Please Select Date of Birth"
            '    boolVal = False
            '    Exit Function
            'End If
            'If (rdlgender.SelectedValue = String.Empty) Then
            '    lblErrorMsg.Text = "Please select a Gender!"
            '    boolVal = False
            '    Exit Function
            'End If
            If (txtmobileno.Text = String.Empty) Then
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                SpnAadharmsg.InnerText = "Please enter Mobile number"
                boolVal = False
                Exit Function
            End If
            Dim selectedItems As String = String.Join(",", chkBoxList.Items.OfType(Of ListItem)().Where(Function(r) r.Selected).[Select](Function(r) r.Value))
            If selectedItems = "" Then
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                SpnAadharmsg.InnerText = "Please check anyone Registrar ID"
                boolVal = False
                Exit Function
            End If
        Catch ex As Exception
            boolVal = False
            Throw New ApplicationException(ex.ToString)
        End Try
        Return boolVal
    End Function

    Protected Sub btnCheck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCheck.Click
        Try
            If ddlRegistrar.SelectedValue = "" Then
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                SpnAadharmsg.InnerText = "Please select Registrar ID"
                Exit Sub
            End If
            If (txtPAN1.Text = String.Empty) Then
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                SpnAadharmsg.InnerText = "Please enter PAN"
                Exit Sub
            ElseIf (txtPAN1.Text <> String.Empty) AndAlso (chkPANExempt1.Checked = False) Then
                If Regex.Match(txtPAN1.Text.ToUpper.Trim, "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$").Success() = False Then
                    divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                    SpnAadharmsg.InnerText = "Invalid PAN Format"
                    Exit Sub
                End If
            End If
            dvgrid.Style.Add(HtmlTextWriterStyle.Display, "none")
            dvlink.Style.Add(HtmlTextWriterStyle.Display, "none")
            strQueryStringVal = "PAN_PEKRN=" & txtPAN1.Text.ToString().ToUpper() & "&SourceProgram=MFSWEB" & "&UserCode=" & StrUser_Code & "&SESS_ID=" & sess_id & "&REQUEST_TYPE=1" & "&REGISTRAR_ID=" & ddlRegistrar.SelectedValue
            Dim url As String = String.Format("../AadharMapping/MFSAadharConfirmation.aspx?" & Server.UrlEncode(smtEncrypt.encrypt(strQueryStringVal)), False)
            Dim script As String = String.Format("window.open('{0}','_blank');", url)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YourStartupScript", script, True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GET", "javascript:radbtnValue('CHECK');", True)

            If ddlRegistrar.SelectedValue = "SUN" Then
                GenerateCHECKxml()
            Else
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=100,height=100,left=350,top=300,resizable=yes');"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", s, True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OPEN", "javascript:OpenCheckAadhaarPopUp('" + url + "');", True)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OPEN", "javascript:Open_PopUp_Digio('" + url + "');", True)
            End If

        Catch ex As Exception
            Call_ErrorPage(sess_id, "", "", ex.ToString, "Error Occured In AADHAR Mapping button check", strPageName)
        End Try

    End Sub
    Protected Sub BtnView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnView.Click
        Try
            BindIINDetails()
        Catch ex As Exception
            Call_ErrorPage(sess_id, "", "", ex.ToString, "Error Occured In AADHAR Mapping button view", strPageName)
        End Try
    End Sub
    Sub BindIINDetails()
        AssignParam = New MFD_TRXN_Select_Property
        MFD_Bll = New MFD_BLL
        Try
            With AssignParam
                .FLAG = "GET_INV_PAN_DETAILS"
                .FolioNo = ModuleInputFilterSQL(txtCustomerID.Text.Trim())
                .BrokerCode = strBrokCode
            End With
            dtIIN = MFD_Bll.GenerateData(AssignParam, StrUser_Code, strPageName, strServerHost)
            If dtIIN.Rows.Count > 0 Then
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "none")
                SpnAadharmsg.InnerText = ""
                dvgrid.Style.Add(HtmlTextWriterStyle.Display, "block")
                grdview.Visible = True
                grdview.DataSource = dtIIN
                grdview.DataBind()
            Else
                grdview.Visible = False
                divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                SpnAadharmsg.InnerText = "No Record Found"
                txtCustomerID.Focus()
            End If
        Catch ex As Exception
            AssignParam = Nothing
            MFD_Bll.Dispose()
            Throw New ApplicationException(ex.ToString)
        Finally
            AssignParam = Nothing
            MFD_Bll.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' Sundaram Check Avalibity
    ''' </summary>
    ''' <remarks></remarks>
    Sub GenerateCHECKxml()
        Dim req As HttpWebRequest = Nothing
        Dim res As HttpWebResponse = Nothing
        Dim xmlInput As XmlDocument = New XmlDocument
        Dim xmlOutput As XmlDocument = New XmlDocument
        Dim _url As String = "https://demo.sundarambnpparibasfs.in/mobileapi/services/uid/v1/CheckStatus/"
        Dim _ds As DataSet
        Dim strInputString As New StringBuilder
        SpnAadharmsg.InnerHtml = ""
        Try
            With strInputString
                '.Append("<?xml version=""1.0"" encoding=""UTF-8""?>")
                .Append("<UID_REQ>")
                .AppendFormat("<USER_ID>{0}</USER_ID>", "UIDCHANNEL")
                .AppendFormat("<PASSWORD>{0}</PASSWORD>", "emp123")
                .AppendFormat("<SESSION_ID>{0}</SESSION_ID>", sess_id.ToString())
                .AppendFormat("<RETN_URL>{0}</RETN_URL>", "")
                .AppendFormat("<PAN_PEKRN>{0}</PAN_PEKRN>", txtPAN1.Text.ToUpper().ToString())
                .AppendFormat("<REQUEST_TYPE>{0}</REQUEST_TYPE>", "1")
                .AppendFormat("<AADHAAR_NAME>{0}</AADHAAR_NAME>", txtAadharName.Text)
                .AppendFormat("<DOB>{0}</DOB>", txtDOB.Text)
                .AppendFormat("<GENDER>{0}</GENDER>", rdlgender.SelectedValue)
                .AppendFormat("<MOBILE_NO>{0}</MOBILE_NO>", txtmobileno.Text)
                .AppendFormat("<EMAIL_ID>{0}</EMAIL_ID>", txtemail.Text)
                .Append("</UID_REQ>")
            End With
            xmlInput.LoadXml(strInputString.ToString())
            req = DirectCast(WebRequest.Create(_url), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "application/xml; charset=utf-8"
            Dim sXml As String = xmlInput.InnerXml
            req.ContentLength = sXml.Length
            Dim sw = New StreamWriter(req.GetRequestStream())
            sw.Write(sXml)
            sw.Close()
            Dim resp As HttpWebResponse = TryCast(req.GetResponse(), HttpWebResponse)
            Dim dsRedeemres As New DataSet
            If resp.StatusCode = HttpStatusCode.OK Then
                Dim stream As StreamReader = New StreamReader(resp.GetResponseStream())
                Try
                    Dim text As String = stream.ReadToEnd()
                    xmlOutput.LoadXml(text.ToString())

                    Dim elemList As XmlNodeList = xmlOutput.GetElementsByTagName("RESPONSE_CODE")
                    Dim i As Integer
                    For i = 0 To elemList.Count - 1
                        divAadharmsg.Style.Add(HtmlTextWriterStyle.Display, "block")
                        SpnAadharmsg.InnerHtml += elemList(i).InnerXml
                    Next i
                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    'Protected Sub grdview_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
    '    Try
    '        If e.CommandName = "Select" Then
    '            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
    '            Dim row As GridViewRow = grdview.Rows(rowIndex)
    '            txtPAN.Text = row.Cells(1).Text
    '            txtAadharName.Text = row.Cells(2).Text
    '            txtemail.Text = row.Cells(4).Text
    '            txtmobileno.Text = row.Cells(5).Text

    '            InsertDetail()
    '        End If
    '        If e.CommandName = "Check" Then
    '            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
    '            Dim row As GridViewRow = grdview.Rows(rowIndex)
    '            txtPAN.Text = row.Cells(1).Text
    '            txtAadharName.Text = row.Cells(2).Text
    '            txtemail.Text = row.Cells(4).Text
    '            txtmobileno.Text = row.Cells(5).Text

    '            InsertDetail()
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class