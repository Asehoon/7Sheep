using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public int maxHP = 5;
    public int currentHP;

    public GameObject heartPrefab;
    public Transform heartContainer;

    private List<GameObject> heartObjects = new List<GameObject>();

    public bool isInvincible = false;
    public float invincibleDuration = 2f;

    private SpriteRenderer spriteRenderer; // 플레이어의 SpriteRenderer

    void Start()
    {
        currentHP = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CreateHearts();
    }

    private void CreateHearts()
    {
        for (int i = 0; i < currentHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heartObjects.Add(heart);
        }
    }

    private void RemoveHeart()
    {
        if (heartObjects.Count > 0)
        {
            GameObject lastHeart = heartObjects[heartObjects.Count - 1];
            heartObjects.RemoveAt(heartObjects.Count - 1);
            Destroy(lastHeart);
        }
    }

    private void AddHeart()
    {
        if (heartObjects.Count < maxHP)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heartObjects.Add(heart);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        for (int i = 0; i < amount; i++)
        {
            RemoveHeart();
        }

        StartCoroutine(Invincibility());

        if (currentHP <= 0)
        {
            Debug.Log("Player Dead");

            BGMManager.Instance?.ToggleMute();
            SceneManager.LoadScene(3); //failed scene
            // TODO: 사망 처리
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        for (int i = 0; i < amount; i++)
        {
            if (heartObjects.Count < currentHP)
                AddHeart();
        }
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;

        float elapsed = 0f;
        float blinkInterval = 0.1f;

        while (elapsed < invincibleDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvincible = false;
    }
}
