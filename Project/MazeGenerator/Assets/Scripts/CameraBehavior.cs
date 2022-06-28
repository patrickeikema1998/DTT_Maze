using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    float startOrtographicSize;
    Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] GameObject uiCanvas;
    UI ui;

    float cameraLerpSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ui.seeMap) MoveTowardsPlayer();
    }

    private void Awake()
    {
        cam = Camera.main;
        ui = uiCanvas.GetComponent<UI>();
    }

    private void MoveTowardsPlayer()
    {
        float wantedPosX = Mathf.Lerp(cam.transform.position.x, player.transform.position.x, cameraLerpSpeed * Time.deltaTime);
        float wantedPosY = Mathf.Lerp(cam.gameObject.transform.position.y, player.gameObject.transform.position.y, cameraLerpSpeed * Time.deltaTime);
        cam.transform.position = new Vector3(wantedPosX, wantedPosY, cam.transform.position.z);
    }

    public void CenterAndScaleCamToMaze(int rowCount, int columnCount)
    {
        //sets camera to center of maze
        transform.position = new Vector3((float)columnCount/2, (float)rowCount/2, transform.position.z);

        //scales the size of the camera to the size of the maze
        if ((float)rowCount * 1.77f >= (float)columnCount)
        {
             cam.orthographicSize = rowCount / 2 + 1; //+ 1 to give a little extra space
        }
        else
        {
            cam.orthographicSize = Mathf.Round(columnCount / (1.77f * 2)) + 1; //+ 1 to give a little extra space
        }

        startOrtographicSize = cam.orthographicSize;
    }

    public void ZoomBehavior(float zoomMultiplier)
    {
        Debug.Log(startOrtographicSize);
        if (startOrtographicSize != 0) cam.orthographicSize = startOrtographicSize * zoomMultiplier;
    }

}
