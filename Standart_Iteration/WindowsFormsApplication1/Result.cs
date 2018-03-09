using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;

namespace WindowsFormsApplication1
{
    public partial class Result : Form
    {

        public Result(int ch)
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 1;
            dataGridView2.ColumnCount = 1;
            dataGridView1.RowCount = Iteration.Count_x;
            dataGridView2.RowCount = Iteration.Count_x;
            Initial = new int[Iteration.Count_x];
                Iteration.MassX.CopyTo(Initial, 0);
                for (int i = 0; i < Iteration.Count_x; i++)
                {
                      dataGridView1[0,i].Value = Iteration.MassX[i];
                }
            if (ch == 0) JacobiM();
            else if (ch == 1) SeidelM();

        }
        static int[] Initial;
        static int[] Second;
       void JacobiM()
        {
            //if (!Initial.SequenceEqual(Iteration.MassX))
            //{
               Jacobi.Iteration(Iteration.MassX);
                dataGridView1.ColumnCount += 1;
                for (int i = 0; i < Iteration.Count_x; i++)
                {
                    dataGridView1[1, i].Value = Iteration.MassX[i];

                }
                Second = new int[Iteration.Count_x];
                Iteration.MassX.CopyTo(Second, 0);

                do
                {
                    int n;
                    Jacobi.Iteration(Iteration.MassX);
                    dataGridView1.ColumnCount += 1;
                    n = dataGridView1.ColumnCount;
                    for (int i = 0; i < Iteration.Count_x; i++)
                    {
                        dataGridView1[n - 1, i].Value = Iteration.MassX[i];

                    }
                } while (!Second.SequenceEqual(Iteration.MassX));

           //} 
        }

        void SeidelM()
       {

       //    if (!Initial.SequenceEqual(Iteration.MassX))
         //  {
               Seidel.Iteration(Iteration.MassX);
               dataGridView1.ColumnCount += 1;
               for (int i = 0; i < Iteration.Count_x; i++)
               {
                   dataGridView1[1, i].Value = Iteration.MassX[i];

               }
               Second = new int[Iteration.Count_x];
               Iteration.MassX.CopyTo(Second, 0);

               do
               {
                   int n;
                   Seidel.Iteration(Iteration.MassX);
                   dataGridView1.ColumnCount += 1;
                   n = dataGridView1.ColumnCount;
                   for (int i = 0; i < Iteration.Count_x; i++)
                   {
                       dataGridView1[n - 1, i].Value = Iteration.MassX[i];

                   }
               } while (!Second.SequenceEqual(Iteration.MassX));

          //0  } 
 
       }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = "";
                uint newuInt;

                // Don't try to validate the 'new row' until finished 
                // editing since there
                // is not any point in validating its initial value.
                //if (dataGridView1.Rows[e.RowIndex].IsNewRow) { return; }
                if (e.FormattedValue.Equals(""))
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].ErrorText = "Значение не введено!";
                }
                else
                    if (!uint.TryParse(e.FormattedValue.ToString(), out newuInt))
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].ErrorText = "Слишком большое число!";

                        if (e.ColumnIndex < Iteration.Count_x && e.RowIndex < Iteration.Count_x - 1)
                        {
                            e.Cancel = true;
                            dataGridView1[e.ColumnIndex, e.RowIndex].ErrorText = "Значение должно быть целым положительным числом!";
                        }
                    }
                    else if (newuInt >= Iteration.Galua)
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].ErrorText = "Число должно быть меньше значения характеристики поля!";
                    }
                    else
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].ErrorText = null;
                    }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for(int i=0; i< Iteration.Count_x;i++)
            {
                Iteration.MassX[i] = (int)dataGridView2[0, i].Value;
            }
            if (radioButton1.Checked) JacobiM();
            else if (radioButton2.Checked) SeidelM();
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //  e.CellStyle.BackColor = Color.Aquamarine;  //Смена фона выборанной ячейки(просто так)

            TextBox tb = (TextBox)e.Control;
            tb.KeyPress -= tb_KeyPress;
            tb.KeyPress += new KeyPressEventHandler(this.tb_KeyPress);
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {

            string vlCell = ((TextBox)sender).Text;

            // проверка ввода суммы
            if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].IsInEditMode == true)


                if (((e.KeyChar < '0') || (e.KeyChar > '9')))
                {
                    if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

                    e.Handled = true;
                };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
