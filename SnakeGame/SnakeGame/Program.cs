using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        //draw and deploy snake
        static void DeploySnake(Queue<Position> snakeElements)
        {
            foreach (Position i in snakeElements)
            {
                Console.SetCursorPosition(i.col, i.row);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("*");
            }
        }

        //draw and deploy food
        static void DeployFood(List<Position> _food)
        {
            int _foodPoint = 10;
            foreach (Position i in _food)
            {
                Console.SetCursorPosition(i.col - 1, i.row);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("({0})", _foodPoint);
                _foodPoint++;
            }
        }

        //draw and deploy obstacles
        static void DeployObstacles(List<Position> obstacles)
        {
            foreach (Position i in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(i.col, i.row);
                Console.Write("||");
            }
        }

        //direction(user key input)
        static int UserInput(byte Up, byte Down, byte Left, byte Right, int Direction)
        {
            while (Console.KeyAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(68, 1);
                ConsoleKeyInfo _userInput = Console.ReadKey();

                Console.SetCursorPosition(68, 1);
                Console.WriteLine(" ");


                if (_userInput.Key == ConsoleKey.LeftArrow)
                {
                    if (Direction != Right) Direction = Left;
                }
                else if (_userInput.Key == ConsoleKey.RightArrow)
                {
                    if (Direction != Left) Direction = Right;
                }
                else if (_userInput.Key == ConsoleKey.UpArrow)
                {
                    if (Direction != Down) Direction = Up;
                }
                else if (_userInput.Key == ConsoleKey.DownArrow)
                {
                    if (Direction != Up) Direction = Down;
                }
            }
            return Direction;
        }



        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            while (true)
            {
                //SleepTime = snake speed
                double sleepTime = 100;
                byte right = 0;
                byte left = 1;
                byte down = 2;
                byte up = 3;
                int lastFoodTime, negativePoints, _foodPoints;
                lastFoodTime = negativePoints = _foodPoints = 0;
                int foodDissapearTime = 10000;
                int health = 1;
                //WinScore = Winning Score
                int WinScore = 150;
                int GameHeightMax = 30;
                int GameHeightMin = 4;
                int userPoints = negativePoints;
                int direction = right;

                Random randomNumbersGenerator = new Random();

                //Snake directions input
                Position[] directions = new Position[]
                {
                    //direction
                    new Position( 0,  1),   //right
                    new Position( 0, -1),   //left
                    new Position( 1,  0),   //down
                    new Position(-1,  0),   //up
                };

                Console.BufferHeight = Console.WindowHeight;
                lastFoodTime = Environment.TickCount;

                //creation of obstacles
                List<Position> obstacles = new List<Position>();
                for (int i = 0; i < 5; i++)
                {
                    obstacles.Add(new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2)));
                }

                //place food in random position 
                List<Position> _food = new List<Position>();
                for (int i = 0; i < 1; i++)
                {
                    _food.Add(new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2)));
                }

                //snake initialization
                Queue<Position> snakeElements = new Queue<Position>();
                for (int i = 0; i <= 3; i++)
                {
                    snakeElements.Enqueue(new Position(4, i));
                }

                Console.CursorVisible = false;
                Console.WindowHeight = 36;
                Console.WindowWidth = 125;
                DeploySnake(snakeElements);

                for (; ; )
                {
                    direction = UserInput(up, down, left, right, direction);
                    DeployObstacles(obstacles);
                    Position snakeHead = snakeElements.Last();
                    Position nextDirection = directions[direction];
                    Position newBody = new Position(snakeHead.row + nextDirection.row, snakeHead.col + nextDirection.col);

                    //snake movement
                    if (newBody.col < 1)
                        newBody.col = Console.WindowWidth - 2;
                    else if (newBody.row < GameHeightMin)
                        newBody.row = GameHeightMax - 1;
                    else if (newBody.row >= GameHeightMax)
                        newBody.row = GameHeightMin;
                    else if (newBody.col >= Console.WindowWidth - 1)
                        newBody.col = 1;

                    //score 
                    Console.SetCursorPosition(57, 1);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    userPoints = negativePoints;
                    Console.WriteLine("Score: {0}", userPoints);

                    //visible horizontal border
                    Console.SetCursorPosition(0, 3);
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                    Console.SetCursorPosition(0, 30);
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");

                    //visible vertical border
                    for (int i = 4; i < 30; i++)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.WriteLine("|");
                        Console.SetCursorPosition(Console.WindowWidth - 1, i);
                        Console.WriteLine("|");
                    }

                    //win game message
                    if (userPoints >= WinScore)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(32, 17);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        userPoints = negativePoints;
                        Console.WriteLine("Congratulations! You have reached {0} points and won the game!", WinScore);
                        Console.SetCursorPosition(43, 18);
                        Console.WriteLine("Press 'Enter' to exit the program.");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    //game over message
                    if (snakeElements.Contains(newBody) || obstacles.Contains(newBody))
                    {
                        Console.Clear();
                        health -= 1;

                        if (health == 0)
                        {
                            Console.Clear();
                            Console.SetCursorPosition(44, 17);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            userPoints = negativePoints;
                            Console.WriteLine("You lost! You scored " + userPoints + " points!");
                            Console.SetCursorPosition(43, 18);
                            Console.WriteLine("Press 'Enter' to exit the program.");
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }
                    Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("*");

                    snakeElements.Enqueue(newBody);
                    Console.SetCursorPosition(newBody.col, newBody.row);
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (direction == right)
                        Console.Write(">");
                    else if (direction == left)
                        Console.Write("<");
                    else if (direction == up)
                        Console.Write("^");
                    else if (direction == down)
                        Console.Write("v");

                    for (int i = 0; i < 1; i++)
                    {
                        if ((newBody.col == _food[i].col - 1 || newBody.col == _food[i].col || newBody.col == _food[i].col + 1) && newBody.row == _food[i].row)
                        {
                            Console.SetCursorPosition(_food[i].col - 1, _food[i].row);
                            Console.Write("    ");
                            snakeElements.Enqueue(_food[i]);

                            //actions when snake eats the food
                            do
                            {
                                //add 10 points
                                negativePoints = negativePoints + 10;
                                //place new food in new random position
                                _food[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(_food[i]) && obstacles.Contains(_food[i]));

                            lastFoodTime = Environment.TickCount;
                            Console.SetCursorPosition(_food[i].col, _food[i].row);
                            Console.ForegroundColor = ConsoleColor.Red;
                            sleepTime--;

                            //add obstacle after eating food
                            Position obstaclecheck = new Position();
                            do
                            {
                                obstaclecheck = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(obstaclecheck) || obstacles.Contains(obstaclecheck) && (_food[i].row != obstaclecheck.row && _food[i].col != obstaclecheck.col && _food[i].col != obstaclecheck.col - 1 && _food[i].col != obstaclecheck.col + 1)); // && is the right code to prevent the blocks from staying the same line as food when random position
                            obstacles.Add(obstaclecheck);
                            DeployObstacles(obstacles);
                            break;
                        }
                    }

                    //remove snake traces
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");

                    //erase traces then generate food randomly
                    if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            Console.SetCursorPosition(_food[i].col - 1, _food[i].row);
                            Console.Write("    ");

                            do
                            {
                                _food[i] = new Position(randomNumbersGenerator.Next(GameHeightMin, GameHeightMax), randomNumbersGenerator.Next(2, Console.WindowWidth - 2));
                            }
                            while (snakeElements.Contains(_food[i]) || obstacles.Contains(_food[i]));
                            lastFoodTime = Environment.TickCount;
                            _foodPoints++;
                        }
                    }

                    DeployFood(_food);
                    sleepTime -= 0.01;
                    Thread.Sleep((int)sleepTime);
                }
            }
        }
    }
}