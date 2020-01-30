using System;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer chipSprite;
    [SerializeField] private BoxCollider2D boxCollider;

    public Chip Chip { get; } = new Chip();
    public int Row { get; private set; }
    public int Col { get; private set; }

    private Action<CellController> onClickDownCallback;
    private Action onClickUpCallback;

    public void InitCell(int i, int j, Action<CellController> onClickDown, Action onClickUp)
    {
        Row = i;
        Col = j;

        boxCollider.enabled = false;
        onClickDownCallback += onClickDown;
        onClickUpCallback += onClickUp;
    }
    public void InitChip(ChipData data)
    {
        Chip.Init(data);

        Show(true);
        
        if (data == null)
        {
            chipSprite.enabled = false;
            boxCollider.enabled = false;
        }
        else
        {
            chipSprite.sprite = data.GetUpgradeSkin(Chip.UpgradeNum);
            boxCollider.enabled = true;
            chipSprite.enabled = true;
        }
    }
    public void Show(bool show)
    {
        var material = chipSprite.material;
        Color color = material.color;
        color.a = show ? 1 : 0.5f;
        material.color = color;
    }
    public void SetPosition(float x, float y) => transform.localPosition = new Vector2(x, y);
    public Vector3 LocalPosition => transform.localPosition;
    private void OnMouseDown()
    {
        onClickDownCallback?.Invoke(this);
        Show(false);
    }
    private void OnMouseUp() => onClickUpCallback?.Invoke();
    public bool Upgrade()
    {
        if (Chip.IsMaxUpgrade())
        {
            Reset();
            return true;
        }
        
        Chip.Upgrade();
        chipSprite.sprite = Chip.GetUpgradeSkin();
        
        return false;
    }
    public void Reset() => InitChip(null);
    public override string ToString()
    {
        return $"cell row {Row} | col {Col} | type {Chip.Type} | upgradeNum {Chip.UpgradeNum}";
    }

    public bool CanUpgrade(Chip otherChip) => Chip.CanUpgrade(otherChip);
}
