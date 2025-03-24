using UnityEngine;

public class BagchalGameAI : MonoBehaviour
{
    // 0 = empty, 1 = player (goat), 2 = AI (tiger)
    public GameObject goatPrefab;
    public GameObject tigerPrefab;
    private int[,] board = new int[5, 5];
    private bool isGoatTurn = true;
    private int goatsRemaining = 20;
    private int tigersRemaining = 4;

    void Start()
    {
        InitializeBoard();
        UpdateBoardVisuals(); // Update visuals after initializing the board
    }

    void InitializeBoard()
    {
        // Set up the initial board state
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                board[i, j] = 0; // Set all positions to empty
            }
        }

        // Place initial tigers on the four corners
        board[0, 0] = 2;
        board[0, 4] = 2;
        board[4, 0] = 2;
        board[4, 4] = 2;
    }

    public void PlaceGoat(int x, int y)
    {
        if (isGoatTurn && board[x, y] == 0 && goatsRemaining > 0)
        {
            board[x, y] = 1; // Place goat
            goatsRemaining--;
            Debug.Log("Goat placed at: " + x + ", " + y);
            UpdateBoardVisuals(); // Update visuals after placing goat
            CheckForWinner();
            isGoatTurn = false; // Switch turn to AI
            AITurn();
        }
        else
        {
            Debug.Log("Invalid move or no goats left!");
        }
    }

    void AITurn()
    {
        // AI (tigers) move randomly
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j] == 2) // Find a tiger
                {
                    // Try to move in a random direction
                    int dirX = Random.Range(-1, 2);
                    int dirY = Random.Range(-1, 2);
                    int newX = i + dirX;
                    int newY = j + dirY;

                    if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5 && board[newX, newY] == 0)
                    {
                        MoveTiger(i, j, newX, newY);
                        return;
                    }
                }
            }
        }

        isGoatTurn = true; // After AI moves, return turn to the player
    }

    public void MoveTiger(int oldX, int oldY, int newX, int newY)
    {
        if (board[oldX, oldY] == 2 && board[newX, newY] == 0)
        {
            board[oldX, oldY] = 0;
            board[newX, newY] = 2;
            Debug.Log("Tiger moved from: " + oldX + ", " + oldY + " to: " + newX + ", " + newY);
            UpdateBoardVisuals();
            CheckForWinner();
            isGoatTurn = true;
        }
        else
        {
            Debug.Log("Invalid tiger move!");
        }
    }

    public bool CanMoveTiger(int oldX, int oldY, int newX, int newY)
    {
        return (board[oldX, oldY] == 2 && board[newX, newY] == 0);
    }

    public bool IsTigerAt(int x, int y)
    {
        return board[x, y] == 2;
    }

    public bool IsGoatTurn
    {
        get { return isGoatTurn; }
    }

    void CheckForWinner()
    {
        // Win condition for goats: All tigers are captured
        if (tigersRemaining == 0)
        {
            Debug.Log("Goats win! All tigers are captured.");
            ResetGame();
        }

        // Win condition for tigers: No goats left on the board
        if (goatsRemaining == 0)
        {
            Debug.Log("Tigers win! No goats left.");
            ResetGame();
        }

        // Additional logic for checking if a goat is captured (i.e., surrounded by tigers)
        CheckGoatsSurrounded();
    }

    void CheckGoatsSurrounded()
    {
        // Loop through the board to check for goats that are surrounded
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j] == 1) // Goat found
                {
                    if (IsGoatSurrounded(i, j))
                    {
                        // Capture the goat if surrounded by tigers
                        board[i, j] = 0;
                        goatsRemaining++;
                        Debug.Log("Goat at " + i + ", " + j + " captured by tigers!");
                    }
                }
            }
        }
    }

    bool IsGoatSurrounded(int x, int y)
    {
        // Check if all adjacent tiles (up, down, left, right) are occupied by tigers (2)
        int[] dirX = { -1, 1, 0, 0 };
        int[] dirY = { 0, 0, -1, 1 };

        int tigerCount = 0;
        for (int i = 0; i < 4; i++)
        {
            int newX = x + dirX[i];
            int newY = y + dirY[i];
            if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5)
            {
                if (board[newX, newY] == 2) // Tiger is adjacent
                {
                    tigerCount++;
                }
            }
        }

        return tigerCount == 4; // Goat is surrounded if all 4 adjacent positions are tigers
    }

    void ResetGame()
    {
        // Reset the game state and board for a new round
        InitializeBoard();
        goatsRemaining = 20;
        tigersRemaining = 4;
        isGoatTurn = true;
        UpdateBoardVisuals();
    }

    void UpdateBoardVisuals()
    {
        // Clear existing pieces
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Spawn new pieces
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j] == 1)
                {
                    Instantiate(goatPrefab, new Vector2(i, j), Quaternion.identity, transform);
                }
                else if (board[i, j] == 2)
                {
                    Instantiate(tigerPrefab, new Vector2(i, j), Quaternion.identity, transform);
                }
            }
        }
    }
}
