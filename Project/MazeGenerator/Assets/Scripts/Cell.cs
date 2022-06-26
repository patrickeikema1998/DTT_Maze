using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    //top, right, bottom, left (clockwards)
    public bool[] walls = { true, true, true, true };
    public bool visited = false;
    public int columnNumber, rowNumber;
    public Cell(int rowNumber, int columnNumber)
    {
        this.rowNumber = rowNumber;
        this.columnNumber = columnNumber;
    }
}
