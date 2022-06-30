using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject uiCanvas;
    UI ui;
    Camera cam;

    private float _fullOrtographicSize;
    private float _cameraLerpSpeed = 2f;

    void Update()
    {
        if (!ui.SeeMap) MoveCamTowardsPlayer();
    }

    private void Awake()
    {
        cam = Camera.main;
        ui = uiCanvas.GetComponent<UI>();
    }

    // This function will move the camera smoothly towards the player. Increase lerp speed to increase the speed of the camera.
    private void MoveCamTowardsPlayer()
    {
        float wantedPosX = Mathf.Lerp(cam.transform.position.x, player.transform.position.x, _cameraLerpSpeed * Time.deltaTime);
        float wantedPosY = Mathf.Lerp(cam.gameObject.transform.position.y, player.gameObject.transform.position.y, _cameraLerpSpeed * Time.deltaTime);
        cam.transform.position = new Vector3(wantedPosX, wantedPosY, cam.transform.position.z);
    }

    public void CenterAndScaleCamToMaze(int rowCount, int columnCount)
    {
        // Sets camera to center of maze.
        transform.position = new Vector3((float)columnCount / 2, (float)rowCount / 2, transform.position.z);

        // Scales the size of the camera to the size of the maze.
        float multiplier = 1.77f;
        if ((float)rowCount * multiplier >= (float)columnCount)
        {
            cam.orthographicSize = rowCount / 2 + 1; //+ 1 to give a little extra space
        }
        else
        {
            cam.orthographicSize = Mathf.Round(columnCount / (multiplier * 2)) + 1; //+ 1 to give a little extra space
        }

        // Used for zoom behavior.
        _fullOrtographicSize = cam.orthographicSize;
    }

    // The slider can zoom in with x10 max zoom. 
    public void ZoomBehavior(float zoomMultiplier)
    {
        if (_fullOrtographicSize != 0) cam.orthographicSize = _fullOrtographicSize * zoomMultiplier;
    }

}
