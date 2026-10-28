using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ZonaEditForm : Form
    {
        private Zona? _zona;
        private bool _esNuevo;
        private readonly ZonaController _controller = new ZonaController();
        private System.ComponentModel.IContainer? components = null;

        public ZonaEditForm(Zona? zona)
        {
            _zona = zona;
            _esNuevo = (zona == null);
            InitializeComponent();
            ConstruirUI();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Name = "ZonaEditForm";
            this.Text = "Zona";
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
            this.Text = _esNuevo ? "Nueva Zona" : "Editar Zona";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Colores.FondoClaro;

            Label lblTitulo = new Label
            {
                Text = _esNuevo ? "🌍 Nueva Zona" : "✏️ Editar Zona",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitulo);

            Label lblNombre = new Label
            {
                Text = "Nombre de la zona:",
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
                Text = _zona?.Nombre ?? ""
            };
            this.Controls.Add(txtNombre);

            Label lblDepto = new Label
            {
                Text = "Departamento:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 150)
            };
            this.Controls.Add(lblDepto);

            ComboBox cmbDepto = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 175),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cmbDepto.Items.AddRange(new object[] {
                "Jinotega", "Río San Juan", "Estelí", "Granada", "Matagalpa",
                "Nueva Segovia", "Madriz", "Chinandega", "León", "Managua",
                "Masaya", "Carazo", "Rivas", "Boaco", "Chontales",
                "Costa Caribe Norte", "Costa Caribe Sur"
            });
            cmbDepto.Text = _zona?.Departamento ?? "";
            this.Controls.Add(cmbDepto);

            Label lblLat = new Label
            {
                Text = "Latitud (opcional):",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 220)
            };
            this.Controls.Add(lblLat);

            TextBox txtLat = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 245),
                Text = _zona?.Latitud?.ToString() ?? "",
                PlaceholderText = "Ej: 14.500000"
            };
            this.Controls.Add(txtLat);

            Label lblLon = new Label
            {
                Text = "Longitud (opcional):",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 290)
            };
            this.Controls.Add(lblLon);

            TextBox txtLon = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 315),
                Text = _zona?.Longitud?.ToString() ?? "",
                PlaceholderText = "Ej: -85.000000"
            };
            this.Controls.Add(txtLon);

            Button btnGuardar = new Button
            {
                Text = "💾 GUARDAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(30, 380),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(cmbDepto.Text))
                {
                    MessageBox.Show("Complete los campos obligatorios.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    decimal? lat = null, lon = null;
                    if (decimal.TryParse(txtLat.Text, out decimal l)) lat = l;
                    if (decimal.TryParse(txtLon.Text, out decimal lo)) lon = lo;

                    bool resultado;

                    if (_esNuevo)
                    {
                        var nueva = new Zona
                        {
                            Nombre = txtNombre.Text,
                            Departamento = cmbDepto.Text,
                            Latitud = lat,
                            Longitud = lon
                        };
                        resultado = _controller.Crear(nueva);
                    }
                    else
                    {
                        _zona!.Nombre = txtNombre.Text;
                        _zona.Departamento = cmbDepto.Text;
                        _zona.Latitud = lat;
                        _zona.Longitud = lon;
                        resultado = _controller.Actualizar(_zona);
                    }

                    if (resultado)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al guardar la zona.", "Error",
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

            Button btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(250, 380),
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