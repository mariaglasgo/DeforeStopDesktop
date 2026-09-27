using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ReportesForm : Form
    {
        private DataGridView? dgvReportes;
        private System.ComponentModel.IContainer? components = null;
        private readonly ReporteController _controller = new ReporteController();

        public ReportesForm()
        {
            InitializeComponent();
            ConstruirUI();
            CargarReportes();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Name = "ReportesForm";
            this.Text = "DeforeStop - Gestión de Reportes";
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
            this.Text = "DeforeStop - Gestión de Reportes";
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
                Text = "📊 Gestión de Reportes",
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
                Text = "➕ Nuevo Reporte",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(190, 45),
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
                Location = new Point(230, 15),
                BackColor = Colores.Dorado,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += (s, e) => EditarSeleccionado();
            panelBotones.Controls.Add(btnEditar);

            Button btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(390, 15),
                BackColor = Colores.Rojo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) => EliminarSeleccionado();
            panelBotones.Controls.Add(btnEliminar);

            this.Controls.Add(panelBotones);

            dgvReportes = new DataGridView
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
            dgvReportes.RowTemplate.Height = 35;

            dgvReportes.ColumnHeadersDefaultCellStyle.BackColor = Colores.VerdeOscuro;
            dgvReportes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReportes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvReportes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvReportes.EnableHeadersVisualStyles = false;

            dgvReportes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
            dgvReportes.DefaultCellStyle.SelectionBackColor = Colores.VerdeNeon;
            dgvReportes.DefaultCellStyle.SelectionForeColor = Color.White;

            this.Controls.Add(dgvReportes);
            dgvReportes.BringToFront();
        }

        private void CargarReportes()
        {
            try
            {
                var reportes = _controller.ObtenerTodos();
                dgvReportes!.DataSource = reportes;
                dgvReportes.Columns["Id"]!.HeaderText = "ID";
                dgvReportes.Columns["Titulo"]!.HeaderText = "Título";
                dgvReportes.Columns["FechaInicio"]!.HeaderText = "Fecha Inicio";
                dgvReportes.Columns["FechaFin"]!.HeaderText = "Fecha Fin";
                dgvReportes.Columns["ZonaId"]!.HeaderText = "Zona ID";
                dgvReportes.Columns["UsuarioId"]!.HeaderText = "Usuario ID";
                dgvReportes.Columns["FechaGeneracion"]!.HeaderText = "Fecha Generación";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reportes: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirEditor(Reporte? reporte)
        {
            ReporteEditForm editor = new ReporteEditForm(reporte);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CargarReportes();
            }
        }

        private void EditarSeleccionado()
        {
            if (dgvReportes!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un reporte para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvReportes.SelectedRows[0].Cells["Id"].Value;
            var reporte = _controller.ObtenerPorId(id);
            if (reporte != null) AbrirEditor(reporte);
        }

        private void EliminarSeleccionado()
        {
            if (dgvReportes!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un reporte para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvReportes.SelectedRows[0].Cells["Id"].Value;
            string titulo = dgvReportes.SelectedRows[0].Cells["Titulo"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar el reporte \"{titulo}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_controller.Eliminar(id))
                {
                    CargarReportes();
                    MessageBox.Show("Reporte eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar reporte.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}