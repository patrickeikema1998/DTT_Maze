using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Update()
    {
        HandleMovement();
    }

    // Moves player to direction if it wil hit no walls.
    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanMove(Vector2.left))
                transform.position = new Vector2(transform.position.x - MazeGenerator.CellLength, transform.position.y);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanMove(Vector2.right))
                transform.position = new Vector2(transform.position.x + MazeGenerator.CellLength, transform.position.y);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CanMove(Vector2.up))
                transform.position = new Vector2(transform.position.x, transform.position.y + MazeGenerator.CellLength);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CanMove(Vector2.down))
                transform.position = new Vector2(transform.position.x, transform.position.y - MazeGenerator.CellLength);
        }
    }

    // Checks if there are colliders in the direction given within cellLength.
    private bool CanMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, MazeGenerator.CellLength);
        if (hit.collider == null) return true;
        else return false;
    }
}
