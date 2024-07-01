using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
        private SpriteRenderer itemIcon;
        [SerializeField] private ItemData itemData;
        private void Awake()
        {
                itemIcon = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
                itemIcon.sprite = itemData.itemIcon;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
                Inventory.instance.AddItem(itemData);
                Destroy(gameObject);
        }
}