Imports System.Xml
Imports System.Net.Http
Imports System.Net.Http.Headers

Public Class MFSAadharMapping_test
    Inherits CommonFunctions


#Region "Variable Declaration"
    Dim strQueryString As String = String.Empty
    Dim strDecryptedData As String = String.Empty

    Dim InsertParam As New MFD_InsupdProperty
    Dim MFD_Bll As New MFD_BLL

    Dim smtEncrypt As New SmartEncript
    Dim nvCollection As NameValueCollection
    Public sess_id As String
    Dim allstr, allstrarr() As String
    Dim strPageName As String = "Aadhar Seeding"
    Dim StrUser_Code, strServerHost, strBrokName, strBrokCode, strUser_level, StrUser_type, strBrokType As String
    Public strReturn_URL, strMSG As String

    Dim strRetrun_SessID, strRetrun_PAN, strRetrun_RespCode, strRetrun_Name, strRetrun_DOB, strRetrun_Gender, strRetrun_mob As String
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
                strQueryString = ModuleInputFilterlines(Request.QueryString.ToString())
                strDecryptedData = smtEncrypt.decrypt(Server.UrlDecode(strQueryString))
                nvCollection = ParseQueryString(strDecryptedData, "")
                sess_id = ModuleInputFilterlines(Trim(nvCollection("sess_id")))
                allstr = ModuleInputFilterlines(Trim(nvCollection("allstr")))
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
            End If
        Catch ex1 As Threading.ThreadAbortException
        Catch ex As Exception
            Call_ErrorPage(sess_id, "", "", ex.ToString, "Error Occured In Page Load", strPageName)
        End Try

    End Sub

    Protected Sub btnTest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTest.Click
        Try
            'Dim data As String = "TEST&PASSWORD=TEST&"
            'data += "SESSION_ID =12312133 &"
            'data += "RETN_URL = http://124.124.236.198/MFDLogin.aspx&"
            'data += "PAN_PEKRN=BLEPM0174P&"
            'data += "REQUEST_TYPE =1&" '1 or 2
            'data += "AADHAAR_NAME =556574062817&"
            'data += "DOB =16-SEP-1988&"
            'data += "GENDER =M&" 'Gender, the value should be M-Male, F-Female, T-Transgender
            'data += "MOBILE_NO =9791465323" 'Mobile number of the investor. Only 10 digit number without prefix of 0 or +91.
            'Dim request As HttpWebRequest = DirectCast(WebRequest.Create("https://uat.rtamicrosite/specialservices/aadhaarmicropage.aspx"), HttpWebRequest)
            'request.Method = "POST"
            ''request.KeepAlive = False
            'request.ContentType = "application/xml; charset=UTF-8"
            'ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
            'Dim json_bytes() As Byte = Encoding.UTF8.GetBytes(data)
            'request.Accept = "application/xml"
            'request.ContentLength = json_bytes.Length
            'Dim stream As Stream = request.GetRequestStream()
            'stream.Write(json_bytes, 0, json_bytes.Length)
            'stream.Close()
            'Dim WebResp As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            ''"{"id":"ENA180115144729973K3PUWPD7EKS221","enach_type":"CREATE","status":"partial"}"
            'Dim dataStream As IO.Stream = WebResp.GetResponseStream()
            'Dim reader As New IO.StreamReader(dataStream)          ' Open the stream using a StreamReader for easy access.
            'Dim getresponse As String = reader.ReadToEnd()
            'If getresponse.Substring(0, 1).ToString = "[" Then
            '    'do nothing
            'Else
            '    getresponse = (Convert.ToString("[") & getresponse) + "]"
            'End If
            'working start 
            'Dim url As String = "https://uat2.camsonline.com/InvestorServices/AMCandCPAadhaar.aspx"
            'Dim client As HttpClient = New HttpClient()
            'Dim data As String = "AL_STERL&PASSWORD=dF$8Bk9a&"
            'data += "SESSION_ID =" & sess_id & "&"
            'data += "RETN_URL = " & strReturn_URL & "&"
            'data += "PAN_PEKRN=BLEPM0174P&"
            'data += "REQUEST_TYPE =1&" '1 or 2
            'data += "AADHAAR_NAME =Mohamed Rasul S&"
            'data += "DOB =16-SEP-1988&"
            'data += "GENDER =M&" 'Gender, the value should be M-Male, F-Female, T-Transgender
            'data += "MOBILE_NO =9791465323" 'Mobile number of the investor. Only 10 digit number without prefix of 0 or +91.
            'Dim queryString As StringContent = New StringContent(data)
            'client.DefaultRequestHeaders.Accept.Clear()
            'client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/xml"))
            'Dim response As HttpResponseMessage = client.PostAsync(New Uri(url), queryString).Result
            'If (response.IsSuccessStatusCode) Then
            '    Dim content = response.Content.ReadAsStringAsync().Result
            'End If

            'working end 

            'string data = "<Userid> & PASSWORD = <Password> & SESSION_ID =<SessionID>& RETN_URL = <Return URL> & PAN_PEKRN=<PAN OR PEKRN>& REQUEST_TYPE =<RequestType1>"; (CPs to pass other parameters according to request type mentioned in above table )
            'StringContent queryString = new StringContent(data);
            'client.DefaultRequestHeaders.Accept.Clear();
            'client.DefaultRequestHeaders.Accept.Add(
            'new MediaTypeWithQualityHeaderValue("application/xml"));
            'HttpResponseMessage response = client.PostAsync(new Uri(url), queryString).Result;
            '            If (Response.IsSuccessStatusCode) Then
            '{
            'var content = response.Content.ReadAsStringAsync().Result;
            '}
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnSubmit1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Try
            Dim strMSG As String
            If Validation() = False Then
                Exit Sub
            End If
            InsertDetail()
            'Dim collections As New NameValueCollection()
            'collections.Add("USER_ID", "AL_STERL")
            'collections.Add("PASSWORD", "dF$8Bk9a")
            'collections.Add("SESSION_ID", sess_id)
            'collections.Add("RETN_URL", strReturn_URL)
            'collections.Add("PAN_PEKRN", txtPAN.Text.ToUpper.Trim())
            'collections.Add("REQUEST_TYPE", "2")
            'collections.Add("AADHAAR_NAME", txtAadharName.Text)
            'collections.Add("DOB", txtDOB.Text.ToUpper.Trim())
            'collections.Add("GENDER", rdlgender.SelectedValue)
            'collections.Add("EMAIL_ID", txtemail.Text)
            'collections.Add("MOBILE_NO", txtmobileno.Text)
            'Dim remoteUrl As String = "https://uat2.camsonline.com/InvestorServices/AMCandCPAadhaar.aspx"
            ''<form action="https://uat.rtamicrosite/specialservices/aadhaarmicropage.aspx" method="post" enctype="multipart/form-data" name="cbsulogin" target="_blank">
            'Dim html As String = "<html><head>"
            'html += "</head><body onload='document.forms[0].submit()'>"
            'html += String.Format("<form name='cbsulogin' method='POST' enctype='multipart/form-data' action='{0}'>", remoteUrl)
            'For Each key As String In collections.Keys
            '    html += String.Format("<input name='{0}' type='hidden' value='{1}'>", key, collections(key))
            '    strMSG += key & " - " & collections(key) & "|"
            'Next
            'html += "</form></body></html>"
            'InsertActivityLog("AADHAR_SEEDING", strBrokCode, "REQUEST", strMSG, strPageName)
            'Response.Clear()
            ''Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1")
            ''Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1")
            ''Response.Charset = "ISO-8859-1"
            'Response.Write(html)
            'Response.End()
        Catch ex As Exception

        End Try
    End Sub
    Sub InsertDetail()
        InsertParam = New MFD_InsupdProperty
        MFD_Bll = New MFD_BLL
        Dim strResult As String = String.Empty
        Try
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
                .user_trxnno = rdlgender.SelectedValue
                .instrm_branch = txtmobileno.Text
                .Usercode = txtemail.Text
            End With
            strResult = MFD_Bll.MFDInsertUpdate(InsertParam, StrUser_Code, strPageName)
            If strResult Then

            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.ToString)
        Finally
            InsertParam = Nothing
            MFD_Bll.Dispose()
        End Try
    End Sub

    Function Validation() As Boolean
        Dim boolVal As Boolean = True
        Try
            If (txtPAN.Text = String.Empty) Then
                lblErrorMsg.Text = "Please enter PAN"
                boolVal = False
                Exit Function
            ElseIf (txtPAN.Text <> String.Empty) AndAlso (chkPANExempt.Checked = False) Then
                If Regex.Match(txtPAN.Text.ToUpper.Trim, "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$").Success() = False Then
                    lblErrorMsg.Text = "Invalid PAN Format"
                    boolVal = False
                    Exit Function
                End If
            End If
            If (txtDOB.Text = String.Empty) Then
                lblErrorMsg.Text = "Please Select Date of Birth"
                boolVal = False
                Exit Function
            End If
            If (rdlgender.SelectedValue = String.Empty) Then
                lblErrorMsg.Text = "Please select a Gender!"
                boolVal = False
                Exit Function
            End If
            If (txtmobileno.Text = String.Empty) Then
                lblErrorMsg.Text = "Please enter Mobile number"
                boolVal = False
                Exit Function
            End If
            boolVal = True
        Catch ex As Exception
            boolVal = False
        End Try
        Return boolVal
    End Function

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    '    Try
    '        ScriptManager.RegisterStartupScript(Me, GetType(Page), "POST_METHOD", "javascript:AadharGateway();", True)
    '    Catch ex As Exception

    '    End Try
    'End Sub

End Class