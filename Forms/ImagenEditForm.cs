using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ImagenEditForm : Form
    {
        private ImagenSatelital? _imagen;
        private bool _esNuevo;
        private string? _rutaSeleccionada;
        private System.ComponentModel.IContainer? components = null;

        public ImagenEditForm(ImagenSatelital? imagen)
        {
            _imagen = imagen;
            _esNuevo = (imagen == null);
            _rutaSeleccionada = imagen?.RutaArchivo;
            InitializeComponent();
            ConstruirUI();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 650);
            this.Name = "ImagenEditForm";
            this.Text = "Imagen";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ConstruirUI()
        {
            this.Text = _esNuevo ? "Nueva Imagen Satelital" : "Editar Imagen Satelital";
            this.Size = new Size(600, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Colores.FondoClaro;

            Label lblTitulo = new Label
            {
                Text = _esNuevo ? "🛰️ Nueva Imagen" : "✏️ Editar Imagen",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitulo);

            // Nombre Archivo
            Label lblNombre = new Label
            {
                Text = "Nombre del archivo:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 80)
            };
            this.Controls.Add(lblNombre);

            TextBox txtNombre = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(530, 30),
                Location = new Point(30, 105),
                Text = _imagen?.NombreArchivo ?? ""
            };
            this.Controls.Add(txtNombre);

            // Archivo
            Label lblArchivo = new Label
            {
                Text = "Archivo de imagen:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 150)
            };
            this.Controls.Add(lblArchivo);

            TextBox txtRuta = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(400, 30),
                Location = new Point(30, 175),
                ReadOnly = true,
                Text = _rutaSeleccionada ?? ""
            };
            this.Controls.Add(txtRuta);

            Button btnBuscar = new Button
            {
                Text = "📁 Buscar...",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(120, 30),
                Location = new Point(440, 175),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += (s, e) =>
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "Seleccionar imagen satelital",
                    Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tif;*.tiff|Todos|*.*"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _rutaSeleccionada = ofd.FileName;
                    txtRuta.Text = ofd.FileName;
                    if (string.IsNullOrWhiteSpace(txtNombre.Text))
                        txtNombre.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                }
            };
            this.Controls.Add(btnBuscar);

            // Preview
            PictureBox picPreview = new PictureBox
            {
                Size = new Size(530, 200),
                Location = new Point(30, 220),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(picPreview);

            if (!string.IsNullOrEmpty(_rutaSeleccionada) && File.Exists(_rutaSeleccionada))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(_rutaSeleccionada);
                    using (var ms = new MemoryStream(bytes))
                        picPreview.Image = Image.FromStream(ms);
                }
                catch { }
            }

            // Fecha Captura
            Label lblFecha = new Label
            {
                Text = "Fecha de captura:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 435)
            };
            this.Controls.Add(lblFecha);

            DateTimePicker dtpFecha = new DateTimePicker
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(530, 30),
                Location = new Point(30, 460),
                Format = DateTimePickerFormat.Short,
                Value = _imagen?.FechaCaptura ?? DateTime.Now
            };
            this.Controls.Add(dtpFecha);

            // Zona
            Label lblZona = new Label
            {
                Text = "Zona:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 500)
            };
            this.Controls.Add(lblZona);

            ComboBox cmbZona = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(530, 30),
                Location = new Point(30, 525),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbZona);

            // Usuario
            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 565)
            };
            this.Controls.Add(lblUsuario);

            ComboBox cmbUsuario = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(530, 30),
                Location = new Point(30, 590),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbUsuario);

            // Cargar datos
            try
            {
                using (var db = new AppDbContext())
                {
                    var zonas = db.Zonas.ToList();
                    cmbZona.DataSource = zonas;
                    cmbZona.DisplayMember = "Nombre";
                    cmbZona.ValueMember = "Id";

                    var usuarios = db.Usuarios.ToList();
                    cmbUsuario.DataSource = usuarios;
                    cmbUsuario.DisplayMember = "Nombre";
                    cmbUsuario.ValueMember = "Id";

                    if (_imagen != null)
                    {
                        cmbZona.SelectedValue = _imagen.ZonaId;
                        cmbUsuario.SelectedValue = _imagen.UsuarioId;
                    }
                }
            }
            catch { }

            // Botones
            Button btnGuardar = new Button
            {
                Text = "💾 GUARDAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(250, 45),
                Location = new Point(30, 640),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(_rutaSeleccionada))
                {
                    MessageBox.Show("Complete todos los campos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Copiar imagen a carpeta uploads del ejecutable
                    string carpetaUploads = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
                    if (!Directory.Exists(carpetaUploads))
                        Directory.CreateDirectory(carpetaUploads);

                    string rutaDestino = _rutaSeleccionada;

                    // Solo copiar si no está ya en uploads
                    if (!_rutaSeleccionada.StartsWith(carpetaUploads))
                    {
                        string extension = Path.GetExtension(_rutaSeleccionada);
                        string nombreUnico = Guid.NewGuid().ToString() + extension;
                        rutaDestino = Path.Combine(carpetaUploads, nombreUnico);
                        File.Copy(_rutaSeleccionada, rutaDestino, true);
                    }

                    using (var db = new AppDbContext())
                    {
                        if (_esNuevo)
                        {
                            var nueva = new ImagenSatelital
                            {
                                NombreArchivo = txtNombre.Text,
                                RutaArchivo = rutaDestino,
                                FechaCaptura = dtpFecha.Value,
                                ZonaId = cmbZona.SelectedValue != null ? (int)cmbZona.SelectedValue : 1,
                                UsuarioId = cmbUsuario.SelectedValue != null ? (int)cmbUsuario.SelectedValue : 1,
                                FechaSubida = DateTime.Now
                            };
                            db.ImagenesSatelitales.Add(nueva);
                        }
                        else
                        {
                            var im = db.ImagenesSatelitales.Find(_imagen!.Id);
                            if (im != null)
                            {
                                im.NombreArchivo = txtNombre.Text;
                                im.RutaArchivo = rutaDestino;
                                im.FechaCaptura = dtpFecha.Value;
                                im.ZonaId = cmbZona.SelectedValue != null ? (int)cmbZona.SelectedValue : 1;
                                im.UsuarioId = cmbUsuario.SelectedValue != null ? (int)cmbUsuario.SelectedValue : 1;
                            }
                        }
                        db.SaveChanges();
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Controls.Add(btnGuardar);

            Button btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(250, 45),
                Location = new Point(310, 640),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            this.Controls.Add(btnCancelar);
        }
    }
}