using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - ai智能控制详情页面窗口
    /// </summary>
    public class GGUIWndMarsIntelligentControlDetail : _ANPGGUIBasicWnd<GGUIMonoMarsIntelligentControlDetail>
    {
        private static GGUIWndMarsIntelligentControlDetail _g_instance;
        public static GGUIWndMarsIntelligentControlDetail instance { get { return _g_instance ??= new GGUIWndMarsIntelligentControlDetail(); } }

        private _IMarsIntelligentControlInfo _m_iInfo; // 智能控制信息
        private NPCommonCostItem _m_iCostItem;
        
        private NPGGUIWndCommonItem _m_wCostItem; // 消耗道具窗口
        private GGUIWndMarsIntelligentControlInfo _m_wIntelligentControlInfo; // 智能控制决策窗口

        public GGUIWndMarsIntelligentControlDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsIntelligentControlDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsIntelligentControlDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化消耗道具窗口
            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);
                
            // 初始化智能控制项窗口
            if (wnd.intelligentControlInfo != null)
                _m_wIntelligentControlInfo = new GGUIWndMarsIntelligentControlInfo(wnd.intelligentControlInfo);

            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }

        protected override void _onDiscard()
        {
            // 解除按钮点击事件绑定
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onClickUse);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }
            
            // 销毁消耗道具窗口
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            
            // 销毁智能控制项窗口
            _m_wIntelligentControlInfo?.discard();
            _m_wIntelligentControlInfo = null;

            _m_iInfo = null;
            _m_iCostItem = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
            
            // 隐藏子窗口
            _m_wCostItem?.hideWnd();
            _m_wIntelligentControlInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wCostItem?.resetWnd();
            _m_wIntelligentControlInfo?.resetWnd();
        }

        /// <summary>
        /// 设置智能控制信息
        /// </summary>
        /// <param name="info">智能控制信息</param>
        public void setInfo(_IMarsIntelligentControlInfo info)
        {
            _m_iInfo = info;
            if (_m_iCostItem == null)
                _m_iCostItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.mars_satisfaction_value_common_item, _m_iInfo?.refObj?.cost_satisfaction_value ?? 0);
            else
                _m_iCostItem.setCount(_m_iInfo?.refObj?.cost_satisfaction_value ?? 0);
            
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iInfo == null)
                return;

            MarsIntelligentControlRefObj refObj = _m_iInfo.refObj;
            if (refObj == null)
                return;

            _refreshCostItemShow();
            
            // 设置智能控制项
            if (_m_wIntelligentControlInfo != null)
            {
                _m_wIntelligentControlInfo.showWnd();
                _m_wIntelligentControlInfo.setInfo(_m_iInfo);
            }
        }

        private void _refreshCostItemShow()
        {
            // 设置消耗道具
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(_m_iCostItem);
            }
        }
        
        /// <summary>
        /// 点击使用按钮
        /// </summary>
        /// <param name="btnGo">按钮GameObject</param>
        private void _onClickUse(GameObject btnGo)
        {
            if (_m_iInfo == null)
                return;

            if (_m_iInfo.getState(true) == EMarsIntelligentControlState.CAN_USE)
            {
                NPPlayer.instance.marsComp.peopleSubComponent.reqDealIntelligent(_m_iInfo.id, (_isSucc, _msg) =>
                {
                    _refreshWnd();

                    // 若使用成功
                    if (_isSucc && _m_iInfo != null && _m_iInfo.refObj != null && _m_iInfo.refObj.client_show_effect_building_id > 0)
                    {
                        _IMarsIntelligentControlInfo intelligentControlInfo = _m_iInfo;
                        GNodeMars marsNode = QueueMgr.instance.findLastNode(typeof(GNodeMars)) as GNodeMars;
                        if (marsNode != null)
                        {
                            float afterUseFocusBuildingTime = wnd == null ? 0.1f : wnd.afterUseFocusBuildingTime;
                            QueueMgr.instance.QuitUntilCanStop(_node => _node == marsNode);
                            WinMsg.SendMsg(WinMsgType.TRIGGER_MARS_BUILDING_FOCUS, intelligentControlInfo.refObj.client_show_effect_building_id, afterUseFocusBuildingTime);//触发聚焦
                            WinMsg.SendMsg(WinMsgType.TRIGGER_MARS_BUILDING_INTELLIGENT_CONTROL_EFFECT, intelligentControlInfo.refObj.client_show_effect_building_id, intelligentControlInfo.id);//触发特效显示
                        }
                    }
                });       
            }
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go">按钮GameObject</param>
        private void _onClickReturn(GameObject _go)
        {
            // 关闭当前窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_INTELLIGENT_CONTROL_DETAIL);
        }
        
        #region 窗口消息监听

        /// <summary>
        /// commonItem数量变化消息
        /// </summary>
        /// <param name="_objs"></param>
        private void _onCommonItemCountChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 3 || !(_objs[0] is ENPItemType itemType) || 
                !(_objs[1] is long subId) || !(_objs[2] is long count))
                return;
            
            if(_m_iCostItem == null || _m_iCostItem.getItemType() != itemType || _m_iCostItem.subId != subId)
                return;
         
            _refreshCostItemShow();
        }

        #endregion

        public static void addNode(_IMarsIntelligentControlInfo _controlInfo)
        {
            if(_controlInfo == null)
                return;
            
            instance.setInfo(_controlInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(instance, () =>
            {
                instance.showWnd();
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_INTELLIGENT_CONTROL_DETAIL, false, false);
        }
    }
}