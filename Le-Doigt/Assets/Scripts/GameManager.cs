using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject titleUI;
    public GameObject camera;

    public bool gameStarted = false;
    public bool isSuccess = false;
    public float successTimer = -1f;

    public static GameManager instance;

    public GameObject firstFinger;
    public GameObject successUI;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scène");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameStarted)
        {
            gameStarted = Input.GetMouseButton(0);
            titleUI.SetActive(((Time.frameCount / 600) % 2) == 0);
        }
        else
        {
            titleUI.SetActive(false);
        }

        if (successTimer > 0 && isSuccess)
        {
            successTimer -= Time.deltaTime;
        }

        if (isSuccess && successTimer <= 0)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 10, camera.transform.position.z);
            isSuccess = false;
        }

        successUI.SetActive(isSuccess);
    }

    public void Success(int levelID)
    {
        firstFinger.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        //firstFinger.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //firstFinger.GetComponent<FingerMovement>().moveSpeed = 0.0f;
        //firstFinger.GetComponent<FingerMovement>().currentMoveSpeed = 0.0f;
        firstFinger.GetComponent<FingerMovement>().enabled = false;
        successTimer = 3;
    }
}
