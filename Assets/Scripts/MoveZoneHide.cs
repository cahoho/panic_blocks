using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AnimationDelay : MonoBehaviour
{
    public AnimationClip appear;
    public GameObject MoveZone;
    void Awake()
    {
        MoveZone = GameObject.FindWithTag("MoveZone");
        MoveZone.SetActive(false);

    }
    void Start()
    {
        
        if (appear != null)
        {
            StartCoroutine(DelayAndExecute(appear.length));
        }
    }
    
    IEnumerator DelayAndExecute(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        MoveZone.SetActive(true);
    }
    
}