using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempLoadWords : MonoBehaviour
{
    TempGameplayManager tempGameplayManager;
    JsonDatabase jsonDatabase;
    List<Sentence> sentences;

    int previousSentenceID;
    int tempLastSentence;
    string tempCurrentSentenceID;
    string tempCurrentSentence; //holding current sentence
    string[] tempCurrentSentenceWords; //collection of individual words
    string[] tempCurrentSentenceRandomWords; //randomizing the words in word list

    float wordHeight = 90;//word object fixed height
    float wordWidth = 550;//word default width

    float wordPositionMaxY = 178f;//word maximum Y position
    float wordPositionMinX = 160f;//Word x position initial

    int tempCurrentLevel;//current level 
    int maxSentencePerLevel;//initiate max sentence per level
   
    public GameObject wordObject;//word object prefab


    // Start is called before the first frame update
    void Start()
    {
        tempGameplayManager = GameObject.FindObjectOfType<TempGameplayManager>();//get temp gameplay gamemanager
        jsonDatabase = JsonDatabase.GetJsonData();//Json database instance call
        tempCurrentLevel = PlayerPrefs.GetInt("temp current level");//get request level
        previousSentenceID = PlayerPrefs.GetInt("previous temp sentence");//get sentence index, if exist
        tempCurrentSentenceID = PlayerPrefs.GetString("tempCurrentSentenceID");//get sentence id. if exist
      
        if (tempCurrentLevel >= 1 || tempCurrentLevel <= 5)
        {
            maxSentencePerLevel = 10;
        }
        else if (tempCurrentLevel >= 6 || tempCurrentLevel <= 10)
        {
            maxSentencePerLevel = 20;
        }
        else
        {
            maxSentencePerLevel = 30;
        }
        RequestSentenceJson();//request sentence function
    }

    //Request for new sentence from database
    public void RequestSentenceJson()
    {
        // sentences = ReadJson();
        sentences = jsonDatabase.GetSentenceList(tempCurrentLevel);
        Sentence[] sentenceArray = sentences.ToArray();
        string wrongId = PlayerPrefs.GetString("temp wrong sentence");
        //Debug.Log(tempCurrentSentenceID);
        //Debug.Log(wrongId);
        if (wrongId == "")//checking if previous sentence was wrong or not
        {
            if (tempCurrentSentenceID == "" || tempCurrentSentenceID == null) { 
                for (int i = 0; i < maxSentencePerLevel; i++)
                {

                    tempCurrentSentence = sentenceArray[i].text;
                    tempCurrentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.SetString("tempCurrentSentence", tempCurrentSentence);
                    PlayerPrefs.SetString("tempCurrentSentenceID", tempCurrentSentenceID);
                    PlayerPrefs.DeleteKey("temp wrong sentence");
                    if (tempCurrentSentence != "")
                        break;

                }
            }
            else
            {
                //if previous sentence was not wrong
                previousSentenceID = Int32.Parse(tempCurrentSentenceID);
                for (int i = 0; i < maxSentencePerLevel; i++)
                {
                    if(sentenceArray[i].id == (previousSentenceID + 1).ToString())//chekcing for next sentence 
                    {
                        tempCurrentSentence = sentenceArray[i].text;
                        tempCurrentSentenceID = sentenceArray[i].id;
                        PlayerPrefs.SetString("tempCurrentSentence", tempCurrentSentence);
                        PlayerPrefs.SetString("tempCurrentSentenceID", tempCurrentSentenceID);
                        PlayerPrefs.DeleteKey("temp wrong sentence");
                        if (tempCurrentSentence != "")
                            break;
                    }                   
                }
            }
        }
        else
        {

            for (int i = 0; i < maxSentencePerLevel; i++)
            {
                if (sentenceArray[i].id == wrongId.ToString())
                {
                    //if previous sentence is wrong
                    tempCurrentSentence = sentenceArray[i].text;
                    tempCurrentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.DeleteKey("temp wrong sentence");//delete previous saved wrong id even if existed
                    PlayerPrefs.SetString("tempCurrentSentence", tempCurrentSentence);//saving new current sentence
                    PlayerPrefs.SetString("tempCurrentSentenceID", tempCurrentSentenceID);//saving new curent sentence id
                    if (tempCurrentSentence != "")
                        break;
                }
            }            
  
        }

        WordSpawn();

    }
    //Refresh sentence
    public void RefreshWords()
    {
        //play button clip sound effect
        tempGameplayManager.GetComponent<AudioSource>().PlayOneShot(tempGameplayManager.GetComponent<AudioSource>().clip);

        DestroyWords();//Destroy prevous existing word objects
        tempCurrentLevel = PlayerPrefs.GetInt("temp current level");//get current temporary level

        GameObject.FindGameObjectWithTag("temp gameplay manager").GetComponent<TempGameplayManager>().wordsOnSentenceHolder.Clear();//clear previous list
        GameObject.FindGameObjectWithTag("temp gameplay manager").GetComponent<TempGameplayManager>().gameOver = false;//game not over just chaning context
        GameObject.FindGameObjectWithTag("temp gameplay manager").GetComponent<TempGameplayManager>().gameTimer = 0f;//reset game timer/countdown

        //This returns all the values as Object
        sentences = jsonDatabase.GetSentenceList(tempCurrentLevel);
        Sentence[] sentenceArray = sentences.ToArray();//
        int tempSentenceID = Int32.Parse(tempCurrentSentenceID);//get sentence id

       

        if (tempSentenceID.ToString() == sentenceArray[maxSentencePerLevel-1].id)//checking if current sentence is the last sentence of the current level
        {
           // Start from first if tempSentenceId is TerrainHeightmapSyncControl last sentence of the level
            for (int i = 0; i < maxSentencePerLevel; i++)
            {
                
                    tempCurrentSentence = sentenceArray[i].text;
                    tempCurrentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.SetString("tempCurrentSentence", tempCurrentSentence);
                    PlayerPrefs.SetString("tempCurrentSentenceID", tempCurrentSentenceID);
                    tempLastSentence = tempCurrentLevel * maxSentencePerLevel - 1;
                    PlayerPrefs.SetInt("temp last sentence", tempLastSentence);
                    Debug.Log(tempLastSentence);
                    if (tempCurrentSentence != "")
                            break;
                
            }
        }
        else
        {
            //if tempSentenceID is not last sentence sentence of the level
                for (int i = tempSentenceID - (maxSentencePerLevel * (tempCurrentLevel - 1)); i < maxSentencePerLevel; i++)
                {
                    tempSentenceID += 1;//increasing sentence id 

                    tempCurrentSentence = sentenceArray[i].text;
                    tempCurrentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.SetString("tempCurrentSentence", tempCurrentSentence);
                    PlayerPrefs.SetString("tempCurrentSentenceID", tempCurrentSentenceID);
                    tempLastSentence = tempCurrentLevel * maxSentencePerLevel - 1;
                    PlayerPrefs.SetInt("temp last sentence", tempSentenceID);
                    
                    if (tempCurrentSentence != "")
                        break;
                }
            
            
        }
        tempGameplayManager.GetLevelDetails();
        WordSpawn();
    }


    //Spawn word on word holder panel
    void WordSpawn()
    {
        tempCurrentSentenceWords = tempCurrentSentence.Split(' '); //split sentence into multiple words
        tempCurrentSentenceWords = tempCurrentSentenceWords.Where(word => !string.IsNullOrEmpty(word)).ToArray();//checking if array has null value. remove null value from array
        tempCurrentSentenceRandomWords = tempCurrentSentenceWords;//set correct sentence onto a temp variable
        
            Randomizer.Randomize(tempCurrentSentenceRandomWords);//randomize the temp variable to shuffle
        
        
        tempCurrentSentenceRandomWords = tempCurrentSentenceRandomWords.Where(word => !string.IsNullOrEmpty(word)).ToArray();//checking null value exist
        GameObject objToSpawn;

        float screenX, screenY;
        Vector2 wordPos;
        if (tempCurrentSentenceRandomWords.Length < 7)
        {
            for (int i = 0; i < tempCurrentSentenceRandomWords.Length; i++)
            {
                objToSpawn = (GameObject)Instantiate(wordObject, transform);


                screenY = wordPositionMaxY - (i * wordHeight);
                screenX = 0;
                wordPos = new Vector2(screenX, screenY);

                objToSpawn.transform.position = new Vector3(wordPos.x, wordPos.y, 0);

                objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(wordWidth, wordHeight);
                objToSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenX, screenY);

                objToSpawn.GetComponentInChildren<Text>().text = tempCurrentSentenceRandomWords[i].ToString();
                objToSpawn.AddComponent<TempWordsController>();
                objToSpawn.GetComponent<TempWordsController>().wordArrayIndex = i;
                objToSpawn.GetComponent<TempWordsController>().wordCorrectIndex = i;

                objToSpawn.GetComponent<TempWordsController>().wordInitialPosition = new Vector2(screenX, screenY);
                objToSpawn.GetComponent<TempWordsController>().wordText = tempCurrentSentenceRandomWords[i].ToString();
                objToSpawn.GetComponent<TempWordsController>().wordWidth = wordWidth;

            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                objToSpawn = (GameObject)Instantiate(wordObject, transform);


                screenY = wordPositionMaxY - i * wordHeight;

                screenX = -wordPositionMinX;
                wordPos = new Vector2(screenX, screenY);

                objToSpawn.transform.position = new Vector3(wordPos.x, wordPos.y, 0);

                objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(wordWidth / 2, wordHeight);
                objToSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenX, screenY);

                objToSpawn.GetComponentInChildren<Text>().text = tempCurrentSentenceRandomWords[i].ToString();
                objToSpawn.AddComponent<TempWordsController>();
                objToSpawn.GetComponent<TempWordsController>().wordArrayIndex = i;

                objToSpawn.GetComponent<TempWordsController>().wordInitialPosition = new Vector2(screenX, screenY);
                objToSpawn.GetComponent<TempWordsController>().wordText = tempCurrentSentenceRandomWords[i].ToString();
                objToSpawn.GetComponent<TempWordsController>().wordWidth = wordWidth / 2;

            }
            for (int i = 6; i < tempCurrentSentenceRandomWords.Length; i++)
            {
                objToSpawn = (GameObject)Instantiate(wordObject, transform);


                screenY = wordPositionMaxY - ((i - 6) * wordHeight);
                screenX = wordPositionMinX;
                wordPos = new Vector2(screenX, screenY);

                objToSpawn.transform.position = new Vector3(wordPos.x, wordPos.y, 0);

                objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(wordWidth / 2, wordHeight);
                objToSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenX, screenY);

                objToSpawn.GetComponentInChildren<Text>().text = tempCurrentSentenceRandomWords[i].ToString();
                objToSpawn.AddComponent<TempWordsController>();
                objToSpawn.GetComponent<TempWordsController>().wordArrayIndex = i;

                objToSpawn.GetComponent<TempWordsController>().wordInitialPosition = new Vector2(screenX, screenY);
                objToSpawn.GetComponent<TempWordsController>().wordText = tempCurrentSentenceRandomWords[i].ToString();
                objToSpawn.GetComponent<TempWordsController>().wordWidth = wordWidth / 2;

            }


        }

    }

    //If undo button pressed
    public void UndoWord(float width, Vector2 initialPosition, int index, string word)
    {
        GameObject objToSpawn;
        GameObject parentObject = GameObject.FindGameObjectWithTag("word holder");
        GameObject wordObject2 = Resources.Load("UI/wordHolder") as GameObject;//load word prefab as gameobject 
        objToSpawn = (GameObject)Instantiate(wordObject2, parentObject.transform);//Instantiante word object as new object

        objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(width, wordHeight);//set word object size-width, height
        objToSpawn.GetComponent<RectTransform>().anchoredPosition = initialPosition;//set UI position
        objToSpawn.GetComponentInChildren<Text>().text = word;//set word text on object text component
        objToSpawn.AddComponent<TempWordsController>();//Add word controller script on word new instance(after undo0
        objToSpawn.GetComponent<TempWordsController>().wordArrayIndex = index;//set word array index
        objToSpawn.GetComponent<TempWordsController>().wordInitialPosition = initialPosition;//set word initial position
        objToSpawn.GetComponent<TempWordsController>().wordText = word;//set word text
        objToSpawn.GetComponent<TempWordsController>().wordWidth = width;//set word width from stored data
    }

    //Destroy all words exist in word holder panel
    void DestroyWords()
    {
        foreach (GameObject word in GameObject.FindGameObjectsWithTag("spawn word"))
        {
            Destroy(word);
        }
        GameObject sentenceHolder = GameObject.FindGameObjectWithTag("sentence holder");
        sentenceHolder.GetComponent<Text>().text = "";//make sentence holder text as null
    }
    [Serializable]
    public class Randomizer
    {
        public static void Randomize<T>(T[] items)
        {

            
                System.Random rand = new System.Random();


                // For each spot in the array, pick
                // a random item to swap into that spot.
                for (int i = 0; i < items.Length - 1; i++)
                {
                    int j = rand.Next(i, items.Length);
                
                    if (i==0 && j == 0)
                    {
                        j = 1;
                    }
                    T temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            


        }
    }
}
