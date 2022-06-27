using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    private int columnCount, rowCount;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile[] tiles;
    private List<Cell> gridCells = new List<Cell>();
    private Stack<Cell> stack = new Stack<Cell>();
    private Cell current, next;

    Camera cam;
    CameraBehavior cameraBehavior;

    void Start()
    {
        cam = Camera.main;
        cameraBehavior = cam.GetComponent<CameraBehavior>();
        GenerateMaze(10,10);
    }

    void Update()
    {

    }

    public void GenerateMaze(int rowAmount, int columnAmount)
    {
        columnCount = columnAmount;
        rowCount = rowAmount;

        FillGrid(rowAmount, columnAmount);
        while (true)
        {
            MazeAlgorithm();
            if (stack.Count == 0)
            {
                for (int i = 0; i < gridCells.Count; i++)
                {
                    DecideAndPlaceTile(gridCells[i]);
                }
                cameraBehavior.CenterAndScaleCamToMaze(rowCount, columnCount);
                break;
            }
        }
    }

    private void MazeAlgorithm()
    {
        current.visited = true;
        next = CheckNextNeighbor(current);

        while(next != null)
        {
            next.visited = true;
            stack.Push(current);
            RemoveWalls(current, next);
            current = next;
            next = CheckNextNeighbor(current);
        }

        if (stack.Count > 0)
        {
            current = stack.Pop();
        }
    }

    //fills the list with cells. Each cell gets a column- and rownumber for placements and algorithm.
    private void FillGrid(int totalRows, int totalColumns)
    {
        //for re-generating.
        gridCells.Clear();

        for (int row = 0; row < totalRows; row++)
        {
            for (int column = 0; column < totalColumns; column++)
            {
                Cell cell = new Cell(row, column);
                gridCells.Add(cell);
            }
        }
        current = gridCells[0];
    }
    //if you multiply the rownumber by the amount of columns, you get the index of the first cell in that rownumber. if you add the column number to that, you get the right index.
    private int Index(int objectRow, int objectColumn)
    {
        if (objectColumn < 0 || objectRow < 0 || objectColumn >= columnCount || objectRow >= rowCount) return -1;
        return objectRow * columnCount + objectColumn;
    }


    //this function returns a random neighbor.
    private Cell CheckNextNeighbor(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        //neighbor indexes
        int topNeighborIndex = Index(cell.rowNumber - 1, cell.columnNumber);
        int bottomNeighborIndex = Index(cell.rowNumber + 1, cell.columnNumber);
        int leftNeighborIndex = Index(cell.rowNumber, cell.columnNumber - 1);
        int rightNeighborIndex = Index(cell.rowNumber, cell.columnNumber + 1);

        //adds neighbors to list if found and not visited.
        if (topNeighborIndex != -1)
        {
            var top = gridCells[topNeighborIndex];
            if (!top.visited) neighbors.Add(top);
        }
        if (rightNeighborIndex != -1)
        {
            var right = gridCells[rightNeighborIndex];
            if (!right.visited) neighbors.Add(right);
        }

        if (bottomNeighborIndex != -1)
        {
            var bottom = gridCells[bottomNeighborIndex];
            if (!bottom.visited) neighbors.Add(bottom);
        }
        if (leftNeighborIndex != -1)
        {
            var left = gridCells[leftNeighborIndex];
            if (!left.visited) neighbors.Add(left);

        }
        //returns random neighbor
        if (neighbors.Count > 0)
        {
            var randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
            return randomNeighbor;
        }
        else
        {
            return null;
        }
    }

    private void RemoveWalls(Cell current, Cell next)
    {
        float columnDifference = current.columnNumber - next.columnNumber;

        //right side of current
        if (columnDifference == 1)
        {
            current.walls[3] = false;
            next.walls[1] = false;
        }
        //left side of current
        else if (columnDifference == -1)
        {
            current.walls[1] = false;
            next.walls[3] = false;
        }

        float rowDifference = current.rowNumber - next.rowNumber;

        // bottom side of current
        if (rowDifference == 1)
        {
            current.walls[2] = false;
            next.walls[0] = false;
        }
        //top side of current
        else if (rowDifference == -1)
        {
            current.walls[0] = false;
            next.walls[2] = false;
        }
    }


    //this function places the tiles accordingly to the false booleans per cell. 1000 means [true, false, false, false].
    private void DecideAndPlaceTile(Cell cell)
    {
        switch (makeSuitableForSwitch(cell.walls))
        {
            case 0111:
                //top wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[0]);
                break;
            case 1011:
                //right wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[1]);
                break;
            case 1101:
                //bottom wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[2]);
                break;
            case 1110:
                //left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[3]);
                break;
            case 1100:
                //bottom and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[4]);
                break;
            case 0110:
                //top and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[5]);
                break;
            case 0011:
                //top and right wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[6]);
                break;
            case 1001:
                //right and bottom wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[7]);
                break;
            case 0101:
                //top and bottom wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[8]);
                break;
            case 1010:
                //right and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[9]);
                break;
            case 1000:
                //right, bottom and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[10]);
                break;
            case 0100:
                //top, bottom and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[11]);
                break;
            case 0010:
                //top, right and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[12]);
                break;
            case 0001:
                //top, right and bottom wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[13]);
                break;
            case 0000:
                //top, right, bottom and left wall missing
                tilemap.SetTile(new Vector3Int(cell.columnNumber, cell.rowNumber, 0), tiles[14]);
                break;


        }
    }

    //boolean arrays are not allowed in switches. This is why the array is calculated into binary.
    private int makeSuitableForSwitch(bool[] values)
    {
        return (values[0] ? 1 : 0) * 1000 + (values[1] ? 1 : 0) * 100 + (values[2] ? 1 : 0) * 10 + (values[3] ? 1 : 0);
    }
}
