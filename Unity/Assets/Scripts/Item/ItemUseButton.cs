using UnityEngine;

public class ItemUseButton : MonoBehaviour
{
    public ItemController itemController;

    public void OnInventoryButtonClick(int slotIndex)
    {
        ItemSlot slot = itemController.inventorySlots[slotIndex];

        if (slot.isFilled || slot.storedItem!=null)
        {
            Item item = slot.storedItem;
            GameObject target = item.target;
            if (target == null)
            {
                Debug.LogWarning("������ Ÿ�� ���� �ʿ�!");
                return;
            }
            Debug.Log(target);
            ItemMananger.Instance.ApplyItemEffect(item.itemType, target, slot.icon.sprite);
            itemController.ClearInventorySlot(slotIndex);
        }
    }
        
}
