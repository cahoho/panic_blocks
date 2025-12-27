using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JP_Controller : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// 右侧面板，控制玩家跳跃。
    /// 由于并不仅仅只有一个玩家，所以该代码仅仅起到调用的作用。
    /// 想了解Player代码架构，详细请看PlayerManager.cs
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteJump();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteJump();
    }

    private void ExecuteJump()
    {
        Singleton<PlayerManager>.Instance.AllPlayersJump();
    }
}
