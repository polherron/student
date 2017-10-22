using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApplication2
{

    public partial class frmStudent : Form
    {
        string filePath = string.Empty;
        string studentXmlFile = string.Empty;

        public frmStudent()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (validateForm())
                    {
                        XElement xelement = XElement.Load(openFileDialog1.FileName);
                        //IEnumerable<XElement> student = xelement.Elements();

                        int studentIdMatch = (from idNo in xelement.Elements("Student")
                                              where (string)idNo.Element("studentID") == txtSNumber.Text
                                              select idNo.Element("studentID")).Count();
                        if (studentIdMatch != 0)
                        {
                            MessageBox.Show("Error: A student with this student ID already exists on file");
                        }
                        else
                        {

                            xelement.Add(new XElement("Student",
                                new XElement("FName", txtFname.Text),
                                new XElement("SName", txtSname.Text),
                                new XElement("studentID", txtSNumber.Text),
                                new XElement("DOB", txtDOB.Text)));
                            xelement.Save(openFileDialog1.FileName);
                            MessageBox.Show(String.Format("Student {0} {1} has been added", txtFname.Text, txtSname.Text), "Student Details Entered");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message, "Invalid File Selection");
                }
            }
        }

        private bool validateForm()
        {
            return (validateFName() && validateSName() && validateDate() && validateMod11());
        }

        private bool validateFName()
        {
            bool fNameOk = (txtFname.Text.Length >= 0 && txtFname.Text.Length <= 20);
            if (!fNameOk)
            {
                MessageBox.Show("First name must be bewtween 1 and 20 characters", "Name Error");
            }
            return fNameOk;
        }

        private bool validateSName()
        {
            bool SNameOk = (txtSname.Text.Length > 0 && txtSname.Text.Length < 20);
            if (!SNameOk)
            {
                MessageBox.Show("Last name must be bewtween 1 and 20 characters", "Name Error");
            }
            return SNameOk;
        }

        private bool validateDate()
        {
            DateTime dt;
            if (!DateTime.TryParse(txtDOB.Text, out dt))
            {             
                MessageBox.Show("Date must be in the format dd/mm/yyyy", "Date Error");
            }
            return (DateTime.TryParse(txtDOB.Text, out dt));
        }

        private bool validateMod11()
        {
            int total = 0;
            int result = 0;


            for (int i = 0; i < txtSNumber.Text.Length; i++)
            {
                if (!int.TryParse(txtSNumber.Text.ElementAt(i).ToString(),out result))
                {
                    MessageBox.Show("Invalid Student ID", "Student ID Error");
                    return false;
                }
                total += result * (6-i);
            }
            if (total % 11 != 0)
            {
                MessageBox.Show("Invalid Student ID", "Student ID Error");
                return false;
            }

            return true;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}

