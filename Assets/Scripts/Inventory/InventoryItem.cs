using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData _itemData;
    public int stackSize;
    private int maxStackSize = 10;

    public Itemslot_UI itemSlotUI;
    public InventoryItem(ItemData _itemData)
    {
        this._itemData = _itemData;
        AddToStack();
    }

    public void AddToStack() => Mathf.Clamp(stackSize++,0,maxStackSize);
    public void RemoveFromStack() => Mathf.Clamp(stackSize--,0,maxStackSize);
}