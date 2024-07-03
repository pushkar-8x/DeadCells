using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
        [SerializeField] private ItemData itemData;
        private void OnValidate()
        {
                GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
                gameObject.name = "Item - " + itemData.itemName;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
                Inventory.instance.AddItem(itemData);
                Destroy(gameObject);
        }
}