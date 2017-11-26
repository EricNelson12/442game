using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyplane : MonoBehaviour
{


    //Public Variables
    private GameObject plane;
    public GameObject enemybullet, playerBullet, explosionAnimatin,heart;
    
    public AudioClip explosiddon;
    gameController planeGC;

    AI ai;
    System.Random RAND;

    //Set initial Speed to .5    
    private float MIN_SPEED = .05f;
    private float MAX_SPEED = .6f;

    //For changing color
    Renderer rend;

    //each pilot likes to bank a certain way
    bool toggleBank;
    int randomShootDelay;

    //Left and Right speeds (or throttles)
    private Vector2 THROTTLES;

    // Use this for initialization
    void Start()
    {

        //initialization
        THROTTLES = new Vector2(0.1f, 0.1f);
        ai = new AI();
        RAND = new System.Random();
        rend = GetComponent<Renderer>();
        plane = GameObject.Find("plane");

        randomShootDelay = (int)Math.Round(RAND.NextDouble() * 30f);
        planeGC = GameObject.Find("plane").GetComponent<gameController>();

        //Start in random spot
        moveToRandom();


    }

    private void moveToRandom()
    {
        Vector3 center = new Vector3(-230, 101, 0);
        float xRand = -230f + ((float)RAND.NextDouble() * 193f) - 96f;
        float yRand = 101f + ((float)RAND.NextDouble() * 110f) - 55f;
        transform.position = new Vector3(xRand, yRand);
    }

    // Update is called once per frame
    void Update()
    {

        float distToPlane = Vector3.Magnitude(transform.position - plane.transform.position);

        if (distToPlane < 17)
        {

            //Debug.Log((Time.frameCount + randomShootDelay) % 10);
            if ((Time.frameCount + randomShootDelay) % 20 == 0)
            {

                float angle = Vector2.Angle(transform.right, plane.transform.position - transform.position);
                //Debug.Log("Angle: " + angle);
                if (angle < 95 && angle > 85)
                {
                    shoot();
                }
            }


            if (distToPlane < 7)
            {
                bankaway();
            }
            else
            {
                chasePlane();
            }
        }
        else
        {
            wander();
        }

        limitThrottle();
        movePlane();
    }

    private void bankaway()
    {
        if ((Time.frameCount + randomShootDelay) % 100 == 0)
        {
            THROTTLES[0] -= .25f;
            THROTTLES[1] += .25f;
        }
        else
        {
            THROTTLES[0] += .25f;
            THROTTLES[1] -= .25f;
        }

    }

    private void wander()
    {
        rend.material.SetColor("_Color", Color.white);
        float scalefactor = .1f;

        THROTTLES += new Vector2(UnityEngine.Random.Range(-.5f,.5f), (UnityEngine.Random.Range(-.5f, .5f)) * scalefactor);

    }

    private void shoot()
    {
       Instantiate(enemybullet, transform.position, transform.rotation);
    }



    private void chasePlane()
    {
        //rend.material.SetColor("_Color", Color.red);
        //angle between plane horizontal (wing span) and line to target (this pos - plane pos)
        //should be 90 to aim at
        float angle = Vector2.Angle(transform.right, plane.transform.position - transform.position);

        if (angle > 90)
        {
            THROTTLES[0] -= .05f;
            THROTTLES[1] += .05f;
        }
        else
        {
            THROTTLES[0] += .05f;
            THROTTLES[1] -= .05f;
        }


    }

    private void limitThrottle()
    {
        if (THROTTLES[0] > MAX_SPEED) { THROTTLES[0] = MAX_SPEED; }
        if (THROTTLES[0] < MIN_SPEED) { THROTTLES[0] = MIN_SPEED; }
        if (THROTTLES[1] > MAX_SPEED) { THROTTLES[1] = MAX_SPEED; }
        if (THROTTLES[1] < MIN_SPEED) { THROTTLES[1] = MIN_SPEED; }
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

    void OnCollisionEnter2D(Collision2D coll)
    {


        if (coll.gameObject.tag.Equals("playerbullet"))
        {
            planeGC.PLAYER_DATA.enemiesDestroyed++;
            planeGC.money += 100;
            planeGC.timeLeft += 2;

            if ((int)UnityEngine.Random.Range(0, 2) == 0)
            {
                Instantiate(heart);
            }
        }
        
        GameObject ex = Instantiate(explosionAnimatin, transform.position - transform.right * .3f, transform.rotation);
        Destroy(this.gameObject, .3f);
        Destroy(ex, .6f);



        //}


    }

    class AI
    {
        public enum Mode { attack, wander }
        public Mode mode;

        public AI()
        {
            mode = Mode.wander;
        }
    }
}
