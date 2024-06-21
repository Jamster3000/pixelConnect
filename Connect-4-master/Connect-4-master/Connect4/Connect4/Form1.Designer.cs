namespace Connect4
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.blankBoardPanel = new System.Windows.Forms.Panel();
            this.lblTurn = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.hoverCounterPanel = new System.Windows.Forms.Panel();
            this.mousePositionTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // blankBoardPanel
            // 
            this.blankBoardPanel.BackColor = System.Drawing.Color.DarkBlue;
            this.blankBoardPanel.Location = new System.Drawing.Point(172, 241);
            this.blankBoardPanel.Margin = new System.Windows.Forms.Padding(4);
            this.blankBoardPanel.Name = "blankBoardPanel";
            this.blankBoardPanel.Size = new System.Drawing.Size(700, 560);
            this.blankBoardPanel.TabIndex = 0;
            this.blankBoardPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.blankBoardPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // lblTurn
            // 
            this.lblTurn.AutoSize = true;
            this.lblTurn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(231)))), ((int)(((byte)(0)))));
            this.lblTurn.Location = new System.Drawing.Point(248, 9);
            this.lblTurn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(571, 93);
            this.lblTurn.TabIndex = 2;
            this.lblTurn.Text = "Player 1\'s Turn";
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Orange;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.Black;
            this.btnReset.Location = new System.Drawing.Point(13, 721);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(151, 77);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnReset_MouseClick);
            // 
            // hoverCounterPanel
            // 
            this.hoverCounterPanel.Location = new System.Drawing.Point(172, 125);
            this.hoverCounterPanel.Name = "hoverCounterPanel";
            this.hoverCounterPanel.Size = new System.Drawing.Size(700, 158);
            this.hoverCounterPanel.TabIndex = 4;
            // 
            // mousePositionTimer
            // 
            this.mousePositionTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1120, 809);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.blankBoardPanel);
            this.Controls.Add(this.hoverCounterPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Pixel Connect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel blankBoardPanel;
        private System.Windows.Forms.Label lblTurn;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel hoverCounterPanel;
        private System.Windows.Forms.Timer mousePositionTimer;
    }
}

