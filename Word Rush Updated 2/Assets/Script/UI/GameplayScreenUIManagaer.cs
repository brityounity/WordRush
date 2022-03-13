using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
//Gameplay screen UI controller
//</summary>
public class GameplayScreenUIManagaer : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    public void LoadLevelsScreen()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Levels");//load Level screen 
    }

    


}
