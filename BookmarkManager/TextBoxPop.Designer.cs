namespace BookmarkManager
{
    partial class TextBoxPop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rtb_Text = new System.Windows.Forms.RichTextBox();
            this.cms_RightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Hide = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_RightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb_Text
            // 
            this.rtb_Text.ContextMenuStrip = this.cms_RightClick;
            this.rtb_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Text.Location = new System.Drawing.Point(0, 0);
            this.rtb_Text.Name = "rtb_Text";
            this.rtb_Text.Size = new System.Drawing.Size(501, 498);
            this.rtb_Text.TabIndex = 0;
            this.rtb_Text.Text = "";
            // 
            // cms_RightClick
            // 
            this.cms_RightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Clear,
            this.tsmi_Hide});
            this.cms_RightClick.Name = "cms_RightClick";
            this.cms_RightClick.Size = new System.Drawing.Size(102, 48);
            // 
            // tsmi_Hide
            // 
            this.tsmi_Hide.Name = "tsmi_Hide";
            this.tsmi_Hide.Size = new System.Drawing.Size(152, 22);
            this.tsmi_Hide.Text = "Hide";
            this.tsmi_Hide.Click += new System.EventHandler(this.tsmi_Hide_Click);
            // 
            // tsmi_Clear
            // 
            this.tsmi_Clear.Name = "tsmi_Clear";
            this.tsmi_Clear.Size = new System.Drawing.Size(152, 22);
            this.tsmi_Clear.Text = "Clear";
            this.tsmi_Clear.Click += new System.EventHandler(this.tsmi_Clear_Click);
            // 
            // TextBoxPop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 498);
            this.Controls.Add(this.rtb_Text);
            this.Name = "TextBoxPop";
            this.Text = "TextBoxPop";
            this.cms_RightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_Text;
        private System.Windows.Forms.ContextMenuStrip cms_RightClick;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Clear;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Hide;
    }
}