using ALPackage;

namespace GOE
{
    public class GGUIWndAkeyTravelConsortEventResultItem : _AGGUIWndAkeyTravelResultItem<GGUIMonoAkeyTravelConsortEventResultItem>
    {
        private _ATravelConsortEventResultInfo _m_iConsortEventResultInfo;
        
        private NPGGUIWndProgress _m_likeProgress;//好感度进度条
        
        public GGUIWndAkeyTravelConsortEventResultItem(GGUIMonoAkeyTravelConsortEventResultItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoLikeSlider != null)
                _m_likeProgress = new NPGGUIWndProgress(wnd.monoLikeSlider);
        }

        protected override void _onDiscardSub()
        {
            _m_likeProgress?.discard();
            _m_likeProgress = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_likeProgress?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_likeProgress?.resetWnd();
        }

        protected override void _onSetData(_ITravelEventResultInfo _data)
        {
            if(_data == null)
                return;
            
            if (!(_data is _ATravelConsortEventResultInfo))
            {
                Debug.LogError_EditorOnly($"[GGUIWndAkeyTravelConsortEventResultItem _onSetData] 事件:[id:{_data.eventInfo?.eventId} type:{_data.eventInfo?.eventType}] 传入的事件结束数据不是_ATravelConsortEventResultInfo类型" +
                                     $"但是却使用妃子相关事件一键展示GGUIWndAkeyTravelConsortEventResultItem, 请检查配置");
                
                _m_iConsortEventResultInfo = null;
            }
            else
            {
                _m_iConsortEventResultInfo = (_ATravelConsortEventResultInfo) _data;
            }
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null || _m_iConsortEventResultInfo == null)
                return;

            if (_m_iConsortEventResultInfo.addIntimacy > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddIntimacyShowGoList, true);
                
                string addIntimacyKey = string.IsNullOrEmpty(wnd.txtAddIntimacyKey) ? TransKeyConst.common_add_num : wnd.txtAddIntimacyKey;
                ALUGUICommon.setLabelTxt(wnd.txtAddIntimacy, TextTranslate.instance.getLanguage(addIntimacyKey, _m_iConsortEventResultInfo.addIntimacy));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddIntimacyShowGoList, false);
            }

            
            if (_m_iConsortEventResultInfo.addLike > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddLikeShowGoList, true);
                
                string addLikeKey = string.IsNullOrEmpty(wnd.txtAddLikeKey) ? TransKeyConst.common_add_num : wnd.txtAddLikeKey;
                ALUGUICommon.setLabelTxt(wnd.txtAddLike, TextTranslate.instance.getLanguage(addLikeKey, _m_iConsortEventResultInfo.addLike));

                if (_m_iConsortEventResultInfo.consortShowInfo != null)
                {
                    TravelConsortRefObj travelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_m_iConsortEventResultInfo.consortShowInfo.consortId);
                    if (_m_likeProgress != null && travelConsortRefObj != null && travelConsortRefObj.marry_need_like > 0)
                    {
                        _m_likeProgress.showWnd();
                        _m_likeProgress.setProgress(_m_iConsortEventResultInfo.afterEventLike, travelConsortRefObj?.marry_need_like ?? 0, EValueFormatType.NORMAL_NOT_LARGE_STR);
                    }       
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddLikeShowGoList, false);
            }
        }
    }
}