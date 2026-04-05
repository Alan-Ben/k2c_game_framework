using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class FindThingsGameRefObj : _AMiniGameSubRefObj
    {
        public NPCommonAssetPathInfo ui_prefab_path;//ui加载预制路径
    }
    
    public class GSOFindThingsGameRefSet : _TALSOBasicRefSet<FindThingsGameRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return GSORefSetAssetPath.MiniGameRefSetAssetPath; } }
        public static string objName { get { return "find_things_game"; } }
    }
}