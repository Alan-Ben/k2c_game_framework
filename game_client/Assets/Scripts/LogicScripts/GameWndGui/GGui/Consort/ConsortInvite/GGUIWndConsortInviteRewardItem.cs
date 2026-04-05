using ALPackage;

namespace GOE
{
    /// <summary>
    /// 邀约结果item
    /// </summary>
    public class GGUIWndConsortInviteRewardItem : _ATALBasicUISubWnd<GGUIMonoConsortInviteRewardItem>
    {
        private Common.ConsortObj.Consort_CallRes _m_ConsortCallRes;//邀约结果

        private GGUIWndConsortIconItem _m_wConsortHead;//妃子头像
        private GGUIWndConsortCardItem _m_wConsortCard;//妃子卡牌
        
        public GGUIWndConsortInviteRewardItem(GGUIMonoConsortInviteRewardItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.consortHeadMono != null)
                _m_wConsortHead = new GGUIWndConsortIconItem(wnd.consortHeadMono);

            if (wnd.consortCardMono != null)
                _m_wConsortCard = new GGUIWndConsortCardItem(wnd.consortCardMono, null);
        }
        
        protected override void _onDiscard()
        {
            _m_wConsortHead?.discard();
            _m_wConsortHead = null;
            
            _m_wConsortCard?.discard();
            _m_wConsortCard = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wConsortHead?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortHead?.resetWnd();
        }
        
        public void setData(Common.ConsortObj.Consort_CallRes _consortCallRes)
        {
            _m_ConsortCallRes = _consortCallRes;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_ConsortCallRes == null)
                return;

            ConsortRefShowInfo consortRefShowInfo = new ConsortRefShowInfo(_m_ConsortCallRes.getConsortId());

            if (_m_wConsortHead != null)
            {
                _m_wConsortHead.showWnd();
                _m_wConsortHead.setInfo(consortRefShowInfo, 0);
            }

            if (_m_wConsortCard != null)
            {
                _m_wConsortCard.showWnd();
                _m_wConsortCard.setInfo(consortRefShowInfo);
            }

            if (string.IsNullOrEmpty(wnd.txtAddCharmPointKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtAddCharmPoint,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_ConsortCallRes.getAddCharmPoint()));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtAddCharmPoint, TextTranslate.instance.getLanguage(wnd.txtAddCharmPointKey, _m_ConsortCallRes.getAddCharmPoint()));
            }

            ConsortStoryRefObj consortStoryRefObj = GRefdataCoreMgr.instance.consortStoryRefCore.getRef(_m_ConsortCallRes.getConsortStoryId());
            long cgId = consortStoryRefObj?.unlock_cg ?? 0;
            ConsortCGRefObj cgRefObj = GRefdataCoreMgr.instance.consortCGRefCore.getRef(cgId);

            ALUGUICommon.setLabelTxt(wnd.txtInviteStoryDesc, TextTranslate.instance.getLanguage(consortStoryRefObj?.story_desc ?? string.Empty));
            if (cgRefObj != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasCgRewardShow, true);

                if (string.IsNullOrEmpty(wnd.txtCgRewardKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtCgReward, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, cgRefObj.add / 100));
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtCgReward, TextTranslate.instance.getLanguage(wnd.txtCgRewardKey, cgRefObj.add / 100));
                }   
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasCgRewardShow, false);
            }
        }
    }
}