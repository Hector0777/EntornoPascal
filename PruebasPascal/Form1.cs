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
            //richTextBox1.Font = new Font("Consolas", 10); // Fuente monoespaciada
            //richTextBox1.BorderStyle = BorderStyle.FixedSingle; // Borde para mejor visibilidad

            //richTextBox1.TextChanged += (s, e) => richTextBox1.Invalidate(); // Redibujar al cambiar texto
            //richTextBox1.Paint += DibujarEstructura; // Dibujar indentación dentro del RichTextBox

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
        //private void DibujarEstructura(object sender, PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //    Pen pen = new Pen(Color.DarkGreen, 1) { DashPattern = new float[] { 3, 3 } }; // Línea punteada verde oscuro

        //    int lineHeight = TextRenderer.MeasureText("X", richTextBox1.Font).Height; // Altura de línea
        //    int indentWidth = 12; // Espaciado entre líneas de indentación
        //    int xStart = 8; // Margen izquierdo para indentación
        //    int y = 5; // Posición inicial en Y

        //    for (int i = 0; i < richTextBox1.Lines.Length; i++)
        //    {
        //        string line = richTextBox1.Lines[i];
        //        int indentLevel = ContarIndentacion(line);

        //        // Dibujar líneas de indentación dentro del RichTextBox
        //        for (int j = 0; j < indentLevel; j++)
        //        {
        //            g.DrawLine(pen, xStart + j * indentWidth, y, xStart + j * indentWidth, y + lineHeight);
        //        }

        //        y += lineHeight; // Moverse a la siguiente línea
        //    }
        //}

        //private int ContarIndentacion(string line)
        //{
        //    int count = 0;
        //    foreach (char c in line)
        //    {
        //        if (c == ' ') count++; // Cuenta espacios
        //        else if (c == '\t') count += 4; // Cuenta tabulaciones como 4 espacios
        //        else break; // Detiene cuando encuentra un carácter que no es espacio
        //    }
        //    return count / 4; // Cada 4 espacios equivalen a 1 nivel de indentación
        //}

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            HighlightSyntax();
        }
    }
}
