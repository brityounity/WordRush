using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            this.GetComponent<Button>().image.sprite = soundOnSprite;
        }
        else
        {
            this.GetComponent<Button>().image.sprite = soundOffSprite;
        }
    }

    public void OnSoundButtonClick()
    {
        //toggle sound on sound button click
        // change sprites according action
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            this.GetComponent<Button>().image.sprite = soundOffSprite;
            PlayerPrefs.SetInt("set sound", 2);
        }
        else
        {
            this.GetComponent<Button>().image.sprite = soundOnSprite;
            PlayerPrefs.SetInt("set sound", 1);
        }
    }
}
