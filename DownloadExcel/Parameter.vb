Namespace Excel.Setting

    Public Class Parameter

        Private _Parameter As sParameter
       
        ''' <summary>
        ''' </summary>
        ''' <param name="pFileType"></param>
        ''' <param name="pParameter"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pFileType As String, ByVal pParameter As String)
            If pParameter = "" Or pParameter Is Nothing Then
                Throw New BaseException(BaseException.ErrorType.Normal, "PARA_NEW_00001", "Parameter unable to null in downlaod module")
            End If

            pParameter = pParameter.Trim

            Select Case pFileType

                'Purchasing Module
                Case ProcessMap.pm_PUI01  ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_PUI02  ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.inputType = pParameter.Split(",")(2)
                    _Parameter.mrNo = pParameter.Split(",")(3)

                Case ProcessMap.pm_PUI03  ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    '//remove multiple input way 
                    '_Parameter.inputType = pParameter.Split(",")(2)
                    '_Parameter.mrNo = pParameter.Split(",")(3)

                Case ProcessMap.pm_PUI04  ''Return Format ( PrjCode,FunctionID,PANum )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.PANum = pParameter.Split(",")(2)

                    'Karrson: Modify on 2009-08-23
                    'Add New Parameter(Vendor Code)
                Case ProcessMap.pm_PUI05  ''Return Format ( PrjCode,FunctionID,CertType )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.inputType = pParameter.Split(",")(2)
                    ' Vendor Code
                    _Parameter.Vendor = pParameter.Split(",")(3)

                    'Karrson: Create on 2009-08-24
                    'New Template 
                Case ProcessMap.pm_PUI06  ''Return Format ( PrjCode,FunctionID,CertType )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.PANum = pParameter.Split(",")(2)

                    'Karrson: Create on 2009-08-24
                    'New Template 
                Case ProcessMap.pm_PUI08  ''Return Format ( PrjCode,FunctionID,CertType )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                    'Commercial Module
                Case ProcessMap.pm_QSI01 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI02 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI03 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI04 ''Return Format ( PrjCode,FunctionID,SubContractor )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    '//Remove SubContractor parmater//
                    '_Parameter.subContractor = pParameter.Split(",")(2)

                Case ProcessMap.pm_QSI07 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI08 ''Return Format ( PrjCode,FunctionID,SubContractor )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI12 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.subContractor = pParameter.Split(",")(2)
                    _Parameter.subContract = pParameter.Split(",")(3)

                Case ProcessMap.pm_QSI17 ''Return Format ( PrjCode,FunctionID,SubContractor,SubContract )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)


                Case ProcessMap.pm_QSI18 ''Return Format ( PrjCode,FunctionID,SubContractor,SubContract )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)


                Case ProcessMap.pm_QSI20 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)


                Case ProcessMap.pm_QSI21 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI22 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI23 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI24 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI25 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI26 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI27 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI28 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI29 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)


                Case ProcessMap.pm_QSI30 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)


                Case ProcessMap.pm_QSI31 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI32 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI33 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI40 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.isBlankTemplate = (pParameter.Split(",")(2) = "M")
                    _Parameter.CutOffDate = pParameter.Split(",")(3)

                Case ProcessMap.pm_QSI41 ''Return Format ( PrjCode,FunctionID,SubContractor )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.subContractor = pParameter.Split(",")(2)
                    _Parameter.subContract = pParameter.Split(",")(3)

                Case ProcessMap.pm_QSI42 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_QSI43 ''Return Format ( PrjCode,FunctionID,CustCode )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.custCode = pParameter.Split(",")(2)

                Case ProcessMap.pm_QSI44 ''Return Format ( PrjCode,FunctionID,CustCode )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.custCode = pParameter.Split(",")(2)

                    'Accounts Moudle
                Case ProcessMap.pm_ACI01 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)

                Case ProcessMap.pm_ACI02 ''Return Format ( PrjCode,FunctionID )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.sectionCode = pParameter.Split(",")(2)


                    'Management Module
                Case ProcessMap.pm_MAI02  ''Return Format ( PrjCode,FunctionID,CertType )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.sectionCode = pParameter.Split(",")(2)

                    'Security Module
                Case ProcessMap.pm_SEI01  ''Return Format ( PrjCode,FunctionID,CertType )
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.projCode = pParameter.Split(",")(0)
                    _Parameter.functionID = pParameter.Split(",")(1)


                Case ProcessMap.pm_MA08
                    _Parameter = New sParameter
                    _Parameter.fileType = pFileType
                    _Parameter.functionID = pParameter.Split(",")(1)
                    _Parameter.sectionCode = pParameter.Split(",")(2)
                    _Parameter.CutOffDate = pParameter.Split(",")(4)
            End Select
        End Sub

        Public Function Parameter() As sParameter
            Return _Parameter
        End Function

        Structure sParameter
            Dim fileType As String
            Dim projCode As String
            Dim functionID As String
            Dim subContractor As String
            Dim mrNo As String
            Dim subContract As String
            Dim PANum As String
            Dim CertType As String
            'Karrson: Modify on 2009-08-23
            ' New Parameter(Vendor Code)
            Dim Vendor As String

            Dim subsiCode As String 'Subsidiary Code
            Dim inputType As String
            Dim sectionCode As String

            Dim CutOffDate As String
            Dim custCode As String
            Dim isBlankTemplate As Boolean
        End Structure
    End Class

End Namespace