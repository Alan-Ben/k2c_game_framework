using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    // 一键合成弹窗
    public class GGUIWndBagItemOnceCombine : _ANPGGUIBasicWnd<GGUIMonoBagItemOnceCombine>
    {
        private static GGUIWndBagItemOnceCombine _g_instance = new GGUIWndBagItemOnceCombine();
        public static GGUIWndBagItemOnceCombine instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemOnceCombine();

                return _g_instance;
            }
        }

        //一键合成数据
        private List<BagOnceCombineItem> _m_onceCombineData;

        //目标物品列表
        private NPGGUIWndCommonItemContainer _m_wTargetItemContainer;
        //合成原料物品列表
        private NPGGUIWndCommonItemContainer _m_wOriItemContainer;
        //合成资源物品列表
        private NPGGUIWndCommonItemContainer _m_wResItemContainer;

        public GGUIWndBagItemOnceCombine()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemOnceCombine.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemOnceCombine.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onDiscard()
        {

            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.discard();
            _m_wTargetItemContainer = null;

            if (_m_wOriItemContainer != null)
                _m_wOriItemContainer.discard();
            _m_wOriItemContainer = null;

            if (_m_wResItemContainer != null)
                _m_wResItemContainer.discard();
            _m_wResItemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCombine, _onClickCombine);
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.resetWnd();
            if (_m_wOriItemContainer != null)
                _m_wOriItemContainer.resetWnd();
            if (_m_wResItemContainer != null)
                _m_wResItemContainer.resetWnd();
        }

        protected override void _onShowWnd()
        {
            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.showWnd();
            if (_m_wOriItemContainer != null)
                _m_wOriItemContainer.showWnd();
            if (_m_wResItemContainer != null)
                _m_wResItemContainer.showWnd();

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTargetItemContainer != null)
                _m_wTargetItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoTargetItemContainer);

            if (wnd.monoOriItemContainer != null)
                _m_wOriItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoOriItemContainer);

            if (wnd.monoResItemContainer != null)
                _m_wResItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoResItemContainer);

            //绑定按钮
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnCombine, _onClickCombine);
        }

        // 初始化
        public void setData(List<BagOnceCombineItem> _onceCombineList)
        {
            if (null == wnd || null == _onceCombineList)
                return;

            _m_onceCombineData = _onceCombineList;
            List<NPCommonCostItem> targetList = new List<NPCommonCostItem>();
            List<NPCommonCostItem> resList = new List<NPCommonCostItem>();
            List<NPCommonCostItem> oriList = new List<NPCommonCostItem>();
            BagOnceCombineItem temp = null;
            for (int i = 0; i < _onceCombineList.Count; i++)
            {
                temp = _onceCombineList[i];
                if (null == temp)
                    continue;

                targetList.Add(temp.targetItem);
                resList.AddRange(temp.resCostItemList);
                oriList.AddRange(temp.oriCostItemList);
            }

            //合并resList
            List<NPCommonCostItem> resListTemp = GCommon.getCombineItemList(resList);

            //目标列表
            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.showItemList(targetList);

            //资源列表
            if (_m_wResItemContainer != null)
                _m_wResItemContainer.showItemList(resListTemp);

            //原材料列表
            if (_m_wOriItemContainer != null)
                _m_wOriItemContainer.showItemList(oriList);
        }

        #region 点击事件

        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_ONCE_COMBINE);
        }

        //点击合成按钮
        private void _onClickCombine(GameObject _go)
        {
            if (_m_onceCombineData == null)
                return;

            NPPlayer.instance.bagComp.reqItemOnceCombine(_m_onceCombineData);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_ONCE_COMBINE);

        }

        #endregion

    }
}
