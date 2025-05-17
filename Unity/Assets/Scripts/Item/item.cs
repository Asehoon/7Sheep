using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemMananger.ItemType itemType;
    public Texture2D texture2D;

    // 아이템 효과가 적용될 대상
    public GameObject target;

    // 예를 들어 인스펙터에서 수동 할당하거나,
    // 생성/획득 시점에 SetTarget()으로 할당 가능
    public void SetTarget(GameObject targetObj)
    {
        target = targetObj;
    }
}
