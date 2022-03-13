using System;
using UnityEngine;
using UnityEngine.UI;


public class WordsController : MonoBehaviour
{
    public int wordArrayIndex;
    public int wordCorrectIndex;
    public Vector2 wordInitialPosition;
    public string wordText;
    public float wordWidth;
   // public WordProperties wordProperties;
    GameObject gameplayManager;
    GameObject sentenceHolder;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GameObject.FindGameObjectWithTag("gameplay manager");
        audioSource = this.GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners(); ;
        gameObject.GetComponent<Button>().onClick.AddListener((() => { WordOnClick(); }));
        gameObject.AddComponent<WordTransform>();
        gameObject.GetComponent<WordTransform>().enabled = false;

        string correctSentence = PlayerPrefs.GetString("currentSentence");
        string[] correctSentenceWords = correctSentence.Split(' ');
        for(int i = 0; i < correctSentenceWords.Length; i++)
        {
            if (correctSentenceWords[i] == wordText)
            {
                wordCorrectIndex = i;
            }
        }
        sentenceHolder = GameObject.FindGameObjectWithTag("sentence holder");
       
    }

    public void WordOnClick()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        gameObject.GetComponent<WordTransform>().enabled = true;
        sentenceHolder.GetComponent<Text>().text += wordText + " ";
        gameplayManager.GetComponent<GameplayManager>().wordsOnSentenceHolder.Add(wordText.ToString());
        gameplayManager.GetComponent<GameplayManager>().indexOnSentenceHolder.Add(wordCorrectIndex);
    }

}
