using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary1;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dataGridView1.RowCount = 1;
            dataGridView1.ColumnCount = 2;
            button1.Visible = true;
            Iteration.Count_x = 1;
            string s;
            for (int i = 0; i < Iteration.Count_x; i++)
            {
                s = String.Format("x{0}", i);
          
                dataGridView1.Columns[i].HeaderText = s;
            }
            dataGridView1.Columns[Iteration.Count_x].HeaderText = "b";
            int a = 1;
           for (int i = 0; i < 100; i++)
            {
                a = Test_matrix.NextSimp(a);
                    domainUpDown1.Items.Insert(0,a); 
            }
           domainUpDown1.SelectedIndex = domainUpDown1.Items.Count - 1;
          button1.Visible = true;
          dataGridView2.Visible = true;

        }



        #region Изменение количества переменных => изменение размера матрицы
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int n = (int)numericUpDown2.Value;
            if (n > 1000)
            {
                MessageBox.Show("Слишком большое значение!");
            }
            else
            {
                Iteration.Count_x = n;
                dataGridView1.RowCount = (int)Iteration.Count_x;
                dataGridView1.ColumnCount = (int)Iteration.Count_x + 1;
                string s;
                for (int i = 0; i < Iteration.Count_x; i++)
                {
                    s = String.Format("x{0}", i + 1);
                    dataGridView1.Columns[i].HeaderText = s;
                    s = String.Format("{0}", i + 1);
                    dataGridView1.Rows[i].HeaderCell.Value = s;

                }
                dataGridView1.Columns[Iteration.Count_x].HeaderText = "b";
            }
        }
        #endregion

        #region  проверка корктности ввода данных в поле
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
                    if (!uint.TryParse(e.FormattedValue.ToString(),  out newuInt))
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].ErrorText = "Слишком большое число!";
                   
                    if  ( e.ColumnIndex < Iteration.Count_x && e.RowIndex < Iteration.Count_x - 1)
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

        #endregion

        #region  метод проверка - введены ли все значения
        bool Val()
        {
            bool end = true;

            for (int i = 0; i < Iteration.Count_x + 1; i++)
            {
                for (int j = 0; j < Iteration.Count_x ; j++)
                {
                    try
                    {
                        if ((dataGridView1[i, j].Value == null) || (dataGridView1[i, j].Value.ToString() == "") )
                        {
                            end = false;
                            MessageBox.Show("Заполнены не все ячейки!");
                            dataGridView1[i, j].ErrorText = "Введите значение!";
                            break;
                        } else
                        if ((dataGridView1[i, j].ErrorText != null) && (dataGridView1[i, j].ErrorText != ""))
                        {
                            end = false;
                            MessageBox.Show(dataGridView1[i, j].ErrorText);
                            break;
                        } 
                    }
                    catch
                    {
                        end = false;
                    }
                }
                if (!end) break;
            }
            return end;
        }
        #endregion

        #region Изменение  значения поля Галуа
        private void domainUpDown1_TextChanged(object sender, EventArgs e)
        {
           try
            {
                int a = int.Parse(domainUpDown1.Text);
                if (a > 10000000) MessageBox.Show("Слишком большое значение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else 
                { 
                Iteration.Galua = a;

                for (int i = 0; i < Iteration.Count_x + 1; i++)
                {
                    for (int j = 0; j < Iteration.Count_x; j++)
                    {
                        try
                        {
                            if ((dataGridView1[i, j].ErrorText != null) && (dataGridView1[i, j].Value.ToString() != "") && (uint.Parse(dataGridView1[i, j].Value.ToString()) < Iteration.Galua))
                            {
                                dataGridView1[i, j].ErrorText = null;
                            }
                            else if (uint.Parse(dataGridView1[i, j].Value.ToString()) >= Iteration.Galua)
                            {
                                dataGridView1[i, j].ErrorText = "Число должно быть меньше значения характеристики поля!";
                            }

                        }
                        catch
                        {

                        }
                    }
                  }
                }
                   }catch (System.OverflowException)
            {
                MessageBox.Show("Слишком большое значение!");
            }catch (System.FormatException)
           {
               MessageBox.Show("Можно вводить только цифры!");
               domainUpDown1.SelectedIndex = domainUpDown1.Items.Count - 1;
           }
        }
        #endregion
        
        #region  Запрет ввода всего, кроме цифр
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
        #endregion

        #region  Нажатие на кномку посчитать
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(domainUpDown1.Text) - 1;
                a = Test_matrix.NextSimp(a);
                domainUpDown1.Text = a.ToString();
                Iteration.Galua = a;
                if (Val())
                {
                    Iteration.coefficients = new int[Iteration.Count_x, Iteration.Count_x + 1];
                    for (int i = 0; i < Iteration.Count_x + 1; i++)
                    {
                        for (int j = 0; j < Iteration.Count_x; j++)
                        {
                            Iteration.coefficients[j, i] = int.Parse(dataGridView1[i, j].Value.ToString());
                        }

                    }
                  int det;
                    det = Test_matrix.CountSolution(Iteration.Count_x, Iteration.coefficients);
                    det = det % Iteration.Galua;
                    if (det == 0 || !Test_matrix.All(Iteration.coefficients)) MessageBox.Show("Система неопределена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        Iteration.Massx();

                        dataGridView2.Visible = true;
                        dataGridView2.ColumnCount = (int)Iteration.Count_x;
                        dataGridView2.RowCount = 1;
                        for (int i = 0; i < Iteration.Count_x; i++) dataGridView2[i, 0].Value = Iteration.MassX[i];
                        string s;
                        for (int i = 0; i < Iteration.Count_x; i++)
                        {
                            s = String.Format("x{0}", i + 1);
                            dataGridView2.Columns[i].HeaderText = s;


                        }
                }
                }
            }catch ( Exception )
            {
                MessageBox.Show("Система не совместна!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           // 

        }
        #endregion

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void заполнитьСлучайнымиЧисламиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random ran = new Random();
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                { 
                    
                    dataGridView1.Rows[j].Cells[i].Value = ran.Next(0, Iteration.Galua); 
                }
            for (int i = 0; i < Iteration.Count_x + 1; i++)
            {
                for (int j = 0; j < Iteration.Count_x; j++)
                {
                    try
                    {
                        if ((dataGridView1[i, j].ErrorText != null) && (dataGridView1[i, j].Value.ToString() != "") && (uint.Parse(dataGridView1[i, j].Value.ToString()) < Iteration.Galua))
                        {
                            dataGridView1[i, j].ErrorText = null;
                        }
                        else if (uint.Parse(dataGridView1[i, j].Value.ToString()) >= Iteration.Galua)
                        {
                            dataGridView1[i, j].ErrorText = "Число должно быть меньше значения характеристики поля!";
                        }

                    }
                    catch
                    {

                    }
                }
            }
 
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Сохранить файл перед выходом?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
            if (DialogResult == DialogResult.Yes)
            {

                try
                {
                    int[,] a = new int[Iteration.Count_x, Iteration.Count_x + 1];

                    for (int i = 0; i < Iteration.Count_x + 1; i++)
                    {
                        for (int j = 0; j < Iteration.Count_x; j++)
                        {
                            a[j, i] = int.Parse(dataGridView1[i, j].Value.ToString());
                        }

                    }
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Файл с матрицей|*.glm";



                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        tsFile = saveFileDialog1.FileName;

                        DataBase.DoSerialization(tsFile, ref a);
                    }
                }
                catch (System.NullReferenceException)
                {
         
                    DialogResult = MessageBox.Show("Заполните всю матрицу!", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (DialogResult == DialogResult.Cancel) Close();
                }
            }
            else if (DialogResult == DialogResult.No) Close();

        }

        public string tsFile;
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           try
            {
                int[,] a = new int[Iteration.Count_x, Iteration.Count_x + 1];

                for (int i = 0; i < Iteration.Count_x + 1; i++)
                {
                    for (int j = 0; j < Iteration.Count_x; j++)
                    {
                        a[j, i] = int.Parse(dataGridView1[i, j].Value.ToString());
                    }

                }
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Файл с матрицей|*.glm";

 

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    tsFile = saveFileDialog1.FileName;

                    DataBase.DoSerialization(tsFile, ref a);
                }
           }
           catch (System.NullReferenceException)
           {
               MessageBox.Show("Заполните всю матрицу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

           } catch(Exception)
           {
               MessageBox.Show("Неверный файл");
            }
        }
    
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Файл с матрицей|*.glm";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    tsFile = openFileDialog1.FileName;

                    DataBase.ReadSerialization(tsFile, out Iteration.coefficients);
                }
                Iteration.Count_x = Iteration.coefficients.GetLength(0);
                numericUpDown2.Value = Iteration.Count_x;
                for (int i = 0; i < Iteration.Count_x + 1; i++)
                {
                    for (int j = 0; j < Iteration.Count_x; j++)
                    {
                        dataGridView1[i, j].Value = Iteration.coefficients[j,i];
                        if ((dataGridView1[i, j].ErrorText != null) && (dataGridView1[i, j].Value.ToString() != "") && (uint.Parse(dataGridView1[i, j].Value.ToString()) < Iteration.Galua))
                        {
                            dataGridView1[i, j].ErrorText = null;
                        }
                        else if (uint.Parse(dataGridView1[i, j].Value.ToString()) >= Iteration.Galua)
                        {
                            dataGridView1[i, j].ErrorText = "Число должно быть меньше значения характеристики поля!";
                        }
                    }
                }

 
            }
            catch (Exception)
            {
                MessageBox.Show("Неверный файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

               
    }

}