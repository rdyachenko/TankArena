using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TankArena.TankBot;
using System.Drawing.Imaging;


namespace TankArena.GameArena
{
    public class GameUI
    {
        private Graphics graphics;
        public Image backgroundImage { get; set; }
        public bool viewHp { get; set; }
        public bool viewAmmo { get; set; }
        public bool viewSimplTanks { get; set; }
        private PictureBox pictureBox;
        private int cols;
        private int rows;
        private List<Position> hpPoints;
        private List<Position> ammoPoints;
        private BufferedGraphics myBuffer;
        private int moveCount;
        private SortedList<int, Bitmap> images;

        delegate void UpdateCallback(List<BotDetailsHandler> botsInGameDetails, List<ShotDescription> collectedShots);


        public void BufferGr()
        {
            if (myBuffer != null) 
            {
                myBuffer.Dispose();
                myBuffer = null;
            }

            BufferedGraphicsContext currentContext;
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(pictureBox.CreateGraphics(), pictureBox.DisplayRectangle);
            graphics = myBuffer.Graphics;
        }

        private void OutGr()
        {
           myBuffer.Render();
           myBuffer.Render(pictureBox.CreateGraphics());

        }

        public GameUI(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            images = new SortedList<int, Bitmap>();
            BufferGr();
        }

        internal void Initialize(int gameFieldWidth, int gameFieldHeight, List<Position> hpPoints, List<Position> ammoPoints)
        {

            this.cols = gameFieldWidth;
            this.rows = gameFieldHeight;
            this.hpPoints = hpPoints;
            this.ammoPoints = ammoPoints;
            moveCount = 0;
        }

        internal void Update(List<BotDetailsHandler> botsInGameDetails, List<ShotDescription> collectedShots)
        {
            if (pictureBox.InvokeRequired)
            {
                UpdateCallback d = new UpdateCallback(Update);
                pictureBox.Invoke(d, new object[] { botsInGameDetails, collectedShots });
            }
            else
            {
                if (graphics != null)
                {
                    moveCount++;
                    float cell_sizeX = (float)(pictureBox.DisplayRectangle.Width - 1) / cols;
                    float cell_sizeY = (float)(pictureBox.DisplayRectangle.Height - 1) / rows;
                    
                    float cell_size = (cell_sizeX < cell_sizeY ? cell_sizeX : cell_sizeY);
                    graphics.Clear(Color.White);
                    DrawGrid(cell_size);
                    foreach (BotDetailsHandler b in botsInGameDetails)
                    {
                        if (viewSimplTanks)
                            DrawBot(b.BotPosition, b.BotDirection, b.HitPoints, b.AmmoCount, cell_size, b.botColor);
                        else
                            DrawBotFromImage(b.BotPosition, b.BotDirection, b.HitPoints, b.AmmoCount, cell_size, b.botColor);
                    }
                    foreach (ShotDescription s in collectedShots)
                    {
                        DrawShot(s, cell_size);
                    }
                    graphics.DrawString(moveCount.ToString(), new Font(FontFamily.GenericSansSerif, (float)10.0), new SolidBrush(Color.Navy), (float)0.0, (float)0.0);
                    foreach (BotDetailsHandler b in botsInGameDetails)
                    {
                        DrawBotInfo(b.BotPosition, b.HitPoints, b.AmmoCount, cell_size);
                    }
                    OutGr();
                }                
            }
        }

        private void DrawShot(ShotDescription s, float cell_size)
        {
            graphics.DrawLine(new Pen(Color.Red, 2),
                (int)(s.FromPosition.X * cell_size + cell_size * 0.5),
                (int)(s.FromPosition.Y * cell_size + cell_size * 0.5),
                (int)(s.ToPosition.X * cell_size + cell_size * 0.5),
                (int)(s.ToPosition.Y * cell_size + cell_size * 0.5));
            graphics.FillEllipse(new SolidBrush(Color.Red),
                (int)(s.ToPosition.X * cell_size + cell_size * 0.5),
                (int)(s.ToPosition.Y * cell_size + cell_size * 0.5),
                (int)(cell_size * 0.25),
                (int)(cell_size * 0.25));
        }


        internal void DisposeUI()
        {
            if (myBuffer != null)
            {
                myBuffer.Dispose();
            }
        }

