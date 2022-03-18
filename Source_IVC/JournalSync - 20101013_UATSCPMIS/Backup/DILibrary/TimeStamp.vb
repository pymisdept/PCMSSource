Imports System.IO
Namespace CPSLIB.Debug
    Public Class TimeSet

        Public Enum Status
            Start = 1
            Finish = 2
        End Enum
        Public Const File As String = "timeset"

        Public Shared Function isLog() As Boolean

            If System.IO.File.Exists(System.Environment.CurrentDirectory & "\TimeStamp.txt") Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function Start() As String
            'Dim _file As AsciiFile
            'Dim _di As DirectoryInfo
            'Dim _Path As String = System.Environment.CurrentDirectory
            'Dim _SubFolder As String = "TimeStamp"
            'Dim _FullPath As String = String.Empty
            'Dim _FileCnt As Integer = 0
            'Dim _FileName As String = ""
            'If isLog() Then
            '    Try
            '        _FullPath = _Path & "\" & _SubFolder & "\" & DateTime.Now.ToString("yyyy-MM-dd")
            '        _di = New DirectoryInfo(_FullPath)
            '        If _di.Exists = False Then
            '            _di.Create()
            '        End If
            '        _FileCnt = _di.GetFiles.Count + 1
            '        _file = New AsciiFile(System.Environment.CurrentDirectory & "\" & File)

            '        If _file.Information.Exists Then
            '            _file.Information.Delete()


            '        End If
            '        _FileName = _FullPath & "\" & "TimeStamp-" & _FileCnt.ToString("d6") & ".tm"
            '        _file.WriteLine(_FileName)
            '    Catch ex As Exception
            '    End Try
            'End If

            'Return _FileName
            Return Nothing
        End Function

        Public Shared Sub Finish()
            'Dim _file As AsciiFile
            'If isLog() Then
            '    Try
            '        _file = New AsciiFile(System.Environment.CurrentDirectory & "\" & File)
            '        If _file.Information.Exists Then
            '            _file.Information.Delete()
            '        End If
            '    Catch ex As Exception
            '    End Try
            'End If

        End Sub

        Public Shared Function ReadTimeStampFile() As String
            ' Check Time Stamp file exists or not
            'Dim _file As AsciiFile
            'Dim _Path As String = System.Environment.CurrentDirectory
            'Try
            '    _file = New AsciiFile(_Path & "\" & TimeSet.File)
            '    If _file.Information.Exists Then
            '        Return _file.FileContent(0)
            '    Else
            '        Return Nothing
            '    End If
            'Catch ex As Exception
            '    Return Nothing
            'End Try
            Return Nothing
        End Function

        Public Shared Sub Log(ByVal strFunc As String, ByVal _Status As TimeSet.Status)
            'Dim _sw As AsciiFile


            'Dim _Path As String = System.Environment.CurrentDirectory
            'Dim _SubFolder As String = "TimeStamp"
            'Dim _FullPath As String = String.Empty
            'Dim _di As DirectoryInfo
            'Dim _LogFileName As String

            'If isLog() Then

            '    _LogFileName = TimeSet.ReadTimeStampFile
            '    If _LogFileName = Nothing Then
            '        _LogFileName = TimeSet.Start()

            '    End If

            '    Try

            '        _sw = New AsciiFile(_LogFileName)

            '        Select Case _Status
            '            Case Status.Finish
            '                _sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff") & vbTab & "Finished" & vbTab & strFunc)
            '            Case Status.Start
            '                _sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff") & vbTab & "Starting" & vbTab & strFunc)
            '        End Select

            '    Catch ex As Exception

            '    End Try
            'End If

        End Sub


    End Class

End Namespace
