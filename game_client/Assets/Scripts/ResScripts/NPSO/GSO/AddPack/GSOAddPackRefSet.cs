
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class AddPackRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        
        public long id;
        public string pack_name;
        public NPGTextureIndex pack_icon;
        
        [NotNull, NonSerialized, ALAutoExportVariableAttr(true, true, true)]
        public readonly List<string> pack_paths = new List<string>();
    }
    public class GSOAddPackRefSet : _TALSOBasicRefSet<AddPackRefObj>
    {
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "add_pack"; } }
    }
}