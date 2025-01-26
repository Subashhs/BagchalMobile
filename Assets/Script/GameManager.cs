using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;
    public Vector2[] boardPositions; // Array of all 25 positions with decimal values
    public Text statusText;

    private int[,] boardState = new int[5, 5]; // 0 = empty, 1 = tiger, 2 = goat
    private bool isTigerTurn = false; // Track whose turn it is
    private int selectedX = -1, selectedY = -1; // Track selected piece for movement

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
        PlaceGoats();
        UpdateStatusText("Goat's Turn");
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
            new Vector2(8.47f, 4.03f), new Vector2(8.47f, 2.85f), new Vector2(8.47f, 1.6f), new Vector2(8.47f, 0.339f), new Vector2(8.47f, -0.86f),
            new Vector2(9.683001f, 4.03f), new Vector2(9.683001f, 2.85f), new Vector2(9.683001f, 1.6f), new Vector2(9.683001f, 0.339f), new Vector2(9.683001f, -0.862f),
            new Vector2(10.943f, 4.03f), new Vector2(10.943f, 2.85f), new Vector2(10.943f, 1.6f), new Vector2(10.943f, 0.339f), new Vector2(10.943f, -0.86f),
            new Vector2(12.213f, 4.03f), new Vector2(12.213f, 2.85f), new Vector2(12.213f, 1.6f), new Vector2(12.213f, 0.339f), new Vector2(12.213f, -1.682f),
            new Vector2(13.413f, 4.03f), new Vector2(13.413f, 2.85f), new Vector2(13.413f, 1.6f), new Vector2(13.413f, 0.339f), new Vector2(13.413f, -1.682f)
        };
    }

    void PlaceTigers()
    {
        // Place tigers at the four corners
        PlacePiece(0, 0, tigerPrefab); // Top-left corner (index 0)
        PlacePiece(0, 4, tigerPrefab); // Top-right corner (index 4)
        PlacePiece(4, 0, tigerPrefab); // Bottom-left corner (index 20)
        PlacePiece(4, 4, tigerPrefab); // Bottom-right corner (index 24)
    }

    void PlaceGoats()
    {
        // Place goats at specific positions
        PlacePiece(2, 2, goatPrefab); // Center of the board (index 12)
        PlacePiece(1, 1, goatPrefab); // (index 6)
        PlacePiece(1, 3, goatPrefab); // (index 8)
        PlacePiece(3, 1, goatPrefab); // (index 16)
        PlacePiece(3, 3, goatPrefab); // (index 18)
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
        if (isTigerTurn)
        {
            HandleTigerTurn(x, y);
        }
        else
        {
            HandleGoatTurn(x, y);
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

    void OnDrawGizmos()
    {
        if (boardPositions != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < boardPositions.Length; i++)
            {
                Gizmos.DrawSphere(boardPositions[i], 0.1f);
            }
        }
    }
}