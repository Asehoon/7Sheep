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
                Debug.LogWarning("ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å¸ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ê¿ï¿½!");
                return;
            }
            Debug.Log(target);
=======
                Debug.LogWarning("¾ÆÀÌÅÛ Å¸°Ù ÁöÁ¤ ÇÊ¿ä!");
                return;
            }
>>>>>>> 6018ab6dcd23027e3821d563781bc700e414561a
            ItemMananger.Instance.ApplyItemEffect(item.itemType, target, slot.icon.sprite);
            itemController.ClearInventorySlot(slotIndex);
        }
    }
        
}
