using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 累计召唤奖励领取窗口
    /// </summary>
    public class GGUIWndSummonCumulativeNumRewardDraw : _ANPGGUIBasicWnd<GGUIMonoSummonCumulativeNumRewardDraw>
    {
        private static GGUIWndSummonCumulativeNumRewardDraw _g_instance;
        public static GGUIWndSummonCumulativeNumRewardDraw instance { get { return _g_instance ??= new GGUIWndSummonCumulativeNumRewardDraw(); } }
        
        private long _m_lGachaPoolId;//抽卡卡池id
        private GachaPoolInfo _m_iGachaPoolInfo;//抽卡卡池信息
        private GachaPoolRefObj _m_rGachaPoolRefObj;//抽卡卡池ref
        private CommonItemData _m_RewardItemData;//奖励item数据
        
        private NPGGUIWndCommonItem _m_wRewardItem;//奖励item
        
        public GGUIWndSummonCumulativeNumRewardDraw() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoSummonCumulativeNumRewardDraw.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSummonCumulativeNumRewardDraw.objName; } }
        protected override _AALResourceCore _resourceCore { get{ return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoRewardItem != null)
                _m_wRewardItem = new NPGGUIWndCommonItem(wnd.monoRewardItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnDrawReward, _onDrawRewardBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDrawReward, _onDrawRewardBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wRewardItem?.discard();
            _m_wRewardItem = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRewardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardItem?.resetWnd();
        }

        public void setData(long _gachaPoolId)
        {
            _m_lGachaPoolId = _gachaPoolId;
            _m_iGachaPoolInfo = NPPlayer.instance.gachaComp.getGachaPoolInfo(_m_lGachaPoolId);
            _m_rGachaPoolRefObj = _m_iGachaPoolInfo?.poolRefObj ?? GRefdataCoreMgr.instance.gachaPoolRefCore.getRef(_m_lGachaPoolId);
            if (_m_rGachaPoolRefObj != null && _m_rGachaPoolRefObj.cumulative_reward_item != null)
            {
                _m_RewardItemData = new CommonItemData(_m_rGachaPoolRefObj.cumulative_reward_item);
            }
            else
            {
                _m_RewardItemData = null;
            }

            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if (wnd == null || _m_rGachaPoolRefObj == null)
                return;

            long cumulativeRewardTimes = _m_iGachaPoolInfo?.cumulativeRewardTimes ?? 0;
            long cumulativeRewardNeedNum = _m_rGachaPoolRefObj.cumulative_reward_need_num;
            if(_m_RewardItemData != null && _m_rGachaPoolRefObj.cumulative_reward_item != null)
                _m_RewardItemData.setCount(_m_rGachaPoolRefObj.cumulative_reward_item.getCount() * cumulativeRewardTimes / cumulativeRewardNeedNum);
            
            string key = string.IsNullOrEmpty(wnd.txtCanDrawRewardNeedSummonCountKey) ? TransKeyConst.common_value : wnd.txtCanDrawRewardNeedSummonCountKey;
            ALUGUICommon.setLabelTxt(wnd.txtCanDrawRewardNeedSummonCount, TextTranslate.instance.getLanguage(key, cumulativeRewardNeedNum));

            if (_m_wRewardItem != null)
            {
                _m_wRewardItem.showWnd();
                _m_wRewardItem.setItem(_m_RewardItemData);
            }
            
            ALUGUICommon.setGameObjEnable(wnd.canDrawRewardShowGoList, cumulativeRewardTimes >= cumulativeRewardNeedNum);
            ALUGUICommon.setGameObjEnable(wnd.cannotDrawRewardShowGoList, cumulativeRewardTimes < cumulativeRewardNeedNum);
            
            if(cumulativeRewardTimes >= cumulativeRewardNeedNum)
                GGameCommonInfo.disgrayImage(wnd.cannotDrawRewardGrayList);
            else
                GGameCommonInfo.grayImage(wnd.cannotDrawRewardGrayList);
        }
        
        /// <summary>
        /// 领取奖励按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onDrawRewardBtnClick(GameObject _go)
        {
            if(_m_iGachaPoolInfo == null || _m_rGachaPoolRefObj == null || _m_iGachaPoolInfo.cumulativeRewardTimes < _m_rGachaPoolRefObj.cumulative_reward_need_num)
                return;
            
            NPPlayer.instance.gachaComp.reqDrawGachaCumulativeReward(_m_rGachaPoolRefObj.id, (_msg) =>
            {
                _refreshWnd();
            }, null);
        }
        
        /// <summary>
        /// 关闭窗口按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SUMMON_CUMULATIVE_NUM_REWARD_DRAW);
        }
    }
}