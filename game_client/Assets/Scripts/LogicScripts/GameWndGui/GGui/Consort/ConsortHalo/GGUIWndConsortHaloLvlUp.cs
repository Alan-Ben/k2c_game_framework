using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 星辉等级提升弹窗
    /// </summary>
    public class GGUIWndConsortHaloLvlUp : _ATALBasicUIWnd<GGUIMonoConsortHaloLvlUp>
    {
        private static GGUIWndConsortHaloLvlUp _g_instance;
        public static GGUIWndConsortHaloLvlUp instance { get { return _g_instance ??= new GGUIWndConsortHaloLvlUp(); } }
        
        private ConsortHaloInfo _m_haloInfo;//星辉信息
        private bool _m_bIsUnlock;//是否是解锁
        private Action _m_aExecuteClose;//执行关闭窗口方法

        private GGUISubWndConsortHaloLvlChg _m_wHaloLvlChg;//星辉等级变化子窗口
        
        public GGUIWndConsortHaloLvlUp() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortHaloLvlUp.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortHaloLvlUp.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance;} }

        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoHaloLvlChg != null)
                _m_wHaloLvlChg = new GGUISubWndConsortHaloLvlChg(wnd.monoHaloLvlChg);
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onBtnSureClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onBtnSureClick);
            }
            
            _m_aExecuteClose = null;
            
            _m_wHaloLvlChg?.discard();
            _m_wHaloLvlChg = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHaloLvlChg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHaloLvlChg?.resetWnd();
        }

        /// <summary>
        /// 是否是解锁
        /// </summary>
        /// <param name="_isUnlock"></param>
        public void setData(ConsortHaloInfo _haloInfo, bool _isUnlock, Action _executeClose)
        {
            _m_haloInfo = _haloInfo;
            _m_bIsUnlock = _isUnlock;
            _m_aExecuteClose = _executeClose;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_haloInfo == null || wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.unlockShow, _m_bIsUnlock);
            ALUGUICommon.setGameObjEnable(wnd.unlockHide, !_m_bIsUnlock);

            // 获取上一级星辉等级配表数据
            ConsortHaloLvlRefObj preHaloLvlRefObj = GRefdataCoreMgr.instance.consortHaloLvlRefCore.getRef(_m_haloInfo.level - 1);
            if (_m_wHaloLvlChg != null)
            {
                _m_wHaloLvlChg.showWnd();
                _m_wHaloLvlChg.setData(preHaloLvlRefObj, _m_haloInfo.nowHaloLvlRefObj);
            }
        }
        
        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnSureClick(GameObject _go)
        {
            _m_aExecuteClose?.Invoke();
        }
    }
}