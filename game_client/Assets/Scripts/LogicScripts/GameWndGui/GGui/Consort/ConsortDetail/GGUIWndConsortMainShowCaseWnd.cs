using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 妃子详细信息子窗口
    /// </summary>
    public class GGUIWndConsortMainShowCaseWnd : _ATALBasicUIWnd<GGUIMonoConsortMainShowCaseWnd>
    {
        private static GGUIWndConsortMainShowCaseWnd _g_instance;
        public static GGUIWndConsortMainShowCaseWnd instance { get { return _g_instance ??= new GGUIWndConsortMainShowCaseWnd(); } }

        private _IConsortShowInfo _m_consortShowInfo;//妃子展示信息
        private GConsortRefObj _m_rConsortRefObj;//妃子配表数据
        
        private GGUIWndConsortShowCaseSubWnd _m_wndCommonShowCase;//展示窗口
        
        [NotNull] private List<CommonUISfxObj> _m_lSendGiftSfxObjList = new List<CommonUISfxObj>();//赠送礼物特效对象
        
        public GGUIWndConsortMainShowCaseWnd() : base(EALUIWndLayer.GAME_WORLD_UI_NOOP)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortMainShowCaseWnd.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortMainShowCaseWnd.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoShowCaseSubWnd != null)
                _m_wndCommonShowCase = new GGUIWndConsortShowCaseSubWnd(wnd.monoShowCaseSubWnd);
        }
        
        protected override void _onDiscard()
        {
            _m_wndCommonShowCase?.discard();
            _m_wndCommonShowCase = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
        }

        protected override void _onHideWnd()
        {

            foreach (var sendGiftSfxObj in _m_lSendGiftSfxObjList)
            {
                sendGiftSfxObj?.forceDiscard();
            }
            _m_lSendGiftSfxObjList.Clear();
            
            _m_wndCommonShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndCommonShowCase?.resetWnd();
        }

        public void setData(_IConsortShowInfo _consortShowInfo, bool _isPlayEnter)
        {
            _m_consortShowInfo = _consortShowInfo;
            _m_rConsortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_consortShowInfo?.consortId ?? 0);
            
            _refreshWnd(_isPlayEnter);
        }

        private void _refreshWnd(bool _isPlayEnter = false)
        {
            if(_m_consortShowInfo == null || wnd == null || _m_rConsortRefObj == null)
                return;

            if (_m_wndCommonShowCase != null)
            {
                _m_wndCommonShowCase.showWnd();
                _m_wndCommonShowCase.setData(_m_consortShowInfo, _isPlayEnter);
            }
        }
        
        /// <summary>
        /// 播放tdShow的动画
        /// </summary>
        /// <param name="_tdShowAniType"></param>
        /// <param name="_onComplete"></param>
        public void setTdShowAni(EConsortTdShowAniType _tdShowAniType, bool _forcePlay, Action _onComplete)
        {
            if (_m_wndCommonShowCase == null)
            {
                _onComplete?.Invoke();
                return;
            }
            
            _m_wndCommonShowCase.setTdShowAni(_tdShowAniType, _forcePlay, _onComplete);
        }

        #region 特效

        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="_sfxId"></param>
        private CommonUISfxObj _playSfx(long _sfxId)
        {
            if(wnd == null || _sfxId <= 0)
                return null;

            return PlaySfxMgr.instance.playUISfx(_sfxId, wnd.consortSfxParent);
        }

        /// <summary>
        /// 播放赠送礼物特效
        /// </summary>
        /// <param name="_sfxId"></param>
        public void playSendGiftSfx(Common.BagItemUseEnum.EBagItemUse_ConsortType _bagItemUseConsortType)
        {
            if(wnd == null)
                return;

            long sfxId = 0;
            switch (_bagItemUseConsortType)
            {
                case Common.BagItemUseEnum.EBagItemUse_ConsortType.INTIMACY:
                    sfxId = wnd.sendIntimacyGiftSfxId;
                    break;
                
                case Common.BagItemUseEnum.EBagItemUse_ConsortType.CHARM:
                    sfxId = wnd.sendCharmGiftSfxId;
                    break;
            }
            
            if(sfxId <= 0)
                return;
            
            CommonUISfxObj sfxObj = _playSfx(sfxId);
            if (sfxObj != null)
            {
                _m_lSendGiftSfxObjList.Add(sfxObj);
            }
        }
        
        #endregion

        #region 窗口消息


        #endregion
    }
}