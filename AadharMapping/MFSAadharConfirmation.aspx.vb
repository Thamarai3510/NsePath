Imports System.Xml
Imports System.Net

Public Class MFSAadharConfirmation
    Inherits CommonFunctions


    Dim smartenc As New SmartEncript
    Dim nvKey As NameValueCollection

    Dim AssignParam As MFD_TRXN_Select_Property
    Dim InsertParam As New MFD_InsupdProperty
    Dim MFD_Bll As New MFD_BLL

    Dim dtAadhar As DataTable
    Dim dtConfig As DataTable
    Dim strEncryptedData As String = String.Empty
    Dim strDecryptedData As String = String.Empty
    Dim strPageName As String = "Aadhar Confirmation"
    Dim strPAN, strSourceProg, struser_code, strSess_ID, strRef_No, strReq_Type, strReg_ID As String
    Dim strRetrun_SessID, strRetrun_PAN, strRetrun_RespCode, strRetrun_Name, strRetrun_DOB, strRetrun_Gender, strRetrun_mob, strRetrun_type As String
    Dim strRet_msg, StrReq_msg As String
    Dim strResult As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Try
                strRetrun_RespCode = ModuleInputFilterlines(Request.Form("RESPONSE_CODE"))
                strRetrun_SessID = ModuleInputFilterlines(Request.Form("SESSION_ID"))
                strRetrun_PAN = ModuleInputFilterlines(Request.Form("PAN_PEKRN"))
                strRetrun_Name = ModuleInputFilterlines(Request.Form("NAME"))
                strRetrun_DOB = ModuleInputFilterlines(Request.Form("DOB"))
                strRetrun_Gender = ModuleInputFilterlines(Request.Form("GENDER"))
                strRetrun_mob = ModuleInputFilterlines(Request.Form("MOBILE_NO"))
                If strRetrun_mob = String.Empty AndAlso strRetrun_SessID.Split("|")(2) = "KARVY" Then
                    strRetrun_mob = ModuleInputFilterlines(Request.Form("MOBILENO"))
                End If
                strRetrun_type = ModuleInputFilterlines(Request.Form("REQUEST_TYPE"))
            Catch ex As Exception

            End Try
            If strRetrun_RespCode <> String.Empty Then
                strRet_msg = "RESPONSE_CODE=" & strRetrun_RespCode & "SESSION_ID=" & strRetrun_SessID & "PAN_PEKRN=" & strRetrun_PAN & "NAME=" & strRetrun_Name & "DOB=" & strRetrun_DOB & "GENDER=" & strRetrun_Gender & "MOBILE_NO=" & strRetrun_mob
                InsertActivityLog("AADHARMAPING", "", "AADHAR_RESPONSE", strRet_msg, strPageName)
                dverror.Visible = True
                spnloadmsg.InnerHtml = ErrorMessage()
                'If strRetrun_type = "2" Then
                UpdateStatus(strRetrun_SessID)
                'End If
                Dim script_msg As String = spnloadmsg.InnerHtml
                'window.parent.postMessage({message: '" + Resqrystr + "'}, '" + appPath + "');
                'window.parent.postMessage({message: '" + script_msg + "'}
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GET", "javascript:window.parent.postMessage({message: '" + script_msg + "'};", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GET", "javascript:GetAadhaarmsg('" + script_msg + "');", True)
            Else
                strEncryptedData = Request.QueryString.ToString()
                strDecryptedData = smartenc.decrypt(Server.UrlDecode(strEncryptedData))
                nvKey = ParseQueryString(strDecryptedData, "")
                strPAN = ModuleInputFilterlines(Trim(nvKey("PAN_PEKRN")))
                strSourceProg = ModuleInputFilterlines(Trim(nvKey("SourceProgram")))
                struser_code = ModuleInputFilterlines(Trim(nvKey("UserCode")))
                strSess_ID = ModuleInputFilterlines(Trim(nvKey("SESS_ID")))
                strRef_No = ModuleInputFilterlines(Trim(nvKey("REF_NO")))
                strReq_Type = ModuleInputFilterlines(Trim(nvKey("REQUEST_TYPE")))
                strReg_ID = ModuleInputFilterlines(Trim(nvKey("REGISTRAR_ID")))
                GetConfigDetail()
                If strReq_Type = "2" Then
                    UpdateRTA()
                    GetAadharDetail()
                ElseIf strReq_Type = "1" Then
                    PAN_PEKRN.Value = strPAN
                    RETN_URL.Value = Request.Url.GetLeftPart(UriPartial.Authority) + IIf(Request.ApplicationPath.Length > 1, Request.ApplicationPath, "") & "/AadharMapping/MFSAadharConfirmation.aspx"
                    REQUEST_TYPE.Value = "1"
                    SESSION_ID.Value = strRef_No & "|" & strSourceProg & "|" & strReg_ID
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "POST", "javascript:AadharGateway();", True)
                End If
            End If
        Catch exe As Threading.ThreadAbortException
        Catch ex As Exception
            Call_ErrorPage(strSess_ID, struser_code, "", ex.ToString, "Error Occured In AADHAAR Conf Page Load", strPageName)
        End Try
    End Sub
    Sub GetAadharDetail()
        AssignParam = New MFD_TRXN_Select_Property
        MFD_Bll = New MFD_BLL
        Try
            With AssignParam
                .FLAG = "GET_AADHAR_DETAILS"
                .FolioNo = strPAN.ToString().ToUpper()
                .UserCode = struser_code
                .To_Trxn_No = strRef_No
            End With
            dtAadhar = MFD_Bll.GenerateData(AssignParam, struser_code, strPageName, "")
            If dtAadhar.Rows.Count > 0 Then
                SESSION_ID.Value = strRef_No & "|" & strSourceProg & "|" & strReg_ID
                PAN_PEKRN.Value = dtAadhar.Rows(0).Item("PAN_PEKRN").ToString
                RETN_URL.Value = Request.Url.GetLeftPart(UriPartial.Authority) + IIf(Request.ApplicationPath.Length > 1, Request.ApplicationPath, "") & "/AadharMapping/MFSAadharConfirmation.aspx"
                REQUEST_TYPE.Value = "2"
                AADHAAR_NAME.Value = dtAadhar.Rows(0).Item("AADHAAR_NAME").ToString
                DOB.Value = dtAadhar.Rows(0).Item("DATE_OF_BIRTH").ToString
                GENDER.Value = dtAadhar.Rows(0).Item("GENDER").ToString
                MOBILE_NO.Value = dtAadhar.Rows(0).Item("MOBILE_NO").ToString
                EMAIL_ID.Value = dtAadhar.Rows(0).Item("EMAIL_ID").ToString
                If (strReg_ID = "KARVY") Then
                    MOBILENO.Value = dtAadhar.Rows(0).Item("MOBILE_NO").ToString
                End If
                If (strReg_ID = "FTI") Then
                    Dim strs As String
                    strs = ""
                    strs = "{userId:" & USER_ID.Value & ",password:" & PASSWORD.Value & ",returnUrl:" & RETN_URL.Value & ",sessionId:" & SESSION_ID.Value & ",PAN:" & PAN_PEKRN.Value & ",requestType:" & REQUEST_TYPE.Value & "}"
                    reqObj.Value = strs
                    '<input name="reqObj" type="hidden"
                    'value='{"userId":"XXXXX","password":"XXXXX","returnUrl":"XXXXX","sessionId":"XXXXX","pan":"XXXX
                    'XXXXXX","requestType": "1"}' />
                End If

                strRet_msg = "PAN_PEKRN=" & PAN_PEKRN.Value & "AADHAAR_NAME=" & AADHAAR_NAME.Value & "DATE_OF_BIRTH=" & DOB.Value & "GENDER=" & strRetrun_Name & "DOB=" & strRetrun_DOB & "GENDER=" & GENDER.Value & "MOBILE_NO=" & MOBILE_NO.Value & "EMAIL_ID=" & EMAIL_ID.Value & "URL=" & RETN_URL.Value & "RETURN_TYPE=" & REQUEST_TYPE.Value
                InsertActivityLog("AADHARMAPING", "", "AADHAR_REQUEST", strRet_msg, strPageName)

                'If (strReg_ID = "SUN") Then
                'GeneratePOSTxml()
                '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "POST", "javascript:XMLGateway();", True)
                'Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "POST", "javascript:AadharGateway();", True)
                'End If
            Else
                dverror.Visible = True
                spnloadmsg.InnerHtml = "Record msimatch"
            End If
        Catch ex As Exception
            WriteToLog(ex.ToString(), "GetAadharDetail", strPageName)
            Throw New ApplicationException(ex.ToString)
        Finally
            AssignParam = Nothing
            MFD_Bll = Nothing
        End Try
    End Sub
    Sub GetConfigDetail()
        AssignParam = New MFD_TRXN_Select_Property
        MFD_Bll = New MFD_BLL
        Try
            With AssignParam
                .FLAG = "GET_AADHAR_CONFIG"
                .REGISTRAR_ID = strReg_ID.ToString().ToUpper()
            End With
            dtConfig = MFD_Bll.GenerateData(AssignParam, struser_code, strPageName, "")
            If dtConfig.Rows.Count > 0 Then
                USER_ID.Value = dtConfig.Rows(0).Item("USER_ID").ToString
                PASSWORD.Value = dtConfig.Rows(0).Item("PASSWORD").ToString
                URL.Value = dtConfig.Rows(0).Item("POST_URL").ToString
            Else
                dverror.Visible = True
                spnloadmsg.InnerHtml = "No Record found - GetConfigDetail"
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.ToString)
        Finally
            AssignParam = Nothing
            MFD_Bll = Nothing
        End Try
    End Sub
    Function ErrorMessage() As String
        Dim strmsg As String
        Try
            If strRetrun_RespCode = "AL000" Then
                strmsg = "Aadhaar information is submitted successfully."
            ElseIf strRetrun_RespCode = "AL001" Then
                strmsg = "Aadhaar has already been linked done for this PAN."
            ElseIf strRetrun_RespCode = "AL002" Then
                strmsg = "CP to display suitable message at their end."
            ElseIf strRetrun_RespCode = "AL003" Then
                strmsg = "Given Aadhaar Number is not valid, please input your correct Aadhaar Number. Maximum number of attempts 3."
            ElseIf strRetrun_RespCode = "AL004" Then
                strmsg = "Please Enter Name / less than 3 characters / special characters found."
            ElseIf strRetrun_RespCode = "AL005" Then
                strmsg = "Please Enter Valid PAN."
            ElseIf strRetrun_RespCode = "AL006" Then
                strmsg = "Please Enter Valid PAN."
            ElseIf strRetrun_RespCode = "AL007" Then
                strmsg = "CP to display suitable message at their end."
            ElseIf strRetrun_RespCode = "AL008" Then
                strmsg = "Page idle for more than 3 mins."
            Else
                strmsg = strRetrun_RespCode
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.ToString)
        End Try
        Return strmsg
    End Function
    Sub UpdateStatus(ByVal strReference_No As String)
        InsertParam = New MFD_InsupdProperty
        MFD_Bll = New MFD_BLL
        Try
            With InsertParam
                .Flag = "UPDATE_PAN_MAPPING"
                If strRetrun_type = "2" Then
                    .remarks = strRetrun_RespCode
                Else
                    .remarks = strRetrun_RespCode.ToUpper()
                End If
                .user_trxnno = strReference_No.Split("|")(0)
                .Folio = strRetrun_PAN.ToString().ToUpper()
                .Usercode = strReference_No.Split("|")(2)
            End With
            strResult = MFD_Bll.MFDInsertUpdate(InsertParam, struser_code, strPageName)

        Catch ex As Exception
            WriteToLog(ex.ToString(), "UpdateStatus", strPageName)
            Throw New ApplicationException(ex.ToString)
        Finally
            strResult = ""
            InsertParam = Nothing
            MFD_Bll = Nothing
        End Try
    End Sub
    Sub UpdateRTA()
        InsertParam = New MFD_InsupdProperty
        MFD_Bll = New MFD_BLL
        Try
            With InsertParam
                .Flag = "UPDATE_RTA_PAN"
                .Usercode = strReg_ID
                .user_trxnno = strRef_No
                .Folio = strPAN.ToString().ToUpper()
            End With
            strResult = MFD_Bll.MFDInsertUpdate(InsertParam, struser_code, strPageName)
        Catch ex As Exception
            WriteToLog(ex.ToString(), "UpdateRTA", strPageName)
            Throw New ApplicationException(ex.ToString)
        Finally
            strResult = ""
            InsertParam = Nothing
            MFD_Bll = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Sundaram Submission
    ''' </summary>
    ''' <remarks></remarks>
    Sub GeneratePOSTxml()
        Dim req As HttpWebRequest = Nothing
        Dim res As HttpWebResponse = Nothing
        Dim xmlInput As XmlDocument = New XmlDocument
        Dim _url As String = "https://demo.sundarambnpparibasfs.in/mobileapi/services/uid/v1/CheckStatus/"
        Dim _ds As DataSet
        Dim strInputString As New StringBuilder

        Try

            With strInputString
                '.Append("<?xml version=""1.0"" encoding=""UTF-8""?>")
                .Append("<UID_REQ>")
                .AppendFormat("<USER_ID>{0}</USER_ID>", USER_ID.Value.ToString())
                .AppendFormat("<PASSWORD>{0}</PASSWORD>", PASSWORD.Value.ToString())
                .AppendFormat("<SESSION_ID>{0}</SESSION_ID>", SESSION_ID.Value.ToString())
                .AppendFormat("<RETN_URL>{0}</RETN_URL>", RETN_URL.Value.ToString())
                .AppendFormat("<PAN_PEKRN>{0}</PAN_PEKRN>", PAN_PEKRN.Value.ToString())
                .AppendFormat("<REQUEST_TYPE>{0}</REQUEST_TYPE>", REQUEST_TYPE.Value.ToString())
                .AppendFormat("<AADHAAR_NAME>{0}</AADHAAR_NAME>", AADHAAR_NAME.Value.ToString())
                .AppendFormat("<DOB>{0}</DOB>", DOB.Value.ToString())
                .AppendFormat("<GENDER>{0}</GENDER>", GENDER.Value.ToString())
                .AppendFormat("<MOBILE_NO>{0}</MOBILE_NO>", MOBILE_NO.Value.ToString())
                .AppendFormat("<EMAIL_ID>{0}</EMAIL_ID>", EMAIL_ID.Value.ToString())
                .Append("</UID_REQ>")
            End With
            hdnXML.Value = strInputString.ToString()
            'xmlInput.LoadXml(strInputString.ToString())
            'req = DirectCast(WebRequest.Create(_url), HttpWebRequest)
            'req.Method = "POST"
            'req.ContentType = "application/xml; charset=utf-8"
            'Dim sXml As String = xmlInput.InnerXml
            'req.ContentLength = sXml.Length
            'Dim sw = New StreamWriter(req.GetRequestStream())
            'sw.Write(sXml)
            'sw.Close()
            'Dim resp As HttpWebResponse = TryCast(req.GetResponse(), HttpWebResponse)
            'Dim dsRedeemres As New DataSet
            'If resp.StatusCode = HttpStatusCode.OK Then
            '    Dim stream As StreamReader = New StreamReader(resp.GetResponseStream())
            '    Try
            '        Dim text As String = stream.ReadToEnd()
            '        'Textarea2.Text = text
            '        _ds.ReadXml(stream)
            '    Catch ex As Exception
            '    End Try
            'End If
        Catch ex As Exception

        End Try
    End Sub
End Class