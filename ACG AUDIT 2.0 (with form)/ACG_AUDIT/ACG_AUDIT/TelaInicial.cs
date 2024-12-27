using System;
using System.Drawing;
using System.Windows.Forms;

namespace ACG_AUDIT
{
    public partial class TelaInicial : Form
    {
        private ProgressBar progressBar;
        private Label statusLabel;

        public TelaInicial()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(750, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Configurando a barra de progresso
            progressBar = new ProgressBar
            {
                Location = new Point(100, 50),
                Size = new Size(500, 30),
                Style = ProgressBarStyle.Marquee // Estilo de carregamento em movimento
                
            };

            // Criar e estilizar o Label
            statusLabel = new Label
            {
                Text = "Iniciando o processo...",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkGray
            };

            // Adicionar os controles ao Formulário
            this.Controls.Add(progressBar);
            this.Controls.Add(statusLabel);
        }

        public void UpdateStatus(string message)
        {
            statusLabel.Text = message;
        }
    }
}