using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    private ItemMananger.ItemType itemType;
    public bool isFilled;
    public Item storedItem;

    public void SetSlot(Item item)
    {
        storedItem = item;
        itemType = item.itemType;

        if (item.texture2D != null)
        {
            Texture2D texture = item.texture2D;
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            icon.sprite = sprite;
            icon.color = Color.white;
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
        }
        isFilled = true;
        storedItem.target = item.target;
    }

    public void ClearSlot()
    {
        storedItem = null;
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 255);
        isFilled = false;
    }
}
