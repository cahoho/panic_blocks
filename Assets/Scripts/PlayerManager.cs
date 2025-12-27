using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 实现单一操作控制多个玩家的核心系统：PlayerManager.cs
/// 
/// 该代码是所有player的中控室。
/// 通过LP_Controller和JP_Controller控制，调用PlayerManager，再通过PlayerManager将所有的函数分发给所有已经实例化的player
/// 从而实现单一操作控制多个角色的效果
/// 
/// 每一种实现都是对AllPlayer进行的，核心逻辑在于
/// 让所有Player实例化，并注册到改代码的list当中
/// 该代码遍历所有实例化的player
/// 
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
    public List<Player> allPlayers = new List<Player>();

    void Start()
    {

    }


    public void RegisterPlayer(Player player)
    {
        if (!allPlayers.Contains(player))
            allPlayers.Add(player);
    }

    public void UnregisterPlayer(Player player)
    {
        allPlayers.Remove(player);
    }

    // 分别各个操作的内容。foreach里面只包含player的函数，实现全在Player.cs
    public void AllPlayersLeftMove()
    {
        foreach (var player in allPlayers)
        {
            player.LeftMove();
        }
    }

    public void AllPlayersRightMove()
    {
        foreach (var player in allPlayers)
        {
            player.RightMove();
        }
    }

    public void AllPlayersJump()
    {
        foreach (var player in allPlayers)
        {

            player.Jump();
        }
    }
    public void AllPlayersStopMoving()//debug使用
    {
        foreach (var player in allPlayers)
        {
            player.StopMoving();
        }
    }
    public void AllPlayersStatic()
    {
        foreach (var player in allPlayers)
        {
            player.rb.bodyType = RigidbodyType2D.Static;
        }
    }
    
}