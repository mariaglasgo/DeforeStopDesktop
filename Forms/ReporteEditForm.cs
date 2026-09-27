using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DeforeStopDesktop.Config;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Forms
{
    public partial class ReporteEditForm : Form
    {
        private Reporte? _reporte;
        private bool _esNuevo;
        private System.ComponentModel.IContainer? components = null;

        public ReporteEditForm(Reporte? reporte)
        {
            _reporte = reporte;
            _esNuevo = (reporte == null);
            InitializeComponent();
            ConstruirUI();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 550);
            this.Name = "ReporteEditForm";
            this.Text = "Reporte";
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
            this.Text = _esNuevo ? "Nuevo Reporte" : "Editar Reporte";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Colores.FondoClaro;

            Label lblTitulo = new Label
            {
                Text = _esNuevo ? "📊 Nuevo Reporte" : "✏️ Editar Reporte",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Colores.VerdeOscuro,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitulo);

            // Título
            Label lblTit = new Label
            {
                Text = "Título del reporte:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 80)
            };
            this.Controls.Add(lblTit);

            TextBox txtTitulo = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 105),
                Text = _reporte?.Titulo ?? ""
            };
            this.Controls.Add(txtTitulo);

            // Fecha Inicio
            Label lblFI = new Label
            {
                Text = "Fecha Inicio:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 150)
            };
            this.Controls.Add(lblFI);

            DateTimePicker dtpFI = new DateTimePicker
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 175),
                Format = DateTimePickerFormat.Short,
                Value = _reporte?.FechaInicio ?? DateTime.Now
            };
            this.Controls.Add(dtpFI);

            // Fecha Fin
            Label lblFF = new Label
            {
                Text = "Fecha Fin:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 220)
            };
            this.Controls.Add(lblFF);

            DateTimePicker dtpFF = new DateTimePicker
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 245),
                Format = DateTimePickerFormat.Short,
                Value = _reporte?.FechaFin ?? DateTime.Now
            };
            this.Controls.Add(dtpFF);

            // Zona (ComboBox dinámico)
            Label lblZona = new Label
            {
                Text = "Zona:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 290)
            };
            this.Controls.Add(lblZona);

            ComboBox cmbZona = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 315),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbZona);

            // Usuario (ComboBox dinámico)
            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Colores.TextoOscuro,
                AutoSize = true,
                Location = new Point(30, 360)
            };
            this.Controls.Add(lblUsuario);

            ComboBox cmbUsuario = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 30),
                Location = new Point(30, 385),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbUsuario);

            // Cargar datos de los ComboBox
            using (var db = new AppDbContext())
            {
                var zonas = db.Zonas.ToList();
                cmbZona.DataSource = zonas;
                cmbZona.DisplayMember = "Nombre";
                cmbZona.ValueMember = "Id";

                var usuarios = db.Usuarios.ToList();
                cmbUsuario.DataSource = usuarios;
                cmbUsuario.DisplayMember = "Nombre";
                cmbUsuario.ValueMember = "Id";

                if (_reporte != null)
                {
                    cmbZona.SelectedValue = _reporte.ZonaId;
                    cmbUsuario.SelectedValue = _reporte.UsuarioId;
                }
            }

            // Botón Guardar
            Button btnGuardar = new Button
            {
                Text = "💾 GUARDAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(30, 445),
                BackColor = Colores.VerdeNeon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                    cmbZona.SelectedValue == null ||
                    cmbUsuario.SelectedValue == null)
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
                            var nuevo = new Reporte
                            {
                                Titulo = txtTitulo.Text,
                                FechaInicio = dtpFI.Value,
                                FechaFin = dtpFF.Value,
                                ZonaId = (int)cmbZona.SelectedValue,
                                UsuarioId = (int)cmbUsuario.SelectedValue,
                                FechaGeneracion = DateTime.Now
                            };
                            db.Reportes.Add(nuevo);
                        }
                        else
                        {
                            var r = db.Reportes.Find(_reporte!.Id);
                            if (r != null)
                            {
                                r.Titulo = txtTitulo.Text;
                                r.FechaInicio = dtpFI.Value;
                                r.FechaFin = dtpFF.Value;
                                r.ZonaId = (int)cmbZona.SelectedValue;
                                r.UsuarioId = (int)cmbUsuario.SelectedValue;
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

            // Botón Cancelar
            Button btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(250, 445),
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