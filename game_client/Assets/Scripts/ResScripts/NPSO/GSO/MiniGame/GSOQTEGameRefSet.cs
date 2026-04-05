using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 引导游戏表
    /// </summary>
    [Serializable]
    public class QTEGameRefObj : _AMiniGameSubRefObj
    {
        public NPCommonAssetPathInfo wnd_path;//窗口路径
    }
    
    public class GSOQTEGameRefSet : _TALSOBasicRefSet<QTEGameRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.MiniGameRefSetAssetPath; } }
        public static string objName { get { return "qte_game"; } }
    }
}