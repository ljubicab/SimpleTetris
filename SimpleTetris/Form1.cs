using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Windows.Forms;
using System.IO;

namespace SimpleTetris
{
    public partial class Form1 : Form
    {
        ScriptEngine pyEngine = null;
        ScriptScope pyScope = null;

        public int Score = 0;
        public int finalScore = 0;
        private Color[] blockColor;
        private Timer timer;
        private int tetrisPiecePosition;
        private bool[] blocksFilled;

        public Form1()
        {
            InitializeComponent();

            pyEngine = Python.CreateEngine();
            pyScope = pyEngine.CreateScope();

            timer = new Timer();
            timer.Interval = 250;
            timer.Enabled = true;
            timer.Tick += new System.EventHandler(TimerTickEevnt);

            blockColor = new Color[2];
            blockColor[0] = Color.LightSkyBlue;
            blockColor[1] = Color.RoyalBlue;

            blocksFilled = new bool[40];

            tetrisPiecePosition = -3; 

            foreach (Label label in BlockLabels)
            {
                label.BackColor = blockColor[0];
            }

            timer.Start();

        }

        public void TimerTickEevnt(object sender, EventArgs e)
        {
            tetrisPiecePosition += 5;

            if (tetrisPiecePosition < 35)
            {
                if (BlockLabels[tetrisPiecePosition].BackColor == blockColor[1])
                {
                    tetrisPiecePosition = 2;
                }
            }

            if (tetrisPiecePosition == 2)
            {
                int amount = 0;
                foreach (Label block in BlockLabels)
                {
                    if (block.BackColor == blockColor[1])
                    {
                        blocksFilled[amount] = true;
                    }
                    amount++;
                }
            }

            if (tetrisPiecePosition > 40)
            {
                tetrisPiecePosition = 2;
            }

            if (tetrisPiecePosition < 40)
            {
                if (BlockLabels[tetrisPiecePosition].BackColor == blockColor[0])
                {
                    BlockLabels[tetrisPiecePosition].BackColor = blockColor[1];
                    if (tetrisPiecePosition > 5)
                    {
                        BlockLabels[tetrisPiecePosition - 5].BackColor = blockColor[0];
                    }
                }
            }

            if (BlockLabels[35].BackColor == blockColor[1] &&
               BlockLabels[36].BackColor == blockColor[1] &&
               BlockLabels[37].BackColor == blockColor[1] &&
               BlockLabels[38].BackColor == blockColor[1] &&
               BlockLabels[39].BackColor == blockColor[1])
            {
                BlockLabels[35].BackColor = blockColor[0];
                BlockLabels[36].BackColor = blockColor[0];
                BlockLabels[37].BackColor = blockColor[0];
                BlockLabels[38].BackColor = blockColor[0];
                BlockLabels[39].BackColor = blockColor[0];

                int amountBlocks = 0;
                tetrisPiecePosition = 2;

                blocksFilled = new bool[40];

                foreach (Label block in BlockLabels)
                {
                    if (block.BackColor == blockColor[1])
                    {
                        blocksFilled[amountBlocks + 5] = true;
                    }

                    amountBlocks++;

                }

                foreach (Label block in BlockLabels)
                {
                    block.BackColor = blockColor[0];
                    Score += 1;
                    finalScore = Score / 40;
                    textBox1.Text = finalScore.ToString();
                }

                int amountBlue = 0;
                foreach (bool blockFilled in blocksFilled)
                {
                    if (blockFilled == true)
                    {
                        BlockLabels[amountBlue].BackColor = blockColor[1];
                    }

                    amountBlue++;

                }

                //"levels"
                if (finalScore > 4)
                {
                    timer.Interval = 200;
                }
                if (finalScore > 9)
                {
                    timer.Interval = 100;
                }
                if (finalScore > 14)
                {
                    timer.Interval = 75;
                }
                if (finalScore > 19)
                {
                    timer.Interval = 50;
                }

            }

            if (BlockLabels[2].BackColor == blockColor[1] && BlockLabels[2 + 5].BackColor == blockColor[1])
            {
                timer.Stop();

                DialogResult quitOrContinue = MessageBox.Show(string.Format("You lost. Final score: {0}. Play again?", finalScore.ToString()), "Game Over", MessageBoxButtons.YesNo);

                if (quitOrContinue == DialogResult.Yes)
                {
                    foreach (Label block in BlockLabels)
                    {
                        block.BackColor = blockColor[0];
                    }

                    blocksFilled = new bool[40];
                    tetrisPiecePosition = -3;
                    textBox1.Text = "0";
                    timer.Interval = 250;
                    Score = 0;
                    timer.Start();
                }
                else
                {
                    Application.Exit();
                }

                string time = DateTime.Now.ToString();
                string path = @"Scores.txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                else if (File.Exists(path))
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        if (finalScore > 5)
                        {
                            tw.WriteLine("You got score {0} at time {1}. Congrats!", finalScore.ToString(), time);
                            tw.Close();
                        }

                        else
                        {
                            tw.WriteLine("You got score {0} at time {1}. Better luck next time.", finalScore.ToString(), time);
                            tw.Close();
                        }

                    }
                }
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                if (tetrisPiecePosition < 39 && blocksFilled[tetrisPiecePosition] == false)
                {
                    if (tetrisPiecePosition != 0 && tetrisPiecePosition != 5 &&
                        tetrisPiecePosition != 10 && tetrisPiecePosition != 15 &&
                        tetrisPiecePosition != 20 && tetrisPiecePosition != 25 &&
                        tetrisPiecePosition != 30 && tetrisPiecePosition != 35 &&
                        BlockLabels[tetrisPiecePosition - 1].BackColor != blockColor[1])
                    {
                        tetrisPiecePosition--;
                        BlockLabels[tetrisPiecePosition].BackColor = blockColor[1];
                        BlockLabels[tetrisPiecePosition + 1].BackColor = blockColor[0];
                        return true;
                    }
                }
            }

            if (keyData == Keys.Right)
            {
                if (tetrisPiecePosition < 39 && blocksFilled[tetrisPiecePosition] == false)
                {
                    if (tetrisPiecePosition != 0 && tetrisPiecePosition != 4 &&
                        tetrisPiecePosition != 9 && tetrisPiecePosition != 14 &&
                        tetrisPiecePosition != 19 && tetrisPiecePosition != 24 &&
                        tetrisPiecePosition != 29 && tetrisPiecePosition != 34 &&
                        BlockLabels[tetrisPiecePosition + 1].BackColor != blockColor[1])
                    {
                        tetrisPiecePosition++;
                        BlockLabels[tetrisPiecePosition].BackColor = blockColor[1];
                        BlockLabels[tetrisPiecePosition - 1].BackColor = blockColor[0];
                        return true;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void loadExtensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            ScriptSource ss = pyEngine.CreateScriptSourceFromFile(openFileDialog1.FileName);
            ss.Execute(pyScope);
            dynamic LoadExtension = pyScope.GetVariable("LoadExtension");
            LoadExtension(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Start();
        }
    }
}
