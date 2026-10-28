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
            this.MinimumSize = new Size(1000, 600);

            // ============ NAVBAR ============
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

            // Logo
            PictureBox picLogo = new PictureBox
            {
                Image = Image.FromFile("pajaro.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(60, 60),
                Location = new Point(20, 10),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(picLogo);

            // Título
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

            // Bienvenida usuario
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

            // Botón cerrar sesión
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
            btnCerrarSesion.Click += (s, e) =>
            {
                if (MessageBox.Show("¿Cerrar sesión?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Close();
                }
            };
            navbar.Controls.Add(btnCerrarSesion);

            this.Controls.Add(navbar);

            // ============ PANEL CENTRAL ============
            Panel panelCentral = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Colores.FondoClaro,
                Padding = new Padding(30)
            };

            // Bienvenida
            Label lblBienvenida = new Label
            {
                Text = $"¡Bienvenido, {_usuarioActual.Nombre}!",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20),
                BackColor = Color.Transparent
            };
            panelCentral.Controls.Add(lblBienvenida);

            Label lblSub = new Label
            {
                Text = "Sistema de Monitoreo Satelital de Deforestación - Nicaragua",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 65),
                BackColor = Color.Transparent
            };
            panelCentral.Controls.Add(lblSub);

            // ============ CONFIGURACIÓN DE TARJETAS ============
            int cardWidth = 250;
            int cardHeight = 150;
            int espacio = 25;

            // Calcular posición inicial para centrar (3 columnas)
            int anchoFila = (cardWidth * 3) + (espacio * 2);
            int xInicial = (this.ClientSize.Width - anchoFila) / 2;
            if (xInicial < 30) xInicial = 30;

            // ============ PRIMERA FILA (3 tarjetas) ============
            int y1 = 140;

            // 1. Usuarios
            CrearTarjeta(panelCentral, "👥 Usuarios", "Gestionar usuarios del sistema",
                xInicial, y1, cardWidth, cardHeight, () => {
                    new DeforeStopDesktop.Forms.UsuariosForm().ShowDialog();
                });

            // 2. Zonas
            CrearTarjeta(panelCentral, "🌍 Zonas", "Reservas y áreas forestales",
                xInicial + cardWidth + espacio, y1, cardWidth, cardHeight, () => {
                    new DeforeStopDesktop.Forms.ZonasForm().ShowDialog();
                });

            // 3. Reportes
            CrearTarjeta(panelCentral, "📊 Reportes", "Generar y consultar reportes",
                xInicial + (cardWidth + espacio) * 2, y1, cardWidth, cardHeight, () => {
                    new DeforeStopDesktop.Forms.ReportesForm().ShowDialog();
                });

            // ============ SEGUNDA FILA (3 tarjetas) ============
            int y2 = y1 + cardHeight + espacio;

            // 4. Imágenes
            CrearTarjeta(panelCentral, "🛰️ Imágenes", "Imágenes satelitales registradas",
                xInicial, y2, cardWidth, cardHeight, () => {
                    new DeforeStopDesktop.Forms.ImagenesForm().ShowDialog();
                });

            // 5. Alertas
            CrearTarjeta(panelCentral, "🚨 Alertas", "Alertas de deforestación",
                xInicial + cardWidth + espacio, y2, cardWidth, cardHeight, () => {
                    new DeforeStopDesktop.Forms.AlertasForm().ShowDialog();
                });

            // 6. Denuncias (Zonas Deforestadas)
            CrearTarjeta(panelCentral, "📢 Denuncias", "Registro de hectáreas afectadas",
                xInicial + (cardWidth + espacio) * 2, y2, cardWidth, cardHeight, () => {
                    new DeforeStopDesktop.Forms.ZonasDeforestadasForm().ShowDialog();
                });

            this.Controls.Add(panelCentral);
            panelCentral.BringToFront();

            // ============ FOOTER ============
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

            // Borde verde
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Colores.VerdeNeon, 3))
                {
                    e.Graphics.DrawRectangle(pen,
                        new Rectangle(0, 0, card.Width - 1, card.Height - 1));
                }
            };

            // Ícono + Título
            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = false,
                Size = new Size(ancho, 40),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblTitulo);

            // Descripción
            Label lblDesc = new Label
            {
                Text = descripcion,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Colores.TextoOscuro,
                AutoSize = false,
                Size = new Size(ancho - 20, 60),
                Location = new Point(10, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblDesc);

            // Eventos de clic
            card.Click += (s, e) => onClick();
            lblTitulo.Click += (s, e) => onClick();
            lblDesc.Click += (s, e) => onClick();

            // Efecto hover
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(240, 255, 240);
                card.Paint += PaintHover;
                card.Invalidate();
            };
            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.White;
                card.Paint -= PaintHover;
                card.Invalidate();
            };

            parent.Controls.Add(card);
            card.BringToFront();
        }

        private void PaintHover(object? sender, PaintEventArgs e)
        {
            if (sender is Panel card)
            {
                using (var pen = new Pen(Colores.VerdeClaro, 4))
                {
                    e.Graphics.DrawRectangle(pen,
                        new Rectangle(0, 0, card.Width - 1, card.Height - 1));
                }
            }
        }
    }
}