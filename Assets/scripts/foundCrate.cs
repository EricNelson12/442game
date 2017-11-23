using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foundCrate : MonoBehaviour
{

    
   

    

    // Use this for initialization
    void Start()
    {       
        
        moveToRandom();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void moveToRandom()
    {
        Vector3 center = new Vector3(-230, 101, 0);
        float xRand = Random.Range(-329.3f, -132.7f);
        float yRand = Random.Range(161f, 42.9f);
        transform.position = new Vector3(xRand, yRand);
    }



  

}
