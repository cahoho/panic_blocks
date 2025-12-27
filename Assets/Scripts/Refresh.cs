using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refresh : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.Instance.RefreshAudioManager();
    }

}
