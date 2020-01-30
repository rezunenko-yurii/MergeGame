using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] 
public class DragChipController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;

    private CellController startCell;
    private CellController endCell;
    
    private Action<CellController> onClickFoundCallback;

    public void Activate(CellController cell)
    {
        startCell = cell;
        spriteRenderer.sprite = cell.Chip.GetUpgradeSkin();
        transform.localPosition = cell.LocalPosition;

        EnableComponents(true);
    }

    public void Deactivate() => EnableComponents(false);

    private void EnableComponents(bool value)
    {
        spriteRenderer.enabled = value;
        circleCollider.enabled = value; 
    }
    public void Move(Vector2 localPos) => transform.localPosition = localPos;
    private void OnTriggerEnter2D(Collider2D other)
    {
        CellController otherCell = other.GetComponent<CellController>();
        
        if (otherCell == startCell || otherCell.Chip.Type != startCell.Chip.Type) return;
            
        endCell = otherCell;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CellController otherCell = other.GetComponent<CellController>();

        if (endCell == otherCell) endCell = null;
    }

    public (CellController, CellController) GetFoundChips() => (startCell, endCell);
}
