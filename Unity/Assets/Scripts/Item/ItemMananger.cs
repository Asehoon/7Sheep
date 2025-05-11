using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D;

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

    public void ApplyItemEffect(ItemType type, GameObject target, Sprite sprite = null)
    {
        switch (type)
        {
            case ItemType.CameraZoom:
                CameraZoomEffect(sprite);
                break;
            case ItemType.SpeedDown:
                SpeedDown(target,sprite);
                break;
        }
    }

    private void CameraZoomEffect(Sprite sprite)
    {
        Camera cam = Camera.main;
        float originalSize = cam.orthographicSize;
        float targetSize = originalSize * 2;
        
        cam.orthographicSize = targetSize;
        StartCoroutine(ZoomSequence(originalSize, sprite, 30f));
    }

    private void SpeedDown(GameObject target, Sprite sprite)
    {
        var movement = target.GetComponent<TopDownCharacterController>();
        float originalSpeed = movement.speed;
        movement.speed *= 0.5f;
        StartCoroutine(SpeedDownSequence(movement, originalSpeed, sprite, 5f));
    }

    private IEnumerator ZoomSequence(float originalSize, Sprite sprite, float duration)
    {
        yield return StartCoroutine(Timer(duration, sprite));
        Camera cam= Camera.main;
        cam.orthographicSize = originalSize;
    }

    private IEnumerator SpeedDownSequence(TopDownCharacterController movement, float originalSpeed, Sprite sprite, float duration)
    {
        yield return StartCoroutine(Timer(duration, sprite));  
        movement.speed = originalSpeed;                        
    }



    private IEnumerator Timer (float timer, Sprite sprite)
    {
        Zoom.SetActive(true); // GameObject 활성화

        TMP_Text text = Zoom.GetComponentInChildren<TMP_Text>(true);
        Image image = Zoom.GetComponentInChildren<Image>(true);

        image.sprite = sprite;

        while (timer > 0)
        {
            if (text != null)
                text.text = $"Time: {timer:F1}s";
            timer -= Time.deltaTime;
            yield return null;
        }

        Zoom.SetActive(false); // GameObject 비활성화
    }
}
