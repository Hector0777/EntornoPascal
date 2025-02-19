using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PruebasPascal
{
    public partial class Form1 : Form
    {
        private ToolStripStatusLabel wordCountLabel;
        private ToolStripStatusLabel lineCountLabel;
        private ToolStripStatusLabel numeroHector;
        private ToolStripStatusLabel numeroAntonio;
        IndentationDrawer indentador;

        private int tabCounter = 1;

        public Form1()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            menuStrip1.Renderer = new CustomMenuRenderer();

            // Crear las etiquetas de conteo de palabras y líneas
            wordCountLabel = new ToolStripStatusLabel();
            lineCountLabel = new ToolStripStatusLabel();
            numeroHector = new ToolStripStatusLabel();
            numeroAntonio = new ToolStripStatusLabel();

            // Añadir las etiquetas al StatusStrip
            statusStrip1.Items.Add(wordCountLabel);
            statusStrip1.Items.Add(lineCountLabel);
            statusStrip1.Items.Add(numeroHector);
            statusStrip1.Items.Add(numeroAntonio);

            sadasdasdToolStripMenuItem.Click += menuAbrir_Click;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.MouseDown += TabControl1_MouseDown;

            button1.Click += (s, e) => SeleccionarColor("reservadas");
            button3.Click += (s, e) => SeleccionarColor("comentarios");
            button4.Click += (s, e) => SeleccionarColor("cadenas");

            // Crear una pestaña antes de asignar el indentador
            CrearNuevaPestaña();

            // Ahora sí, asignar el indentador a la pestaña creada
            AsignarIndentadorATabActiva();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AsignarIndentadorATabActiva();
        }

        private void AsignarIndentadorATabActiva()
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                RichTextBox rtb = tabControl1.SelectedTab.Controls[0] as RichTextBox;
                if (rtb != null)
                {
                    indentador = new IndentationDrawer(rtb);
                }
            }
        }

        private void SeleccionarColor(string tipo)
        {
            if (tabControl1.SelectedTab == null) return;
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ColorConfig config = (ColorConfig)tabControl1.SelectedTab.Tag;
                switch (tipo)
                {
                    case "reservadas": config.Reservadas = colorDialog.Color; break;
                    case "comentarios": config.Comentarios = colorDialog.Color; break;
                    case "cadenas": config.Cadenas = colorDialog.Color; break;
                }
                AplicarResaltado((RichTextBox)tabControl1.SelectedTab.Controls[0]);
            }
        }

        private void AplicarResaltado(RichTextBox rtb)
        {
            if (tabControl1.SelectedTab == null) return;

            ColorConfig config = (ColorConfig)tabControl1.SelectedTab.Tag;

            string[] palabrasReservadas = {
        "absolute", "and", "array", "asm", "begin", "case", "const", "constructor",
        "destructor", "div", "do", "downto", "else", "end", "external", "false",
        "file", "for", "forward", "function", "goto", "if", "implementation", "in",
        "inline", "interface", "label", "mod", "nil", "not", "object", "of", "on",
        "operator", "or", "packed", "procedure", "program", "record", "repeat",
        "set", "shl", "shr", "string", "then", "to", "true", "type", "unit", "until",
        "uses", "var", "while", "with", "xor"
    };

            Regex regexReservadas = new Regex($"\\b({string.Join("|", palabrasReservadas)})\\b", RegexOptions.IgnoreCase);
            Regex regexComentarios = new Regex(@"(\{.*?\}|//.*?$)", RegexOptions.Multiline);
            Regex regexCadenas = new Regex(@"'([^']*)'", RegexOptions.Singleline);

            int selStart = rtb.SelectionStart;
            int selLength = rtb.SelectionLength;

            rtb.SuspendLayout();

            // Restaurar todo el texto a color negro antes de aplicar los resaltados
            rtb.Select(0, rtb.Text.Length);
            rtb.SelectionColor = Color.Black;

            // Aplicar color a palabras reservadas
            foreach (Match m in regexReservadas.Matches(rtb.Text))
            {
                rtb.Select(m.Index, m.Length);
                rtb.SelectionColor = config.Reservadas;
            }

            // Aplicar color a comentarios
            foreach (Match m in regexComentarios.Matches(rtb.Text))
            {
                rtb.Select(m.Index, m.Length);
                rtb.SelectionColor = config.Comentarios;
            }

            // Aplicar color a cadenas de texto
            foreach (Match m in regexCadenas.Matches(rtb.Text))
            {
                rtb.Select(m.Index, m.Length);
                rtb.SelectionColor = config.Cadenas;
            }

            // Restaurar la posición del cursor y establecer el color negro para seguir escribiendo correctamente
            rtb.SelectionStart = selStart;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = Color.Black; // Esto es clave para que el texto nuevo sea negro

            rtb.ResumeLayout();
        }

        class ColorConfig
        {
            public Color Reservadas { get; set; } = Color.Blue;
            public Color Comentarios { get; set; } = Color.Green;
            public Color Cadenas { get; set; } = Color.Red;
        }
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tab = tabControl1.TabPages[e.Index];
            Rectangle rect = tabControl1.GetTabRect(e.Index);
            e.Graphics.DrawString(tab.Text, Font, Brushes.Black, rect.X + 2, rect.Y + 4);

            // Dibuja el botón "X"
            Rectangle closeButton = new Rectangle(rect.Right - 15, rect.Top + 5, 10, 10);
            e.Graphics.DrawString("X", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, closeButton);
        }

        private void TabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                Rectangle rect = tabControl1.GetTabRect(i);
                Rectangle closeButton = new Rectangle(rect.Right - 15, rect.Top + 5, 10, 10);

                if (closeButton.Contains(e.Location))
                {
                    if (tabControl1.TabPages.Count > 1)
                    {
                        tabControl1.TabPages.RemoveAt(i);
                    }
                    else
                    {
                        MessageBox.Show("No puedes cerrar la última pestaña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                }
            }
        }

        private void CrearNuevaPestaña(string contenido = "")
        {
            TabPage nuevaPestaña = new TabPage("Nueva " + tabCounter);
            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 12),
                Text = string.IsNullOrEmpty(contenido) ? "program Ejemplo;\nbegin\n\nend." : contenido
            };

            richTextBox.TextChanged += (s, e) => UpdateStatus(richTextBox.Text);

            nuevaPestaña.Controls.Add(richTextBox);
            tabControl1.TabPages.Add(nuevaPestaña);
            tabControl1.SelectedTab = nuevaPestaña;

            // Inicializa el Tag con un nuevo objeto ColorConfig
            nuevaPestaña.Tag = new ColorConfig();

            tabCounter++;
        }
        private void AbrirArchivoPascal()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos Pascal (*.pas)|*.pas",
                Title = "Abrir archivo Pascal"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string contenido = File.ReadAllText(openFileDialog.FileName);
                CrearNuevaPestaña(contenido);

                // Llamar a UpdateStatus para el contenido cargado
                RichTextBox rtb = (RichTextBox)tabControl1.SelectedTab.Controls[0];
                UpdateStatus(rtb.Text);
            }
        }

        private void menuAbrir_Click(object sender, EventArgs e)
        {
            AbrirArchivoPascal();
        }

        private void UpdateStatus(string content)
        {
            if (wordCountLabel == null || lineCountLabel == null)
            {
                MessageBox.Show("Las etiquetas del StatusStrip no están inicializadas.");
                return;
            }

            int wordCount = content.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int lineCount = content.Split('\n').Length;

            wordCountLabel.Text = $"Palabras: {wordCount}";
            lineCountLabel.Text = $"Líneas: {lineCount}";

            numeroHector.Text = "23310624";
            numeroAntonio.Text = "23310627";
        }

        private void sadasdasdToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public class CustomMenuRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected) // Si el mouse está sobre el ítem
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightGreen), e.Item.ContentRectangle); // Color personalizado
                    e.Item.ForeColor = Color.Black; // Cambia el color del texto si es necesario
                }
                else
                {
                    base.OnRenderMenuItemBackground(e); // Usa el renderizado predeterminado si no está seleccionado
                }
            }
        }
    }
}
