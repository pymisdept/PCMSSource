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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rb_server_tw = New System.Windows.Forms.RadioButton
        Me.rb_server_msc = New System.Windows.Forms.RadioButton
        Me.rb_server_hk = New System.Windows.Forms.RadioButton
        Me.rb_server_testing = New System.Windows.Forms.RadioButton
        Me.Button1 = New System.Windows.Forms.Button
        Me.btn_switchuser = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
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
        Me.GroupBox1.Location = New System.Drawing.Point(12, 34)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(466, 121)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Criteria"
        '
        'btn_fromENList
        '
        Me.btn_fromENList.Location = New System.Drawing.Point(301, 77)
        Me.btn_fromENList.Name = "btn_fromENList"
        Me.btn_fromENList.Size = New System.Drawing.Size(159, 37)
        Me.btn_fromENList.TabIndex = 10
        Me.btn_fromENList.Text = "Retrieve from email notification project list"
        Me.btn_fromENList.UseVisualStyleBackColor = True
        '
        'txt_toPeriod
        '
        Me.txt_toPeriod.Location = New System.Drawing.Point(194, 53)
        Me.txt_toPeriod.MaxLength = 7
        Me.txt_toPeriod.Name = "txt_toPeriod"
        Me.txt_toPeriod.Size = New System.Drawing.Size(63, 20)
        Me.txt_toPeriod.TabIndex = 7
        Me.txt_toPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(168, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "To"
        '
        'btn_importfile
        '
        Me.btn_importfile.Location = New System.Drawing.Point(301, 46)
        Me.btn_importfile.Name = "btn_importfile"
        Me.btn_importfile.Size = New System.Drawing.Size(159, 25)
        Me.btn_importfile.TabIndex = 9
        Me.btn_importfile.Text = "Import from text file"
        Me.btn_importfile.UseVisualStyleBackColor = True
        '
        'txt_fromPeriod
        '
        Me.txt_fromPeriod.Location = New System.Drawing.Point(96, 53)
        Me.txt_fromPeriod.MaxLength = 7
        Me.txt_fromPeriod.Name = "txt_fromPeriod"
        Me.txt_fromPeriod.Size = New System.Drawing.Size(65, 20)
        Me.txt_fromPeriod.TabIndex = 6
        Me.txt_fromPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Report Period"
        '
        'txt_PrjCode
        '
        Me.txt_PrjCode.Location = New System.Drawing.Point(96, 23)
        Me.txt_PrjCode.Name = "txt_PrjCode"
        Me.txt_PrjCode.Size = New System.Drawing.Size(161, 20)
        Me.txt_PrjCode.TabIndex = 5
        '
        'btn_Find
        '
        Me.btn_Find.Location = New System.Drawing.Point(301, 14)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(159, 25)
        Me.btn_Find.TabIndex = 8
        Me.btn_Find.Text = "Retrieve"
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Project Code"
        '
        'btn_Next
        '
        Me.btn_Next.Location = New System.Drawing.Point(406, 459)
        Me.btn_Next.Name = "btn_Next"
        Me.btn_Next.Size = New System.Drawing.Size(75, 33)
        Me.btn_Next.TabIndex = 13
        Me.btn_Next.Text = "Next"
        Me.btn_Next.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 165)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(116, 19)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Search Result"
        '
        'btn_addPrj
        '
        Me.btn_addPrj.Location = New System.Drawing.Point(230, 219)
        Me.btn_addPrj.Name = "btn_addPrj"
        Me.btn_addPrj.Size = New System.Drawing.Size(32, 25)
        Me.btn_addPrj.TabIndex = 11
        Me.btn_addPrj.Text = "+"
        Me.btn_addPrj.UseVisualStyleBackColor = True
        '
        'btn_deletePrj
        '
        Me.btn_deletePrj.Location = New System.Drawing.Point(229, 390)
        Me.btn_deletePrj.Name = "btn_deletePrj"
        Me.btn_deletePrj.Size = New System.Drawing.Size(33, 25)
        Me.btn_deletePrj.TabIndex = 12
        Me.btn_deletePrj.Text = "-"
        Me.btn_deletePrj.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(263, 165)
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
        Me.cbx_Result.Location = New System.Drawing.Point(12, 194)
        Me.cbx_Result.Name = "cbx_Result"
        Me.cbx_Result.Size = New System.Drawing.Size(211, 214)
        Me.cbx_Result.Sorted = True
        Me.cbx_Result.TabIndex = 14
        '
        'cbx_Export
        '
        Me.cbx_Export.CheckOnClick = True
        Me.cbx_Export.FormattingEnabled = True
        Me.cbx_Export.Location = New System.Drawing.Point(268, 194)
        Me.cbx_Export.Name = "cbx_Export"
        Me.cbx_Export.Size = New System.Drawing.Size(210, 214)
        Me.cbx_Export.Sorted = True
        Me.cbx_Export.TabIndex = 15
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(12, 419)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(126, 17)
        Me.CheckBox1.TabIndex = 16
        Me.CheckBox1.Text = "Uncheck / Check All"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(268, 417)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(126, 17)
        Me.CheckBox2.TabIndex = 17
        Me.CheckBox2.Text = "Uncheck / Check All"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rb_server_tw)
        Me.Panel2.Controls.Add(Me.rb_server_msc)
        Me.Panel2.Controls.Add(Me.rb_server_hk)
        Me.Panel2.Controls.Add(Me.rb_server_testing)
        Me.Panel2.Location = New System.Drawing.Point(274, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(207, 34)
        Me.Panel2.TabIndex = 19
        '
        'rb_server_tw
        '
        Me.rb_server_tw.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb_server_tw.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.rb_server_tw.Location = New System.Drawing.Point(163, 6)
        Me.rb_server_tw.Name = "rb_server_tw"
        Me.rb_server_tw.Size = New System.Drawing.Size(40, 23)
        Me.rb_server_tw.TabIndex = 22
        Me.rb_server_tw.TabStop = True
        Me.rb_server_tw.Text = "TW"
        Me.rb_server_tw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rb_server_tw.UseVisualStyleBackColor = True
        '
        'rb_server_msc
        '
        Me.rb_server_msc.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb_server_msc.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.rb_server_msc.Location = New System.Drawing.Point(121, 6)
        Me.rb_server_msc.Name = "rb_server_msc"
        Me.rb_server_msc.Size = New System.Drawing.Size(40, 23)
        Me.rb_server_msc.TabIndex = 21
        Me.rb_server_msc.TabStop = True
        Me.rb_server_msc.Text = "MSC"
        Me.rb_server_msc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rb_server_msc.UseVisualStyleBackColor = True
        '
        'rb_server_hk
        '
        Me.rb_server_hk.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb_server_hk.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.rb_server_hk.Location = New System.Drawing.Point(78, 6)
        Me.rb_server_hk.Name = "rb_server_hk"
        Me.rb_server_hk.Size = New System.Drawing.Size(40, 23)
        Me.rb_server_hk.TabIndex = 20
        Me.rb_server_hk.TabStop = True
        Me.rb_server_hk.Text = "HK"
        Me.rb_server_hk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rb_server_hk.UseVisualStyleBackColor = True
        '
        'rb_server_testing
        '
        Me.rb_server_testing.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb_server_testing.Location = New System.Drawing.Point(6, 6)
        Me.rb_server_testing.Name = "rb_server_testing"
        Me.rb_server_testing.Size = New System.Drawing.Size(70, 23)
        Me.rb_server_testing.TabIndex = 19
        Me.rb_server_testing.TabStop = True
        Me.rb_server_testing.Text = "Testing HK"
        Me.rb_server_testing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rb_server_testing.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(81, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(43, 23)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "Check Connection"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'btn_switchuser
        '
        Me.btn_switchuser.Location = New System.Drawing.Point(12, 4)
        Me.btn_switchuser.Name = "btn_switchuser"
        Me.btn_switchuser.Size = New System.Drawing.Size(112, 23)
        Me.btn_switchuser.TabIndex = 21
        Me.btn_switchuser.Text = "Switch User"
        Me.btn_switchuser.UseVisualStyleBackColor = True
        '
        'fmMain
        '
        Me.AcceptButton = Me.btn_Find
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 511)
        Me.Controls.Add(Me.btn_switchuser)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel2)
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
        Me.MaximumSize = New System.Drawing.Size(500, 538)
        Me.MinimumSize = New System.Drawing.Size(500, 538)
        Me.Name = "fmMain"
        Me.Text = "PCMS | Project Report Exporter"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
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
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rb_server_hk As System.Windows.Forms.RadioButton
    Friend WithEvents rb_server_testing As System.Windows.Forms.RadioButton
    Friend WithEvents rb_server_tw As System.Windows.Forms.RadioButton
    Friend WithEvents rb_server_msc As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btn_switchuser As System.Windows.Forms.Button

End Class
