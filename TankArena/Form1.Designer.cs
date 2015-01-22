namespace TankArena
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbSimplTank = new System.Windows.Forms.CheckBox();
            this.ViewDetails = new System.Windows.Forms.GroupBox();
            this.chkShowHp = new System.Windows.Forms.CheckBox();
            this.chkShowUI = new System.Windows.Forms.CheckBox();
            this.chkShowAmmo = new System.Windows.Forms.CheckBox();
            this.lvBotsOnArena = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.lvBotsAvailable = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.txtStepDelay = new System.Windows.Forms.TextBox();
            this.txtMaxRounds = new System.Windows.Forms.TextBox();
            this.txtArenaSize = new System.Windows.Forms.TextBox();
            this.btnStartEndGame = new System.Windows.Forms.Button();
            this.btnRemoveBot = new System.Windows.Forms.Button();
            this.btnAddBot = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.imgDrawField = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLogFileName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.progressBarMain = new System.Windows.Forms.ProgressBar();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtBattlesToPlay = new System.Windows.Forms.TextBox();
            this.txtCurrentBattle = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lvwTournament = new System.Windows.Forms.ListView();
            this.colBotName = new System.Windows.Forms.ColumnHeader();
            this.colRoundsCount = new System.Windows.Forms.ColumnHeader();
            this.colWinsCount = new System.Windows.Forms.ColumnHeader();
            this.colDrow = new System.Windows.Forms.ColumnHeader();
            this.colPlace = new System.Windows.Forms.ColumnHeader();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnStartTounament = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.ViewDetails.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgDrawField)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cbSimplTank);
            this.panel2.Controls.Add(this.ViewDetails);
            this.panel2.Controls.Add(this.lvBotsOnArena);
            this.panel2.Controls.Add(this.lvBotsAvailable);
            this.panel2.Controls.Add(this.txtStepDelay);
            this.panel2.Controls.Add(this.txtMaxRounds);
            this.panel2.Controls.Add(this.txtArenaSize);
            this.panel2.Controls.Add(this.btnStartEndGame);
            this.panel2.Controls.Add(this.btnRemoveBot);
            this.panel2.Controls.Add(this.btnAddBot);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(482, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(342, 459);
            this.panel2.TabIndex = 0;
            // 
            // cbSimplTank
            // 
            this.cbSimplTank.AutoSize = true;
            this.cbSimplTank.Location = new System.Drawing.Point(103, 311);
            this.cbSimplTank.Name = "cbSimplTank";
            this.cbSimplTank.Size = new System.Drawing.Size(114, 17);
            this.cbSimplTank.TabIndex = 10;
            this.cbSimplTank.Text = "Show simple tanks";
            this.cbSimplTank.UseVisualStyleBackColor = true;
            this.cbSimplTank.CheckedChanged += new System.EventHandler(this.cbSimplTank_CheckedChanged);
            // 
            // ViewDetails
            // 
            this.ViewDetails.Controls.Add(this.chkShowHp);
            this.ViewDetails.Controls.Add(this.chkShowUI);
            this.ViewDetails.Controls.Add(this.chkShowAmmo);
            this.ViewDetails.Location = new System.Drawing.Point(213, 213);
            this.ViewDetails.Name = "ViewDetails";
            this.ViewDetails.Size = new System.Drawing.Size(89, 92);
            this.ViewDetails.TabIndex = 9;
            this.ViewDetails.TabStop = false;
            this.ViewDetails.Text = "Show:";
            // 
            // chkShowHp
            // 
            this.chkShowHp.AutoSize = true;
            this.chkShowHp.Location = new System.Drawing.Point(15, 44);
            this.chkShowHp.Name = "chkShowHp";
            this.chkShowHp.Size = new System.Drawing.Size(40, 17);
            this.chkShowHp.TabIndex = 1;
            this.chkShowHp.Text = "Hp";
            this.chkShowHp.UseVisualStyleBackColor = true;
            this.chkShowHp.CheckStateChanged += new System.EventHandler(this.ViewHp_CheckStateChanged);
            // 
            // chkShowUI
            // 
            this.chkShowUI.AutoSize = true;
            this.chkShowUI.Checked = true;
            this.chkShowUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUI.Location = new System.Drawing.Point(15, 21);
            this.chkShowUI.Name = "chkShowUI";
            this.chkShowUI.Size = new System.Drawing.Size(37, 17);
            this.chkShowUI.TabIndex = 0;
            this.chkShowUI.Text = "UI";
            this.chkShowUI.UseVisualStyleBackColor = true;
            this.chkShowUI.CheckStateChanged += new System.EventHandler(this.ViewAmmo_CheckStateChanged);
            this.chkShowUI.CheckedChanged += new System.EventHandler(this.chkShowUI_CheckedChanged);
            // 
            // chkShowAmmo
            // 
            this.chkShowAmmo.AutoSize = true;
            this.chkShowAmmo.Location = new System.Drawing.Point(15, 67);
            this.chkShowAmmo.Name = "chkShowAmmo";
            this.chkShowAmmo.Size = new System.Drawing.Size(55, 17);
            this.chkShowAmmo.TabIndex = 2;
            this.chkShowAmmo.Text = "Ammo";
            this.chkShowAmmo.UseVisualStyleBackColor = true;
            this.chkShowAmmo.CheckStateChanged += new System.EventHandler(this.ViewAmmo_CheckStateChanged);
            // 
            // lvBotsOnArena
            // 
            this.lvBotsOnArena.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvBotsOnArena.HideSelection = false;
            this.lvBotsOnArena.Location = new System.Drawing.Point(213, 16);
            this.lvBotsOnArena.Name = "lvBotsOnArena";
            this.lvBotsOnArena.Size = new System.Drawing.Size(124, 191);
            this.lvBotsOnArena.TabIndex = 3;
            this.lvBotsOnArena.UseCompatibleStateImageBehavior = false;
            this.lvBotsOnArena.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 100;
            // 
            // lvBotsAvailable
            // 
            this.lvBotsAvailable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvBotsAvailable.HideSelection = false;
            this.lvBotsAvailable.Location = new System.Drawing.Point(6, 16);
            this.lvBotsAvailable.Name = "lvBotsAvailable";
            this.lvBotsAvailable.Size = new System.Drawing.Size(120, 191);
            this.lvBotsAvailable.TabIndex = 0;
            this.lvBotsAvailable.UseCompatibleStateImageBehavior = false;
            this.lvBotsAvailable.View = System.Windows.Forms.View.Details;
            this.lvBotsAvailable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvBotsAvailable_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 100;
            // 
            // txtStepDelay
            // 
            this.txtStepDelay.Location = new System.Drawing.Point(103, 239);
            this.txtStepDelay.Name = "txtStepDelay";
            this.txtStepDelay.Size = new System.Drawing.Size(66, 20);
            this.txtStepDelay.TabIndex = 5;
            this.txtStepDelay.Text = "50";
            // 
            // txtMaxRounds
            // 
            this.txtMaxRounds.Location = new System.Drawing.Point(103, 265);
            this.txtMaxRounds.Name = "txtMaxRounds";
            this.txtMaxRounds.Size = new System.Drawing.Size(66, 20);
            this.txtMaxRounds.TabIndex = 6;
            this.txtMaxRounds.Text = "200";
            // 
            // txtArenaSize
            // 
            this.txtArenaSize.Location = new System.Drawing.Point(103, 213);
            this.txtArenaSize.Name = "txtArenaSize";
            this.txtArenaSize.Size = new System.Drawing.Size(66, 20);
            this.txtArenaSize.TabIndex = 4;
            this.txtArenaSize.Text = "20,20";
            // 
            // btnStartEndGame
            // 
            this.btnStartEndGame.Location = new System.Drawing.Point(6, 305);
            this.btnStartEndGame.Name = "btnStartEndGame";
            this.btnStartEndGame.Size = new System.Drawing.Size(81, 23);
            this.btnStartEndGame.TabIndex = 7;
            this.btnStartEndGame.Text = "Start game";
            this.btnStartEndGame.UseVisualStyleBackColor = true;
            this.btnStartEndGame.Click += new System.EventHandler(this.btnStartEndGame_Click);
            // 
            // btnRemoveBot
            // 
            this.btnRemoveBot.Location = new System.Drawing.Point(132, 50);
            this.btnRemoveBot.Name = "btnRemoveBot";
            this.btnRemoveBot.Size = new System.Drawing.Size(79, 23);
            this.btnRemoveBot.TabIndex = 2;
            this.btnRemoveBot.Text = "Remove <<";
            this.btnRemoveBot.UseVisualStyleBackColor = true;
            this.btnRemoveBot.Click += new System.EventHandler(this.btnRemoveBot_Click);
            // 
            // btnAddBot
            // 
            this.btnAddBot.Location = new System.Drawing.Point(132, 21);
            this.btnAddBot.Name = "btnAddBot";
            this.btnAddBot.Size = new System.Drawing.Size(79, 23);
            this.btnAddBot.TabIndex = 1;
            this.btnAddBot.Text = "Add >>";
            this.btnAddBot.UseVisualStyleBackColor = true;
            this.btnAddBot.Click += new System.EventHandler(this.btnAddBot_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Arena:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 268);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Max game rounds:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Step delay (ms):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Arena size (x,y):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IBots:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(464, 459);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.imgDrawField);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(456, 433);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Debug / UI";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // imgDrawField
            // 
            this.imgDrawField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.imgDrawField.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgDrawField.BackgroundImage")));
            this.imgDrawField.InitialImage = null;
            this.imgDrawField.Location = new System.Drawing.Point(6, 6);
            this.imgDrawField.Name = "imgDrawField";
            this.imgDrawField.Size = new System.Drawing.Size(444, 421);
            this.imgDrawField.TabIndex = 1;
            this.imgDrawField.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.lvwTournament);
            this.tabPage2.Controls.Add(this.btnExport);
            this.tabPage2.Controls.Add(this.btnClearLogs);
            this.tabPage2.Controls.Add(this.btnStartTounament);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(456, 433);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tournament";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLogFileName);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.progressBarMain);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.txtBattlesToPlay);
            this.groupBox1.Controls.Add(this.txtCurrentBattle);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(6, 247);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 120);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statisticks:";
            // 
            // txtLogFileName
            // 
            this.txtLogFileName.Location = new System.Drawing.Point(118, 15);
            this.txtLogFileName.Name = "txtLogFileName";
            this.txtLogFileName.ReadOnly = true;
            this.txtLogFileName.Size = new System.Drawing.Size(320, 20);
            this.txtLogFileName.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Current log file name:";
            // 
            // progressBarMain
            // 
            this.progressBarMain.Location = new System.Drawing.Point(85, 94);
            this.progressBarMain.Name = "progressBarMain";
            this.progressBarMain.Size = new System.Drawing.Size(353, 16);
            this.progressBarMain.TabIndex = 2;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(351, 68);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(87, 20);
            this.textBox4.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(351, 41);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(87, 20);
            this.textBox2.TabIndex = 3;
            // 
            // txtBattlesToPlay
            // 
            this.txtBattlesToPlay.Location = new System.Drawing.Point(118, 67);
            this.txtBattlesToPlay.Name = "txtBattlesToPlay";
            this.txtBattlesToPlay.ReadOnly = true;
            this.txtBattlesToPlay.Size = new System.Drawing.Size(87, 20);
            this.txtBattlesToPlay.TabIndex = 2;
            // 
            // txtCurrentBattle
            // 
            this.txtCurrentBattle.Location = new System.Drawing.Point(118, 41);
            this.txtCurrentBattle.Name = "txtCurrentBattle";
            this.txtCurrentBattle.ReadOnly = true;
            this.txtCurrentBattle.Size = new System.Drawing.Size(131, 20);
            this.txtCurrentBattle.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Total progress:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(303, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Round:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(225, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Total rounds left to play:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Battles left to play:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Current battle:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(292, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(158, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "* select options on panel right ->";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Toutnament table:";
            // 
            // lvwTournament
            // 
            this.lvwTournament.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBotName,
            this.colRoundsCount,
            this.colWinsCount,
            this.colDrow,
            this.colPlace});
            this.lvwTournament.Location = new System.Drawing.Point(6, 31);
            this.lvwTournament.Name = "lvwTournament";
            this.lvwTournament.Size = new System.Drawing.Size(444, 210);
            this.lvwTournament.TabIndex = 3;
            this.lvwTournament.UseCompatibleStateImageBehavior = false;
            this.lvwTournament.View = System.Windows.Forms.View.Details;
            // 
            // colBotName
            // 
            this.colBotName.Text = "Bot name";
            this.colBotName.Width = 130;
            // 
            // colRoundsCount
            // 
            this.colRoundsCount.Text = "Rounds";
            this.colRoundsCount.Width = 70;
            // 
            // colWinsCount
            // 
            this.colWinsCount.Text = "Wins";
            this.colWinsCount.Width = 70;
            // 
            // colDrow
            // 
            this.colDrow.Text = "Drow games";
            this.colDrow.Width = 80;
            // 
            // colPlace
            // 
            this.colPlace.Text = "Place";
            this.colPlace.Width = 70;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(293, 402);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(133, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export table";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Enabled = false;
            this.btnClearLogs.Location = new System.Drawing.Point(154, 402);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(133, 23);
            this.btnClearLogs.TabIndex = 1;
            this.btnClearLogs.Text = "Clear logs directory";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            // 
            // btnStartTounament
            // 
            this.btnStartTounament.Enabled = false;
            this.btnStartTounament.Location = new System.Drawing.Point(15, 402);
            this.btnStartTounament.Name = "btnStartTounament";
            this.btnStartTounament.Size = new System.Drawing.Size(133, 23);
            this.btnStartTounament.TabIndex = 0;
            this.btnStartTounament.Text = "Start tounament";
            this.btnStartTounament.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 483);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(844, 510);
            this.Name = "Form1";
            this.Text = "Tank Arena";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ViewDetails.ResumeLayout(false);
            this.ViewDetails.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgDrawField)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtStepDelay;
        private System.Windows.Forms.TextBox txtArenaSize;
        private System.Windows.Forms.Button btnRemoveBot;
        private System.Windows.Forms.Button btnAddBot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartEndGame;
        private System.Windows.Forms.ListView lvBotsAvailable;
        private System.Windows.Forms.ListView lvBotsOnArena;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox ViewDetails;
        private System.Windows.Forms.CheckBox chkShowAmmo;
        private System.Windows.Forms.CheckBox chkShowHp;
        private System.Windows.Forms.CheckBox chkShowUI;
        private System.Windows.Forms.TextBox txtMaxRounds;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox imgDrawField;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnStartTounament;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView lvwTournament;
        private System.Windows.Forms.ColumnHeader colBotName;
        private System.Windows.Forms.ColumnHeader colRoundsCount;
        private System.Windows.Forms.ColumnHeader colWinsCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader colDrow;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.ProgressBar progressBarMain;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtCurrentBattle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox txtBattlesToPlay;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLogFileName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ColumnHeader colPlace;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox cbSimplTank;
    }
}

