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
        private int tabCounter = 1;

        public Form1()
        {
            InitializeComponent();
            sadasdasdToolStripMenuItem.Click += menuAbrir_Click;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.MouseDown += TabControl1_MouseDown;


            button1.Click += (s, e) => SeleccionarColor("reservadas");
            button3.Click += (s, e) => SeleccionarColor("comentarios");
            button4.Click += (s, e) => SeleccionarColor("cadenas");
            CrearNuevaPestaña();
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
            ColorConfig config = (ColorConfig)tabControl1.SelectedTab.Tag;
            string[] palabrasReservadas = { "program", "begin", "end", "var", "if", "then", "else", "while", "do", "procedure", "function" };
            Regex regexReservadas = new Regex($"\\b({string.Join("|", palabrasReservadas)})\\b", RegexOptions.IgnoreCase);
            Regex regexComentarios = new Regex(@"(\{.*?\}|//.*?$)", RegexOptions.Multiline);
            Regex regexCadenas = new Regex("\"(?:[^\"]|\"\")*?\"");

            int selStart = rtb.SelectionStart;
            rtb.SelectAll();
            rtb.SelectionColor = Color.Black;

            foreach (Match m in regexReservadas.Matches(rtb.Text))
            {
                rtb.Select(m.Index, m.Length);
                rtb.SelectionColor = config.Reservadas;
            }
            foreach (Match m in regexComentarios.Matches(rtb.Text))
            {
                rtb.Select(m.Index, m.Length);
                rtb.SelectionColor = config.Comentarios;
            }
            foreach (Match m in regexCadenas.Matches(rtb.Text))
            {
                rtb.Select(m.Index, m.Length);
                rtb.SelectionColor = config.Cadenas;
            }

            rtb.SelectionStart = selStart;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = Color.Black;
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
    }
}
