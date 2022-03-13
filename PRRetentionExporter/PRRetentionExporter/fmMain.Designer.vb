<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmMain))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btn_fromENList = New System.Windows.Forms.Button
        Me.txt_toPeriod = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btn_importfile = New System.Windows.Forms.Button
        Me.txt_fromPeriod = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txt_PrjCode = New System.Windows.Forms.TextBox
        Me.btn_Find = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.btn_Next = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.btn_addPrj = New System.Windows.Forms.Button
        Me.btn_deletePrj = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.ofDlg = New System.Windows.Forms.OpenFileDialog
        Me.cbx_Result = New System.Windows.Forms.CheckedListBox
        Me.cbx_Export = New System.Windows.Forms.CheckedListBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btn_fromENList)
        Me.GroupBox1.Controls.Add(Me.txt_toPeriod)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btn_importfile)
        Me.GroupBox1.Controls.Add(Me.txt_fromPeriod)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txt_PrjCode)
        Me.GroupBox1.Controls.Add(Me.btn_Find)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(466, 112)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Criteria"
        '
        'btn_fromENList
        '
        Me.btn_fromENList.Location = New System.Drawing.Point(301, 71)
        Me.btn_fromENList.Name = "btn_fromENList"
        Me.btn_fromENList.Size = New System.Drawing.Size(159, 34)
        Me.btn_fromENList.TabIndex = 10
        Me.btn_fromENList.Text = "Retrieve from email notification project list"
        Me.btn_fromENList.UseVisualStyleBackColor = True
        '
        'txt_toPeriod
        '
        Me.txt_toPeriod.Location = New System.Drawing.Point(194, 49)
        Me.txt_toPeriod.Name = "txt_toPeriod"
        Me.txt_toPeriod.Size = New System.Drawing.Size(63, 22)
        Me.txt_toPeriod.TabIndex = 7
        Me.txt_toPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(168, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 12)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "To"
        '
        'btn_importfile
        '
        Me.btn_importfile.Location = New System.Drawing.Point(301, 42)
        Me.btn_importfile.Name = "btn_importfile"
        Me.btn_importfile.Size = New System.Drawing.Size(159, 23)
        Me.btn_importfile.TabIndex = 9
        Me.btn_importfile.Text = "Import from text file"
        Me.btn_importfile.UseVisualStyleBackColor = True
        '
        'txt_fromPeriod
        '
        Me.txt_fromPeriod.Location = New System.Drawing.Point(96, 49)
        Me.txt_fromPeriod.Name = "txt_fromPeriod"
        Me.txt_fromPeriod.Size = New System.Drawing.Size(65, 22)
        Me.txt_fromPeriod.TabIndex = 6
        Me.txt_fromPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 12)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Report Period"
        '
        'txt_PrjCode
        '
        Me.txt_PrjCode.Location = New System.Drawing.Point(96, 21)
        Me.txt_PrjCode.Name = "txt_PrjCode"
        Me.txt_PrjCode.Size = New System.Drawing.Size(161, 22)
        Me.txt_PrjCode.TabIndex = 5
        '
        'btn_Find
        '
        Me.btn_Find.Location = New System.Drawing.Point(301, 13)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(159, 23)
        Me.btn_Find.TabIndex = 8
        Me.btn_Find.Text = "Retrieve"
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 12)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Project Code"
        '
        'btn_Next
        '
        Me.btn_Next.Location = New System.Drawing.Point(405, 426)
        Me.btn_Next.Name = "btn_Next"
        Me.btn_Next.Size = New System.Drawing.Size(75, 30)
        Me.btn_Next.TabIndex = 13
        Me.btn_Next.Text = "Next"
        Me.btn_Next.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 153)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(116, 19)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Search Result"
        '
        'btn_addPrj
        '
        Me.btn_addPrj.Location = New System.Drawing.Point(230, 203)
        Me.btn_addPrj.Name = "btn_addPrj"
        Me.btn_addPrj.Size = New System.Drawing.Size(32, 23)
        Me.btn_addPrj.TabIndex = 11
        Me.btn_addPrj.Text = "+"
        Me.btn_addPrj.UseVisualStyleBackColor = True
        '
        'btn_deletePrj
        '
        Me.btn_deletePrj.Location = New System.Drawing.Point(229, 361)
        Me.btn_deletePrj.Name = "btn_deletePrj"
        Me.btn_deletePrj.Size = New System.Drawing.Size(33, 23)
        Me.btn_deletePrj.TabIndex = 12
        Me.btn_deletePrj.Text = "-"
        Me.btn_deletePrj.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(263, 153)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(177, 19)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Project to be exported"
        '
        'ofDlg
        '
        Me.ofDlg.FileName = "OpenFileDialog1"
        '
        'cbx_Result
        '
        Me.cbx_Result.CheckOnClick = True
        Me.cbx_Result.FormattingEnabled = True
        Me.cbx_Result.Location = New System.Drawing.Point(12, 180)
        Me.cbx_Result.Name = "cbx_Result"
        Me.cbx_Result.Size = New System.Drawing.Size(211, 208)
        Me.cbx_Result.Sorted = True
        Me.cbx_Result.TabIndex = 14
        '
        'cbx_Export
        '
        Me.cbx_Export.CheckOnClick = True
        Me.cbx_Export.FormattingEnabled = True
        Me.cbx_Export.Location = New System.Drawing.Point(268, 180)
        Me.cbx_Export.Name = "cbx_Export"
        Me.cbx_Export.Size = New System.Drawing.Size(210, 208)
        Me.cbx_Export.Sorted = True
        Me.cbx_Export.TabIndex = 15
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(12, 399)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(121, 16)
        Me.CheckBox1.TabIndex = 16
        Me.CheckBox1.Text = "Uncheck / Check All"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(268, 399)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(121, 16)
        Me.CheckBox2.TabIndex = 17
        Me.CheckBox2.Text = "Uncheck / Check All"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'fmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 461)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.cbx_Export)
        Me.Controls.Add(Me.cbx_Result)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btn_deletePrj)
        Me.Controls.Add(Me.btn_addPrj)
        Me.Controls.Add(Me.btn_Next)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(508, 499)
        Me.MinimumSize = New System.Drawing.Size(508, 499)
        Me.Name = "fmMain"
        Me.Text = "PCMS | Retention Exporter "
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents btn_Next As System.Windows.Forms.Button
    Friend WithEvents txt_PrjCode As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btn_addPrj As System.Windows.Forms.Button
    Friend WithEvents btn_deletePrj As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_importfile As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_toPeriod As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_fromPeriod As System.Windows.Forms.TextBox
    Friend WithEvents btn_fromENList As System.Windows.Forms.Button
    Friend WithEvents ofDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cbx_Result As System.Windows.Forms.CheckedListBox
    Friend WithEvents cbx_Export As System.Windows.Forms.CheckedListBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox

End Class
