using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键邀约奖励窗口
    /// </summary>
    public class GGUIWndConsortAkeyInviteReward : _ATALBasicUIWnd<GGUIMonoConsortAkeyInviteReward>
    {
        private static GGUIWndConsortAkeyInviteReward _g_instance;
        public static GGUIWndConsortAkeyInviteReward instance { get { return _g_instance ??= new GGUIWndConsortAkeyInviteReward(); } }
        
        private List<Common.ConsortObj.Consort_CallRes> _m_ConsortCallResList;//邀约结果
        
        private GGUIWndConsortInviteRewardItemContainer _m_wRewardItemContainer;
        
        public GGUIWndConsortAkeyInviteReward() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoConsortAkeyInviteReward.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortAkeyInviteReward.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if(wnd.rewardItemContainer != null)
                _m_wRewardItemContainer = new GGUIWndConsortInviteRewardItemContainer(wnd.rewardItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wRewardItemContainer?.discard();
            _m_wRewardItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRewardItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardItemContainer?.resetWnd();
        }

        public void setData(List<Common.ConsortObj.Consort_CallRes> _resulResList)
        {
            _m_ConsortCallResList = _resulResList;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_ConsortCallResList == null || wnd == null)
                return;

            if (string.IsNullOrEmpty(wnd.txtInviteCountKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtInviteCount, _m_ConsortCallResList.Count);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtInviteCount, TextTranslate.instance.getLanguage(wnd.txtInviteCountKey, _m_ConsortCallResList.Count));
            }

            if (_m_wRewardItemContainer != null)
            {
                _m_wRewardItemContainer.showWnd();
                _m_wRewardItemContainer.setData(_m_ConsortCallResList);
            }
        }
        
        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_AKEY_INVITE_REWARD);
        }
    }
}