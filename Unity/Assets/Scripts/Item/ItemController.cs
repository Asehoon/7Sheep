using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public List<Image> inventorySlots; // UI 슬롯들 (Image 컴포넌트 리스트)
    private int currentSlotIndex = 0;

    // 아이템과 충돌하면 아이템을 인벤토리에 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            AddItemToInventory(item); //인벤토리 추가
            Destroy(collision.gameObject); // 아이템 제거
        }
    }

    // 아이템을 인벤토리에 추가
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

        ItemMananger.Instance.ApplyItemEffect(item.itemType, gameObject, sprite); //효과적용
    }

    // 특정 슬롯만 삭제하고 나머지를 앞으로 당김
    public void ClearInventorySlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("Can't Delete.");
            return;
        }

        // 슬롯 앞으로 당기기
        for (int i = slotIndex; i < inventorySlots.Count - 1; i++)
        {
            inventorySlots[i].sprite = inventorySlots[i + 1].sprite;
            inventorySlots[i].color = inventorySlots[i + 1].color;
        }

        // 마지막 슬롯 비우기
        inventorySlots[inventorySlots.Count - 1].sprite = null;
        inventorySlots[inventorySlots.Count - 1].color = new Color(1, 1, 1, 0);

        // currentSlotIndex 재계산
        currentSlotIndex = 0;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].sprite == null)
            {
                currentSlotIndex = i;
                break;
            }
            currentSlotIndex = inventorySlots.Count; // 모두 차있으면 마지막으로
        }
    }
}
