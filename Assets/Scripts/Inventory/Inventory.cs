using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
        [SerializeField] private Itemslot_UI _itemslotUIPrefab;
        public static Inventory instance;
        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
        private Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        public event Action<InventoryItem> OnInventoryChanged;
        private List<Itemslot_UI> inventoryItemUIs;
        [SerializeField] Transform inventoryContentParent;
        private void Awake()
        {
                if (instance == null)
                        instance = this;
                else
                {
                        Destroy(gameObject);
                }
        }

        private void Start()
        {
                inventoryItemUIs = new List<Itemslot_UI>();
        }

        public void AddItem(ItemData itemData)
        {
                if (inventoryDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
                {
                        inventoryItem.AddToStack();
                        OnInventoryChanged?.Invoke(inventoryItem);
                }
                else
                {
                        InventoryItem item = new InventoryItem(itemData);
                        item.itemSlotUI = Instantiate(_itemslotUIPrefab,inventoryContentParent);
                        item.itemSlotUI.UpdateSlotUI(item);

                        inventoryItemUIs.Add(item.itemSlotUI);
                        inventoryItems.Add(item);
                        inventoryDictionary.Add(itemData,item);
                }
                
        }

        public void RemoveItem(ItemData itemData)
        {
                if (inventoryDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
                {
                        if (inventoryItem.stackSize >= 1)
                        {
                                inventoryItem.RemoveFromStack();
                        }
                        else
                        {
                                inventoryItems.Remove(inventoryItem);
                                inventoryDictionary.Remove(itemData);
                                inventoryItemUIs.Remove(inventoryItem.itemSlotUI);
                                Destroy(inventoryItem.itemSlotUI);
                        }
                        OnInventoryChanged?.Invoke(inventoryItem);
                        
                }
                else
                {
                        Debug.Log("You don't have any such item");
                }
        }
}