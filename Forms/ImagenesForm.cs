using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ImagenesForm : Form
    {
        private DataGridView? dgvImagenes;
        private PictureBox? picPreview;
        private Label? lblSinPreview;
        private System.ComponentModel.IContainer? components = null;
        private static readonly HttpClient httpClient = new HttpClient();

        public ImagenesForm()
        {
            InitializeComponent();
            ConstruirUI();
            CargarImagenes();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Name = "ImagenesForm";
            this.Text = "DeforeStop - Imágenes Satelitales";
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
            this.Text = "DeforeStop - Imágenes Satelitales";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Colores.FondoClaro;

            // ===== NAVBAR =====
            Panel navbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.VerdeOscuro
            };

            Label lblTitulo = new Label
            {
                Text = "🛰️ Imágenes Satelitales",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(lblTitulo);
            this.Controls.Add(navbar);

            // ===== BOTONES =====
            Panel panelBotones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.FondoClaro,
                Padding = new Padding(20)
            };

            Button btnNuevo = new Button
            {
                Text = "➕ Nueva Imagen",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 45),
                Location = new Point(20, 15),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => AbrirEditor(null);
            panelBotones.Controls.Add(btnNuevo);

            Button btnEditar = new Button
            {
                Text = "✏️ Editar",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(220, 15),
                BackColor = Colores.Dorado,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += (s, e) => EditarSeleccionada();
            panelBotones.Controls.Add(btnEditar);

            Button btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(380, 15),
                BackColor = Colores.Rojo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) => EliminarSeleccionada();
            panelBotones.Controls.Add(btnEliminar);

            this.Controls.Add(panelBotones);

            // ===== SPLIT: GRID + PREVIEW =====
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 700,
                BackColor = Colores.FondoClaro
            };

            // DataGridView
            dgvImagenes = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 10),
                ColumnHeadersHeight = 40
            };
            dgvImagenes.RowTemplate.Height = 35;
            dgvImagenes.ColumnHeadersDefaultCellStyle.BackColor = Colores.VerdeOscuro;
            dgvImagenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvImagenes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvImagenes.EnableHeadersVisualStyles = false;
            dgvImagenes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
            dgvImagenes.DefaultCellStyle.SelectionBackColor = Colores.VerdeNeon;
            dgvImagenes.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvImagenes.SelectionChanged += (s, e) => MostrarPreview();

            split.Panel1.Controls.Add(dgvImagenes);

            // Panel de Preview
            Panel panelPreview = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblPreviewTitulo = new Label
            {
                Text = "🖼️ Vista Previa",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelPreview.Controls.Add(lblPreviewTitulo);

            picPreview = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 249, 245),
                Cursor = Cursors.Hand
            };
            picPreview.Click += (s, e) => MostrarImagenGrande();
            panelPreview.Controls.Add(picPreview);
            picPreview.BringToFront();

            // Tooltip
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(picPreview, "🖱️ Clic para ampliar la imagen");

            lblSinPreview = new Label
            {
                Text = "Selecciona una imagen para ver la vista previa",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            panelPreview.Controls.Add(lblSinPreview);
            lblSinPreview.BringToFront();

            split.Panel2.Controls.Add(panelPreview);

            this.Controls.Add(split);
            split.BringToFront();
        }

        private void CargarImagenes()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var imagenes = db.ImagenesSatelitales.OrderBy(i => i.Id).ToList();
                    dgvImagenes!.DataSource = imagenes;
                    dgvImagenes.Columns["Id"]!.HeaderText = "ID";
                    dgvImagenes.Columns["NombreArchivo"]!.HeaderText = "Nombre";
                    dgvImagenes.Columns["FechaCaptura"]!.HeaderText = "Fecha Captura";
                    dgvImagenes.Columns["ZonaId"]!.HeaderText = "Zona ID";
                    dgvImagenes.Columns["UsuarioId"]!.HeaderText = "Usuario ID";
                    if (dgvImagenes.Columns["RutaArchivo"] != null)
                        dgvImagenes.Columns["RutaArchivo"]!.Visible = false;
                    if (dgvImagenes.Columns["FechaSubida"] != null)
                        dgvImagenes.Columns["FechaSubida"]!.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar imágenes: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MostrarPreview()
        {
            if (dgvImagenes == null || picPreview == null) return;

            if (dgvImagenes.SelectedRows.Count == 0)
            {
                if (picPreview.Image != null)
                {
                    picPreview.Image.Dispose();
                    picPreview.Image = null;
                }
                if (lblSinPreview != null)
                {
                    lblSinPreview.Text = "Selecciona una imagen para ver la vista previa";
                    lblSinPreview.Visible = true;
                }
                return;
            }

            string? ruta = dgvImagenes.SelectedRows[0].Cells["RutaArchivo"].Value?.ToString();

            if (string.IsNullOrEmpty(ruta))
            {
                if (picPreview.Image != null)
                {
                    picPreview.Image.Dispose();
                    picPreview.Image = null;
                }
                if (lblSinPreview != null)
                {
                    lblSinPreview.Text = "Esta imagen no tiene archivo asociado";
                    lblSinPreview.Visible = true;
                }
                return;
            }

            try
            {
                if (picPreview.Image != null)
                {
                    picPreview.Image.Dispose();
                    picPreview.Image = null;
                }

                if (ruta.StartsWith("http"))
                {
                    byte[] data = await httpClient.GetByteArrayAsync(ruta);
                    using (var ms = new MemoryStream(data))
                    {
                        picPreview.Image = Image.FromStream(ms);
                    }
                }
                else if (File.Exists(ruta))
                {
                    byte[] bytes = File.ReadAllBytes(ruta);
                    using (var ms = new MemoryStream(bytes))
                    {
                        picPreview.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    if (lblSinPreview != null)
                    {
                        lblSinPreview.Text = "No se encontró el archivo:\n" + ruta;
                        lblSinPreview.Visible = true;
                    }
                    return;
                }

                if (lblSinPreview != null) lblSinPreview.Visible = false;
            }
            catch (Exception ex)
            {
                if (lblSinPreview != null)
                {
                    lblSinPreview.Text = "Error al cargar imagen:\n" + ex.Message;
                    lblSinPreview.Visible = true;
                }
            }
        }

        private void MostrarImagenGrande()
        {
            if (picPreview?.Image == null)
            {
                MessageBox.Show("No hay imagen para ampliar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear ventana modal con la imagen en grande
            Form ventanaGrande = new Form
            {
                Text = "Vista Previa - DeforeStop",
                Size = new Size(1000, 750),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(20, 20, 20),
                FormBorderStyle = FormBorderStyle.Sizable,
                MaximizeBox = true,
                MinimizeBox = false,
                KeyPreview = true
            };

            PictureBox picGrande = new PictureBox
            {
                Image = picPreview.Image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(20, 20, 20),
                Cursor = Cursors.Hand
            };
            ventanaGrande.Controls.Add(picGrande);

            // Cerrar con ESC
            ventanaGrande.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) ventanaGrande.Close();
            };

            // Cerrar con clic en la imagen
            picGrande.Click += (s, e) => ventanaGrande.Close();

            // Botón cerrar
            Button btnCerrar = new Button
            {
                Text = "✕  Cerrar (ESC)",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(150, 40),
                Location = new Point(ventanaGrande.Width - 170, 10),
                BackColor = Colores.Rojo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => ventanaGrande.Close();
            ventanaGrande.Controls.Add(btnCerrar);
            btnCerrar.BringToFront();

            ventanaGrande.ShowDialog();
        }

        private void AbrirEditor(ImagenSatelital? imagen)
        {
            ImagenEditForm editor = new ImagenEditForm(imagen);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CargarImagenes();
                MostrarPreview();
            }
        }

        private void EditarSeleccionada()
        {
            if (dgvImagenes!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una imagen para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvImagenes.SelectedRows[0].Cells["Id"].Value;
            using (var db = new AppDbContext())
            {
                var imagen = db.ImagenesSatelitales.Find(id);
                if (imagen != null) AbrirEditor(imagen);
            }
        }

        private void EliminarSeleccionada()
        {
            if (dgvImagenes!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una imagen para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvImagenes.SelectedRows[0].Cells["Id"].Value;
            string nombre = dgvImagenes.SelectedRows[0].Cells["NombreArchivo"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar la imagen \"{nombre}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var db = new AppDbContext())
                    {
                        var imagen = db.ImagenesSatelitales.Find(id);
                        if (imagen != null)
                        {
                            // Borrar archivo físico si existe
                            if (!string.IsNullOrEmpty(imagen.RutaArchivo) &&
                                !imagen.RutaArchivo.StartsWith("http") &&
                                File.Exists(imagen.RutaArchivo))
                            {
                                try { File.Delete(imagen.RutaArchivo); } catch { }
                            }

                            db.ImagenesSatelitales.Remove(imagen);
                            db.SaveChanges();
                        }
                    }
                    CargarImagenes();
                    MessageBox.Show("Imagen eliminada correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}