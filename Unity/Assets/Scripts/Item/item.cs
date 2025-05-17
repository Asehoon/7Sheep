using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemMananger.ItemType itemType;
    public Texture2D texture2D;

    // ������ ȿ���� ����� ���
    public GameObject target;

    // ���� ��� �ν����Ϳ��� ���� �Ҵ��ϰų�,
    // ����/ȹ�� ������ SetTarget()���� �Ҵ� ����
    public void SetTarget(GameObject targetObj)
    {
        target = targetObj;
    }
}
