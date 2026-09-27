using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class UsuarioEditForm : Form
    {
        private Usuario? _usuario;
        private bool _esNuevo;

        public UsuarioEditForm(Usuario? usuario)
        {
            _usuario = usuario;
            _esNuevo = (usuario == null);
            InitializeComponent();
            ConstruirUI();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 550);
            this.Name = "UsuarioEditForm";
            this.Text = "Usuario";
        }

        private System.ComponentModel.IContainer? components = null;

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
            this.Text = _esNuevo ? "Nuevo Usuario" : "Editar Usuario";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Colores.FondoClaro;

            Label lblTitulo = new Label
            {
                Text = _esNuevo ? "👤 Nuevo Usuario" : "✏️ Editar Usuario",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitulo);

            Label lblNombre = new Label
            {
                Text = "Nombre completo:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 80)
            };
            this.Controls.Add(lblNombre);

            TextBox txtNombre = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 105),
                Text = _usuario?.Nombre ?? ""
            };
            this.Controls.Add(txtNombre);

            Label lblCorreo = new Label
            {
                Text = "Correo electrónico:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 150)
            };
            this.Controls.Add(lblCorreo);

            TextBox txtCorreo = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 175),
                Text = _usuario?.Correo ?? ""
            };
            this.Controls.Add(txtCorreo);

            Label lblPass = new Label
            {
                Text = "Contraseña:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 220)
            };
            this.Controls.Add(lblPass);

            TextBox txtPass = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 245),
                UseSystemPasswordChar = true,
                Text = _usuario?.Contrasena ?? ""
            };
            this.Controls.Add(txtPass);

            Label lblRol = new Label
            {
                Text = "Rol:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 290)
            };
            this.Controls.Add(lblRol);

            ComboBox cmbRol = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 315),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRol.Items.AddRange(new object[] { "Administrador", "Analista", "Observador" });
            cmbRol.SelectedItem = _usuario?.Rol ?? "Observador";
            this.Controls.Add(cmbRol);

            CheckBox chkActivo = new CheckBox
            {
                Text = "Usuario activo",
                Font = new Font("Segoe UI", 10),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 360),
                Checked = _usuario?.Activo ?? true
            };
            this.Controls.Add(chkActivo);

            Button btnGuardar = new Button
            {
                Text = "💾 GUARDAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(30, 420),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                    string.IsNullOrWhiteSpace(txtPass.Text))
                {
                    MessageBox.Show("Complete todos los campos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (var db = new AppDbContext())
                    {
                        if (_esNuevo)
                        {
                            var nuevo = new Usuario
                            {
                                Nombre = txtNombre.Text,
                                Correo = txtCorreo.Text,
                                Contrasena = txtPass.Text,
                                Rol = cmbRol.SelectedItem?.ToString() ?? "Observador",
                                Activo = chkActivo.Checked,
                                FechaCreacion = DateTime.Now
                            };
                            db.Usuarios.Add(nuevo);
                        }
                        else
                        {
                            var u = db.Usuarios.Find(_usuario!.Id);
                            if (u != null)
                            {
                                u.Nombre = txtNombre.Text;
                                u.Correo = txtCorreo.Text;
                                u.Contrasena = txtPass.Text;
                                u.Rol = cmbRol.SelectedItem?.ToString() ?? "Observador";
                                u.Activo = chkActivo.Checked;
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
                Size = new Size(200, 45),
                Location = new Point(250, 420),
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