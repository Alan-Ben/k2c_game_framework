
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    [Serializable]
    public class ChildSeatRefObj : _IALBasicRefObj 
    {
        public long _refId { get { return seat_id; } }

        public long seat_id;
        public _NPPlayerConditionSerializeInfo seat_unlock_condition;
        public string seat_unlock_desc;
        public List<string> seat_unlock_desc_args;


        [Pure]
        public string getTranslatedUnlockDesc()
        {
            return TextTranslate.instance.getLanguage(seat_unlock_desc, seat_unlock_desc_args);
        }
    }
    public class GSOChildSeatRefSet : _TALSOBasicRefSet<ChildSeatRefObj>
    {
        public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
        public static string objName { get { return "child_seat"; } }
    }
}