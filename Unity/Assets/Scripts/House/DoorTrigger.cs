using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private HouseController houseController;

    private void Start()
    {
        houseController = GetComponentInParent<HouseController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            houseController?.ToggleHouse();
        }
    }
}
