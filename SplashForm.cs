using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DeforeStopDesktop.Config;

namespace DeforeStopDesktop
{
    public partial class SplashForm : Form
    {
        private System.Windows.Forms.Timer? _timer;
        private int _opacidad = 0;

        public SplashForm()
        {
            InitializeComponent();
            ConstruirUI();
            IniciarAnimacion();
        }

        private void ConstruirUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 400);
            this.BackColor = Colores.VerdeOscuro;
            this.Opacity = 0;
            this.ShowInTaskbar = false;

            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(this.ClientRectangle,
                    Colores.VerdeOscuro, Colores.VerdeNeon, 135f))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
                using (var pen = new Pen(Color.FromArgb(118, 255, 3), 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                }
            };

            PictureBox picLogo = new PictureBox
            {
                Image = Image.FromFile("logo.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(180, 180),
                Location = new Point(210, 50),
                BackColor = Color.Transparent
            };
            this.Controls.Add(picLogo);

            Label lblTitulo = new Label
            {
                Text = "DeforeStop",
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(600, 60),
                Location = new Point(0, 240),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblTitulo);

            Label lblSub = new Label
            {
                Text = "Monitoreo Satelital de Deforestación - Nicaragua",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 255, 200),
                AutoSize = false,
                Size = new Size(600, 30),
                Location = new Point(0, 310),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblSub);

            Panel barraFondo = new Panel
            {
                Size = new Size(400, 8),
                Location = new Point(100, 360),
                BackColor = Color.FromArgb(50, 255, 255, 255)
            };
            this.Controls.Add(barraFondo);

            Panel barraProgreso = new Panel
            {
                Size = new Size(0, 8),
                Location = new Point(100, 360),
                BackColor = Colores.VerdeClaro,
                Name = "barraProgreso"
            };
            this.Controls.Add(barraProgreso);

            System.Windows.Forms.Timer timerBarra = new System.Windows.Forms.Timer { Interval = 50 };
            timerBarra.Tick += (s, e) =>
            {
                if (barraProgreso.Width < 400) barraProgreso.Width += 8;
                else timerBarra.Stop();
            };
            timerBarra.Start();

            Label lblVersion = new Label
            {
                Text = "v1.0 - 2026",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(200, 255, 200),
                AutoSize = true,
                Location = new Point(530, 375),
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblVersion);
        }

        private void IniciarAnimacion()
        {
            _timer = new System.Windows.Forms.Timer { Interval = 20 };
            _timer.Tick += (s, e) =>
            {
                if (_opacidad < 100)
                {
                    _opacidad += 5;
                    this.Opacity = _opacidad / 100.0;
                }
                else
                {
                    _timer.Stop();
                    System.Windows.Forms.Timer timerCierre = new System.Windows.Forms.Timer { Interval = 5000 };
                    timerCierre.Tick += (s2, e2) =>
                    {
                        timerCierre.Stop();
                        this.Hide();
                        LoginForm login = new LoginForm();
                        login.ShowDialog();
                        this.Close();
                    };
                    timerCierre.Start();
                }
            };
            _timer.Start();
        }
    }
}