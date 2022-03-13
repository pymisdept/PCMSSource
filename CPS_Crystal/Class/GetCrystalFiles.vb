Imports System.IO

Public Class GetCrystalFiles
    Private mPath As String
    Private mFileName() As String
    Private mFileFull() As String
    Private mCounter As Integer


    Public Sub New(ByVal pPath As String)
        mPath = pPath
    End Sub

    Public ReadOnly Property RetrieveFileName() As String()
        Get
            Return mFileName
        End Get
    End Property

    Public ReadOnly Property RetrieveFileFull() As String()
        Get
            Return mFileFull
        End Get
    End Property

    Public ReadOnly Property RetrieveCounter() As Integer
        Get
            Return mCounter
        End Get
    End Property

    Public ReadOnly Property RetrieveCrystalList() As DataTable
        Get
            Dim mdt As DataTable
            mCounter = 0
            Dim mGetFile_Class As New IO.DirectoryInfo(mPath)
            Dim mGetFiles As IO.FileInfo() = mGetFile_Class.GetFiles("*.rpt")
            Dim mGetFile As IO.FileInfo

            mdt = New DataTable
            mdt.Columns.Add(New DataColumn("DataValueField", GetType(String)))
            mdt.Columns.Add(New DataColumn("DataTextField", GetType(String)))

            Dim mdr As DataRow
            For Each mGetFile In mGetFiles


                ReDim Preserve mFileName(mCounter), mFileFull(mCounter)
                mFileName(mCounter) = mGetFile.Name
                mFileFull(mCounter) = mGetFile.FullName

                mdr = mdt.NewRow()
                mdr("DataValueField") = mGetFile.FullName
                mdr("DataTextField") = mGetFile.Name
                mdt.Rows.Add(mdr)
                mCounter += 1
            Next
            Return mdt
        End Get
    End Property
End Class
