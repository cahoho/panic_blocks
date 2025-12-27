using UnityEngine;
using TMPro; 
/// <summary>
/// 场景左上角的计时器
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))] 
public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float currentTime = 0f;

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        UpdateTimerDisplay();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
