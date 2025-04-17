// TigerMovementB3.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TigerMovementB5 : MonoBehaviour
{
    public Dictionary<string, List<string>> validMoves = new Dictionary<string, List<string>>()
    {
        { "Tile_0_0", new List<string> { "Tile_1_2", "Tile_1_0", "Tile_1_1" } },
        { "Tile_1_0", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_0" } },
        { "Tile_1_1", new List<string> { "Tile_0_0", "Tile_1_0", "Tile_1_2" } },
        { "Tile_1_2", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_2" } },
        { "Tile_2_0", new List<string> { "Tile_1_0", "Tile_2_1" } },
        { "Tile_2_1", new List<string> { "Tile_2_0", "Tile_2_2", "Tile_3_1" } },
        { "Tile_2_2", new List<string> { "Tile_1_2", "Tile_2_1" } },
        { "Tile_3_0", new List<string> { "Tile_3_1" } },
        { "Tile_3_1", new List<string> { "Tile_2_1", "Tile_3_0", "Tile_3_2", "Tile_4_1" } },
        { "Tile_3_2", new List<string> { "Tile_3_1" } },
    
    };

    private Dictionary<string, string> jumpMoves = new Dictionary<string, string>()
    {
        { "Tile_0_0|Tile_1_0", "Tile_2_0" },
        { "Tile_0_0|Tile_1_2", "Tile_2_2" },
        { "Tile_1_0|Tile_1_1", "Tile_1_2" },
        { "Tile_1_2|Tile_1_1", "Tile_1_0" },
        { "Tile_2_0|Tile_1_0", "Tile_0_0" },
        { "Tile_2_0|Tile_2_1", "Tile_2_2" },
        { "Tile_2_2|Tile_2_1", "Tile_2_0" },
        { "Tile_2_1|Tile_3_1", "Tile_4_1" },
        { "Tile_3_0|Tile_3_1", "Tile_3_2" },
        { "Tile_3_2|Tile_3_1", "Tile_3_0" },
        
    };

    public bool TryMove(GameObject tiger, GameObject targetTile, Dictionary<string, GameObject> boardTiles, List<GameObject> opponentGoats)
    {
        string currentTile = GetTileName(tiger.transform.position, boardTiles);
        string targetTileName = targetTile.name;

        Debug.Log($"Attempting to move tiger from {currentTile} to {targetTileName}.");

        if (currentTile == targetTileName)
        {
            Debug.LogWarning($"Cannot move tiger to the same tile: {currentTile}.");
            return false;
        }

        if (validMoves.ContainsKey(currentTile) && validMoves[currentTile].Contains(targetTileName))
        {
            if (!IsTileOccupied(targetTile, opponentGoats))
            {
                tiger.transform.position = targetTile.transform.position;
                Debug.Log($"Tiger moved from {currentTile} to {targetTileName}.");
                return true;
            }
            else
            {
                Debug.LogWarning($"Cannot move tiger to {targetTileName} because it is occupied.");
            }
        }
        else if (CanJumpAndCapture(currentTile, targetTileName))
        {
            string middleTile = GetMiddleTile(currentTile, targetTileName);
            GameObject capturedGoat = GetGoatAtTile(middleTile, opponentGoats, boardTiles);
            if (capturedGoat != null)
            {
                Destroy(capturedGoat);
                GameManagerBoard5.Instance.RemoveGoat(capturedGoat);
                tiger.transform.position = targetTile.transform.position;
                Debug.Log($"Tiger jumped from {currentTile} to {targetTileName}, capturing goat at {middleTile}.");
                return true;
            }
        }
        else
        {
            Debug.LogWarning($"Invalid move for tiger from {currentTile} to {targetTileName}.");
        }

        return false;
    }

    private bool CanJumpAndCapture(string fromTile, string toTile)
    {
        string middleTile = GetMiddleTile(fromTile, toTile);
        if (string.IsNullOrEmpty(middleTile)) return false;
        string key = $"{fromTile}|{middleTile}";
        return jumpMoves.ContainsKey(key) && jumpMoves[key] == toTile;
    }

    public string GetMiddleTile(string fromTile, string toTile)
    {
        foreach (var jumpMove in jumpMoves)
        {
            string[] tiles = jumpMove.Key.Split('|');
            if (tiles[0] == fromTile && jumpMove.Value == toTile)
            {
                return tiles[1];
            }
            else if (tiles[1] == fromTile && jumpMove.Value == toTile) // Check reverse jump as well
            {
                return tiles[0];
            }
        }
        return string.Empty;
    }

    private GameObject GetGoatAtTile(string tileName, List<GameObject> goats, Dictionary<string, GameObject> boardTiles)
    {
        foreach (var goat in goats)
        {
            if (GetTileName(goat.transform.position, boardTiles) == tileName)
            {
                return goat;
            }
        }
        return null;
    }

    public string GetTileName(Vector3 position, Dictionary<string, GameObject> boardTiles)
    {
        foreach (var tile in boardTiles)
        {
            if (Vector3.Distance(tile.Value.transform.position, position) < 0.1f)
                return tile.Key;
        }
        Debug.LogError("Tiger position does not match any tile.");
        return string.Empty;
    }

    public bool IsTileOccupied(GameObject tile, List<GameObject> opponentGoats)
    {
        Vector3 tilePos = tile.transform.position;
        if (GameManagerBoard5.Instance.GetTiger() != null &&
            Vector3.Distance(GameManagerBoard5.Instance.GetTiger().transform.position, tilePos) < 0.1f)
        {
            return true;
        }
        foreach (var goat in opponentGoats)
        {
            if (goat != null && Vector3.Distance(goat.transform.position, tilePos) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }
}