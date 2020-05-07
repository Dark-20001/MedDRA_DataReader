using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace WindowsFormsApplication1
{
    public partial class DBViewer : Form
    {
        public DBViewer()
        {
            InitializeComponent();
        }

        public DataSet ds;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public DataSet ReadData(string datafile)
        {
            Stream _Stream = File.Open(datafile, FileMode.Open);
            _Stream.Position = 0;
            IFormatter formatter = new BinaryFormatter();

            DataSet ds;

            try
            {
                ds = (DataSet)formatter.Deserialize(_Stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ds = null;
            }

            _Stream.Flush();
            _Stream.Close();
            _Stream.Dispose();

            return ds;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = (Int32)numericUpDown1.Value;
            DataTable t = ds.Tables[i];
            dataGridView1.DataSource = t;

            label2.Text = "Row Count: "+t.Rows.Count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ds = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                string cnFile = ofd.FileName;
                ds = ReadData(cnFile);
            }
        }

        private void DBViewer_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }
    }
}
