using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CenterAndScaleCamToMaze(int rowCount, int columnCount)
    {
        //sets camera to center of maze
        transform.position = new Vector3((float)columnCount/2, (float)rowCount/2, transform.position.z);

        //scales the size of the camera to the size of the maze
        if ((float)rowCount * 1.77f >= (float)columnCount)
        {
             cam.orthographicSize = rowCount / 2 + 1;

        }
        else
        {
            cam.orthographicSize = Mathf.Round(columnCount / (1.77f * 2)) + 1;
        }
    }
}
