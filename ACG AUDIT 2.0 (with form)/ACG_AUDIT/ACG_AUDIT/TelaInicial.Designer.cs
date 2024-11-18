
using System.Windows.Forms;

namespace ACG_AUDIT
{
    partial class TelaInicial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TelaInicial));
            TituloSoftwareLabel = new Label();
            ExecucaoProcessoLabel = new Label();
            TituloLabel = new Label();
            SuspendLayout();
            // 
            // TituloSoftwareLabel
            // 
            TituloSoftwareLabel.Location = new Point(0, 0);
            TituloSoftwareLabel.Name = "TituloSoftwareLabel";
            TituloSoftwareLabel.Size = new Size(100, 23);
            TituloSoftwareLabel.TabIndex = 3;
            TituloSoftwareLabel.Click += label1_Click;
            // 
            // ExecucaoProcessoLabel
            // 
            ExecucaoProcessoLabel.Location = new Point(0, 0);
            ExecucaoProcessoLabel.Name = "ExecucaoProcessoLabel";
            ExecucaoProcessoLabel.Size = new Size(100, 23);
            ExecucaoProcessoLabel.TabIndex = 2;
            // 
            // TituloLabel
            // 
            TituloLabel.Location = new Point(0, 0);
            TituloLabel.Name = "TituloLabel";
            TituloLabel.Size = new Size(100, 23);
            TituloLabel.TabIndex = 0;
            // 
            // TelaInicial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MintCream;
            ClientSize = new Size(800, 400);
            Controls.Add(TituloLabel);
            Controls.Add(ExecucaoProcessoLabel);
            Controls.Add(TituloSoftwareLabel);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TelaInicial";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "ACG AUDIT";
            ResumeLayout(false);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Label TituloSoftwareLabel;
        private Label ExecucaoProcessoLabel;
        private Label TituloLabel;
    }
}
