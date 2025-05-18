using UnityEngine;
using UnityEngine.Tilemaps;

public class HouseController : MonoBehaviour
{
    public Tilemap roofTilemap;    // TilemapRenderer ���� Tilemap
    public GameObject interior;

    public bool isInside = false;

    public void ToggleHouse()
    {
        if (!isInside)
        {
            EnterHouse();
        }
        else
        {
            ExitHouse();
        }
    }

    private void EnterHouse()
    {
        isInside = true;

        if (roofTilemap != null)
            roofTilemap.gameObject.SetActive(false);  // ���� ������
    }

    private void ExitHouse()
    {
        isInside = false;

        if (roofTilemap != null)
            roofTilemap.gameObject.SetActive(true);   // ���� �ٽ� �ѱ�

        // interior�� �״�� �� (���� ����)
    }
}
