using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerBoard4 : MonoBehaviour
{
    public static GameManagerBoard4 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat; // Start with Goat's turn

    public TMP_Text turnText;
    public TMP_Text winText;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
    public GameObject tiger;
    public List<GameObject> goats = new List<GameObject>();

    private GoatMovementB4 goatMovement;
    private TigerMovementB4 tigerMovement;

    public Turn playerRole;
    public GameObject playAsTigerButton;
    public GameObject playAsGoatButton;
    public GameObject gameBoardParent;

    public AiPlayerB4 aiPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        goatMovement = goatPrefab.GetComponent<GoatMovementB4>();
        tigerMovement = tigerPrefab.GetComponent<TigerMovementB4>();
    }

    private void Start()
    {
        // AI vs. Player scene logic
        Debug.Log("GameManagerBoard3: Start - AI vs. Player scene detected.");
        gameBoardParent.SetActive(false);
        playAsTigerButton.SetActive(true);
        playAsGoatButton.SetActive(true);

        // Error check
        if (playAsTigerButton != null)
        {
            playAsTigerButton.GetComponent<Button>().onClick.AddListener(OnPlayAsTigerButtonClick);
        }
        else
        {
            Debug.LogError("playAsTigerButton is null. Make sure it is assigned in the inspector.");
        }

        if (playAsGoatButton != null)
        {
            playAsGoatButton.GetComponent<Button>().onClick.AddListener(OnPlayAsGoatButtonClick);
        }
        else
        {
            Debug.LogError("playAsGoatButton is null. Make sure it is assigned in the inspector.");
        }
        if (aiPlayer == null)
        {
            aiPlayer = GetComponent<AiPlayerB4>();
            if (aiPlayer == null)
                Debug.LogError("aiPlayer is null. Make sure it is assigned in the inspector.");
        }
    }

    public void OnPlayAsTigerButtonClick()
    {
        playerRole = Turn.Tiger;
        aiPlayer.aiPlayerTurn = Turn.Goat;
        StartGame();
        Debug.Log("GameManagerBoard3: OnPlayAsTigerButtonClick");
        Debug.Log($"Player Role set to: {playerRole}");
    }

    public void OnPlayAsGoatButtonClick()
    {
        playerRole = Turn.Goat;
        aiPlayer.aiPlayerTurn = Turn.Tiger;
        StartGame();
        Debug.Log("GameManagerBoard3: OnPlayAsGoatButtonClick");
        Debug.Log($"Player Role set to: {playerRole}");
    }

    private void StartGame()
    {
        Debug.Log("GameManagerBoard3: StartGame");
        Debug.Log("playAsTigerButton: " + (playAsTigerButton != null ? "Assigned" : "Null"));
        Debug.Log("playAsGoatButton: " + (playAsGoatButton != null ? "Assigned" : "Null"));
        Debug.Log("gameBoardParent: " + (gameBoardParent != null ? "Assigned" : "Null"));
        Debug.Log("aiPlayer: " + (aiPlayer != null ? "Assigned" : "Null"));
        Debug.Log("turnText: " + (turnText != null ? "Assigned" : "Null"));
        Debug.Log("winText: " + (winText != null ? "Assigned" : "Null"));
        Debug.Log("tigerPrefab: " + (tigerPrefab != null ? "Assigned" : "Null"));
        Debug.Log("goatPrefab: " + (goatPrefab != null ? "Assigned" : "Null"));

        playAsTigerButton.SetActive(false);
        playAsGoatButton.SetActive(false);
        gameBoardParent.SetActive(true);
        InitializeBoard();
        UpdateTurnText();

        Debug.Log($"Player Role: {playerRole}, Current Turn: {currentTurn}");

        // Start AI turn if it's AI's turn at the beginning of the game.
        if (currentTurn == aiPlayer.aiPlayerTurn && aiPlayer != null)
        {
            aiPlayer.MakeAiMove();
        }
    }

    private void InitializeBoard()
    {
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2", "Tile_4_0", "Tile_4_1", "Tile_4_2" };

        foreach (string tileName in tileNames)
        {
            GameObject tile = GameObject.Find(tileName);
            if (tile != null)
            {
                tiles[tileName] = tile;
                Debug.Log($"Tile {tileName} found.");
            }
            else
            {
                Debug.LogError($"Tile {tileName} not found!");
            }
        }

        if (tiles.ContainsKey("Tile_0_0") && tigerPrefab != null)
        {
            tiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
            tiger.tag = "Tiger"; // Add tag
            Debug.Log("Tiger instantiated.");
            Debug.Log($"Tiger gameobject name is: {tiger.name}");
        }

        string[] goatPositions = { "Tile_4_0", "Tile_4_1", "Tile_4_2" };
        for (int i = 0; i < goatPositions.Length; i++)
        {
            if (tiles.ContainsKey(goatPositions[i]) && goatPrefab != null)
            {
                GameObject goat = Instantiate(goatPrefab, tiles[goatPositions[i]].transform.position, Quaternion.identity);
                goat.tag = "Goat"; // Add tag
                goats.Add(goat);
                Debug.Log($"Goat instantiated at {goatPositions[i]}.");
                Debug.Log($"Goat gameobject name is: {goat.name}");
            }
        }
    }

    public void SelectPiece(GameObject piece)
    {
        Debug.Log($"SelectPiece called for: {piece.name} from {GetCallingMethod()}");
        Debug.Log($"Current Turn: {currentTurn}, Player Role: {playerRole}");

        if (currentTurn == playerRole)
        {
            if (currentTurn == Turn.Tiger && piece.tag == "Tiger")
            {
                selectedTiger = piece;
                Debug.Log("Tiger selected.");
            }
            else if (currentTurn == Turn.Goat && piece.tag == "Goat")
            {
                selectedGoat = piece;
                Debug.Log("Goat selected.");
            }
            else
            {
                Debug.LogWarning("Invalid selection for the current turn.");
            }
        }
        else
        {
            Debug.LogWarning("It is not your turn");
        }
    }

    public bool IsPieceSelected()
    {
        return selectedTiger != null || selectedGoat != null;
    }

    public void MovePiece(GameObject tile)
    {
        Debug.Log($"Attempting to move piece to tile: {tile.name}, Current Turn: {currentTurn}");

        bool moveSuccessful = false;

        if (selectedTiger != null && currentTurn == Turn.Tiger && tiles.ContainsValue(tile))
        {
            if (tigerMovement.TryMove(selectedTiger, tile, tiles))
            {
                selectedTiger = null;
                currentTurn = Turn.Goat;
                moveSuccessful = true;
            }
            else
            {
                Debug.LogWarning("Invalid move for the Tiger.");
            }
        }
        else if (selectedGoat != null && currentTurn == Turn.Goat && tiles.ContainsValue(tile))
        {
            if (goatMovement.TryMove(selectedGoat, tile, tiles))
            {
                selectedGoat.transform.position = tile.transform.position;
                selectedGoat = null;
                currentTurn = Turn.Tiger;
                moveSuccessful = true;
            }
            else
            {
                Debug.LogWarning("Invalid move for the Goat.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid move attempt.");
        }

        if (moveSuccessful)
        {
            CheckForWinCondition();
            UpdateTurnText();

            Debug.Log($"Turn switched to: {currentTurn}");

            if (aiPlayer != null && currentTurn == aiPlayer.aiPlayerTurn)
            {
                aiPlayer.MakeAiMove();
            }
        }
    }

    public void CheckForWinCondition()
    {
        if (goats.Count < 3)
        {
            winText.text = "TIGER WINS! CAPTURED A GOAT!";
            Invoke("ReturnToOptionBoard", 3f);
            return;
        }

        if (IsTigerBlocked())
        {
            winText.text = "GOATS WIN! TIGER IS BLOCKED!";
            Invoke("ReturnToOptionBoard", 3f);
        }
    }

    private bool IsTigerBlocked()
    {
        string currentTigerTile = tigerMovement.GetTileName(tiger.transform.position, tiles);
        if (!tigerMovement.validMoves.ContainsKey(currentTigerTile))
        {
            return false;
        }

        foreach (string possibleMove in tigerMovement.validMoves[currentTigerTile])
        {
            GameObject targetTile = tiles[possibleMove];
            if (!tigerMovement.IsTileOccupied(targetTile))
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = (currentTurn == Turn.Tiger) ? "TIGER'S MOVE" : "GOAT'S MOVE";
        }
    }

    private void ReturnToOptionBoard()
    {
        SceneManager.LoadScene("OptionBoard");
    }

    private string GetCallingMethod()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame frame = stackTrace.GetFrame(2);
        if (frame != null)
        {
            return frame.GetMethod().Name;
        }
        return "Unknown";
    }
}