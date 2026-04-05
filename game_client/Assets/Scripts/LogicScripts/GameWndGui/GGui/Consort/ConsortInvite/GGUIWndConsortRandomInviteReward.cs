using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子随机邀约奖励窗口
    /// </summary>
    public class GGUIWndConsortRandomInviteReward : _ATALBasicUIWnd<GGUIMonoConsortRandomInviteReward>
    {
        private static GGUIWndConsortRandomInviteReward _g_instance;
        public static GGUIWndConsortRandomInviteReward instance { get { return _g_instance ??= new GGUIWndConsortRandomInviteReward(); } }
        
        private Common.ConsortObj.Consort_CallRes _m_ConsortCallRes;//邀约结果
        
        private GGUIWndConsortInviteRewardItem _m_wRewardItem;
        
        public GGUIWndConsortRandomInviteReward() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoConsortRandomInviteReward.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortRandomInviteReward.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if(wnd.rewardItem != null)
                _m_wRewardItem = new GGUIWndConsortInviteRewardItem(wnd.rewardItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wRewardItem?.discard();
            _m_wRewardItem = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wRewardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardItem?.resetWnd();
        }

        public void setData(Common.ConsortObj.Consort_CallRes _result)
        {
            _m_ConsortCallRes = _result;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_ConsortCallRes == null || !isShow)
                return;

            if (_m_wRewardItem != null)
            {
                _m_wRewardItem.showWnd();
                _m_wRewardItem.setData(_m_ConsortCallRes);
            }
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_RANDOM_INVITE_REWARD);
        }
    }
}