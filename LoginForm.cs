using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ConstruirUI();
        }

        private void ConstruirUI()
        {
            this.Text = "DeforeStop - Iniciar Sesión";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Colores.FondoClaro;

            Panel panelIzq = new Panel
            {
                Dock = DockStyle.Left,
                Width = 400,
                BackColor = Colores.VerdeOscuro
            };
            panelIzq.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(panelIzq.ClientRectangle,
                    Colores.VerdeOscuro, Colores.VerdeNeon, 135f))
                {
                    e.Graphics.FillRectangle(brush, panelIzq.ClientRectangle);
                }
            };

            PictureBox picLogo = new PictureBox
            {
                Image = Image.FromFile("pajaro.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(180, 180),
                Location = new Point(110, 100),
                BackColor = Color.Transparent
            };
            panelIzq.Controls.Add(picLogo);

            Label lblTitulo = new Label
            {
                Text = "DeforeStop",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(400, 50),
                Location = new Point(0, 300),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelIzq.Controls.Add(lblTitulo);

            Label lblSub = new Label
            {
                Text = "Monitoreo Satelital de\nDeforestación en Nicaragua",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 255, 200),
                AutoSize = false,
                Size = new Size(400, 60),
                Location = new Point(0, 360),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelIzq.Controls.Add(lblSub);

            Panel panelDer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Colores.FondoClaro
            };

            Label lblLogin = new Label
            {
                Text = "Iniciar Sesión",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(80, 100)
            };
            panelDer.Controls.Add(lblLogin);

            Label lblCorreo = new Label
            {
                Text = "Correo Electrónico",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(80, 170)
            };
            panelDer.Controls.Add(lblCorreo);

            TextBox txtCorreo = new TextBox
            {
                Font = new Font("Segoe UI", 12),
                Size = new Size(350, 35),
                Location = new Point(80, 195),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            panelDer.Controls.Add(txtCorreo);

            Label lblPass = new Label
            {
                Text = "Contraseña",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(80, 250)
            };
            panelDer.Controls.Add(lblPass);

            TextBox txtPass = new TextBox
            {
                Font = new Font("Segoe UI", 12),
                Size = new Size(350, 35),
                Location = new Point(80, 275),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true,
                BackColor = Color.White
            };
            panelDer.Controls.Add(txtPass);

            Button btnLogin = new Button
            {
                Text = "🔐  INICIAR SESIÓN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(350, 50),
                Location = new Point(80, 340),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Colores.VerdeOscuro;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Colores.VerdeNeon;
            btnLogin.Click += (s, e) => IniciarSesion(txtCorreo.Text, txtPass.Text);
            panelDer.Controls.Add(btnLogin);

            Button btnSalir = new Button
            {
                Text = "🚪  SALIR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(350, 45),
                Location = new Point(80, 405),
                BackColor = Color.Transparent,
                ForeColor = Colores.Rojo,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalir.FlatAppearance.BorderSize = 2;
            btnSalir.FlatAppearance.BorderColor = Colores.Rojo;
            btnSalir.MouseEnter += (s, e) => { btnSalir.BackColor = Colores.Rojo; btnSalir.ForeColor = Color.White; };
            btnSalir.MouseLeave += (s, e) => { btnSalir.BackColor = Color.Transparent; btnSalir.ForeColor = Colores.Rojo; };
            btnSalir.Click += (s, e) =>
            {
                if (MessageBox.Show("¿Estás seguro que quieres salir?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Application.Exit();
            };
            panelDer.Controls.Add(btnSalir);

            Label lblEstado = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Colores.Rojo,
                AutoSize = false,
                Size = new Size(350, 30),
                Location = new Point(80, 460),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblEstado"
            };
            panelDer.Controls.Add(lblEstado);

            Label lblInfo = new Label
            {
                Text = "Usuario de prueba: admin@deforestop.com / Admin123!",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(80, 500)
            };
            panelDer.Controls.Add(lblInfo);

            Button btnCerrar = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(35, 35),
                Location = new Point(this.Width - 45, 10),
                BackColor = Colores.Rojo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => Application.Exit();
            panelDer.Controls.Add(btnCerrar);

            this.Controls.Add(panelDer);
            this.Controls.Add(panelIzq);
        }

        private void IniciarSesion(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                MostrarError("Por favor, complete todos los campos.");
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);
                    if (usuario != null)
                    {
                        if (!usuario.Activo) { MostrarError("El usuario está desactivado."); return; }
                        MainForm main = new MainForm(usuario);
                        this.Hide();
                        main.ShowDialog();
                        this.Show();
                    }
                    else MostrarError("Correo o contraseña incorrectos.");
                }
            }
            catch (Exception ex) { MostrarError("Error de conexión: " + ex.Message); }
        }

        private void MostrarError(string mensaje)
        {
            var lbl = this.Controls.Find("lblEstado", true).FirstOrDefault() as Label;
            if (lbl != null) { lbl.Text = mensaje; lbl.ForeColor = Colores.Rojo; }
        }
    }
}