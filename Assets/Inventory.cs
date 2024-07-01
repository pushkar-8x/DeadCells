using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

        public static Inventory instance;
        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
        private Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();


        private void Awake()
        {
                if (instance == null)
                        instance = this;
                else
                {
                        Destroy(gameObject);
                }
        }

        public void AddItem(ItemData itemData)
        {
                if (inventoryDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
                {
                        inventoryItem.AddToStack();
                }
                else
                {
                        InventoryItem item = new InventoryItem(itemData);
                        inventoryItems.Add(item);
                        inventoryDictionary.Add(itemData,item);
                }
        }

        public void RemoveItem(ItemData itemData)
        {
                if (inventoryDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
                {
                        if (inventoryItem.stackSize > 0)
                        {
                                inventoryItem.RemoveFromStack();
                        }
                        else
                        {
                                inventoryItems.Remove(inventoryItem);
                                inventoryDictionary.Remove(itemData);
                        }
                        
                }
                else
                {
                        Debug.Log("You don't have any such item");
                }
        }
}