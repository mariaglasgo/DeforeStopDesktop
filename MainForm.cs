using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop
{
    public partial class MainForm : Form
    {
        private Usuario _usuarioActual;

        public MainForm(Usuario usuario)
        {
            _usuarioActual = usuario;
            InitializeComponent();
            ConstruirUI();
        }

        private void ConstruirUI()
        {
            this.Text = "DeforeStop - Panel Principal";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Colores.FondoClaro;

            Panel navbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Colores.VerdeOscuro
            };
            navbar.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(navbar.ClientRectangle,
                    Colores.VerdeOscuro, Colores.VerdeNeon, 0f))
                    e.Graphics.FillRectangle(brush, navbar.ClientRectangle);
            };

            PictureBox picLogo = new PictureBox
            {
                Image = Image.FromFile("pajaro.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(60, 60),
                Location = new Point(20, 10),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(picLogo);

            Label lblTitulo = new Label
            {
                Text = "DeforeStop",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(90, 20),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(lblTitulo);

            Label lblUsuario = new Label
            {
                Text = $"👤 {_usuarioActual.Nombre} ({_usuarioActual.Rol})",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(this.Width - 400, 25),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            navbar.Controls.Add(lblUsuario);

            Button btnCerrarSesion = new Button
            {
                Text = "Cerrar Sesión",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(130, 35),
                Location = new Point(this.Width - 180, 22),
                BackColor = Colores.Rojo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += (s, e) => this.Close();
            navbar.Controls.Add(btnCerrarSesion);

            this.Controls.Add(navbar);

            Panel panelCentral = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Colores.FondoClaro,
                Padding = new Padding(30)
            };

            Label lblBienvenida = new Label
            {
                Text = $"¡Bienvenido, {_usuarioActual.Nombre}!",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 30)
            };
            panelCentral.Controls.Add(lblBienvenida);

            Label lblSub = new Label
            {
                Text = "Sistema de Monitoreo Satelital de Deforestación - Nicaragua",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 80)
            };
            panelCentral.Controls.Add(lblSub);

            int x = 30, y = 150, cardWidth = 250, cardHeight = 180, espacio = 25;

            CrearTarjeta(panelCentral, "👥 Usuarios", "Gestionar usuarios del sistema", x, y, cardWidth, cardHeight, () => {
                DeforeStopDesktop.Forms.UsuariosForm form = new DeforeStopDesktop.Forms.UsuariosForm();
                form.ShowDialog();
            });
            x += cardWidth + espacio;
            CrearTarjeta(panelCentral, "🌍 Zonas", "Reservas y áreas forestales", x, y, cardWidth, cardHeight, () => {
                DeforeStopDesktop.Forms.ZonasForm form = new DeforeStopDesktop.Forms.ZonasForm();
                form.ShowDialog();
            });
            x += cardWidth + espacio;
            CrearTarjeta(panelCentral, "📊 Reportes", "Generar y consultar reportes", x, y, cardWidth, cardHeight, () => {
                DeforeStopDesktop.Forms.ReportesForm form = new DeforeStopDesktop.Forms.ReportesForm();
                form.ShowDialog();
            });
            x += cardWidth + espacio;
            CrearTarjeta(panelCentral, "🛰️ Imágenes", "Imágenes satelitales registradas", x, y, cardWidth, cardHeight, () => {
                DeforeStopDesktop.Forms.ImagenesForm form = new DeforeStopDesktop.Forms.ImagenesForm();
                form.ShowDialog();
            });

            this.Controls.Add(panelCentral);
            panelCentral.BringToFront();

            Panel footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Colores.VerdeOscuro
            };
            Label lblFooter = new Label
            {
                Text = "🌳 DeforeStop © 2026 - Protegiendo nuestros bosques con tecnología",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            footer.Controls.Add(lblFooter);
            this.Controls.Add(footer);
        }

        private void CrearTarjeta(Panel parent, string titulo, string descripcion,
            int x, int y, int ancho, int alto, Action onClick)
        {
            Panel card = new Panel
            {
                Size = new Size(ancho, alto),
                Location = new Point(x, y),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Colores.VerdeNeon, 3),
                    new Rectangle(0, 0, card.Width - 1, card.Height - 1));
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = false,
                Size = new Size(ancho, 40),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblTitulo);

            Label lblDesc = new Label
            {
                Text = descripcion,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Colores.TextoOscuro,
                AutoSize = false,
                Size = new Size(ancho - 20, 60),
                Location = new Point(10, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblDesc);

            card.Click += (s, e) => onClick();
            lblTitulo.Click += (s, e) => onClick();
            lblDesc.Click += (s, e) => onClick();
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(240, 255, 240);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            parent.Controls.Add(card);
        }
    }
}