using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class FingerSnakeMovement : MonoBehaviour
{

    public static FingerSnakeMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de FingerSnakeMovement dans la scène");
            return;
        }

        instance = this;
    }

    public float rangeMin = -3f;

    public float moveSpeed = 3f;
    public float currentMoveSpeed = 0f;
    public float releaseFingerSpeed = 4f;

    public bool isDragged = false;
    public bool canBeDragged = true;

    private float horizontalMovement;
    private float verticalMovement;

    public Rigidbody2D rb;

    public Vector3 velocity = Vector3.zero; //Vector3.zero -> Vector3(0, 0, 0)

    public float speedLimit = 20f;

    public bool isTooFast = false;

    public GameObject tooFastUi;

    public Image sliderBackground;

    public AudioClip successSound;
    public AudioClip stretchSound;
    public AudioClip stretchReleaseSound;
    public AudioClip[] failedSounds;
    public bool notAlreadyFailed = true;
    public bool isStretching = false;
    public AudioSource stretchAudioSource;

    public SpriteRenderer middleFingerSpriteRenderer;

    public enum StretchingState
    {
        NotStretching,
        StartStretching,
        Stretching,
        Releasing
    }

    public StretchingState state = StretchingState.NotStretching;

    Vector3 startScale;

    Stack<GameObject> fingerBody = new Stack<GameObject>();

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
        //Vector3 targetVelocity = new Vector2(releaseFingerSpeed * -1, rb.velocity.y);
        /*
        if (isSuccess())
        {
            Success.instance.successMessage();
            targetVelocity = new Vector2(0, rb.velocity.y);
            canBeDragged = false;
            GameManager.instance.Success(1);
            GameManager.instance.isSuccess = true;
            AudioManager.instance.PlayClipAt(successSound, transform.position);
        }
        */
        if (rb.transform.position.x < rangeMin) rb.velocity = new Vector2(0, 0);
        /*
        else if (isDragged == false && !isSuccess())
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f); //Ramène le doigt au début
        }
        */
        //updateIsTooFast();
        tooFastUi.SetActive(isTooFast);
        sliderBackground.color = isTooFast ? Color.red : Color.green;
        /*if (isTooFast && notAlreadyFailed)
        {
            AudioManager.instance.PlayClipAt(failedSounds[Random.Range(1, 8)], transform.position);
            notAlreadyFailed = false;
        }
        */
        //if (isTooFast == false) notAlreadyFailed = true;
        isDragged = false;
        MovePlayer(0,0); //Lance notre fonction Moveplayer en envoyant notre mouvement calculer au dessus
        currentMoveSpeed = GetCurrentMoveSpeed();

        if (state == StretchingState.Stretching && !stretchAudioSource.isPlaying)
        {
            stretchAudioSource = AudioManager.instance.PlayClipAt(stretchSound, transform.position);
        }
        if (state == StretchingState.Releasing)
        {
            state = StretchingState.NotStretching;
        }
        if (state == StretchingState.NotStretching && fingerBody.Count > 3)
        {
            GameObject temp = fingerBody.Pop();
            GameObject temp2 = fingerBody.Pop();
            GameObject temp3 = fingerBody.Pop();
            GameObject temp4 = fingerBody.Pop();
            rb.transform.position = temp4.transform.position;
            Destroy(temp);
            Destroy(temp2);
            Destroy(temp3);
            Destroy(temp4);
        }
        else if (state == StretchingState.NotStretching && fingerBody.Count <= 3)
        {
            for(int i = 0; i < fingerBody.Count; i++)
            {
                Destroy(fingerBody.Pop());
            }
        }
    }

    public void MovePlayer(float _horizontalMovement, float _verticalMovement) //Fonction qui fait bouger notre personnage
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, _verticalMovement); //Récupère le mouvement en 2D du joueur avec un Vector2
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f); //Donne le mouvement au joueur
    }

    void OnMouseDrag()
    {
        isDragged = true;
        if (canBeDragged) {
            MovePlayer((GetMousePos().x - rb.transform.position.x) * moveSpeed, (GetMousePos().y - rb.transform.position.y) * moveSpeed);
            GameObject middleFinder = new GameObject();
            middleFinder.transform.position = rb.transform.position;
            var renderer = middleFinder.AddComponent<SpriteRenderer>();
            renderer.sprite = middleFingerSpriteRenderer.sprite;
            renderer.color = middleFingerSpriteRenderer.color;
            middleFinder.transform.localScale = rb.transform.localScale;
            fingerBody.Push(middleFinder);
        } 
        if (state == StretchingState.NotStretching)
        {
            state = StretchingState.Stretching;
            stretchAudioSource = AudioManager.instance.PlayClipAt(stretchSound, transform.position);
        }
    }

    void OnMouseUp()
    {
        if (state == StretchingState.Stretching)
        {
            state = StretchingState.NotStretching;
            stretchAudioSource.Stop();
            AudioManager.instance.PlayClipAt(stretchReleaseSound, transform.position);
        }
    }

    Vector3 GetMousePos()
    {
        var mosePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mosePos.z = 0;
        if (mosePos.x < rangeMin) mosePos.x = rangeMin;
        return mosePos;
    }

    float GetCurrentMoveSpeed()
    {
        return Mathf.Abs(rb.velocity.x);
    }

    bool isSuccess()
    {
        return false;
        //return !isTooFast && (rangeMax - rb.transform.position.x) <= 0.1f;
    }

    void updateIsTooFast()
    {
        if (isDragged) isTooFast = isTooFast || currentMoveSpeed > speedLimit;
        else isTooFast = false;
    }
}
