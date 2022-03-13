using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//<summary>
//Load levels button in scroll contents
//</summary>
public class LoadLevels : MonoBehaviour
{
    public GameObject completeLevelButton; //for completed level
    public GameObject currentLevelButton;//for current level
    public GameObject lockedLevelButton;//for locked or upcoming level

    int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
       //PlayerPrefs.SetInt("current level", 30);
        
        currentLevel = PlayerPrefs.GetInt("current level"); //get player current level or last incomplete level
        
        //Delete all temp PlayerPref variable
        PlayerPrefs.DeleteKey("temp current sentence");
        PlayerPrefs.DeleteKey("temp current level");
        PlayerPrefs.DeleteKey("temp sentence complete");
        PlayerPrefs.DeleteKey("tempCurrentSentenceID");
        PlayerPrefs.DeleteKey("tempCurrentSentence");
       
        if (currentLevel <= 0 || currentLevel >= 51) //checking irregular or level number exist or not
        {
            currentLevel = 1; //set default level if level number not exist
        }

        LoadLevelsOnStart(); //load levels button
    }

    
    void LoadLevelsOnStart()
    {
        GameObject newLevelButton; //declaring new buton object to spawn
        for (int i = 1; i < 51; i++)
        {
            if (i < currentLevel) //checking completed levels
            {
                newLevelButton = Instantiate(completeLevelButton, this.transform); //instantiate button object 
                newLevelButton.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Text>().text = i.ToString(); //level or button number to recognise in future
                newLevelButton.GetComponentInChildren<LoadTempGameplayScreen>().level = i; //adding script to load completed level gameplay if user want to play again
            }
            else if (i == currentLevel) //checking current level
            {
                newLevelButton = Instantiate(currentLevelButton, this.transform);
                newLevelButton.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Text>().text = i.ToString();
                newLevelButton.GetComponentInChildren<LoadGameplayScreen>().level = i; //LoadGameplayScreen is for current level gameplay

                
            }
            else
            {
                //levels not played yet
                newLevelButton = Instantiate(lockedLevelButton, this.transform);
                newLevelButton.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Text>().text = i.ToString();
               
            }
            newLevelButton.name = i.ToString(); //placing button or level number
            newLevelButton.transform.parent = gameObject.transform; //setting buttons or levels parent object
        }
    }
}
