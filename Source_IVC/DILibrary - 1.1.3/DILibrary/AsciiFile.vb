Imports System.IO


Namespace CPSLIB.Debug
    Public Class AsciiFile

        Dim _FullFileName As String
        Dim _FilePath As String
        Dim _FileName As String
        Dim _Fi As FileInfo
        Dim _FileExists As Boolean
        Dim _sr As StreamReader
        Dim _sw As StreamWriter
        Dim _ReadOnly As Boolean


        Dim _lineContent As ArrayList


        Public Sub New(ByVal _FullFileName As String)

            If _FullFileName <> String.Empty Then

                Me._FullFileName = _FullFileName
            Else
                'Write Log

            End If
            Initizial()
        End Sub

#Region "Process"

        Private Function Initizial() As Boolean
            Try
                _Fi = New FileInfo(_FullFileName)
                If _Fi.Exists Then
                    _FilePath = _Fi.Directory.FullName
                    _FileName = _Fi.Name
                    _ReadOnly = _Fi.IsReadOnly
                    ' Get Content when file exists
                    ReadFileContent()
                Else
                    ' File Does Not Found

                    '_Fi.Create()

                End If

            Catch ex As Exception
                ' Throw CPSLIB Exception
            End Try
        End Function

        Private Sub ReadFileContent()

            _lineContent = New ArrayList
            Try

                _sr = New StreamReader(_Fi.FullName, System.Text.Encoding.ASCII)

                While _sr.Peek() > -1
                    _lineContent.Add(_sr.ReadLine())

                End While
                _sr.Close()
            Catch ex As Exception
                ' Throw CPSLib Exception

            End Try

        End Sub

        Public Sub WriteLine(ByVal s As String)
            Dim _sw As StreamWriter
            Try

                'If Not _Fi.IsReadOnly Then
                _sw = New StreamWriter(_Fi.FullName, True)
                _sw.WriteLine(s)
                _sw.Close()
                'End If
            Catch ioex As IOException

            Catch ex As Exception

            End Try




        End Sub
#End Region
#Region "Property"

        Public ReadOnly Property Information() As FileInfo
            Get
                Return _Fi
            End Get
        End Property

        Public ReadOnly Property StreamReader() As StreamReader
            Get
                Return _sr
            End Get
        End Property

        Public ReadOnly Property FileContent() As ArrayList
            Get
                Return _lineContent
            End Get
        End Property

#End Region
    End Class

End Namespace

