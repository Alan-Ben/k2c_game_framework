using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingVideoShow
    {
        private readonly GGUIMonoBusinessBuildingVideoShow _m_wnd;
        private bool _m_isShow;
        
        private BusinessBuildingVideoGroupRefObj _m_videoGroupRef;
        private int _m_buildingLevel;
        private List<BusinessBuildingDevelopRefObj> _m_developList;
            
        private int _m_currentIndex;
        
        private NPGGUIWndCommonShowCase _m_monitorShowCase;
        private GGUISubWndBusinessBuildingVideoIndexContainer _m_indexContainer;
        private NPGGUIWndCommonToggleEx _m_autoPlayToggle;
        private int _m_autoPlayTimeCounter;
        private CommonUISfxObj _m_videoChgSfxObj;

        private ALCommonEnableTaskController _m_tickPerSecondTask;


        public GGUIWndBusinessBuildingVideoShow(GGUIMonoBusinessBuildingVideoShow _wnd)
        {
            _m_wnd = _wnd;
            initWnd();
        }
        
        
        public GGUIMonoBusinessBuildingVideoShow wnd { get { return _m_wnd; } }


        public void showWnd()
        {
            _m_isShow = true;
            
            _m_monitorShowCase?.showWnd();
            _m_indexContainer?.showWnd();
            _m_autoPlayToggle?.showWnd();

            _m_autoPlayTimeCounter = 0;
            _m_tickPerSecondTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_tickPerSecond, 1f);

            refreshWnd();

            GGUIWndBusinessBuildingUpgradeSuccess.instance.onWndHide += _playVideoChgEffect;
        }
        public void hideWnd()
        {
            GGUIWndBusinessBuildingUpgradeSuccess.instance.onWndHide -= _playVideoChgEffect;
            
            _m_monitorShowCase?.hideWnd();
            _m_indexContainer?.hideWnd();
            _m_autoPlayToggle?.hideWnd();
            
            _m_tickPerSecondTask.setDisable();
            
            _m_isShow = false;
        }
        public void resetWnd()
        {
            _m_monitorShowCase?.resetWnd();
            _m_indexContainer?.resetWnd();
            _m_autoPlayToggle?.resetWnd();
        }
        public void discard()
        {
            _m_monitorShowCase?.discard();
            _m_indexContainer?.discard();
            _m_monitorShowCase = null;
            _m_indexContainer = null;

            if (_m_autoPlayToggle != null)
            {
                _m_autoPlayToggle.clickDelegate -= _onAutoPlayToggleClick;
                _m_autoPlayToggle.discard();
                _m_autoPlayToggle = null;
            }
            
            _m_videoChgSfxObj?.forceDiscard();
            _m_videoChgSfxObj = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onBtnNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrev, _onBtnPrev);
        }
        public void initWnd()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoMonitorShowcase != null)
                _m_monitorShowCase = new NPGGUIWndCommonShowCase(wnd.monoMonitorShowcase);
            if (wnd.monoIndexContainer != null)
                _m_indexContainer = new GGUISubWndBusinessBuildingVideoIndexContainer(wnd.monoIndexContainer, _onIndexItemClick);
            if (wnd.monoAutoPlay != null)
            {
                _m_autoPlayToggle = new NPGGUIWndCommonToggleEx(wnd.monoAutoPlay);
                _m_autoPlayToggle.clickDelegate += _onAutoPlayToggleClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onBtnNext);
            ALUGUICommon.combineBtnClick(wnd.btnPrev, _onBtnPrev);
        }


        public void refreshWnd(BusinessBuildingVideoGroupRefObj _videoGroupRef, long _buildingId, int _buildingLevel)
        {
            _m_developList = GRefdataCoreMgr.instance.getBusinessBuildingDevelopRefList(_buildingId);
            _m_buildingLevel = _buildingLevel;
            // 如果有差异更新视频的部分
            if (_m_videoGroupRef != _videoGroupRef)
            {
                _m_videoGroupRef = _videoGroupRef;
                _m_monitorShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_videoGroupRef.video_res_index));
                setVideoIndex(0);
            }
            else 
                _refreshLockMask(_m_currentIndex);

            _refreshTimeNow();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_isShow || _m_videoGroupRef == null)
                return;

            _m_monitorShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_videoGroupRef.video_res_index));
            
            setVideoIndex(0);
            _refreshTimeNow();
        }
        public void setVideoIndex(int _index, int _customLevel = -1)
        {
            if (wnd == null)
                return;
            
            if (_m_videoGroupRef?.video_anim_name_list is not { Count: > 0 })
                return;

            _m_currentIndex = NPGameUtility.intRepeat(_index, _m_videoGroupRef.video_anim_name_list.Count);
            _m_monitorShowCase?.forceSetAni(0, _m_videoGroupRef.video_anim_name_list[_m_currentIndex]);
            _m_indexContainer?.refreshWnd(_m_videoGroupRef.video_anim_name_list.Count, _m_currentIndex);
            ALUGUICommon.setLabelTxt(wnd.txtVideoName, _m_videoGroupRef.video_name_list.SafeGet(_m_currentIndex));
            _refreshLockMask(_m_currentIndex, _customLevel);
            _refreshSplitLine(_m_currentIndex);
        }
        
        
        private void _onBtnNext(GameObject _obj)
        {
            setVideoIndex(_m_currentIndex + 1);
        }
        private void _onBtnPrev(GameObject _obj)
        {
            setVideoIndex(_m_currentIndex - 1);
        }
        private void _onIndexItemClick(int _index)
        {
            setVideoIndex(_index);
        }
        private void _onAutoPlayToggleClick(NPGGUIWndCommonToggleEx _obj)
        {
            _m_autoPlayToggle?.setSelected(!_m_autoPlayToggle.isOn);
        }
        private void _tickPerSecond()
        {
            _refreshTimeNow();

            if (wnd == null)
                return;

            if (_m_autoPlayToggle == null || _m_autoPlayToggle.isOn)
            {
                _m_autoPlayTimeCounter++;
                if (_m_autoPlayTimeCounter >= wnd.autoPlayInterval)
                {
                    _m_autoPlayTimeCounter = 0;
                    setVideoIndex(_m_currentIndex + 1);
                }
            }
        }
        private void _playVideoChgEffect(bool _play, BusinessBuildingDevelopRefObj _newestDevelopRef)
        {
            if (wnd == null || !_play || _m_videoGroupRef == null)
                return;

            _m_videoChgSfxObj?.forceDiscard();
            int index = _m_developList?.IndexOf(_newestDevelopRef) ?? -1;
            if (index < 0 || index >= wnd.listLockMask.Count)
                return;

            GGUIMonoBusinessBuildingVideoShowLockMask maskMono = wnd.listLockMask[index];
            if (maskMono == null)
                return;
            
            _m_videoChgSfxObj = PlaySfxMgr.instance.playUISfx(maskMono.videoChgEffectSfxId, maskMono.videoChgEffectPos);
            setVideoIndex(_m_videoGroupRef.need_show_lock_mask_index);
        }

        private void _refreshTimeNow()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtTimeNow, TimeUtil.DateTime2StringMDYHMS(DateTime.Now));
        }
        private void _refreshLockMask(int _index, int _customLevel = -1)
        {
            if (wnd == null || _m_videoGroupRef == null)
                return;
            
            int buildingLevel = _customLevel >= 0 ? _customLevel : _m_buildingLevel;
            // 按顺序对遮罩进行处理
            for (int i = 0; i < wnd.listLockMask.Count; i++)
            {
                bool needShow = _index == _m_videoGroupRef.need_show_lock_mask_index && // 需要当前的索引是需要展示的索引才有可能需要展示
                                _m_developList != null &&
                                (i >= _m_developList.Count || // 如果遮罩所需要求没配置那么多，就直接显示
                                 _m_developList[i].level_required > buildingLevel); // 或是建筑等级没达到要求的等级

                ALUGUICommon.setGameObjEnable(wnd.listLockMask[i].goMask, needShow);
                ALUGUICommon.setLabelTxt(wnd.listLockMask[i].txtUnlockTip, TextTranslate.instance.getLanguage(TransKeyConst.building_videoUnlockTip_level, _m_developList.SafeGet(i)?.level_required ?? 0));
            }
        }
        private void _refreshSplitLine(int _index)
        {
            if (wnd == null || _m_videoGroupRef == null)
                return;

            bool needShow = _m_videoGroupRef.show_split_line_index == _index;
            ALUGUICommon.setGameObjEnable(wnd.listSplitLineShow, needShow);
        }
    }
}