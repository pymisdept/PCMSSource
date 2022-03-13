Imports Microsoft.VisualBasic
Imports B1WizardBase
Imports SAPbobsCOM
Imports SAPbouiCOM
Module PublicModule

    Public SBOApplication As SAPbouiCOM.Application
    Public SBOCompany As SAPbobsCOM.Company
    Public SysDataTable As System.Data.DataTable
    Public blnRunTechPur As Boolean
    Public ReadOnly SpecInedx() As String = {"%", "~", "`", "!", "@", "#", "$", "^", ";", _
                                             "&", "(", ")", "-", "_", "+", "=", "{", "}", _
                                             "[", "]", "<", ">", "?", "/", "|", "\"}

    Public Function CreateForm(ByVal pXMLFile As String, Optional ByRef pIsNew As Boolean = True) As SAPbouiCOM.Form
        Dim oForm As SAPbouiCOM.Form

        Dim oForms As SAPbouiCOM.Forms = SBOApplication.Forms
        Dim oFormParams As SAPbouiCOM.FormCreationParams = SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)

        Try
            Dim oXmlDoc As New Xml.XmlDocument
            oXmlDoc.Load(pXMLFile)

            oFormParams.XmlData = oXmlDoc.InnerXml
            oForm = oForms.AddEx(oFormParams)

            pIsNew = True

            oForm.Visible = True
            Return oForm

            ' Angus:        Handle Authoization error 
        Catch comEx As Runtime.InteropServices.COMException

            If comEx.ErrorCode = -7010 Then
                oForm = oForms.Item(oFormParams.UniqueID)

                oForm.Visible = True
                oForm.Select()
            End If

            pIsNew = False

        Catch ex As Exception
            oForm = oForms.Item(oFormParams.UniqueID)

            pIsNew = False

            oForm.Visible = True
            oForm.Select()
            Return oForm
        End Try
    End Function
End Module
