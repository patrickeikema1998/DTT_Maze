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
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] Tilemap tilemap;

    private UnityAction onGenerateMaze;
    private UnityAction<Slider, Text> onSliderValueChanged;
    // Start is called before the first frame update
    void Start()
    {
        onGenerateMaze += CreateMaze;
        onSliderValueChanged += UpdateSliderText;

        //UI listeners
        generateButton.onClick.AddListener(onGenerateMaze);
        columnsSlider.onValueChanged.AddListener(delegate { onSliderValueChanged(columnsSlider, columnsText); });
        rowsSlider.onValueChanged.AddListener(delegate { onSliderValueChanged(rowsSlider, rowsText); });

    }

    private void UpdateSliderText(Slider slider, Text text)
    {
        text.text = Mathf.Floor(slider.value).ToString();
    }

    private void CreateMaze()
    {
        tilemap.ClearAllTiles();
        mazeGenerator.GenerateMaze((int)rowsSlider.value, (int)columnsSlider.value);
    }
}
