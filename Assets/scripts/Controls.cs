using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{

    int ENEMY_BULLET_DAMAGE = 5;

    //Set initial Speed to .5    
    private float MIN_SPEED = .05f;

    //Left and Right speeds (or throttles)
    private Vector2 THROTTLES;

    public Text debugtextL;
    public Text debugtextR;

    //For finding double taps
    float taptime;
    float tapthresh = .3f;
    int TAP_COUNT = 0;

    gameController planeGC;
    private AudioSource audioSource;
    private GameObject plane, boosterLeft, boosterRight;
    public GameObject bullet, supplies;


    // Use this for initialization
    void Start()
    {
        //Debug text
        debugtextL = GameObject.Find("debugtextL").GetComponent<Text>();
        debugtextR = GameObject.Find("debugtextR").GetComponent<Text>();

        //Game objects
        planeGC = GameObject.Find("plane").GetComponent<gameController>();

        //Game objects

        //initialization
        THROTTLES = new Vector2(0.1f, 0.1f);

    }

    // Update is called once per frame
    void Update()
    {

        //HandleSupplyDropTaps();
        if (TAP_COUNT < Input.touchCount)
        {
            fireBullet();
        }
        TAP_COUNT = Input.touchCount;

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


        limitThrottle();
        movePlane();

    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag.Equals("enemyplane"))
        {
            planeGC.health -= 20;
        }
        if (coll.gameObject.tag.Equals("fort"))
        {
            planeGC.health -= 60;
        }
        else if (coll.gameObject.tag.Equals("enemybullet"))
        {
            Destroy(coll.gameObject, .1f);
            planeGC.health -= ENEMY_BULLET_DAMAGE;
        }
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
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
        
        if (TAP_COUNT > 0)
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

    public void dropSupplies()
    {
        if (planeGC.supplyAmount > 0)
        {
            Instantiate(supplies, transform.position, transform.rotation);
            planeGC.supplyAmount--;
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
            float temp = tl[1];
            tl[1] = tr[1];
            tr[1] = temp;
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
