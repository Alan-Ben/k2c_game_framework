using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - ai智能控制页面窗口
    /// </summary>
    public class GGUIWndMarsIntelligentControl : _ANPGGUIBasicWnd<GGUIMonoMarsIntelligentControl>
    {
        private static GGUIWndMarsIntelligentControl _g_instance;
        public static GGUIWndMarsIntelligentControl instance { get { return _g_instance ??= new GGUIWndMarsIntelligentControl(); } }
        
        private GGUIWndCommonSimpleItem _m_wSatisfactionValueItem; // 满意值item窗口
        private GGUIWndMarsIntelligentControlContainer _m_wIntelligentControlContainer; // 智能控制列表容器

        private List<_IMarsIntelligentControlInfo> _m_lTmpIntelligentControlInfoList; // 智能控制信息列表
        
        public GGUIWndMarsIntelligentControl() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsIntelligentControl.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsIntelligentControl.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化满意值item窗口
            if (wnd.satisfactionValueItem != null)
                _m_wSatisfactionValueItem = new GGUIWndCommonSimpleItem(wnd.satisfactionValueItem);

            // 初始化智能控制列表容器
            if (wnd.intelligentControlContainer != null)
                _m_wIntelligentControlContainer = new GGUIWndMarsIntelligentControlContainer(wnd.intelligentControlContainer);

            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnAddSatisfactionValue, _onClickAddSatisfactionValue);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }

        protected override void _onDiscard()
        {
            // 销毁满意值item窗口
            _m_wSatisfactionValueItem?.discard();
            _m_wSatisfactionValueItem = null;

            // 销毁智能控制列表容器
            _m_wIntelligentControlContainer?.discard();
            _m_wIntelligentControlContainer = null;

            _m_lTmpIntelligentControlInfoList?.Clear();
            _m_lTmpIntelligentControlInfoList = null;
            
            // 解除按钮点击事件绑定
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnAddSatisfactionValue, _onClickAddSatisfactionValue);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }
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
            _m_wSatisfactionValueItem?.hideWnd();
            _m_wIntelligentControlContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wSatisfactionValueItem?.resetWnd();
            _m_wIntelligentControlContainer?.resetWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            _refreshIntelligentControlList();
            _refreshSatisfactionValueItem();
        }

        private void _refreshIntelligentControlList()
        {
            if (wnd == null || !isShow)
                return;

            if (_m_lTmpIntelligentControlInfoList == null)
                _m_lTmpIntelligentControlInfoList = new List<_IMarsIntelligentControlInfo>();
            _m_lTmpIntelligentControlInfoList.Clear();
            _IMarsIntelligentControlInfo intelligentControlInfo = null;
            foreach (var intelligentControlRefObj in GRefdataCoreMgr.instance.marsIntelligentControlRefCore.refList)
            {
                if(intelligentControlRefObj == null)
                    continue;

                intelligentControlInfo = NPPlayer.instance.marsComp.peopleSubComponent.getIntelligentControlInfo(intelligentControlRefObj.id);
                if(intelligentControlInfo != null) 
                    _m_lTmpIntelligentControlInfoList.Add(intelligentControlInfo);
            }
            
            if (_m_wIntelligentControlContainer != null)
            {
                _m_wIntelligentControlContainer.showWnd();
                _m_wIntelligentControlContainer.setData(_m_lTmpIntelligentControlInfoList);
            }
        }
        
        /// <summary>
        /// 刷新满意值item显示
        /// </summary>
        private void _refreshSatisfactionValueItem()
        {
            if (wnd == null || !isShow)
                return;

            if (_m_wSatisfactionValueItem != null)
            {
                _m_wSatisfactionValueItem.showWnd();
                _m_wSatisfactionValueItem.setItem(GRefdataCoreMgr.instance.npGeneral.mars_satisfaction_value_common_item, true, EValueFormatType.NORMAL);
            }
        }

        /// <summary>
        /// 点击增加满意值按钮
        /// </summary>
        /// <param name="_go">按钮GameObject</param>
        private void _onClickAddSatisfactionValue(GameObject _go)
        {
            NPCommonItem statisfactionValueItem = GRefdataCoreMgr.instance.npGeneral.mars_satisfaction_value_common_item;
            if(statisfactionValueItem == null)
                return;

            GCommon.popItemAccessWays(statisfactionValueItem.itemType, statisfactionValueItem.itemId);
        }
        
        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go">按钮GameObject</param>
        private void _onClickReturn(GameObject _go)
        {
            // 关闭当前窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_INTELLIGENT_CONTROL);
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
            
            NPCommonItem statisfactionValueItem = GRefdataCoreMgr.instance.npGeneral.mars_satisfaction_value_common_item;
            if(statisfactionValueItem == null || statisfactionValueItem.itemType != itemType || statisfactionValueItem.itemId != subId)
                return;
            
            _refreshSatisfactionValueItem();
        }

        #endregion
    }
}