using System;
using ALPackage;
using GS2GC.p004_PlayerOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分享item
    /// </summary>
    public class NPGGUIWndCommonShareItem : _ATALBasicUISubWnd<NPGGUIMonoCommonShareItem>
    {
        private long _m_lCid;//玩家cid
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;//玩家信息
        private long _m_lShowSerial;//显示序列号
        private Action<long,Action> _m_shareAction;
        private Func<long,bool> _m_isOnCDFunc;

        public NPGGUIWndCommonShareItem(NPGGUIMonoCommonShareItem _mono) : base(_mono)
        {
            initWnd();
        }

        public long cid { get => _m_lCid; }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lCid = 0;
            _m_shareAction = null;
            _m_isOnCDFunc = null;
            _m_lShowSerial = ALSerializeOpMgr.next();

            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_lCid = 0;
            _m_shareAction = null;
            _m_isOnCDFunc = null;
            _m_lShowSerial = ALSerializeOpMgr.next();

            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnShare, _onClickBtnShare);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //玩家信息
            if (wnd.monoPlayerIcon != null)
            {
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
            }

            ALUGUICommon.combineBtnClick(wnd.btnShare, _onClickBtnShare);
        }


        #region 点击事件

        /// <summary>
        /// 点击分享按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnShare(GameObject _go)
        {
            if (_m_shareAction == null)
                return;

            _m_shareAction(_m_lCid,_refreshShareCdShow);
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //请求玩家信息
            long serialize = _m_lShowSerial = ALSerializeOpMgr.next();
            GCommon.reqPlayerInfoSer(_m_lCid,(_msg) =>
            {
                if (serialize != _m_lShowSerial || _msg == null || wnd == null)
                    return;
                NPCommonSimplePlayerInfo playerInfo = new NPCommonSimplePlayerInfo(_msg.getSomeOneShowInfo());
                _m_wPlayerIcon?.setPlayerInfo(playerInfo);
                ALUGUICommon.setGameObjEnable(wnd.goListShowOnOffline, !playerInfo.isOnline);
                ALUGUICommon.setGameObjEnable(wnd.goListHideOnOffline, playerInfo.isOnline);
            });
            //分享cd相关显示
            _refreshShareCdShow();
            // 刷新每种不同子类需要显示的额外文本
            _refreshExText();
        }

        /// <summary>
        /// 额外显示的文本
        /// </summary>
        /// <param name="_exText"></param>
        protected virtual void _refreshExText()
        {
            // ALUGUICommon.setLabelTxt(wnd.exText,null);
        }

        /// <summary>
        /// 刷新分享cd相关显示
        /// </summary>
        private void _refreshShareCdShow()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goListShowOnShareCd, _isOnShareCD());
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnShareCd, !_isOnShareCD());
        }

        /// <summary>
        /// 是否在cd中的判断
        /// </summary>
        /// <returns></returns>
        protected bool _isOnShareCD()
        {
            bool isOnChareCD = (null == _m_isOnCDFunc) ? false : _m_isOnCDFunc(_m_lCid);
            return isOnChareCD;
        }

        #endregion


        #region 外部调用

        public bool tryDealShare()
        {
            if (_m_shareAction == null)
                return false;

            if (null != _m_isOnCDFunc && _m_isOnCDFunc(_m_lCid))
            {
                return false;
            }
            else
            {
                _m_shareAction(_m_lCid, _refreshShareCdShow);
                return true;
            }
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_shareAction">分享的回调</param>
        /// <param name="_isOnCDFunc">是否在cd中</param>
        public void setInfo(long _cid,Action<long,Action> _shareAction,Func<long,bool> _isOnCDFunc)
        {
            
            _m_shareAction =  _shareAction;
            _m_isOnCDFunc = _isOnCDFunc;
            _m_lCid = _cid;
            _refreshWnd();
        }

        #endregion

    }
}
