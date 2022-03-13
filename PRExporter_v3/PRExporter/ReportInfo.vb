Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class ReportInfo
    Private mUsername As String
    Private mPassword As String
    Private mReportPath As String
    Private mOutputPath As String
    Private mServerName As String
    Private mDatabaseName As String
    Private mOutputFormat As String
    Private mReportParameterString As List(Of String)

    Private mReportParameterName As List(Of String)
    Private mReportParameterValue As List(Of String)
    Private mGetHelp As Boolean

    Public Sub New(ByVal parameters As String())
        Dim defaultFileFormat As String = "txt"
        mGetHelp = False

        mReportParameterString = New List(Of String)()
        mReportParameterName = New List(Of String)()
        mReportParameterValue = New List(Of String)()


        ' Assign variables
        For i As Integer = 0 To parameters.Count() - 1
            If i + 1 < parameters.Count() Then
                If parameters(i + 1).Length > 0 Then
                    If parameters(i) = "-U" Then
                        mUsername = parameters(i + 1)
                    ElseIf parameters(i) = "-P" Then
                        mPassword = parameters(i + 1)
                    ElseIf parameters(i) = "-F" Then
                        mReportPath = parameters(i + 1)
                    ElseIf parameters(i) = "-O" Then
                        mOutputPath = parameters(i + 1)
                    ElseIf parameters(i) = "-S" Then
                        mServerName = parameters(i + 1)
                    ElseIf parameters(i) = "-D" Then
                        mDatabaseName = parameters(i + 1)
                    ElseIf parameters(i) = "-E" Then
                        mOutputFormat = parameters(i + 1)
                    ElseIf parameters(i) = "-a" Then
                        mReportParameterString.Add(parameters(i + 1))
                    End If
                End If
            End If

            If parameters(i) = "-?" Or parameters(i) = "/?" Then
                mGetHelp = True
            End If
        Next

        For Each input As String In mReportParameterString
            ProcessParameter(input)
        Next

        ' Handle default output path and output format
        If mReportPath IsNot Nothing And mReportPath.Length > 0 Then
            Dim fileExt As String = ""
            If mOutputPath Is Nothing Or mOutputPath.Trim() = "" Then

                If mOutputFormat Is Nothing Then
                    mOutputFormat = defaultFileFormat
                End If

                If mOutputFormat = "xlsdata" Then
                    fileExt = "xls"
                ElseIf mOutputFormat = "tab" Then
                    fileExt = "txt"
                ElseIf mOutputFormat = "ertf" Then
                    fileExt = "rtf"
                Else
                    fileExt = mOutputFormat
                End If

                If mReportPath.LastIndexOf(".rpt") = -1 Then
                    Throw New Exception("Invalid Crystal Reports file")
                End If

                mOutputPath = String.Format("{0}-{1}.{2}", mReportPath.Substring(0, mReportPath.LastIndexOf(".rpt")), DateTime.Now.ToString("yyyyMMddHHmmss"))
            Else
                If mOutputFormat Is Nothing Then
                    Dim lastIndexDot As Int32 = mOutputPath.LastIndexOf(".")
                    fileExt = mOutputPath.Substring(lastIndexDot + 1, 3)

                    If mOutputPath.Length = lastIndexDot + 4 And (fileExt = "rtf" Or fileExt = "txt" Or fileExt = "csv" Or fileExt = "pdf" Or fileExt = "rpt" Or fileExt = "doc" Or fileExt = "xls" Or fileExt = "xml" Or fileExt = "htm") Then
                        mOutputFormat = mOutputPath.Substring(lastIndexDot + 1, 3)
                    Else
                        mOutputFormat = defaultFileFormat
                    End If
                End If
            End If
        Else
            If mGetHelp = False Then
                Throw New Exception("Invalid Crystal Reports file")
            End If
        End If
    End Sub

    Private Sub ProcessParameter(ByVal paraString As String)
        mReportParameterName.Add(paraString.Substring(0, paraString.IndexOf(":")).Trim())
        mReportParameterValue.Add((paraString.Substring(paraString.IndexOf(":") + 1, paraString.Length - (paraString.IndexOf(":") + 1))).Trim())
    End Sub

#Region "Public Properties"

    Public ReadOnly Property GetHelp() As Boolean
        Get
            Return mGetHelp
        End Get
    End Property


    Public ReadOnly Property Username() As String
        Get
            Return mUsername
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get
            Return mPassword
        End Get
    End Property

    Public ReadOnly Property ReportPath() As String
        Get
            Return mReportPath
        End Get
    End Property

    Public ReadOnly Property OutputPath() As String
        Get
            Return mOutputPath
        End Get
    End Property

    Public ReadOnly Property ServerName() As String
        Get
            Return mServerName
        End Get
    End Property

    Public ReadOnly Property DatabaseName() As String
        Get
            Return mDatabaseName
        End Get
    End Property

    Public ReadOnly Property OutputFormat() As String
        Get
            Return mOutputFormat
        End Get
    End Property

    Public ReadOnly Property ReportParameterName() As List(Of String)
        Get
            Return mReportParameterName
        End Get
    End Property

    Public ReadOnly Property ReportParameterValue() As List(Of String)
        Get
            Return mReportParameterValue
        End Get
    End Property

#End Region

End Class
