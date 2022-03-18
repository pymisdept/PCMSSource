Imports System.IO
Module AdditFunction
    Public Sub WriteDebug(ByVal s As String, Optional ByVal ModuleCode As String = "")
        Dim dir As New DirectoryInfo(System.Environment.CurrentDirectory & "\\Debug\\" & DateTime.Now.ToString("yyyy-MM-dd"))
        Dim sw As StreamWriter
        Dim args As String

        Try
            If dir.Exists = False Then
                dir.Create()
            End If
        Catch ex As Exception

        End Try

        Try

            If System.Environment.GetCommandLineArgs.Length = 2 Then
                args = "_M" & System.Environment.GetCommandLineArgs(1)
            Else
                args = ""
            End If

            If ModuleCode = "" Then
                sw = New StreamWriter(dir.FullName & "\\" & DateTime.Now.ToString("yyyyMMdd_HHmm") & args & ".txt", True)
            Else
                sw = New StreamWriter(dir.FullName & "\\" & DateTime.Now.ToString("yyyyMMdd_HHmm") & args & "_" & ModuleCode & ".txt", True)
            End If

            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & ": Check Point: " & s)
            sw.Close()
        Catch ex As Exception
            If sw IsNot Nothing Then sw.Dispose()
        End Try

    End Sub
End Module
