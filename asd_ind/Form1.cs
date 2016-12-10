using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace asd_ind
{
    public partial class Form1 : Form
    {
        HashTable table;
        string chain;

        public Form1()
        {
            table = new HashTable();
            InitializeComponent();
        }

        private void updateTable()
        {
            if (dataGridView1.Rows.Count > 1)
            {
                DataGridViewRow[] rows = new DataGridViewRow[dataGridView1.Rows.Count];
                dataGridView1.Rows.CopyTo(rows, 0);
                dataGridView1.Rows.Clear();
                string[] row_strings = new string[4];
                Record r;
                foreach (DataGridViewRow row in rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        row_strings[0] = row.Cells[0].Value.ToString();
                        chain = "";
                        r = table.findCallback(row_strings[0], addToStr);
                        chain += "null";
                        row_strings[1] = row.Cells[1].Value.ToString();
                        row_strings[2] = table.hash(r.key).ToString();
                        row_strings[3] = chain;
                        dataGridView1.Rows.Add(row_strings);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Any() && textBox2.Text.Any())
                {
                    Record r = new Record(textBox1.Text, textBox2.Text);
                    table.insert(r);
                    chain = "";
                    table.findCallback(r.key, addToStr);
                    string[] row = new string[4];
                    row[0] = r.key;
                    row[1] = r.value;
                    row[2] = table.hash(r.key).ToString();
                    row[3] = chain + "null";

                    foreach (DataGridViewRow t in dataGridView1.Rows)
                    {
                        if (t.Cells[3].Value != null)
                        {
                            if (chain.Contains(t.Cells[3].Value.ToString().Substring(0, t.Cells[3].Value.ToString().Length - 6)))
                                t.Cells[3].Value = chain + "null";
                        }
                    }
                    dataGridView1.Rows.Add(row);
                    updateTable();
                }
                else
                {
                    MessageBox.Show("Введите ключ и значение!", "Внимание");
                }
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Запись с таким ключом уже есть в таблице!", "Ошибка");
            }
        }

        private void addToStr(string s)
        {
            chain += s + " -> ";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text.Any())
                {
                    string text;
                    chain = "";
                    Record r = table.findCallback(textBox3.Text, addToStr);
                    chain += "null";
                    text = "Удалена запись!\n\n";
                    text += "Ключ: " + r.key + "\n";
                    text += "Значение: " + r.value + "\n";
                    text += "Цепочка переполнения: " + chain;
                    table.remove(textBox3.Text);

                    string newChain = chain.Replace(r.key + " -> ", "");

                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[3].Value != null && row.Cells[0].Value.ToString() == textBox3.Text)
                        { 
                            dataGridView1.Rows.Remove(row);
                            break;
                        }
                    }

                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[3].Value != null && row.Cells[3].Value.ToString().Contains(chain))
                        {
                            if (row.Cells[3].Value != null)
                                row.Cells[3].Value = newChain;
                        }
                    }

                    MessageBox.Show(text, "Результат");
                }
                else
                {
                    MessageBox.Show("Введите ключ!", "Внимание");
                }
            }
            catch(IndexOutOfRangeException exc)
            {
                MessageBox.Show("Записи с таким ключом нет в таблице!", "Ошибка");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text.Any())
                {
                    string text;
                    chain = "";
                    Record r = table.findCallback(textBox3.Text, addToStr);
                    chain += "null";
                    text = "Ключ: " + r.key + "\n";
                    text += "Значение: " + r.value + "\n";
                    text += "Цепочка переполнения: " + chain;
                    MessageBox.Show(text, "Результат");
                }
                else
                {
                    MessageBox.Show("Введите ключ!", "Внимание");
                }
            }
            catch(IndexOutOfRangeException exc)
            {
                MessageBox.Show("Записи с таким ключом нет в таблице!", "Ошибка");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            table.clearTable();
            dataGridView1.Rows.Clear();
        }
    }
}
