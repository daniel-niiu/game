using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Media;

namespace SnakeGame
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    class Program
    {
        //EXTRA - look for console window elements
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;
        public static bool muteMusic = false;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        //CORE - draw and deploy snake
        static void DeploySnake(Queue<Position> snakeElements)
        {
            foreach (Position i in snakeElements)
            {
                Console.SetCursorPosition(i.col, i.row);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("●");
            }
        }

        //CORE - draw and deploy food
        static void DeployFood(List<Position> food)
        {
            int foodPoints = 10;
            foreach (Position i in food)
            {
                Console.SetCursorPosition(i.col - 1, i.row);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("({0})", foodPoints);
                foodPoints++;
            }
        }
        static void DeploySpeicalFood(List<Position> Speicalfood)
        {
            int foodPoints2 = 20;
            foreach (Position i in Speicalfood)
            {
                Console.SetCursorPosition(i.col - 1, i.row);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("({0})", foodPoints2);
                foodPoints2++;
            }
        }

        //CORE - draw and deploy obstacles
        static void DeployObstacles(List<Position> obstacles)
        {
            foreach (Position i in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.SetCursorPosition(i.col, i.row);
                Console.Write("█");
            }
        }

        //CORE - user input direction
        static int UserInput(byte Up, byte Down, byte Left, byte Right, int Direction, bool pause, List<string> PauseMenu)
        {
            while (Console.KeyAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(68, 1);
                ConsoleKeyInfo userInput = Console.ReadKey();

                Console.SetCursorPosition(68, 1);
                Console.WriteLine(" ");


                if (userInput.Key == ConsoleKey.LeftArrow)
                {
                    if (Direction != Right) Direction = Left;
                }
                else if (userInput.Key == ConsoleKey.RightArrow)
                {
                    if (Direction != Left) Direction = Right;
                }
                else if (userInput.Key == ConsoleKey.UpArrow)
                {
                    if (Direction != Down) Direction = Up;
                }
                else if (userInput.Key == ConsoleKey.DownArrow)
                {
                    if (Direction != Up) Direction = Down;
                }
                else if (userInput.Key == ConsoleKey.Enter && pause == false)
                {
                    Console.Clear();
                    MainMenu(PauseMenu, "", 0, 0, false);
                }
            }
            return Direction;
        }


        //EXTRA - main menu
        static void MainMenu(List<string> menuOptions, string statement, int? pointsGet, int? pointsAim, bool noPoints)
        {
            int index = 0;
            string result = "";

            while (true)
            {
                if (muteMusic == false)
                    menuOptions[4] = "|   Mute Music   |";
                else
                    menuOptions[4] = "|   Play Music   |";

                if (pointsGet >= pointsAim && noPoints == true)
                {
                    result = "YOU WIN!";
                }
                else if (pointsGet <= pointsAim)
                {
                    if ((pointsGet == 0 || pointsAim == 0) && noPoints == false)
                    {
                        pointsAim = pointsGet = null;
                        result = "";
                    }
                    else if ((pointsGet == 0 || pointsAim == 0) && noPoints == true)
                    {
                        result = "GAME OVER!";
                    }
                    else
                    {
                        result = "GAME OVER!";
                    }
                }

                Console.Clear();
                Console.SetCursorPosition(59, 13);
                Console.ForegroundColor = ConsoleColor.Blue;

                //display result and statement in main menu
                if (pointsGet >= pointsAim && noPoints == true)
                    Console.WriteLine(result + "\n\t\t\t\t   " + statement);
                else if (pointsGet <= pointsAim)
                    Console.WriteLine(result + "\n\t\t\t\t\t\t     " + statement);
                else
                    Console.WriteLine(result + "\n\t\t\t      " + statement);

                //display game logo in main menu
                Console.SetCursorPosition(41, 2);
                Console.WriteLine("● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ●");
                Console.SetCursorPosition(41, 3);
                Console.WriteLine("●                                             ●");
                Console.SetCursorPosition(41, 4);
                Console.WriteLine("●   ███████╗███╗  ██╗ █████╗ ██╗ ██╗██████╗   ●");
                Console.SetCursorPosition(41, 5);
                Console.WriteLine("●   ██╔════╝████╗ ██║██╔══██╗██║██╔╝██╔═══╝   ●");
                Console.SetCursorPosition(41, 6);
                Console.WriteLine("●   ███████╗██╔██╗██║███████║████╔╝ ██████╗   ●");
                Console.SetCursorPosition(41, 7);
                Console.WriteLine("●   ╚════██║██║╚████║██╔══██║██╔██╗ ██╔═══╝   ●");
                Console.SetCursorPosition(41, 8);
                Console.WriteLine("●   ███████║██║ ╚███║██║  ██║██║ ██╗██████╗   ●");
                Console.SetCursorPosition(41, 9);
                Console.WriteLine("●   ╚══════╝╚═╝  ╚══╝╚═╝  ╚═╝╚═╝ ╚═╝╚═════╝   ●");
                Console.SetCursorPosition(41, 10);
                Console.WriteLine("●                                              ");
                Console.SetCursorPosition(41, 11);
                Console.WriteLine("● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ►   ♥");

                //List print out options for user to select by pressing up and down keys
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    Console.SetCursorPosition(55, 16 + i);
                    if (i == index)
                    {
                        //selecton background colour
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(menuOptions[i]);
                }

                //obtains the next character or any key pressed by the user  
                ConsoleKeyInfo presskey = Console.ReadKey();

                //moves selection down by index
                if (presskey.Key == ConsoleKey.DownArrow)
                {
                    if (index == menuOptions.Count - 1)
                    {
                        index = 0;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        index += 2;
                    }

                    //moves selection up by index
                }
                else if (presskey.Key == ConsoleKey.UpArrow)
                {
                    if (index <= 0)
                    {
                        index = menuOptions.Count - 1;
                    }
                    else
                    {
                        index -= 2;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                else if (presskey.Key == ConsoleKey.Enter)
                {
                    //Start to proceed to the game
                    if (index == 0)
                    {
                        Console.Clear();
                        return;
                    }

                    else if (index == 2)
                    {
                        //display scoreboard
                        ScoreBoard();
                    }

                    else if (index == 4)
                    {
                        //mute or unmute music 
                        if (muteMusic == false)
                        {
                            muteMusic = true;
                            menuOptions[4] = "|   Play Music   |";
                        }
                        else
                        {
                            muteMusic = false;
                            menuOptions[4] = "|   Mute Music   |";
                        }

                        SoundEffect("Background");

                    }

                    else if (index == 6)
                    {
                        //view rules as help
                        HelpPage();
                    }

                    else if (index == 8)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    //to prevent background from turning blue when player accidentally presses other keys (left and right keys)                 
                }
            }
        }

        //get player name
        static string PlayerName()
        {
            string playerName;
            string message;

            Console.Clear();

            for (; ; )
            {
                Console.SetCursorPosition(42, 16);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Please enter your name and press 'Enter'");

                Console.SetCursorPosition(57, 17);
                Console.ForegroundColor = ConsoleColor.Gray;
                playerName = Console.ReadLine();

                if (playerName == "")
                    message = "\t  You must enter your name \n\t\t\t\t\t  (Press 'Enter' to enter your name again)";

                else if (!Regex.IsMatch(playerName, @"^[a-zA-Z]+$"))
                    message = "  Your name can only contain alphabets \n\t\t\t\t\t  (Press 'Enter' to enter your name again)";

                else if (playerName.Length < 2)
                    message = "\t   Your name is too short \n\t\t\t\t\t   (Press 'Enter' to enter your name again)";

                else
                    break;

                Console.Clear();
                Console.SetCursorPosition(42, 16);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message);
                Console.ReadKey();
                Console.Clear();
            }

            Console.Clear();

            return playerName;
        }

        //write player name and score in file
        static void WriteFile(string playerName, int userPoints)
        {
            int x = 0;
            int totalScore;
            bool ExistingName = false;

            string fileName = @"Records\records.txt";
            string[] fileLine = File.ReadAllLines(fileName);

            foreach (string i in fileLine)
            {
                string name = (Regex.Replace(i, "[^a-zA-Z]", ""));

                if (name == playerName)
                {
                    totalScore = Int32.Parse(Regex.Replace(i, "[^0-9]", ""));
                    totalScore = totalScore + userPoints;
                    fileLine[x] = playerName + "  " + totalScore;
                    ExistingName = true;
                }

                x++;
            }

            File.WriteAllText(fileName, string.Empty);

            foreach (string i in fileLine)
            {
                File.AppendAllText(fileName, i + Environment.NewLine);
            }

            if (ExistingName == false)
            {
                File.AppendAllText(fileName, playerName + "  " + userPoints + Environment.NewLine);
            }

        }

        //allow player to mute or unmute music 
        static void SoundEffect(string soundEffect)
        {
            var BackgroundMusic = new SoundPlayer();
            BackgroundMusic.SoundLocation = @"SoundEffect\Background.wav";

            var GameOverSoundEffect = new SoundPlayer();
            GameOverSoundEffect.SoundLocation = @"SoundEffect\GameOver.wav";;

            if (muteMusic == true)
            {
                //music will stop playing
                BackgroundMusic.Stop();
            }

            else
            {
                if (soundEffect == "Background")
                {
                    //playing the music in a loop
                    BackgroundMusic.PlayLooping();
                }

                else if (soundEffect == "GameOver")
                {
                    //plays game over sound effect
                    GameOverSoundEffect.Play();
                }
            }
        }

        static void ScoreBoard()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(56, 1);
            Console.WriteLine("Top 10 Players ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(42, 3);
            Console.Write("Player Name");
            Console.SetCursorPosition(72, 3);
            Console.Write("Total Score");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 4);
            Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");

            string fileName = @"Records\records.txt";
            string[] fileLine = File.ReadAllLines(fileName);

            List<string> name = new List<string>();
            List<string> score = new List<string>();

            foreach (string i in fileLine)
            {
                name.Add(Regex.Replace(i, "[^a-zA-Z]", ""));
                score.Add(Regex.Replace(i, "[^0-9]", ""));
            }

            for (int x = 0; x < 10; x++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(43, x + 5);
                Console.Write(name[x]);

                Console.SetCursorPosition(76, x + 5);
                Console.Write(score[x]);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(42, 31);
            Console.Write("Press 'ENTER' to return to the main menu");

            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 32);
                ConsoleKeyInfo presskey = Console.ReadKey();
                Console.SetCursorPosition(0, 32);
                Console.WriteLine(" ");
                if (presskey.Key == ConsoleKey.Enter)
                    break;
            }
        }

        static void HelpPage()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(56, 1);
            Console.WriteLine("Snake Game");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(54, 3);
            Console.Write("Help and Rules");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(13, 7);
            Console.WriteLine("Hello! Welcome to the Snake Game! Here are some tips and guides to help you play and win this game!");
            Console.SetCursorPosition(20, 10);
            Console.WriteLine("1. The snake can be controlled using your arrow keys.");
            Console.SetCursorPosition(20, 12);
            Console.WriteLine("2. In order to win, collect as many points till you can reach 100 points.");
            Console.SetCursorPosition(20, 14);
            Console.WriteLine("3. To collect points, snake must be navigated to the food which each hold 10 points.");
            Console.SetCursorPosition(20, 16);
            Console.WriteLine("4. Player is given 3 healths or chances to win this game.");
            Console.SetCursorPosition(20, 18);
            Console.WriteLine("5. If snake hits the obstacles, the player will lose a health.");
            Console.SetCursorPosition(20, 20);
            Console.WriteLine("6. If player managed to lose all 3 healths, the player will lose and the game will be over.");
            Console.SetCursorPosition(48, 23);
            Console.WriteLine("Happy playing and good luck!");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(42, 31);
            Console.Write("Press 'ENTER' to return to the main menu");

            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 32);
                ConsoleKeyInfo presskey = Console.ReadKey();
                Console.SetCursorPosition(0, 32);
                Console.WriteLine(" ");
                if (presskey.Key == ConsoleKey.Enter)
                    break;
            }
        }


        static void Main(string[] args)
        {
            Console.Title = "Snake";
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            while (true)
            {
                //SleepTime = snake speed
                double sleepTime = 100;
                byte right = 0;
                byte left = 1;
                byte down = 2;
                byte up = 3;
                int lastFood, countPoints, foodPoints;
                lastFood = countPoints = foodPoints = 0;
                int foodDissapearTime = 15000;
                int health = 3;
                int WinScore = 100;
                int GameHeightMax = 30;
                int GameHeightMin = 4;
                int userPoints = countPoints;
                int direction = right;
                bool pauseGame = false;
                bool checkSoundEffect = false;
                string LineStatement = "";
                int soundEffectPlayTime = 3;
                string playerName;

                Random randomNumbersGenerator = new Random();

                //EXTRA - restrict resizing of console window
                IntPtr handle = GetConsoleWindow();
                IntPtr sysMenu = GetSystemMenu(handle, false);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);

                //CORE - snake directions input
                Position[] directions = new Position[]
                {
                    //direction
                    new Position( 0,  1),   //right
                    new Position( 0, -1),   //left
                    new Position( 1,  0),   //down
                    new Position(-1,  0),   //up
                };

                Console.BufferHeight = Console.WindowHeight;
                lastFood = Environment.TickCount;

                //CORE - creation of obstacles
                List<Position> obstacles = new List<Position>();
                for (int i = 0; i < 5; i++)
                {
                    obstacles.Add(new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2)));
                }

                //CORE - place food in random position 
                List<Position> food = new List<Position>();
                for (int i = 0; i < 1; i++)
                {
                    food.Add(new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2)));
                }
                //CORE - place food in random position 
                List<Position> Speicalfood = new List<Position>();
                for (int i = 0; i < 1; i++)
                {
                    Speicalfood.Add(new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2)));
                }
                //CORE - snake initialization
                Queue<Position> snakeElements = new Queue<Position>();
                for (int i = 0; i <= 3; i++)
                {
                    snakeElements.Enqueue(new Position(4, i));
                }

                //EXTRA - main menu initialization
                List<string> startMenu = new List<string>()
                {
                    "|   Start Game   |", "\n\n",
                    "|   Scoreboard   |", "\n\n",
                    "|   Mute Music   |", "\n\n",
                    "|  Help & Rules  |", "\n\n",
                    "|   Leave Game   |"
                };

                List<string> pauseMenu = new List<string>()
                {
                    "|    Continue    |", "\n\n",
                    "|   Scoreboard   |", "\n\n",
                    "|   Mute Music   |", "\n\n",
                    "|  Help & Rules  |", "\n\n",
                    "|   Leave Game   |"
                };

                List<string> overMenu = new List<string>()
                {
                    "|     Return     |", "\n\n",
                    "|   Scoreboard   |", "\n\n",
                    "|   Mute Music   |", "\n\n",
                    "|  Help & Rules  |", "\n\n",
                    "|   Leave Game   |"
                };

                Console.CursorVisible = false;
                Console.WindowHeight = 36;
                Console.WindowWidth = 125;
                DeploySnake(snakeElements);

                SoundEffect("Background");
                LineStatement = "Welcome to the Snake Game! Do you have what it takes to win the game?";
                MainMenu(startMenu, LineStatement, 0, 0, false);
                playerName = PlayerName();

                for (; ; )
                {
                    direction = UserInput(up, down, left, right, direction, pauseGame, pauseMenu);
                    DeployObstacles(obstacles);
                    Position snakeHead = snakeElements.Last();
                    Position nextDirection = directions[direction];
                    Position snakeBody = new Position(snakeHead.row + nextDirection.row, snakeHead.col + nextDirection.col);

                    //CORE - snake movement
                    if (snakeBody.col < 1)
                        snakeBody.col = Console.WindowWidth - 2;
                    else if (snakeBody.row < GameHeightMin)
                        snakeBody.row = GameHeightMax - 1;
                    else if (snakeBody.row >= GameHeightMax)
                        snakeBody.row = GameHeightMin;
                    else if (snakeBody.col >= Console.WindowWidth - 1)
                        snakeBody.col = 1;

                    //CORE - score 
                    Console.SetCursorPosition(1, 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    userPoints = countPoints;
                    Console.WriteLine("Score: {0}", userPoints);
                    Console.SetCursorPosition(114, 1);
                    Console.WriteLine("Health: {0}", health);
                    Console.SetCursorPosition(57, 1);
                    Console.WriteLine("Snake Game");

                    //EXTRA - visible horizontal border
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(0, 3);
                    Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(0, 30);
                    Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");

                    //EXTRA - visible vertical border
                    for (int i = 4; i < 30; i++)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.WriteLine("║");
                        Console.SetCursorPosition(Console.WindowWidth - 1, i);
                        Console.WriteLine("║");
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(45, 33);
                    Console.Write("[Press 'ENTER' to pause the game]");

                    //CORE - win game message
                    if (userPoints >= WinScore)
                    {
                        LineStatement = "Congratulations! You have reached the goal and won the game!";
                        MainMenu(overMenu, LineStatement, userPoints, WinScore, true);
                        break;
                    }

                    //CORE - game over message
                    if (snakeElements.Contains(snakeBody) || obstacles.Contains(snakeBody))
                    {
                        Console.Clear();
                        checkSoundEffect = true;
                        health -= 1;

                        if (health == 0)
                        {
                            SoundEffect("GameOver");
                            LineStatement = "You scored " + userPoints + " points!";
                            MainMenu(overMenu, LineStatement, userPoints, WinScore, true);
                            WriteFile(playerName, userPoints);
                            break;
                        }
                    }
                    Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("●");

                    snakeElements.Enqueue(snakeBody);
                    Console.SetCursorPosition(snakeBody.col, snakeBody.row);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (direction == right)
                        Console.Write("►");
                    else if (direction == left)
                        Console.Write("◄");
                    else if (direction == up)
                        Console.Write("▲");
                    else if (direction == down)
                        Console.Write("▼");

                    for (int i = 0; i < 1; i++)
                    {
                        if ((snakeBody.col == food[i].col - 1 || snakeBody.col == food[i].col || snakeBody.col == food[i].col + 1) && snakeBody.row == food[i].row)
                        {
                            Console.SetCursorPosition(food[i].col - 1, food[i].row);
                            Console.Write("    ");
                            checkSoundEffect = true;
                            snakeElements.Enqueue(food[i]);

                            //CORE - actions when snake eats the food
                            do
                            {
                                //add 10 points
                                countPoints = countPoints + 10;
                                //place new food in new random position
                                food[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(food[i]) && obstacles.Contains(food[i]));
                            lastFood = Environment.TickCount;
                            Console.SetCursorPosition(food[i].col, food[i].row);
                            Console.ForegroundColor = ConsoleColor.Green;
                            sleepTime--;

                            //add obstacle after eating food
                            Position obstaclecheck = new Position();
                            do
                            {
                                obstaclecheck = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(obstaclecheck) || obstacles.Contains(obstaclecheck) && (food[i].row != obstaclecheck.row && food[i].col != obstaclecheck.col && food[i].col != obstaclecheck.col - 1 && food[i].col != obstaclecheck.col + 1));
                            obstacles.Add(obstaclecheck);
                            DeployObstacles(obstacles);
                            break;
                        }
                    }
                    // when snake eats speical food
                    for (int i = 0; i < 1; i++)
                    {
                        if ((snakeBody.col == Speicalfood[i].col - 1 || snakeBody.col == Speicalfood[i].col || snakeBody.col == Speicalfood[i].col + 1) && snakeBody.row == Speicalfood[i].row)
                        {
                            Console.SetCursorPosition(Speicalfood[i].col - 1, Speicalfood[i].row);
                            Console.Write("    ");
                            checkSoundEffect = true;
                            snakeElements.Enqueue(Speicalfood[i]);

                            //CORE - actions when snake eats the food
                            do
                            {
                                //add 10 points
                                countPoints = countPoints + 20;
                                //place new food in new random position
                                Speicalfood[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(Speicalfood[i]) && obstacles.Contains(Speicalfood[i]));
                            lastFood = Environment.TickCount;
                            Console.SetCursorPosition(Speicalfood[i].col, Speicalfood[i].row);
                            Console.ForegroundColor = ConsoleColor.Green;
                            sleepTime--;
                        }
                    }
                    //CORE - remove snake traces
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");

                    //CORE - erase traces then generate food randomly
                    if (Environment.TickCount - lastFood >= foodDissapearTime && foodPoints <=100)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            Console.SetCursorPosition(food[i].col - 1, food[i].row);
                            Console.Write("    ");

                            do
                            {
                                food[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(food[i]) || obstacles.Contains(food[i]));
                            lastFood = Environment.TickCount;
                            foodPoints++;
                        }
                    }
                    else
                    if (Environment.TickCount - lastFood >= foodDissapearTime && foodPoints <= 200)
                    {
                        foodDissapearTime = 10000;
                        sleepTime = 50;
                        for (int i = 0; i < 1; i++)
                        {
                            Console.SetCursorPosition(food[i].col - 1, food[i].row);
                            Console.Write("    ");

                            do
                            {
                                food[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(food[i]) || obstacles.Contains(food[i]));
                            lastFood = Environment.TickCount;
                            foodPoints++;
                        }
                    }
                    else
                    if (Environment.TickCount - lastFood >= foodDissapearTime && foodPoints <= 500)
                    {
                        sleepTime = 30;
                        foodDissapearTime = 1000;
                        for (int i = 0; i < 1; i++)
                        {
                            Console.SetCursorPosition(food[i].col - 1, food[i].row);
                            Console.Write("    ");

                            do
                            {
                                food[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(food[i]) || obstacles.Contains(food[i]));
                            lastFood = Environment.TickCount;
                            foodPoints++;
                        }
                    }

                    DeployFood(food);
                    sleepTime -= 0.01;
                    DeploySpeicalFood(Speicalfood);
                  Thread.Sleep((int)sleepTime);

                    if (checkSoundEffect == true)
                    {
                        if (soundEffectPlayTime == 0)
                        {
                            SoundEffect("Background");
                            soundEffectPlayTime = 2;
                            checkSoundEffect = false;
                        }
                        else
                        {
                            soundEffectPlayTime--;
                        }
                    }
                }
            }
        }
    }
}