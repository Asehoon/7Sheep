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
        SpeedUp,
        HpUP
    }

    public static ItemMananger Instance;

    public GameObject effectUIPrefab;  // UI 프리팹
    public Transform uiGroup;          // VerticalLayoutGroup이 붙은 부모

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

        TopDownCharacterController movement = target.GetComponent<TopDownCharacterController>();
        TopDownWolfController enemy = target.GetComponent<TopDownWolfController>();

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
                    onStart: () =>
                    {
                        enemy.isItemOn = true;
                        enemy.speed /= 2;
                    },
                    onEnd: () => enemy.isItemOn = false,
                    targetName: target.name
                ));
                break;

            case ItemType.Trap:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    effectDuration,
                    sprite,
                    onStart: () =>
                    {
                        enemy.isItemOn = true;
                        enemy.speed = 0;
                    },
                    onEnd: () => enemy.isItemOn = false,
                    targetName: target.name
                ));
                break;
            case ItemType.SpeedUp:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    effectDuration,
                    sprite,
                    onStart: () =>
                    {
                        movement.speed *= 2;
                    },
                    onEnd: () => movement.speed/=2,
                    targetName: target.name
                ));
                break;
            case ItemType.HpUP:
                runningCoroutines[type] = StartCoroutine(RunTimedEffect(
                    type,
                    effectDuration,
                    sprite,
                    onStart: () =>
                    {
                        PlayerHP hp = movement.GetComponent<PlayerHP>();
                        if (hp != null)
                        {
                            hp.Heal(1); // 1만큼 HP 회복
                        }
                    },
                    onEnd: () => { },
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
            ItemType.SpeedDown => 10f,
            ItemType.Trap => 3f,
            ItemType.SpeedUp=> 30f,
            ItemType.HpUP =>1f,
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
