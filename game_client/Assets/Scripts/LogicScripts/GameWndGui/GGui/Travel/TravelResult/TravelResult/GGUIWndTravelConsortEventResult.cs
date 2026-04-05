
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子事件结果窗口
    /// </summary>
    public class GGUIWndTravelConsortEventResult : _ATravelResultWnd<GGUIMonoTravelConsortEventResult, _ATravelConsortEventResultInfo>
    {
        private _ATravelConsortEventResultInfo _m_consortEventResultInfo;

        private NPGGuiWndTexture _m_wConsortMidIcon;//妃子半身像
        private NPGGuiWndTexture _m_wPlayerMidIcon;//玩家半身像
        
        private NPGGUIWndProgress _m_wLikeProgress;//好感度进度条
        
        public GGUIWndTravelConsortEventResult(_ATravelConsortEventResultInfo _eventResultInfo) : base(_eventResultInfo, GGUIMonoTravelConsortEventResult.uiResPathId, EALUIWndLayer.ADDITION)
        {
            _m_consortEventResultInfo = _eventResultInfo;  
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.consortMidIcon != null)
                _m_wConsortMidIcon = new NPGGuiWndTexture(wnd.consortMidIcon);

            if (wnd.playerMidIcon != null)
                _m_wPlayerMidIcon = new NPGGuiWndTexture(wnd.playerMidIcon);

            if (wnd.monoLikeSlider != null)
                _m_wLikeProgress = new NPGGUIWndProgress(wnd.monoLikeSlider);
        }

        protected override void _onDiscardSub()
        {
            _m_wConsortMidIcon?.discard();
            _m_wConsortMidIcon = null;
            
            _m_wPlayerMidIcon?.discard();
            _m_wPlayerMidIcon = null;
            
            _m_wLikeProgress?.discard();
            _m_wLikeProgress = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wConsortMidIcon?.hideWnd();
            _m_wPlayerMidIcon?.hideWnd();
            _m_wLikeProgress?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wConsortMidIcon?.discardTexture();
            _m_wPlayerMidIcon?.discardTexture();
            _m_wLikeProgress?.resetWnd();
        }

        protected override void _onRefreshWnd()
        {
            if (_m_consortEventResultInfo == null || _m_consortEventResultInfo.consortShowInfo == null || wnd == null)
                return;
            
            if (_m_wConsortMidIcon != null)
            {
                _m_wConsortMidIcon.showWnd();
                _m_wConsortMidIcon.setTexture(_m_consortEventResultInfo.consortShowInfo.consortSkinShowInfo?.consortCardImage);
            }

            if (_m_wPlayerMidIcon != null)
            {
                if(NPPlayer.instance.playerInfo.curSkinRef == null)
                    _m_wPlayerMidIcon.hideWnd();
                else
                {
                    _m_wPlayerMidIcon.showWnd();
                    _m_wPlayerMidIcon.setTexture(NPPlayer.instance.playerInfo.curSkinRef.card_image);
                }
            }
            
            // 有好感度增加时
            if (_m_consortEventResultInfo.addLike > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddLikeShow, true);

                TravelConsortRefObj travelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_m_consortEventResultInfo.consortShowInfo.consortId);
                if (_m_wLikeProgress != null)
                {
                    _m_wLikeProgress.showWnd();
                    _m_wLikeProgress.setProgress(_m_consortEventResultInfo.afterEventLike, travelConsortRefObj?.marry_need_like ?? 0, EValueFormatType.NORMAL_NOT_LARGE_STR);
                }
                
                string addLikeKey = string.IsNullOrEmpty(wnd.txtAddLikeKey) ? TransKeyConst.common_add_num : wnd.txtAddLikeKey;
                ALUGUICommon.setLabelTxt(wnd.txtAddLike, TextTranslate.instance.getLanguage(addLikeKey, _m_consortEventResultInfo.addLike));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddLikeShow, false);
            }
            
            // 有亲密度增加时
            if (_m_consortEventResultInfo.addIntimacy > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddIntimacyShow, true);
                string addIntimacyKey = string.IsNullOrEmpty(wnd.txtAddIntimacyKey) ? TransKeyConst.common_add_num : wnd.txtAddIntimacyKey;
                ALUGUICommon.setLabelTxt(wnd.txtAddIntimacy, TextTranslate.instance.getLanguage(addIntimacyKey, _m_consortEventResultInfo.addIntimacy));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddIntimacyShow, false);
            }
        }
    }
}