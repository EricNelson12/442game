using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class fortScript : MonoBehaviour {

    int fortHealth = 100;

    //Public Variables
    private GameObject plane;
    public GameObject enemybullet, playerBullet, explosionAnimatin, heart,turretLeft,turretRight,smoke;

    public AudioClip explosiddon;
    gameController planeGC;

    AI ai;
    System.Random RAND;

    //Set initial Speed to .5    
    private float MIN_SPEED = .001f;
    private float MAX_SPEED = .1f;

    //For changing color
    Renderer rend;

    //each pilot likes to bank a certain way
    bool toggleBank;
    int randomShootDelay;

    //Left and Right speeds (or throttles)
    private Vector2 THROTTLES;


    //Animations
    Animator taniml, tanimr;

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

        //Get turret animaton
        taniml = turretLeft.GetComponent<Animator>();
        tanimr = turretRight.GetComponent<Animator>();

        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerBullet.GetComponent<Collider2D>());

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

        if (fortHealth > 50)
        {
            if (distToPlane < 15)
            {
                rotateTurrets();

                tanimr.SetBool("shoot", true);
                taniml.SetBool("shoot", true);
                //Debug.Log((Time.frameCount + randomShootDelay) % 10);
                if ((Time.frameCount + randomShootDelay) % 15 == 0)
                {

                    shoot();

                }


                if (distToPlane < 7)
                {
                    bankaway();
                }
                else
                {
                    MoveTowards(plane.transform.position);
                }
            }
            else
            {
                tanimr.SetBool("shoot", false);
                taniml.SetBool("shoot", false);
                MoveTowards(planeGC.countryList[planeGC.CurrentCountry].transform.position);
            }

            limitThrottle();
            movePlane();
        }
        else
        {
           
            MoveTowards(planeGC.countryList[0].transform.position);
        }
    }

    private void rotateTurrets()
    {
        Vector3 futurePlane = plane.transform.position + plane.transform.up * UnityEngine.Random.Range(2f,10);
        float angle = Vector2.Angle(turretLeft.transform.right, futurePlane - turretLeft.transform.position);
        if (angle < 90)
        {
            turretLeft.transform.Rotate(Vector3.forward * -1);
            turretRight.transform.Rotate(Vector3.forward * -1);
        }
        else
        {
            turretLeft.transform.Rotate(Vector3.forward * 1);
            turretRight.transform.Rotate(Vector3.forward * 1);
        }
        
        
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

    

    private void shoot()
    {
        Instantiate(enemybullet, turretLeft.transform.position, turretLeft.transform.rotation);
        Instantiate(enemybullet, turretRight.transform.position, turretRight.transform.rotation);
    }



    private void MoveTowards(Vector3 dest)
    {
        //rend.material.SetColor("_Color", Color.red);
        //angle between plane horizontal (wing span) and line to target (this pos - plane pos)
        //should be 90 to aim at
        float angle = Vector2.Angle(transform.right, dest - transform.position);

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
            smoke = Instantiate(smoke, coll.gameObject.transform.position, transform.rotation);
         
            smoke.transform.parent = transform;
            smoke.transform.localScale = new Vector3(5, 5, 5);
            
            fortHealth -= 10;
            planeGC.PLAYER_DATA.enemiesDestroyed++;
            
            

            if ((int)UnityEngine.Random.Range(0, 2) == 0)
            {
                Instantiate(heart);
            }
        }

        if(fortHealth < 0 || coll.gameObject.name.Equals("plane"))
        {
            planeGC.money += 500;
            planeGC.timeLeft += 4;
            GameObject ex = Instantiate(explosionAnimatin, transform.position - transform.right * .3f, transform.rotation);
            Destroy(this.gameObject, .3f);
            Destroy(ex, .6f);
        }
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
