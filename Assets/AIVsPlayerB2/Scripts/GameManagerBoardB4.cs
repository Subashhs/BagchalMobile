// GameManagerBoard3.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq; // For LINQ operations

public class GameManagerBoard5 : MonoBehaviour
{
    public static GameManagerBoard5 Instance { get; private set; }
    public static bool PlayerIsTiger { get; set; } // Static property to store player's choice

    public enum Turn { Player, AI }
    public Turn currentTurn;

    public TMP_Text turnText;
    public TMP_Text winText;

    public GameObject tigerPrefab; // Assign your Tiger prefab in the Inspector
    public GameObject goatPrefab;  // Assign your Goat prefab in the Inspector

    public GameObject playerTiger;
    public List<GameObject> playerGoats = new List<GameObject>();
    private GameObject aiTiger;
    private List<GameObject> aiGoats = new List<GameObject>();

    private BaseAI2 tigerAI; // AI for controlling the Tiger
    private BaseAI2 goatAI;  // AI for controlling the Goats

    public GameObject selectedPiece; // Can be either a tiger or a goat
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>(); // Populate in InitializeBoard

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes
            Debug.Log("GameManager: Instance created.");
        }
        else
        {
            Debug.Log("GameManager: Instance already exists, destroying new one.");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Debug.Log($"GameManager: Start called. PlayerIsTiger is {PlayerIsTiger}"); // Added logging
        InitializeBoard();
        SetInitialTurn();
        UpdateTurnText(); // Call after definition
    }

    private void SetInitialTurn()
    {
        Debug.Log("GameManager: SetInitialTurn called."); // Added logging
        if (PlayerIsTiger)
        {
            currentTurn = Turn.Player; // Player (Tiger) starts
            Debug.Log($"GameManager: Initial turn set to Player (Tiger).");
        }
        else
        {
            currentTurn = Turn.AI; // AI (Tiger) starts
            Debug.Log($"GameManager: Initial turn set to AI (Tiger).");
            Invoke("MakeAIMove", 1f);
        }
    }

    private void InitializeBoard()
    {
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2" };

        Debug.Log("GameManager: Starting tile finding in InitializeBoard.");
        foreach (string tileName in tileNames)
        {
            GameObject tile = GameObject.Find(tileName); // Ensure your tile GameObjects in the scene are named correctly
            if (tile != null)
            {
                tiles[tileName] = tile;
                Debug.Log($"Tile {tileName} found: {tile.name}");
            }
            else
            {
                Debug.LogError($"Tile {tileName} not found!");
            }
        }
        Debug.Log("GameManager: Finished tile finding in InitializeBoard.");

        Debug.Log("GameManager: Starting Tiger/Goat instantiation in InitializeBoard.");
        if (PlayerIsTiger)
        {
            Debug.Log("GameManager: Player is Tiger, instantiating Tiger and AI Goats.");
            if (tiles.ContainsKey("Tile_0_0") && tigerPrefab != null)
            {
                playerTiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
                playerTiger.AddComponent<NewTigerSelectionB5>();
                playerTiger.GetComponent<TigerMovementB5>().enabled = true;
                Debug.Log($"GameManager: Player Tiger instantiated: {playerTiger.name}");
            }
            else
            {
                Debug.LogError($"GameManager: Could not instantiate Player Tiger. Tile exists: {tiles.ContainsKey("Tile_0_0")}, Prefab exists: {tigerPrefab != null}");
            }
            string[] aiGoatPositions = { "Tile_3_0", "Tile_3_1", "Tile_3_2" };
            List<GameObject> tempAIGoats = new List<GameObject>();
            foreach (string position in aiGoatPositions)
            {
                if (tiles.ContainsKey(position) && goatPrefab != null)
                {
                    GameObject goat = Instantiate(goatPrefab, tiles[position].transform.position, Quaternion.identity);
                    goat.GetComponent<GoatMovementB5>().enabled = true;
                    tempAIGoats.Add(goat);
                    Debug.Log($"GameManager: AI Goat instantiated at {position}: {goat.name}");
                }
                else
                {
                    Debug.LogError($"GameManager: Could not instantiate AI Goat at {position}. Tile exists: {tiles.ContainsKey(position)}, Prefab exists: {goatPrefab != null}");
                }
            }
            aiGoats = tempAIGoats;
            goatAI = new GoatAI2(aiGoats.ToArray(), playerTiger, tiles); // AI controls Goats
            Debug.Log("GameManager: GoatAI instantiated (controlling AI Goats).");
        }
        else
        {
            Debug.Log("GameManager: Player is Goat, instantiating AI Tiger and Player Goats.");
            if (tiles.ContainsKey("Tile_0_0") && tigerPrefab != null)
            {
                aiTiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
                aiTiger.GetComponent<TigerMovementB5>().enabled = true;
                Debug.Log($"GameManager: AI Tiger instantiated: {aiTiger.name}");
            }
            else
            {
                Debug.LogError($"GameManager: Could not instantiate AI Tiger. Tile exists: {tiles.ContainsKey("Tile_0_0")}, Prefab exists: {tigerPrefab != null}");
            }
            string[] playerGoatPositions = { "Tile_3_0", "Tile_3_1", "Tile_3_2" };
            List<GameObject> tempPlayerGoats = new List<GameObject>();
            foreach (string position in playerGoatPositions)
            {
                if (tiles.ContainsKey(position) && goatPrefab != null)
                {
                    GameObject goat = Instantiate(goatPrefab, tiles[position].transform.position, Quaternion.identity);
                    goat.AddComponent<NewGoatSelectionB5>();
                    goat.GetComponent<GoatMovementB5>().enabled = true;
                    tempPlayerGoats.Add(goat);
                    Debug.Log($"GameManager: Player Goat instantiated at {position}: {goat.name}");
                }
                else
                {
                    Debug.LogError($"GameManager: Could not instantiate Player Goat at {position}. Tile exists: {tiles.ContainsKey(position)}, Prefab exists: {goatPrefab != null}");
                }
            }
            playerGoats = tempPlayerGoats;
            tigerAI = new TigerAI2(aiTiger, playerGoats.ToArray(), tiles); // AI controls Tiger
            Debug.Log("GameManager: TigerAI instantiated (controlling AI Tiger).");
        }
        Debug.Log("GameManager: Finished Tiger/Goat instantiation in InitializeBoard.");
    }

    public void SelectPiece(GameObject piece)
    {
        if (currentTurn == Turn.Player)
        {
            if (PlayerIsTiger && piece == playerTiger)
            {
                selectedPiece = piece;
                Debug.Log($"GameManager: Player Tiger selected: {piece.name}");
            }
            else if (!PlayerIsTiger && playerGoats.Contains(piece))
            {
                selectedPiece = piece;
                Debug.Log($"GameManager: Player Goat selected: {piece.name}");
            }
            else
            {
                Debug.LogWarning($"GameManager: Invalid selection for the current player's turn: {piece.name}");
                selectedPiece = null;
            }
        }
        else
        {
            Debug.LogWarning("GameManager: It's the AI's turn. Player cannot select a piece.");
            selectedPiece = null;
        }
    }

    public bool IsPieceSelected()
    {
        return selectedPiece != null;
    }

    public void MovePiece(GameObject targetTile)
    {
        if (currentTurn == Turn.Player && selectedPiece != null && tiles.ContainsValue(targetTile))
        {
            bool moved = false;
            if (PlayerIsTiger && selectedPiece == playerTiger)
            {
                TigerMovementB5 tigerMovement = playerTiger.GetComponent<TigerMovementB5>();
                if (tigerMovement != null)
                {
                    moved = tigerMovement.TryMove(playerTiger, targetTile, tiles, aiGoats);
                    if (moved)
                    {
                        selectedPiece = null;
                        currentTurn = Turn.AI;
                        Debug.Log($"GameManager: Turn changed to AI (Goats) after player tiger move.");
                        CheckForWinCondition();
                        UpdateTurnText();
                        Invoke("MakeAIMove", 1f);
                    }
                    else
                    {
                        Debug.LogWarning("GameManager: Invalid move for the Player Tiger.");
                    }
                }
            }
            else if (!PlayerIsTiger && playerGoats.Contains(selectedPiece))
            {
                GoatMovementB5 goatMovement = selectedPiece.GetComponent<GoatMovementB5>();
                if (goatMovement != null)
                {
                    moved = goatMovement.TryMove(selectedPiece, targetTile, tiles);
                    if (moved)
                    {
                        selectedPiece = null;
                        currentTurn = Turn.AI;
                        Debug.Log($"GameManager: Turn changed to AI (Tiger) after player goat move.");
                        CheckForWinCondition();
                        UpdateTurnText();
                        Invoke("MakeAIMove", 1f);
                    }
                    else
                    {
                        Debug.LogWarning("GameManager: Invalid move for the Player Goat.");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("GameManager: Invalid move attempt.");
        }
    }

    private void MakeAIMove()
    {
        Debug.Log("GameManager: MakeAIMove function called.");
        Debug.Log($"MakeAIMove: Current Turn is {currentTurn}, PlayerIsTiger is {PlayerIsTiger}");
        if (currentTurn == Turn.AI)
        {
            Debug.Log("GameManager: AI's turn to move.");
            bool moved = false;
            if (PlayerIsTiger && aiGoats.Count > 0 && goatAI != null)
            {
                // AI controls Goats
                Debug.Log("GameManager (MakeAIMove): AI controlling Goats.");
                moved = goatAI.MakeMove();
                if (moved)
                {
                    currentTurn = Turn.Player;
                    Debug.Log($"GameManager: Turn changed to Player (Tiger) after AI goat move.");
                    CheckForWinCondition();
                    UpdateTurnText();
                }
                else
                {
                    Debug.LogError("GameManager: AI Goats could not make a move!");
                    currentTurn = Turn.Player; // To prevent infinite AI turn
                    UpdateTurnText();
                }
            }
            else if (!PlayerIsTiger && aiTiger != null && tigerAI != null)
            {
                // AI controls Tiger
                Debug.Log("GameManager (MakeAIMove): AI controlling Tiger.");
                Debug.Log($"GameManager (MakeAIMove): aiTiger is {aiTiger.name}, tigerAI is not null: {tigerAI != null}");
                moved = tigerAI.MakeMove();
                if (moved)
                {
                    currentTurn = Turn.Player;
                    Debug.Log($"GameManager: Turn changed to Player (Goats) after AI tiger move.");
                    CheckForWinCondition();
                    UpdateTurnText();
                }
                else
                {
                    Debug.LogError("GameManager: AI Tiger could not make a move!");
                    currentTurn = Turn.Player; // To prevent infinite AI turn
                    UpdateTurnText();
                }
            }
            else
            {
                string conditions = $"PlayerIsTiger: {PlayerIsTiger}, aiGoats.Count > 0: {aiGoats.Count > 0}, goatAI != null: {goatAI != null}, !PlayerIsTiger: {!PlayerIsTiger}, aiTiger != null: {aiTiger != null}, tigerAI != null: {tigerAI != null}";
                
            }
        }
        else
        {
            Debug.Log("GameManager: It's not the AI's turn.");
        }
    }

    private void CheckForWinCondition()
    {
        int currentPlayerGoatCount = PlayerIsTiger ? aiGoats.Count : playerGoats.Count;
        GameObject currentTiger = PlayerIsTiger ? playerTiger : aiTiger;

        // Tiger wins if goats count is less than 3.
        if (currentPlayerGoatCount < 3)
        {
            winText.text = (PlayerIsTiger) ? "TIGER WINS! CAPTURED A GOAT!" : "AI TIGER WINS! CAPTURED A GOAT!";
            Invoke("ReturnToOptionBoard", 3f);
            return;
        }

        // Goats win if tiger is blocked.
        if (IsTigerBlocked(currentTiger))
        {
            winText.text = (PlayerIsTiger) ? "AI GOATS WIN! TIGER IS BLOCKED!" : "GOATS WIN! TIGER IS BLOCKED!";
            Invoke("ReturnToOptionBoard", 3f);
        }
    }

    private bool IsTigerBlocked(GameObject tigerToCheck)
    {
        TigerMovementB5 tigerMovement = tigerToCheck.GetComponent<TigerMovementB5>();
        if (tigerMovement == null) return false;

        string currentTigerTile = tigerMovement.GetTileName(tigerToCheck.transform.position, tiles);
        if (!tigerMovement.validMoves.ContainsKey(currentTigerTile))
        {
            return false; // Tiger is not on a valid tile, so can't be blocked.
        }

        foreach (string possibleMove in tigerMovement.validMoves[currentTigerTile])
        {
            GameObject targetTile = tiles[possibleMove];
            if (!IsTileOccupied(targetTile))
            {
                return false; // Tiger has at least one valid move.
            }
        }
        return true; // Tiger is completely blocked.
    }

    public bool IsTileOccupied(GameObject tile)
    {
        Vector3 tilePos = tile.transform.position;
        if (playerTiger != null && Vector3.Distance(playerTiger.transform.position, tilePos) < 0.1f) return true;
        if (aiTiger != null && Vector3.Distance(aiTiger.transform.position, tilePos) < 0.1f) return true;
        foreach (var goat in playerGoats)
        {
            if (goat != null && Vector3.Distance(goat.transform.position, tilePos) < 0.1f) return true;
        }
        foreach (var goat in aiGoats)
        {
            if (goat != null && Vector3.Distance(goat.transform.position, tilePos) < 0.1f) return true;
        }
        return false;
    }

    public string GetTileName(Vector3 position)
    {
        foreach (var tile in tiles)
        {
            if (Vector3.Distance(tile.Value.transform.position, position) < 0.1f)
            {
                return tile.Key;
            }
        }
        Debug.LogError("Position does not match any tile.");
        return string.Empty;
    }

    public GameObject GetTileByName(string tileName)
    {
        if (tiles.ContainsKey(tileName))
        {
            return tiles[tileName];
        }
        return null;
    }

    public List<GameObject> GetAllGoats()
    {
        return PlayerIsTiger ? aiGoats : playerGoats;
    }

    public GameObject GetTiger()
    {
        return PlayerIsTiger ? playerTiger : aiTiger;
    }

    public List<GameObject> GetPlayerGoats()
    {
        return !PlayerIsTiger ? playerGoats : new List<GameObject>();
    }

    public void RemoveGoat(GameObject goatToRemove)
    {
        if (PlayerIsTiger)
        {
            aiGoats.Remove(goatToRemove);
        }
        else
        {
            playerGoats.Remove(goatToRemove);
        }
    }

    public void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = (currentTurn == Turn.Player) ? "YOUR MOVE" : "AI'S MOVE";
        }
    }

    private void ReturnToOptionBoard()
    {
        Debug.Log("GameManager: Returning to Option Board.");
        Destroy(gameObject); // Clean up GameManager instance
        SceneManager.LoadScene("OptionBoard");
    }
}