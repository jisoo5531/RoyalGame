using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider sound_slider;

    public Toggle mute_toggle;

    private float prevSoundValue;

    public void OnMuteClick()
    {
        if (mute_toggle.isOn)
        {
            prevSoundValue = sound_slider.value;
            sound_slider.value = 0;
        }
        else
        {
            if (prevSoundValue != 0)
            {
                sound_slider.value = prevSoundValue;
            }
        }
    }
    public void OnMuteCheck()
    {
        if(sound_slider.value == 0 && !mute_toggle.isOn)
        {
            mute_toggle.isOn = true;
        }
        else if(sound_slider.value !=0 && mute_toggle.isOn)
        {
            mute_toggle.isOn = false;
        }
    }
}
