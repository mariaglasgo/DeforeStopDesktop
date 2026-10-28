using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class AlertaEditForm : Form
    {
        private Alerta? _alerta;
        private bool _esNuevo;
        private readonly AlertaController _controller = new AlertaController();
        private readonly ZonaController _zonaController = new ZonaController();
        private System.ComponentModel.IContainer? components = null;

        public AlertaEditForm(Alerta? alerta)
        {
            _alerta = alerta;
            _esNuevo = (alerta == null);
            InitializeComponent();
            ConstruirUI();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Name = "AlertaEditForm";
            this.Text = "Alerta";
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
            this.Text = _esNuevo ? "Nueva Alerta" : "Editar Alerta";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Colores.FondoClaro;

            Label lblTitulo = new Label
            {
                Text = _esNuevo ? "🚨 Nueva Alerta" : "✏️ Editar Alerta",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitulo);

            // Mensaje
            Label lblMensaje = new Label
            {
                Text = "Mensaje:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 80)
            };
            this.Controls.Add(lblMensaje);

            TextBox txtMensaje = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 60),
                Location = new Point(30, 105),
                Multiline = true,
                Text = _alerta?.Mensaje ?? ""
            };
            this.Controls.Add(txtMensaje);

            // Zona
            Label lblZona = new Label
            {
                Text = "Zona:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 180)
            };
            this.Controls.Add(lblZona);

            ComboBox cmbZona = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 205),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbZona);

            // Cargar zonas
            try
            {
                var zonas = _zonaController.ObtenerTodas();
                cmbZona.DataSource = zonas;
                cmbZona.DisplayMember = "Nombre";
                cmbZona.ValueMember = "Id";

                if (_alerta != null)
                    cmbZona.SelectedValue = _alerta.ZonaId;
            }
            catch { }

            // Leída
            CheckBox chkLeida = new CheckBox
            {
                Text = "Alerta leída",
                Font = new Font("Segoe UI", 10),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 250),
                Checked = _alerta?.Leida ?? false
            };
            this.Controls.Add(chkLeida);

            // Botón Guardar
            Button btnGuardar = new Button
            {
                Text = "💾 GUARDAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(30, 300),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMensaje.Text) || cmbZona.SelectedValue == null)
                {
                    MessageBox.Show("Complete todos los campos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool resultado;

                    if (_esNuevo)
                    {
                        var nueva = new Alerta
                        {
                            Mensaje = txtMensaje.Text,
                            ZonaId = (int)cmbZona.SelectedValue!,
                            Leida = chkLeida.Checked
                        };
                        resultado = _controller.Crear(nueva);
                    }
                    else
                    {
                        _alerta!.Mensaje = txtMensaje.Text;
                        _alerta.ZonaId = (int)cmbZona.SelectedValue!;
                        _alerta.Leida = chkLeida.Checked;
                        resultado = _controller.Actualizar(_alerta);
                    }

                    if (resultado)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al guardar la alerta.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Controls.Add(btnGuardar);

            // Botón Cancelar
            Button btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(250, 300),
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