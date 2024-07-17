using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    //슬라이더 등록될 변수
    public Slider sound_slider;

    //음소거 토글
    public Toggle mute_toggle;

    //이전 사운드값 저장변수
    private float prevSoundValue;

    //음소거 이벤트 
    public void OnMuteClick()
    {
        //Toggle이 체크되었을때
        if (mute_toggle.isOn)
        {
            //음소거 되기전 값 저장
            prevSoundValue = sound_slider.value;
            sound_slider.value = 0;
        }
        else  //Toggle 체크 해제되었을때
        {
            //음소거 취소시 이전에 저장된 값으로 변경
            if (prevSoundValue != 0)
            {
                sound_slider.value = prevSoundValue;
            }
        }
    }

    //음량 확인 후 활성화 이벤트
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
