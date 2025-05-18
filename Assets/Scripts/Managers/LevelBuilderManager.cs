using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class LevelBuilderManager : MonoBehaviour
{
    [SerializeField] private GridSystem setup;
    [SerializeField] private GridManager manager;
    [SerializeField] int maxGridSize = 7;
    [SerializeField] TextMeshProUGUI widthText;
    [SerializeField] TextMeshProUGUI heightText;
    [SerializeField] Button addBlockBtn;
    [SerializeField] Button addBusBtn;
    [SerializeField] GameObject busColorParent;
    [SerializeField] Button proceedBtn;

    private int gridWidth = 1;
    private int gridHeight = 1;
    private List<int> selectedCellIndex = new List<int>();
    private bool isMousePressed = false;
    private int selecetedColor = -1;
    private bool isSetPassage = false;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        gridWidth = setup.columns;
        widthText.text = gridWidth.ToString();
        gridHeight = setup.rows;
        heightText.text = gridHeight.ToString();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.IsPressed() && !isMousePressed)
        {
            HandleClick();
            isMousePressed = true;
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isMousePressed = false;
        }
#elif UNITY_ANDROID
        if (Touch.activeTouches.Count > 0&& !isMousePressed)
        {
        heightText.text = "Touch on";
            HandleClick();
            isMousePressed = true;
        }
        else if (Touch.activeTouches.Count == 0 && isMousePressed)
        {
        heightText.text = "Touch off";
            isMousePressed = false;
        }
#endif
    }

    public void ChangeWidth(bool increment)
    {
        if (increment)
        {
            if (gridWidth < maxGridSize)
                gridWidth++;
        }
        else
        {
            if (gridWidth > 1)
                gridWidth--;
        }
        setup.columns = gridWidth;
        widthText.text = gridWidth.ToString();
        setup.ValidateData();
        setup.ResetAllBuses();
        manager.GenerateGridWithBuses();
        proceedBtn.interactable = false;
    }


    public void ChangeHeight(bool increment)
    {
        if (increment)
        {
            if (gridHeight < maxGridSize)
                gridHeight++;
        }
        else
        {
            if (gridHeight > 1)
                gridHeight--;
        }
        setup.rows = gridHeight;
        heightText.text = gridHeight.ToString();
        setup.ValidateData();
        setup.ResetAllBuses() ;
        manager.GenerateGridWithBuses();
        proceedBtn.interactable = false;
    }

    private void HandleClick()
    {
        Vector2 pos = new Vector2(0, 0);
#if UNITY_EDITOR
        pos = Mouse.current.position.ReadValue();
#elif UNITY_ANDROID
        pos = Touch.activeTouches[0].screenPosition;
#endif
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 worldPosition = hit.point;
            int cellIndex = GetCellIndexFromWorldPosition(worldPosition);

            if (cellIndex >= 0)
            {

                if (!isSetPassage)
                {
                    GameObject cell = manager.GetCellFromIndex(cellIndex);
                    if (selectedCellIndex.Contains(cellIndex))
                    {
                        selectedCellIndex.Remove(cellIndex);
                        cell.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        if (!CheckConnectedCell(cellIndex))
                        {
                            ResetSelectedCells();
                        }
                        selectedCellIndex.Add(cellIndex);
                        cell.transform.GetChild(0).gameObject.SetActive(true);
                    }

                    if (selectedCellIndex.Count == 1)
                    {
                        addBlockBtn.interactable = true;
                        busColorParent.SetActive(false);
                        addBusBtn.interactable = false;
                    }
                    else if (selectedCellIndex.Count > 1)
                    {
                        busColorParent.SetActive(true);
                        addBlockBtn.interactable = false;
                    }
                }
                else
                {
                    if (selectedCellIndex.Contains(cellIndex))
                    {
                        setup.SetPassagePosition(cellIndex, selecetedColor);
                        ResetSelectedCells();
                        isSetPassage = false;
                        manager.GenerateGridWithBuses();
                    }
                }
            }

        }
    }

    private bool CheckConnectedCell(int index)
    {
        if (selectedCellIndex.Count == 0)
            return true;

        int lastIndex = selectedCellIndex[selectedCellIndex.Count - 1];
        if (index == lastIndex + 1 || index == lastIndex - 1 || index == lastIndex + setup.columns || index == lastIndex - setup.columns)
            return true;
        return false;
    }

    private int GetCellIndexFromWorldPosition(Vector3 worldPosition)
    {
        Vector3 relativePosition = worldPosition - setup.originPosition;

        int x = Mathf.FloorToInt(relativePosition.x + 0.5f);
        int z = Mathf.FloorToInt(relativePosition.z + 0.5f);

        if (x >= 0 && x < setup.columns && z >= 0 && z < setup.rows)
        {
            int index = z * setup.columns + x;
            return index;
        }

        return -1; // Invalid cell
    }

    public void AddBlock()
    {
        if (selectedCellIndex[0] != -1)
        {

            setup.AddObstacle(selectedCellIndex[0]);
            manager.GenerateGridWithBuses();
            selectedCellIndex.Clear();
        }
    }

    public void SelectBusColor(int colorIndex)
    {
        selecetedColor = colorIndex;
    }

    public void AddBus()
    {
        setup.AddBus(CloneList(), selecetedColor);
        manager.GenerateGridWithBuses();
        selectedCellIndex.Clear();
        StartSetPassage();
        busColorParent.SetActive(false);
        proceedBtn.interactable = true;
    }

    private List<int> CloneList()
    {
        List<int> clone = new List<int>();
        foreach (int index in selectedCellIndex)
        {
            clone.Add(index);
        }
        return clone;
    }

    private void StartSetPassage()
    {
        //Highlight all edge tiles
        //Enable selection of tiles 

        for (int y = 0; y < setup.rows; y++)
        {
            for (int x = 0; x < setup.columns; x++)
            {
                bool isEdge = x == 0 || x == setup.columns - 1 || y == 0 || y == setup.rows - 1;
                if (isEdge)
                {
                    int index = y * setup.columns + x;
                    if (!setup.obstacleIndex.Contains(index) && setup.bluePassagePosition != index && setup.greenPassagePosition != index
                        && setup.orangePassagePosition != index && setup.purplePassagePosition != index && setup.redPassagePosition != index)
                    {
                        selectedCellIndex.Add(index);
                        GameObject cell = manager.GetCellFromIndex(index);
                        cell.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
        isSetPassage = true;
    }

    private void ResetSelectedCells()
    {
        foreach (int index in selectedCellIndex)
        {
            GameObject cell = manager.GetCellFromIndex(index);
            cell.transform.GetChild(0).gameObject.SetActive(false);
        }
        selectedCellIndex.Clear();
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
