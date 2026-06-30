using System;

[Serializable]
public class PuzzleItemRequirement
{
    public ItemData itemData;
    public int requiredAmount = 1;
    public bool requirementMet = false;
}
