'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.8800
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports B1WizardBase
Imports SAPbobsCOM
Imports SAPbouiCOM

Namespace PaymentCertApproval
    
    Public Class PaymentCertApproval
        Inherits B1AddOn
        
        Public Sub New()
            MyBase.New
            'ADD YOUR INITIALIZATION CODE HERE	...
        End Sub
        
        Public Overrides Sub OnShutDown()
            'ADD YOUR TERMINATION CODE HERE	...
        End Sub
        
        Public Overrides Sub OnCompanyChanged()
            'ADD YOUR COMPANY CHANGE CODE HERE	...
        End Sub
        
        Public Overrides Sub OnLanguageChanged(ByVal language As BoLanguages)
            InitializeMenus
            'ADD YOUR LANGUAGE CHANGE CODE HERE	...
        End Sub
        
        Public Overrides Sub OnStatusBarErrorMessage(ByVal txt As String)
            'ADD YOUR CODE HERE	...
        End Sub
        
        Public Overrides Sub OnStatusBarSuccessMessage(ByVal txt As String)
            'ADD YOUR CODE HERE	...
        End Sub
        
        Public Overrides Sub OnStatusBarWarningMessage(ByVal txt As String)
            'ADD YOUR CODE HERE	...
        End Sub
        
        Public Overrides Sub OnStatusBarNoTypedMessage(ByVal txt As String)
            'ADD YOUR CODE HERE	...
        End Sub
        
        Public Overrides Function OnBeforeProgressBarCreated() As Boolean
            'ADD YOUR CODE HERE	...
            Return true
        End Function
        
        Public Overrides Function OnAfterProgressBarCreated() As Boolean
            'ADD YOUR CODE HERE	...
            Return true
        End Function
        
        Public Overrides Function OnBeforeProgressBarStopped(ByVal success As Boolean) As Boolean
            'ADD YOUR CODE HERE	...
            Return true
        End Function
        
        Public Overrides Function OnAfterProgressBarStopped(ByVal success As Boolean) As Boolean
            'ADD YOUR CODE HERE	...
            Return true
        End Function
        
        Public Overrides Function OnProgressBarReleased() As Boolean
            'ADD YOUR CODE HERE	...
            Return true
        End Function
        
        Public Shared Sub Main()
            Dim retCode As Integer = 0
            Dim connStr As String = ""
            Dim cnxType As B1WizardBase.B1Connections.ConnectionType = B1WizardBase.B1Connections.ConnectionType.SSO
            'CHANGE ADDON IDENTIFIER BEFORE RELEASING TO CUSTOMER (Solution Identifier)
            Dim addOnIdentifierStr As String = Nothing
            If (System.Environment.GetCommandLineArgs().Length = 1) Then
                connStr = B1Connections.connStr
            Else
                connStr = System.Environment.GetCommandLineArgs().GetValue(1).ToString()
            End If
            Try 
                'INIT CONNECTIONS
                retCode = B1Connections.Init(connStr, addOnIdentifierStr, cnxType)
                'CONNECTION FAILED
                If (retCode <> 0) Then
                    System.Windows.Forms.MessageBox.Show("ERROR - Connection failed: "+ B1Connections.diCompany.GetLastErrorDescription())
                    return
                End If
                'CREATE DB
                'MANAGE COCKPITS
                If ((cnxType = B1WizardBase.B1Connections.ConnectionType.SSO)  _
                            OrElse (cnxType = B1WizardBase.B1Connections.ConnectionType.MultipleAddOns)) Then
                    Dim addOnDb As PaymentCertApproval_Db = New PaymentCertApproval_Db
                    addOnDb.Add(B1Connections.diCompany)
                    Dim addOnCockpit As PaymentCertApproval_Cockpits = New PaymentCertApproval_Cockpits
                    addOnCockpit.Manage(B1Connections.theAppl, B1Connections.diCompany)
                End If
                'CREATE ADD-ON
                Dim addOn As PaymentCertApproval = New PaymentCertApproval
                SBOApplication = B1Connections.theAppl
                SBOCompany = B1Connections.diCompany
                System.Windows.Forms.Application.Run()

            Catch com_err As System.Runtime.InteropServices.COMException
                'HANDLE ANY COMException HERE
                System.Windows.Forms.MessageBox.Show("ERROR - Connection failed: " + com_err.Message)
            End Try
        End Sub
    End Class
End Namespace
