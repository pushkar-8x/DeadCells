using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Itemslot_UI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemCount;
    private InventoryItem _inventoryItem;

    private void Start()
    {
        Inventory.instance.OnInventoryChanged += OnInventoryItemChanged;
    }

    private void OnDestroy()
    {
        Inventory.instance.OnInventoryChanged -= OnInventoryItemChanged;
    }

    private void OnInventoryItemChanged(InventoryItem obj)
    {
        if (obj == _inventoryItem)
        {
            UpdateSlotUI(obj);
        }
    }

    public void UpdateSlotUI(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
        itemName.text = inventoryItem._itemData.itemName;
        itemIcon.sprite = inventoryItem._itemData.itemIcon;
        
        if (inventoryItem.stackSize > 0)
        {
            itemCount.text = inventoryItem.stackSize.ToString();
        }
       
    }
}
