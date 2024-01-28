using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{

    public Sprite normalCursorImage;
    public Sprite cursorClickedImage;
    public bool isClicked = false;

    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.transform.position = GetMousePos();
        isClicked = Input.GetMouseButton(0);
        spriteRenderer.sprite = isClicked ? cursorClickedImage : normalCursorImage;
    }

    Vector3 GetMousePos()
    {
        var mosePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mosePos.z = 0;
        return mosePos;
    }
}
