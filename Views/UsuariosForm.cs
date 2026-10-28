using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class UsuariosForm : Form
    {
        private DataGridView? dgvUsuarios;
        private System.ComponentModel.IContainer? components = null;
        private readonly UsuarioController _controller = new UsuarioController();

        public UsuariosForm()
        {
            InitializeComponent();
            ConstruirUI();
            CargarUsuarios();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Name = "UsuariosForm";
            this.Text = "DeforeStop - Gestión de Usuarios";
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
            this.Text = "DeforeStop - Gestión de Usuarios";
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
                Text = "👥 Gestión de Usuarios",
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
                Text = "➕ Nuevo Usuario",
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
            btnEditar.Click += (s, e) => EditarSeleccionado();
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
            btnEliminar.Click += (s, e) => EliminarSeleccionado();
            panelBotones.Controls.Add(btnEliminar);

            this.Controls.Add(panelBotones);

            // DataGridView
            dgvUsuarios = new DataGridView
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
            dgvUsuarios.RowTemplate.Height = 35;

            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Colores.VerdeOscuro;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvUsuarios.EnableHeadersVisualStyles = false;

            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Colores.VerdeNeon;
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;

            this.Controls.Add(dgvUsuarios);
            dgvUsuarios.BringToFront();
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = _controller.ObtenerTodos();
                dgvUsuarios!.DataSource = usuarios;
                dgvUsuarios.Columns["Id"]!.HeaderText = "ID";
                dgvUsuarios.Columns["Nombre"]!.HeaderText = "Nombre";
                dgvUsuarios.Columns["Correo"]!.HeaderText = "Correo";
                dgvUsuarios.Columns["Contrasena"]!.Visible = false;
                dgvUsuarios.Columns["Rol"]!.HeaderText = "Rol";
                dgvUsuarios.Columns["Activo"]!.HeaderText = "Activo";
                dgvUsuarios.Columns["FechaCreacion"]!.HeaderText = "Fecha Creación";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirEditor(Usuario? usuario)
        {
            UsuarioEditForm editor = new UsuarioEditForm(usuario);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CargarUsuarios();
            }
        }

        private void EditarSeleccionado()
        {
            if (dgvUsuarios!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            var usuario = _controller.ObtenerPorId(id);
            if (usuario != null) AbrirEditor(usuario);
        }

        private void EliminarSeleccionado()
        {
            if (dgvUsuarios!.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            string nombre = dgvUsuarios.SelectedRows[0].Cells["Nombre"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar al usuario \"{nombre}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_controller.Eliminar(id))
                {
                    CargarUsuarios();
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar usuario.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}