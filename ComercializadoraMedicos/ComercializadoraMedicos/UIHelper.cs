using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public static class UIHelper
    {
        // Paleta de colores moderna - TEMA CLARO
        public static readonly Color PrimaryColor = Color.FromArgb(37, 99, 235);      // Azul moderno
        public static readonly Color SecondaryColor = Color.FromArgb(100, 116, 139);  // Gris azulado
        public static readonly Color AccentColor = Color.FromArgb(59, 130, 246);      // Azul claro
        public static readonly Color LightBackground = Color.FromArgb(248, 250, 252); // Fondo claro
        public static readonly Color CardBackground = Color.White;                    // Fondo de tarjetas
        public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);        // Texto principal oscuro
        public static readonly Color TextSecondary = Color.FromArgb(100, 116, 139);   // Texto secundario
        public static readonly Color BorderColor = Color.FromArgb(226, 232, 240);     // Bordes claros
        public static readonly Color SuccessColor = Color.FromArgb(34, 197, 94);      // Verde éxito
        public static readonly Color WarningColor = Color.FromArgb(251, 146, 60);     // Naranja advertencia
        public static readonly Color DangerColor = Color.FromArgb(239, 68, 68);       // Rojo peligro

        public enum ButtonStyle
        {
            Primary,
            Secondary,
            Success,
            Danger,
            Warning
        }

        /// <summary>
        /// Aplica el estilo moderno a un formulario completo
        /// </summary>
        public static void ApplyModernStyle(Form form)
        {
            // Apply basic colors but avoid overriding designer font and padding so layouts remain intact
            form.BackColor = LightBackground;
            form.ForeColor = TextPrimary;

            ApplyStylesToControls(form.Controls);
            AdjustButtonSpacing(form.Controls);
        }

        /// <summary>
        /// Ajusta el espaciado entre botones automáticamente
        /// </summary>
        private static void AdjustButtonSpacing(Control.ControlCollection controls)
        {
            List<Button> buttons = new List<Button>();
            
            // Recolectar todos los botones
            foreach (Control control in controls)
            {
                if (control is Button btn)
                {
                    buttons.Add(btn);
                }
                
                if (control.HasChildren)
                {
                    AdjustButtonSpacing(control.Controls);
                }
            }

            // Agrupar botones por fila (misma coordenada Y aproximada)
            var buttonRows = buttons.GroupBy(b => b.Top / 10).OrderBy(g => g.Key);
            
            foreach (var row in buttonRows)
            {
                var rowButtons = row.OrderBy(b => b.Left).ToList();
                
                // Ajustar espaciado horizontal entre botones de la misma fila
                for (int i = 1; i < rowButtons.Count; i++)
                {
                    int prevRight = rowButtons[i - 1].Right;
                    int currentLeft = rowButtons[i].Left;
                    
                    // Si están muy pegados (menos de 8px), ajustar
                    if (currentLeft - prevRight < 8)
                    {
                        rowButtons[i].Left = prevRight + 10;
                    }
                }
            }
        }

        /// <summary>
        /// Aplica estilos recursivamente a todos los controles
        /// </summary>
        private static void ApplyStylesToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Skip styling for the top panel created in designer
                if (control is Panel && string.Equals(control.Name, "panelTop", StringComparison.OrdinalIgnoreCase))
                {
                    // keep designer styling for top panel
                }
                else if (control is TextBox txt)
                {
                    StyleTextBox(txt);
                }
                else if (control is ComboBox cmb)
                {
                    StyleComboBox(cmb);
                }
                else if (control is Label lbl)
                {
                    // Preserve large title labels created in designer (do not override font if already large)
                    if (lbl.Font.Size < 14)
                    {
                        StyleLabel(lbl, false);
                    }
                }
                else if (control is Panel panel)
                {
                    StylePanel(panel, false);
                }
                else if (control is DataGridView dgv)
                {
                    // No aplicar aquí, se hace manualmente
                }
                else if (control is GroupBox gb)
                {
                    gb.ForeColor = TextPrimary;
                    gb.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }

                // Aplicar recursivamente a controles hijos
                if (control.HasChildren)
                {
                    ApplyStylesToControls(control.Controls);
                }
            }
        }

        /// <summary>
        /// Estiliza un botón con el estilo especificado
        /// </summary>
        public static void StyleButton(Button btn, ButtonStyle style = ButtonStyle.Primary)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 6, 12, 6);
            btn.Margin = new Padding(5);
            btn.MinimumSize = new Size(90, 32);
            btn.AutoSize = true;

            switch (style)
            {
                case ButtonStyle.Primary:
                    btn.BackColor = PrimaryColor;
                    btn.ForeColor = Color.White;
                    break;
                case ButtonStyle.Secondary:
                    btn.BackColor = SecondaryColor;
                    btn.ForeColor = Color.White;
                    break;
                case ButtonStyle.Success:
                    btn.BackColor = SuccessColor;
                    btn.ForeColor = Color.White;
                    break;
                case ButtonStyle.Danger:
                    btn.BackColor = DangerColor;
                    btn.ForeColor = Color.White;
                    break;
                case ButtonStyle.Warning:
                    btn.BackColor = WarningColor;
                    btn.ForeColor = Color.White;
                    break;
            }

            AddButtonHoverEffect(btn, style);
        }

        /// <summary>
        /// Agrega efecto hover a un botón
        /// </summary>
        private static void AddButtonHoverEffect(Button btn, ButtonStyle style)
        {
            Color originalColor = btn.BackColor;
            Color hoverColor = DarkenColor(originalColor, 15);

            btn.MouseEnter += (s, e) => {
                btn.BackColor = hoverColor;
            };

            btn.MouseLeave += (s, e) => {
                btn.BackColor = originalColor;
            };
        }

        /// <summary>
        /// Oscurece un color en un porcentaje
        /// </summary>
        private static Color DarkenColor(Color color, int percent)
        {
            int r = Math.Max(0, color.R - (color.R * percent / 100));
            int g = Math.Max(0, color.G - (color.G * percent / 100));
            int b = Math.Max(0, color.B - (color.B * percent / 100));
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Estiliza un TextBox
        /// </summary>
        public static void StyleTextBox(TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 9F);
            txt.Margin = new Padding(3);
            txt.Height = 26;
        }

        /// <summary>
        /// Estiliza un ComboBox
        /// </summary>
        public static void StyleComboBox(ComboBox cmb)
        {
            cmb.BackColor = Color.White;
            cmb.ForeColor = TextPrimary;
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = new Font("Segoe UI", 9F);
            cmb.Margin = new Padding(3);
            cmb.Height = 26;
        }

        /// <summary>
        /// Estiliza un Label
        /// </summary>
        public static void StyleLabel(Label lbl, bool isTitle = false)
        {
            lbl.ForeColor = TextPrimary;
            if (isTitle)
            {
                lbl.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            }
            else
            {
                lbl.Font = new Font("Segoe UI", 9F);
            }
            lbl.Margin = new Padding(3);
            lbl.AutoSize = true;
        }

        /// <summary>
        /// Estiliza un Panel
        /// </summary>
        public static void StylePanel(Panel panel, bool isCard = false)
        {
            if (isCard)
            {
                panel.BackColor = CardBackground;
                panel.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                panel.BackColor = LightBackground;
            }
        }

        /// <summary>
        /// Estiliza un DataGridView con tema claro
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            // Configuración general
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = BorderColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Estilo de encabezados de columna
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(6);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Estilo de celdas
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.Padding = new Padding(6);
            dgv.RowTemplate.Height = 32;

            // Filas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;
        }
    }
}
