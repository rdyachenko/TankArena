using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TankArena.GameArena;

namespace TankArena
{
    public partial class Form1 : Form
    {
        private GameArena.GameUI gameUI;
        private GameArena.GameArena gameArena;
        private int colorCounter = 0;

        #region Helper functionality

        private void PopulateBotsListBox()
        {
            Dictionary<int, string> botsAbailable = gameArena.GetAvailableBots();
            if (botsAbailable != null)
            {
                foreach (int id in botsAbailable.Keys)
                {
                    string name = botsAbailable[id];
                    
                    //lstBotsAvailable.Items.Add(new BotDescription() { ID = id, Name = name });
                    //new BotDescription() { ID = id, Name = name }
                    ListViewItem lastItem = lvBotsAvailable.Items.Add(name);
                    lastItem.ForeColor = GetNextColor();
                }
                
                lvBotsAvailable.Items[0].Selected = true;
            }
        }

        private Color GetNextColor()
        {
            
            switch(colorCounter++)
            {
                case 0:
                    return Color.Black;
                    break;
                case 1:
                    return Color.Red;
                    break;
                case 2:
                    return Color.Green;
                    break;
                case 3:
                    return Color.Tomato;
                    break;
                case 4:
                    return Color.Brown;
                    break;
                case 5:
                    return Color.Magenta;
                    break;
                default:
                    return Color.Black;
                break;
            }
            
        }

        private void AddSelectedBotsToArena()
        {
            gameArena.RemoveAllBotsFromArena();
            Dictionary<int, string> botsAbailable = gameArena.GetAvailableBots();
            foreach (ListViewItem lst in lvBotsOnArena.Items)
            {
                foreach (KeyValuePair<int, string> pair in botsAbailable)
                    if (lst.Text == pair.Value)
                    {
                        gameArena.AddBotToArena(pair.Key);
                        gameArena.AddColor(pair.Value, lst.ForeColor);
                    }
            }
        }

        class BotDescription
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        public Form1()
        {
            InitializeComponent();           
            lvBotsOnArena.HeaderStyle = ColumnHeaderStyle.None;
            lvBotsAvailable.HeaderStyle = ColumnHeaderStyle.None;
        }

        private void ViewHp_CheckStateChanged(Object sender, EventArgs e)
        {
            if (chkShowHp.CheckState == CheckState.Checked)
            {
                gameUI.viewHp = true;
            }
            else
            {
                gameUI.viewHp = false;
            }
        }

        private void ViewAmmo_CheckStateChanged(Object sender, EventArgs e)
        {
            if (chkShowAmmo.CheckState == CheckState.Checked)
            {
                gameUI.viewAmmo = true;
            }
            else
            {
                gameUI.viewAmmo = false;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            ProcessUI();
            gameArena = new GameArena.GameArena(gameUI);
            gameArena.OnGameOver += new TankArena.GameArena.GameArena.OnGameOverHandler(gameArena_OnGameOver);
            PopulateBotsListBox();
        }

        void gameArena_OnGameOver(BotDetailsHandler winnerBotDetails)
        {
            if (btnStartEndGame.InvokeRequired)
            {
                TankArena.GameArena.GameArena.OnGameOverHandler del = new TankArena.GameArena.GameArena.OnGameOverHandler(gameArena_OnGameOver);
                btnStartEndGame.Invoke(del, new object[] { winnerBotDetails });
            }
            else
            {
                btnStartEndGame.Text = "Start game";
                if (chkShowUI.Checked == false)
                {
                    // For game without UI- show messageBox
                    string strMessage = "Game is over\nWinner is: " + ((winnerBotDetails == null) ? "none" : winnerBotDetails.Name);
                    MessageBox.Show(strMessage, "Tank Arena", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if(gameArena.IsGameRunning)
            {
                gameArena.EndGame();
                gameUI.DisposeUI();
            }
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            gameUI.BufferGr();
        }

        private void ProcessUI()
        {
            gameUI = new GameArena.GameUI(imgDrawField);
            gameUI.backgroundImage = (Image)imgDrawField.BackgroundImage.Clone();
        }

        #region Button event handlers

        private void btnAddBot_Click(object sender, EventArgs e)
        {           
            foreach (ListViewItem lst in lvBotsAvailable.SelectedItems)
            {
                lvBotsOnArena.Items.Add((ListViewItem)(lst.Clone()));
            }
        }


        private void btnRemoveBot_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lst in lvBotsOnArena.SelectedItems)
            {
                lvBotsOnArena.Items.Remove(lst);
            }
        }

        private void btnStartEndGame_Click(object sender, EventArgs e)
        {
            if (gameArena.IsGameRunning) 
            {
                btnStartEndGame.Text = "Start game";
                imgDrawField.BackgroundImage = gameUI.backgroundImage;
                gameArena.EndGame();
            }
            else
            {
                btnStartEndGame.Text = "Stop";
                AddSelectedBotsToArena();
                string[] strArenaSize = txtArenaSize.Text.Split(',');
                gameArena.GameFieldWidth = Convert.ToInt32(strArenaSize[0]);
                gameArena.GameFieldHeight = Convert.ToInt32(strArenaSize[1]);
                gameArena.GameMoveDelay = Convert.ToInt32(txtStepDelay.Text);
                gameArena.GameMaxRounds = Convert.ToInt32(txtMaxRounds.Text);
                if (chkShowUI.Checked)
                {
                    imgDrawField.BackgroundImage = null;
                }
                if (!gameArena.StartGame(chkShowUI.Checked))
                {
                    MessageBox.Show("Error while starting game");
                }
            }
        }

        #endregion

        private void lvBotsAvailable_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            foreach (ListViewItem lst in lvBotsAvailable.SelectedItems)
            {
                lvBotsOnArena.Items.Add((ListViewItem)(lst.Clone()));
            }
        }

        private void chkShowUI_CheckedChanged(object sender, EventArgs e)
        {
            chkShowHp.Enabled = chkShowUI.Checked;
            chkShowAmmo.Enabled = chkShowUI.Checked;
        }

        private void cbSimplTank_CheckedChanged(object sender, EventArgs e)
        {
            gameUI.viewSimplTanks = cbSimplTank.Checked;
        }
    }
}
