using UnityEngine;
using UnityEngine.Tilemaps;

public class HouseController : MonoBehaviour
{
    public Tilemap roofTilemap;    // TilemapRenderer ¸»°í Tilemap
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
    public void ToggleLayer()
    {

    }

    public void EnterHouse()
    {
        isInside = true;

        if (roofTilemap != null)
            roofTilemap.gameObject.SetActive(false);  // ÁöºØ ²¨¹ö¸²
    }

    public void ExitHouse()
    {
        isInside = false;

        if (roofTilemap != null)
            roofTilemap.gameObject.SetActive(true);   // ÁöºØ ´Ù½Ã ÄÑ±â

        // interior´Â ±×´ë·Î µÒ (²ôÁö ¾ÊÀ½)
    }
}
