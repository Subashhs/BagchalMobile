using UnityEngine;
using System.Collections.Generic;

public class TigerMovement : MonoBehaviour
{
    private Dictionary<string, List<string>> validMoves = new Dictionary<string, List<string>>()
    {
        { "Tile_0_0", new List<string> { "Tile_0_1", "Tile_1_0", "Tile_1_1" } },
        { "Tile_0_1", new List<string> { "Tile_0_0", "Tile_0_2", "Tile_1_1" } },
        { "Tile_0_2", new List<string> { "Tile_0_1", "Tile_0_3", "Tile_1_1", "Tile_1_2", "Tile_1_3" } },
        { "Tile_0_3", new List<string> { "Tile_0_2", "Tile_0_4", "Tile_1_3" } },
        { "Tile_0_4", new List<string> { "Tile_0_3", "Tile_1_3", "Tile_1_4" } },
        { "Tile_1_0", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_0" } },
        { "Tile_1_1", new List<string> { "Tile_0_0", "Tile_0_1", "Tile_0_2", "Tile_1_0", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2" } },
        { "Tile_1_2", new List<string> { "Tile_0_2", "Tile_1_1", "Tile_2_2", "Tile_1_3" } },
        { "Tile_1_3", new List<string> { "Tile_0_2", "Tile_0_3", "Tile_0_4", "Tile_1_2", "Tile_1_4", "Tile_2_2", "Tile_2_3", "Tile_2_4" } },
        { "Tile_1_4", new List<string> { "Tile_0_4", "Tile_1_3", "Tile_2_4" } },
        { "Tile_2_0", new List<string> { "Tile_1_0", "Tile_1_1", "Tile_2_1", "Tile_3_0", "Tile_3_1" } },
        { "Tile_2_1", new List<string> { "Tile_1_1", "Tile_2_0", "Tile_2_2", "Tile_3_1" } },
        { "Tile_2_2", new List<string> { "Tile_1_1", "Tile_1_2", "Tile_1_3", "Tile_2_1", "Tile_2_3", "Tile_3_1", "Tile_3_2", "Tile_3_3" } },
        { "Tile_2_3", new List<string> { "Tile_1_3", "Tile_2_2", "Tile_2_4", "Tile_3_3" } },
        { "Tile_2_4", new List<string> { "Tile_1_3", "Tile_1_4", "Tile_2_3", "Tile_3_3", "Tile_3_4" } },
        { "Tile_3_0", new List<string> { "Tile_2_0", "Tile_3_1", "Tile_4_0" } },
        { "Tile_3_1", new List<string> { "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_2", "Tile_4_0", "Tile_4_1", "Tile_4_2" } },
        { "Tile_3_2", new List<string> { "Tile_2_2", "Tile_3_1", "Tile_3_3", "Tile_4_2" } },
        { "Tile_3_3", new List<string> { "Tile_2_2", "Tile_2_3", "Tile_2_4", "Tile_3_2", "Tile_3_4", "Tile_4_2", "Tile_4_3", "Tile_4_4" } },
        { "Tile_3_4", new List<string> { "Tile_2_4", "Tile_3_3", "Tile_4_4" } },
        { "Tile_4_0", new List<string> { "Tile_3_0", "Tile_3_1", "Tile_4_1" } },
        { "Tile_4_1", new List<string> { "Tile_3_1", "Tile_4_0", "Tile_4_2" } },
        { "Tile_4_2", new List<string> { "Tile_3_1", "Tile_3_2", "Tile_3_3", "Tile_4_1", "Tile_4_3" } },
        { "Tile_4_3", new List<string> { "Tile_3_3", "Tile_4_2", "Tile_4_4" } },
        { "Tile_4_4", new List<string> { "Tile_3_3", "Tile_3_4", "Tile_4_3" } }
    };

    private Dictionary<string, string> jumpMoves = new Dictionary<string, string>()
    {
         { "Tile_0_0|Tile_0_1", "Tile_0_2" },
        { "Tile_0_0|Tile_1_1", "Tile_2_2" },
        { "Tile_0_0|Tile_1_0", "Tile_2_0" },
        { "Tile_0_1|Tile_0_2", "Tile_3_0" },
        { "Tile_0_1|Tile_1_1", "Tile_2_1" },
        { "Tile_0_2|Tile_0_3", "Tile_0_4" },
        { "Tile_0_2|Tile_1_1", "Tile_2_0" },
        { "Tile_0_2|Tile_1_2", "Tile_2_2" },
        { "Tile_0_2|Tile_1_3", "Tile_2_4" },
        { "Tile_0_3|Tile_0_2", "Tile_0_1" },
        { "Tile_0_3|Tile_1_3", "Tile_2_3" },
        { "Tile_0_4|Tile_0_3", "Tile_0_2" },
        { "Tile_0_4|Tile_1_3", "Tile_2_2" },
        { "Tile_0_4|Tile_1_4", "Tile_2_4" },
        { "Tile_1_0|Tile_1_1", "Tile_1_2" },
        { "Tile_1_0|Tile_2_0", "Tile_3_0" },
        { "Tile_1_1|Tile_1_2", "Tile_1_3" },
        { "Tile_1_1|Tile_2_2", "Tile_3_3" },
        { "Tile_1_1|Tile_2_1", "Tile_3_1" },
        { "Tile_1_2|Tile_1_1", "Tile_1_0" },
        { "Tile_1_2|Tile_1_3", "Tile_1_4" },
        { "Tile_1_2|Tile_2_2", "Tile_3_2" },
        { "Tile_1_3|Tile_1_2", "Tile_1_1" },
        { "Tile_1_3|Tile_2_2", "Tile_3_1" },
        { "Tile_1_3|Tile_2_3", "Tile_3_3" },
        { "Tile_1_4|Tile_1_3", "Tile_1_2" },
        { "Tile_1_4|Tile_2_4", "Tile_3_4" },
        { "Tile_2_0|Tile_1_1", "Tile_0_2" },
        { "Tile_2_0|Tile_2_1", "Tile_2_2" },
        { "Tile_2_0|Tile_3_1", "Tile_4_2" },
        { "Tile_2_0|Tile_3_0", "Tile_4_0" },
        { "Tile_2_1|Tile_1_1", "Tile_0_1" },
        { "Tile_2_1|Tile_2_2", "Tile_2_3" },
        { "Tile_2_1|Tile_3_1", "Tile_4_1" },
        { "Tile_2_2|Tile_1_1", "Tile_0_0" },
        { "Tile_2_2|Tile_1_2", "Tile_0_2" },
        { "Tile_2_2|Tile_1_3", "Tile_0_4" },
        { "Tile_2_2|Tile_2_3", "Tile_2_4" },
        { "Tile_2_2|Tile_3_3", "Tile_4_4" },
        { "Tile_2_2|Tile_3_2", "Tile_4_2" },
        { "Tile_2_2|Tile_3_1", "Tile_4_0" },
        { "Tile_2_2|Tile_2_1", "Tile_2_0" },
        { "Tile_2_3|Tile_1_3", "Tile_0_3" },
        { "Tile_2_3|Tile_3_3", "Tile_4_3" },
        { "Tile_2_3|Tile_2_2", "Tile_2_1" },
        { "Tile_2_4|Tile_1_3", "Tile_0_2" },
        { "Tile_2_4|Tile_1_4", "Tile_0_4" },
        { "Tile_2_4|Tile_3_4", "Tile_4_4" },
        { "Tile_2_4|Tile_3_3", "Tile_4_2" },
        { "Tile_2_4|Tile_2_3", "Tile_2_2" },
        { "Tile_3_0|Tile_2_0", "Tile_1_0" },
        { "Tile_3_0|Tile_3_1", "Tile_3_2" },
        { "Tile_3_1|Tile_2_1", "Tile_1_1" },
        { "Tile_3_1|Tile_2_2", "Tile_1_3" },
        { "Tile_3_1|Tile_3_2", "Tile_3_3" },
        { "Tile_3_2|Tile_2_2", "Tile_1_2" },
        { "Tile_3_2|Tile_3_3", "Tile_3_4" },
        { "Tile_3_2|Tile_3_1", "Tile_3_0" },
        { "Tile_3_3|Tile_2_2", "Tile_1_1" },
        { "Tile_3_3|Tile_2_3", "Tile_1_3" },
        { "Tile_3_3|Tile_3_2", "Tile_3_1" },
        { "Tile_3_4|Tile_2_4", "Tile_1_4" },
        { "Tile_3_4|Tile_3_3", "Tile_3_2" },
        { "Tile_4_0|Tile_3_0", "Tile_2_0" },
        { "Tile_4_0|Tile_3_1", "Tile_2_2" },
        { "Tile_4_0|Tile_4_1", "Tile_4_2" },
        { "Tile_4_1|Tile_3_1", "Tile_2_1" },
        { "Tile_4_1|Tile_4_2", "Tile_4_3" },
        { "Tile_4_2|Tile_3_1", "Tile_2_0" },
        { "Tile_4_2|Tile_3_2", "Tile_2_2" },
        { "Tile_4_2|Tile_3_3", "Tile_2_4" },
        { "Tile_4_2|Tile_4_3", "Tile_4_4" },
        { "Tile_4_2|Tile_4_1", "Tile_4_0" },
        { "Tile_4_3|Tile_3_3", "Tile_2_3" },
        { "Tile_4_2|Tile_4_2", "Tile_4_1" },
        { "Tile_4_4|Tile_3_3", "Tile_2_2" },
        { "Tile_4_4|Tile_3_4", "Tile_2_4" },
        { "Tile_4_4|Tile_4_3", "Tile_4_2" },
    };

    public bool TryMove(GameObject tiger, GameObject targetTile, Dictionary<string, GameObject> boardTiles)
    {
        string currentTile = GetTileName(tiger.transform.position, boardTiles);
        string targetTileName = targetTile.name;

        if (currentTile == targetTileName)
        {
            Debug.LogWarning($"Cannot move tiger to the same tile: {currentTile}.");
            return false;
        }

        if (validMoves.ContainsKey(currentTile) && validMoves[currentTile].Contains(targetTileName))
        {
            if (!IsTileOccupied(targetTile))
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
            GameObject middleGoat = GetGoatAtTile(middleTile);
            if (middleGoat != null)
            {
                Destroy(middleGoat);
                GameManager.Instance.goats.Remove(middleGoat);
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
        string key = $"{fromTile}|{GetMiddleTile(fromTile, toTile)}";
        return jumpMoves.ContainsKey(key) && jumpMoves[key] == toTile;
    }

    private string GetMiddleTile(string fromTile, string toTile)
    {
        foreach (var jumpMove in jumpMoves)
        {
            string[] tiles = jumpMove.Key.Split('|');
            if (tiles[0] == fromTile && jumpMove.Value == toTile)
            {
                return tiles[1];
            }
        }
        return string.Empty;
    }

    private GameObject GetGoatAtTile(string tileName)
    {
        foreach (var goat in GameManager.Instance.goats)
        {
            if (GetTileName(goat.transform.position, GameManager.Instance.tiles) == tileName)
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

    public bool IsTileOccupied(GameObject tile)
    {
        foreach (var goat in GameManager.Instance.goats)
        {
            if (Vector3.Distance(goat.transform.position, tile.transform.position) < 0.1f)
            {
                return true;
            }
        }
        if (GameManagerBoard2.Instance.tiger != null &&
            Vector3.Distance(GameManager.Instance.tiger.transform.position, tile.transform.position) < 0.1f)
        {
            return true;
        }
        return false;
    }
}
