using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            ItemMananger.Instance.ApplyItemEffect(item.itemType, gameObject); // gameObject = 플레이어 자신
            Destroy(other.gameObject); // 아이템 제거
        }
    }
}
