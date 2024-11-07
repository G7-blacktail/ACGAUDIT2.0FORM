
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
            statusStrip1 = new StatusStrip();
            ContadorProgressBar = new ToolStripProgressBar();
            TituloLabel = new Label();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // TituloSoftwareLabel
            // 
            resources.ApplyResources(TituloSoftwareLabel, "TituloSoftwareLabel");
            TituloSoftwareLabel.Name = "TituloSoftwareLabel";
            TituloSoftwareLabel.Click += label1_Click;
            // 
            // ExecucaoProcessoLabel
            // 
            resources.ApplyResources(ExecucaoProcessoLabel, "ExecucaoProcessoLabel");
            ExecucaoProcessoLabel.Name = "ExecucaoProcessoLabel";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ContadorProgressBar });
            resources.ApplyResources(statusStrip1, "statusStrip1");
            statusStrip1.Name = "statusStrip1";
            statusStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            // 
            // ContadorProgressBar
            // 
            ContadorProgressBar.Name = "ContadorProgressBar";
            resources.ApplyResources(ContadorProgressBar, "ContadorProgressBar");
            // 
            // TituloLabel
            // 
            resources.ApplyResources(TituloLabel, "TituloLabel");
            TituloLabel.Name = "TituloLabel";
            // 
            // TelaInicial
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MintCream;
            Controls.Add(TituloLabel);
            Controls.Add(statusStrip1);
            Controls.Add(ExecucaoProcessoLabel);
            Controls.Add(TituloSoftwareLabel);
            ForeColor = Color.Black;
            Name = "TelaInicial";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Label TituloSoftwareLabel;
        private Label ExecucaoProcessoLabel;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar ContadorProgressBar;
        private Label TituloLabel;
    }
}
