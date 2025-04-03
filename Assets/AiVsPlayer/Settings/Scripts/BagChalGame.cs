using UnityEngine;

public class BagchalGameAI : MonoBehaviour
{
    public GameObject goatPrefab;
    public GameObject tigerPrefab;
    private int[,] board = new int[5, 5];
    private bool isPlayerGoat;
    public bool IsGoatTurn { get; private set; } = true;
    private int goatsRemaining = 20;
    private int tigersRemaining = 4;

    void Start()
    {
        // InitializeBoard will be called from GameManagerAI with the playerCharacter parameter
    }

    public void InitializeBoard(string playerCharacter)
    {
        isPlayerGoat = playerCharacter == "Goat";
        if (!isPlayerGoat)
        {
            // If player is Tiger, start with Tiger's turn
            IsGoatTurn = false;
        }

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

        UpdateBoardVisuals();
    }

    void Update()
    {
        if (!isPlayerGoat && IsGoatTurn)
        {
            // AI logic for placing goats if player is Tiger
            AIPlaceGoat();
        }
        else if (isPlayerGoat && !IsGoatTurn)
        {
            // AI logic for moving tigers if player is Goat
            AIMoveTiger();
        }
    }

    void AIPlaceGoat()
    {
        // Simple AI logic to place a goat at the first available position
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j] == 0)
                {
                    PlaceGoat(i, j);
                    IsGoatTurn = false; // After placing a goat, it's the tiger's turn
                    return;
                }
            }
        }
    }

    void AIMoveTiger()
    {
        // Simple AI logic to move the first tiger found
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j] == 2)
                {
                    // Try moving the tiger to a random adjacent position
                    int[] dx = { 1, -1, 0, 0 };
                    int[] dy = { 0, 0, 1, -1 };
                    for (int k = 0; k < 4; k++)
                    {
                        int newX = i + dx[k];
                        int newY = j + dy[k];
                        if (newX >= 0 && newX < 5 && newY >= 0 && newY < 5 && board[newX, newY] == 0)
                        {
                            MoveTiger(i, j, newX, newY);
                            IsGoatTurn = true; // After moving a tiger, it's the goat's turn
                            return;
                        }
                    }
                }
            }
        }
    }

    public void PlaceGoat(int x, int y)
    {
        if (board[x, y] == 0 && goatsRemaining > 0)
        {
            board[x, y] = 1; // 1 represents a goat
            Instantiate(goatPrefab, new Vector3(x, y, 0), Quaternion.identity);
            goatsRemaining--;
            IsGoatTurn = false; // After placing a goat, it's the tiger's turn
        }
    }

    public bool IsTigerAt(int x, int y)
    {
        return board[x, y] == 2; // 2 represents a tiger
    }

    public bool CanMoveTiger(int startX, int startY, int endX, int endY)
    {
        // Add logic to check if the tiger can move from (startX, startY) to (endX, endY)
        // This is a placeholder logic
        return board[startX, startY] == 2 && board[endX, endY] == 0;
    }

    public void MoveTiger(int startX, int startY, int endX, int endY)
    {
        if (CanMoveTiger(startX, startY, endX, endY))
        {
            board[endX, endY] = 2; // Move tiger to the new position
            board[startX, startY] = 0; // Clear the old position
            IsGoatTurn = true; // After moving a tiger, it's the goat's turn
        }
    }

    void UpdateBoardVisuals()
    {
        // Code to update the visual representation of the board
    }
}