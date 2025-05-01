using UnityEngine;

public class BushTriggerHandler : MonoBehaviour
{
    public enum UnitType { Player, Enemy }
    public UnitType unitType;

    public float transparentAlpha = 0.5f;
    private SpriteRenderer sr;
    private Color originalColor;
    private int originalLayer;

    private bool isInBush = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        originalLayer = gameObject.layer;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bush")) return;

        isInBush = true;

        if (unitType == UnitType.Player)
        {
            BushController.Instance?.SetPlayerInBush(isInBush);
            SetTransparency(isInBush);
        }
        else if (unitType == UnitType.Enemy)
        {
            SetTransparency(isInBush);
            BushController.Instance?.RegisterEnemyInBush(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Bush")) return;

        isInBush = false;

        if (unitType == UnitType.Player)
        {
            BushController.Instance?.SetPlayerInBush(isInBush);
            SetTransparency(isInBush);
        }
        else if (unitType == UnitType.Enemy)
        {
            SetTransparency(isInBush);
            BushController.Instance?.UnregisterEnemyFromBush(this);
        }
    }

    public void SetTransparency(bool transparent)
    {
        if (sr == null) return;
        Color c = sr.color;
        c.a = transparent ? transparentAlpha : originalColor.a;
        sr.color = c;
    }

    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
