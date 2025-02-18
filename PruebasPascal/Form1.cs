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


        private void sadasdasdToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void UpdateStatus(string content)
        {
            if (wordCountLabel == null || lineCountLabel == null)
            {
                MessageBox.Show("Las etiquetas del StatusStrip no están inicializadas.");
                return;
            }

            // Expresión regular para identificar palabras (evita contar caracteres sueltos como "_")
            int wordCount = Regex.Matches(content, @"\b\w+\b").Count;
            int lineCount = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

            wordCountLabel.Text = $"Palabras: {wordCount}";
            lineCountLabel.Text = $"Líneas: {lineCount}";

            // Aplicar resaltado de sintaxis
            HighlightSyntax();
        }

        private void HighlightSyntax()
        {
            if (richTextBox1 == null) return;

            // Guardar la posición del cursor para no interferir con la edición
            int selectionStart = richTextBox1.SelectionStart;
            int selectionLength = richTextBox1.SelectionLength;

            // Desactivar la actualización visual para mejorar rendimiento
            richTextBox1.SuspendLayout();

            // Resetear el color de todo el texto
            richTextBox1.SelectAll();
            richTextBox1.SelectionColor = Color.Black;

            // Resaltar comentarios
            HighlightPattern(@"\{.*?\}|\(\*.*?\*\)|//.*", Color.Green); // Comentarios Pascal

            // Resaltar palabras clave de Pascal
            string[] keywords = { "begin", "end", "program", "var", "procedure", "function", "const", "type", "if", "then", "else", "while", "do", "for", "repeat", "until", "case", "of", "record", "array", "set", "uses", "unit", "interface", "implementation", "with", "try", "except", "finally", "class", "private", "public", "protected", "published", "inherited", "override", "virtual", "constructor", "destructor" };
            HighlightPattern(@"\b(" + string.Join("|", keywords) + @")\b", Color.Blue);

            // Restaurar selección y actualizar
            richTextBox1.SelectionStart = selectionStart;
            richTextBox1.SelectionLength = selectionLength;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.ResumeLayout();
        }

        private void HighlightPattern(string pattern, Color color)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = color;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            HighlightSyntax();
        }
    }
}
