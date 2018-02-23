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

namespace MatchMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            // Create FolderBrowserDialog object
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            // Create DialogResult object that reads the user's input from the dialog
            DialogResult diagResult = folderDialog.ShowDialog();

            if (diagResult == DialogResult.OK)
            {
                // Display selected path in read-only textboxes
                tbSelectedDirectory.Text = folderDialog.SelectedPath;
                tbOutputDirectory.Text = folderDialog.SelectedPath;

                // Display number of files found
                tbFilesFound.Text = Directory.GetFiles(folderDialog.SelectedPath).Length.ToString();
            }
        }
    }
}
