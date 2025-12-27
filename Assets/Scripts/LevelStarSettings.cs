using UnityEngine;
/// <summary>
/// 采用可编程物体，设置每个关卡的通关时间和星设置
/// </summary>

[CreateAssetMenu(fileName = "LevelStarSetting", menuName = "LevelStarSetting", order = 0)]
public class LevelStarSetting : ScriptableObject
{
    public int level;
    public float threeStarTime;
    public float twoStarTime;
    public float oneStarTime;
}
