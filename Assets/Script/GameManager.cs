using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;
    public Vector2[] boardPositions; // Array of all 25 positions with decimal values
    public TextMeshProUGUI statusText;

    private int[,] boardState = new int[5, 5]; // 0 = empty, 1 = tiger, 2 = goat
    private bool isGoatPlacementPhase = true; // Goats are placed one by one in the beginning
    private bool isTigerTurn = false; // Track whose turn it is
    private int selectedX = -1, selectedY = -1; // Track selected piece for movement
    private int goatsPlaced = 0; // Track how many goats have been placed
    private int goatsCaptured = 0; // Track how many goats have been captured by tigers

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeBoard();
        AssignDecimalPositions(); // Assign decimal positions to the board
        PlaceTigers();
        UpdateStatusText("Place a Goat");
    }

    void InitializeBoard()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                boardState[i, j] = 0; // Initialize all positions as empty
            }
        }
    }

    void AssignDecimalPositions()
    {
        boardPositions = new Vector2[25]
        {
            new Vector2(-137.521f, -255.408f), new Vector2(-137.521f, -256.5981f), new Vector2(-137.521f, -255.408f), new Vector2(-137.521f, -259.119f), new Vector2(-137.521f, -260.33f),
            new Vector2(-136.28f, -255.408f), new Vector2(-136.28f, -256.5981f), new Vector2(-136.28f, -255.408f), new Vector2(-136.28f, -259.119f), new Vector2(-136.28f, -260.33f),
            new Vector2(-135.041f, -255.408f), new Vector2(-135.041f, -256.5981f), new Vector2(-135.041f, -255.408f), new Vector2(-135.041f, -259.119f), new Vector2(-135.041f, -260.33f),
            new Vector2(-133.801f, -255.408f), new Vector2(-133.801f, -256.5981f), new Vector2(-133.801f, -255.408f), new Vector2(-133.801f, -259.119f), new Vector2(-133.801f, -260.33f),
            new Vector2(-132.58f, -255.408f), new Vector2(-132.58f, -256.5981f), new Vector2(-132.58f, -255.408f), new Vector2(-132.58f, 0-259.119f), new Vector2(-132.58f, -260.33f)
        };
    }

  

    void PlaceTigers()
    {
        // Place 4 tigers at the corners
        PlacePiece(0, 0, tigerPrefab); // Top-left corner
        PlacePiece(0, 4, tigerPrefab); // Top-right corner
        PlacePiece(4, 0, tigerPrefab); // Bottom-left corner
        PlacePiece(4, 4, tigerPrefab); // Bottom-right corner
    }

    void PlacePiece(int x, int y, GameObject piecePrefab)
    {
        if (boardState[x, y] == 0)
        {
            Vector2 position = boardPositions[x * 5 + y]; // Get the position from the array
            GameObject piece = Instantiate(piecePrefab, position, Quaternion.identity);
            boardState[x, y] = (piecePrefab == tigerPrefab) ? 1 : 2;
            Debug.Log($"Placed {piecePrefab.name} at ({x}, {y}) - Position: {position}");
        }
        else
        {
            Debug.LogWarning($"Position ({x}, {y}) is already occupied!");
        }
    }

    public void OnPositionClicked(int x, int y)
    {
        if (isGoatPlacementPhase)
        {
            HandleGoatPlacement(x, y);
        }
        else
        {
            if (isTigerTurn)
            {
                HandleTigerTurn(x, y);
            }
            else
            {
                HandleGoatTurn(x, y);
            }
        }
    }

    void HandleGoatPlacement(int x, int y)
    {
        if (boardState[x, y] == 0)
        {
            PlacePiece(x, y, goatPrefab);
            goatsPlaced++;
            if (goatsPlaced >= 5) // After placing 5 goats, start the movement phase
            {
                isGoatPlacementPhase = false;
                isTigerTurn = true;
                UpdateStatusText("Tiger's Turn");
            }
            else
            {
                UpdateStatusText("Place a Goat");
            }
        }
        else
        {
            Debug.Log("Position is already occupied!");
        }
    }

    void HandleTigerTurn(int x, int y)
    {
        if (selectedX == -1 && selectedY == -1)
        {
            // Select a tiger to move
            if (boardState[x, y] == 1) // Check if the position has a tiger
            {
                selectedX = x;
                selectedY = y;
                Debug.Log($"Tiger selected at: ({x}, {y})");
            }
        }
        else
        {
            // Move the selected tiger
            if (IsValidTigerMove(selectedX, selectedY, x, y))
            {
                MovePiece(selectedX, selectedY, x, y);
                selectedX = -1;
                selectedY = -1;
                isTigerTurn = false;
                UpdateStatusText("Goat's Turn");
                CheckWinConditions();
            }
            else
            {
                Debug.Log("Invalid move!");
                selectedX = -1;
                selectedY = -1;
            }
        }
    }

    void HandleGoatTurn(int x, int y)
    {
        if (selectedX == -1 && selectedY == -1)
        {
            // Select a goat to move
            if (boardState[x, y] == 2) // Check if the position has a goat
            {
                selectedX = x;
                selectedY = y;
                Debug.Log($"Goat selected at: ({x}, {y})");
            }
        }
        else
        {
            // Move the selected goat
            if (IsValidGoatMove(selectedX, selectedY, x, y))
            {
                MovePiece(selectedX, selectedY, x, y);
                selectedX = -1;
                selectedY = -1;
                isTigerTurn = true;
                UpdateStatusText("Tiger's Turn");
                CheckWinConditions();
            }
            else
            {
                Debug.Log("Invalid move!");
                selectedX = -1;
                selectedY = -1;
            }
        }
    }

    bool IsValidTigerMove(int fromX, int fromY, int toX, int toY)
    {
        // Check if the target position is empty
        if (boardState[toX, toY] != 0)
        {
            return false;
        }

        // Check if the move is one step in any direction
        int dx = Mathf.Abs(toX - fromX);
        int dy = Mathf.Abs(toY - fromY);
        if (dx <= 1 && dy <= 1)
        {
            return true;
        }

        // Check for a capture move (jumping over a goat)
        if (dx == 2 && dy == 0 || dx == 0 && dy == 2)
        {
            int midX = (fromX + toX) / 2;
            int midY = (fromY + toY) / 2;
            if (boardState[midX, midY] == 2) // Check if there's a goat in the middle
            {
                boardState[midX, midY] = 0; // Capture the goat
                Destroy(GetPieceAtPosition(midX, midY)); // Remove the goat from the scene
                goatsCaptured++;
                return true;
            }
        }

        return false;
    }

    bool IsValidGoatMove(int fromX, int fromY, int toX, int toY)
    {
        // Check if the target position is empty
        if (boardState[toX, toY] != 0)
        {
            return false;
        }

        // Check if the move is one step in any direction
        int dx = Mathf.Abs(toX - fromX);
        int dy = Mathf.Abs(toY - fromY);
        if (dx <= 1 && dy <= 1)
        {
            return true;
        }

        return false;
    }

    void MovePiece(int fromX, int fromY, int toX, int toY)
    {
        int pieceType = boardState[fromX, fromY];
        boardState[fromX, fromY] = 0; // Clear the original position
        boardState[toX, toY] = pieceType; // Update the new position

        // Move the GameObject to the new position
        GameObject piece = GetPieceAtPosition(fromX, fromY);
        if (piece != null)
        {
            piece.transform.position = boardPositions[toX * 5 + toY];
        }
    }

    GameObject GetPieceAtPosition(int x, int y)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(boardPositions[x * 5 + y]);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tiger") || collider.CompareTag("Goat"))
            {
                return collider.gameObject;
            }
        }
        return null;
    }

    void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    void CheckWinConditions()
    {
        if (goatsCaptured >= 5)
        {
            UpdateStatusText("Tigers Win!");
            EndGame();
        }
        else if (AreTigersBlocked())
        {
            UpdateStatusText("Goats Win!");
            EndGame();
        }
    }

    bool AreTigersBlocked()
    {
        // Check if all tigers are blocked (no valid moves)
        // Implement logic to check if tigers can move or capture
        return false; // Placeholder
    }

    void EndGame()
    {
        // Disable further moves
        isGoatPlacementPhase = false;
        isTigerTurn = false;
    }

    void OnDrawGizmos()
    {
        if (boardPositions != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < boardPositions.Length; i++)
            {
                Gizmos.DrawSphere(boardPositions[i], 0.1f); // This will show spheres at each position
            }
        }
    
}
    }
