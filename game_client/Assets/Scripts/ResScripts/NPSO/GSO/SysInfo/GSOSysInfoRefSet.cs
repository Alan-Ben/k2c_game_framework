using ALPackage;

namespace GOE
{
    /****************
     * 系统相关信息对象，用于展示如系统解锁，功能解锁等
     **/
    [System.Serializable]
    public class SysInfoRef : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;            
    }

    /**************
     * 物品表
     **/
    public class GSOSysInfoRefSet : _TALSOBasicRefSet<SysInfoRef>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "sys_info"; } }
    }
}

