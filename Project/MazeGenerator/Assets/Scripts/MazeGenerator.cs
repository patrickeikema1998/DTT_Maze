using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] [Range(10, 250)] private int columnCount, rowCount;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile[] tiles;
    public static List <Cell> gridCells = new List<Cell> ();

    private Cell current, next;

    void Start()
    {
        FillGrid();
    }

    void Update()
    {
        
    }

    private void MazeAlgorithm()
    {
/*        //pseudo
        current.visited = true;
        next = find random neighbor;

        if(next != null)
        {
            next.visited = true;
            stack.push(current);
            set walls on false;
            current = next;
            next = find random neighbor;
        }

        if(stack > 0)
        {
            current = stack.Pop();
        }*/
    }

    private void FillGrid()
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                Cell cell = new Cell(row, column);
                gridCells.Add(cell);
            }
        }
        current = gridCells[0];
    }
}
