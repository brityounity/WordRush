using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    /* This class control the music volume slider*/
    Slider musicSlider;
    private void Start()
    {
        musicSlider = this.GetComponent<Slider>();
       
        //check if music set or not
        //if music set set the valume
        if(PlayerPrefs.GetInt("set music") == 0)
        {
            musicSlider.value = 1;
        }
        else
        {
            musicSlider.value = PlayerPrefs.GetFloat("music volume");
        }
    }

    public void OnSliderValueChange()
    {
        //set music volume on slider value change
        PlayerPrefs.SetInt("set music", 1);
        PlayerPrefs.SetFloat("music volume", musicSlider.value);
       
    }
}
