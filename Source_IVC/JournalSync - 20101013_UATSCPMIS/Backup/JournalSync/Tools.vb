Public Module Tools
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
