using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 物品概率详情界面
    /// </summary>
    public class GGUIWndItemPercentDetail : _ATALBasicUIWnd<GGUIMonoItemPercentDetail>
    {
        private static GGUIWndItemPercentDetail _g_instance = new GGUIWndItemPercentDetail();
    
        public static GGUIWndItemPercentDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndItemPercentDetail();
                return _g_instance;
            }
        }
        
        private GGUIWndItemPercentDetailGrid _m_wItemGridWnd;
        private List<NPCommonCostItem> _m_lIitemDataList = new List<NPCommonCostItem>();
        private List<int> _m_lIitemWeightList = new List<int>();// 物品权重列表
        private int _m_iTotalWeight = 0;// 总权重

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndItemPercentDetail() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoItemPercentDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoItemPercentDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_wItemGridWnd?.discard();
            _m_wItemGridWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_wItemGridWnd = new GGUIWndItemPercentDetailGrid(wnd.itemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }

        public void setInfo(List<NPCommonCostItem> _itemDataList,  List<int> _itemWeightList)
        {
            _m_lIitemDataList = _itemDataList;
            _m_lIitemWeightList = _itemWeightList;
            _m_iTotalWeight = 0;
            if (_m_lIitemWeightList != null)
            {
                for (int i = 0; i < _m_lIitemWeightList.Count; i++)
                {
                    _m_iTotalWeight += _m_lIitemWeightList[i];
                }
            }
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _m_wItemGridWnd?.showWnd();
            _m_wItemGridWnd?.showItemList(_m_lIitemDataList, _m_lIitemWeightList, _m_iTotalWeight);
        }
        
        // <AutoGen:Method>
        
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ITEM_PERCENT_DETAIL);
        }
        // </AutoGen:Method>
    }
}