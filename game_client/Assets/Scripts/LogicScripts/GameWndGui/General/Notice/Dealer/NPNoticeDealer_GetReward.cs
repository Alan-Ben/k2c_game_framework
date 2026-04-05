using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class NPNoticeDealer_GetReward : _ATNPNoticeDealer_GetReward<NPGGUIMonoGetItem, NPGGUIWndGetItem>
    {
        public static NPNoticeDealer_GetReward makeRewardNotice(List<NPCommon.NPCommon_ItemInfo> _itemList, string _titleKey = TransKeyConst.common_getreward_tip, Action _closeAction = null)
        {
            List<NPCommon.NPCommon_ItemInfo> resList = GCommon.filterPopWndEnableShowItemList(_itemList);
            if (resList == null || resList.Count <= 0)
                return null;

            return new NPNoticeDealer_GetReward(resList, _titleKey, _closeAction);
        }

        protected NPNoticeDealer_GetReward(List<NPCommon.NPCommon_ItemInfo> _itemList, string _titleKey,  Action _closeAction) : base(_itemList, _titleKey, _closeAction)
        {

        }
        
        protected override NPGGUIWndGetItem _wnd { get { return NPGGUIWndGetItem.instance; } }
    }
}
