using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour {

    //Set initial Speed to .5    
    private float MIN_SPEED = .05f;

    //Left and Right speeds (or throttles)
    private Vector2 THROTTLES;

    public Text debugtextL;
    public Text debugtextR;

    //For finding double taps
    float taptime;
    float tapthresh = 1f;
    int TAP_COUNT = 0;

    gameController planeGC;
    private AudioSource audioSource;
    private GameObject plane, boosterLeft, boosterRight;
    public GameObject bullet;
    

    // Use this for initialization
    void Start () {
        //Debug text
        debugtextL = GameObject.Find("debugtextL"). GetComponent<Text>();
        debugtextR = GameObject.Find("debugtextR").GetComponent<Text>();

        //Game objects
        planeGC = GameObject.Find("plane").GetComponent<gameController>();        

        //initialization
        THROTTLES = new Vector2(0.1f, 0.1f);
        
    } 

    // Update is called once per frame
    void Update () {        

        //If there is touch input
        if (Input.touchCount > 1)
        {
            handleTouch();
        }
        else if (Input.anyKey)
        {
            handleKeyboard();
        }

        //Slowly reduce the speed if there is no input.
        //This is to remind them to keep their fingers on.
        else
        {
            THROTTLES[0] -= .01f;
            THROTTLES[1] -= .01f;
        }

        HandleSupplyDropTaps();
        limitThrottle();
        movePlane();       

    }

    private void handleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            THROTTLES[0] += .1f;
        }
        //else if(Input.GetKeyDown(KeyCode.Keypad1))
        //{
        //    THROTTLES[0] -= .1f;
        //}
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            THROTTLES[1] += .1f;
        }
        //else if (Input.GetKeyDown(KeyCode.Keypad3))
        //{
        //    THROTTLES[1] -= .1f;}
        else if (Input.GetKeyDown(KeyCode.Space)){
            //dropSupplies();
            fireBullet();
        }

        
    }

    private void limitThrottle()
    {
        if (THROTTLES[0] > 1) { THROTTLES[0] = 1; }
        if (THROTTLES[0] < 0) { THROTTLES[0] = 0; }
        if (THROTTLES[1] > 1) { THROTTLES[1] = 1; }
        if (THROTTLES[1] < 0) { THROTTLES[1] = 0; }
    }
    private void HandleSupplyDropTaps()
    {
        updateTapCount();
        if (TAP_COUNT >= 2)
        {
            fireBullet();
            TAP_COUNT = 0;
            //dropSupplies();
        }
        
    }

    private void fireBullet()
    {
        Instantiate(bullet);
    }

    private void dropSupplies()
    {
        if(planeGC.supplyAmount > 0)
            {
            Instantiate(GameObject.Find("supplies"));
            planeGC.supplyAmount--;
        }
    }

    private void updateTapCount()
    {
        //For double taps  
        if (Input.touchCount > 0)
        {
            bool began = false;
            bool ended = false;

            //debugtextL.text = taptime.ToString();

            if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
                began = true;
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                began = true;
            else if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Ended)
                ended = true;
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                ended = true;

            if (began)
            {
                //debugtextR.text = "Began";
                if (TAP_COUNT != 1)
                {
                    taptime = Time.time;
                }
            }

            if (ended)
            {
                //debugtextR.text = "Ended";
                if ((Time.time - taptime) < tapthresh)
                {
                    TAP_COUNT++;
                }
                else
                {
                    TAP_COUNT = 0;
                }
            }
            //Touch has just began, start timer 
        }
    }

    void handleTouch()
    {
        // Get movement of the finger since last frame
        Vector2 tl = Input.GetTouch(0).position;
        Vector2 tr = Input.GetTouch(1).position;
        //debugtextL.text = tl.ToString();
        //debugtextR.text = tr.ToString();

        //Left x value is greater, so they need to be switched.
        if (tl[0] > tr[0])
        {
            tl[1] = tl[1] * -1;
            tr[1] = tr[1] * -1;
        }

        //Turn y values into percentage of screen
        tl[1] = tl[1] / Screen.height;
        tr[1] = tr[1] / Screen.height;

        THROTTLES = new Vector2(tl[1], tr[1]);
    }

    public void movePlane()
    {
        //Variables
        float throttleLeft = THROTTLES[0];
        float throttleRight = THROTTLES[1];
        float rot;
        float speed;        

        //Update speed with min speed 
        speed = Mathf.Max(Mathf.Abs((throttleLeft + throttleRight)) / 10, MIN_SPEED);

        //Update rotation
        rot = (throttleRight - throttleLeft) * 2;

        //Speed (will retain last value if there is no change)  
        transform.position += transform.up * speed;
        transform.Rotate(Vector3.forward * rot);
    }
}
