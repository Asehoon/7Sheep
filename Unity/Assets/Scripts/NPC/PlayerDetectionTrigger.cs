using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerDetectionTrigger : MonoBehaviour
{
    [SerializeField] private Image buttonImage1; // 버튼 이미지 1
    [SerializeField] private Image buttonImage2; // 버튼 이미지 2
    [SerializeField] private Vector3 interactionIconOffset; // UI 위치 오프셋

    private bool isPlayerInside = false;
    private Coroutine blinkCoroutine;
    private Coroutine positionCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = true;

        buttonImage1.gameObject.SetActive(true);
        buttonImage2.gameObject.SetActive(false);

        positionCoroutine = StartCoroutine(UpdateIconPosition());
        blinkCoroutine = StartCoroutine(BlinkButton());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = false;

        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (positionCoroutine != null) StopCoroutine(positionCoroutine);

        buttonImage1.gameObject.SetActive(false);
        buttonImage2.gameObject.SetActive(false);
    }

    private IEnumerator BlinkButton()
    {
        while (isPlayerInside)
        {
            buttonImage1.gameObject.SetActive(true);
            buttonImage2.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            buttonImage1.gameObject.SetActive(false);
            buttonImage2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator UpdateIconPosition()
    {
        while (isPlayerInside)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            buttonImage1.rectTransform.position = screenPos + interactionIconOffset;
            buttonImage2.rectTransform.position = screenPos + interactionIconOffset;
            yield return null;
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("구출 성공!");
            Rescue();
        }
    }

    private void Rescue()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (positionCoroutine != null) StopCoroutine(positionCoroutine);

        buttonImage1.gameObject.SetActive(false);
        buttonImage2.gameObject.SetActive(false);

        // 구출 처리
        SpriteRenderer spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite sheepSprite = spriteRenderer.sprite;
            GameManager.Instance?.RescueSheep(sheepSprite);
        }

        Destroy(transform.parent.gameObject);
    }

}
