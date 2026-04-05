using ALPackage;
using UnityEngine;

namespace GOE
{
    // 背包弹窗:兑换物品
    public class GGUIWndBagPopItemConvertSimple : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoBagPopItemConvertSimple>
    {
        // 物品数据
        private BagItem _m_iItem = null;

        public GGUIWndBagPopItemConvertSimple(Transform _parent)
            : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagPopItemConvertSimple.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagPopItemConvertSimple.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnCombine, _onclickCombine);
            ALUGUICommon.combineBtnClick(wnd.btnNotEnough, _onclickCombine);
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
        protected override void _onDiscard()
        {
            _m_iItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnCombine, _onclickCombine);
            ALUGUICommon.uncombineBtnClick(wnd.btnNotEnough, _onclickCombine);
        }

        // 初始化
        public void init(BagItem _item)
        {
            if (null == wnd || null == _item || null == _item.baseItemData)
                return;

            _m_iItem = _item;
            //物品名字
            ALUGUICommon.setLabelTxt(wnd.itemName, _item.baseItemData.transName);
            //物品描述
            ALUGUICommon.setLabelTxt(wnd.itemDesc, _item.baseItemData.transDesc);

            //是否可合成，不可合成置灰
            bool canCombine = GCommon.isItemCanCombine(_m_iItem.itemId, 1);
            if (canCombine)
                GGameCommonInfo.disgrayImage(wnd.notEnoughGrayList);
            else
                GGameCommonInfo.grayImage(wnd.notEnoughGrayList);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughShowList, !canCombine);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughHideList, canCombine);
        }

        //点击合成按钮
        private void _onclickCombine(GameObject _go)
        {
            if (null == _m_iItem)
                return;

            //如果物品为空时提示
            if (_m_iItem.count == 0)
            {
                //使用key
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_notSelect_tip);
                return;
            }

            //打开合成窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemBeCombined.instance, () =>
            {
                GGUIWndBagItemBeCombined.instance.showWnd();
                GGUIWndBagItemBeCombined.instance.initOri(_m_iItem.itemId);
            }, UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
        }
        
    }
}
