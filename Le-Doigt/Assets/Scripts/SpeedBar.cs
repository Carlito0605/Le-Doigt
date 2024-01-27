using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{


    public static SpeedBar instance;

    public Slider slider; //On récupère le slider

    public Gradient gradient; //Pour le changement de couleur de la barre d'HP

    public Image fill;

    public float currentMoveSpeed;

    public float smoothSpeed = 5f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de SpeedBar dans la scène");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSpeed(0);
    }

    // Update is called once per frame
    void Update()
    {
        currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, FingerMovement.instance.currentMoveSpeed, smoothSpeed * Time.deltaTime);
        if (FingerMovement.instance.isDragged) SetSpeed((int)(currentMoveSpeed * 10));
        else SetSpeed(0);
    }

    public void SetSpeed(int speed) //Fonction update la vitesse
    {
        slider.value = speed;

        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
