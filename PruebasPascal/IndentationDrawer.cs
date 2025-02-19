using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebasPascal
{
    internal class IndentationDrawer
    {
        private RichTextBox editor;
        private List<int> indentPositions = new List<int>();

        public IndentationDrawer(RichTextBox rtb)
        {
            this.editor = rtb;
            this.editor.Paint += Editor_Paint;
            this.editor.TextChanged += Editor_TextChanged;
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            CalcularIndentaciones();
            editor.Invalidate(); // Forzar redibujado
        }

        private void Editor_Paint(object sender, PaintEventArgs e)
        {
            DibujarLineasIndentacion(e.Graphics);
        }

        private void CalcularIndentaciones()
        {
            indentPositions.Clear();
            int nivelIndentacion = 0;
            string[] lineas = editor.Text.Split('\n');

            for (int i = 0; i < lineas.Length; i++)
            {
                string linea = lineas[i].Trim();

                if (Regex.IsMatch(linea, @"\b(begin|case|repeat|while|if|for)\b", RegexOptions.IgnoreCase))
                {
                    indentPositions.Add(i);
                    nivelIndentacion++;
                }

                if (Regex.IsMatch(linea, @"\b(end|until)\b", RegexOptions.IgnoreCase))
                {
                    nivelIndentacion--;
                    indentPositions.Add(i);
                }
            }
        }

        private void DibujarLineasIndentacion(Graphics g)
        {
            int leftMargin = 10;
            int lineSpacing = (int)editor.Font.GetHeight();

            foreach (int linea in indentPositions)
            {
                int y = linea * lineSpacing;
                g.DrawLine(Pens.LightGray, leftMargin, y, leftMargin, y + lineSpacing);
            }
        }
    }
}
