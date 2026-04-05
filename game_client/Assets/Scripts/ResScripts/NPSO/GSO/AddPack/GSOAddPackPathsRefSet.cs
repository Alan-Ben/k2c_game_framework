

using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class AddPackPathsRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        
        public long id;
        public long pack_id;
        public string pack_path;
    }
    public class GSOAddPackPathsRefSet : _TALSOBasicRefSet<AddPackPathsRefObj>
    {
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "add_pack_paths"; } }
    }
}