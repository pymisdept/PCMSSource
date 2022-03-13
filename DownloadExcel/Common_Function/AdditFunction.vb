Imports System.IO


Module AdditFunction
    Public DebugMode As Boolean = True
    Public Sub WriteDebug(ByVal s As String)
        Dim dir As New DirectoryInfo(System.Environment.CurrentDirectory & "\\Debug\\" & DateTime.Now.ToString("yyyy-MM-dd"))
        Dim sw As StreamWriter
        If DebugMode Then
            Try
                If dir.Exists = False Then
                    dir.Create()
                End If
            Catch ex As Exception

            End Try
            sw = New StreamWriter(dir.FullName & "\\" & DateTime.Now.ToString("yyyyMMdd_hhmm") & ".txt", True)
            Try
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") & ": Check Point: " & s)
                sw.Close()
            Catch ex As Exception

            End Try
        End If

    End Sub
    Public Function isProcessExists(ByVal vProcessName As String) As Boolean

        Try

            Dim vCurrProcessID = System.Diagnostics.Process.GetCurrentProcess.Id()
            Dim vProcess As Process() = System.Diagnostics.Process.GetProcessesByName(vProcessName)
            If Not vProcess Is Nothing Then
                If vProcess.Length > 1 Then
                    Return True
                Else
                    Return False
                End If
            Else

                Return False
            End If
        Catch ex As Exception

            Return False
        End Try


    End Function
End Module
