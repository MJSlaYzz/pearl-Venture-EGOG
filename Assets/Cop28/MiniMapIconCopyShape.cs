using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIconCopyShape : MonoBehaviour
{
    #region Variables
    [SerializeField] private SpriteRenderer orginalSpriteRendere;
    [SerializeField] private SpriteRenderer miniMapSpriteRendere;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        orginalSpriteRendere = transform.parent.GetComponent<SpriteRenderer>();
        miniMapSpriteRendere = GetComponent<SpriteRenderer>();
        if (miniMapSpriteRendere != null)
        {
            miniMapSpriteRendere.sprite = orginalSpriteRendere.sprite;
        }
    }
    // Update is called once per frame
    void Update()
    {
        miniMapSpriteRendere.sprite = orginalSpriteRendere.sprite;
        if (orginalSpriteRendere.gameObject.GetComponent<PolygonCollider2D>().enabled == false)
        {
            miniMapSpriteRendere.enabled = false;
        }
    }
}
