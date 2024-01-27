using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private float horizontalMovement;

    public Rigidbody2D rb;

    public Vector3 velocity = Vector3.zero; //Vector3.zero -> Vector3(0, 0, 0)

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

        //horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; //Calcul notre mouvemment horizontal avec Input.GetAxis("Horizontal") récupère input (->, <-, A, D)
        Vector3 targetVelocity = new Vector2(releaseFingerSpeed * -1, rb.velocity.y); //Récupère le mouvement en 2D du joueur avec un Vector2
        if (rb.transform.position.x < rangeMin) rb.velocity = new Vector2(0, 0);
        else if(isDragged == false) rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f); //Ramène le doigt au début
        isDragged = false;
        MovePlayer(horizontalMovement); //Lance notre fonction Moveplayer en envoyant notre mouvement calculer au dessus
        currentMoveSpeed = GetCurrentMoveSpeed();
    }

    public void MovePlayer(float _horizontalMovement) //Fonction qui fait bouger notre personnage
    {
        if (horizontalMovement != 0) Debug.Log("Movement");
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y); //Récupère le mouvement en 2D du joueur avec un Vector2
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f); //Donne le mouvement au joueur
    }

    void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        //transform.position = GetMousePos();
        isDragged = true;
        MovePlayer((GetMousePos().x - rb.transform.position.x) * moveSpeed);
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
        Debug.Log(rb.velocity.x);
        return Mathf.Abs(rb.velocity.x);
    }
}
