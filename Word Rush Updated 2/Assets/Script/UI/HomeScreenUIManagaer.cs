using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
//Home or main screen UI controller
//</summary>
public class HomeScreenUIManagaer : MonoBehaviour
{
    AudioSource audioSource;

    public GameObject quitPanel;
    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        quitPanel.SetActive(false); //disable quit panel on game start
    }

    //load Option or setting screen 
    public void LoadOptionScreen()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Options");
        
    }

    //Load level screen
    public void LoadLevelsScreen()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Levels");
       
    }

    //Load Instruction screen
    public void LoadInstructionScreen()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Instruction");
    }

    //Open google playsotre or AppStore app to rate game
    public void LikeUs()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/APP_ID");
#endif
    }

    //on Quit button click
    public void OnQuitButtonClick()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        quitPanel.SetActive(true); //enable quit panel

    }

    //On quit confirmation
    public void QuitGameYes()
    {
        Application.Quit(); //quit game or application 

    }

    //on Quit game declained
    public void QuitGameNo()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        quitPanel.SetActive(false); //disable quit panel

    }

}
