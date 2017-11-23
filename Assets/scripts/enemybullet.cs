using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemybullet : MonoBehaviour
{

    GameObject plane;
    
    
    Rigidbody2D rb;
    Vector3 offset = new Vector3(0, 5, 0);

    // Use this for initialization
    void Start()
    {
        transform.position += transform.up * 1.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 2f, ForceMode2D.Impulse);
        //Debug.Log(transform.forward);
        Destroy(this.gameObject, 3f);

    }

    // Update is called once per frame
    void Update()
    {

    }

   
}
