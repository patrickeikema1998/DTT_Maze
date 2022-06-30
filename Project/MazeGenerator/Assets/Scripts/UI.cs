using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class UI : MonoBehaviour
{
    [SerializeField] private Slider _columnsSlider;
    [SerializeField] private Slider _rowsSlider;
    [SerializeField] private Text _columnsText;
    [SerializeField] private Text _rowsText;
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _seeMapButton;
    [SerializeField] private Button _seePlayerButton;
    [SerializeField] private Slider _zoomSlider;
    [SerializeField] private MazeGenerator _mazeGenerator;
    [SerializeField] private Tilemap _tilemap;
    private Camera _cam;
    private CameraBehavior _camBehavior;

    private UnityAction _onGenerateMaze;
    private UnityAction<Slider, Text> _onGridSliderValueChanged;
    private UnityAction<float> _onZoomSliderValueChanged;

    private bool _seeMap = false;

    public bool SeeMap
    {
        get { return _seeMap; }
    }


    void Start()
    {
        _cam = Camera.main;
        _camBehavior = _cam.GetComponent<CameraBehavior>();

        // Actions.
        _onGenerateMaze += CreateMaze;
        _onGridSliderValueChanged += UpdateSliderText;
        _onZoomSliderValueChanged += HandleZoomSlider;

        // UI listeners:
        // Buttons.
        _generateButton.onClick.AddListener(_onGenerateMaze);
        _seeMapButton.onClick.AddListener(SeeMapButtonBehavior);
        _seePlayerButton.onClick.AddListener(SeePlayerButtonBehavior);
        // Sliders.
        _columnsSlider.onValueChanged.AddListener(delegate { _onGridSliderValueChanged(_columnsSlider, _columnsText); });
        _rowsSlider.onValueChanged.AddListener(delegate { _onGridSliderValueChanged(_rowsSlider, _rowsText); });
        _zoomSlider.onValueChanged.AddListener(delegate { _onZoomSliderValueChanged(_zoomSlider.value); });



    }

    // This function zooms out the camera to see the full maze.
    private void SeeMapButtonBehavior()
    {
        _zoomSlider.value = _zoomSlider.maxValue;
        _camBehavior.CenterAndScaleCamToMaze(_mazeGenerator.RowCount, _mazeGenerator.ColumnCount);
        _seeMap = true;
    }

    // This function is called when the see player button is pressed. zooms the camera in to the player. 
    private void SeePlayerButtonBehavior()
    {
        _seeMap = false;
        _zoomSlider.value = _zoomSlider.minValue;
    }

    private void UpdateSliderText(Slider slider, Text text)
    {
        text.text = Mathf.Floor(slider.value).ToString();
    }

    // This function is called when the generate button is pressed.
    private void CreateMaze()
    {
        // For re-use.
        _tilemap.ClearAllTiles();
        // Resets slider, because cam is centered and scaled to maze.
        _zoomSlider.value = _columnsSlider.minValue; 
        _mazeGenerator.GenerateMaze((int)_rowsSlider.value, (int)_columnsSlider.value);
    }

    // This function is called when the zoom slider value is changed. 
    private void HandleZoomSlider(float sliderValue)
    {
        _camBehavior.ZoomBehavior(sliderValue);
    }


}
