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
        public Form1()
        {
            InitializeComponent();
            sadasdasdToolStripMenuItem.Click += OpenFile;
            if (richTextBox1 == null) richTextBox1 = new RichTextBox();
            if (statusStrip1 == null) statusStrip1 = new StatusStrip();
            if (wordCountLabel == null) wordCountLabel = new ToolStripStatusLabel("Palabras: 0");
            if (lineCountLabel == null) lineCountLabel = new ToolStripStatusLabel("Líneas: 0");

            statusStrip1.Items.Add(wordCountLabel);
            statusStrip1.Items.Add(lineCountLabel);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos Pascal (*.pas)|*.pas",
                Title = "Seleccionar archivo Pascal"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);

                if (richTextBox1 != null)
                {
                    richTextBox1.Text = fileContent;
                }
                else
                {
                    MessageBox.Show("El RichTextBox no está inicializado.");
                    return;
                }

                UpdateStatus(fileContent); //penepenefsdfdsfdsfds
            }
        }

        /*private void UpdateStatus(string content)
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
        }*/

        private void UpdateStatus(string content)
        {
            if (wordCountLabel == null || lineCountLabel == null)
            {
                MessageBox.Show("Las etiquetas del StatusStrip no están inicializadas.");
                return;
            }

            // Expresión regular para identificar palabras (maneja palabras separadas por espacios, signos, etc.)
            int wordCount = Regex.Matches(content, @"\b\w+\b").Count;
            int lineCount = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

            wordCountLabel.Text = $"Palabras: {wordCount}";
            lineCountLabel.Text = $"Líneas: {lineCount}";
        }

        private void sadasdasdToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
