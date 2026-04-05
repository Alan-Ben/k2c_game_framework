using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndDinnerInvitePage<T>  :  _ATALBasicLoadPrefabSubUIWnd<T> where T: GGUIMonoDinnerInviteBasePage
    {
        //资源路径
        private string _m_sAssetPath;
        private string _m_sObjName;
        
        
        private GDinnerInfo _m_dinnerInfo;
        //数据列表
        [NotNull] protected List<DinnerInviteInfo> _m_litemDataList = new List<DinnerInviteInfo>();
        private int _m_tickSerialize;
        //列表窗口
        private GGUIWndDinnerInviteFriendItemGrid _m_itemGridWnd;

        public _AGGUIWndDinnerInvitePage(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }
    
        protected override string _monoAssetPath { get => _m_sAssetPath; }
        protected override string _monoObjName { get => _m_sObjName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            _onShowWndEx();
        }

        protected override void _onHideWnd()
        {
            _stopTickSec();
            _onHideWndEx();
        }

        protected override void _onReset()
        {
            _onResetEx();
        }

        protected override void _onDiscard()
        {
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
        
            if (wnd == null)
                return;

            _onDiscardEx();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.friendGrid)
                _m_itemGridWnd = new GGUIWndDinnerInviteFriendItemGrid (wnd.friendGrid);
            _onWndInitDoneEx();
        }
        
        public void setInfo(GDinnerInfo _dinnerInfo, List<DinnerInviteInfo> _itemDataList)
        {
            _m_dinnerInfo = _dinnerInfo;
            _m_litemDataList = _itemDataList;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        protected void _refreshWnd()
        {
            if(null == wnd)
                return; 
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_dinnerInfo, _m_litemDataList);
            _refreshWndEx();
            _startTickSec();
        }
              
        /// <summary>
        /// 开启每秒tick
        /// </summary>
        private void _startTickSec()
        {
            int tickSerialize = _m_tickSerialize;
            _refresnSec();
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(tickSerialize != _m_tickSerialize)
                    return;
                _startTickSec();
            }, 1f);
        }

        /// <summary>
        /// 停止每秒tick
        /// </summary>
        private void _stopTickSec()
        {
            _m_tickSerialize = ALSerializeOpMgr.next();
        }
        /// <summary>
        /// 每秒刷新
        /// </summary>
        protected  void _refresnSec()
        {
            _m_itemGridWnd?.refresnSec();
            _refresnSecEx();
        }
        
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        
        
        protected abstract void _refreshWndEx();
        
        protected abstract void _refresnSecEx();
    }
}