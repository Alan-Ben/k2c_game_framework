using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GiftPackGroupPatchDealer : _ACommonListHotRefPatchDealer
    {
        public override string getTableName
        {
            get { return GSOGiftPackGroupRefSet.objName; }
        }

        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            GiftPackGroupRefObj obj = GRefdataCoreMgr.instance.giftPackGroupRefCore.getRef(_refId);
            if (obj == null)
                return false;
            
            if (_valueDict.TryGetValue("gift_pack_id_list", out string idListStr))
                obj.gift_pack_id_list = ALCommon.ParseLongList(idListStr);

            if (_valueDict.TryGetValue("activity_id", out string actIdStr))
                obj.activity_id = ALCommon.GetLong(actIdStr);

            return true;
        }
    }
}