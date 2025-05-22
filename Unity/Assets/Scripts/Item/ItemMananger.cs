using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cainos.PixelArtTopDown_Basic;
<<<<<<< HEAD
using Mono.Cecil.Cil;
=======
>>>>>>> 6018ab6dcd23027e3821d563781bc700e414561a

public class ItemMananger : MonoBehaviour
{
    public enum ItemType
    {
        CameraZoom,
        SpeedDown,
        Trap,
    }

    public static ItemMananger Instance;

<<<<<<< HEAD
    public GameObject effectUIPrefab;  // UI 프리팹
    public Transform uiGroup;          // VerticalLayoutGroup이 붙은 부모

    private Dictionary<ItemType, float> activeEffects = new();
    private Dictionary<ItemType, Coroutine> runningCoroutines = new();

    float originalSpeed;
=======
    public GameObject Zoom;
    public GameObject Speed;

    private Dictionary<ItemType, float> activeEffects = new();
    private Dictionary<ItemType, Coroutine> runningCoroutines = new();
>>>>>>> 6018ab6dcd23027e3821d563781bc700e414561a

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

<<<<<<< HEAD
    // 1. originalSpeed를 각 GameObject별로 저장
    private Dictionary<GameObject, float> originalSpeeds = new();

=======
>>>>>>> 6018ab6dcd23027e3821d563781bc700e414561a
    public void ApplyItemEffect(ItemType type, GameObject target, Sprite sprite = null)
    {
        float effectDuration = GetEffectDuration(type);

        if (runningCoroutines.ContainsKey(type))
<<<<<<< HEAD
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
=======
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
>>>>>>> 6018ab6dcd23027e3821d563781bc700e414561a
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
