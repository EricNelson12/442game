using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour {

    
    public int diff;


    void Awake()
    {
        
       if(GameObject.FindGameObjectsWithTag("diff").Length > 1)
        {
            Destroy(GameObject.FindGameObjectsWithTag("diff")[0]);
        }
        
        DontDestroyOnLoad(this.gameObject);

    }
    

// Update is called once per frame
void Update () {
		
	}

public  void setHard() { diff = 2; }
public  void setEasy() { diff = 0; }
public  void setMed() { diff = 1; }
}
