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

    public List<GameObject> boardTiles = new List<GameObject>();

    private bool isTigerSelected = false;
    private GameObject selectedTiger = null;
    private GameObject selectedGoat = null;

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;  // Goat starts first

    private List<GameObject> placedGoats = new List<GameObject>();
    private int totalGoatsPlaced = 0;  // Track number of goats placed

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
            else
            {
                Debug.Log("Tile is already occupied by a tiger or another goat.");
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
