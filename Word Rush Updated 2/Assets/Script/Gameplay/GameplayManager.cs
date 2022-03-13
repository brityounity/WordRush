using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameplayManager : MonoBehaviour
{
   
    private JsonDatabase jsonDatabase;
    private AdManager adManager;
    public bool gameOver;
    public bool answerStatus;
    public int sentenceScore;
    public int totalScore;
    public Text sentenceCompleteText;
    public Text LevelText;
    public Text timerText;
    public Text scoreText;
    public Text totalScoreText;
    public Text totalScoreTextOnLevelComplete;
    public Text levelCompleteText;
    public Text totalSentenceCompleteText;
    public GameObject refreshButton;
    public GameObject nextButton;
    public GameObject answerStatusHolder;
    public GameObject rightStatus;
    public GameObject wrongStatus;
    public GameObject winEffect;
    public GameObject congratsPanel;
    public GameObject levelCompletePanel;
    public GameObject starPanel;
    public GameObject[] fulfillStar;
    public GameObject[] words;
    public string[] correctWordList;
    public string[] tempWords;
    public List<string> wordsOnSentenceHolder;
    public List<int> indexOnSentenceHolder;

    protected internal float gameTimer = 0.0f;
    protected internal int sentenceCompleteCounter;
    string playerSentence;
    string correctSentence;
    bool gameOverCheck;
    bool scoreChecked;
    bool levelChecked;
    bool compareTwoArray;
    bool answerStatusSoundCheck;
    bool answerCheck;
    int currentLevel;
    int currentBoard;
    int maxSentenceForLevel;
    int currentSentence;
    int totalTime = 0;
    float waitTime = 0f;
    string answerMarking;
    GameObject sentenceHolder;
    AudioSource audioSource;
    LoadWords loadWords;
    ScoreCount scoreCount = new ScoreCount();
    private void Start()
    {
        // jsonDatabase = JsonDatabase.GetJsonData();
        jsonDatabase = GameObject.FindObjectOfType<JsonDatabase>();
        adManager = GameObject.FindObjectOfType<AdManager>();
        sentenceHolder = GameObject.FindGameObjectWithTag("sentence holder");
        audioSource = this.GetComponent<AudioSource>();

        // get level details-level number, board number, current senetence number etc. 
        GetLevelDetails();
        //reset variables to initial value
        OnGameStart();
       
        
    }
    //reset values when game start
    void OnGameStart()
    {
        gameOver = false;
        gameOverCheck = false;
        answerStatus = true;
        scoreChecked = false;
        levelChecked = false;
        compareTwoArray = true;
        answerCheck = false;
        answerStatusSoundCheck = false;
        answerMarking = "";
        refreshButton.SetActive(true);
        nextButton.SetActive(false);
        answerStatusHolder.SetActive(false);
        rightStatus.SetActive(false);
        wrongStatus.SetActive(false);
        winEffect.SetActive(false);
        congratsPanel.SetActive(false);
        levelCompletePanel.SetActive(false);

        int previousTotalScore = PlayerPrefs.GetInt("total score");
        scoreText.text = previousTotalScore.ToString();
        if(currentLevel >= 1 || currentLevel <= 5)
        {
            maxSentenceForLevel = 10;
        }else if(currentLevel >= 6 || currentLevel <= 10)
        {
            maxSentenceForLevel = 20;
        }
        else
        {
            maxSentenceForLevel = 30;
        }
        totalSentenceCompleteText.text = sentenceCompleteCounter + "/"+maxSentenceForLevel;
    }


    public void GetLevelDetails()
    {
        currentLevel = PlayerPrefs.GetInt("current level");       
        currentSentence = PlayerPrefs.GetInt("current sentence");
       
        if (currentLevel == 0)
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("current level", currentLevel);
        }
        /* (currentSentence == 0)
        {
            currentSentence = 1;
            PlayerPrefs.SetInt("current sentence", currentSentence);
        }*/

        LevelText.text = "Level " + currentLevel.ToString();
        correctSentence = PlayerPrefs.GetString("currentSentence");
        correctWordList = correctSentence.Split(' ');
        correctWordList = correctWordList.Where(word => !string.IsNullOrEmpty(word)).ToArray();
    }

    private void Update()
    {
        words = GameObject.FindGameObjectsWithTag("spawn word");
        correctSentence = PlayerPrefs.GetString("currentSentence");
        string playerAnswer = sentenceHolder.GetComponent<Text>().text;
        
        string[] playerAnswerWords = playerAnswer.Split(' ');
       
        if (gameOver == false)
        {
            CheckGameOver();
            TimeCounter();
        }
        else if(gameOver == true && gameOverCheck == false)
        {
           
            string tempPlayerAnswer = sentenceHolder.GetComponent<Text>().text;
            // string[] tempPlayerAnswerWords = tempPlayerAnswer.Split(' ');
            // tempPlayerAnswerWords = tempPlayerAnswerWords.Where(word => !string.IsNullOrEmpty(word)).ToArray();
            // tempWords = tempPlayerAnswerWords;
            if (gameOverCheck == false)
            {
                if (tempPlayerAnswer != correctSentence + " ")
                {
                    for (int i = 0; i < wordsOnSentenceHolder.Count; i++)
                    {
                        if (wordsOnSentenceHolder[i] != correctWordList[i])
                        {
                            answerMarking += "<color=red>" + playerAnswerWords[i] + "</color> ";
                            answerStatus = false;
                        }
                        else
                        {
                            answerMarking += playerAnswerWords[i] + " ";
                        }


                        if (i == wordsOnSentenceHolder.Count - 1)
                        {
                            gameOverCheck = true;
                            answerCheck = true;
                        }
                    }
                }
                else
                {
                    answerMarking = correctSentence;
                    answerStatus = true;
                    gameOverCheck = true;
                    answerCheck = true;
                }
            }   

        }
        if (answerCheck == true)
        {
            sentenceCompleteText.text = answerMarking;
            if (sentenceCompleteText.text.ToString() != null)
            {
                sentenceHolder.GetComponent<Text>().color = Color.clear;
            }

            GameOver(answerStatus);
        }
        
    }

   
    // start to count time when gameplay start
    void TimeCounter()
    {
        if (!gameOver)
        {
            gameTimer += Time.deltaTime;
            int seconds = Convert.ToInt32(gameTimer % 60);
            totalTime = seconds;
            int minute = Convert.ToInt32(gameTimer % 3600);
            timerText.text = seconds.ToString();
        }

    }


    // Call if undo button pressed. this function will find out the last word user select and withdrow from answer sentence
    public void UndoLastWord()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        if (!gameOver)
        {
            answerStatus = false;
            if (wordsOnSentenceHolder.Count > 0)
            {
                int wordIndex =  indexOnSentenceHolder[indexOnSentenceHolder.Count - 1];
            
               foreach(GameObject o in words)
                {
                    if (o.GetComponent<WordsController>().wordCorrectIndex == wordIndex)
                    {

                        Vector2 initialPosition = o.GetComponent<WordsController>().wordInitialPosition;
                        float width = o.GetComponent<WordsController>().wordWidth;
                        int index = wordIndex;
                        string word = o.GetComponent<WordsController>().wordText;
                        loadWords = new LoadWords();
                        loadWords.UndoWord(width, initialPosition, index, word);
                        Destroy(o.gameObject);
                    }
                }
                wordsOnSentenceHolder.RemoveAt(wordsOnSentenceHolder.Count - 1);
                indexOnSentenceHolder.RemoveAt(indexOnSentenceHolder.Count - 1);
               // index.RemoveAt(wordsOnSentenceHolder.Count - 1);
            
                string currentSentence = sentenceHolder.GetComponent<Text>().text.ToString();
           
                string[] currentSentenceWords = currentSentence.Split(' ');
           
                sentenceHolder.GetComponent<Text>().text = "";
                for (int i = 0; i < currentSentenceWords.Length - 2; i++)
                {
               
                    sentenceHolder.GetComponent<Text>().text += currentSentenceWords[i].ToString() + " ";
                }

            }
       // words = GameObject.FindGameObjectsWithTag("spawn words");
        }

    }

    // repeatedly call this function to check if game is over or not
    public void CheckGameOver()
    {

        
        if (words.Length == wordsOnSentenceHolder.Count)
        {
            gameOver = true;
        }
        else
        {
            gameOver =  false;
        } 
    }

    //this function will be called after selecting all words from word board
    public void GameOver(bool answerStatus)
    {
       
        //Hide Refresh button
        refreshButton.SetActive(false);
        //active sentence status panel
        answerStatusHolder.SetActive(true);
        // answer status 
        rightStatus.SetActive(answerStatus);


        wrongStatus.SetActive(!answerStatus);
        if (answerStatus == false && answerStatusSoundCheck == false)
        {
            Handheld.Vibrate();
            if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
            {
                wrongStatus.GetComponent<AudioSource>().enabled = true;
                wrongStatus.GetComponent<AudioSource>().PlayOneShot(wrongStatus.GetComponent<AudioSource>().clip);
                answerStatusSoundCheck = true;
            }
            else
            {
                wrongStatus.GetComponent<AudioSource>().enabled = false;
            }

            ScoreDecrease();
        }

        if (answerStatus == true && answerStatusSoundCheck == false)
        {
            winEffect.SetActive(answerStatus);
            if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
            {
                winEffect.GetComponent<AudioSource>().enabled = true;
                winEffect.GetComponent<AudioSource>().PlayOneShot(winEffect.GetComponent<AudioSource>().clip);
                answerStatusSoundCheck = true;
            }
            else
            {
                winEffect.GetComponent<AudioSource>().enabled = false;
            }
        }
        
        if (answerStatus == true)
        {
            
            ScoreCount();
            if (levelChecked == false)
            {
                jsonDatabase.UpdateJsonData(currentLevel, answerStatus);
                SetLevelDetails(answerStatus);
                levelChecked = true;
            }

        }
        else
        {
            PlayerPrefs.SetString("wrong sentence", PlayerPrefs.GetString("currentSentenceID"));
        }
        
        

        
        waitTime += Time.deltaTime;
        if (waitTime > 1.3f)
        {
            nextButton.SetActive(true);
        }
       
        if (waitTime > 5f)
        {
            GoNextLevel();
        }

    }
   
    //Count score if game over based on play time. this function is called if player give the correct answer
    void ScoreCount()
    {
        if (scoreChecked == false)
        {
            scoreChecked = true;
            int score = scoreCount.Score(currentLevel, totalTime);
            score = score <= 0 ? 0 : score;
            sentenceScore = score;
            int previousTotalScore = PlayerPrefs.GetInt("total score");
            previousTotalScore += sentenceScore;
            PlayerPrefs.SetInt("total score", previousTotalScore);
            scoreText.text = previousTotalScore.ToString();
            totalScoreText.text = "Total Score " + previousTotalScore.ToString();
            totalScoreTextOnLevelComplete.text = "Total Score " + previousTotalScore.ToString();
        }
    }

    void ScoreDecrease()
    {
        if (scoreChecked == false)
        {
            scoreChecked = true;
            int score = scoreCount.Score(currentLevel, totalTime);
            score = score <= 0 ? 0 : score;
            sentenceScore = score;
            int previousTotalScore = PlayerPrefs.GetInt("total score");
            previousTotalScore -= 50;
            
            PlayerPrefs.SetInt("total score", previousTotalScore);
          //  scoreText.text = previousTotalScore.ToString();
           // totalScoreText.text = "Total Score " + previousTotalScore.ToString();
           // totalScoreTextOnLevelComplete.text = "Total Score " + previousTotalScore.ToString();
        }
    }

    bool CompareAnswer(string[] playerAnswer, string[] correctAnswer)
    {
        if (playerAnswer.Length == correctAnswer.Length)
        {

            // Traverse both array and compare
            //each element
            for (int i = 0; i < correctAnswer.Length; i++)
            {

                // set true if each corresponding
                //elements of arrays are equal
                if (correctAnswer[i] != playerAnswer[i])
                {
                   
                    compareTwoArray = false;
                }
            }
        }
        else
        {
            compareTwoArray = false;
        }
        return compareTwoArray;
    }
    void SetLevelDetails(bool answerStatus)
    {
       
        if (answerStatus == true)
        {
            
            currentSentence += 1;
            totalSentenceCompleteText.text = currentSentence + "/" + maxSentenceForLevel;
            if (currentSentence > maxSentenceForLevel-1 && sentenceCompleteCounter>=maxSentenceForLevel-1)
            {
                Debug.Log(currentSentence);
                levelCompleteText.text = "You completed level " + currentLevel;
                totalScoreText.text = "Total Score " + PlayerPrefs.GetInt("total score");
                currentSentence = 0;
                currentLevel += 1;
               
                if(currentLevel==11 || currentLevel==21 || currentLevel == 31 || currentLevel == 41 || currentLevel == 51)
                {
                    congratsPanel.SetActive(true);
                    int starCount = 0;
                    if (currentLevel == 11)
                        starCount = 1;
                    else if (currentLevel == 21)
                        starCount = 2;
                    else if (currentLevel == 31)
                        starCount = 3;
                    else if (currentLevel == 41)
                        starCount = 4;
                    else if (currentLevel == 51)
                        starCount = 5;

                    for(int i=0;i< starCount; i++)
                    {
                        starPanel.SetActive(true);
                        fulfillStar[i].SetActive(true);
                    }

                }
                else
                {
                    levelCompletePanel.SetActive(true);
                    adManager.gameObject.GetComponent<AdManager>().RequestAndLoadInterstitialAd();
                    starPanel.SetActive(false);
                }

                PlayerPrefs.DeleteKey("data");
            }
        }

        PlayerPrefs.SetInt("current level", currentLevel);
        PlayerPrefs.SetInt("current sentence", currentSentence);
       // Debug.Log(currentSentence);
    }
   
    public void GoNextLevel()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        
        if (currentLevel < 51)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Levels");
        }
       
       
    }
 
}
