using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public List<ItemSlot> inventorySlots;
    private int currentSlotIndex = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            AddItemToInventory(item);
            Destroy(collision.gameObject);
        }
    }

    private void AddItemToInventory(Item item)
    {
        if (currentSlotIndex >= inventorySlots.Count)
        {
            Debug.Log("Inventory is full!");
            return;
        }
        inventorySlots[currentSlotIndex].SetSlot(item);
        currentSlotIndex++;
    }


    public void ClearInventorySlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("Invalid slot index.");
            return;
        }

        int maxCount = inventorySlots.Count - 1;

        // 뒤의 슬롯들을 앞으로 당김
        for (int i = slotIndex; i < maxCount; i++)
        {
            if (inventorySlots[i + 1].isFilled)
            {
                inventorySlots[i].SetSlot(inventorySlots[i + 1].storedItem);
            }
            else
            {
                inventorySlots[i].ClearSlot();
                currentSlotIndex = i;
                return; // 뒤는 비었으니 더 안 당겨도 됨
            }
        }

        // 마지막 슬롯은 항상 비워줌
        inventorySlots[maxCount].ClearSlot();

        // currentSlotIndex 업데이트sdds
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].isFilled)
            {
                currentSlotIndex = i;
                return;
            }
        }

        currentSlotIndex = inventorySlots.Count;
    }

}
