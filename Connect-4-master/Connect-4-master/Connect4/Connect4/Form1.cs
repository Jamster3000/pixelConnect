using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace Connect4
{
    public partial class Form1 : Form
    {
        //instance of Game
        Game game1 = new Game();
        //List of Games to save and redraw with
        List<Game> pieces;

        //for knowing if and where to draw the counter
        private Point currentMousePosition = Point.Empty;
        private bool isDrawing = false;
        private const int fixedYCoordinate = 10;
        private bool isCounterVisible = false;
        private Point lastValidPosition = Point.Empty;

        //constructor of the Form
        public Form1()
        {
            InitializeComponent();
            pieces = new List<Game>();

            // Set double buffering for existing panels
            SetDoubleBuffered(hoverCounterPanel);
            SetDoubleBuffered(blankBoardPanel);

            this.MouseMove += Form1_MouseMove;

            //moving the rendered counter when mouse moves
            hoverCounterPanel.MouseMove += Panel2_MouseMove;
            blankBoardPanel.MouseMove += Panel2_MouseMove;

            //handle mouse leave event so the counter stays rendered.
            hoverCounterPanel.MouseLeave += Panel_MouseLeave;
            blankBoardPanel.MouseLeave += Panel_MouseLeave;

            //rendering the counter above the board
            hoverCounterPanel.Paint += Panel2_Paint;

            game1.startGame();
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            isCounterVisible = false;
            hoverCounterPanel.Invalidate(Region);
        }

        private void SetDoubleBuffered(Control control)
        {
            // Set the DoubleBuffered property using reflection
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });

            // Set the OptimizedDoubleBuffer style
            control.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(control, new object[] { ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true });
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // Update current mouse position relative to the form
            currentMousePosition = e.Location;

            hoverCounterPanel.Invalidate();
        }

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            // Update current mouse position relative to panel2
            currentMousePosition = e.Location;

            // Invalidate panel2 to trigger repaint
            hoverCounterPanel.Invalidate();
            lastValidPosition = e.Location; 

            isCounterVisible = true;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Update current mouse position relative to panel2
            currentMousePosition = e.Location;

            // Invalidate panel2 to trigger repaint
            hoverCounterPanel.Invalidate();

            isCounterVisible = true;
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            if (game1.GameStarted) // Check if the game has started
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias; // Enable anti-aliasing for smoother curves

                int radius = 30; // Example radius for the counter
                int diameter = radius * 2;

                // Calculate X-coordinate for the circle based on the last valid position
                int x = lastValidPosition.X - radius;

                // Ensure the counter doesn't touch the left and right borders of panel2
                x = Math.Max(x, 0);
                x = Math.Min(x, hoverCounterPanel.Width - diameter);

                int y = fixedYCoordinate;

                // Draw the counter
                SolidBrush myBrush = game1.player1 ? new SolidBrush(Color.FromArgb(255, 215, 0)) : new SolidBrush(Color.FromArgb(255, 69, 0));
                g.FillEllipse(myBrush, x, y, diameter, diameter);
            }
        }

        //Method to handle painting of the form
        //handles repainting
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            game1.drawBoard(e); // Always draw the game board

            foreach (Game piece in pieces)
            {
                piece.redrawGamePiece(e.Graphics);
            }
        }

        //Method to handle mouse click in the panel
        //handles drawing the pieces, and checking win
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Color pcolor = new Color();
            Game piece = new Game(e.X, e.Y, pcolor);
            using (Graphics f = this.blankBoardPanel.CreateGraphics())
            {
                game1.drawGamePiece(e, f);
                if (game1.player1)
                {
                    lblTurn.ForeColor = Color.FromArgb(255, 215, 0);
                    lblTurn.Text = "Player 1's Turn";
                    pcolor = Color.FromArgb(255, 69, 0);
                    pieces.Add(piece);
                }
                else
                {
                    lblTurn.ForeColor = Color.FromArgb(255, 69, 0);
                    lblTurn.Text = "Player 2's Turn";
                    pcolor = Color.FromArgb(255, 215, 0);
                    pieces.Add(piece);
                }

            }

            if (game1.WinningPlayer() == Color.FromArgb(255, 69, 0))
            {
                MessageBox.Show("Yellow Player Wins", "Yellow Beat Black", MessageBoxButtons.OK);
                game1.Reset(lblTurn);
                blankBoardPanel.Invalidate();
            }
            else if (game1.WinningPlayer() == Color.FromArgb(255, 215, 0))
            {
                MessageBox.Show("Red Player Wins", "Red Beat Red", MessageBoxButtons.OK);
                game1.Reset(lblTurn);
                blankBoardPanel.Invalidate();
            }

        }

        //Method for reset button, resets game when clicked
        private void btnReset_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to reset?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                blankBoardPanel.Invalidate();
                game1.Reset(lblTurn);
                lblTurn.ForeColor = Color.FromArgb(255, 215, 0);
                lblTurn.Text = "Player 1's Turn";
            }


        }

        //Method for saving the game
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter fileWriter;
            DialogResult result;
            string fileName;

            using (SaveFileDialog fileChooser = new SaveFileDialog())
            {
                fileChooser.CheckFileExists = false;
                fileChooser.AddExtension = true;
                fileChooser.DefaultExt = ".txt";
                fileChooser.Filter = "Text Files (*.txt)|*.txt";
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName;
            }

            if (result == DialogResult.OK)
            {
                if (fileName == String.Empty)
                    MessageBox.Show("There is no name for that File.  You need to name your File.", "Give a File Name", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {

                    FileStream OutputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    fileWriter = new StreamWriter(OutputFile);
                    foreach (var piece in pieces)
                        fileWriter.WriteLine(piece.ToString());
                    fileWriter.Close();
                    OutputFile.Close();
                }
            }
        }

        //Method for opeing the game
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            DialogResult Result;
            StreamReader fileReader;

            using (OpenFileDialog fileChooser = new OpenFileDialog())
            {
                Result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName;
            }
            if (fileName == string.Empty)
                MessageBox.Show("You must have a name for the file", "Please insert File Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {

                    pieces = new List<Game>();

                    FileStream input = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    fileReader = new StreamReader(input);
                    string FileLine;

                    do
                    {

                        FileLine = fileReader.ReadLine();

                        if (FileLine != null)
                        {

                            string[] parts = FileLine.Split(',');
                            // Game temporary = new Game(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Color.FromName(parts[3]));                                
                            //pieces.Add(temporary);                                
                        }
                    } while (FileLine != null);

                    blankBoardPanel.Invalidate();
                    input.Close();
                    fileReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "You chose the wrong file. ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
