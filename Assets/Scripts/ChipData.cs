using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChipData", menuName = "Chip Data", order = 51)] 
public class ChipData : ScriptableObject
{
    [SerializeField] private ChipType type;
    [SerializeField] private Sprite[] sprites;

    public ChipType Type => type;
    public int MaxUpgrades => sprites.Length - 1;
    public Sprite GetUpgradeSkin(int upgradeNum) => sprites[upgradeNum];
}
