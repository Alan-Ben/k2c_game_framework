using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 拼图游戏窗口
    /// </summary>
    public class GGUIMonoPuzzleGame : _AALBasicUIWndMono
    {
        [ALHeader("游戏prefab挂载父节点")]
        public Transform parent;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10300); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10300);} }
    }
}