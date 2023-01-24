# command-line-snake

The class SnakeGame is a console-based game where the player controls a snake to move on a grid and collect food while avoiding collision with walls and its own tail. The game includes several functions such as Main(), Initialize(), Draw(), Input(), and Logic().

The Main() function serves as the entry point of the program, it calls other functions in a loop until the game is over. Initialize() sets the initial state of the game, Draw() renders the game on the console, Input() gets input from the user, and Logic() updates the game state based on user input and other factors.

The game uses the Directions enumeration to store the possible directions the snake can move, such as up, down, left, and right. The game also uses several static variables to store the game state such as the width and height of the grid, the position of the snake, the food, and the score. The game also uses two lists to store the position of the snake's tail. The game uses a 2D char array to store the grid and a random object to generate random positions for the food.

Overall, the game reads input from the user and updates the position of the snake and its tail based on the input and other factors, it also renders the game on the console and checks for collision with walls and its own tail.