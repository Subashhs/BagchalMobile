using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    private Dictionary<string, GameObject> boardPositions = new Dictionary<string, GameObject>();
    private int totalGoats = 20;
    private int placedGoats = 0;
    private bool isTigerTurn = false;
    private bool isPlacementPhase = true;
    private GameObject selectedTiger = null;
    private GameObject selectedGoat = null;
    private List<GameObject> tigers = new List<GameObject>();
    private int goatsCaptured = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        InitializeBoard();
        PlaceInitialTigers();
        isTigerTurn = false; // Goat starts first
    }

    #region Board Initialization
    void InitializeBoard()
    {
        float tileSize = 200f; // Each tile is 200x200
        float boardStartX = -500f; // Top-left corner (-500, +500)
        float boardStartY = 500f;

        // Initialize all 25 positions (5x5 grid)
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                string key = $"Tile_{x}_{y}";
                float tileX = boardStartX + tileSize * x + tileSize / 2;
                float tileY = boardStartY - tileSize * y - tileSize / 2;

                boardPositions[key] = null;

                // Optionally, debug tile positions
                Debug.Log($"Tile {key} position: ({tileX}, {tileY})");
            }
        }

        // Link buttons to positions
        foreach (Button button in FindObjectsOfType<Button>())
        {
            string positionKey = button.name;
            if (boardPositions.ContainsKey(positionKey))
            {
                button.onClick.AddListener(() => OnPositionClicked(button, positionKey));
            }
            else
            {
                Debug.LogError($"Invalid button name: {positionKey}");
            }
        }
    }

    void PlaceInitialTigers()
    {
        string[] tigerPositions = { "Tile_0_0", "Tile_0_4", "Tile_4_0", "Tile_4_4" };

        foreach (string position in tigerPositions)
        {
            if (boardPositions.ContainsKey(position))
            {
                string[] parts = position.Split('_');
                int x = int.Parse(parts[1]);
                int y = int.Parse(parts[2]);

                float tileSize = 200f;
                float boardStartX = -500f;
                float boardStartY = 500f;

                float tigerX = boardStartX + tileSize * x + tileSize / 2;
                float tigerY = boardStartY - tileSize * y - tileSize / 2;

                // Instantiate and place tiger
                GameObject tiger = Instantiate(tigerPrefab);
                tiger.transform.position = new Vector3(tigerX, tigerY, -1f);

                // Set sorting layer for visibility
                SpriteRenderer tigerRenderer = tiger.GetComponent<SpriteRenderer>();
                if (tigerRenderer != null)
                {
                    tigerRenderer.sortingLayerName = "Animals";
                    tigerRenderer.sortingOrder = 1;
                }

                tigers.Add(tiger);
                boardPositions[position] = tiger;
            }
            else
            {
                Debug.LogError($"Position {position} not found in boardPositions!");
            }
        }
    }
    #endregion

    #region Turn Handling
    public void OnPositionClicked(Button button, string positionKey)
    {
        if (!boardPositions.ContainsKey(positionKey))
        {
            Debug.LogError($"Invalid position: {positionKey}");
            return;
        }

        if (boardPositions[positionKey] != null)
        {
            Debug.Log("Tile occupied!");
            return;
        }

        if (isTigerTurn)
        {
            HandleTigerTurn(button, positionKey);
        }
        else
        {
            if (isPlacementPhase)
            {
                PlaceGoat(button, positionKey);
            }
            else
            {
                HandleGoatMovement(button, positionKey);
            }
        }

        CheckWinConditions();
    }
    #endregion

    #region Tiger Logic
    void HandleTigerTurn(Button button, string positionKey)
    {
        if (selectedTiger == null)
        {
            GameObject piece = boardPositions[GetPositionKeyOfPiece(button.gameObject)];
            if (piece != null && piece.CompareTag("Tiger"))
            {
                selectedTiger = piece;
                Debug.Log("Tiger selected: " + selectedTiger.name);
            }
        }
        else
        {
            string currentPos = GetPositionKeyOfPiece(selectedTiger);
            if (IsValidTigerMove(currentPos, positionKey))
            {
                if (IsCaptureMove(currentPos, positionKey))
                {
                    CaptureGoat(currentPos, positionKey);
                    goatsCaptured++;
                }
                MoveTiger(button, positionKey);
                isTigerTurn = false;
                selectedTiger = null;
            }
            else Debug.Log("Invalid tiger move!");
        }
    }

    void MoveTiger(Button button, string positionKey)
    {
        selectedTiger.transform.position = button.transform.position;
        boardPositions[GetPositionKeyOfPiece(selectedTiger)] = null; // Clear old position
        boardPositions[positionKey] = selectedTiger;
    }

    bool IsValidTigerMove(string fromPos, string toPos)
    {
        List<string> adjacent = GetValidAdjacentPositions(fromPos);
        if (adjacent.Contains(toPos) && boardPositions[toPos] == null) return true;
        else return IsCaptureMove(fromPos, toPos);
    }

    bool IsCaptureMove(string fromPos, string toPos)
    {
        string[] fromParts = fromPos.Split('_');
        int fromX = int.Parse(fromParts[1]), fromY = int.Parse(fromParts[2]);
        string[] toParts = toPos.Split('_');
        int toX = int.Parse(toParts[1]), toY = int.Parse(toParts[2]);

        int dx = toX - fromX, dy = toY - fromY;
        if (Mathf.Abs(dx) != 2 && Mathf.Abs(dy) != 2) return false;

        int midX = (fromX + toX) / 2, midY = (fromY + toY) / 2;
        string midPos = $"Tile_{midX}_{midY}";
        return boardPositions.ContainsKey(midPos) &&
               boardPositions[midPos] != null &&
               boardPositions[midPos].CompareTag("Goat");
    }

    void CaptureGoat(string fromPos, string toPos)
    {
        string[] fromParts = fromPos.Split('_');
        int fromX = int.Parse(fromParts[1]), fromY = int.Parse(fromParts[2]);
        string[] toParts = toPos.Split('_');
        int toX = int.Parse(toParts[1]), toY = int.Parse(toParts[2]);

        int midX = (fromX + toX) / 2, midY = (fromY + toY) / 2;
        string midPos = $"Tile_{midX}_{midY}";
        Destroy(boardPositions[midPos]);
        boardPositions[midPos] = null;
    }
    #endregion

    #region Goat Logic
    void PlaceGoat(Button button, string positionKey)
    {
        GameObject goat = Instantiate(goatPrefab);
        goat.transform.position = button.transform.position;
        boardPositions[positionKey] = goat;
        placedGoats++;

        if (placedGoats >= totalGoats) isPlacementPhase = false;

        isTigerTurn = true; // Switch to tiger's turn
    }

    void HandleGoatMovement(Button button, string positionKey)
    {
        if (selectedGoat == null)
        {
            GameObject piece = boardPositions[GetPositionKeyOfPiece(button.gameObject)];
            if (piece != null && piece.CompareTag("Goat")) selectedGoat = piece;
        }
        else
        {
            string currentPos = GetPositionKeyOfPiece(selectedGoat);
            if (IsValidGoatMove(currentPos, positionKey))
            {
                MoveGoat(button, positionKey);
                isTigerTurn = true;
                selectedGoat = null;
            }
            else Debug.Log("Invalid goat move!");
        }
    }

    void MoveGoat(Button button, string positionKey)
    {
        string currentPos = GetPositionKeyOfPiece(selectedGoat);
        boardPositions[currentPos] = null;
        selectedGoat.transform.position = button.transform.position;
        boardPositions[positionKey] = selectedGoat;
    }

    bool IsValidGoatMove(string fromPos, string toPos)
    {
        List<string> adjacent = GetValidAdjacentPositions(fromPos);
        return adjacent.Contains(toPos) && boardPositions[toPos] == null;
    }
    #endregion

    #region Helpers
    string GetPositionKeyOfPiece(GameObject piece)
    {
        foreach (var entry in boardPositions)
        {
            if (entry.Value == piece) return entry.Key;
        }
        return null;
    }

    List<string> GetValidAdjacentPositions(string currentPos)
    {
        List<string> adjacent = new List<string>();
        string[] parts = currentPos.Split('_');
        int x = int.Parse(parts[1]), y = int.Parse(parts[2]);

        // Orthogonal moves
        AddIfValid(adjacent, x - 1, y);
        AddIfValid(adjacent, x + 1, y);
        AddIfValid(adjacent, x, y - 1);
        AddIfValid(adjacent, x, y + 1);

        // Diagonal moves
        AddIfValid(adjacent, x - 1, y - 1);
        AddIfValid(adjacent, x - 1, y + 1);
        AddIfValid(adjacent, x + 1, y - 1);
        AddIfValid(adjacent, x + 1, y + 1);

        return adjacent;
    }

    void AddIfValid(List<string> list, int x, int y)
    {
        if (x >= 0 && x < 5 && y >= 0 && y < 5)
        {
            string key = $"Tile_{x}_{y}";
            if (boardPositions.ContainsKey(key)) list.Add(key);
        }
    }

    void CheckWinConditions()
    {
        if (goatsCaptured >= 5) Debug.Log("Tigers win!");
        else if (placedGoats == totalGoats && !IsAnyValidTigerMove()) Debug.Log("Goats win!");
    }

    bool IsAnyValidTigerMove()
    {
        foreach (GameObject tiger in tigers)
        {
            string currentPos = GetPositionKeyOfPiece(tiger);
            if (currentPos == null) continue;

            foreach (string adjacent in GetValidAdjacentPositions(currentPos))
            {
                if (boardPositions[adjacent] == null || IsCaptureMove(currentPos, adjacent)) return true;
            }
        }
        return false;
    }
    #endregion
}
