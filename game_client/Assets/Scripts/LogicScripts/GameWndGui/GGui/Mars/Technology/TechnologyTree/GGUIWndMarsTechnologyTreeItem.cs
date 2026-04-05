using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星科技树节点item
    /// </summary>
    public class GGUIWndMarsTechnologyTreeItem : _ANPGGUIBasicSubWnd<GGUIMonoMarsTechnologyTreeItem>
    {
        private long _m_lTechnologyId;
        private MarsTechnologyRefObj _m_rTechnologyRefObj;
        private MarsTechnologyInfo _m_iTechnologyInfo;
        private EMarsTechnologyState _m_eTechnologyState;
        
        private GGUIWndMarsTechnologyInfo _m_wTechnologyInfoWnd;


        public GGUIWndMarsTechnologyTreeItem(GGUIMonoMarsTechnologyTreeItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long technologyId { get { return _m_lTechnologyId; } }
        public MarsTechnologyRefObj technologyRefObj { get { return _m_rTechnologyRefObj; } }
        public MarsTechnologyInfo technologyInfo { get { return _m_iTechnologyInfo; } }
        public EMarsTechnologyState technologyState { get { return _m_eTechnologyState; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            // 构建科技信息子窗口
            if (wnd.monoTechnologyInfo != null)
                _m_wTechnologyInfoWnd = new GGUIWndMarsTechnologyInfo(wnd.monoTechnologyInfo);
            
            // 绑定点击按钮
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }


        protected override void _onDiscard()
        {
            // 销毁科技信息子窗口
            _m_wTechnologyInfoWnd?.discard();
            _m_wTechnologyInfoWnd = null;
            
            // 解绑按钮
            if (wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
            
            // 清空引用
            _m_lTechnologyId = 0;
            _m_rTechnologyRefObj = null;
            _m_iTechnologyInfo = null;
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }


        protected override void _onHideWnd()
        {
            // 隐藏子窗口
            _m_wTechnologyInfoWnd?.hideWnd();
        }


        protected override void _onReset()
        {
            _m_wTechnologyInfoWnd?.resetWnd();
        }


        /// <summary>
        /// 通过科技ID刷新窗口
        /// </summary>
        /// <param name="_technologyId">科技ID</param>
        public void setData(long _technologyId)
        {
            _m_lTechnologyId = _technologyId;
            _m_rTechnologyRefObj = GRefdataCoreMgr.instance.marsTechnologyRefCore.getRef(_m_lTechnologyId);
            _m_iTechnologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent?.getTechnologyInfoById(_m_lTechnologyId);
            _updateTechnologyState();
            
            refreshWnd();
        }

        /// <summary>
        /// 通过科技配表对象刷新窗口
        /// </summary>
        /// <param name="_technologyRefObj">科技配表对象</param>
        public void setData(MarsTechnologyRefObj _technologyRefObj)
        {
            _m_rTechnologyRefObj = _technologyRefObj;
            _m_lTechnologyId = _technologyRefObj?.id ?? 0;
            _m_iTechnologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent?.getTechnologyInfoById(_m_lTechnologyId);
            _updateTechnologyState();
            
            refreshWnd();
        }

        public void setData(MarsTechnologyInfo _technologyInfo)
        {
            _m_iTechnologyInfo = _technologyInfo;
            _m_lTechnologyId = _m_iTechnologyInfo?.technologyId ?? 0;
            _m_rTechnologyRefObj = _technologyInfo?.technologyRefObj;
            _updateTechnologyState();
            
            refreshWnd();
        }

        /// <summary>
        /// 更新科技状态
        /// </summary>
        private void _updateTechnologyState()
        {
            if(_m_iTechnologyInfo == null)
                _m_eTechnologyState = _m_rTechnologyRefObj?.technologyState ?? EMarsTechnologyState.NONE;
            else
                _m_eTechnologyState = MarsUtil.getTechnologyState(_m_iTechnologyInfo);
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow)
                return;
            
            // 刷新科技信息子窗口
            _refreshTechnologyInfo();
        }

        /// <summary>
        /// 刷新科技信息显示
        /// </summary>
        private void _refreshTechnologyInfo()
        {
            if (_m_wTechnologyInfoWnd == null)
                return;

            if (_m_iTechnologyInfo != null)
            {
                _m_wTechnologyInfoWnd.showWnd();
                _m_wTechnologyInfoWnd.setData(_m_iTechnologyInfo);
            }
            if (_m_rTechnologyRefObj != null)
            {
                _m_wTechnologyInfoWnd.showWnd();
                _m_wTechnologyInfoWnd.setData(_m_rTechnologyRefObj);
            }
            else if (_m_lTechnologyId != 0)
            {
                _m_wTechnologyInfoWnd.showWnd();
                _m_wTechnologyInfoWnd.setData(_m_lTechnologyId);
            }
        }

        /// <summary>
        /// 点击按钮回调
        /// </summary>
        /// <param name="_obj">点击的对象</param>
        private void _onBtnClick(GameObject _obj)
        {
            GGUIWndMasrTechnologyDetail.instance.setData(_m_rTechnologyRefObj);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndMasrTechnologyDetail.instance, () =>
            {
                GGUIWndMasrTechnologyDetail.instance.showWnd();   
            }, UINodeTagConst.C_MARS_TECHNOLOGY_DETAIL);
        }
    }
}
