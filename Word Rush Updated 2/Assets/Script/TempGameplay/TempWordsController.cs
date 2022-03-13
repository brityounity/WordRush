using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//<summary>
//Spawn word controller instantiate on gameplay word panel
//</summary>
public class TempWordsController : MonoBehaviour
{
    public int wordArrayIndex;//word object spawn index
    public int wordCorrectIndex;//word object correct index according sentence split
    public Vector2 wordInitialPosition;//word spawn position on gameplay screen
    public string wordText;//text of word
    public float wordWidth; //word width -size
    // public WordProperties wordProperties;
    GameObject tempGameplayManager; //get gameplay manager
    GameObject sentenceHolder;//get sentence holder. parent object of instantiant words
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        tempGameplayManager = GameObject.FindGameObjectWithTag("temp gameplay manager");
        audioSource = this.GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();//remove previous button listeners
        gameObject.GetComponent<Button>().onClick.AddListener((() => { WordOnClick(); })); //active listerns only when word object button clicked
        gameObject.AddComponent<WordTransform>(); //add WordTransform script
        gameObject.GetComponent<WordTransform>().enabled = false;//disbale WordTransform script just after it added

        string correctSentence = PlayerPrefs.GetString("tempCurrentSentence");//get correct sentence 
        string[] correctSentenceWords = correctSentence.Split(' '); // split corrent sentence into an array
        for (int i = 0; i < correctSentenceWords.Length; i++)
        {
            if (correctSentenceWords[i] == wordText) //checking which sentence part matched with 
            {
                wordCorrectIndex = i; //setting word correct index
            }
        }
        sentenceHolder = GameObject.FindGameObjectWithTag("sentence holder");
    }


    //If word click 
    public void WordOnClick()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        gameObject.GetComponent<WordTransform>().enabled = true;//enable WordTransform script
        sentenceHolder.GetComponent<Text>().text += wordText + " "; //add word text with player answer text object
        tempGameplayManager.GetComponent<TempGameplayManager>().wordsOnSentenceHolder.Add(wordText);//add word text value on gameplay manager list
        tempGameplayManager.GetComponent<TempGameplayManager>().indexOnSentenceHolder.Add(wordCorrectIndex);// add word actual index on gameplay manager list
    }
}
