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
    private GameObject selectedTiger = null;
    private List<GameObject> tigers = new List<GameObject>();
    private int goatsCaptured = 0;
    private bool isPlacementPhase = true;
    private GameObject selectedGoat = null;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        InitializeBoard();
        PlaceInitialTigers();
        isTigerTurn = false; // Goat starts
    }

    #region Board Initialization
    void InitializeBoard()
    {
        // Initialize all 25 positions (5x5 grid)
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                string key = $"Tile_{x}_{y}";
                boardPositions[key] = null;
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
            Button button = GameObject.Find(position)?.GetComponent<Button>();
            if (button != null)
            {
                GameObject tiger = Instantiate(tigerPrefab, button.transform.position, Quaternion.identity);
                tiger.transform.position = new Vector3(tiger.transform.position.x, tiger.transform.position.y, -1f);
                SpriteRenderer tigerRenderer = tiger.GetComponent<SpriteRenderer>();
                if (tigerRenderer != null)
                {
                    tigerRenderer.sortingLayerName = "Animals";
                    tigerRenderer.sortingOrder = 1;
                }
                tigers.Add(tiger);
                boardPositions[position] = tiger;
            }
            else Debug.LogError($"Button {position} not found!");
        }
    }
    #endregion

    #region Click Handling
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

        if (isTigerTurn) HandleTigerTurn(button, positionKey);
        else
        {
            if (isPlacementPhase) PlaceGoat(button, positionKey);
            else HandleGoatMovement(button, positionKey);
        }

        CheckWinConditions();
    }
    #endregion

    #region Tiger Logic
    void HandleTigerTurn(Button button, string positionKey)
    {
        if (selectedTiger == null)
        {
            // Select tiger
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

    bool IsValidTigerMove(string fromPos, string toPos)
    {
        List<string> adjacent = GetValidAdjacentPositions(fromPos);
        if (adjacent.Contains(toPos) && boardPositions[toPos] == null) return true;
        else return IsCaptureMove(fromPos, toPos);
    }

    void MoveTiger(Button button, string positionKey)
    {
        selectedTiger.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, -1f);
        boardPositions[GetPositionKeyOfPiece(selectedTiger)] = null; // Clear old position
        boardPositions[positionKey] = selectedTiger;
    }
    #endregion

    #region Goat Logic
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
                MoveGoat(selectedGoat, button, positionKey);
                isTigerTurn = true;
                selectedGoat = null;
            }
            else Debug.Log("Invalid goat move!");
        }
    }

    bool IsValidGoatMove(string fromPos, string toPos)
    {
        List<string> adjacent = GetValidAdjacentPositions(fromPos);
        return adjacent.Contains(toPos) && boardPositions[toPos] == null;
    }

    void PlaceGoat(Button button, string positionKey)
    {
        GameObject goat = Instantiate(goatPrefab, button.transform.position, Quaternion.identity);
        goat.transform.position = new Vector3(goat.transform.position.x, goat.transform.position.y, -1f);
        SpriteRenderer goatRenderer = goat.GetComponent<SpriteRenderer>();
        if (goatRenderer != null)
        {
            goatRenderer.sortingLayerName = "Animals";
            goatRenderer.sortingOrder = 1;
        }
        boardPositions[positionKey] = goat;
        placedGoats++;
        if (placedGoats >= totalGoats) isPlacementPhase = false;
    }

    void MoveGoat(GameObject goat, Button button, string positionKey)
    {
        string currentPos = GetPositionKeyOfPiece(goat);
        boardPositions[currentPos] = null;
        goat.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, -1f);
        boardPositions[positionKey] = goat;
    }
    #endregion

    #region Capture & Win Conditions
    bool IsCaptureMove(string fromPos, string toPos)
    {
        string[] fromParts = fromPos.Split('_');
        int fromX = int.Parse(fromParts[1]), fromY = int.Parse(fromParts[2]);
        string[] toParts = toPos.Split('_');
        int toX = int.Parse(toParts[1]), toY = int.Parse(toParts[2]);

        int dx = toX - fromX, dy = toY - fromY;
        if (Mathf.Abs(dx) != 2 && Mathf.Abs(dy) != 2 && Mathf.Abs(dx) != Mathf.Abs(dy)) return false;

        int midX = fromX + dx / 2, midY = fromY + dy / 2;
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

    void CheckWinConditions()
    {
        if (goatsCaptured >= 5) Debug.Log("Tigers win!");

        bool allBlocked = true;
        foreach (GameObject tiger in tigers)
        {
            string pos = GetPositionKeyOfPiece(tiger);
            foreach (string adjacent in GetValidAdjacentPositions(pos))
            {
                if (boardPositions[adjacent] == null) allBlocked = false;
            }
        }
        if (allBlocked) Debug.Log("Goats win!");
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
        if ((x == 0 && y == 0) || (x == 0 && y == 4) || (x == 4 && y == 0) || (x == 4 && y == 4))
            AddIfValid(adjacent, 2, 2);
        else if ((x == 2 && y == 0) || (x == 0 && y == 2) || (x == 4 && y == 2) || (x == 2 && y == 4))
            AddIfValid(adjacent, 2, 2);
        else if (x == 2 && y == 2)
        {
            AddIfValid(adjacent, 0, 0); AddIfValid(adjacent, 0, 4);
            AddIfValid(adjacent, 4, 0); AddIfValid(adjacent, 4, 4);
            AddIfValid(adjacent, 0, 2); AddIfValid(adjacent, 2, 0);
            AddIfValid(adjacent, 4, 2); AddIfValid(adjacent, 2, 4);
        }

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
    #endregion
}