using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mute : MonoBehaviour {

    void start()
    {

    }

    bool isMute;

    public void ToggleMute()
    {
        isMute = !isMute;
        AudioListener.volume = isMute ? 0 : 1;
    }
}
