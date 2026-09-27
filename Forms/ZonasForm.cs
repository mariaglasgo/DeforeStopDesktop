using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ZonasForm : Form
    {
        private DataGridView? dgvZonas;
        private System.ComponentModel.IContainer? components = null;

        public ZonasForm()
        {
            InitializeComponent();
            ConstruirUI();
            CargarZonas();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Name = "ZonasForm";
            this.Text = "DeforeStop - Gestión de Zonas";
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
            this.Text = "DeforeStop - Gestión de Zonas";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Colores.FondoClaro;

            Panel navbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.VerdeOscuro
            };

            Label lblTitulo = new Label
            {
                Text = "🌍 Gestión de Zonas",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(lblTitulo);
            this.Controls.Add(navbar);

            Panel panelBotones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.FondoClaro,
                Padding = new Padding(20)
            };

            Button btnNuevo = new Button
            {
                Text = "➕ Nueva Zona",
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

            dgvZonas = new DataGridView
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
            dgvZonas.RowTemplate.Height = 35;

            dgvZonas.ColumnHeadersDefaultCellStyle.BackColor = Colores.VerdeOscuro;
            dgvZonas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvZonas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvZonas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvZonas.EnableHeadersVisualStyles = false;

            dgvZonas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
            dgvZonas.DefaultCellStyle.SelectionBackColor = Colores.VerdeNeon;
            dgvZonas.DefaultCellStyle.SelectionForeColor = Color.White;

            this.Controls.Add(dgvZonas);
            dgvZonas.BringToFront();
        }

        private void CargarZonas()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var zonas = db.Zonas.OrderBy(z => z.Id).ToList();
                    dgvZonas!.DataSource = zonas;
                    dgvZonas.Columns["Id"]!.HeaderText = "ID";
                    dgvZonas.Columns["Nombre"]!.HeaderText = "Nombre";
                    dgvZonas.Columns["Departamento"]!.HeaderText = "Departamento";
                    dgvZonas.Columns["Latitud"]!.HeaderText = "Latitud";
                    dgvZonas.Columns["Longitud"]!.HeaderText = "Longitud";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar zonas: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirEditor(Zona? zona)
        {
            ZonaEditForm editor = new ZonaEditForm(zona);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CargarZonas();
            }
        }

        private void EditarSeleccionada()
        {
            if (dgvZonas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una zona para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvZonas.SelectedRows[0].Cells["Id"].Value;
            using (var db = new AppDbContext())
            {
                var zona = db.Zonas.Find(id);
                if (zona != null) AbrirEditor(zona);
            }
        }

        private void EliminarSeleccionada()
        {
            if (dgvZonas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una zona para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvZonas.SelectedRows[0].Cells["Id"].Value;
            string nombre = dgvZonas.SelectedRows[0].Cells["Nombre"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar la zona \"{nombre}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var db = new AppDbContext())
                    {
                        var zona = db.Zonas.Find(id);
                        if (zona != null)
                        {
                            db.Zonas.Remove(zona);
                            db.SaveChanges();
                        }
                    }
                    CargarZonas();
                    MessageBox.Show("Zona eliminada correctamente.", "Éxito",
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