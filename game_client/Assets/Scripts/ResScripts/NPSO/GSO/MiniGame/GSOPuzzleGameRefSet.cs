using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 拼图游戏表
    /// </summary>
    [Serializable]
    public class PuzzleGameRefObj : _AMiniGameSubRefObj
    {
        public NPCommonAssetPathInfo ui_match_prefab;//ui不同匹配prefab
    }
    
    public class GSOPuzzleGameRefSet : _TALSOBasicRefSet<PuzzleGameRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.MiniGameRefSetAssetPath; } }
        public static string objName { get { return "puzzle_game"; } }
    }
}