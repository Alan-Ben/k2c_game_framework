using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 切换入口妃子窗口
    /// </summary>
    public class GGUIWndChangeEntranceConsort : _ANPGGUIBasicWnd<GGUIMonoChangeEntranceConsort>
    {
        private static GGUIWndChangeEntranceConsort _g_instance;
        public static GGUIWndChangeEntranceConsort instance { get { return _g_instance ??= new GGUIWndChangeEntranceConsort(); } }
        
        private GGUISubWndUnlockConsortDetailInfo _m_wSelectConsortDetailInfo;//选中妃子的详细信息
        private GGUIWndConsortIconItemContainer _m_wConsortHeadContainer;//妃子头像列表

        private List<_IConsortShowInfo> _m_lGottenConsortInfoList;//妃子列表
        private _IConsortShowInfo _m_SelectConsortInfo;//选中妃子信息
        
        public GGUIWndChangeEntranceConsort() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoChangeEntranceConsort.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChangeEntranceConsort.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSelectConsortDetailInfo != null)
                _m_wSelectConsortDetailInfo = new GGUISubWndUnlockConsortDetailInfo(wnd.monoSelectConsortDetailInfo);

            if (wnd.monoConsortHeadContainer != null)
            {
                _m_wConsortHeadContainer = new GGUIWndConsortIconItemContainer(wnd.monoConsortHeadContainer);
                _m_wConsortHeadContainer.onSelectItemChg += _onConsortItemClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_SelectConsortInfo = null;
            _m_lGottenConsortInfoList?.Clear();
            _m_lGottenConsortInfoList = null;
            
            _m_wSelectConsortDetailInfo?.discard();
            _m_wSelectConsortDetailInfo = null;

            if (_m_wConsortHeadContainer != null)
            {
                _m_wConsortHeadContainer.onSelectItemChg -= _onConsortItemClick;
                _m_wConsortHeadContainer.discard();
                _m_wConsortHeadContainer = null;       
            }
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX, _onSimulateSelectConsortByIndex);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_CONFIRM, _onSimulateClickConfirm);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX, _onSimulateSelectConsortByIndex);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_CONFIRM, _onSimulateClickConfirm);
            
            _m_SelectConsortInfo = null;
            _m_lGottenConsortInfoList?.Clear();

            _m_wSelectConsortDetailInfo?.hideWnd();
            _m_wConsortHeadContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_SelectConsortInfo = null;
            _m_lGottenConsortInfoList?.Clear();
            
            _m_wSelectConsortDetailInfo?.resetWnd();
            _m_wConsortHeadContainer?.resetWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            long selectConsortId = NPPlayer.instance.consortComp.remarkInfo?.getConsortEntranceShowConsortId() ?? 0;
            if (_m_lGottenConsortInfoList == null)
                _m_lGottenConsortInfoList = new List<_IConsortShowInfo>();
            _m_lGottenConsortInfoList.Clear();
            ConsortUtil.dealAllUnlockConsortList((_consortInfo) =>
            {
                if(_consortInfo != null)
                    _m_lGottenConsortInfoList.Add(_consortInfo);
            });
            _m_lGottenConsortInfoList.Sort(ConsortUtil.sortConsortShowInfoDefaultWithoutUnlockCheck);
            _m_SelectConsortInfo = null;
            
            if (_m_lGottenConsortInfoList.Count <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasConsortShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noConsortShow, true);
                
                _m_wSelectConsortDetailInfo?.hideWnd();
                _m_wConsortHeadContainer?.hideWnd();
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasConsortShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noConsortShow, true);

                _m_SelectConsortInfo = _m_lGottenConsortInfoList.Find((_consortInfo) =>
                {
                    return _consortInfo != null && _consortInfo.consortId == selectConsortId;
                });
                // 若没有选中妃子, 则默认选中第一个妃子
                if (_m_SelectConsortInfo == null)
                {
                    _m_SelectConsortInfo = _m_lGottenConsortInfoList[0];
                }

                if (_m_wConsortHeadContainer != null)
                {
                    _m_wConsortHeadContainer.showWnd();
                    _m_wConsortHeadContainer.showItemList(_m_lGottenConsortInfoList, _m_SelectConsortInfo?.consortId ?? 0);
                }

                if (_m_wSelectConsortDetailInfo != null)
                {
                    _m_wSelectConsortDetailInfo.showWnd();
                    _m_wSelectConsortDetailInfo.setData((GGottenConsortInfo)_m_SelectConsortInfo);
                }
            }
        }

        private void _onConsortItemClick(GGUIWndConsortIconItem _itemWnd)
        {
            if(_itemWnd == null)
                return;

            _m_SelectConsortInfo = _itemWnd.consortShowInfo;
            // 刷新选中妃子信息
            if (_m_wSelectConsortDetailInfo != null)
            {
                _m_wSelectConsortDetailInfo.showWnd();
                _m_wSelectConsortDetailInfo.setData((GGottenConsortInfo)_m_SelectConsortInfo);
            }
        }
        
        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            if(_m_SelectConsortInfo != null && NPPlayer.instance.consortComp.remarkInfo != null)
                NPPlayer.instance.consortComp.remarkInfo.setConsortEntranceShowConsortId(_m_SelectConsortInfo.consortId);
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHANGE_ENTRANCE_CONSORT);
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHANGE_ENTRANCE_CONSORT);
        }
        
        /// <summary>
        /// 模拟选中第几个妃子，item下标从0开始
        /// </summary>
        private void _onSimulateSelectConsortByIndex(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0 || _m_wConsortHeadContainer == null)
                return;

            int targetIndex = 0;
            if (_objs[0] is long l)
                targetIndex = (int)l;
            else if (_objs[0] is int i)
                targetIndex = i;

            if (_m_wConsortHeadContainer._m_lItemGroupList == null || targetIndex < 0 || targetIndex >= _m_wConsortHeadContainer._m_lItemGroupList.Count)
                return;

            GGUIWndConsortIconItem targetItem = _m_wConsortHeadContainer._m_lItemGroupList[targetIndex];
            if (targetItem != null)
                _m_wConsortHeadContainer.setSelectItem(targetItem);
        }

        /// <summary>
        /// 模拟点击确认按钮
        /// </summary>
        private void _onSimulateClickConfirm()
        {
            _onSureBtnClick(null);
        }
    }
}