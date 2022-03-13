using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWords : MonoBehaviour
{
    GameplayManager gameplayManager;
    JsonDatabase jsonDatabase;
    List<Sentence> sentences;
    string currentSentenceID;
    string currentSentence;
    string[] currentSentenceWords; //collection of individual words
    string[] currentSentenceRandomWords; //randomizing the words in word list
   
    float wordHeight = 90;
    float wordWidth = 550;

    float wordPositionMaxY = 178f;
    float wordPositionMinX = 160f;

    int currentLevel;
    int maxSentencePerLevel;
    int counter;
    public GameObject wordObject;
    

    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GameObject.FindObjectOfType<GameplayManager>();
        jsonDatabase = JsonDatabase.GetJsonData();
        currentLevel = PlayerPrefs.GetInt("current level");
        currentSentenceID =PlayerPrefs.GetString("currentSentenceID");
        currentLevel = currentLevel == 0 ? 1 : currentLevel;
        currentSentenceID = currentSentenceID == "" ? "1" : currentSentenceID;
        counter = 0;
        CompleteCount();
        RequestSentenceJson();
    }
    void CompleteCount()
    {
        sentences = jsonDatabase.GetSentenceList(currentLevel);
        Sentence[] sentenceArray = sentences.ToArray();
        if (currentLevel >= 1 || currentLevel <= 5)
        {
            maxSentencePerLevel = 10;
        }
        else if (currentLevel >= 6 || currentLevel <= 10)
        {
            maxSentencePerLevel = 20;
        }
        else
        {
            maxSentencePerLevel = 30;
        }
        for (int i = 0; i < maxSentencePerLevel; i++)
        {
            if (sentenceArray[i].complete == "true")
            {
                counter++;
            }
        }
        gameplayManager.sentenceCompleteCounter = counter;
    }
    public void RequestSentenceJson()
    {
        // sentences = ReadJson();
        sentences = jsonDatabase.GetSentenceList(currentLevel);
        Sentence[] sentenceArray = sentences.ToArray();
        string wrongId= PlayerPrefs.GetString("wrong sentence");
        
        if(wrongId == "")
        {       
            for (int i = 0; i < maxSentencePerLevel; i++)
            {
                if (sentenceArray[i].complete == "false")
                {
                    currentSentence = sentenceArray[i].text;
                    currentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.SetString("currentSentence", currentSentence);
                    PlayerPrefs.SetString("currentSentenceID", currentSentenceID);
                    if (currentSentence != "")
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < maxSentencePerLevel; i++)
            {
                if (sentenceArray[i].id == wrongId.ToString())
                {
                    currentSentence = sentenceArray[i].text;
                    currentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.DeleteKey("wrong sentence");
                    PlayerPrefs.SetString("currentSentence", currentSentence);
                    PlayerPrefs.SetString("currentSentenceID", currentSentenceID);
                    if (currentSentence != "")
                        break;
                }
            }
        }
       

        WordSpawn();
        
    }
   
    public void RefreshWords()
    {
        //play button clip sound effect
        gameplayManager.GetComponent<AudioSource>().PlayOneShot(gameplayManager.GetComponent<AudioSource>().clip);
        
        DestroyWords();
        currentLevel = PlayerPrefs.GetInt("current level");

        GameObject.FindGameObjectWithTag("gameplay manager").GetComponent<GameplayManager>().wordsOnSentenceHolder.Clear();
        GameObject.FindGameObjectWithTag("gameplay manager").GetComponent<GameplayManager>().gameOver = false;
        GameObject.FindGameObjectWithTag("gameplay manager").GetComponent<GameplayManager>().gameTimer = 0f;
       
        //This returns all the values as Object
        sentences = jsonDatabase.GetSentenceList(currentLevel);
        Sentence[] sentenceArray = sentences.ToArray();
        int tempSentenceID = Int32.Parse(currentSentenceID);

        //Debug.Log(sentenceArray[maxSentencePerLevel - 1].id);
        

        if (tempSentenceID.ToString() == sentenceArray[maxSentencePerLevel-1].id)
        {
            
            for (int i = tempSentenceID; i < maxSentencePerLevel; i++)
            {
                
                if (sentenceArray[i].complete == "false")
                {
                    currentSentence = sentenceArray[i].text;
                    currentSentenceID = sentenceArray[i].id;
                    PlayerPrefs.SetString("currentSentence", currentSentence);
                    PlayerPrefs.SetString("currentSentenceID", currentSentenceID);
                    
                    if (currentSentence != "")
                        break;
                }
            }
        }
        else
        {
            
            if (tempSentenceID > maxSentencePerLevel)
            {
                for (int i = tempSentenceID - (maxSentencePerLevel*(currentLevel-1)); i < maxSentencePerLevel; i++)
                {
                    tempSentenceID += 1;
                    if (sentenceArray[i].complete == "false")
                    {
                        currentSentence = sentenceArray[i].text;
                        currentSentenceID = sentenceArray[i].id;
                        PlayerPrefs.SetString("currentSentence", currentSentence);
                        PlayerPrefs.SetString("currentSentenceID", currentSentenceID);
                        
                        break;
                    }
                }
            }
            else
            {
                for (int i = tempSentenceID; i < maxSentencePerLevel; i++)
                {
                    tempSentenceID += 1;
                    if (sentenceArray[i].complete == "false")
                    {
                        currentSentence = sentenceArray[i].text;
                        currentSentenceID = sentenceArray[i].id;
                        PlayerPrefs.SetString("currentSentence", currentSentence);
                        PlayerPrefs.SetString("currentSentenceID", currentSentenceID);
                       
                        break;
                    }
                }
            }
            
        }
        // PlayerPrefs.SetInt("current sentence", Int32.Parse(currentSentenceID));
        gameplayManager.GetLevelDetails();
        WordSpawn();
    }
    
    

    void WordSpawn()
    {
        currentSentenceWords = currentSentence.Split(' ');
        currentSentenceWords = currentSentenceWords.Where(word => !string.IsNullOrEmpty(word)).ToArray();
        currentSentenceRandomWords = currentSentenceWords;
        Randomizer.Randomize(currentSentenceRandomWords);
        GameObject objToSpawn;
        currentSentenceRandomWords = currentSentenceRandomWords.Where(word => !string.IsNullOrEmpty(word)).ToArray();
        float screenX, screenY;
        Vector2 wordPos;
        if (currentSentenceRandomWords.Length < 7)
        {
            for (int i = 0; i < currentSentenceRandomWords.Length; i++)
            {
                objToSpawn = (GameObject)Instantiate(wordObject, transform);

                
                screenY = wordPositionMaxY - (i * wordHeight);
              
                screenX = 0;
                wordPos = new Vector2(screenX, screenY);

                objToSpawn.transform.position = new Vector3(wordPos.x, wordPos.y, 0);
                
                objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(wordWidth,wordHeight);
                objToSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenX, screenY);

                objToSpawn.GetComponentInChildren<Text>().text = currentSentenceRandomWords[i].ToString();
                objToSpawn.AddComponent<WordsController>();
                objToSpawn.GetComponent<WordsController>().wordArrayIndex = i;

                objToSpawn.GetComponent<WordsController>().wordInitialPosition = new Vector2(screenX, screenY);
                objToSpawn.GetComponent<WordsController>().wordText = currentSentenceRandomWords[i].ToString();
                objToSpawn.GetComponent<WordsController>().wordWidth = wordWidth;

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

                objToSpawn.GetComponentInChildren<Text>().text = currentSentenceRandomWords[i].ToString();
                objToSpawn.AddComponent<WordsController>();
                objToSpawn.GetComponent<WordsController>().wordArrayIndex = i;

                objToSpawn.GetComponent<WordsController>().wordInitialPosition = new Vector2(screenX, screenY);
                objToSpawn.GetComponent<WordsController>().wordText = currentSentenceRandomWords[i].ToString();
                objToSpawn.GetComponent<WordsController>().wordWidth = wordWidth / 2;

            }
            for (int i = 6; i < currentSentenceRandomWords.Length; i++)
            {
                objToSpawn = (GameObject)Instantiate(wordObject, transform);


                screenY = wordPositionMaxY - ((i-6) * wordHeight);
                screenX = wordPositionMinX;
                wordPos = new Vector2(screenX, screenY);

                objToSpawn.transform.position = new Vector3(wordPos.x, wordPos.y, 0);

                objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(wordWidth / 2, wordHeight);
                objToSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenX, screenY);

                objToSpawn.GetComponentInChildren<Text>().text = currentSentenceRandomWords[i].ToString();
                objToSpawn.AddComponent<WordsController>();
                objToSpawn.GetComponent<WordsController>().wordArrayIndex = i;

                objToSpawn.GetComponent<WordsController>().wordInitialPosition = new Vector2(screenX, screenY);
                objToSpawn.GetComponent<WordsController>().wordText = currentSentenceRandomWords[i].ToString();
                objToSpawn.GetComponent<WordsController>().wordWidth = wordWidth / 2;

            }


        }







    }


    public void UndoWord(float width, Vector2 initialPosition, int index, string word)
    {
        GameObject objToSpawn;
        GameObject parentObject = GameObject.FindGameObjectWithTag("word holder");
        GameObject wordObject2 = Resources.Load("UI/wordHolder") as GameObject;
        objToSpawn = (GameObject)Instantiate(wordObject2, parentObject.transform);

        objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(width, wordHeight);
        objToSpawn.GetComponent<RectTransform>().anchoredPosition = initialPosition;
        objToSpawn.GetComponentInChildren<Text>().text = word;
        objToSpawn.AddComponent<WordsController>();
        objToSpawn.GetComponent<WordsController>().wordArrayIndex = index;
        objToSpawn.GetComponent<WordsController>().wordInitialPosition = initialPosition;
        objToSpawn.GetComponent<WordsController>().wordText = word;
        objToSpawn.GetComponent<WordsController>().wordWidth = width;
    }
    void DestroyWords()
    {
        foreach (GameObject word in GameObject.FindGameObjectsWithTag("spawn word"))
        {
            Destroy(word);
        }
        GameObject sentenceHolder = GameObject.FindGameObjectWithTag("sentence holder");
        sentenceHolder.GetComponent<Text>().text = "";

    }
    [Serializable]
    public class Randomizer
    {
        public static void Randomize<T>(T[] items)
        {

            
            
                System.Random rand = new System.Random();
                for (int i = 0; i < items.Length - 1; i++)
                {
                    int j = rand.Next(i, items.Length);
                    if (i == 0 && j == 0)
                    {
                        j = 1;
                    }
                    T temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            
            // For each spot in the array, pick
            // a random item to swap into that spot.
           

        }
    }
 
}
