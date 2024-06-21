using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Connect4
{
    class Game
    {
        public bool GameStarted = false;

        // Constants for the board dimensions
        private const int BoardWidth = 500;
        private const int BoardHeight = 400;
        private const int CellSize = 75;
        private const int boardYOffset = 0;

        // Booleans for the players
        public bool player1;
        public bool player2;

        // Color for the color of each game piece
        private Color pieceColor;

        // State Enum for the state of each place on the game board
        public enum State { empty = 0, player1 = 1, player2 = 2 };

        // 2D array of states to represent the board
        private State[,] board = new State[7, 6];

        // Array of integers to keep track of the topmost empty row in each column
        private List<int> full = new List<int> { 5, 5, 5, 5, 5, 5, 5 };

        // Constructor for the game
        public Game()
        {
            player1 = true;
            player2 = false;
            pieceColor = Color.FromArgb(255, 215, 0);
            ResetBoard();
        }

        // Constructor for redraw
        public Game(int x, int y, Color pcolor)
        {
            try
            {
                State STATE = State.empty;
                if (pcolor == Color.FromArgb(255, 215, 0))
                {
                    player1 = true;
                    player2 = false;
                    pieceColor = Color.FromArgb(255, 215, 0);
                    STATE = State.player1;
                }
                else if (pcolor == Color.FromArgb(255, 69, 0))
                {
                    player2 = true;
                    player1 = false;
                    pieceColor = Color.FromArgb(255, 69, 0);
                    STATE = State.player2;
                }

                board[x / CellSize, y / CellSize] = STATE;
            } catch (System.IndexOutOfRangeException) {}
        }

        public void startGame()
        {
            GameStarted = true;
        }

        // Method for resetting the board
        public void Reset(Label turnLabel)
        {
            full = new List<int> { 5, 5, 5, 5, 5, 5, 5 };
            player1 = true;
            player2 = false;
            pieceColor = Color.FromArgb(255, 215, 0);
            ResetBoard();
            turnLabel.Text = "Player 1's Turn";
            turnLabel.ForeColor = Color.FromArgb(255, 215, 0);
        }

        private void ResetBoard()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    board[i, j] = State.empty;
                }
            }
        }

        // Method to draw blank game board
        public void drawBoard(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias; // Enable anti-aliasing for smoother curves

            Pen line = new Pen(Color.FromArgb(0, 255, 69, 0), 2);
            SolidBrush myBrush = new SolidBrush(Color.White);

            for (int startY = 0; startY <= BoardWidth; startY += CellSize)
            {
                g.DrawLine(line, startY, boardYOffset, startY, BoardHeight + boardYOffset);
            }

            for (int startX = 0; startX <= BoardHeight; startX += CellSize)
            {
                g.DrawLine(line, 0, startX + boardYOffset, BoardWidth, startX + boardYOffset);
            }

            for (int y = boardYOffset; y < BoardHeight + boardYOffset; y += CellSize)
            {
                for (int x = 0; x <= BoardWidth; x += CellSize)
                {
                    //g.FillEllipse(myBrush, new Rectangle(x, y, CellSize, CellSize));
                    Rectangle cellRect = new Rectangle(x, y, CellSize, CellSize);
                    Draw3DCell(g, cellRect, myBrush);
                }
            }
        }

        private void Draw3DCell(Graphics g, Rectangle rect, Brush brush)
        {
            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(rect, Color.LightGray, Color.Gray, LinearGradientMode.ForwardDiagonal))
            {
                g.FillEllipse(gradientBrush, rect);
            }

            using (Pen highlightPen = new Pen(Color.White, 2))
            {
                g.DrawArc(highlightPen, rect.X, rect.Y, rect.Width, rect.Height, 45, 180);
            }

            using (Pen shadowPen = new Pen(Color.Black, 2))
            {
                g.DrawArc(shadowPen, rect.X, rect.Y, rect.Width, rect.Height, 225, 180);
            }

            using (Pen borderPen = new Pen(Color.DarkGray, 1))
            {
                g.DrawEllipse(borderPen, rect);
            }
        }

        // Method that changes the player's turn and game piece color
        private void PlayerTurn(MouseEventArgs e, Graphics f)
        {
            player1 = !player1;
            player2 = !player2;
            pieceColor = player1 ? Color.FromArgb(255, 215, 0) : Color.FromArgb(255, 69, 0);
        }

        // Method to draw the individual game pieces
        // Draws yellow piece if player 1 and red piece if player 2
        public void drawGamePiece(MouseEventArgs e, Graphics f)
        {
            f.SmoothingMode = SmoothingMode.AntiAlias; // Enable anti-aliasing for smoother curves
            SolidBrush myBrush = new SolidBrush(pieceColor);
            int xlocal = (e.X / CellSize);

            if (full[xlocal] >= 0)
            {
                if (player1 && board[xlocal, full[xlocal]] == State.empty)
                {
                    board[xlocal, full[xlocal]] = State.player1;
                    f.FillEllipse(myBrush, xlocal * CellSize, (full[xlocal] * CellSize) + boardYOffset, CellSize, CellSize);
                    full[xlocal]--;
                    PlayerTurn(e, f);
                }
                else if (player2 && board[xlocal, full[xlocal]] == State.empty)
                {
                    board[xlocal, full[xlocal]] = State.player2;
                    f.FillEllipse(myBrush, xlocal * CellSize, (full[xlocal] * CellSize) + boardYOffset, CellSize, CellSize);
                    full[xlocal]--;
                    PlayerTurn(e, f);
                }
            }
        }


        // Method to redraw the pieces after maximization
        public void redrawGamePiece(Graphics f)
        {
            f.SmoothingMode = SmoothingMode.AntiAlias; // Enable anti-aliasing for smoother curves
            for (int col = 0; col < 7; col++)
            {
                for (int row = 0; row < 6; row++)
                {
                    if (board[col, row] == State.player1)
                    {
                        using (SolidBrush myBrush = new SolidBrush(Color.FromArgb(255, 215, 0)))
                        {
                            f.FillEllipse(myBrush, col * CellSize, row * CellSize, CellSize, CellSize);
                        }
                    }
                    else if (board[col, row] == State.player2)
                    {
                        using (SolidBrush myBrush = new SolidBrush(Color.FromArgb(255, 69, 0)))
                        {
                            f.FillEllipse(myBrush, col * CellSize, row * CellSize, CellSize, CellSize);
                        }
                    }
                }
            }
        }

        // Method to check if there is a winner
        public Color WinningPlayer()
        {
            // Check vertical, horizontal, and diagonal lines for a win
            if (CheckWin(State.player1))
            {
                return Color.FromArgb(255, 69, 0);
            }
            else if (CheckWin(State.player2))
            {
                return Color.FromArgb(255, 215, 0);
            }
            return Color.Empty;
        }

        // Check if a player has won
        private bool CheckWin(State player)
        {
            // Check vertical
            for (int col = 0; col < 7; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (board[col, row] == player && board[col, row + 1] == player && board[col, row + 2] == player && board[col, row + 3] == player)
                    {
                        return true;
                    }
                }
            }

            // Check horizontal
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[col, row] == player && board[col + 1, row] == player && board[col + 2, row] == player && board[col + 3, row] == player)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal (bottom-left to top-right)
            for (int col = 0; col < 4; col++)
            {
                for (int row = 3; row < 6; row++)
                {
                    if (board[col, row] == player && board[col + 1, row - 1] == player && board[col + 2, row - 2] == player && board[col + 3, row - 3] == player)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal (top-left to bottom-right)
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (board[col, row] == player && board[col + 1, row + 1] == player && board[col + 2, row + 2] == player && board[col + 3, row + 3] == player)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
