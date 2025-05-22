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

        // ���� ���Ե��� ������ ���
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
                return; // �ڴ� ������� �� �� ��ܵ� ��
            }
        }

        // ������ ������ �׻� �����
        inventorySlots[maxCount].ClearSlot();

        // currentSlotIndex ������Ʈsdds
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
