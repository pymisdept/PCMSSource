Imports System
Imports System.Reflection
Imports System.Text
Imports System.Collections
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

#Region "Exception Classes"

Public Class NullArgumentException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class InvalidOutputException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class InvalidServerException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class NullParamNameException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class NullParamValueException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class NullExportTypeException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

#End Region


Public Class CrExport

    <STAThread()> _
    Public Function doExport(ByVal args As String()) As Boolean
        Dim isOK As Boolean = False

        Try
            Dim rptinfo As New ReportInfo(args)
            If rptinfo.GetHelp = False Then
                Dim Report As New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                Try
                    If args.Length = 0 Then
                        Throw New NullArgumentException("No parameter is specified!")
                    End If

                    If rptinfo.ServerName = Nothing Then
                        Throw New InvalidServerException("Unspecified Server Name")
                    End If

                    If rptinfo.ReportPath = Nothing Or rptinfo.ReportPath = "" Then
                        Throw New Exception("Invalid Crystal Reports file")
                    End If

                    If rptinfo.OutputPath = Nothing Then
                        Throw New InvalidOutputException("Unspecified Output path and filename")
                    End If


                    Report.Load(rptinfo.ReportPath)

                    ' Login to db
                    Dim logonInfo As New TableLogOnInfo()


                    For Each table As Table In Report.Database.Tables
                        logonInfo.ConnectionInfo.ServerName = rptinfo.ServerName
                        logonInfo.ConnectionInfo.DatabaseName = rptinfo.DatabaseName

                        If rptinfo.Username <> Nothing Then
                            logonInfo.ConnectionInfo.UserID = rptinfo.Username
                        End If

                        If rptinfo.Password <> Nothing Then
                            logonInfo.ConnectionInfo.Password = rptinfo.Password
                        End If
                        table.ApplyLogOnInfo(logonInfo)
                    Next

                    ' Export Format
                    If rptinfo.OutputFormat.ToUpper() = "RTF" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.RichText
                    ElseIf rptinfo.OutputFormat.ToUpper() = "TXT" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.Text
                    ElseIf rptinfo.OutputFormat.ToUpper() = "TAB" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.TabSeperatedText
                    ElseIf rptinfo.OutputFormat.ToUpper() = "CSV" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.CharacterSeparatedValues
                    ElseIf rptinfo.OutputFormat.ToUpper() = "PDF" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
                    ElseIf rptinfo.OutputFormat.ToUpper() = "RPT" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.CrystalReport
                    ElseIf rptinfo.OutputFormat.ToUpper() = "DOC" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.WordForWindows
                    ElseIf rptinfo.OutputFormat.ToUpper() = "XLS" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.Excel
                    ElseIf rptinfo.OutputFormat.ToUpper() = "XLSDATA" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.ExcelRecord
                    ElseIf rptinfo.OutputFormat.ToUpper() = "ERTF" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.EditableRTF
                    ElseIf rptinfo.OutputFormat.ToUpper() = "XML" Then
                        Report.ExportOptions.ExportFormatType = ExportFormatType.Xml
                    ElseIf rptinfo.OutputFormat.ToUpper() = "HTM" Then
                        Dim i As Integer = 1
                        While rptinfo.OutputPath.IndexOf("\", (rptinfo.OutputPath.Length) - i, i) < 0
                            i += 1
                        End While

                        Dim lastSlashPos = rptinfo.OutputPath.Length - i + 1
                        Dim baseFolder = rptinfo.OutputPath.Substring(0, lastSlashPos - 1)

                        Dim htmlFormatOptions As New HTMLFormatOptions()
                        htmlFormatOptions.HTMLBaseFolderName = baseFolder
                        htmlFormatOptions.HTMLFileName = rptinfo.OutputPath
                        htmlFormatOptions.HTMLEnableSeparatedPages = False
                        htmlFormatOptions.HTMLHasPageNavigator = True
                        htmlFormatOptions.FirstPageNumber = 1

                        Report.ExportOptions.ExportFormatType = ExportFormatType.HTML40
                        Report.ExportOptions.ExportFormatOptions = htmlFormatOptions
                    End If

                    If Report.ParameterFields.Count > 0 Then
                        Dim paramDefs As ParameterFieldDefinitions = Report.DataDefinition.ParameterFields
                        Dim paramValues As New ParameterValues()
                        Dim singleParamValue As New List(Of String)

                        For i As Integer = 0 To paramDefs.Count() - 1
                            For j As Integer = 0 To rptinfo.ReportParameterName.Count - 1
                                If paramDefs(i).Name = rptinfo.ReportParameterName(j) Then
                                    If paramDefs(i).EnableAllowMultipleValue And rptinfo.ReportParameterValue(j).IndexOf("|") <> -1 Then
                                        singleParamValue = SplitIntoSingleValue(rptinfo.ReportParameterValue(j))

                                        For k As Integer = 0 To singleParamValue.Count
                                            AddParameter(paramValues, paramDefs(i).DiscreteOrRangeKind, singleParamValue(k), paramDefs(i).Name)
                                        Next
                                        singleParamValue.Clear()
                                    Else
                                        AddParameter(paramValues, paramDefs(i).DiscreteOrRangeKind, rptinfo.ReportParameterValue(j), paramDefs(i).Name)
                                    End If

                                    paramDefs(i).ApplyCurrentValues(paramValues)
                                    paramValues.Clear()

                                    Exit For
                                End If
                            Next
                        Next

                    Else
                        Report.Refresh()
                    End If

                    Report.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
                    Dim diskOptions As New DiskFileDestinationOptions()
                    Report.ExportOptions.DestinationOptions = diskOptions
                    diskOptions.DiskFileName = rptinfo.OutputPath

                    Report.Export()

                    isOK = True

                Catch naex As NullArgumentException
                    Console.WriteLine("\nError: " + naex.Message)
                    isOK = False
                Catch ioex As InvalidOutputException
                    Console.WriteLine("\nError: " + ioex.Message)
                    isOK = False
                Catch isex As InvalidServerException
                    Console.WriteLine("\nError: " + isex.Message)
                    isOK = False
                Catch loex As LogOnException
                    Console.WriteLine("\nError: " + loex.Message)
                    isOK = False
                Catch lrex As LoadSaveReportException
                    Console.WriteLine("\nError: " + lrex.Message)
                    isOK = False
                Catch npex As NullParamNameException
                    Console.WriteLine("\nError: " + npex.Message)
                    isOK = False
                Catch ntex As NullExportTypeException
                    Console.WriteLine("\nError: " + ntex.Message)
                    isOK = False
                Catch ex As Exception
                    Console.WriteLine("\nError: " + ex.Message)
                    isOK = False
                Finally
                    Report.Close()
                End Try
            End If

        Catch ex As Exception
            Console.WriteLine("\n System Error: {0}", ex.Message)
            isOK = False
        End Try

        Return isOK

    End Function

    Sub AddParameter(ByRef pValues As ParameterValues, ByVal DoR As DiscreteOrRangeKind, ByVal inputString As String, ByVal pName As String)
        If inputString.Trim().Length > 0 Then
            Dim paraValue As ParameterValue
            If DoR = DiscreteOrRangeKind.DiscreteValue Or (DoR = DiscreteOrRangeKind.DiscreteAndRangeValue And inputString.IndexOf("(") = -1) Then
                paraValue = New ParameterDiscreteValue()
                DirectCast(paraValue, ParameterDiscreteValue).Value = inputString
                Console.WriteLine("Discrete Parameter : {0} = {1}", pName, inputString)
                pValues.Add(paraValue)
                paraValue = Nothing
            ElseIf DoR = DiscreteOrRangeKind.RangeValue Or (DoR = DiscreteOrRangeKind.DiscreteAndRangeValue And inputString.IndexOf("(") <> -1) Then
                paraValue = New ParameterRangeValue()
                DirectCast(paraValue, ParameterRangeValue).StartValue = GetStartValue(inputString)
                DirectCast(paraValue, ParameterRangeValue).EndValue = GetEndValue(inputString)
                Console.WriteLine("Range Parameter : {0} = {1} to {2} ", pName, DirectCast(paraValue, ParameterRangeValue).StartValue, DirectCast(paraValue, ParameterRangeValue).EndValue)
                pValues.Add(paraValue)
                paraValue = Nothing
            End If
        End If

    End Sub

    Function GetStartValue(ByVal parameterString As String) As String
        Dim delimiter As Integer = parameterString.IndexOf(",")
        Dim leftbracket As Integer = parameterString.IndexOf("(")

        If delimiter = -1 Or leftbracket = -1 Then
            Throw New Exception("Invalid Range Parameter value. eg. -a [parameter name] : (FromRange, ToRange) ")
        End If

        Return parameterString.Substring(leftbracket + 1, delimiter - 1).Trim()

    End Function

    Function GetEndValue(ByVal parameterString As String) As String
        Dim delimiter As Integer = parameterString.IndexOf(",")
        Dim rightbracket As Integer = parameterString.IndexOf(")")

        If delimiter = -1 Or rightbracket = -1 Then
            Throw New Exception("Invalid Range Parameter value. eg. -a [parameter name] : (FromRange, ToRange) ")
        End If

        Return parameterString.Substring(delimiter + 1, rightbracket - delimiter - 1).Trim()

    End Function

    Private Function SplitIntoSingleValue(ByVal multipleValueString As String) As List(Of String)
        Dim pipeStartIndex As Integer = 0
        Dim singleValue As New List(Of String)()
        Dim IsLoop As Boolean = True
        While IsLoop = True
            If pipeStartIndex = multipleValueString.LastIndexOf("|") + 1 Then
                IsLoop = False
            End If

            If IsLoop = True Then
                singleValue.Add(multipleValueString.Substring(pipeStartIndex, multipleValueString.IndexOf("|", pipeStartIndex + 1) - pipeStartIndex).Trim())
            Else
                singleValue.Add(multipleValueString.Substring(pipeStartIndex, multipleValueString.Length - pipeStartIndex).Trim())
            End If

            pipeStartIndex = multipleValueString.IndexOf("|", pipeStartIndex) + 1
        End While
        Return singleValue
    End Function


End Class
