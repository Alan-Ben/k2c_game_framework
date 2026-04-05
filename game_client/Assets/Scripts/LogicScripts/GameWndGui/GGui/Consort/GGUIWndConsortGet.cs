using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortGet : _ATALBasicUIWnd<GGUIMonoConsortGet>
    {
        private static GGUIWndConsortGet _g_instance;

        public static GGUIWndConsortGet instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndConsortGet();
                }

                return _g_instance;
            }
        }
        private GGottenConsortInfo _m_consortInfo;
        private Action _m_closeAction;

        private GGUISubWndUnlockConsortDetailInfo _m_wConsortDetail;//妃子详情信息子窗口
        
        public GGUIWndConsortGet() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoConsortGet.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortGet.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        /// <summary>
        /// 在切换主视图时是否会需要释放
        /// 一般不释放，如果子类有需要可以重载函数处理
        /// </summary>
        public override bool needDiscardOnSwitch
        {
            get { return true; }
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            _m_wConsortDetail?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortDetail?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _clickClose);
            }

            _m_wConsortDetail?.discard();
            _m_wConsortDetail = null;
            
            if (_m_closeAction != null)
            {
                Action onClose = _m_closeAction;
                _m_closeAction = null;
                onClose?.Invoke();
            }
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoConsortDetailInfo != null)
                _m_wConsortDetail = new GGUISubWndUnlockConsortDetailInfo(wnd.monoConsortDetailInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
        }

        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_GET);
        }
        
        /// <summary>
        /// 获取转换道具部件设置信息
        /// </summary>
        public void setInfo(GGottenConsortInfo _consortInfo, Action _closeAction)
        {
            _m_consortInfo = _consortInfo;
            _m_closeAction = _closeAction;
            
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(null == _m_consortInfo)
                return;

            if (_m_wConsortDetail != null)
            {
                _m_wConsortDetail.showWnd();
                _m_wConsortDetail.setData(_m_consortInfo);
            }

            ConsortVoiceMgr.instance.playVoice(_m_consortInfo.consortId, EConsortVoiceType.Unlock);
        }
    }
}