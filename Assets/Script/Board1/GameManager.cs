using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text turnIndicator;
    public TMP_Text goatsCapturedText;
    public TMP_Text winText;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    public List<GameObject> boardTiles = new List<GameObject>();

    private bool isTigerSelected = false;
    private GameObject selectedTiger = null;
    private GameObject selectedGoat = null;

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;  // Goat starts first

    private List<GameObject> placedGoats = new List<GameObject>();
    private int totalGoatsPlaced = 0;  // Track number of goats placed
    private int goatsCaptured = 0;  // Track number of goats captured

    private void Awake()
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

    private void Start()
    {
        PopulateBoardTiles();
        UpdateTurnDisplay();
        PlaceTigers(); // Place tigers on the board at the start
        winText.gameObject.SetActive(false); // Hide the win text initially
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
            if (IsValidMove(selectedTiger, destinationTile, true))
            {
                Vector3 direction = destinationTile.transform.position - selectedTiger.transform.position;
                Vector3 midPoint = selectedTiger.transform.position + direction / 2;

                GameObject capturedGoat = GetGoatAtPosition(midPoint);
                if (capturedGoat != null)
                {
                    CaptureGoat(capturedGoat);
                }

                selectedTiger.transform.position = destinationTile.transform.position;
                DeselectTiger();
                CheckWinCondition();
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

    // Check if the move for the tiger or goat is valid
    private bool IsValidMove(GameObject piece, GameObject destinationTile, bool isTiger)
    {
        Vector3 piecePosition = piece.transform.position;
        Vector3 destinationPosition = destinationTile.transform.position;

        // Define valid moves based on the piece's current position
        switch (piecePosition)
        {
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position:
                return isTiger ? IsValidMoveFromTile_0_0(destinationPosition) : IsValidMoveFromTile_Goat_0_0(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position:
                return isTiger ? IsValidMoveFromTile_0_1(destinationPosition) : IsValidMoveFromTile_Goat_0_1(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position:
                return isTiger ? IsValidMoveFromTile_0_2(destinationPosition) : IsValidMoveFromTile_Goat_0_2(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position:
                return isTiger ? IsValidMoveFromTile_0_3(destinationPosition) : IsValidMoveFromTile_Goat_0_3(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position:
                return isTiger ? IsValidMoveFromTile_0_4(destinationPosition) : IsValidMoveFromTile_Goat_0_4(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position:
                return isTiger ? IsValidMoveFromTile_1_0(destinationPosition) : IsValidMoveFromTile_Goat_1_0(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position:
                return isTiger ? IsValidMoveFromTile_1_1(destinationPosition) : IsValidMoveFromTile_Goat_1_1(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position:
                return isTiger ? IsValidMoveFromTile_1_2(destinationPosition) : IsValidMoveFromTile_Goat_1_2(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position:
                return isTiger ? IsValidMoveFromTile_1_3(destinationPosition) : IsValidMoveFromTile_Goat_1_3(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position:
                return isTiger ? IsValidMoveFromTile_1_4(destinationPosition) : IsValidMoveFromTile_Goat_1_4(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position:
                return isTiger ? IsValidMoveFromTile_2_0(destinationPosition) : IsValidMoveFromTile_Goat_2_0(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position:
                return isTiger ? IsValidMoveFromTile_2_1(destinationPosition) : IsValidMoveFromTile_Goat_2_1(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position:
                return isTiger ? IsValidMoveFromTile_2_2(destinationPosition) : IsValidMoveFromTile_Goat_2_2(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position:
                return isTiger ? IsValidMoveFromTile_2_3(destinationPosition) : IsValidMoveFromTile_Goat_2_3(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position:
                return isTiger ? IsValidMoveFromTile_2_4(destinationPosition) : IsValidMoveFromTile_Goat_2_4(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position:
                return isTiger ? IsValidMoveFromTile_3_0(destinationPosition) : IsValidMoveFromTile_Goat_3_0(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position:
                return isTiger ? IsValidMoveFromTile_3_1(destinationPosition) : IsValidMoveFromTile_Goat_3_1(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position:
                return isTiger ? IsValidMoveFromTile_3_2(destinationPosition) : IsValidMoveFromTile_Goat_3_2(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position:
                return isTiger ? IsValidMoveFromTile_3_3(destinationPosition) : IsValidMoveFromTile_Goat_3_3(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position:
                return isTiger ? IsValidMoveFromTile_3_4(destinationPosition) : IsValidMoveFromTile_Goat_3_4(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position:
                return isTiger ? IsValidMoveFromTile_4_0(destinationPosition) : IsValidMoveFromTile_Goat_4_0(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position:
                return isTiger ? IsValidMoveFromTile_4_1(destinationPosition) : IsValidMoveFromTile_Goat_4_1(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position:
                return isTiger ? IsValidMoveFromTile_4_2(destinationPosition) : IsValidMoveFromTile_Goat_4_2(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position:
                return isTiger ? IsValidMoveFromTile_4_3(destinationPosition) : IsValidMoveFromTile_Goat_4_3(destinationPosition);
            case var pos when pos == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position:
                return isTiger ? IsValidMoveFromTile_4_4(destinationPosition) : IsValidMoveFromTile_Goat_4_4(destinationPosition);
            default:
                return false;
        }
    }

    // Define valid moves for each tile
    private bool IsValidMoveFromTile_0_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) == null);
    }

    private bool IsValidMoveFromTile_0_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) == null);
    }

    private bool IsValidMoveFromTile_0_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_0_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_0_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_1_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) == null);
    }

    private bool IsValidMoveFromTile_1_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_1_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_1_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_1_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_2_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position) == null);
    }

    private bool IsValidMoveFromTile_2_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position) == null);
    }

    private bool IsValidMoveFromTile_2_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) == null);
    }

    private bool IsValidMoveFromTile_2_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) == null);
    }

    private bool IsValidMoveFromTile_2_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null);
    }

    private bool IsValidMoveFromTile_3_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) == null);
    }

    private bool IsValidMoveFromTile_3_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_3_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_3_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_3_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_4_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) == null);
    }

    private bool IsValidMoveFromTile_4_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_4_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position) == null);
    }

    private bool IsValidMoveFromTile_4_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position) == null);
    }

    private bool IsValidMoveFromTile_4_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position) == null) ||
               (destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position) != null &&
                GetGoatAtPosition(boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position) == null);
    }

    // Capture a goat
    private void CaptureGoat(GameObject goat)
    {
        placedGoats.Remove(goat);
        Destroy(goat);
        goatsCaptured++;
        goatsCapturedText.text = "Goats Captured: " + goatsCaptured;
    }

    // Get the goat at a specific position
    private GameObject GetGoatAtPosition(Vector3 position)
    {
        foreach (var goat in placedGoats)
        {
            if (goat.transform.position == position)
                return goat;
        }
        return null;
    }

    // Check for win conditions
    private void CheckWinCondition()
    {
        if (goatsCaptured >= 5)
        {
            winText.text = "Tigers Win!";
            winText.gameObject.SetActive(true);
            // Handle win condition for tigers
        }

        if (AreAllTigersBlocked())
        {
            winText.text = "Goats Win!";
            winText.gameObject.SetActive(true);
            // Handle win condition for goats
        }
    }

    // Check if all tigers are blocked
    private bool AreAllTigersBlocked()
    {
        foreach (var tiger in GameObject.FindGameObjectsWithTag("Tiger"))
        {
            if (HasValidMoves(tiger))
                return false;
        }
        return true;
    }

    // Check if a tiger has any valid moves
    private bool HasValidMoves(GameObject tiger)
    {
        foreach (var tile in boardTiles)
        {
            if (IsValidMove(tiger, tile, true))
                return true;
        }
        return false;
    }

    // Start Goat movement after all goats are placed
    public void MoveGoat(GameObject destinationTile)
    {
        if (currentTurn == Turn.Goat && selectedGoat != null)
        {
            if (IsValidMove(selectedGoat, destinationTile, false))
            {
                selectedGoat.transform.position = destinationTile.transform.position;
                selectedGoat = null;  // Deselect the goat after move
                SwitchTurn();  // Switch turn to the tiger
            }
            else
            {
                Debug.Log("Invalid move");
            }
        }
    }

    // Select a goat for moving (only after 20 goats are placed)
    public void SelectGoat(GameObject goat)
    {
        if (currentTurn == Turn.Goat && totalGoatsPlaced >= 20 && selectedGoat == null)
        {
            selectedGoat = goat;
        }
    }

    // Define valid moves for each goat tile
    private bool IsValidMoveFromTile_Goat_0_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_0_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_0_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_0_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_0_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_1_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_1_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_1_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_1_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_1_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_0_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_2_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_2_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_2_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_2_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_2_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_1_3").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_3_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_3_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_3_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_3_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_3_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_2_4").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_4_0(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_4_1(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_0").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_4_2(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_1").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_4_3(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_2").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_4").transform.position;
    }

    private bool IsValidMoveFromTile_Goat_4_4(Vector3 destinationPosition)
    {
        return destinationPosition == boardTiles.Find(tile => tile.name == "Tile_4_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_3").transform.position ||
               destinationPosition == boardTiles.Find(tile => tile.name == "Tile_3_4").transform.position;
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
}
