﻿using MatchMaker.bus;
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
        // Create global list of Trials
        List<Trial> trialsList;

        public Form1()
        {
            InitializeComponent();

            // Initialize global list of Trials
            trialsList = new List<Trial>();
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            BrowseInputDirectory();
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            BrowseOutputDirectory();
        }

        private void BrowseInputDirectory()
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

        private void BrowseOutputDirectory()
        {
            // Create FolderBrowserDialog object
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            // Create DialogResult object that reads the user's input from the dialog
            DialogResult diagResult = folderDialog.ShowDialog();

            if (diagResult == DialogResult.OK)
            {
                // Display selected path in read-only textbox
                tbOutputDirectory.Text = folderDialog.SelectedPath;
            }
        }

        private void btnRunExperiment_Click(object sender, EventArgs e)
        {
            // Clear global list of Trials
            trialsList.Clear();
        }
    }
}
