
using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class ChildCareerRefObj : _IALBasicRefObj 
    {
        public long _refId { get { return career_id; } }

        public long career_id;
        public long career_group_id;
        public string career_name;
        public int career_add;
        public int career_rand_wei;
        
        
        public string transName { get { return TextTranslate.instance.getLanguage(career_name); } }
    }
    public class GSOChildCareerRefSet : _TALSOBasicRefSet<ChildCareerRefObj>
    {
        public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
        public static string objName { get { return "child_career"; } }
    }
}