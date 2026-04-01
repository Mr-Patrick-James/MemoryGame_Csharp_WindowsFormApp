using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MemoCardsDemo
{
    public partial class Form1 : Form
    {
        public List<int> ImageIds = new List<int>();
        public PictureBox[] picBoxes = new PictureBox[12];

        string firstCard = null;
        string secondCard = null;
        int firstCardId = -1;
        int secondCardId = -1;
        bool gamereset = false;
        bool isPreviewing = false;
        bool isFlippingBack = false;

        int clicks = 0;
        int score = 0;
        int matchCount = 0;
        int timeLeft = 100; // 4 minutes
        int moveLimit = 30;

        Timer gameTimer = new Timer();
        Timer previewTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 12; i++)
            {
                picBoxes[i] = (PictureBox)this.Controls["pictureBox" + i.ToString()];
                picBoxes[i].Image = Properties.Resources.back;
                picBoxes[i].Visible = false;
                picBoxes[i].Tag = "hidden";
                picBoxes[i].Click += CardClick;
            }

            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;

            previewTimer.Interval = 2000;
            previewTimer.Tick += PreviewTimer_Tick;

            labelTime.Text = "Time: 1:40";
            labelMoves.Text = "Moves: 0";
            label1.Text = "Score: 0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            ImageIds.Clear();
            Random rand = new Random();

            while (ImageIds.Count < 12)
            {
                int id = rand.Next(0, 6);
                if (ImageIds.FindAll(i => i == id).Count < 2)
                    ImageIds.Add(id);
            }

            for (int i = 0; i < 12; i++)
            {
                picBoxes[i].Image = GetCardPicture(ImageIds[i]); // Preview
                picBoxes[i].Visible = true;
                picBoxes[i].Tag = "preview";
            }

            firstCard = null;
            secondCard = null;
            gamereset = false;
            isFlippingBack = false;
            score = 0;
            matchCount = 0;
            clicks = 0;
            timeLeft = 100;

            label1.Text = "Score: 0";
            labelMoves.Text = "Moves: 0";
            labelTime.Text = "Time: 1:40";

            gameTimer.Stop();
            isPreviewing = true;
            previewTimer.Start();
        }

        private void PreviewTimer_Tick(object sender, EventArgs e)
        {
            previewTimer.Stop();
            isPreviewing = false;

            for (int i = 0; i < 12; i++)
            {
                picBoxes[i].Image = Properties.Resources.back;
                picBoxes[i].Tag = "hidden";
            }

            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;

            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            labelTime.Text = $"Time: {minutes}:{seconds:D2}";

            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                MessageBox.Show("Time's up! Game Over.");
                EndGame();
            }
        }

        private void CardClick(object sender, EventArgs e)
        {
            if (isPreviewing || !gameTimer.Enabled || isFlippingBack)
                return;

            PictureBox clicked = (PictureBox)sender;
            int id = int.Parse(clicked.Name.Replace("pictureBox", ""));

            if (!clicked.Visible || picBoxes[id].Tag.ToString() == "open")
                return;

            // Prevent double-click on same card
            if (id == firstCardId)
                return;

            clicked.Image = GetCardPicture(ImageIds[id]);
            clicked.Tag = "open";

            if (firstCard == null)
            {
                firstCard = ImageIds[id].ToString();
                firstCardId = id;
            }
            else if (secondCard == null)
            {
                secondCard = ImageIds[id].ToString();
                secondCardId = id;

                clicks++;
                labelMoves.Text = $"Moves: {clicks}";

                if (firstCard == secondCard)
                {
                    picBoxes[firstCardId].Visible = false;
                    picBoxes[secondCardId].Visible = false;

                    score += 100;
                    matchCount++;
                    label1.Text = $"Score: {score}";

                    firstCard = null;
                    secondCard = null;

                    if (matchCount == 6)
                    {
                        gameTimer.Stop();
                        MessageBox.Show($"You won! Final Score: {score} in {clicks} moves.");
                        EndGame();
                    }
                }
                else
                {
                    // Mismatch: flip back after short delay
                    isFlippingBack = true;

                    Timer flipBack = new Timer();
                    flipBack.Interval = 600;
                    flipBack.Tick += (s, ev) =>
                    {
                        flipBack.Stop();
                        if (picBoxes[firstCardId].Visible)
                        {
                            picBoxes[firstCardId].Image = Properties.Resources.back;
                            picBoxes[firstCardId].Tag = "hidden";
                        }
                        if (picBoxes[secondCardId].Visible)
                        {
                            picBoxes[secondCardId].Image = Properties.Resources.back;
                            picBoxes[secondCardId].Tag = "hidden";
                        }

                        firstCard = null;
                        secondCard = null;
                        isFlippingBack = false;
                    };
                    flipBack.Start();
                }

                if (clicks >= moveLimit)
                {
                    MessageBox.Show("You've reached the move limit! Game Over.");
                    EndGame();
                }
            }
        }

        private void EndGame()
        {
            gameTimer.Stop();
            for (int i = 0; i < 12; i++)
                picBoxes[i].Visible = false;
        }

        private Bitmap GetCardPicture(int picId)
        {
            switch (picId)
            {
                case 0: return Properties.Resources.Img0;
                case 1: return Properties.Resources.Img1;
                case 2: return Properties.Resources.Img2;
                case 3: return Properties.Resources.Img3;
                case 4: return Properties.Resources.Img4;
                case 5: return Properties.Resources.Img5;
                default: return Properties.Resources.back;
            }
        }
    }
}