        private void ReplaseColor(Bitmap img, Color src, Color dest)
        {
            Color curColor = Color.Empty;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    curColor = img.GetPixel(i, j);
                    if (curColor == src)
                        img.SetPixel(i, j, dest);
                }
            }
        }

        private Bitmap GetImage(Color col)
        {
            if (images.ContainsKey(col.ToArgb()))
                return (Bitmap)images[col.ToArgb()].Clone();
            else
            {
                Bitmap img = (Bitmap)Properties.Resources.tank.Clone();
                ReplaseColor(img, Color.FromArgb(255, 157, 56), col);
                images.Add(col.ToArgb(), img);
                return (Bitmap)img.Clone();
            }

            return null;
        }

        private void DrawBotFromImage(TankArena.TankBot.Position position, TankArena.TankBot.Direction direction,
            int hitPoints, int ammoCount, float cell_size, Color botColor)
        {
            if(hitPoints != 0)
            {
                Bitmap img = GetImage(botColor);

//                 if(botColor == Color.Black)
//                     img = (Image)Properties.Resources.t1.Clone();
//                 else if(botColor == Color.Red)
//                     img = (Image)Properties.Resources.t2.Clone();
//                 else if(botColor == Color.Green)
//                     img = (Image)Properties.Resources.t3.Clone();
//                 else if(botColor == Color.Tomato)
//                     img = (Image)Properties.Resources.t4.Clone();
//                 else
//                     img = (Image)Properties.Resources.t5.Clone();

                int w = (int)cell_size;
                int p = (int)cell_size;

                int x = (int)(position.X * cell_size);
                int y = (int)(position.Y * cell_size);

                Rectangle rect = Rectangle.Empty;

                switch(direction)
                {
                    case Direction.Down:
                        rect = new Rectangle((int)(x + cell_size / 2 - p / 2), y, p, w);
                        break;
                    case Direction.Up:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        rect = new Rectangle((int)(x + cell_size / 2 - p / 2), y, p, w);
                        break;
                    case Direction.Right:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        rect = new Rectangle(x, (int)(y + cell_size / 2 - p / 2), w, p);
                        break;
                    case Direction.Left:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        rect = new Rectangle(x, (int)(y + cell_size / 2 - p / 2), w, p);
                        break;
                }

                graphics.DrawImage(img, rect);
            }
        }

        private void DrawBot(TankArena.TankBot.Position position, TankArena.TankBot.Direction direction,
            int hitPoints, int ammoCount, float cell_size, Color botColor)
        {
            if(hitPoints != 0)
            {

                float xEngle = (float)position.X * cell_size + (float)(0.2 * cell_size);
                float yEngle = (float)position.Y * cell_size + (float)(0.2 * cell_size);

                float x = (float)position.X * cell_size + (float)(0.5 * cell_size);
                float y = (float)position.Y * cell_size + (float)(0.5 * cell_size);
                
                Brush b = new SolidBrush(botColor);
                

                if (cell_size > 28)
                {
                    float w = (float)(cell_size * 0.6);
                    float p = (float)(cell_size * 0.2);
                    float shift = (float)(cell_size * 0.05);
                    if (direction == Direction.Up || direction == Direction.Down)
                    {
                        graphics.DrawRectangle(new Pen(botColor), xEngle, yEngle, p, w);
                        graphics.DrawRectangle(new Pen(botColor), xEngle + w - p, yEngle, p, w);
                    }
                    else
                    {
                        graphics.DrawRectangle(new Pen(botColor), xEngle, yEngle, w, p);
                        graphics.DrawRectangle(new Pen(botColor), xEngle, yEngle + w - p, w, p);
                    }
                    graphics.FillRectangle(b, new RectangleF(xEngle + shift, yEngle + shift, (float)(cell_size * 0.5), (float)(cell_size * 0.5)));
                } 
                else
                {
                    graphics.FillRectangle(b, new RectangleF(xEngle, yEngle, (float)(cell_size * 0.6), (float)(cell_size * 0.6)));
                }


                if (TankArena.TankBot.Direction.Up == direction)
                {
                    graphics.DrawLine(new Pen(botColor,2), x, y, x, y - (float)(cell_size * 0.50));
                }
                if (TankArena.TankBot.Direction.Down == direction)
                {
                    graphics.DrawLine(new Pen(botColor,2), x, y, x, y + (float)(cell_size * 0.50));
                }
                if (TankArena.TankBot.Direction.Right == direction)
                {
                    graphics.DrawLine(new Pen(botColor,2), x, y, x + (float)(cell_size * 0.50), y);
                }
                if (TankArena.TankBot.Direction.Left == direction)
                {
                    graphics.DrawLine(new Pen(botColor,2), x, y, x - (float)(cell_size * 0.50), y);
                }
            }
            
        }

        void DrawBotInfo(TankArena.TankBot.Position position, int hitPoints, int ammoCount, float cell_size)
        {
            if (hitPoints != 0)
            {
                string s = "";
                if (viewHp)
                    s = s + Convert.ToString(hitPoints) + "/";
                if (viewAmmo)
                    s = s + Convert.ToString(ammoCount);

                if (s.EndsWith("/"))
                    s = s.Remove(s.Length - 1);

                if (s == "")
                    return;

                float x = (float)position.X * cell_size + (float)(0.7 * cell_size);
                float y = (float)position.Y * cell_size + (float)(0.1 * cell_size);

                
                float strLng = 0;
                if (viewHp)
                    strLng = strLng + 21;
                if (viewAmmo)
                    strLng = strLng + 9;
                
                graphics.FillRectangle(new SolidBrush(Color.Yellow), x, y - 10f, strLng, 10f);
                graphics.DrawString(s, new Font(FontFamily.GenericSansSerif, (float)8), new SolidBrush(Color.Black), x, y - 10f);
            }
            
        }
        void DrawGrid(float cell_size)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            float x = 0;
            float y = 0;

            for (int i = 0; i <= ((cols > rows) ? cols : rows) + 1; i++)
            {
                if (i <= cols)
                    graphics.DrawLine(pen, x, 0, x, cell_size * rows );

                if (i <= rows)
                    graphics.DrawLine(pen, 0, y, cell_size * cols, y);

                x += cell_size;
                y += cell_size;
            }
            foreach(Position p in hpPoints)
            {
                graphics.FillRectangle(new SolidBrush(Color.Red), p.X * cell_size, p.Y * cell_size, cell_size, cell_size);
            }
            foreach (Position p in ammoPoints)
            {
                graphics.FillRectangle(new SolidBrush(Color.BlueViolet), p.X * cell_size, p.Y * cell_size, cell_size, cell_size);
            }

        }

        internal void GameOver(BotDetailsHandler winnerBotDetails)
        {
            //if (myBuffer != null)
            //{
            //    myBuffer.Dispose();
            //}

            pictureBox.BackgroundImage = backgroundImage;
            if (winnerBotDetails == null)
            {
                MessageBox.Show("No winner");
            }
            else
            {
                MessageBox.Show(winnerBotDetails.Bot.GetName() + " - THIS BOT WIN!!!");
            }
        }
    }
}
