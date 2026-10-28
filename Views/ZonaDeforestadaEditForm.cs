using System;
using System.Drawing;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Controllers;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ZonaDeforestadaEditForm : Form
    {
        private ZonaDeforestada? _zona;
        private bool _esNuevo;
        private readonly ZonaDeforestadaController _controller = new ZonaDeforestadaController();
        private readonly ZonaController _zonaController = new ZonaController();
        private System.ComponentModel.IContainer? components = null;

        public ZonaDeforestadaEditForm(ZonaDeforestada? zona)
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
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Name = "ZonaDeforestadaEditForm";
            this.Text = "Registro Deforestación";
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
            this.Text = _esNuevo ? "Nuevo Registro" : "Editar Registro";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Colores.FondoClaro;

            Label lblTitulo = new Label
            {
                Text = _esNuevo ? "🌲 Nueva Zona Deforestada" : "✏️ Editar Registro",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitulo);

            // Zona
            Label lblZona = new Label
            {
                Text = "Zona Afectada:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 80)
            };
            this.Controls.Add(lblZona);

            ComboBox cmbZona = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 105),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbZona);

            // Hectáreas
            Label lblHectareas = new Label
            {
                Text = "Hectáreas afectadas:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 150)
            };
            this.Controls.Add(lblHectareas);

            TextBox txtHectareas = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 175),
                Text = _zona?.Hectareas.ToString() ?? ""
            };
            this.Controls.Add(txtHectareas);

            // Fecha
            Label lblFecha = new Label
            {
                Text = "Fecha de Detección:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 220)
            };
            this.Controls.Add(lblFecha);

            DateTimePicker dtpFecha = new DateTimePicker
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 245),
                Format = DateTimePickerFormat.Short,
                Value = _zona?.FechaDeteccion ?? DateTime.Now
            };
            this.Controls.Add(dtpFecha);

            // Nivel Alerta
            Label lblNivel = new Label
            {
                Text = "Nivel de Alerta:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 290)
            };
            this.Controls.Add(lblNivel);

            ComboBox cmbNivel = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 315),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNivel.Items.AddRange(new object[] { "Bajo", "Medio", "Alto", "Crítico" });
            cmbNivel.SelectedItem = _zona?.NivelAlerta ?? "Bajo";
            this.Controls.Add(cmbNivel);

            // Cargar zonas
            try
            {
                var zonas = _zonaController.ObtenerTodas();
                cmbZona.DataSource = zonas;
                cmbZona.DisplayMember = "Nombre";
                cmbZona.ValueMember = "Id";

                if (_zona != null)
                    cmbZona.SelectedValue = _zona.ZonaId;
            }
            catch { }

            // Botón Guardar
            Button btnGuardar = new Button
            {
                Text = "💾 GUARDAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(30, 360),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtHectareas.Text) || cmbZona.SelectedValue == null)
                {
                    MessageBox.Show("Complete todos los campos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtHectareas.Text, out decimal hectareas))
                {
                    MessageBox.Show("Las hectáreas deben ser un número válido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool resultado;

                    if (_esNuevo)
                    {
                        var nueva = new ZonaDeforestada
                        {
                            ZonaId = (int)cmbZona.SelectedValue!,
                            Hectareas = hectareas,
                            FechaDeteccion = dtpFecha.Value,
                            NivelAlerta = cmbNivel.SelectedItem?.ToString() ?? "Bajo"
                        };
                        resultado = _controller.Crear(nueva);
                    }
                    else
                    {
                        _zona!.ZonaId = (int)cmbZona.SelectedValue!;
                        _zona.Hectareas = hectareas;
                        _zona.FechaDeteccion = dtpFecha.Value;
                        _zona.NivelAlerta = cmbNivel.SelectedItem?.ToString() ?? "Bajo";
                        resultado = _controller.Actualizar(_zona);
                    }

                    if (resultado)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al guardar.", "Error",
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
                Location = new Point(250, 360),
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