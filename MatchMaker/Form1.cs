using MatchMaker.bus;
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
