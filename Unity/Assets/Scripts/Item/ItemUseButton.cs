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
<<<<<<< HEAD
                Debug.LogWarning("������ Ÿ�� ���� �ʿ�!");
                return;
            }
            Debug.Log(target);
=======
                Debug.LogWarning("������ Ÿ�� ���� �ʿ�!");
                return;
            }
>>>>>>> 6018ab6dcd23027e3821d563781bc700e414561a
            ItemMananger.Instance.ApplyItemEffect(item.itemType, target, slot.icon.sprite);
            itemController.ClearInventorySlot(slotIndex);
        }
    }
        
}
