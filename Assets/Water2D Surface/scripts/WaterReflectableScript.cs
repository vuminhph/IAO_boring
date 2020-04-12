// 2016 - Damien Mayance (@Valryon)
// Source: https://github.com/valryon/water2d-unity/
using UnityEngine;

/// <summary>
/// Automagically create a water reflect for a sprite.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class WaterReflectableScript : MonoBehaviour
{
    #region Members

    [Header("Reflect properties")]
    public Vector3 localPosition = new Vector3(0, -2, 0);
    Vector3 localRotation = new Vector3(0, 0, 0);
    public Vector3 scaleOffset = new Vector3(0, 0, 0);
    [Tooltip("Optionnal: force the reflected sprite. If null it will be a copy of the source.")]
    public Sprite sprite;
    public string spriteLayer = "Default";
    public int spriteLayerOrder = -5;

    private SpriteRenderer spriteSource;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region Timeline

    void Awake()
    {
        GameObject reflectGo = new GameObject("Reflection");
        reflectGo.transform.parent = this.transform;
        reflectGo.transform.localPosition = localPosition;
        reflectGo.transform.localRotation = Quaternion.Euler(localRotation);
        reflectGo.tag = "Reflection";
        Vector3 parentScale = reflectGo.transform.parent.transform.localScale;
        // reflectGo.transform.localScale = new Vector3(parentScale.x, -parentScale.y, parentScale.z) + scaleOffset;
        reflectGo.transform.localScale = new Vector3(1, -1, 1) + scaleOffset;

        spriteRenderer = reflectGo.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = spriteLayer;
        spriteRenderer.sortingOrder = spriteLayerOrder;
        spriteSource = GetComponent<SpriteRenderer>();
    }

    // void OnDestroy()
    // {
    //   if (spriteRenderer != null)
    //   {
    //     Destroy(spriteRenderer.gameObject);
    //   }
    // }

    public bool active = true;
    void LateUpdate()
    {
        if (active == true)
        {
            if (spriteSource != null)
            {
                if (sprite == null)
                {
                    spriteRenderer.sprite = spriteSource.sprite;
                }
                else
                {
                    spriteRenderer.sprite = sprite;
                }
                spriteRenderer.flipX = spriteSource.flipX;
                spriteRenderer.flipY = spriteSource.flipY;
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.25f);
            }
        }
        else spriteRenderer.sprite = null;
    }

    #endregion
}