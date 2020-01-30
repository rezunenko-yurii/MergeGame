using Enums;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
public class FieldController : MonoBehaviour
{
    private const float Spacing = 1.2f;
    private const int Rows = 5;
    private const int Cols = 5;
    private const int StartChipsCount = 5;
    private const int ScoreIncrement = 100;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private DragChipController dragChip;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private ChipData[] chipData;
    
    private int score = 0;
    private CellController[,] cells = new CellController[Rows,Cols];
    private bool isCanDrag = false;

    private void Start()
    {
        SetPosition();
        GenerateField();
    }
    private void SetPosition()
    {
        float startX = 0;
        float startY = 0;
        
        float endX = Spacing * (Rows - 1);
        float endY = Spacing * (Cols - 1);

        float centerX = startX + ((endX - startX) / 2);
        float centerY = startY + ((endY - startY) / 2);
        
        transform.localPosition = new Vector2(-centerX, -centerY);
    }
    private void GenerateField()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                GameObject go = Instantiate(cellPrefab, transform, true);

                cells[i, j] = go.GetComponent<CellController>();
                cells[i, j].InitCell(i, j, OnChipMouseDown, OnChipMouseUp);
                cells[i, j].SetPosition(i * Spacing, j * Spacing);
            }
        }

        AddNewElement(StartChipsCount);
    }
    public void AddNewElement(int amount = 1)
    {
        int count = 0;
        int emptyCellsCount = CountEmptyCells();

        if (emptyCellsCount <= 0) return;
        
        do 
        {
            if (emptyCellsCount <= 0) return;
            
            int row = Random.Range(0, Rows);
            int col = Random.Range(0, Cols);

            if (cells[row, col].Chip.Type == ChipType.Empty)
            {
                int index = Random.Range(0, chipData.Length);
                cells[row, col].InitChip(chipData[index]);

                count++;
                emptyCellsCount--;
            }
        } 
        while (count < amount);
    }
    private int CountEmptyCells()
    {
        int count = 0;
        foreach (var cell in cells)
        {
            if (cell.Chip.Type == ChipType.Empty)
            {
                count++;
            }
        }

        return count;
    }
    private void OnChipMouseDown(CellController cell)
    {
        dragChip.Activate(cell);
        isCanDrag = true;
    }
    void FixedUpdate()
    {
        if (!isCanDrag) return;
        
        Vector2 localPos = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        dragChip.Move(localPos);
    }
    private void OnChipMouseUp()
    {
        isCanDrag = false;
        var foundChips = dragChip.GetFoundChips();
        
        dragChip.Deactivate();
        TryToUpgrade(foundChips);
    }
    private void TryToUpgrade((CellController, CellController) chips)
    {
        CellController startCell = chips.Item1;
        CellController endCell = chips.Item2;

        if(endCell == null)
        {
            startCell.Show(true);
            return;
        }
        
        if (startCell.CanUpgrade(endCell.Chip))
        {
            startCell.Reset();
            bool isMaxUpgrade = endCell.Upgrade();
                
            if (isMaxUpgrade) UpdateScore(ScoreIncrement);
        }
        else
        {
            startCell.Show(true);
        }
    }

    private void UpdateScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void Reset()
    {
        foreach (var cell in cells)
        {
            if (cell.Chip.Type != ChipType.Empty)
            {
                cell.InitChip(null);
            }
        }

        score = 0;
        scoreText.text = score.ToString();
        
        AddNewElement(StartChipsCount);
    }
}
