using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public static float CellLength = 1f;

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _finish;
    CameraBehavior _cameraBehavior;

    private int _rowCount = 10, _columnCount = 10;
    private List<Cell> _gridCells = new List<Cell>();
    private Stack<Cell> _stack = new Stack<Cell>();
    private Cell _currentCell, _nextCell;

    public int RowCount
    {
        get { return _rowCount; }
    }

    public int ColumnCount
    {
        get { return _columnCount; }
    }

    void Start()
    {
        GenerateMaze(_rowCount, _columnCount);
    }

    private void Awake()
    {
        _cameraBehavior = Camera.main.GetComponent<CameraBehavior>();
    }


    // This function is called at the start and when the generate button is pressed.
    // The grid is filled, the algorithm decides what walls should be removed and then the tiles are placed. Also the camera scales and centers to the maze.
    public void GenerateMaze(int rowAmount, int columnAmount)
    {
        _columnCount = columnAmount;
        _rowCount = rowAmount;

        FillGrid(rowAmount, columnAmount);
        while (true)
        {
            MazeAlgorithm();
            if (_stack.Count == 0)
            {
                for (int i = 0; i < _gridCells.Count; i++)
                {
                    DecideAndPlaceTile(_gridCells[i]);
                }
                if (_cameraBehavior)
                {
                    _cameraBehavior.CenterAndScaleCamToMaze(_rowCount, _columnCount);
                }
                break;
            }
        }
    }


    
      // Algorithm:
      // Visited is set to true, so that the algorithm cant traverse back to it.
      // The next cell is the neighbor of the current cell.
      // While a next cell (neighbor) is found, we set next.visited to true. The current cell is pushed to the stack for traversing.The walls need to be set to false of both the current and the next cell, determined by the direction.
      // To go through the list, we set current to the next. Now we check for the next neighbor again. If no unvisited neighbor is found, the while loop will stop, because next will be null.
      // When the while loop stops, it means that no unvisited neighbor is found.
      // The if statement checks to if it is not the last cell. If it is not, the current is set to the last Cell and removed from the stack. Now the algorithm will start over and over until it is the last cell.
    
    private void MazeAlgorithm()
    {
        _currentCell.Visited = true;
        _nextCell = CheckNextNeighbor(_currentCell);

        while (_nextCell != null)
        {
            _nextCell.Visited = true;
            _stack.Push(_currentCell);
            SetWallsToFalse(_currentCell, _nextCell);
            _currentCell = _nextCell;
            _nextCell = CheckNextNeighbor(_currentCell);
        }

        if (_stack.Count > 0)
        {
            _currentCell = _stack.Pop();
        }
    }

    // Fills the list with cells. Each cell gets a column- and rownumber for placements and algorithm.
    private void FillGrid(int totalRows, int totalColumns)
    {
        //for re-generating.
        _gridCells.Clear();

        for (int row = 0; row < totalRows; row++)
        {
            for (int column = 0; column < totalColumns; column++)
            {
                Cell cell = new Cell(row, column);
                _gridCells.Add(cell);
            }
        }
        _currentCell = _gridCells[0];
        _player.transform.position = new Vector2((_currentCell.ColumnNumber / 2f) + (CellLength / 2f), (_currentCell.ColumnNumber / 2f) + (CellLength / 2f)); //sets player pos to start of maze
        _finish.transform.position = new Vector2(_gridCells[_gridCells.Count - 1].ColumnNumber + (CellLength / 2f), _gridCells[_gridCells.Count - 1].RowNumber + (CellLength / 2f));
    }


    // If you multiply the rownumber by the amount of columns, you get the index of the first cell in that rownumber. if you add the column number to that, you get the right index.
    private int GetIndex(int objectRow, int objectColumn)
    {
        if (objectColumn < 0 || objectRow < 0 || objectColumn >= _columnCount || objectRow >= _rowCount) return -1;
        return objectRow * _columnCount + objectColumn;
    }


    // This function returns a random neighbor of a cell.
    private Cell CheckNextNeighbor(Cell currentCell)
    {
        List<Cell> neighbors = new List<Cell>();

        // Neighbor indexes.
        int topNeighborIndex = GetIndex(currentCell.RowNumber - 1, currentCell.ColumnNumber);
        int bottomNeighborIndex = GetIndex(currentCell.RowNumber + 1, currentCell.ColumnNumber);
        int leftNeighborIndex = GetIndex(currentCell.RowNumber, currentCell.ColumnNumber - 1);
        int rightNeighborIndex = GetIndex(currentCell.RowNumber, currentCell.ColumnNumber + 1);

        // Adds neighbors to list if found and not visited.
        if (topNeighborIndex != -1)
        {
            Cell top = _gridCells[topNeighborIndex];
            if (!top.Visited) neighbors.Add(top);
        }
        if (rightNeighborIndex != -1)
        {
            Cell right = _gridCells[rightNeighborIndex];
            if (!right.Visited) neighbors.Add(right);
        }

        if (bottomNeighborIndex != -1)
        {
            Cell bottom = _gridCells[bottomNeighborIndex];
            if (!bottom.Visited) neighbors.Add(bottom);
        }
        if (leftNeighborIndex != -1)
        {
            Cell left = _gridCells[leftNeighborIndex];
            if (!left.Visited) neighbors.Add(left);

        }
        // Returns random neighbor.
        if (neighbors.Count > 0)
        {
            Cell randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
            return randomNeighbor;
        }
        else
        {
            return null;
        }
    }

    // This function setts walls to false to later decide what tiles should be placed.
    // When a right neighbor is returned, the right wall of the current and the left wall of the next should be set to false.
    // This way the walls 'break open' and a path is created.
    private void SetWallsToFalse(Cell current, Cell next)
    {
        float columnDifference = current.ColumnNumber - next.ColumnNumber;

        // Right side of current.
        if (columnDifference == 1)
        {
            current.Walls[3] = false;
            next.Walls[1] = false;
        }
        // Left side of current.
        else if (columnDifference == -1)
        {
            current.Walls[1] = false;
            next.Walls[3] = false;
        }

        float rowDifference = current.RowNumber - next.RowNumber;

        // Bottom side of current.
        if (rowDifference == 1)
        {
            current.Walls[2] = false;
            next.Walls[0] = false;
        }
        // Top side of current.
        else if (rowDifference == -1)
        {
            current.Walls[0] = false;
            next.Walls[2] = false;
        }
    }


    // This function decides what walls should be placed, determined by the false booleans per cell. 1000 means [true, false, false, false].
    // Also places the tiles on the tilemap.
    private void DecideAndPlaceTile(Cell cell)
    {
        switch (MakeSuitableForSwitch(cell.Walls))
        {
            case 0111:
                //top wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[0]);
                break;
            case 1011:
                //right wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[1]);
                break;
            case 1101:
                //bottom wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[2]);
                break;
            case 1110:
                //left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[3]);
                break;
            case 1100:
                //bottom and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[4]);
                break;
            case 0110:
                //top and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[5]);
                break;
            case 0011:
                //top and right wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[6]);
                break;
            case 1001:
                //right and bottom wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[7]);
                break;
            case 0101:
                //top and bottom wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[8]);
                break;
            case 1010:
                //right and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[9]);
                break;
            case 1000:
                //right, bottom and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[10]);
                break;
            case 0100:
                //top, bottom and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[11]);
                break;
            case 0010:
                //top, right and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[12]);
                break;
            case 0001:
                //top, right and bottom wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[13]);
                break;
            case 0000:
                //top, right, bottom and left wall missing
                _tilemap.SetTile(new Vector3Int(cell.ColumnNumber, cell.RowNumber, 0), _tiles[14]);
                break;


        }
    }

    // Boolean arrays are not allowed in switches. This is why the array is calculated into binary.
    private int MakeSuitableForSwitch(bool[] values)
    {
        return (values[0] ? 1 : 0) * 1000 + (values[1] ? 1 : 0) * 100 + (values[2] ? 1 : 0) * 10 + (values[3] ? 1 : 0);
    }
}
