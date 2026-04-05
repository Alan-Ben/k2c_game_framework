using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// Q版形象表
    /// </summary>
    [Serializable]
    public class CuteActorRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//Q版形象id
        public NPGGoIndex go_index;//Q版形象go_index
    }
    
    public class GSOCuteActorRefSet : _TALSOBasicRefSet<CuteActorRefObj>
    {
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "cute_actor"; } }
    }
}