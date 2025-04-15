using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

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
        winText.gameObject.SetActive(false); // Hide win text at the start
    }

    private void PopulateBoardTiles()
    {
        boardTiles.AddRange(GameObject.FindGameObjectsWithTag("BoardTile"));
    }

    private void UpdateTurnDisplay()
    {
        turnIndicator.text = currentTurn == Turn.Tiger ? "Tiger's Turn" : "Goat's Turn";
        goatsCapturedText.text = "Goats Captured: " + goatsCaptured;
    }

    // Place the tigers at the four corners at the start of the game
    private void PlaceTigers()
    {
        Vector3[] tigerStartPositions = new Vector3[]
        {
            GetTilePosition("Tile_0_0"),
            GetTilePosition("Tile_0_4"),
            GetTilePosition("Tile_4_0"),
            GetTilePosition("Tile_4_4")
        };

        foreach (Vector3 position in tigerStartPositions)
        {
            Instantiate(tigerPrefab, position, Quaternion.identity);
        }
    }

    // Select a tiger to move
    public void SelectTiger(GameObject tiger)
    {
        Debug.Log("Selecting tiger: " + tiger.name);
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
            Debug.Log("Moving tiger to: " + destinationTile.name);
            if (selectedTiger == null)
            {
                Debug.LogError("selectedTiger is null!");
                return;
            }
            if (destinationTile == null)
            {
                Debug.LogError("destinationTile is null!");
                return;
            }

            string currentTile = GetTileName(selectedTiger.transform.position);
            string destinationTileName = destinationTile.name;

            Debug.Log("Current tile: " + currentTile + ", Destination tile: " + destinationTileName);

            TigerMovement tigerMovement = selectedTiger.GetComponent<TigerMovement>();

            if (tigerMovement == null)
            {
                Debug.LogError("TigerMovement component is missing on selectedTiger!");
                return;
            }

            if (tigerMovement.IsValidMove(currentTile, destinationTileName))
            {
                selectedTiger.transform.position = destinationTile.transform.position;
                DeselectTiger();
                SwitchTurn();
            }
            else if (tigerMovement.IsValidCapture(currentTile, destinationTileName, out string goatTile))
            {
                selectedTiger.transform.position = destinationTile.transform.position;
                CaptureGoat(goatTile);
                DeselectTiger();
                SwitchTurn();
            }
            else
            {
                Debug.Log("Invalid move");
            }
        }
    }

    // Capture a goat and update the count
    private void CaptureGoat(string goatTile)
    {
        Vector3 goatPosition = GetTilePosition(goatTile);
        GameObject capturedGoat = placedGoats.Find(goat => goat.transform.position == goatPosition);
        placedGoats.Remove(capturedGoat);
        Destroy(capturedGoat);

        // Update the captured goats count
        goatsCaptured++;
        UpdateTurnDisplay();

        // Check for tiger win condition
        if (goatsCaptured >= 5)
        {
            EndGame("Tigers Win!");
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

    // Start Goat movement after all goats are placed
    public void SelectGoat(GameObject goat)
    {
        Debug.Log("SelectGoat called, totalGoatsPlaced: " + totalGoatsPlaced);
        if (currentTurn == Turn.Goat && totalGoatsPlaced >= 20 && selectedGoat == null)
        {
            Debug.Log("Goat selected: " + goat.name);
            selectedGoat = goat;
        }
        else
        {
            Debug.Log("Goat selection failed. currentTurn: " + currentTurn + ", selectedGoat: " + selectedGoat);
        }
    }

    public void MoveGoat(GameObject destinationTile)
    {
        Debug.Log("MoveGoat called, selectedGoat: " + selectedGoat);
        if (currentTurn == Turn.Goat && selectedGoat != null)
        {
            string currentTile = GetTileName(selectedGoat.transform.position);
            string destinationTileName = destinationTile.name;

            GoatMovement goatMovement = selectedGoat.GetComponent<GoatMovement>();

            if (goatMovement == null)
            {
                Debug.LogError("GoatMovement component missing on selectedGoat!");
                return;
            }

            if (goatMovement.IsValidMove(currentTile, destinationTileName))
            {
                Debug.Log("Valid Goat move");
                selectedGoat.transform.position = destinationTile.transform.position;
                selectedGoat = null;
                SwitchTurn();
            }
            else
            {
                Debug.Log("Invalid Goat move");
            }
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
        CheckGoatWinCondition();
    }

    // Switch turns between Tiger and Goat
    private void SwitchTurn()
    {
        currentTurn = currentTurn == Turn.Tiger ? Turn.Goat : Turn.Tiger;
        UpdateTurnDisplay();
    }

    // Check for goat win condition
    // ... (other GameManager code)

    // Check for goat win condition
    private void CheckGoatWinCondition()
    {
        bool allMovesBlocked = true;

        foreach (var tiger in GameObject.FindGameObjectsWithTag("Tiger"))
        {
            string currentTile = GetTileName(tiger.transform.position);
            TigerMovement tigerMovement = tiger.GetComponent<TigerMovement>();

            if (tigerMovement == null)
            {
                Debug.LogError("TigerMovement component is missing on tiger!");
                continue;
            }

            bool tigerCanMove = false;
            foreach (var tile in boardTiles)
            {
                if (tile != null && tigerMovement.IsValidMove(currentTile, tile.name))
                {
                    tigerCanMove = true;
                    break;
                }

                if (tile != null && tigerMovement.IsValidCapture(currentTile, tile.name, out string goatTile))
                {
                    tigerCanMove = true;
                    break;
                }
            }

            if (tigerCanMove)
            {
                allMovesBlocked = false;
                break;
            }
        }

        if (allMovesBlocked)
        {
            EndGame("Goats Win!");
        }
    }

    // ... (rest of GameManager code)

    private void EndGame(string result)
    {
        winText.text = result;
        winText.gameObject.SetActive(true);
        turnIndicator.gameObject.SetActive(false);
        StartCoroutine(LoadOptionBoardScene());
    }

    private IEnumerator LoadOptionBoardScene()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds before switching scenes
        SceneManager.LoadScene("OptionBoard");
    }

    // Get the tile name from the position
    public string GetTileName(Vector3 position)
    {
        foreach (var tile in boardTiles)
        {
            if (tile.transform.position == position)
                return tile.name;
        }
        return null;
    }

    // Get the tile position from the name
    public Vector3 GetTilePosition(string tileName)
    {
        GameObject tile = boardTiles.Find(t => t.name == tileName);
        if (tile != null)
        {
            return tile.transform.position;
        }
        return Vector3.zero;
    }

    // Check if there is a goat at the given position
    public bool IsGoatAtPosition(Vector3 position)
    {
        foreach (var goat in placedGoats)
        {
            if (goat.transform.position == position)
            {
                return true;
            }
        }
        return false;
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