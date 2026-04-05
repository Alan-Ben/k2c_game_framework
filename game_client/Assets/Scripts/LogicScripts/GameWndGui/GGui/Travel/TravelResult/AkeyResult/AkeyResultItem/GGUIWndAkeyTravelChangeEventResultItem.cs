using ALPackage;
using NPEnum;

namespace GOE
{
    public class GGUIWndAkeyTravelChangeEventResultItem : _AGGUIWndAkeyTravelResultItem<GGUIMonoAkeyTravelChangeEventResultItem>
    {
        // 游历体力值奖励数据（从奖励列表中提取）
        private CommonItemData _m_travelLazyCdRewardItem;

        public GGUIWndAkeyTravelChangeEventResultItem(GGUIMonoAkeyTravelChangeEventResultItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneSub()
        {
        }

        protected override void _onDiscardSub()
        {
            _m_travelLazyCdRewardItem = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
        }

        protected override void _onResetSub()
        {
            _m_travelLazyCdRewardItem = null;
        }

        protected override void _onSetData(_ITravelEventResultInfo _data)
        {
            _m_travelLazyCdRewardItem = null;

            if (_data == null || _data.showRewardItemList == null)
                return;

            // 从奖励列表中提取游历体力值道具，并从列表中移除
            long lazyCdId = GRefdataCoreMgr.instance.npGeneral.travel_cost_lazycd_id;
            _m_travelLazyCdRewardItem = new CommonItemData(ENPItemType.LAZY_CD, lazyCdId, 0);

            for (int i = _data.showRewardItemList.Count - 1; i >= 0; i--)
            {
                _IItem rewardItem = _data.showRewardItemList[i];
                if (rewardItem != null && rewardItem.getItemType() == ENPItemType.LAZY_CD && rewardItem.subId == lazyCdId)
                {
                    _m_travelLazyCdRewardItem.setCount(_m_travelLazyCdRewardItem.getCount() + rewardItem.getCount());

                    // 取出体力道具后，从奖励列表中移除
                    _data.showRewardItemList.RemoveAt(i);
                }
            }
        }

        protected override void _onRefreshWnd()
        {
            if (wnd == null)
                return;

            if (_m_travelLazyCdRewardItem != null && _m_travelLazyCdRewardItem.getCount() > 0)
            {
                // 有体力值奖励时，显示对应区域
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdShow, true);
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdHide, false);

                string travelLazyCdRewardKey = string.IsNullOrEmpty(wnd.gainTravelCostLazyCdKey)
                    ? TransKeyConst.common_somethingAdd_str_num
                    : wnd.gainTravelCostLazyCdKey;

                ALUGUICommon.setLabelTxt(wnd.gainTravelCostLazyCd,
                    TextTranslate.instance.getLanguage(travelLazyCdRewardKey,
                        _m_travelLazyCdRewardItem.getItemName(),
                        _m_travelLazyCdRewardItem.getCount()));
            }
            else
            {
                // 无体力值奖励时，隐藏对应区域
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdShow, false);
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdHide, true);
            }
        }
    }
}
