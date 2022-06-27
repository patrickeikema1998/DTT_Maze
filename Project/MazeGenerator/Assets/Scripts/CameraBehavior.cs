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
        Vector2 camPos = new Vector2();

        camPos.x = columnCount / 2;
        camPos.y = rowCount / 2;
        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);

        //size of camera.
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
