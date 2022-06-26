using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile tile;
    [SerializeField] [Range(10, 250)] private int columnCount, rowCount;

    void Start()
    {
        for(int row = 0; row < rowCount; row++)
        {
            for(int column = 0; column < columnCount; column++)
            {
                tilemap.SetTile(new Vector3Int(column, row, 0), tile);
            }
        }
    }

    void Update()
    {
        
    }
}
