Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Dim pathroot As String
        pathroot = Server.MapPath("")
        FileOpen(2, pathroot & "/settings.ini", OpenMode.Input)
        Dim strS, strPara() As String
        Do While Not EOF(2)
            strS = LineInput(2)
            strPara = Split(strS, "=", 2)
            If strPara(0) = "[SERVER]" Then
                Session("Server") = strPara(1)
            ElseIf strPara(0) = "[USERID]" Then
                Session("UserID") = strPara(1)
            ElseIf strPara(0) = "[USERPW]" Then
                Session("UserPW") = strPara(1)
            ElseIf strPara(0) = "[B1DATABASE]" Then
                Session("B1Database") = strPara(1)
            ElseIf strPara(0) = "[REPORTDATADB]" Then
                Session("ReportDataDB") = strPara(1)
            ElseIf strPara(0) = "[REPORTFOLDER]" Then
                Session("ReportFolder") = Server.MapPath("") + "\" + strPara(1)
            End If

            pServer = Session("Server")
            pUserID = Session("UserID")
            pUserPW = Session("UserPW")
            pB1Database = Session("B1Database")
            pReportDataDB = Session("ReportDataDB")
        Loop
        FileClose(2)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        Session("Server") = Nothing
        Session("UserID") = Nothing
        Session("UserPW") = Nothing
        Session("B1Database") = Nothing
        Session("ReportDataDB") = Nothing
        Session("ReportFolder") = Nothing
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        Session("Server") = Nothing
        Session("UserID") = Nothing
        Session("UserPW") = Nothing
        Session("B1Database") = Nothing
        Session("ReportDataDB") = Nothing
        Session("ReportFolder") = Nothing
    End Sub

End Class