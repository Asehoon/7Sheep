using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Image> sheepIcons; // UI �̹����� �ޱ�

    private HashSet<Sprite> rescuedSheep = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RescueSheep(Sprite sheepSprite)
    {
        if (rescuedSheep.Contains(sheepSprite)) return;

        rescuedSheep.Add(sheepSprite);

        foreach (var image in sheepIcons)
        {
            if (image.sprite == sheepSprite)
            {
                Color color = image.color;
                color.a = 1f; // ���ĸ� 255�� ����
                image.color = color;
                break;
            }
        }

        if (rescuedSheep.Count >= 7)
        {

            BGMManager.Instance.PlaySuccess();
            SceneManager.LoadScene(2); //���� scene
        }
    }
}
