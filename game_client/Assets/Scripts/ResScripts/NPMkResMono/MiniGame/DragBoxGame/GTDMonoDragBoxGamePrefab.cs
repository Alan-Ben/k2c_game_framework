using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE.MiniGame
{
    public class GTDMonoDragBoxGamePrefab : MonoBehaviour
    {
        [ALHeader("拖拽物体列表")]
        public List<GTDMonoDragBoxGameBox> monoList;

        [ALHeader("玩家Q版形象脚本")]
        public GTDMonoDragBoxGamePlayerCuteActor monoPlayerCuteActor;
        
        public event Action gameSuccess;
        
        public void onGameSuccess()
        {
            gameSuccess?.Invoke();
        }
    }
}