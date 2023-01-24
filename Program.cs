class SnakeGame
{
    enum Directions
    {
        up, right, down, left
    }

    static int width = 20;
    static int height = 20;
    static int x, y;
    static int foodX, foodY;
    static int score;
    static int speed = 100;
    static bool gameOver;
    static Directions direction;

    static List<int> tailX = new List<int>();
    static List<int> tailY = new List<int>();
    static char[,] grid = new char[height, width];
    static Random rand = new Random();

    static void Main()
    {
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

    static void Initialize()
    {
        Console.CursorVisible = false;
        gameOver = false;
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
        Console.Write("Your speed: {0:D3}ms", speed);

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
                if (y < 1) y = height - 2;
                break;
            case Directions.down:
                y++;
                if (y > height - 2) y = 1;
                break;
            case Directions.left:
                x--;
                if (x < 1) x = width - 2;
                break;
            case Directions.right:
                x++;
                if (x > width - 2) x = 1;
                break;
        }

        // Check for collision with the snake's tail
        for (int i = 0; i < tailX.Count; i++)
        {
            if (x == tailX[i] && y == tailY[i]) gameOver = true;
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
    }
}