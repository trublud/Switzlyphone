

namespace MusicMaker
{
    partial class frmMusicMaker
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tbMusicHistory = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.cbEnableMelody = new System.Windows.Forms.CheckBox();
            this.tbTotalTimeMS = new System.Windows.Forms.TextBox();
            this.cmbTranspose = new System.Windows.Forms.ComboBox();
            this.cbHorn1_Sax = new System.Windows.Forms.CheckBox();
            this.cbHorn2_Trumpet = new System.Windows.Forms.CheckBox();
            this.cbHorn3_TenorSax = new System.Windows.Forms.CheckBox();
            this.cbHorn4_Trombone = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.cbSynth = new System.Windows.Forms.CheckBox();
            this.cbClosedAndOpenHihat = new System.Windows.Forms.CheckBox();
            this.cbTriangleAndTomAndSnare = new System.Windows.Forms.CheckBox();
            this.cbGuitar = new System.Windows.Forms.CheckBox();
            this.cbStrings = new System.Windows.Forms.CheckBox();
            this.cbElectricPiano = new System.Windows.Forms.CheckBox();
            this.cbVocals = new System.Windows.Forms.CheckBox();
            this.camera = new System.Windows.Forms.PictureBox();
            this.handLx = new System.Windows.Forms.Label();
            this.freqLabel = new System.Windows.Forms.Label();
            this.handRx = new System.Windows.Forms.Label();
            this.handLy = new System.Windows.Forms.Label();
            this.handLz = new System.Windows.Forms.Label();
            this.handRy = new System.Windows.Forms.Label();
            this.handRz = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.tbSpeed = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbKickDrum = new System.Windows.Forms.CheckBox();
            this.cbBass = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.takePicButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tbTest = new System.Windows.Forms.TextBox();
            this.lbBodies = new System.Windows.Forms.Label();
            this.lb1x = new System.Windows.Forms.Label();
            this.lbHandDistance = new System.Windows.Forms.Label();
            this.cbSkeleton = new System.Windows.Forms.CheckBox();
            this.cbHands = new System.Windows.Forms.CheckBox();
            this.cbBodies = new System.Windows.Forms.CheckBox();
            this.cbBody = new System.Windows.Forms.CheckBox();
            this.pulseLabel = new System.Windows.Forms.Label();
            this.emotionLabel = new System.Windows.Forms.Label();
            this.emotionStatusLabel = new System.Windows.Forms.Label();
            this.rightHandLabel = new System.Windows.Forms.Label();
            this.leftHandLabel = new System.Windows.Forms.Label();
            this.rightHandStatusLabel = new System.Windows.Forms.Label();
            this.leftHandStausLabel = new System.Windows.Forms.Label();
            this.cbFancy = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.camera)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(2, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Beeps";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(2, 98);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(2, 74);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(144, 20);
            this.button3.TabIndex = 2;
            this.button3.Text = "Start Music";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbMusicHistory
            // 
            this.tbMusicHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMusicHistory.Location = new System.Drawing.Point(73, 395);
            this.tbMusicHistory.Margin = new System.Windows.Forms.Padding(2);
            this.tbMusicHistory.Multiline = true;
            this.tbMusicHistory.Name = "tbMusicHistory";
            this.tbMusicHistory.ReadOnly = true;
            this.tbMusicHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMusicHistory.Size = new System.Drawing.Size(672, 125);
            this.tbMusicHistory.TabIndex = 3;
            this.tbMusicHistory.TextChanged += new System.EventHandler(this.tbMusicHistory_TextChanged);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Location = new System.Drawing.Point(2, 26);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(144, 20);
            this.button4.TabIndex = 4;
            this.button4.Text = "Start Semitones";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cbEnableMelody
            // 
            this.cbEnableMelody.AutoSize = true;
            this.cbEnableMelody.Checked = true;
            this.cbEnableMelody.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableMelody.Location = new System.Drawing.Point(2, 2);
            this.cbEnableMelody.Margin = new System.Windows.Forms.Padding(2);
            this.cbEnableMelody.Name = "cbEnableMelody";
            this.cbEnableMelody.Size = new System.Drawing.Size(99, 17);
            this.cbEnableMelody.TabIndex = 5;
            this.cbEnableMelody.Text = "Enable Melody ";
            this.cbEnableMelody.UseVisualStyleBackColor = true;
            this.cbEnableMelody.CheckedChanged += new System.EventHandler(this.cbEnableHorns_CheckedChanged);
            // 
            // tbTotalTimeMS
            // 
            this.tbTotalTimeMS.Location = new System.Drawing.Point(2, 82);
            this.tbTotalTimeMS.Margin = new System.Windows.Forms.Padding(2);
            this.tbTotalTimeMS.Name = "tbTotalTimeMS";
            this.tbTotalTimeMS.Size = new System.Drawing.Size(61, 20);
            this.tbTotalTimeMS.TabIndex = 6;
            this.tbTotalTimeMS.Text = "333";
            this.tbTotalTimeMS.TextChanged += new System.EventHandler(this.tbTotalTimeMS_TextChanged);
            // 
            // cmbTranspose
            // 
            this.cmbTranspose.FormattingEnabled = true;
            this.cmbTranspose.Location = new System.Drawing.Point(2, 80);
            this.cmbTranspose.Margin = new System.Windows.Forms.Padding(2);
            this.cmbTranspose.Name = "cmbTranspose";
            this.cmbTranspose.Size = new System.Drawing.Size(57, 21);
            this.cmbTranspose.TabIndex = 8;
            // 
            // cbHorn1_Sax
            // 
            this.cbHorn1_Sax.AutoSize = true;
            this.cbHorn1_Sax.Location = new System.Drawing.Point(2, 268);
            this.cbHorn1_Sax.Margin = new System.Windows.Forms.Padding(2);
            this.cbHorn1_Sax.Name = "cbHorn1_Sax";
            this.cbHorn1_Sax.Size = new System.Drawing.Size(85, 16);
            this.cbHorn1_Sax.TabIndex = 10;
            this.cbHorn1_Sax.Text = "Horn 1 - Sax";
            this.cbHorn1_Sax.UseVisualStyleBackColor = true;
            // 
            // cbHorn2_Trumpet
            // 
            this.cbHorn2_Trumpet.AutoSize = true;
            this.cbHorn2_Trumpet.Location = new System.Drawing.Point(2, 288);
            this.cbHorn2_Trumpet.Margin = new System.Windows.Forms.Padding(2);
            this.cbHorn2_Trumpet.Name = "cbHorn2_Trumpet";
            this.cbHorn2_Trumpet.Size = new System.Drawing.Size(106, 16);
            this.cbHorn2_Trumpet.TabIndex = 11;
            this.cbHorn2_Trumpet.Text = "Horn 2 - Trumpet";
            this.cbHorn2_Trumpet.UseVisualStyleBackColor = true;
            // 
            // cbHorn3_TenorSax
            // 
            this.cbHorn3_TenorSax.AutoSize = true;
            this.cbHorn3_TenorSax.Location = new System.Drawing.Point(2, 308);
            this.cbHorn3_TenorSax.Margin = new System.Windows.Forms.Padding(2);
            this.cbHorn3_TenorSax.Name = "cbHorn3_TenorSax";
            this.cbHorn3_TenorSax.Size = new System.Drawing.Size(116, 16);
            this.cbHorn3_TenorSax.TabIndex = 12;
            this.cbHorn3_TenorSax.Text = "Horn 3 - Tenor Sax";
            this.cbHorn3_TenorSax.UseVisualStyleBackColor = true;
            // 
            // cbHorn4_Trombone
            // 
            this.cbHorn4_Trombone.AutoSize = true;
            this.cbHorn4_Trombone.Location = new System.Drawing.Point(2, 328);
            this.cbHorn4_Trombone.Margin = new System.Windows.Forms.Padding(2);
            this.cbHorn4_Trombone.Name = "cbHorn4_Trombone";
            this.cbHorn4_Trombone.Size = new System.Drawing.Size(115, 16);
            this.cbHorn4_Trombone.TabIndex = 13;
            this.cbHorn4_Trombone.Text = "Horn 4 - Trombone";
            this.cbHorn4_Trombone.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.Location = new System.Drawing.Point(2, 50);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(144, 20);
            this.button5.TabIndex = 14;
            this.button5.Text = "Start Scales";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // cbSynth
            // 
            this.cbSynth.AutoSize = true;
            this.cbSynth.Location = new System.Drawing.Point(2, 348);
            this.cbSynth.Margin = new System.Windows.Forms.Padding(2);
            this.cbSynth.Name = "cbSynth";
            this.cbSynth.Size = new System.Drawing.Size(53, 16);
            this.cbSynth.TabIndex = 15;
            this.cbSynth.Text = "Synth";
            this.cbSynth.UseVisualStyleBackColor = true;
            // 
            // cbClosedAndOpenHihat
            // 
            this.cbClosedAndOpenHihat.AutoSize = true;
            this.cbClosedAndOpenHihat.Location = new System.Drawing.Point(2, 188);
            this.cbClosedAndOpenHihat.Margin = new System.Windows.Forms.Padding(2);
            this.cbClosedAndOpenHihat.Name = "cbClosedAndOpenHihat";
            this.cbClosedAndOpenHihat.Size = new System.Drawing.Size(51, 16);
            this.cbClosedAndOpenHihat.TabIndex = 18;
            this.cbClosedAndOpenHihat.Text = "Hihat";
            this.cbClosedAndOpenHihat.UseVisualStyleBackColor = true;
            // 
            // cbTriangleAndTomAndSnare
            // 
            this.cbTriangleAndTomAndSnare.AutoSize = true;
            this.cbTriangleAndTomAndSnare.Location = new System.Drawing.Point(2, 168);
            this.cbTriangleAndTomAndSnare.Margin = new System.Windows.Forms.Padding(2);
            this.cbTriangleAndTomAndSnare.Name = "cbTriangleAndTomAndSnare";
            this.cbTriangleAndTomAndSnare.Size = new System.Drawing.Size(93, 16);
            this.cbTriangleAndTomAndSnare.TabIndex = 19;
            this.cbTriangleAndTomAndSnare.Text = "Tri,Tom,Snare";
            this.cbTriangleAndTomAndSnare.UseVisualStyleBackColor = true;
            // 
            // cbGuitar
            // 
            this.cbGuitar.AutoSize = true;
            this.cbGuitar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGuitar.Location = new System.Drawing.Point(2, 110);
            this.cbGuitar.Margin = new System.Windows.Forms.Padding(2);
            this.cbGuitar.Name = "cbGuitar";
            this.cbGuitar.Size = new System.Drawing.Size(144, 15);
            this.cbGuitar.TabIndex = 20;
            this.cbGuitar.Text = "Guitar";
            this.cbGuitar.UseVisualStyleBackColor = true;
            this.cbGuitar.CheckedChanged += new System.EventHandler(this.cbGuitar_CheckedChanged);
            // 
            // cbStrings
            // 
            this.cbStrings.AutoSize = true;
            this.cbStrings.Location = new System.Drawing.Point(2, 208);
            this.cbStrings.Margin = new System.Windows.Forms.Padding(2);
            this.cbStrings.Name = "cbStrings";
            this.cbStrings.Size = new System.Drawing.Size(58, 17);
            this.cbStrings.TabIndex = 21;
            this.cbStrings.Text = "Strings";
            this.cbStrings.UseVisualStyleBackColor = true;
            // 
            // cbElectricPiano
            // 
            this.cbElectricPiano.AutoSize = true;
            this.cbElectricPiano.Checked = true;
            this.cbElectricPiano.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbElectricPiano.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbElectricPiano.Location = new System.Drawing.Point(2, 129);
            this.cbElectricPiano.Margin = new System.Windows.Forms.Padding(2);
            this.cbElectricPiano.Name = "cbElectricPiano";
            this.cbElectricPiano.Size = new System.Drawing.Size(144, 15);
            this.cbElectricPiano.TabIndex = 22;
            this.cbElectricPiano.Text = "Electric Piano";
            this.cbElectricPiano.UseVisualStyleBackColor = true;
            // 
            // cbVocals
            // 
            this.cbVocals.AutoSize = true;
            this.cbVocals.Location = new System.Drawing.Point(2, 368);
            this.cbVocals.Margin = new System.Windows.Forms.Padding(2);
            this.cbVocals.Name = "cbVocals";
            this.cbVocals.Size = new System.Drawing.Size(58, 17);
            this.cbVocals.TabIndex = 23;
            this.cbVocals.Text = "Vocals";
            this.cbVocals.UseVisualStyleBackColor = true;
            // 
            // camera
            // 
            this.camera.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.camera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camera.Location = new System.Drawing.Point(74, 3);
            this.camera.Name = "camera";
            this.camera.Size = new System.Drawing.Size(670, 387);
            this.camera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.camera.TabIndex = 24;
            this.camera.TabStop = false;
            // 
            // handLx
            // 
            this.handLx.AutoSize = true;
            this.handLx.Location = new System.Drawing.Point(3, 0);
            this.handLx.Name = "handLx";
            this.handLx.Size = new System.Drawing.Size(42, 13);
            this.handLx.TabIndex = 25;
            this.handLx.Text = "handLx";
            this.handLx.Click += new System.EventHandler(this.handLx_Click);
            // 
            // freqLabel
            // 
            this.freqLabel.AutoSize = true;
            this.freqLabel.Location = new System.Drawing.Point(386, 0);
            this.freqLabel.Name = "freqLabel";
            this.freqLabel.Size = new System.Drawing.Size(51, 13);
            this.freqLabel.TabIndex = 26;
            this.freqLabel.Text = "freqLabel";
            // 
            // handRx
            // 
            this.handRx.AutoSize = true;
            this.handRx.Location = new System.Drawing.Point(3, 0);
            this.handRx.Name = "handRx";
            this.handRx.Size = new System.Drawing.Size(44, 13);
            this.handRx.TabIndex = 27;
            this.handRx.Text = "handRx";
            // 
            // handLy
            // 
            this.handLy.AutoSize = true;
            this.handLy.Location = new System.Drawing.Point(3, 30);
            this.handLy.Name = "handLy";
            this.handLy.Size = new System.Drawing.Size(42, 13);
            this.handLy.TabIndex = 28;
            this.handLy.Text = "handLy";
            // 
            // handLz
            // 
            this.handLz.AutoSize = true;
            this.handLz.Location = new System.Drawing.Point(3, 60);
            this.handLz.Name = "handLz";
            this.handLz.Size = new System.Drawing.Size(42, 13);
            this.handLz.TabIndex = 29;
            this.handLz.Text = "handLz";
            // 
            // handRy
            // 
            this.handRy.AutoSize = true;
            this.handRy.Location = new System.Drawing.Point(3, 29);
            this.handRy.Name = "handRy";
            this.handRy.Size = new System.Drawing.Size(44, 13);
            this.handRy.TabIndex = 30;
            this.handRy.Text = "handRy";
            // 
            // handRz
            // 
            this.handRz.AutoSize = true;
            this.handRz.Location = new System.Drawing.Point(3, 58);
            this.handRz.Name = "handRz";
            this.handRz.Size = new System.Drawing.Size(44, 13);
            this.handRz.TabIndex = 31;
            this.handRz.Text = "handRz";
            // 
            // cbMode
            // 
            this.cbMode.Enabled = false;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Color",
            "Depth",
            "ColorDepth",
            "Infrared",
            "LongIR"});
            this.cbMode.Location = new System.Drawing.Point(73, 14);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(78, 21);
            this.cbMode.TabIndex = 32;
            this.cbMode.Text = "Depth";
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new System.Drawing.Point(3, 101);
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(55, 20);
            this.tbSpeed.TabIndex = 33;
            this.tbSpeed.Text = "90";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.cbVocals, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.cbElectricPiano, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbKickDrum, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbTriangleAndTomAndSnare, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbClosedAndOpenHihat, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbStrings, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cbGuitar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbBass, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.cbEnableMelody, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbHorn2_Trumpet, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.cbHorn3_TenorSax, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.cbHorn1_Sax, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.cbSynth, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.cbHorn4_Trombone, 0, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(832, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.44444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.55556F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(148, 387);
            this.tableLayoutPanel1.TabIndex = 34;
            // 
            // cbKickDrum
            // 
            this.cbKickDrum.AutoSize = true;
            this.cbKickDrum.Location = new System.Drawing.Point(2, 148);
            this.cbKickDrum.Margin = new System.Windows.Forms.Padding(2);
            this.cbKickDrum.Name = "cbKickDrum";
            this.cbKickDrum.Size = new System.Drawing.Size(75, 16);
            this.cbKickDrum.TabIndex = 17;
            this.cbKickDrum.Text = "Kick Drum";
            this.cbKickDrum.UseVisualStyleBackColor = true;
            // 
            // cbBass
            // 
            this.cbBass.AutoSize = true;
            this.cbBass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBass.Location = new System.Drawing.Point(2, 229);
            this.cbBass.Margin = new System.Windows.Forms.Padding(2);
            this.cbBass.Name = "cbBass";
            this.cbBass.Size = new System.Drawing.Size(144, 15);
            this.cbBass.TabIndex = 16;
            this.cbBass.Text = "Bass";
            this.cbBass.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.15094F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.84906F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tableLayoutPanel3.Controls.Add(this.takePicButton, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.camera, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbMusicHistory, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbTest, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 81);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.40984F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.59016F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(983, 522);
            this.tableLayoutPanel3.TabIndex = 36;
            this.tableLayoutPanel3.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel3_Paint);
            // 
            // takePicButton
            // 
            this.takePicButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.takePicButton.Location = new System.Drawing.Point(750, 396);
            this.takePicButton.Name = "takePicButton";
            this.takePicButton.Size = new System.Drawing.Size(76, 23);
            this.takePicButton.TabIndex = 57;
            this.takePicButton.Text = "Take Picture";
            this.takePicButton.UseVisualStyleBackColor = true;
            this.takePicButton.Click += new System.EventHandler(this.takePicButton_Click_1);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.button4, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.button5, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.button2, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.button3, 0, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(832, 396);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 5;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.01961F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.98039F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(148, 123);
            this.tableLayoutPanel5.TabIndex = 37;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tbSpeed, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.handRx, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbTranspose, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.handRz, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.handRy, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(750, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(61, 118);
            this.tableLayoutPanel2.TabIndex = 37;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.handLx, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.handLy, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.handLz, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tbTotalTimeMS, 0, 3);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(65, 100);
            this.tableLayoutPanel4.TabIndex = 38;
            // 
            // tbTest
            // 
            this.tbTest.Location = new System.Drawing.Point(3, 396);
            this.tbTest.Name = "tbTest";
            this.tbTest.Size = new System.Drawing.Size(65, 20);
            this.tbTest.TabIndex = 39;
            // 
            // lbBodies
            // 
            this.lbBodies.AutoSize = true;
            this.lbBodies.Location = new System.Drawing.Point(2, 0);
            this.lbBodies.Name = "lbBodies";
            this.lbBodies.Size = new System.Drawing.Size(30, 13);
            this.lbBodies.TabIndex = 39;
            this.lbBodies.Text = "body";
            this.lbBodies.Click += new System.EventHandler(this.lbBodies_Click);
            // 
            // lb1x
            // 
            this.lb1x.AutoSize = true;
            this.lb1x.Location = new System.Drawing.Point(124, 0);
            this.lb1x.Name = "lb1x";
            this.lb1x.Size = new System.Drawing.Size(38, 13);
            this.lb1x.TabIndex = 40;
            this.lb1x.Text = "p1 lh x";
            // 
            // lbHandDistance
            // 
            this.lbHandDistance.AutoSize = true;
            this.lbHandDistance.Location = new System.Drawing.Point(255, 0);
            this.lbHandDistance.Name = "lbHandDistance";
            this.lbHandDistance.Size = new System.Drawing.Size(33, 13);
            this.lbHandDistance.TabIndex = 41;
            this.lbHandDistance.Text = "clap?";
            // 
            // cbSkeleton
            // 
            this.cbSkeleton.AutoSize = true;
            this.cbSkeleton.Location = new System.Drawing.Point(5, 16);
            this.cbSkeleton.Name = "cbSkeleton";
            this.cbSkeleton.Size = new System.Drawing.Size(56, 17);
            this.cbSkeleton.TabIndex = 42;
            this.cbSkeleton.Text = "Kinect";
            this.cbSkeleton.UseVisualStyleBackColor = true;
            this.cbSkeleton.CheckedChanged += new System.EventHandler(this.cbSkeleton_CheckedChanged);
            // 
            // cbHands
            // 
            this.cbHands.AutoSize = true;
            this.cbHands.Location = new System.Drawing.Point(73, 38);
            this.cbHands.Name = "cbHands";
            this.cbHands.Size = new System.Drawing.Size(57, 17);
            this.cbHands.TabIndex = 43;
            this.cbHands.Text = "Hands";
            this.cbHands.UseVisualStyleBackColor = true;
            this.cbHands.CheckedChanged += new System.EventHandler(this.CbHands_CheckedChangedAsync);
            // 
            // cbBodies
            // 
            this.cbBodies.AutoSize = true;
            this.cbBodies.Enabled = false;
            this.cbBodies.Location = new System.Drawing.Point(217, 16);
            this.cbBodies.Name = "cbBodies";
            this.cbBodies.Size = new System.Drawing.Size(58, 17);
            this.cbBodies.TabIndex = 44;
            this.cbBodies.Text = "Bodies";
            this.cbBodies.UseVisualStyleBackColor = true;
            // 
            // cbBody
            // 
            this.cbBody.AutoSize = true;
            this.cbBody.Enabled = false;
            this.cbBody.Location = new System.Drawing.Point(157, 16);
            this.cbBody.Name = "cbBody";
            this.cbBody.Size = new System.Drawing.Size(54, 17);
            this.cbBody.TabIndex = 45;
            this.cbBody.Text = "Music";
            this.cbBody.UseVisualStyleBackColor = true;
            // 
            // pulseLabel
            // 
            this.pulseLabel.AutoSize = true;
            this.pulseLabel.Location = new System.Drawing.Point(317, 5);
            this.pulseLabel.Name = "pulseLabel";
            this.pulseLabel.Size = new System.Drawing.Size(35, 13);
            this.pulseLabel.TabIndex = 46;
            this.pulseLabel.Text = "label1";
            // 
            // emotionLabel
            // 
            this.emotionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emotionLabel.AutoSize = true;
            this.emotionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emotionLabel.Location = new System.Drawing.Point(135, 68);
            this.emotionLabel.Name = "emotionLabel";
            this.emotionLabel.Size = new System.Drawing.Size(45, 13);
            this.emotionLabel.TabIndex = 52;
            this.emotionLabel.Text = "Emotion";
            // 
            // emotionStatusLabel
            // 
            this.emotionStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emotionStatusLabel.AutoSize = true;
            this.emotionStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emotionStatusLabel.Location = new System.Drawing.Point(209, 68);
            this.emotionStatusLabel.Name = "emotionStatusLabel";
            this.emotionStatusLabel.Size = new System.Drawing.Size(69, 13);
            this.emotionStatusLabel.TabIndex = 51;
            this.emotionStatusLabel.Text = "Not Tracking";
            // 
            // rightHandLabel
            // 
            this.rightHandLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightHandLabel.AutoSize = true;
            this.rightHandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightHandLabel.Location = new System.Drawing.Point(532, 65);
            this.rightHandLabel.Name = "rightHandLabel";
            this.rightHandLabel.Size = new System.Drawing.Size(61, 13);
            this.rightHandLabel.TabIndex = 50;
            this.rightHandLabel.Text = "Right Hand";
            // 
            // leftHandLabel
            // 
            this.leftHandLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.leftHandLabel.AutoSize = true;
            this.leftHandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftHandLabel.Location = new System.Drawing.Point(-221, 65);
            this.leftHandLabel.Name = "leftHandLabel";
            this.leftHandLabel.Size = new System.Drawing.Size(54, 13);
            this.leftHandLabel.TabIndex = 49;
            this.leftHandLabel.Text = "Left Hand";
            // 
            // rightHandStatusLabel
            // 
            this.rightHandStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightHandStatusLabel.AutoSize = true;
            this.rightHandStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightHandStatusLabel.Location = new System.Drawing.Point(600, 65);
            this.rightHandStatusLabel.Name = "rightHandStatusLabel";
            this.rightHandStatusLabel.Size = new System.Drawing.Size(69, 13);
            this.rightHandStatusLabel.TabIndex = 48;
            this.rightHandStatusLabel.Text = "Not Tracking";
            // 
            // leftHandStausLabel
            // 
            this.leftHandStausLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.leftHandStausLabel.AutoSize = true;
            this.leftHandStausLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftHandStausLabel.Location = new System.Drawing.Point(-152, 65);
            this.leftHandStausLabel.Name = "leftHandStausLabel";
            this.leftHandStausLabel.Size = new System.Drawing.Size(69, 13);
            this.leftHandStausLabel.TabIndex = 47;
            this.leftHandStausLabel.Text = "Not Tracking";
            this.leftHandStausLabel.Click += new System.EventHandler(this.leftHandStausLabel_Click);
            // 
            // cbFancy
            // 
            this.cbFancy.AutoSize = true;
            this.cbFancy.Location = new System.Drawing.Point(3, 38);
            this.cbFancy.Name = "cbFancy";
            this.cbFancy.Size = new System.Drawing.Size(55, 17);
            this.cbFancy.TabIndex = 60;
            this.cbFancy.Text = "Fancy";
            this.cbFancy.UseVisualStyleBackColor = true;
            this.cbFancy.CheckedChanged += new System.EventHandler(this.cbFancy_CheckedChanged);
            // 
            // frmMusicMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 610);
            this.Controls.Add(this.cbFancy);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.emotionLabel);
            this.Controls.Add(this.emotionStatusLabel);
            this.Controls.Add(this.rightHandLabel);
            this.Controls.Add(this.leftHandLabel);
            this.Controls.Add(this.rightHandStatusLabel);
            this.Controls.Add(this.leftHandStausLabel);
            this.Controls.Add(this.pulseLabel);
            this.Controls.Add(this.cbBody);
            this.Controls.Add(this.cbBodies);
            this.Controls.Add(this.cbHands);
            this.Controls.Add(this.cbSkeleton);
            this.Controls.Add(this.lbHandDistance);
            this.Controls.Add(this.lb1x);
            this.Controls.Add(this.lbBodies);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.freqLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMusicMaker";
            this.Text = "Switlyfone";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.FrmMusicMaker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.camera)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbMusicHistory;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox cbEnableMelody;
        private System.Windows.Forms.TextBox tbTotalTimeMS;
        private System.Windows.Forms.ComboBox cmbTranspose;
        private System.Windows.Forms.CheckBox cbHorn1_Sax;
        private System.Windows.Forms.CheckBox cbHorn2_Trumpet;
        private System.Windows.Forms.CheckBox cbHorn3_TenorSax;
        private System.Windows.Forms.CheckBox cbHorn4_Trombone;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox cbSynth;
        private System.Windows.Forms.CheckBox cbClosedAndOpenHihat;
        private System.Windows.Forms.CheckBox cbTriangleAndTomAndSnare;
        private System.Windows.Forms.CheckBox cbGuitar;
        private System.Windows.Forms.CheckBox cbStrings;
        private System.Windows.Forms.CheckBox cbElectricPiano;
        private System.Windows.Forms.CheckBox cbVocals;
        private System.Windows.Forms.PictureBox camera;
        private System.Windows.Forms.Label handLx;
        private System.Windows.Forms.Label freqLabel;
        private System.Windows.Forms.Label handRx;
        private System.Windows.Forms.Label handLy;
        private System.Windows.Forms.Label handLz;
        private System.Windows.Forms.Label handRy;
        private System.Windows.Forms.Label handRz;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.TextBox tbSpeed;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox cbBass;
        private System.Windows.Forms.CheckBox cbKickDrum;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label lbBodies;
        private System.Windows.Forms.Label lb1x;
        private System.Windows.Forms.Label lbHandDistance;
      static  private System.Windows.Forms.TextBox tbGreeting;
        public System.Windows.Forms.CheckBox cbSkeleton;
        public System.Windows.Forms.TextBox tbTest;
        private System.Windows.Forms.CheckBox cbHands;
        private System.Windows.Forms.CheckBox cbBodies;
        private System.Windows.Forms.CheckBox cbBody;
        private System.Windows.Forms.Label pulseLabel;
        private System.Windows.Forms.Button takePicButton;
        private System.Windows.Forms.Label emotionLabel;
        private System.Windows.Forms.Label emotionStatusLabel;
        private System.Windows.Forms.Label rightHandLabel;
        private System.Windows.Forms.Label leftHandLabel;
        private System.Windows.Forms.Label rightHandStatusLabel;
        private System.Windows.Forms.Label leftHandStausLabel;

        private System.Windows.Forms.CheckBox cbFancy;
    }
}

