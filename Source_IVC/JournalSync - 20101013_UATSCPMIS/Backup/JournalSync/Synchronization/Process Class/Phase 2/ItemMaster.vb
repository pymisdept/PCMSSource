Imports SAPbobsCOM

Namespace SyncMainClass
    Class ItemMaster
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "Item Master Synchronization (Fiex & SAP)"

        Public Sub New(ByVal pCompany As Company)
            MyBase.New(pCompany)
        End Sub

        Public Overrides Sub Export()

        End Sub

        Public Overrides Sub Import()

        End Sub

        Public Overrides Property ObjType() As Integer
            Get

            End Get
            Set(ByVal value As Integer)

            End Set
        End Property
    End Class
End Namespace