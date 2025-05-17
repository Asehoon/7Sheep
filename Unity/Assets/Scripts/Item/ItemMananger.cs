using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cainos.PixelArtTopDown_Basic;

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
    public GameObject Speed;

    private Dictionary<ItemType, float> activeEffects = new();
    private Dictionary<ItemType, Coroutine> runningCoroutines = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ApplyItemEffect(ItemType type, GameObject target, Sprite sprite = null)
    {
        float effectDuration = GetEffectDuration(type);

        if (runningCoroutines.ContainsKey(type))
        {
            activeEffects[type] += effectDuration;
            return;
        }

        switch (type)
        {
            case ItemType.CameraZoom:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    Zoom,
                    effectDuration,
                    sprite,
                    onStart: () =>
                    {
                        Camera.main.orthographicSize *= 2;
                    },
                    onEnd: () =>
                    {
                        Camera.main.orthographicSize /= 2;
                    }
                ));
                break;

            case ItemType.SpeedDown:
                var movement = target.GetComponent<TopDownCharacterController>();
                float originalSpeed = movement.speed;

                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    Speed,
                    effectDuration,
                    sprite,
                    onStart: () => movement.speed *= 0.5f,
                    onEnd: () => movement.speed = originalSpeed
                ));
                break;

            case ItemType.Trap:
                var collider = target.GetComponent<Collider2D>();

                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    Speed, // 임시로 Speed UI 사용하거나 새 UI GameObject 추가
                    effectDuration,
                    sprite,
                    onStart: () => collider.enabled = false,
                    onEnd: () => collider.enabled = true
                ));
                break;
        }
    }

    private float GetEffectDuration(ItemType type)
    {
        return type switch
        {
            ItemType.CameraZoom => 30f,
            ItemType.SpeedDown => 5f,
            ItemType.Trap => 10f,
            _ => 5f
        };
    }

    private IEnumerator RunTimedEffect(
        ItemType type,
        GameObject uiObject,
        float duration,
        Sprite sprite,
        System.Action onStart,
        System.Action onEnd)
    {
        activeEffects[type] = duration;

        uiObject.SetActive(true);
        TMP_Text text = uiObject.GetComponentInChildren<TMP_Text>(true);
        Image image = uiObject.GetComponentInChildren<Image>(true);
        if (image != null) image.sprite = sprite;

        onStart?.Invoke();

        while (activeEffects[type] > 0)
        {
            if (text != null)
                text.text = $"Time: {activeEffects[type]:F1}s";

            activeEffects[type] -= Time.deltaTime;
            yield return null;
        }

        onEnd?.Invoke();

        uiObject.SetActive(false);
        activeEffects.Remove(type);
        runningCoroutines.Remove(type);
    }
}
