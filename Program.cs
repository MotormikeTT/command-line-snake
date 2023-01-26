class SnakeGame
{
    enum Directions
    {
        up, right, down, left
    }

    static int width = 20;
    static int height = 20;
    static bool allowPassThroughWalls = true;
    static int x, y;
    static int foodX, foodY;
    static int score;
    static int speed = 100;
    static bool gameOver;
    static Directions direction;

    static List<int> tailX;
    static List<int> tailY;
    static char[,] grid;
    static Random rand = new Random();

    static void Main()
    {
        MainMenu();
        Initialize();
        while (!gameOver)
        {
            Draw();
            Input();
            Logic();
            Thread.Sleep(speed);
        }
        End();
    }

    static void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Main Menu:");
        Console.WriteLine($"1. Set Game Speed (Current: {speed}ms)");
        Console.WriteLine($"2. Set Play Size (Current: width={width} height={height})");
        Console.WriteLine($"3. Set Pass Through Walls (Current: {allowPassThroughWalls})");
        Console.WriteLine("4. Start Game\n");
        char option = Console.ReadKey().KeyChar;
        bool isValid = true;
        switch (option)
        {
            case '1':
                Console.Write("\n\nEnter new speed (in milliseconds): ");
                isValid = int.TryParse(Console.ReadLine(), out speed);
                goto default;
            case '2':
                Console.Write("\n\nEnter width: ");
                width = int.TryParse(Console.ReadLine(), out width) ? width : 20;
                Console.Write("Enter height: ");
                height = int.TryParse(Console.ReadLine(), out height) ? height : 20;
                goto default;
            case '3':
                Console.Write("\n\nAllow pass through walls? (y/n) ");
                char passThroughWalls = Console.ReadKey(true).KeyChar;
                if (passThroughWalls == 'n')
                {
                    allowPassThroughWalls = false;
                }
                goto default;
            case '4':
                break;
            default:
                if (!isValid)
                {
                    Console.WriteLine("\n\nInvalid option, please try again.");
                }
                MainMenu();
                break;
        }
    }

    static void Initialize()
    {
        Console.Clear();
        Console.CursorVisible = false;
        tailX = new List<int>();
        tailY = new List<int>();
        gameOver = false;
        grid = new char[height, width];
        x = width / 2;
        y = height / 2;
        foodX = rand.Next(1, width - 2);
        foodY = rand.Next(1, height - 2);
        score = 0;
        tailX.Add(x);
        tailY.Add(y);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j == 0 || j == width - 1)
                    grid[i, j] = '#';
                else
                    grid[i, j] = ' ';
            }
        }
        for (int i = 0; i < width; i++)
        {
            grid[0, i] = '#';
            grid[height - 1, i] = '#';
        }
    }

    static void Draw()
    {
        // Draw the game grid
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }

        // Draw the snake's tail
        for (int i = 0; i < tailX.Count; i++)
        {
            Console.SetCursorPosition(tailX[i], tailY[i]);
            Console.Write("o");
        }

        // Draw the snake's head
        Console.SetCursorPosition(x, y);
        Console.Write("O");

        // Draw the food
        Console.SetCursorPosition(foodX, foodY);
        Console.Write("X");

        // Draw the score
        Console.SetCursorPosition(width + 2, 0);
        Console.Write("Score: {0}", score);
        
        // Draw the score
        Console.SetCursorPosition(width + 2, 1);
        Console.Write("Your position: {0:D2},{1:D2}", x, y);

        // Draw the score
        Console.SetCursorPosition(width + 2, 2);
        Console.Write("Food's position: {0:D2},{1:D2}", foodX, foodY);

        // Draw the score
        Console.SetCursorPosition(width + 2, 3);
        Console.Write("Your speed: {0:N3}x", (100 - speed) * 0.01 + 1);

        Console.SetCursorPosition(0, 0);
    }

    // Get input from the user
    static void Input()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    direction = Directions.up;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    direction = Directions.right;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    direction = Directions.down;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    direction = Directions.left;
                    break;
            }
        }
    }

    static void Logic()
    {
        // Move the snake's tail
        for (int i = tailX.Count - 1; i > 0; i--)
        {
            tailX[i] = tailX[i - 1];
            tailY[i] = tailY[i - 1];
        }
        tailX[0] = x;
        tailY[0] = y;

        // Move the snake's head based on the current direction
        switch (direction)
        {
            case Directions.up:
                y--;
                break;
            case Directions.down:
                y++;
                break;
            case Directions.left:
                x--;                break;
            case Directions.right:
                x++;
                break;
        }

        // Check for collision with the snake's tail
        for (int i = 0; i < tailX.Count; i++)
        {
            if (x == tailX[i] && y == tailY[i]) gameOver = true;
        }

        // Check for collision with the wall if active
        if (!allowPassThroughWalls)
        {
            if (x == 0 || y == 0 || x == height || y == height) gameOver = true;
        }
        else
        {
            if (y < 1) y = height - 2;
            else if (y > height - 2) y = 1;
            else if (x < 1) x = width - 2;
            else if (x > width - 2) x = 1;
        }
        // Check for collision with the food
        if (x == foodX && y == foodY)
        {
            score++;
            // Up the speed
            if (speed > 50) speed -= 2 ;
            // Add a new segment to the snake's tail
            tailX.Add(x);
            tailY.Add(y);
            // Generate new food coordinates
            do
            {
                foodX = rand.Next(1, width - 2);
                foodY = rand.Next(1, height - 2);
                // Check that the new food coordinates are not on the snake's tail
                for (int i = 0; i < tailX.Count; i++)
                {
                    if (tailX[i] == foodX && tailY[i] == foodY)
                    {
                        foodX = -1;
                        foodY = -1;
                        break;
                    }
                }
            } while (foodX == -1 && foodY == -1);
        }
    }

    static void End()
    {
        Console.Clear();
        Console.WriteLine("Game over!\n");
        Console.WriteLine("Your final score is: {0}\n", score);
        Console.WriteLine("Do you want to play again? (y/n)\n");
        if (Console.ReadKey().KeyChar == 'y') Main();
    }
}