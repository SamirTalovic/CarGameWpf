using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();

        ImageBrush playerImage = new ImageBrush();
        ImageBrush starImage = new ImageBrush();

        Rect playerhitbox;

        int speed = 10;
        int carspeed = 10;
        int carNum;
        int starCounter = 30;
        int powerModeCounet = 200;
        int playerSpeed = 10;

        double score;
        double i;

        bool moveLeft,moveRight,gameOver,powerMode;


        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            score += .05;
            starCounter -= 1;
            scoreTxt.Content = "Preziveli ste" + score.ToString() + "sekundi";

            playerhitbox = new Rect(Canvas.GetLeft(player),Canvas.GetTop(player),player.Width,player.Height);
            if(moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }
            if(starCounter < 1)
            {
                MakeStar();
                starCounter = rand.Next(600, 900);
            }
            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "roadMarks")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);

                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }
                }
                

                if ((string)x.Tag == "car")
                {
                    Canvas.SetTop(x,Canvas.GetTop(x) + carspeed);
                    if (Canvas.GetTop(x) > 510)
                    {
                        
                        ChangeCars(x);
                    }

                }

                Rect carHitBox = new Rect(Canvas.GetLeft(x),Canvas.GetTop(x),x.Width,x.Height);


                if(playerhitbox.IntersectsWith(carHitBox) && powerMode == true)
                {
                    ChangeCars(x);
                }
                else if(playerhitbox.IntersectsWith(carHitBox) && powerMode == true && score > 1)
                {
                    gameTimer.Stop();
                    scoreTxt.Content += "Pritinsi enter da bi restartovao";
                    gameOver = true;
                }

                if ((string)x.Tag == "star")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerhitbox.IntersectsWith(starHitBox))
                    {
                        itemRemover.Add(x);
                        powerMode = true;
                        powerModeCounet = 200;
                    }
                    if (Canvas.GetTop(x) > 400)
                    {
                        itemRemover.Add(x);
                    }
                    if (powerMode == true)
                    {
                        PowerUp();
                        if (powerModeCounet < 1)
                            powerMode = false;
                    }
                    else
                    {
                        playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                        MyCanvas.Background = Brushes.Gray;

                    }
                    //foreach (Rectangle y in itemRemover)
                    //{
                    //    MyCanvas.Children.Remove(y);
                    //}

                    if (score > 10 && score < 20)
                    {
                        speed = 12;
                    }
                    if (score > 20 && score < 30)
                    {
                        speed = 14;
                    }
                    if (score > 30 && score < 40)
                    {
                        speed = 16;
                    }
                    if (score > 40 && score < 50)
                    {
                        speed = 18;
                    }
                    if (score > 50)
                    {
                        speed = 20;
                    }

                }

                

            }
            
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if(e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if(e.Key == Key.Enter && gameOver == true)
            {
                StartGame();
            }

        }
        private void StartGame()
        {
            speed = 8;
            gameTimer.Start();

            moveLeft = false;
            moveRight = false;
            gameOver = false;
            powerMode = false;
            score = 0;
            scoreTxt.Content = "Preživeli ste 0 sekundi";
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
            starImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/star.png"));
            player.Fill = playerImage;
            MyCanvas.Background = Brushes.Gray;
            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if((string)x.Tag == "car"){
                    Canvas.SetTop(x, (rand.Next(100, 400) * -1));
                    Canvas.SetLeft(x, (rand.Next(0, 430)));
                    ChangeCars(x);
                }
                if((string)x.Tag == "star")
                {
                    itemRemover.Add(x);
                }
                
            }
            itemRemover.Clear();
        }

        private void ChangeCars(Rectangle car)
        {
            carNum = rand.Next(1, 6);
            ImageBrush carImage = new ImageBrush();

            switch (carNum)
            {
                case 1:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car1.png"));
                    break;
                case 2:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car2.png"));
                    break;
                case 3:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car3.png"));
                    break;
                case 4:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car4.png"));
                    break;
                case 5:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car5.png"));
                    break;
                case 6:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car6.png"));
                    break;

                    
            }
            car.Fill = carImage;
            Canvas.SetTop(car, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(car, rand.Next(0, 420));

        }
        private void PowerUp()
        {
            i += .5;
            if(i > 4)
            {
                i = 1;
            }

            switch (i)
            {
                case 1:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode1.png"));
                    break;
                case 2:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode2.png"));
                    break;
                case 3:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermodel3.png"));
                    break;
                case 4:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermodel4.png"));
                    break;
            }
            MyCanvas.Background = Brushes.LightCoral;

        }
        private void MakeStar()
        {
            Rectangle newStar = new Rectangle {
                Height = 50,
                Width = 50,
                Tag = "star",
                Fill = starImage
            
            };

            Canvas.SetTop(newStar, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(newStar, (rand.Next(0, 430)));

            MyCanvas.Children.Add(newStar);


        }
    }
}
