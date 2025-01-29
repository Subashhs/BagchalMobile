using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text turnIndicator;
    public TMP_Text goatsCapturedText;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

<<<<<<< HEAD
    private Dictionary<string, GameObject> boardPositions = new Dictionary<string, GameObject>();
    private int totalGoats = 20;
    private int placedGoats = 0;
    private bool isTigerTurn = false;
    private bool isPlacementPhase = true;
    private GameObject selectedTiger = null;
    private GameObject selectedGoat = null;
    private List<GameObject> tigers = new List<GameObject>();
    private int goatsCaptured = 0;
=======
    public List<GameObject> boardTiles = new List<GameObject>();

    private bool isTigerSelected = false;
    private GameObject selectedTiger = null;
    private GameObject selectedGoat = null;
>>>>>>> 213ef0d5d2081dc2e39925bba63e6569fc176e18

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;  // Goat starts first

    private List<GameObject> placedGoats = new List<GameObject>();
    private int totalGoatsPlaced = 0;  // Track number of goats placed

    private void Awake()
    {
<<<<<<< HEAD
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
=======
        if (Instance == null)
        {
            Instance = this;
>>>>>>> 213ef0d5d2081dc2e39925bba63e6569fc176e18
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PopulateBoardTiles();
        UpdateTurnDisplay();
        PlaceTigers(); // Place tigers on the board at the start
    }

    private void PopulateBoardTiles()
    {
        boardTiles.AddRange(GameObject.FindGameObjectsWithTag("BoardTile"));
    }

    private void UpdateTurnDisplay()
    {
        turnIndicator.text = currentTurn == Turn.Tiger ? "Tiger's Turn" : "Goat's Turn";
    }

    // Place the tigers at the four corners at the start of the game
    private void PlaceTigers()
    {
        Vector3[] tigerStartPositions = new Vector3[]
        {
            boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position,
            boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position,
            boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position,
            boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position
        };

        foreach (Vector3 position in tigerStartPositions)
        {
            Instantiate(tigerPrefab, position, Quaternion.identity);
        }
    }

    // Select a tiger to move
    public void SelectTiger(GameObject tiger)
    {
        if (currentTurn == Turn.Tiger && !isTigerSelected)
        {
            selectedTiger = tiger;
            isTigerSelected = true;
        }
    }

    // Move the selected tiger to the new tile
    public void MoveTiger(GameObject destinationTile)
    {
        if (isTigerSelected)
        {
            if (IsValidMove(selectedTiger, destinationTile))
            {
                selectedTiger.transform.position = destinationTile.transform.position;
                DeselectTiger();
                SwitchTurn();
            }
            else
            {
                Debug.Log("Invalid move");
            }
        }
    }

    // Select a goat to place on the board (Goat Placement)
    public void PlaceGoat(GameObject destinationTile)
    {
<<<<<<< HEAD
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
=======
        if (currentTurn == Turn.Goat && totalGoatsPlaced < 20)
        {
            if (IsTileAvailable(destinationTile))
            {
                GameObject goat = Instantiate(goatPrefab, destinationTile.transform.position, Quaternion.identity);
                placedGoats.Add(goat);
                totalGoatsPlaced++;
                SwitchTurn();
            }
            else
>>>>>>> 213ef0d5d2081dc2e39925bba63e6569fc176e18
            {
                Debug.Log("Tile is already occupied by a tiger or another goat.");
            }
        }

        if (totalGoatsPlaced >= 20)
        {
            // Once 20 goats are placed, allow goat movement
            currentTurn = Turn.Goat;
            UpdateTurnDisplay();
        }
    }

    // Check if the tile is available for placing a goat
    private bool IsTileAvailable(GameObject tile)
    {
        // Check if tile is already occupied by a tiger or another goat
        foreach (var goat in placedGoats)
        {
            if (goat.transform.position == tile.transform.position)
                return false;
        }

        foreach (var tiger in GameObject.FindGameObjectsWithTag("Tiger"))
        {
            if (tiger.transform.position == tile.transform.position)
                return false;
        }

        return true;
    }

    // After the tiger moves, deselect it
    private void DeselectTiger()
    {
        isTigerSelected = false;
        selectedTiger = null;
    }

    // Switch turns between Tiger and Goat
    private void SwitchTurn()
    {
        currentTurn = currentTurn == Turn.Tiger ? Turn.Goat : Turn.Tiger;
        UpdateTurnDisplay();
    }

    // Check if the move for the tiger is valid
    private bool IsValidMove(GameObject tiger, GameObject destinationTile)
    {
        // For simplicity, let's assume any move is valid for now
        return true;
    }

    // Start Goat movement after all goats are placed
    public void MoveGoat(GameObject destinationTile)
    {
        if (currentTurn == Turn.Goat && selectedGoat != null)
        {
            if (IsTileAvailable(destinationTile))
            {
                selectedGoat.transform.position = destinationTile.transform.position;
                selectedGoat = null;  // Deselect the goat after move
                SwitchTurn();  // Switch turn to the tiger
            }
<<<<<<< HEAD
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
=======
            else
            {
                Debug.Log("Tile is already occupied by a tiger or another goat.");
            }
        }
>>>>>>> 213ef0d5d2081dc2e39925bba63e6569fc176e18
    }

    // Select a goat for moving (only after 20 goats are placed)
    public void SelectGoat(GameObject goat)
    {
<<<<<<< HEAD
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
=======
        if (currentTurn == Turn.Goat && totalGoatsPlaced >= 20 && selectedGoat == null)
        {
            selectedGoat = goat;
        }
    }

    // Getter methods for board and placed goats
    public List<GameObject> GetBoardTiles()
    {
        return boardTiles;
    }

    public List<GameObject> GetPlacedGoats()
    {
        return placedGoats;
    }

    // Is a tiger currently selected?
    public bool IsTigerSelected()
    {
        return isTigerSelected;
    }

    // Get the currently selected goat
    public GameObject GetSelectedGoat()
    {
        return selectedGoat;
    }
>>>>>>> 213ef0d5d2081dc2e39925bba63e6569fc176e18
}
