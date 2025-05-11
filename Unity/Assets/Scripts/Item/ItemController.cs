using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public List<Image> inventorySlots; // UI ���Ե� (Image ������Ʈ ����Ʈ)
    private int currentSlotIndex = 0;

    // �����۰� �浹�ϸ� �������� �κ��丮�� �߰�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            AddItemToInventory(item); //�κ��丮 �߰�
            Destroy(collision.gameObject); // ������ ����
        }
    }

    // �������� �κ��丮�� �߰�
    private void AddItemToInventory(Item item)
    {
        if (currentSlotIndex >= inventorySlots.Count)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        Texture2D texture = item.texture2D;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        inventorySlots[currentSlotIndex].sprite = sprite;
        inventorySlots[currentSlotIndex].color = Color.white;
        currentSlotIndex++;

        ItemMananger.Instance.ApplyItemEffect(item.itemType, gameObject, sprite); //ȿ������
    }

    // Ư�� ���Ը� �����ϰ� �������� ������ ���
    public void ClearInventorySlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("Can't Delete.");
            return;
        }

        // ���� ������ ����
        for (int i = slotIndex; i < inventorySlots.Count - 1; i++)
        {
            inventorySlots[i].sprite = inventorySlots[i + 1].sprite;
            inventorySlots[i].color = inventorySlots[i + 1].color;
        }

        // ������ ���� ����
        inventorySlots[inventorySlots.Count - 1].sprite = null;
        inventorySlots[inventorySlots.Count - 1].color = new Color(1, 1, 1, 0);

        // currentSlotIndex ����
        currentSlotIndex = 0;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].sprite == null)
            {
                currentSlotIndex = i;
                break;
            }
            currentSlotIndex = inventorySlots.Count; // ��� �������� ����������
        }
    }
}
