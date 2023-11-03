namespace Recovery.UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            RecoverBtn = new Button();
            XrayListBox = new ListBox();
            PreviewBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)PreviewBox).BeginInit();
            SuspendLayout();
            // 
            // RecoverBtn
            // 
            RecoverBtn.Font = new Font("Segoe UI", 25F, FontStyle.Regular, GraphicsUnit.Point);
            RecoverBtn.ImageAlign = ContentAlignment.TopCenter;
            RecoverBtn.Location = new Point(12, 377);
            RecoverBtn.Name = "RecoverBtn";
            RecoverBtn.RightToLeft = RightToLeft.Yes;
            RecoverBtn.Size = new Size(725, 60);
            RecoverBtn.TabIndex = 0;
            RecoverBtn.Text = "Recover";
            RecoverBtn.TextAlign = ContentAlignment.TopCenter;
            RecoverBtn.UseVisualStyleBackColor = true;
            RecoverBtn.Click += RecoverBtn_Click;
            // 
            // XrayListBox
            // 
            XrayListBox.FormattingEnabled = true;
            XrayListBox.ItemHeight = 15;
            XrayListBox.Location = new Point(12, 12);
            XrayListBox.Name = "XrayListBox";
            XrayListBox.Size = new Size(350, 349);
            XrayListBox.TabIndex = 1;
            XrayListBox.SelectedIndexChanged += XrayListBox_SelectedIndexChanged;
            // 
            // PreviewBox
            // 
            PreviewBox.InitialImage = Properties.Resources.CS7600;
            PreviewBox.Location = new Point(387, 12);
            PreviewBox.Name = "PreviewBox";
            PreviewBox.Size = new Size(350, 350);
            PreviewBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PreviewBox.TabIndex = 2;
            PreviewBox.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(749, 452);
            Controls.Add(PreviewBox);
            Controls.Add(XrayListBox);
            Controls.Add(RecoverBtn);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Carestream Xray Recovery Tool";
            ((System.ComponentModel.ISupportInitialize)PreviewBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button RecoverBtn;
        private ListBox XrayListBox;
        private PictureBox PreviewBox;
    }
}