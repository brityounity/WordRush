using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class TempGameplayManager : MonoBehaviour
{
    private JsonDatabase jsonDatabase;

    public bool gameOver;
    public bool answerStatus;
    public int sentenceScore;
    public int totalScore;
    public Text sentenceCompleteText;
    public Text LevelText;
    public Text timerText;
    public Text scoreText;
    public Text totalScoreText;
    public Text totalSentenceCompleteText;
    public GameObject refreshButton;
    public GameObject nextButton;
    public GameObject answerStatusHolder;
    public GameObject rightStatus;
    public GameObject wrongStatus;
    public GameObject winEffect;
    public GameObject congratsPanel;
    public GameObject[] fulfillStar;
    public GameObject[] words;
    public string[] correctWordList;
    public List<string> wordsOnSentenceHolder;
    public List<int> indexOnSentenceHolder;

    protected internal float gameTimer = 0.0f;
    protected internal int sentenceCompleteCounter;
    string playerSentence;
    string tempCorrectSentence;
    bool gameOverCheck;
    bool scoreChecked;
    bool answerChecked;
    bool levelCheked;
    bool answerStatusSoundCheck;
    int tempCurrentLevel;
    int totalSentenceComplete;
    int currentSentence;
    int maxSentenceForLevel;
    int totalTime = 0;
    float waitTime;
    string answerMarking;
    GameObject sentenceHolder;
    AudioSource audioSource;
    TempLoadWords tempLoadWords;
    ScoreCount scoreCount = new ScoreCount();//ScoreCount class instance


    private void Start()
    {
        // jsonDatabase = JsonDatabase.GetJsonData();
        jsonDatabase = GameObject.FindObjectOfType<JsonDatabase>();
        sentenceHolder = GameObject.FindGameObjectWithTag("sentence holder");
        sentenceCompleteCounter = PlayerPrefs.GetInt("temp sentence complete");
        audioSource = this.GetComponent<AudioSource>();
        answerChecked = false;
        // get level details-level number, board number, current senetence number etc. 
        GetLevelDetails();
        //reset variables to initial value
        OnGameStart();
        
        // totalSentenceCompleteText.text = sentenceCompleteCounter + "/30";
        // totalSentenceCompleteText.text = "Done";
    }
    //reset values when game start
    void OnGameStart()
    {
        gameOver = false;
        gameOverCheck = false;
        answerStatus = true;
        levelCheked = false;
        scoreChecked = false;
        answerStatusSoundCheck = false;
        waitTime = 0f;
        answerMarking = "";
        refreshButton.SetActive(true);
        nextButton.SetActive(false);
        answerStatusHolder.SetActive(false);
        rightStatus.SetActive(false);
        wrongStatus.SetActive(false);
        winEffect.SetActive(false);
        congratsPanel.SetActive(false);
        int previousTotalScore = PlayerPrefs.GetInt("total score");
        scoreText.text = previousTotalScore.ToString();

        if (tempCurrentLevel >= 1 || tempCurrentLevel <= 5)
        {
            maxSentenceForLevel = 10;
        }
        else if (tempCurrentLevel >= 6 || tempCurrentLevel <= 10)
        {
            maxSentenceForLevel = 15;
        }
        else
        {
            maxSentenceForLevel = 30;
        }
    }

    //get current level details
    public void GetLevelDetails()
    {
        tempCurrentLevel = PlayerPrefs.GetInt("temp current level");
        
        currentSentence = PlayerPrefs.GetInt("temp current sentence");

        if (tempCurrentLevel == 0)
        {
            //if current level was not set
            tempCurrentLevel = 1;
            PlayerPrefs.SetInt("temp current level", tempCurrentLevel);
        }
        if (currentSentence == 0)
        {

            currentSentence = 1;
            PlayerPrefs.SetInt("temp current sentence", currentSentence);
        }

        LevelText.text = "Level " + tempCurrentLevel.ToString();
        tempCorrectSentence = PlayerPrefs.GetString("tempCurrentSentence");
        correctWordList = tempCorrectSentence.Split(' ');
        correctWordList = correctWordList.Where(word => !string.IsNullOrEmpty(word)).ToArray();

    }

    private void Update()
    {
        words = GameObject.FindGameObjectsWithTag("spawn word");
        string playerAnswer = sentenceHolder.GetComponent<Text>().text;//get player 

        string[] playerAnswerWords = playerAnswer.Split(' ');//split player answer into array
        
        if (gameOver == false)
        {
            //if game not over
            CheckGameOver();
            TimeCounter();
        }
        else if (gameOver == true && gameOverCheck == false)//checking game over or not
        {
          
            string tempPlayerAnswer = sentenceHolder.GetComponent<Text>().text;//get player final answer
            if (gameOverCheck == false)//checking if player answer already checked or not
            {
                if (tempPlayerAnswer != tempCorrectSentence + " ")//if player answer is wrong
                {

                    for (int i = 0; i < wordsOnSentenceHolder.Count; i++)
                    {
                        if (wordsOnSentenceHolder[i] != correctWordList[i])//checking if words are correctly placed
                        {
                            answerMarking += "<color=red>" + playerAnswerWords[i] + "</color> ";//make wrong word red to show user
                            answerStatus = false;//found not matched word. user answer is wrong
                        }
                        else
                        {
                            answerMarking += playerAnswerWords[i] + " ";//correct word
                        }

                        if (i == wordsOnSentenceHolder.Count - 1)
                        {
                            gameOverCheck = true;
                            answerChecked = true;//true to stop repeted same action
                        }
                    }
                }
                else
                {
                    answerMarking = tempCorrectSentence;//set correct answer without edit
                    answerStatus = true;
                    gameOverCheck = true;//true to stop repeted same action
                    answerChecked = true;
                }
            }
            
        }
        if (answerChecked == true)
        {
            sentenceCompleteText.text = answerMarking;//set actual answer sentence to highlited text object

            if (sentenceCompleteText.text.ToString() != null)
            {
                sentenceHolder.GetComponent<Text>().color = Color.clear;//clear main answer to visible update answer text
            }

            GameOver(answerStatus);//call gameover() 
        }

    }
    //Gameplay timer. counting reset for new sentence everytime
    void TimeCounter()
    {
        if (!gameOver)
        {
            gameTimer += Time.deltaTime;
            int seconds = Convert.ToInt32(gameTimer % 60);//convert delta time into seconds
            timerText.text = seconds.ToString();
        }

    }

    //Undo last word from player answer
    public void UndoLastWord()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        if (!gameOver)
        {
            //if game not over
            answerStatus = false;//answer not complete yet
            if (wordsOnSentenceHolder.Count > 0)//count how many word exist in given answer till now
            {
                int wordIndex = indexOnSentenceHolder[indexOnSentenceHolder.Count - 1];//last word index 

                foreach (GameObject o in words)//each words from answer sentence
                {
                    if (o.GetComponent<TempWordsController>().wordCorrectIndex == wordIndex)
                    {

                        Vector2 initialPosition = o.GetComponent<TempWordsController>().wordInitialPosition;
                        float width = o.GetComponent<TempWordsController>().wordWidth;
                        int index = wordIndex;
                        string word = o.GetComponent<TempWordsController>().wordText;//get word text
                        tempLoadWords = new TempLoadWords();//TempLoadWords instance
                        tempLoadWords.UndoWord(width, initialPosition, index, word);//spawn undo word from answer to word panel
                        Destroy(o.gameObject);//remove word from list
                    }
                }
                wordsOnSentenceHolder.RemoveAt(wordsOnSentenceHolder.Count - 1);//remove last word from word list
                indexOnSentenceHolder.RemoveAt(wordsOnSentenceHolder.Count - 1);//remove last word index from index list

                string currentSentence = sentenceHolder.GetComponent<Text>().text.ToString();//get current answer before word remove from actual answer

                string[] currentSentenceWords = currentSentence.Split(' ');//split answer

                sentenceHolder.GetComponent<Text>().text = "";//make answer null for short time
                for (int i = 0; i < currentSentenceWords.Length - 2; i++)
                {
                    //re added words in answer sentence without previous last word
                    sentenceHolder.GetComponent<Text>().text += currentSentenceWords[i].ToString() + " ";
                }

            }
            // words = GameObject.FindGameObjectsWithTag("spawn words");
        }

    }

    // repeatedly call this function to check if game is over or not
    public void CheckGameOver()
    {
        if (words.Length == wordsOnSentenceHolder.Count)//checking how many word added
        {
            //if no word left in word panel. game over = true
            gameOver = true;
        }
        else
        {   
            //if word still left in word panel to be clicked
            gameOver = false;
        }
    }

    //this function will be called after selecting all words from word board
    public void GameOver(bool answerStatus)
    {
        
        //Hide Refresh button
        refreshButton.SetActive(false);
        //active sentence status panel
        answerStatusHolder.SetActive(true);//active answer status after player answer
        // answer status 
        rightStatus.SetActive(answerStatus);//active right sign based on answer
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
            if (levelCheked == false)
            {
                //check level details set or not
                SetLevelDetails(answerStatus);
                levelCheked = true;//stop repeating for this action
            }
            
        }
        else
        {
            PlayerPrefs.SetString("temp wrong sentence", PlayerPrefs.GetString("tempCurrentSentenceID"));
        }
        
        //active next button after updating the data value
        waitTime += Time.deltaTime;

        if (waitTime > .5f)
        {
            nextButton.SetActive(true);
        }

        if (waitTime > 4f)
        {
           //Go to next level after 2 scond pause
            GoNextLevel();
        }

    }
    //set level details after finishing current sentence
    void SetLevelDetails(bool answerStatus)
    {
        if (answerStatus == true)
        {
            currentSentence += 1;//for loading next sentence
            sentenceCompleteCounter += 1;//counting played sentences 
            PlayerPrefs.SetInt("temp sentence complete", sentenceCompleteCounter);
        }

        PlayerPrefs.SetInt("temp current sentence", currentSentence);

    }

    //count score
    void ScoreCount()
    {
        if (scoreChecked == false)
        {
            scoreChecked = true;
            int score = scoreCount.Score(tempCurrentLevel, totalTime);
            score = score <= 0 ? 0 : score;//checking if score is less than 0 
            sentenceScore = score;
            score = score <= 0 ? 0 : score;
            totalScoreText.text = "Sentence Score " + score.ToString();//show level score
        }
    }

    //Reload next gameplay
    public void GoNextLevel()
    {

        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        if (currentSentence > maxSentenceForLevel)//chekcing player played all sentence of he current sentence
        {
            //if all sentences done for current level
            UnityEngine.SceneManagement.SceneManager.LoadScene("Levels");
        } 
        else
        {
            //else reload gameplay again
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay 2");
        }
       
    }
}
