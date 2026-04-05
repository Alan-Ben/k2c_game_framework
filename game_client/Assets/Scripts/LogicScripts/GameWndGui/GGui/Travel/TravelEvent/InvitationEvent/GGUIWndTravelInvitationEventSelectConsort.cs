using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子指定邀约事件处理窗口
    /// </summary>
    public class GGUIWndTravelInvitationEventSelectConsort : _ANPGGUIBasicWnd<GGUIMonoTravelInvitationEventSelectConsort>
    {
        private static GGUIWndTravelInvitationEventSelectConsort _g_instance;
        public static GGUIWndTravelInvitationEventSelectConsort instance { get { return _g_instance ??= new GGUIWndTravelInvitationEventSelectConsort(); } }

        [NotNull] private TravelInvitationEventInfo _m_iEventInfo;//事件信息
        
        private GGUIWndConsortCardItemContainer _m_wConsortCardItemContainer;//妃子卡片容器
        
        public GGUIWndTravelInvitationEventSelectConsort() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelInvitationEventSelectConsort.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelInvitationEventSelectConsort.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.consortCardItemContainer != null)
            {
                _m_wConsortCardItemContainer = new GGUIWndConsortCardItemContainer(wnd.consortCardItemContainer);
                _m_wConsortCardItemContainer.onSelectItemChg += _onSelectItemChg;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (_m_wConsortCardItemContainer != null)
            {
                _m_wConsortCardItemContainer.onSelectItemChg -= _onSelectItemChg;
                _m_wConsortCardItemContainer.discard();
            }
            _m_wConsortCardItemContainer = null;       
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        public void setData([NotNull] TravelInvitationEventInfo _eventInfo)
        {
            _m_iEventInfo = _eventInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            List<_IConsortShowInfo> consortList = new List<_IConsortShowInfo>();
            ConsortUtil.dealAllUnlockConsortList((_consortInfo) =>
            {
                if(_consortInfo != null)
                    consortList.Add(_consortInfo);
            });
            consortList.Sort(ConsortUtil.sortConsortShowInfoDefaultWithoutUnlockCheck);

            if (_m_wConsortCardItemContainer != null)
            {
                _m_wConsortCardItemContainer.showWnd();
                _m_wConsortCardItemContainer.showItemList(consortList);
            }
        }
        
        /// <summary>
        /// 当选中的妃子发生变化
        /// </summary>
        /// <param name="_item"></param>
        private void _onSelectItemChg(GGUIWndConsortCardItem _item)
        {
            if(_item == null)
                return;
            
            _m_iEventInfo.selectConsortInfo = _item.consortCardInfo;
        }
        
        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void _onSureBtnClick(GameObject _go)
        {
            if (_m_iEventInfo.selectConsortInfo == null)
            {
                // 提示未选择妃子
                if (wnd != null)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(wnd.noConsortSelectedTip);
                }
                return;
            }
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_INVITATION_EVENT_SELECT_CONSORT);
        }
    }
}