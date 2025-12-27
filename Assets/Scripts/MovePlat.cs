using System;
using UnityEngine;
/// <summary>
/// 2D可移动的平台
/// </summary>
public class MovePlat : MonoBehaviour
{
    public enum PlayMode// 两种移动模式：
    {
        Restart, // 回到起点
        BbOR    // 来回往返
    }

    [Header("移动模式")]
    public PlayMode playMode = PlayMode.Restart;

    [Header("路径点（顺序）")]
    public Transform[] points; // 必须至少有 2 个点

    [Header("需要移动的物体")]
    public Transform Obj; // 常见是平台的父对象或子对象

    [Header("移动速度")]
    public float moveSpeed = 2f;

    private int current;        // 当前目标点索引
    private int currentMax;     // points.Length - 1
    private bool isPlus = true; // true：向 forward 移动，false：向 backward 移动

    private void Start()
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogError("[MovePlat] points 数组为空，请在 Inspector 中配置！");
            enabled = false;
            return;
        }
        Obj.position = new Vector3(points[0].position.x, points[0].position.y, points[0].position.z);
        currentMax = points.Length - 1;
        current = 0;
        isPlus = true;
    }

    private void Update()
    {
        if (current == currentMax) isPlus = false;

        Vector3 dir = (points[current].position - Obj.position).normalized;

        if (Vector3.Distance(Obj.position, points[current].position) > 0.25f)//防止卡住
        {
            Obj.position += dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (isPlus)
            {
                current++;
            }
            else
            {
                if (playMode == PlayMode.BbOR)
                {
                    current--;
                    if (current <= 0)
                    {
                        current = 0;
                        isPlus = true;
                    }
                }
                else if (playMode == PlayMode.Restart)
                {
                    current = 0;
                    isPlus = true;
                }
            }
        }
    }
}
