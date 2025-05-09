using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using TMPro;

public class ItemMananger : MonoBehaviour
{
    public enum ItemType
    {
        CameraZoom,
        SpeedDown,
        Trap,
    }

    public static ItemMananger Instance;

    public GameObject Zoom; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ApplyItemEffect(ItemType type, GameObject target)
    {
        switch (type)
        {
            case ItemType.CameraZoom:
                StartCoroutine(CameraZoomEffect());
                break;
            case ItemType.SpeedDown:
                StartCoroutine(SpeedDown(target));
                break;
        }
    }

    private IEnumerator CameraZoomEffect()
    {
        Camera cam = Camera.main;
        float originalSize = cam.orthographicSize;
        float targetSize = originalSize * 2;
        float duration = 30f;
        float timer = duration;

        cam.orthographicSize = targetSize;

        Zoom.SetActive(true); // GameObject 활성화

        TMP_Text text = Zoom.GetComponentInChildren<TMP_Text>(true);

        while (timer > 0)
        {
            if (text != null)
                text.text = $"Zoom Time: {timer:F1}s";
            timer -= Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = originalSize;
        Zoom.SetActive(false); // GameObject 비활성화
    }

    private IEnumerator SpeedDown(GameObject target)
    {
        var movement = target.GetComponent<TopDownCharacterController>();
        if (movement != null)
        {
            float originalSpeed = movement.speed;
            movement.speed *= 0.5f;
            yield return new WaitForSeconds(3f);
            movement.speed = originalSpeed;
        }
    }
}
