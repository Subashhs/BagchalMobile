using UnityEngine;
using System.Collections.Generic;

public abstract class BaseAI
{
    protected Dictionary<string, GameObject> tiles;

    public BaseAI(Dictionary<string, GameObject> boardTiles)
    {
        tiles = boardTiles;
    }

    public abstract bool MakeMove();
}