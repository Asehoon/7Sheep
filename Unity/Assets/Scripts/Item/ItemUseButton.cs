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
                Debug.LogWarning("아이템 타겟 지정 필요!");
                return;
            }
            ItemMananger.Instance.ApplyItemEffect(item.itemType, target, slot.icon.sprite);
            itemController.ClearInventorySlot(slotIndex);
        }
    }
        
}
