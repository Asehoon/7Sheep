using System.Collections;
using System.Collections.Generic;
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
    public bool is_Chest = false;
    public bool is_Open = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (is_Open) return;


        isPlayerInside = true;

        buttonImage1.gameObject.SetActive(true);
        buttonImage2.gameObject.SetActive(false);

        positionCoroutine = StartCoroutine(UpdateIconPosition());
        blinkCoroutine = StartCoroutine(BlinkButton());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (is_Open) return;

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
    // -3~-1.5 또는 1.5~3 중 랜덤 선택
    float GetDonutRandom()
    {
        bool negative = Random.value < 0.5f;
        return negative
            ? Random.Range(-2f, -1f)
            : Random.Range(1f, 2f);
    }
    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            if (is_Open)
                return;

            if (is_Chest)
            {

                // 1. Chest 상태 전환
                Transform closedChest = transform.Find("PF Props Chest");
                Transform openChest = transform.Find("PF Props Chest Open");

                if (closedChest != null) closedChest.gameObject.SetActive(false);
                if (openChest != null) openChest.gameObject.SetActive(true);

                GameObject itemGroup = GameObject.Find("ITEM");

                if (itemGroup != null)
                {
                    int dropCount = Random.Range(1, 4); // 1~3개 드랍
                    int childCount = itemGroup.transform.childCount;

                    // 0부터 childCount-1까지 인덱스를 만든다
                    List<int> indices = new List<int>();
                    for (int i = 0; i < childCount; i++)
                    {
                        indices.Add(i);
                    }

                    // dropCount만큼 인덱스를 랜덤하게 선택 (중복 없이)
                    List<int> selectedIndices = new List<int>();
                    for (int i = 0; i < dropCount && indices.Count > 0; i++)
                    {
                        int randIndex = Random.Range(0, indices.Count);
                        selectedIndices.Add(indices[randIndex]);
                        indices.RemoveAt(randIndex); // 중복 방지
                    }

                    foreach (int idx in selectedIndices)
                    {
                        Transform itemTransform = itemGroup.transform.GetChild(idx);
                        GameObject droppedItem = Instantiate(itemTransform.gameObject);
                        droppedItem.SetActive(true); // 원본이 비활성화일 경우 대비

                        Vector3 randomOffset = new Vector3(
                            GetDonutRandom(), // X축 퍼짐
                            GetDonutRandom(), // Y축 퍼짐
                            0f                // Z축 고정
                        );

                        Vector3 dropPosition = transform.position + randomOffset;
                        dropPosition.z = transform.position.z; // Z축 고정

                        droppedItem.transform.position = dropPosition;
                        Debug.Log("Dropped item: " + droppedItem.name + " at " + droppedItem.transform.position);
                    }

                    // 한 번만 열리게 할 경우 처리
                    is_Open = true;
                    isPlayerInside = false;

                    if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
                    if (positionCoroutine != null) StopCoroutine(positionCoroutine);

                    buttonImage1.gameObject.SetActive(false);
                    buttonImage2.gameObject.SetActive(false);
                }
            }

            else
            {
                Debug.Log("구출 성공!");
                Rescue();
            }
      
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
