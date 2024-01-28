using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FingerMovement : MonoBehaviour
{

    public static FingerMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de FingerMovement dans la scène");
            return;
        }

        instance = this;
    }

    public float rangeMin = -3f;
    public float rangeMax = 5f;

    public float moveSpeed = 3f;
    public float currentMoveSpeed = 0f;
    public float releaseFingerSpeed = 4f;

    public bool isDragged = false;
    public bool canBeDragged = true;

    private float horizontalMovement;

    public Rigidbody2D rb;

    public Vector3 velocity = Vector3.zero; //Vector3.zero -> Vector3(0, 0, 0)

    public float speedLimit = 20f;

    public bool isTooFast = false;

    public GameObject tooFastUi;

    public GameObject middleFinger;

    public Image sliderBackground;

    Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = middleFinger.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(releaseFingerSpeed * -1, rb.velocity.y);
        if (isSuccess())
        {
            Success.instance.successMessage();
            targetVelocity = new Vector2(0, rb.velocity.y);
            canBeDragged = false;
            GameManager.instance.Success(1);
            GameManager.instance.isSuccess = true;
        }
        if (rb.transform.position.x < rangeMin) rb.velocity = new Vector2(0, 0);
        else if(isDragged == false && !isSuccess()) {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f); //Ramène le doigt au début
        }
        updateIsTooFast();
        tooFastUi.SetActive(isTooFast);
        sliderBackground.color = isTooFast ? Color.red : Color.green;
        isDragged = false;
        MovePlayer(0); //Lance notre fonction Moveplayer en envoyant notre mouvement calculer au dessus
        currentMoveSpeed = GetCurrentMoveSpeed();

        middleFinger.transform.position = new Vector3(
            (rb.transform.position.x - 3) / 2 - 1, 
            rb.transform.position.y, 
            rb.transform.position.z);

        middleFinger.transform.localScale = new Vector3(
                startScale.x + (rb.transform.position.x + 3) * 1.5f,
                startScale.y,
                startScale.z
            );
    }

    public void MovePlayer(float _horizontalMovement) //Fonction qui fait bouger notre personnage
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y); //Récupère le mouvement en 2D du joueur avec un Vector2
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f); //Donne le mouvement au joueur
    }

    void OnMouseDrag()
    {
        isDragged = true;
        if(canBeDragged) MovePlayer((GetMousePos().x - rb.transform.position.x) * moveSpeed);
    }

    Vector3 GetMousePos()
    {
        var mosePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mosePos.z = 0;
        mosePos.y = 0;
        if (mosePos.x < rangeMin) mosePos.x = rangeMin;
        if (mosePos.x > rangeMax) mosePos.x = rangeMax;
        return mosePos;
    }

    float GetCurrentMoveSpeed()
    {
        return Mathf.Abs(rb.velocity.x);
    }

    bool isSuccess()
    {
        return !isTooFast && (rangeMax - rb.transform.position.x) <= 0.1f ;
    }

    void updateIsTooFast()
    {
        if(isDragged) isTooFast = isTooFast || currentMoveSpeed > speedLimit;
        else isTooFast = false;
    }
}
