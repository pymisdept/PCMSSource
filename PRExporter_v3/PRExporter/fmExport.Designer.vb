<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmExport))
        Me.lbl_hdr = New System.Windows.Forms.Label
        Me.lbx_Export = New System.Windows.Forms.ListBox
        Me.btn_Export = New System.Windows.Forms.Button
        Me.btn_back = New System.Windows.Forms.Button
        Me.txt_toPeriod = New System.Windows.Forms.TextBox
        Me.lbl_to = New System.Windows.Forms.Label
        Me.txt_fromPeriod = New System.Windows.Forms.TextBox
        Me.lbl_from = New System.Windows.Forms.Label
        Me.progressbar = New System.Windows.Forms.ProgressBar
        Me.bgWorker = New System.ComponentModel.BackgroundWorker
        Me.lbl_processing = New System.Windows.Forms.Label
        Me.folderDlg = New System.Windows.Forms.FolderBrowserDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.txt_outputPath = New System.Windows.Forms.TextBox
        Me.btn_browse = New System.Windows.Forms.Button
        Me.cb_PDF = New System.Windows.Forms.CheckBox
        Me.cb_Excel = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'lbl_hdr
        '
        Me.lbl_hdr.AutoSize = True
        Me.lbl_hdr.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_hdr.Location = New System.Drawing.Point(56, 8)
        Me.lbl_hdr.Name = "lbl_hdr"
        Me.lbl_hdr.Size = New System.Drawing.Size(177, 19)
        Me.lbl_hdr.TabIndex = 8
        Me.lbl_hdr.Text = "Project to be exported"
        '
        'lbx_Export
        '
        Me.lbx_Export.FormattingEnabled = True
        Me.lbx_Export.Location = New System.Drawing.Point(19, 138)
        Me.lbx_Export.Name = "lbx_Export"
        Me.lbx_Export.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.lbx_Export.Size = New System.Drawing.Size(249, 186)
        Me.lbx_Export.TabIndex = 7
        Me.lbx_Export.TabStop = False
        '
        'btn_Export
        '
        Me.btn_Export.Location = New System.Drawing.Point(29, 381)
        Me.btn_Export.Name = "btn_Export"
        Me.btn_Export.Size = New System.Drawing.Size(100, 39)
        Me.btn_Export.TabIndex = 3
        Me.btn_Export.Text = "Export"
        Me.btn_Export.UseVisualStyleBackColor = True
        '
        'btn_back
        '
        Me.btn_back.Location = New System.Drawing.Point(152, 380)
        Me.btn_back.Name = "btn_back"
        Me.btn_back.Size = New System.Drawing.Size(100, 40)
        Me.btn_back.TabIndex = 4
        Me.btn_back.Text = "Back"
        Me.btn_back.UseVisualStyleBackColor = True
        '
        'txt_toPeriod
        '
        Me.txt_toPeriod.Location = New System.Drawing.Point(195, 76)
        Me.txt_toPeriod.Name = "txt_toPeriod"
        Me.txt_toPeriod.Size = New System.Drawing.Size(70, 20)
        Me.txt_toPeriod.TabIndex = 2
        Me.txt_toPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_to
        '
        Me.lbl_to.AutoSize = True
        Me.lbl_to.Location = New System.Drawing.Point(168, 79)
        Me.lbl_to.Name = "lbl_to"
        Me.lbl_to.Size = New System.Drawing.Size(20, 13)
        Me.lbl_to.TabIndex = 12
        Me.lbl_to.Text = "To"
        '
        'txt_fromPeriod
        '
        Me.txt_fromPeriod.Location = New System.Drawing.Point(93, 76)
        Me.txt_fromPeriod.Name = "txt_fromPeriod"
        Me.txt_fromPeriod.Size = New System.Drawing.Size(70, 20)
        Me.txt_fromPeriod.TabIndex = 1
        Me.txt_fromPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_from
        '
        Me.lbl_from.AutoSize = True
        Me.lbl_from.Location = New System.Drawing.Point(16, 79)
        Me.lbl_from.Name = "lbl_from"
        Me.lbl_from.Size = New System.Drawing.Size(40, 13)
        Me.lbl_from.TabIndex = 14
        Me.lbl_from.Text = "Period:"
        '
        'progressbar
        '
        Me.progressbar.Location = New System.Drawing.Point(19, 340)
        Me.progressbar.Name = "progressbar"
        Me.progressbar.Size = New System.Drawing.Size(249, 25)
        Me.progressbar.TabIndex = 15
        '
        'bgWorker
        '
        Me.bgWorker.WorkerReportsProgress = True
        Me.bgWorker.WorkerSupportsCancellation = True
        '
        'lbl_processing
        '
        Me.lbl_processing.AutoSize = True
        Me.lbl_processing.Location = New System.Drawing.Point(111, 326)
        Me.lbl_processing.Name = "lbl_processing"
        Me.lbl_processing.Size = New System.Drawing.Size(68, 13)
        Me.lbl_processing.TabIndex = 16
        Me.lbl_processing.Text = "Processing..."
        Me.lbl_processing.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Output Path"
        '
        'txt_outputPath
        '
        Me.txt_outputPath.Location = New System.Drawing.Point(93, 46)
        Me.txt_outputPath.Name = "txt_outputPath"
        Me.txt_outputPath.Size = New System.Drawing.Size(145, 20)
        Me.txt_outputPath.TabIndex = 18
        '
        'btn_browse
        '
        Me.btn_browse.BackgroundImage = CType(resources.GetObject("btn_browse.BackgroundImage"), System.Drawing.Image)
        Me.btn_browse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn_browse.Location = New System.Drawing.Point(241, 44)
        Me.btn_browse.Name = "btn_browse"
        Me.btn_browse.Size = New System.Drawing.Size(24, 26)
        Me.btn_browse.TabIndex = 19
        Me.btn_browse.UseVisualStyleBackColor = True
        '
        'cb_PDF
        '
        Me.cb_PDF.AutoSize = True
        Me.cb_PDF.Checked = True
        Me.cb_PDF.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cb_PDF.Location = New System.Drawing.Point(19, 107)
        Me.cb_PDF.Name = "cb_PDF"
        Me.cb_PDF.Size = New System.Drawing.Size(118, 17)
        Me.cb_PDF.TabIndex = 20
        Me.cb_PDF.Text = "Project Report PDF"
        Me.cb_PDF.UseVisualStyleBackColor = True
        '
        'cb_Excel
        '
        Me.cb_Excel.AutoSize = True
        Me.cb_Excel.Location = New System.Drawing.Point(149, 107)
        Me.cb_Excel.Name = "cb_Excel"
        Me.cb_Excel.Size = New System.Drawing.Size(123, 17)
        Me.cb_Excel.TabIndex = 21
        Me.cb_Excel.Text = "Project Report Excel"
        Me.cb_Excel.UseVisualStyleBackColor = True
        '
        'fmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(285, 452)
        Me.Controls.Add(Me.cb_Excel)
        Me.Controls.Add(Me.cb_PDF)
        Me.Controls.Add(Me.btn_browse)
        Me.Controls.Add(Me.txt_outputPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl_processing)
        Me.Controls.Add(Me.progressbar)
        Me.Controls.Add(Me.lbl_from)
        Me.Controls.Add(Me.txt_toPeriod)
        Me.Controls.Add(Me.lbl_to)
        Me.Controls.Add(Me.txt_fromPeriod)
        Me.Controls.Add(Me.btn_back)
        Me.Controls.Add(Me.btn_Export)
        Me.Controls.Add(Me.lbl_hdr)
        Me.Controls.Add(Me.lbx_Export)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(293, 479)
        Me.MinimumSize = New System.Drawing.Size(293, 479)
        Me.Name = "fmExport"
        Me.Text = "PCMS | Project Report Exporter"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_hdr As System.Windows.Forms.Label
    Friend WithEvents lbx_Export As System.Windows.Forms.ListBox
    Friend WithEvents btn_Export As System.Windows.Forms.Button
    Friend WithEvents btn_back As System.Windows.Forms.Button
    Friend WithEvents txt_toPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lbl_to As System.Windows.Forms.Label
    Friend WithEvents txt_fromPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lbl_from As System.Windows.Forms.Label
    Friend WithEvents progressbar As System.Windows.Forms.ProgressBar
    Friend WithEvents bgWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents lbl_processing As System.Windows.Forms.Label
    Friend WithEvents folderDlg As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_outputPath As System.Windows.Forms.TextBox
    Friend WithEvents btn_browse As System.Windows.Forms.Button
    Friend WithEvents cb_PDF As System.Windows.Forms.CheckBox
    Friend WithEvents cb_Excel As System.Windows.Forms.CheckBox
End Class
