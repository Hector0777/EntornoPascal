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
        //private ToolStripMenuItem sadasdasdToolStripMenuItem;
        //private RichTextBox richTextBox1;
        //private StatusStrip statusStrip1;
        private ToolStripStatusLabel wordCountLabel;
        private ToolStripStatusLabel lineCountLabel;
        private int tabCounter = 1;

        public Form1()
        {
            
            InitializeComponent();
            sadasdasdToolStripMenuItem.Click += menuAbrir_Click;
            CrearNuevaPestaña();
        }

        private void CrearNuevaPestaña(string contenido = "")
        {
            TabPage nuevaPestaña = new TabPage("Nueva Pestaña " + tabCounter);
            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 12),
                Text = string.IsNullOrEmpty(contenido) ? "program Ejemplo;\nbegin\n\nend." : contenido
            };

            nuevaPestaña.Controls.Add(richTextBox);
            tabControl1.TabPages.Add(nuevaPestaña);
            tabControl1.SelectedTab = nuevaPestaña;
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
