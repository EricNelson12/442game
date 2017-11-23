using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Threading.Tasks;

public class gameController : MonoBehaviour {

    const int HEALTH_PRICE = 0;
    const int SUPPLY_PRICE = 0;
    const int ENEMY_AMOUNT = 1;
    const int TIME = 30;

    public Text dbg, dbg2, inst,moneytxt,healthtxt,suptext;
    Button drop;
    Canvas go,scoreCanvas;
    
    public List<GameObject> countryList, countriesFound;
    public List<int> timeBonus;
    public int supplyAmount;
    Scene scene;

    public GameObject enemyplane,crate,heart, timeupSound;
    public int money;
    public int health;
    bool GO = true;

    System.Random RAND;

    public int CurrentCountry;
    public float timeLeft = TIME;

    public PlayerData PLAYER_DATA;
    //DatabaseReference reference;

    // Use this for initialization
    void Start () {

        dbg = GameObject.Find("debugtextL").GetComponent<Text>();
        dbg2 = GameObject.Find("debugtextR").GetComponent<Text>();

        //Database stuff
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://foreign-aid-fighter.firebaseio.com/");
        //reference= FirebaseDatabase.DefaultInstance.RootReference;
        PLAYER_DATA = new PlayerData();

        RAND = new System.Random();
        scene = SceneManager.GetActiveScene();
        money = 200;
        health = 100;
        supplyAmount = 2;
        go = GameObject.Find("goCanvas").GetComponent<Canvas>();
        scoreCanvas = GameObject.Find("scoreCanvas").GetComponent<Canvas>();
        go.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(false);

        countryList =  new List<GameObject>();
        
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("countries"))
        {
            countryList.Add(g);
        }

        CurrentCountry = RAND.Next(countryList.Count);

        //incrementCountriesFound(countryList[CurrentCountry].name);

        
        
        inst.text = "Drop Supplies on: " +countryList[CurrentCountry].name + "start";
        updateText();
        Debug.Log("Staring GC");

        if (scene.name.Equals("tutorial"))
        {
            supplyAmount = 9999;
        }
    }

    public void setrandCountry()
    {
        //remove current from list
        countryList.RemoveAt(CurrentCountry);

        if (countryList.Count > 0)
        {
            

            //Pick random country        
            CurrentCountry = RAND.Next(countryList.Count);

            //reset timer
            timeLeft = TIME;
        }
    }

    public void buySupplies()
    {
        PLAYER_DATA.triedToBuySupplies++;
        if (money > SUPPLY_PRICE)
        {
            PLAYER_DATA.boughtSupplies++;
            supplyAmount++;
            money -= SUPPLY_PRICE;
        }
        
    }

    public void buyHealth()
    {
        PLAYER_DATA.triedToBuyHealth++;
        if(money > HEALTH_PRICE)
        {
            if (health < 100)
            {
                money -= HEALTH_PRICE;

                PLAYER_DATA.boughtHealth++;
                if (health > 75)
                {
                    health = 100;
                }
                else
                {                    
                    health += 25;
                }
            }
            
        }
    }

    void updateText()
    {
        healthtxt.text = health + "%";
        //moneytxt.text = "$" + money;
        suptext.text = "x" + supplyAmount.ToString();
        if (countryList.Count > 0)
        {
            inst.text = "Drop Supplies on: " + countryList[CurrentCountry].name + "( "+Mathf.Round(timeLeft)+" )";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (GO)
        {
            //Count down
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                GameObject s = Instantiate(timeupSound);                
                Destroy(s, 3f);
                PLAYER_DATA.timeRanOut++;
                setrandCountry();
            }

            updateText();
            if (!(GameObject.FindGameObjectsWithTag("enemyplane").Length > ENEMY_AMOUNT))
            {
                Instantiate(enemyplane);
            }

            if (!(GameObject.FindGameObjectsWithTag("sup").Length > 10))
            {
                Instantiate(crate);
            }

            if (!(GameObject.FindGameObjectsWithTag("heart").Length > 10))
            {
                Instantiate(heart);
            }

            if (countryList.Count <= 0 || health <= 0)
            {
                Debug.Log("Game Over");
                playerNameInput();
            }

        }
       
        
    }

    public void gameover(Text name)
        
    {
        scoreCanvas.gameObject.SetActive(false);        
        go.gameObject.SetActive(true);        
        
        Text gotext = GameObject.Find("goText").GetComponent<Text>();        
        gotext.gameObject.SetActive(true); 

        string found = "";
        int score = 0;

        for (int i =0;i<countriesFound.Count;i++)
        {
            score += (timeBonus[i] * 100);
            Debug.Log("For Loop" + countriesFound[i].name);
            found = found + "\n" + countriesFound[i].name+" (100 x "+(timeBonus[i])+"s Left)";
        }

        PLAYER_DATA.name = name.text;
        saveScore(name.text, score);
        

        gotext.text = "Game Over\nCountries Found\n" + found + "\nTotal Score: " + score;

        int count = 0;
        string highscores = "Scores:";
        //count up to find an empty key 
        while (PlayerPrefs.HasKey(count.ToString()))
        {
            count++;
            highscores = highscores +"\n"+ PlayerPrefs.GetString(count.ToString());
        }


        gotext.text = gotext.text + "\n\n\n" + highscores;



    }

    private void saveScore(String n, int s)
    {
        int count = 0;

        //count up to find an empty key 
        while (PlayerPrefs.HasKey(count.ToString()))
        {
            count++;
        }


        if (n.Equals(""))
        {
            return;
        }
        PlayerPrefs.SetString(count.ToString(), (n + " " + s));
        PlayerPrefs.Save();
        //writePlayerData();
        
    
    }



    private void playerNameInput()
    {
        PLAYER_DATA.Finalcash = money;
        PLAYER_DATA.Finalhealth = health;
        PLAYER_DATA.totalTime = Time.frameCount;
        PLAYER_DATA.countriesLeft = countryList.Count;


        GO = false;
        //GameObject.Find("buyHealth").SetActive(false);
        //GameObject.Find("buy").SetActive(false);
        GameObject.Find("drop").SetActive(false);
        inst.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(true);
    }

    private void writePlayerData()
    {     
        //string json = JsonUtility.ToJson(PLAYER_DATA);
        //reference.Child("games").Child(PLAYER_DATA.name+PLAYER_DATA.totalTime.ToString()).SetRawJsonValueAsync(json); 
    }

    //public void incrementCountriesFound(String country)
    //{
    //    dbg.text = (country);

    //    reference.Child("countries").RunTransaction(countryData =>
    //    {
    //        Dictionary<string, object> countries = countryData.Value as Dictionary<string, object>;

    //        dbg2.text = countries.ToString();
    //        if (!countries.ContainsKey(country)){ 
                
    //            countries.Add(country, 1);               
    //        }
    //        else
    //        {               
    //            // INCREMENT COUNT
    //            countries[country] = int.Parse(countries[country].ToString()) + 1;                
    //        }
    //        // END TRANSACTION
            
    //        countryData.Value = countries;
    //        return TransactionResult.Success(countryData);
    //    });
    //}

}


public class PlayerData
{
    public String name = "Default";
    
    public double enemiesDestroyed = 0;
    public double totalTime = 0;
    public double bulletsFired = 0;

    public double timeRanOut = 0;

    public double suppliesDropped = 0;

    public double Finalhealth = 0;
    public double Finalcash = 0;
   
    public double countriesLeft = 0;

    public double boughtHealth = 0;
    public double triedToBuyHealth = 0;

    public double boughtSupplies = 0;
    public double triedToBuySupplies = 0;    
}