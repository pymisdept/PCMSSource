<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmXLS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmXLS))
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.cb_projects = New System.Windows.Forms.CheckBox
        Me.cbx_Export = New System.Windows.Forms.CheckedListBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.cb_result = New System.Windows.Forms.CheckBox
        Me.cbx_Result = New System.Windows.Forms.CheckedListBox
        Me.btn_import = New System.Windows.Forms.Button
        Me.btn_retrieve = New System.Windows.Forms.Button
        Me.btn_deletePrj = New System.Windows.Forms.Button
        Me.btn_addPrj = New System.Windows.Forms.Button
        Me.txt_PrjCode = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.cb_PR4 = New System.Windows.Forms.CheckBox
        Me.cbx_PR4 = New System.Windows.Forms.CheckedListBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cb_TD2 = New System.Windows.Forms.CheckBox
        Me.cbx_TD2 = New System.Windows.Forms.CheckedListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cb_PR2 = New System.Windows.Forms.CheckBox
        Me.cbx_PR2 = New System.Windows.Forms.CheckedListBox
        Me.lbl_from = New System.Windows.Forms.Label
        Me.txt_toPeriod = New System.Windows.Forms.TextBox
        Me.lbl_to = New System.Windows.Forms.Label
        Me.txt_fromPeriod = New System.Windows.Forms.TextBox
        Me.lbl_hdr = New System.Windows.Forms.Label
        Me.lbl_processing = New System.Windows.Forms.Label
        Me.progressbar = New System.Windows.Forms.ProgressBar
        Me.btn_Export = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.ofDlg = New System.Windows.Forms.OpenFileDialog
        Me.bgWorker = New System.ComponentModel.BackgroundWorker
        Me.lbl_testingSvr = New System.Windows.Forms.Label
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.cb_projects)
        Me.GroupBox5.Controls.Add(Me.cbx_Export)
        Me.GroupBox5.Location = New System.Drawing.Point(271, 99)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(214, 177)
        Me.GroupBox5.TabIndex = 60
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Projects to be exported"
        '
        'cb_projects
        '
        Me.cb_projects.AutoSize = True
        Me.cb_projects.Location = New System.Drawing.Point(10, 156)
        Me.cb_projects.Name = "cb_projects"
        Me.cb_projects.Size = New System.Drawing.Size(126, 17)
        Me.cb_projects.TabIndex = 39
        Me.cb_projects.Text = "Uncheck / Check All"
        Me.cb_projects.UseVisualStyleBackColor = True
        '
        'cbx_Export
        '
        Me.cbx_Export.CheckOnClick = True
        Me.cbx_Export.FormattingEnabled = True
        Me.cbx_Export.Location = New System.Drawing.Point(9, 20)
        Me.cbx_Export.Name = "cbx_Export"
        Me.cbx_Export.Size = New System.Drawing.Size(198, 109)
        Me.cbx_Export.Sorted = True
        Me.cbx_Export.TabIndex = 37
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.cb_result)
        Me.GroupBox4.Controls.Add(Me.cbx_Result)
        Me.GroupBox4.Location = New System.Drawing.Point(13, 99)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(214, 177)
        Me.GroupBox4.TabIndex = 59
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Search Result"
        '
        'cb_result
        '
        Me.cb_result.AutoSize = True
        Me.cb_result.Location = New System.Drawing.Point(10, 156)
        Me.cb_result.Name = "cb_result"
        Me.cb_result.Size = New System.Drawing.Size(126, 17)
        Me.cb_result.TabIndex = 38
        Me.cb_result.Text = "Uncheck / Check All"
        Me.cb_result.UseVisualStyleBackColor = True
        '
        'cbx_Result
        '
        Me.cbx_Result.CheckOnClick = True
        Me.cbx_Result.FormattingEnabled = True
        Me.cbx_Result.Location = New System.Drawing.Point(9, 20)
        Me.cbx_Result.Name = "cbx_Result"
        Me.cbx_Result.Size = New System.Drawing.Size(198, 109)
        Me.cbx_Result.Sorted = True
        Me.cbx_Result.TabIndex = 36
        '
        'btn_import
        '
        Me.btn_import.Location = New System.Drawing.Point(360, 36)
        Me.btn_import.Name = "btn_import"
        Me.btn_import.Size = New System.Drawing.Size(125, 25)
        Me.btn_import.TabIndex = 58
        Me.btn_import.Text = "Import from Text file"
        Me.btn_import.UseVisualStyleBackColor = True
        '
        'btn_retrieve
        '
        Me.btn_retrieve.Location = New System.Drawing.Point(280, 37)
        Me.btn_retrieve.Name = "btn_retrieve"
        Me.btn_retrieve.Size = New System.Drawing.Size(75, 25)
        Me.btn_retrieve.TabIndex = 57
        Me.btn_retrieve.Text = "Retrieve"
        Me.btn_retrieve.UseVisualStyleBackColor = True
        '
        'btn_deletePrj
        '
        Me.btn_deletePrj.Location = New System.Drawing.Point(233, 200)
        Me.btn_deletePrj.Name = "btn_deletePrj"
        Me.btn_deletePrj.Size = New System.Drawing.Size(32, 25)
        Me.btn_deletePrj.TabIndex = 56
        Me.btn_deletePrj.Text = "<"
        Me.btn_deletePrj.UseVisualStyleBackColor = True
        '
        'btn_addPrj
        '
        Me.btn_addPrj.Location = New System.Drawing.Point(233, 132)
        Me.btn_addPrj.Name = "btn_addPrj"
        Me.btn_addPrj.Size = New System.Drawing.Size(32, 25)
        Me.btn_addPrj.TabIndex = 55
        Me.btn_addPrj.Text = ">"
        Me.btn_addPrj.UseVisualStyleBackColor = True
        '
        'txt_PrjCode
        '
        Me.txt_PrjCode.Location = New System.Drawing.Point(99, 67)
        Me.txt_PrjCode.Name = "txt_PrjCode"
        Me.txt_PrjCode.Size = New System.Drawing.Size(166, 20)
        Me.txt_PrjCode.TabIndex = 54
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 53
        Me.Label4.Text = "Project Code:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 297)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 19)
        Me.Label3.TabIndex = 52
        Me.Label3.Text = "Export Fields"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DimGray
        Me.Label1.Location = New System.Drawing.Point(9, 262)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(760, 22)
        Me.Label1.TabIndex = 51
        Me.Label1.Text = "                                                                                 " & _
            "                                                                     "
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cb_PR4)
        Me.GroupBox3.Controls.Add(Me.cbx_PR4)
        Me.GroupBox3.Location = New System.Drawing.Point(479, 329)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(283, 276)
        Me.GroupBox3.TabIndex = 50
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "PR4"
        '
        'cb_PR4
        '
        Me.cb_PR4.AutoSize = True
        Me.cb_PR4.Location = New System.Drawing.Point(6, 251)
        Me.cb_PR4.Name = "cb_PR4"
        Me.cb_PR4.Size = New System.Drawing.Size(126, 17)
        Me.cb_PR4.TabIndex = 32
        Me.cb_PR4.Text = "Uncheck / Check All"
        Me.cb_PR4.UseVisualStyleBackColor = True
        '
        'cbx_PR4
        '
        Me.cbx_PR4.CheckOnClick = True
        Me.cbx_PR4.FormattingEnabled = True
        Me.cbx_PR4.Location = New System.Drawing.Point(6, 20)
        Me.cbx_PR4.Name = "cbx_PR4"
        Me.cbx_PR4.Size = New System.Drawing.Size(268, 199)
        Me.cbx_PR4.TabIndex = 23
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cb_TD2)
        Me.GroupBox2.Controls.Add(Me.cbx_TD2)
        Me.GroupBox2.Location = New System.Drawing.Point(238, 329)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(235, 276)
        Me.GroupBox2.TabIndex = 49
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "TD2"
        '
        'cb_TD2
        '
        Me.cb_TD2.AutoSize = True
        Me.cb_TD2.Location = New System.Drawing.Point(6, 251)
        Me.cb_TD2.Name = "cb_TD2"
        Me.cb_TD2.Size = New System.Drawing.Size(126, 17)
        Me.cb_TD2.TabIndex = 31
        Me.cb_TD2.Text = "Uncheck / Check All"
        Me.cb_TD2.UseVisualStyleBackColor = True
        '
        'cbx_TD2
        '
        Me.cbx_TD2.CheckOnClick = True
        Me.cbx_TD2.FormattingEnabled = True
        Me.cbx_TD2.Location = New System.Drawing.Point(6, 20)
        Me.cbx_TD2.Name = "cbx_TD2"
        Me.cbx_TD2.Size = New System.Drawing.Size(220, 199)
        Me.cbx_TD2.TabIndex = 22
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cb_PR2)
        Me.GroupBox1.Controls.Add(Me.cbx_PR2)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 329)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 276)
        Me.GroupBox1.TabIndex = 48
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "PR2"
        '
        'cb_PR2
        '
        Me.cb_PR2.AutoSize = True
        Me.cb_PR2.Location = New System.Drawing.Point(6, 251)
        Me.cb_PR2.Name = "cb_PR2"
        Me.cb_PR2.Size = New System.Drawing.Size(126, 17)
        Me.cb_PR2.TabIndex = 30
        Me.cb_PR2.Text = "Uncheck / Check All"
        Me.cb_PR2.UseVisualStyleBackColor = True
        '
        'cbx_PR2
        '
        Me.cbx_PR2.CheckOnClick = True
        Me.cbx_PR2.FormattingEnabled = True
        Me.cbx_PR2.Location = New System.Drawing.Point(6, 20)
        Me.cbx_PR2.Name = "cbx_PR2"
        Me.cbx_PR2.Size = New System.Drawing.Size(211, 199)
        Me.cbx_PR2.TabIndex = 21
        '
        'lbl_from
        '
        Me.lbl_from.AutoSize = True
        Me.lbl_from.Location = New System.Drawing.Point(12, 44)
        Me.lbl_from.Name = "lbl_from"
        Me.lbl_from.Size = New System.Drawing.Size(75, 13)
        Me.lbl_from.TabIndex = 47
        Me.lbl_from.Text = "Report Period:"
        '
        'txt_toPeriod
        '
        Me.txt_toPeriod.Location = New System.Drawing.Point(194, 37)
        Me.txt_toPeriod.Name = "txt_toPeriod"
        Me.txt_toPeriod.Size = New System.Drawing.Size(71, 20)
        Me.txt_toPeriod.TabIndex = 44
        Me.txt_toPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_to
        '
        Me.lbl_to.AutoSize = True
        Me.lbl_to.Location = New System.Drawing.Point(169, 43)
        Me.lbl_to.Name = "lbl_to"
        Me.lbl_to.Size = New System.Drawing.Size(20, 13)
        Me.lbl_to.TabIndex = 46
        Me.lbl_to.Text = "To"
        '
        'txt_fromPeriod
        '
        Me.txt_fromPeriod.Location = New System.Drawing.Point(99, 38)
        Me.txt_fromPeriod.Name = "txt_fromPeriod"
        Me.txt_fromPeriod.Size = New System.Drawing.Size(65, 20)
        Me.txt_fromPeriod.TabIndex = 43
        Me.txt_fromPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_hdr
        '
        Me.lbl_hdr.AutoSize = True
        Me.lbl_hdr.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_hdr.Location = New System.Drawing.Point(9, 5)
        Me.lbl_hdr.Name = "lbl_hdr"
        Me.lbl_hdr.Size = New System.Drawing.Size(197, 19)
        Me.lbl_hdr.TabIndex = 45
        Me.lbl_hdr.Text = "Export Projects && Period"
        '
        'lbl_processing
        '
        Me.lbl_processing.AutoSize = True
        Me.lbl_processing.Location = New System.Drawing.Point(323, 628)
        Me.lbl_processing.Name = "lbl_processing"
        Me.lbl_processing.Size = New System.Drawing.Size(68, 13)
        Me.lbl_processing.TabIndex = 63
        Me.lbl_processing.Text = "Processing..."
        Me.lbl_processing.Visible = False
        '
        'progressbar
        '
        Me.progressbar.Location = New System.Drawing.Point(238, 645)
        Me.progressbar.Name = "progressbar"
        Me.progressbar.Size = New System.Drawing.Size(235, 25)
        Me.progressbar.TabIndex = 62
        '
        'btn_Export
        '
        Me.btn_Export.Location = New System.Drawing.Point(238, 676)
        Me.btn_Export.Name = "btn_Export"
        Me.btn_Export.Size = New System.Drawing.Size(235, 39)
        Me.btn_Export.TabIndex = 61
        Me.btn_Export.Text = "Export to Excel"
        Me.btn_Export.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.DimGray
        Me.Label2.Location = New System.Drawing.Point(10, 594)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(760, 22)
        Me.Label2.TabIndex = 64
        Me.Label2.Text = "                                                                                 " & _
            "                                                                     "
        '
        'ofDlg
        '
        Me.ofDlg.FileName = "OpenFileDialog1"
        '
        'bgWorker
        '
        Me.bgWorker.WorkerReportsProgress = True
        Me.bgWorker.WorkerSupportsCancellation = True
        '
        'lbl_testingSvr
        '
        Me.lbl_testingSvr.AutoSize = True
        Me.lbl_testingSvr.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_testingSvr.ForeColor = System.Drawing.Color.Gray
        Me.lbl_testingSvr.Location = New System.Drawing.Point(626, 2)
        Me.lbl_testingSvr.Name = "lbl_testingSvr"
        Me.lbl_testingSvr.Size = New System.Drawing.Size(150, 17)
        Me.lbl_testingSvr.TabIndex = 65
        Me.lbl_testingSvr.Text = "(Testing Environment)"
        Me.lbl_testingSvr.Visible = False
        '
        'fmXLS
        '
        Me.AcceptButton = Me.btn_retrieve
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(782, 739)
        Me.Controls.Add(Me.lbl_testingSvr)
        Me.Controls.Add(Me.lbl_processing)
        Me.Controls.Add(Me.progressbar)
        Me.Controls.Add(Me.btn_Export)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.btn_import)
        Me.Controls.Add(Me.btn_retrieve)
        Me.Controls.Add(Me.btn_deletePrj)
        Me.Controls.Add(Me.btn_addPrj)
        Me.Controls.Add(Me.txt_PrjCode)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lbl_from)
        Me.Controls.Add(Me.txt_toPeriod)
        Me.Controls.Add(Me.lbl_to)
        Me.Controls.Add(Me.txt_fromPeriod)
        Me.Controls.Add(Me.lbl_hdr)
        Me.Controls.Add(Me.Label2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(790, 766)
        Me.MinimumSize = New System.Drawing.Size(790, 766)
        Me.Name = "fmXLS"
        Me.Text = "PCMS | Retention Exporter"
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents cb_projects As System.Windows.Forms.CheckBox
    Friend WithEvents cbx_Export As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents cb_result As System.Windows.Forms.CheckBox
    Friend WithEvents cbx_Result As System.Windows.Forms.CheckedListBox
    Friend WithEvents btn_import As System.Windows.Forms.Button
    Friend WithEvents btn_retrieve As System.Windows.Forms.Button
    Friend WithEvents btn_deletePrj As System.Windows.Forms.Button
    Friend WithEvents btn_addPrj As System.Windows.Forms.Button
    Friend WithEvents txt_PrjCode As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cb_PR4 As System.Windows.Forms.CheckBox
    Friend WithEvents cbx_PR4 As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cb_TD2 As System.Windows.Forms.CheckBox
    Friend WithEvents cbx_TD2 As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cb_PR2 As System.Windows.Forms.CheckBox
    Friend WithEvents cbx_PR2 As System.Windows.Forms.CheckedListBox
    Friend WithEvents lbl_from As System.Windows.Forms.Label
    Friend WithEvents txt_toPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lbl_to As System.Windows.Forms.Label
    Friend WithEvents txt_fromPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lbl_hdr As System.Windows.Forms.Label
    Friend WithEvents lbl_processing As System.Windows.Forms.Label
    Friend WithEvents progressbar As System.Windows.Forms.ProgressBar
    Friend WithEvents btn_Export As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ofDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents bgWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents lbl_testingSvr As System.Windows.Forms.Label
End Class
