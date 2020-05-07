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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public DataSet ds =new System.Data.DataSet();
        public DataSet dsSource;
        public DataSet dsTarget;

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
            dsSource.Tables[0].TableName = "S_" + dsSource.Tables[0].TableName;
            dsTarget.Tables[0].TableName = "T_" + dsTarget.Tables[0].TableName;

            ds.Tables.Add(dsSource.Tables[0].Copy());
            ds.Tables.Add(dsTarget.Tables[0].Copy());
                        
            ds.Tables[0].PrimaryKey = new DataColumn[1] { ds.Tables[0].Columns["SOC_CODE"] };
            ds.Tables[1].PrimaryKey = new DataColumn[1] { ds.Tables[1].Columns["SOC_CODE"] };
            ds.Relations.Add("FK_1", ds.Tables[0].Columns["SOC_CODE"], ds.Tables[1].Columns["SOC_CODE"]);

            var query = from s in ds.Tables[0].AsEnumerable()
                    join t in ds.Tables[1].AsEnumerable()
                    on s.Field<Int32>("SOC_CODE") equals t.Field<Int32>("SOC_CODE")
                    select new
                    {
                        SOC_CODE = s.Field<Int32>("SOC_CODE"),
                        SOC_Name_Source = s.Field<string>("SOC_NAME"),
                        SOC_ABBREV = s.Field<string>("SOC_ABBREV"),
                        SOC_Name_Target = t.Field<string>("SOC_NAME")
                    };

            DataTable dtNew = new System.Data.DataTable();
            dtNew.Columns.Add(new DataColumn("SOC_CODE"));
            dtNew.Columns.Add(new DataColumn("SOC_Name_Source"));
            dtNew.Columns.Add(new DataColumn("SOC_ABBREV"));
            dtNew.Columns.Add(new DataColumn("SOC_Name_Target"));
            foreach (var obj in query)
            {
                dtNew.Rows.Add(obj.SOC_CODE, obj.SOC_Name_Source,obj.SOC_ABBREV, obj.SOC_Name_Target);
            }
            //ds.Tables.Add(dtNew);

            dataGridView1.DataSource = dtNew;
            label2.Text = "Row Count: " + dtNew.Rows.Count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dsSource = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                string cnFile = ofd.FileName;
                dsSource = ReadData(cnFile);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dsTarget = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                string enFile = ofd.FileName;
                dsTarget = ReadData(enFile);
            }
        }
    }
}
