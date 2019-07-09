using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cookbook7
{
    public partial class Form1 : Form
    {
        double timerTtl = 10.0D;
        private DateTime timeToLive;
        private int cacheValue;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set the label starting value
            lblTimer.Text = $"Timer TTL {timerTtl} sec (Stopped)";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //If timer hits zero, reset it to 5 sec
            if(timerTtl == 0)
            {
                timerTtl = 5;
            }
            //Otherwise make the countdown
            else
            {
                timerTtl -= 1;
            }
            //Display changed label value
            lblTimer.Text = $"Timer TTL {timerTtl} sec (Running)";
        }

        //Simulate a running task
        public async Task<int> GetValue()
        {
            //Delay for 1 sec
            await Task.Delay(1000);

            //Generate a random num and asign it to cacheValue var
            Random r = new Random();
            cacheValue = r.Next();
            //Set the starting time
            timeToLive = DateTime.Now.AddSeconds(timerTtl);
            //Start the timer
            timer1.Start();

            return cacheValue;
        }

        //Check to see if time to live is valid for the current cached value
        public ValueTask<int> LoadReadCache(out bool blnCached)
        {
            //If time to live has expired run the code that returns a Task to get and set the cached value 
            if(timeToLive < DateTime.Now)
            {
                blnCached = false;
                return new ValueTask<int>(GetValue());
            }
            else
            {
                blnCached = true;
                return new ValueTask<int>(cacheValue);
            }
        }

        //Code for the button click uses the out var isCashedValue and sets the text in textbox 
        private async void btnTestAsync_Click(object sender, EventArgs e)
        {
            int iVal = await LoadReadCache(out bool isCachedValue);

            if (isCachedValue)
            {
                txtOutput.Text = $"Cached value {iVal} read";
            }
            else
            {
                txtOutput.Text = $"New value {iVal} read";
            }
        }
    }
}
