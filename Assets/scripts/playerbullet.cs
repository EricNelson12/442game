using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbullet : MonoBehaviour {

    GameObject plane;    
    
    Rigidbody2D rb;
    Vector3 offset = new Vector3(0,5,0);
    gameController planeGC;

	// Use this for initialization
	void Start () {
        planeGC = GameObject.Find("plane").GetComponent<gameController>();
        plane = GameObject.Find("plane");
        transform.position = plane.transform.position;
        transform.rotation = plane.transform.rotation;        
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 5f,ForceMode2D.Impulse);
        //Debug.Log(transform.forward);
        Destroy(this.gameObject, .5f);
        planeGC.PLAYER_DATA.bulletsFired++;
        
        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    
}
