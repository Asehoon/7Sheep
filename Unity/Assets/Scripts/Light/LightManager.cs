using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    public Light2D globalLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
    public float transitionDuration = 10.0f;

    public static int PlayerGold = 500;

    private bool lightsAreEnabled = true;
    public Light2D[] childLights;
    private Coroutine timeFlowCoroutine;
    private Coroutine lightingCoroutine;
    private Color targetColor;

    [Header("Time")]
    public TMP_Text TimeText;

    [Header("Player")]
    public TMP_Text PlayerGoldText;

    [Header("Clock Hands")]
    public Transform hourHand;

    private float realTime = 1f;
    private float time;

    // 오후 9시 시작 (21 * 60 = 1260분)
    private int gameTime = 900;

    public static bool isNight = false;

    [Header("Target Transforms")]
    public Transform playerTransform;
    public Transform wolfTransform;
    public Transform lampTransform;

    public void Awake()
    {
        childLights[0].transform.position = playerTransform.position;
        childLights[1].transform.position = wolfTransform.position;
        childLights[2].transform.position = lampTransform.position;

        SetLights(false);
    }
    public void Start()
    {
        timeFlowCoroutine = StartCoroutine(TimeFlow());
    }

    private IEnumerator TimeFlow()
    {
        while (true)
        {
            yield return new WaitForSeconds(realTime);
            gameTime += 10;

            if (gameTime >= 1440) // 하루 1440분을 넘기면 초기화
            {
                gameTime = gameTime % 1440;
            }

            TimeTextChange();
            UpdateClockRotation();
            UpdateLightingTransition();
        }
    }

    private void UpdateClockRotation()
    {
        float hour24 = (gameTime / 60f); // 현재 시각 (0~24)

        // 하루 24시간 = 360도 반시계 회전, 기준점 오후 3시(-90도)
        float angle = ((hour24 - 15f + 24f) % 24f) * -15f;

        hourHand.localRotation = Quaternion.Euler(0, 0, -90f + angle);
    }






    public void TimeTextChange(bool isDay = false)
    {
        int totalMinutes = gameTime % 1440;
        int hour = totalMinutes / 60;
        int minute = totalMinutes % 60;

        string period = (hour >= 12) ? "오후  " : "오전  ";
        int displayHour = hour % 12;
        if (displayHour == 0) displayHour = 12;

        string minuteStr = minute.ToString("D2");
        string hourStr = displayHour.ToString("D2");

        if (isDay)
            TimeText.text = "오전  06 : 00";
        else
            TimeText.text = period + hourStr + " : " + minuteStr;
    }

    private void UpdateLightingTransition()
    {
        int hour = (gameTime / 60) % 24;
        int minute = gameTime % 60;
        float currentTime = hour + (minute / 60f); // 0~24

        Color newTargetColor = globalLight.color;

        if (currentTime >= 8f && currentTime < 9f) // 밤 → 낮 전환
        {
            float t = currentTime - 8f;
            newTargetColor = Color.Lerp(nightColor, dayColor, t);
            if (lightsAreEnabled)
                SetLights(false);
        }
        else if (currentTime >= 9f && currentTime < 21f) // 낮
        {
            newTargetColor = dayColor;
        }
        else if (currentTime >= 20f && currentTime < 21f) // 낮 → 밤 전환
        {
            float t = currentTime - 20f;
            newTargetColor = Color.Lerp(dayColor, nightColor, t);
        }
        else // 밤
        {
            newTargetColor = nightColor;
            if (!lightsAreEnabled)
                SetLights(true);
        }

        if (targetColor != newTargetColor)
        {
            targetColor = newTargetColor;

            if (lightingCoroutine != null)
                StopCoroutine(lightingCoroutine);

            lightingCoroutine = StartCoroutine(LerpLightColor(globalLight, targetColor, transitionDuration));
        }
    }

    private IEnumerator LerpLightColor(Light2D light, Color targetColor, float duration)
    {
        Color startColor = light.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            light.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        light.color = targetColor;
    }

    private void SetLights(bool enable)
    {
        lightsAreEnabled = enable;
        foreach (Light2D light in childLights)
        {
            light.enabled = enable;
        }
    }
}
