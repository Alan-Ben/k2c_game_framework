using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会举办成功弹窗
    /// </summary>
    public class GGUIWndDinnerCreateSucc : _ATALBasicUIWnd<GGUIMonoDinnerCreateSucc>
    {
        private static GGUIWndDinnerCreateSucc _g_instance = new GGUIWndDinnerCreateSucc();
        private GDinnerInfo _m_dinnerInfo;
        private Action _m_onClose;

        public static GGUIWndDinnerCreateSucc instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerCreateSucc();
                return _g_instance;
            }
        }
    
        private GGUIWndConsortIconItem _m_consortIconItem;
    private GGUISubWndChildInfo _m_childCardItem;
    private long _m_showSerializeOp;
        public GGUIWndDinnerCreateSucc() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get => GGUIMonoDinnerCreateSucc.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerCreateSucc.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override void _onShowWnd()
        {
        
        }

        protected override void _onHideWnd()
        {
            _m_showSerializeOp = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_showSerializeOp = ALSerializeOpMgr.next();
            _m_childCardItem?.resetWnd();
            _m_consortIconItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_consortIconItem != null)
                _m_consortIconItem.discard();
            _m_consortIconItem = null;
            _m_showSerializeOp = ALSerializeOpMgr.next();
            _m_childCardItem?.discard();
            _m_childCardItem = null;
            if(null == wnd)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.consortCardItem != null)
                _m_consortIconItem = new GGUIWndConsortIconItem(wnd.consortCardItem);
            if (wnd.childCardItem != null)
                _m_childCardItem = new GGUISubWndChildInfo(wnd.childCardItem);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_dinnerInfo"></param>
        public void setInfo(GDinnerInfo _dinnerInfo, Action _onClose)
        {
            _m_dinnerInfo = _dinnerInfo;
            _m_onClose = _onClose;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(null == _m_dinnerInfo)
                return;
            _m_showSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_showSerializeOp;
            
            if (_m_dinnerInfo.isConsortDinner())
            {
                _m_consortIconItem?.showWnd();
                _m_consortIconItem?.setInfo(new ConsortInfo(_m_dinnerInfo.getPermitConsortId()), 0); 
                _m_childCardItem?.hideWnd();
            }
            else if (_m_dinnerInfo.isGiftdeChildCeleDinner())
            {
                _m_consortIconItem?.hideWnd();
                _m_childCardItem?.hideWnd();
                NPPlayer.instance.childComp.getAdultChildInfoById(_m_dinnerInfo.getPermitChildId(), (_childInfo) =>
                {
                    if (serializeOp != _m_showSerializeOp)
                        return;

                    if (_childInfo == null)
                    {
                        _m_childCardItem?.hideWnd();
                        return;
                    }

                    _m_childCardItem?.showWnd();
                    _m_childCardItem?.refreshWnd(_childInfo);
                });
            }
            else
            {
                _m_consortIconItem?.hideWnd();
                _m_childCardItem?.hideWnd();
            }
         

            GDinnerTypeRefObj dinnerTypeRefObj = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(_m_dinnerInfo.dinnerId);

            if (dinnerTypeRefObj != null)
                DinnerTypeShow.SetDinnerType(wnd.dinnerTypeShowList, dinnerTypeRefObj.dinner_show_type);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            _m_onClose?.Invoke();
            _m_onClose = null;
        }
    }
}