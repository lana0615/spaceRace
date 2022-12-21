using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spaceRace
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(8, 180, 20, 20);
        Rectangle player2 = new Rectangle(8, 180, 20, 20);

        int player1Speed = 10;
        int player2Speed = 10;

        List<Rectangle> obstacles = new List<Rectangle>();
        List<Rectangle> obstacles2 = new List<Rectangle>();

 
        int obstacleWidth = 8;
        int obstacleHeight = 2;
        int obstacleSpeed = 6;
        int obstacleSpeed2 = -6;

        int player1Score = 0;
        int player2Score = 0;

        bool leftDown = false;
        bool rightDown = false;
        bool upDown = false;
        bool dDown = false;
        bool wDown = false;
        bool sDown = false;

        SolidBrush pinkBrush = new SolidBrush(Color.HotPink);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random randGen = new Random();
        int randValue = 0;

        string gameState = "waiting";
        public Form1()
        {
            InitializeComponent();
        }

        public void GameSetup()
        {
            gameState = "running";
            player1ScoreLabel.Text = "0";
            player2ScoreLabel.Text = "0";
            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameLoop.Enabled = true;
            player1Score = 0;
            player2Score = 0;

            player1.X = 200;
            player2.X = 380;
            player1.Y = 335;
            player2.Y = 335;
            obstacles.Clear();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    dDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "player1won" || gameState == "player2won")
                    {
                        GameSetup();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "player1won" || gameState == "player2won")
                    {
                        this.Close();
                    }
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    dDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                player1ScoreLabel.Text = "";
                player2ScoreLabel.Text = "";
                titleLabel.Text = "space Race";
                subtitleLabel.Text = "Press space to start or esc to exit";
            }
            else if (gameState == "running")
            {
                //draw player 1
                e.Graphics.FillRectangle(pinkBrush, player1);

                //draw player 2
                e.Graphics.FillRectangle(pinkBrush, player2);

                //draw obstacles
                for (int i = 0; i < obstacles.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, obstacles[i]);
                }
                for (int i = 0; i < obstacles2.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, obstacles2[i]);
                }
            }
            else if (gameState == "player1won")
            {
                titleLabel.Text = "Player 1 got  three points!! You win!!";
                subtitleLabel.Text = "Press space to start or esc to exit";
            }
            else if (gameState == "player2won")
            {
                titleLabel.Text = "Player 2 got  three points!! You win!!";
                subtitleLabel.Text = "Press space to start or esc to exit";
            }

        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //move player 1 up and down
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= player1Speed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += player1Speed;
            }
            // move player 2 up and down
            if (upDown == true && player2.Y > 0)
            {
                player2.Y -= player2Speed;
            }
            if (dDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += player2Speed;
            }

            // add points when players reach the top
            if (player1.Y <= 0)
            {
                player1Score++;
                player1ScoreLabel.Text = $"{player1Score}";
                player1.X = 200;
                player1.Y = 335;

            }
            if (player2.Y <= 0)
            {
                player2Score++;
                player2ScoreLabel.Text = $"{player2Score}";
                player2.X = 380;
                player2.Y = 335;
            }
            //make balls
            for (int i = 0; i < obstacles.Count; i++)
            {
                int x = obstacles[i].X + obstacleSpeed;
                obstacles[i] = new Rectangle(x, obstacles[i].Y, obstacleWidth, obstacleHeight);
            }
            //make balls opposite side
            for (int i = 0; i < obstacles2.Count; i++)
            {
                int x = obstacles2[i].X + obstacleSpeed2;
                obstacles2[i] = new Rectangle(x, obstacles2[i].Y, obstacleWidth, obstacleHeight);
            }
            // generate random value
            randValue = randGen.Next(1, 101);

            if (randValue < 15)
            {
                obstacles.Add(new Rectangle(0, randGen.Next(0, this.Height - 40), obstacleWidth, obstacleHeight));
            }

            randValue = randGen.Next(1, 101);

            if (randValue < 15)
            {
                obstacles2.Add(new Rectangle(this.Width, randGen.Next(0, this.Height - 40), obstacleWidth, obstacleHeight));
            }
            // remove ball when it reaches the side
            for (int i = 0; i < obstacles.Count; i++)
                {
                    if (obstacles[i].Y >= this.Width)
                    {
                        obstacles.RemoveAt(i);
                    }
                }
            for (int i = 0; i < obstacles2.Count; i++)
            {
                if (obstacles2[i].Y <= 0)
                {
                    obstacles2.RemoveAt(i);
                }
            }

            // collisons 
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (player1.IntersectsWith(obstacles[i]))
                {
                    player1.X = 200;
                    player1.Y = 335;
                }
            }
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (player2.IntersectsWith(obstacles[i]))
                {
                    player2.X = 380;
                    player2.Y = 335;
                }
            }
            // check for collisons
            for (int i = 0; i < obstacles2.Count; i++)
            {
                if (player1.IntersectsWith(obstacles2[i]))
                {
                    player1.X = 200;
                    player1.Y = 335;
                }
            }
            for (int i = 0; i < obstacles2.Count; i++)
            {
                if (player2.IntersectsWith(obstacles2[i]))
                {
                    player2.X = 380;
                    player2.Y = 335;
                }
            }

            if (player1Score == 3)
            {
                gameState = "player1won";
            }

            if (player2Score == 3)
            {
                gameState = "player2won";
            }
            Refresh();
        }
    }
}
