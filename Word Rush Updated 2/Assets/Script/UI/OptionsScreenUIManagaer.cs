using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
//Option or setting screen UI controller
//</summary>
public class OptionsScreenUIManagaer : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void LoadHomeScreen()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home"); //Load home screen
    }

}
