using Enums;
using UnityEngine;

public class Chip
{
    public int UpgradeNum = 0;
    public ChipType Type = ChipType.Empty;
    private ChipData chipData;
    
    public void Init(ChipData data)
    {
        chipData = data;
        UpgradeNum = 0;

        Type = chipData != null ? chipData.Type : ChipType.Empty; 
    }
    public bool CanUpgrade(Chip otherChip) => Type == otherChip.Type && UpgradeNum == otherChip.UpgradeNum;
    public void Upgrade() => UpgradeNum++;
    public bool IsMaxUpgrade() => chipData.MaxUpgrades == UpgradeNum;
    public Sprite GetUpgradeSkin() => chipData.GetUpgradeSkin(UpgradeNum);
}