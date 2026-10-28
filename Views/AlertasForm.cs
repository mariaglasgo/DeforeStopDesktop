using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class AlertasForm : Form
    {
        private DataGridView? dgvAlertas;
        private System.ComponentModel.IContainer? components = null;
        private readonly AlertaController _controller = new AlertaController();

        public AlertasForm()
        {
            InitializeComponent();
            ConstruirUI();
            CargarAlertas();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Name = "AlertasForm";
            this.Text = "DeforeStop - Gestión de Alertas";
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
            this.Text = "DeforeStop - Gestión de Alertas";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Colores.FondoClaro;

            // Navbar
            Panel navbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.VerdeOscuro
            };

            Label lblTitulo = new Label
            {
                Text = "🚨 Gestión de Alertas",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            navbar.Controls.Add(lblTitulo);
            this.Controls.Add(navbar);

            // Botones
            Panel panelBotones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Colores.FondoClaro,
                Padding = new Padding(20)
            };

            Button btnNuevo = new Button
            {
                Text = "➕ Nueva Alerta",
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

            Button btnMarcarLeida = new Button
            {
                Text = "✅ Marcar Leída",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 45),
                Location = new Point(380, 15),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMarcarLeida.FlatAppearance.BorderSize = 0;
            btnMarcarLeida.Click += (s, e) => MarcarComoLeida();
            panelBotones.Controls.Add(btnMarcarLeida);

            Button btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(580, 15),
                BackColor = Colores.Rojo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) => EliminarSeleccionada();
            panelBotones.Controls.Add(btnEliminar);

            this.Controls.Add(panelBotones);

            // DataGridView
            dgvAlertas = new DataGridView
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
            dgvAlertas.RowTemplate.Height = 35;

            dgvAlertas.ColumnHeadersDefaultCellStyle.BackColor = Colores.VerdeOscuro;
            dgvAlertas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAlertas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAlertas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvAlertas.EnableHeadersVisualStyles = false;

            dgvAlertas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
            dgvAlertas.DefaultCellStyle.SelectionBackColor = Colores.VerdeNeon;
            dgvAlertas.DefaultCellStyle.SelectionForeColor = Color.White;

            this.Controls.Add(dgvAlertas);
            dgvAlertas.BringToFront();
        }

        private void CargarAlertas()
        {
            try
            {
                var alertas = _controller.ObtenerTodas();
                dgvAlertas!.DataSource = alertas;
                dgvAlertas.Columns["Id"]!.HeaderText = "ID";
                dgvAlertas.Columns["Mensaje"]!.HeaderText = "Mensaje";
                dgvAlertas.Columns["NombreZona"]!.HeaderText = "Zona";
                dgvAlertas.Columns["FechaAlerta"]!.HeaderText = "Fecha";
                dgvAlertas.Columns["Leida"]!.HeaderText = "Leída";
                dgvAlertas.Columns["ZonaId"]!.Visible = false;

                // Colorear filas según Leída
                foreach (DataGridViewRow row in dgvAlertas.Rows)
                {
                    bool leida = (bool)(row.Cells["Leida"].Value ?? false);
                    if (!leida)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                        row.DefaultCellStyle.ForeColor = Colores.Rojo;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar alertas: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirEditor(Alerta? alerta)
        {
            AlertaEditForm editor = new AlertaEditForm(alerta);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CargarAlertas();
            }
        }

        private void EditarSeleccionada()
        {
            if (dgvAlertas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una alerta para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvAlertas.SelectedRows[0].Cells["Id"].Value;
            var alerta = _controller.ObtenerPorId(id);
            if (alerta != null) AbrirEditor(alerta);
        }

        private void MarcarComoLeida()
        {
            if (dgvAlertas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una alerta.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvAlertas.SelectedRows[0].Cells["Id"].Value;
            if (_controller.MarcarComoLeida(id))
            {
                CargarAlertas();
                MessageBox.Show("Alerta marcada como leída.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EliminarSeleccionada()
        {
            if (dgvAlertas!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una alerta para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvAlertas.SelectedRows[0].Cells["Id"].Value;
            string mensaje = dgvAlertas.SelectedRows[0].Cells["Mensaje"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar la alerta \"{mensaje}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_controller.Eliminar(id))
                {
                    CargarAlertas();
                    MessageBox.Show("Alerta eliminada correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar alerta.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}