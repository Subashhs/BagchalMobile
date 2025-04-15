using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManagerBoard3 : MonoBehaviour
{
    public static GameManagerBoard3 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    public TMP_Text turnText;
    public TMP_Text winText;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
    public GameObject tiger;
    public List<GameObject> goats = new List<GameObject>();

    private GoatMovementB3 goatMovement;
    private TigerMovementB3 tigerMovement;

    private bool tigerCapturedGoat = false;

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

        goatMovement = goatPrefab.GetComponent<GoatMovementB3>();
        tigerMovement = tigerPrefab.GetComponent<TigerMovementB3>();
    }

    public void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = (currentTurn == Turn.Tiger) ? "TIGER'S MOVE" : "GOAT'S MOVE";
        }
    }

    private void Start()
    {
        InitializeBoard();
        UpdateTurnText();
    }

    private void InitializeBoard()
    {
        GameObject tilesParent = GameObject.Find("TilesParent"); // Assign this in the editor

        if (tilesParent != null)
        {
            foreach (Transform child in tilesParent.transform)
            {
                tiles[child.name] = child.gameObject;
                Debug.Log($"Tile {child.name} found.");
            }
        }
        else
        {
            Debug.LogError("TilesParent GameObject not found!");
        }

        if (tiles.ContainsKey("Tile_0_0") && tigerPrefab != null)
        {
            tiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
            Debug.Log("Tiger instantiated.");
        }

        string[] goatPositions = { "Tile_4_0", "Tile_4_1", "Tile_4_2" };
        foreach (string position in goatPositions)
        {
            if (tiles.ContainsKey(position) && goatPrefab != null)
            {
                GameObject goat = Instantiate(goatPrefab, tiles[position].transform.position, Quaternion.identity);
                goats.Add(goat);
                Debug.Log($"Goat instantiated at {position}.");
            }
        }
    }

    public void SelectPiece(GameObject clickedObject)
    {
        Debug.Log($"SelectPiece called with object: {clickedObject.name}, currentTurn: {currentTurn}, Stored Tiger: {(tiger != null ? tiger.name : "null")}");

        if (currentTurn == Turn.Tiger)
        {
            if (clickedObject == tiger)
            {
                selectedTiger = clickedObject;
                Debug.Log("Tiger selected.");
            }
            else if (clickedObject.GetComponent<TigerMovementB3>() != null)
            {
                if (tiger != null && clickedObject == tiger)
                {
                    selectedTiger = clickedObject;
                    Debug.Log("Tiger selected (by component check).");
                }
                else
                {
                    Debug.LogWarning($"Clicked on an object with TigerMovementB3 ('{clickedObject.name}'), but it does not match the stored Tiger object.");
                }
            }
            else
            {
                Debug.LogWarning($"Attempted to select '{clickedObject.name}' as Tiger, but it's not the Tiger.");
            }
        }
        else if (currentTurn == Turn.Goat && goats.Contains(clickedObject))
        {
            selectedGoat = clickedObject;
            Debug.Log("Goat selected.");
        }
        else
        {
            Debug.LogWarning("Invalid selection for the current turn.");
        }
    }

    public bool IsPieceSelected()
    {
        return selectedTiger != null || selectedGoat != null;
    }

    public void MovePiece(GameObject tile)
    {
        if (selectedTiger != null && currentTurn == Turn.Tiger && tiles.ContainsValue(tile))
        {
            if (tigerMovement.TryMove(selectedTiger, tile, tiles))
            {
                selectedTiger = null;
                currentTurn = Turn.Goat;
                CheckForWinCondition();
                UpdateTurnText();
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
                selectedGoat = null;
                currentTurn = Turn.Tiger;
                CheckForWinCondition();
                UpdateTurnText();
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
    }

    public void CheckForWinCondition()
    {
        Debug.Log("CheckForWinCondition() called.");

        if (!tigerCapturedGoat && goats.Count < 3)
        {
            tigerCapturedGoat = true;
            winText.text = "TIGER WINS! CAPTURED A GOAT!";
            Invoke("ReturnToOptionBoard", 3f);
            return;
        }

        string currentTigerTile = tigerMovement.GetTileName(tiger.transform.position, tiles);
        Debug.Log($"Tiger current tile: {currentTigerTile}");

        if (tigerMovement.validMoves.ContainsKey(currentTigerTile))
        {
            bool tigerIsCompletelyBlocked = true;
            Debug.Log($"Tiger valid moves: {tigerMovement.validMoves[currentTigerTile].Count}");

            foreach (string possibleMove in tigerMovement.validMoves[currentTigerTile])
            {
                Debug.Log($"Checking possible move: {possibleMove}");
                if (tiles.ContainsKey(possibleMove))
                {
                    GameObject targetTile = tiles[possibleMove];
                    Debug.Log($"Target tile occupied: {tigerMovement.IsTileOccupied(targetTile)}");

                    if (!tigerMovement.IsTileOccupied(targetTile))
                    {
                        Debug.Log($"Checking jump move from {currentTigerTile} to {possibleMove}");
                        if (!tigerMovement.CanTigerJumpTo(currentTigerTile, possibleMove, tiles))
                        {
                            tigerIsCompletelyBlocked = false;
                            Debug.Log("Tiger has a valid move (not blocked).");
                            break;
                        }
                        else
                        {
                            Debug.Log("Tiger can jump but all jump places are blocked.");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Tile {possibleMove} not found in tiles dictionary.");
                }
            }

            Debug.Log($"Tiger is completely blocked: {tigerIsCompletelyBlocked}");

            if (tigerIsCompletelyBlocked)
            {
                winText.text = "GOATS WIN! TIGER IS BLOCKED!";
                Invoke("ReturnToOptionBoard", 3f);
            }
        }
        else
        {
            Debug.LogError($"Tiger current tile {currentTigerTile} not found in valid moves dictionary.");
        }
    }

    private void ReturnToOptionBoard()
    {
        Debug.Log("ReturnToOptionBoard() called.");
        SceneManager.LoadScene("OptionBoard");
    }
}