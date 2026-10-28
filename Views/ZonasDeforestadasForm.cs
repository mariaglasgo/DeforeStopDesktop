using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ZonasDeforestadasForm : Form
    {
        private DataGridView? dgvZonas;
        private System.ComponentModel.IContainer? components = null;
        private readonly ZonaDeforestadaController _controller = new ZonaDeforestadaController();

        public ZonasDeforestadasForm()
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
            this.Name = "ZonasDeforestadasForm";
            this.Text = "DeforeStop - Zonas Deforestadas";
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
            this.Text = "DeforeStop - Zonas Deforestadas";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Colores.FondoClaro;

            // 1. DataGridView (Fondo)
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

            // 2. Panel de Botones
            Panel panelBotones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.FondoClaro,
                Padding = new Padding(20)
            };

            Button btnNuevo = new Button
            {
                Text = "➕ Nuevo Registro",
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

            // 3. Navbar (Va al final para quedar arriba de todo)
            Panel navbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.VerdeOscuro
            };

            Label lblTitulo = new Label
            {
                Text = "🌲 Zonas Deforestadas",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(lblTitulo);
            this.Controls.Add(navbar);
        }

        private void CargarZonas()
        {
            try
            {
                var zonas = _controller.ObtenerTodas();
                dgvZonas!.DataSource = zonas;
                dgvZonas.Columns["Id"]!.HeaderText = "ID";
                dgvZonas.Columns["NombreZona"]!.HeaderText = "Zona Afectada";
                dgvZonas.Columns["Hectareas"]!.HeaderText = "Hectáreas";
                dgvZonas.Columns["FechaDeteccion"]!.HeaderText = "Fecha Detección";
                dgvZonas.Columns["NivelAlerta"]!.HeaderText = "Nivel";
                dgvZonas.Columns["ZonaId"]!.Visible = false;

                // Colorear según Nivel de Alerta
                foreach (DataGridViewRow row in dgvZonas.Rows)
                {
                    string nivel = row.Cells["NivelAlerta"].Value?.ToString() ?? "";
                    if (nivel == "Crítico")
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    else if (nivel == "Alto")
                        row.DefaultCellStyle.ForeColor = Color.OrangeRed;
                    else if (nivel == "Medio")
                        row.DefaultCellStyle.ForeColor = Color.DarkGoldenrod;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar zonas: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirEditor(ZonaDeforestada? zona)
        {
            ZonaDeforestadaEditForm editor = new ZonaDeforestadaEditForm(zona);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CargarZonas();
            }
        }

        private void EditarSeleccionada()
        {
            if (dgvZonas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un registro para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvZonas.SelectedRows[0].Cells["Id"].Value;
            var zona = _controller.ObtenerPorId(id);
            if (zona != null) AbrirEditor(zona);
        }

        private void EliminarSeleccionada()
        {
            if (dgvZonas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un registro para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvZonas.SelectedRows[0].Cells["Id"].Value;
            string nombre = dgvZonas.SelectedRows[0].Cells["NombreZona"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar el registro de \"{nombre}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_controller.Eliminar(id))
                {
                    CargarZonas();
                    MessageBox.Show("Registro eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}