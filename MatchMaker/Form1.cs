using MatchMaker.bus;
using Microsoft.Office.Interop.Excel;
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

        // Create random generator
        Random randomGenerator;

        public Form1()
        {
            InitializeComponent();

            // Initialize global list of Trials
            trialsList = new List<Trial>();

            // Initialize random generator
            randomGenerator = new Random();

            // Welcome message in tbDisplay
            DisplayUpdate("Welcome to MatchMaker!"
                + "\r\n\r\n\r\n" + "Step 1: Select the directory in which you have placed *.txt files"
                + "\r\n\r\n" + "Step 2 (optional): Change the ouput directory, if different from input"
                + "\r\n\r\n" + "Step 3: Enter the number of trials needed to run"
                + "\r\n\r\n" + "Step 4: Press 'Run Analysis' button"
                + "\r\n\r\n\r\n" + "Sample Run:"
                + "\r\n\r\n\t" + "24 Files"
                + "\r\n\t" + "1000 Trials"
                + "\r\n\t" + "Total Time: ~19 mins", false);
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

            // Validate that files directory is selected and number of Trials legally specified
            if (tbSelectedDirectory.Text == String.Empty)
            {
                MessageBox.Show("Please select the directory in which the *.txt files are contained", "Missing Files Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tbTrials.Text == String.Empty)
            {
                MessageBox.Show("Please specify how many trials are to be run in the experiment", "Missing Number Of Trials", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbTrials.Focus();
            }
            else if (!int.TryParse(tbTrials.Text, out int numberOfTrials))
            {
                MessageBox.Show("Please enter a valid integer value for number of trials", "Invalid Number Of Trials Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTrials.Focus();
                tbTrials.SelectAll();
            }
            else
            {
                /*--------------------------------------------------EXPERIMENT START--------------------------------------------------*/
                DisplayUpdate("Experiment Started\r\n\r\n", false);
                
                // Save selected folder's files' paths
                string[] filePaths = Directory.GetFiles(tbSelectedDirectory.Text);

                for (int trial = 0; trial < numberOfTrials; trial++)
                {
                    /*---------------------------------------------------TRIAL START---------------------------------------------------*/
                    // Create a new Trial
                    Trial newTrial = new Trial();

                    // Loop through array of file paths
                    for (int i = 0; i < filePaths.Count(); i++)
                    {
                        /*-----------------------------------------------SUBTRIAL START-----------------------------------------------*/
                        // Create a new SubTrial
                        SubTrial newSubTrial = new SubTrial();

                        List<double> rootNumbersList = new List<double>();

                        // Open StreamReader object for current file
                        using (StreamReader reader = new StreamReader(filePaths[i]))
                        {
                            string line;
                            int count = 0;

                            // Read every line and add each value as a Number object in numbersList
                            while ((line = reader.ReadLine()) != null)
                            {
                                rootNumbersList.Add(Convert.ToDouble(line));
                                count++;
                            }
                        }

                        // Loop through root numbers, to be paired up
                        for (int j = 0; j < rootNumbersList.Count(); j++)
                        {
                            // Generate random number between 0 and the number of files in the user selected folder
                            int randomFile = randomGenerator.Next(0, filePaths.Count());

                            // Ensure the generated number is not the same as the current file's index
                            while (randomFile == i)
                            {
                                randomFile = randomGenerator.Next(0, filePaths.Count());
                            }

                            // Open StreamReader object for random file
                            using (StreamReader reader = new StreamReader(filePaths[randomFile]))
                            {
                                // Generate random index within the number of lines
                                int randomIndex = randomGenerator.Next(0, File.ReadAllLines(filePaths[randomFile]).Count());
                                // Read line at random index
                                string randomLine = File.ReadLines(filePaths[randomFile]).Skip(randomIndex).Take(1).First();

                                // Create Number object for root number
                                double rootNumber = Math.Abs(rootNumbersList[j]);
                                // Create Number object for random number
                                double randomNumber = Math.Abs(Convert.ToDouble(randomLine));

                                // Create Pairing object of root and random numbers
                                Pairing pairing = new Pairing(rootNumber, randomNumber);

                                // Add pairing object to SubTrial object's pairings list
                                newSubTrial.SubTrialPairings.Add(pairing);
                            }
                        }

                        // Calculate Average Percent Similarity in SubTrial
                        newSubTrial.CalculateAverageSimilarity();

                        /*------------------------------------------------SUBTRIAL END------------------------------------------------*/

                        // Save SubTrial in Trial object's list of SubTrials
                        newTrial.SubTrialsList.Add(newSubTrial);
                    }

                    // Calculate Total Average Percent Similarity in Trial
                    newTrial.CalculateTrialAverageSimilarity();

                    // Calculate Trial Standard Deviation
                    newTrial.CalculateTrialStandardDeviation();

                    /*----------------------------------------------------TRIAL END----------------------------------------------------*/

                    // Add Trial to global list of trials
                    trialsList.Add(newTrial);
                }

                /*---------------------------------------------------EXPIREMENT END---------------------------------------------------*/
                DisplayUpdate("Experiment Ended\r\n", true);

                // Randomly select a trial to print all records of
                int random = randomGenerator.Next(0, trialsList.Count);

                // Print (Save) randomly selected trial to Excel file
                PrintTrial(random);

                // Print master Excel file, with each trial's average percent similarity and standard deviation
                PrintMaster();
            }

        }

        // Saves (as .xls) all records of given Trial
        void PrintTrial(int trialNumber)
        {
            DisplayUpdate("\r\nWriting Trial " + trialNumber + " to Excel file ...\r\n\r\n", true);

            // Initialize Excel Application Object
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            // Check if Excel is properly installed on current system
            if (xlApp == null)
            {
                MessageBox.Show("Error Loading Excel Application", "Microsoft Excel must be fully installed on this system in order to use the output function. Please check your installation of Microsoft Office and try again.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string saveDirectory = tbOutputDirectory.Text + "\\Output";

                // Create new Excel WorkBook
                Workbook xlWorkbook = xlApp.Workbooks.Add();

                // Rename "Sheet1" to something more appropriate
                Worksheet trialWorksheet = (Worksheet)xlWorkbook.Worksheets[1];
                trialWorksheet.Name = "Trial " + trialNumber;

                // Write Trial Average Percent Similarity
                trialWorksheet.Cells[1, 1] = "Trial Average Percent Similarity";
                trialWorksheet.Cells[1, 2] = trialsList[trialNumber].TrialAverageSimilarity;

                // Write Trial Standard Deviation on Trial Sheet
                trialWorksheet.Cells[2, 1] = "Trial Standard Deviation";
                trialWorksheet.Cells[2, 2] = trialsList[trialNumber].TrialStandardDeviation;

                // Loop through the SubTrials in Trial object
                for (int i = 0; i < trialsList[trialNumber].SubTrialsList.Count; i++)
                {
                    // Create new Worksheet and name it appropriate to its corresponding SubTrial
                    Worksheet newWorksheet = (Worksheet)xlWorkbook.Worksheets.Add();
                    newWorksheet.Name = "SubTrial " + (i + 1);

                    // Write table headers
                    newWorksheet.Cells[1, 1] = "Root Number";
                    newWorksheet.Cells[1, 2] = "Random Number";
                    newWorksheet.Cells[1, 3] = "Percent Similarity";

                    int row = 2;
                    int col = 1;

                    // Loop through the SubTrial's list of Pairings to write
                    for (int z = 0; z < trialsList[trialNumber].SubTrialsList[i].SubTrialPairings.Count; z++)
                    {
                        col = 1;
                        newWorksheet.Cells[row, col] = trialsList[trialNumber].SubTrialsList[i].SubTrialPairings[z].RootNumber;
                        col++;
                        newWorksheet.Cells[row, col] = trialsList[trialNumber].SubTrialsList[i].SubTrialPairings[z].RandomNumber;
                        col++;
                        newWorksheet.Cells[row, col] = trialsList[trialNumber].SubTrialsList[i].SubTrialPairings[z].SimilarityPercent;
                        row++;
                    }

                    newWorksheet.Cells[2, 5] = "SubTrial Average Similarity";
                    newWorksheet.Cells[2, 6] = trialsList[trialNumber].SubTrialsList[i].SubTrialAverageSimilarity;

                    DisplayUpdate("Completed Writing SubTrial " + (i + 1) + "...\r\n", true);
                }

                // If the Output directory does not exist, create it
                if (!Directory.Exists(saveDirectory))
                {
                    // Create subdirectory for Trial
                    Directory.CreateDirectory(saveDirectory);
                }

                // Save Excel Workbook
                xlWorkbook.SaveAs(saveDirectory + "\\Trial" + trialNumber + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xls", XlFileFormat.xlWorkbookNormal, null, null, null, null, XlSaveAsAccessMode.xlExclusive, null, null, null, null, null);

                // Quit Excel App
                xlApp.Quit();

                DisplayUpdate("Done!", true);
            }
        }

        // Saves (as .xls) master file
        void PrintMaster()
        {
            DisplayUpdate("\r\n\r\nWriting Master File...\r\n\r\n", true);

            // Initialize Excel Application Object
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            // Check if Excel is properly installed on current system
            if (xlApp == null)
            {
                MessageBox.Show("Error Loading Excel Application", "Microsoft Excel must be fully installed on this system in order to use the output function. Please check your installation of Microsoft Office and try again.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int row = 1;
                int col = 1;
                string saveDirectory = tbOutputDirectory.Text + "\\Output";

                // Create new Excel WorkBook
                Workbook xlWorkbook = xlApp.Workbooks.Add();

                // Rename "Sheet1" to something more appropriate
                Worksheet experimentWorksheet = (Worksheet)xlWorkbook.Worksheets[1];
                experimentWorksheet.Name = "Analysis Overview";

                // Write Trial Average Percent Similarity
                experimentWorksheet.Cells[row, col] = "Average Percent Similarity";
                col++;
                // Write Trial Standard Deviation
                experimentWorksheet.Cells[row, col] = "Standard Deviation";
                row++;
                col = 1;

                // Loop through the Trials in global trials list
                for (int i = 0; i < trialsList.Count; i++)
                {
                    experimentWorksheet.Cells[row, col] = trialsList[i].TrialAverageSimilarity;
                    col++;
                    experimentWorksheet.Cells[row, col] = trialsList[i].TrialStandardDeviation;
                    row++;
                    col = 1;
                }

                DisplayUpdate("\r\nCompleted Writing Master File", true);

                if (!Directory.Exists(saveDirectory))
                {
                    // Create subdirectory for Trial
                    Directory.CreateDirectory(saveDirectory);
                }
                // Save Excel Workbook
                xlWorkbook.SaveAs(saveDirectory + "\\Master_Analysis_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xls", XlFileFormat.xlWorkbookNormal, null, null, null, null, XlSaveAsAccessMode.xlExclusive, null, null, null, null, null);
                // Quit Excel App
                xlApp.Quit();

                DisplayUpdate("\r\n\r\nDone!", true);
            }
        }

        void DisplayUpdate(string message, bool append)
        {
            if (append)
            {
                tbDisplay.Text += message;
            }
            else
            {
                tbDisplay.Text = message;
            }
        }
    }
}
