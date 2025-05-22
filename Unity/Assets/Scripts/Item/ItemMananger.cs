using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cainos.PixelArtTopDown_Basic;
using Mono.Cecil.Cil;

public class ItemMananger : MonoBehaviour
{
    public enum ItemType
    {
        CameraZoom,
        SpeedDown,
        Trap,
    }

    public static ItemMananger Instance;

    public GameObject effectUIPrefab;  // UI 프리팹
    public Transform uiGroup;          // VerticalLayoutGroup이 붙은 부모

    private Dictionary<ItemType, float> activeEffects = new();
    private Dictionary<ItemType, Coroutine> runningCoroutines = new();

    float originalSpeed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 1. originalSpeed를 각 GameObject별로 저장
    private Dictionary<GameObject, float> originalSpeeds = new();

    public void ApplyItemEffect(ItemType type, GameObject target, Sprite sprite = null)
    {
        float effectDuration = GetEffectDuration(type);

        if (runningCoroutines.ContainsKey(type))
        {
            activeEffects[type] += effectDuration;
            return;
        }

        System.Action<float> SetSpeed = null;

        var movement = target.GetComponent<TopDownCharacterController>();
        if (movement != null)
        {
            if (!originalSpeeds.ContainsKey(target))
                originalSpeeds[target] = movement.speed;

            SetSpeed = value => movement.speed = value;
        }
        else
        {
            var enemy = target.GetComponent<TopDownWolfController>();
            if (!originalSpeeds.ContainsKey(target))
                originalSpeeds[target] = enemy.speed;

            SetSpeed = value => enemy.speed = value;
        }

        float originalSpeed = originalSpeeds[target];

        switch (type)
        {
            case ItemType.CameraZoom:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    effectDuration,
                    sprite,
                    onStart: () => Camera.main.orthographicSize *= 2,
                    onEnd: () => Camera.main.orthographicSize /= 2,
                    targetName: target.name
                ));
                break;

            case ItemType.SpeedDown:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    effectDuration,
                    sprite,
                    onStart: () => SetSpeed?.Invoke(originalSpeed * 0.5f),
                    onEnd: () => SetSpeed?.Invoke(originalSpeed),
                    targetName: target.name
                ));
                break;

            case ItemType.Trap:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    effectDuration,
                    sprite,
                    onStart: () => SetSpeed?.Invoke(0f),
                    onEnd: () => SetSpeed?.Invoke(originalSpeed),
                    targetName: target.name
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
            ItemType.Trap => 3f,
            _ => 5f
        };
    }

    private IEnumerator RunTimedEffect(
        ItemType type,
        float duration,
        Sprite sprite,
        System.Action onStart,
        System.Action onEnd,
        string targetName = "")
    {
        activeEffects[type] = duration;

        // UI 프리팹을 생성
        GameObject uiObject = Instantiate(effectUIPrefab, uiGroup);
        TMP_Text text = uiObject.GetComponentInChildren<TMP_Text>(true);
        Image image = uiObject.GetComponentInChildren<Image>(true);
        if (image != null) image.sprite = sprite;

        onStart?.Invoke();

        while (activeEffects[type] > 0)
        {
            if (text != null)
                text.text = $"{targetName} - {type}: {activeEffects[type]:F1}s";

            activeEffects[type] -= Time.deltaTime;
            yield return null;
        }

        onEnd?.Invoke();

        activeEffects.Remove(type);
        runningCoroutines.Remove(type);

        Destroy(uiObject); // 효과가 끝나면 UI 제거
    }
}
