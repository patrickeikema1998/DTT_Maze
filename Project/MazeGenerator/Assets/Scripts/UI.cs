using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class UI : MonoBehaviour
{
    [SerializeField] Slider columnsSlider;
    [SerializeField] Slider rowsSlider;
    [SerializeField] Text columnsText;
    [SerializeField] Text rowsText;
    [SerializeField] Button generateButton;
    [SerializeField] Button seeMapButton;
    [SerializeField] Button seePlayerButton;
    [SerializeField] Slider zoomSlider;
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] Tilemap tilemap;

    private UnityAction onGenerateMaze;
    private UnityAction<Slider, Text> onGridSliderValueChanged;
    private UnityAction<float> onZoomSliderValueChanged;

    public bool seeMap = false;

    int mazeColumns = 10, mazeRows = 10;

    Camera cam;
    CameraBehavior camBehavior;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camBehavior = cam.GetComponent<CameraBehavior>();

        onGenerateMaze += CreateMaze;
        onGridSliderValueChanged += UpdateSliderText;
        onZoomSliderValueChanged += HandleZoomSlider;

        //UI listeners
        //buttons
        generateButton.onClick.AddListener(onGenerateMaze);
        seeMapButton.onClick.AddListener(SeeMap);
        seePlayerButton.onClick.AddListener(SeePlayer);
        //sliders
        columnsSlider.onValueChanged.AddListener(delegate { onGridSliderValueChanged(columnsSlider, columnsText); });
        rowsSlider.onValueChanged.AddListener(delegate { onGridSliderValueChanged(rowsSlider, rowsText); });
        zoomSlider.onValueChanged.AddListener(delegate { onZoomSliderValueChanged(zoomSlider.value); });



    }

    private void SeeMap()
    {
        zoomSlider.value = zoomSlider.maxValue;
        camBehavior.CenterAndScaleCamToMaze(mazeRows, mazeColumns);
        seeMap = true;
    }

    private void SeePlayer()
    {
        seeMap = false;
        zoomSlider.value = zoomSlider.minValue;
    }

    private void UpdateSliderText(Slider slider, Text text)
    {
        text.text = Mathf.Floor(slider.value).ToString();
    }

    private void CreateMaze()
    {
        tilemap.ClearAllTiles(); //for re-use
        zoomSlider.value = columnsSlider.minValue; //resets slider, because the cam zooms out.
        mazeColumns = (int)rowsSlider.value;
        mazeRows = (int)columnsSlider.value;
        mazeGenerator.GenerateMaze(mazeRows, mazeColumns);
    }

    private void HandleZoomSlider(float sliderValue)
    {
        camBehavior.ZoomBehavior(sliderValue);
    }


}
