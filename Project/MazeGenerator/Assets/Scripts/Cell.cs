using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    // Top, right, bottom, left (clockwards).
    public bool[] Walls = { true, true, true, true };

    public bool Visited = false;
    public int ColumnNumber, RowNumber;
    public Cell(int rowNumber, int columnNumber)
    {
        this.RowNumber = rowNumber;
        this.ColumnNumber = columnNumber;
    }
}
