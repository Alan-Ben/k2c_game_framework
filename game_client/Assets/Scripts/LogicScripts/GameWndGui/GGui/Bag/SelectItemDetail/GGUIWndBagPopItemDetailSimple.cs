using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 背包弹窗:展示物品
    public class GGUIWndBagPopItemDetailSimple : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoBagPopItemDetailSimple>
    {
        // 物品数据
        private BagItem _m_iItem = null;


        public GGUIWndBagPopItemDetailSimple(Transform _parent)
            : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagPopItemDetailSimple.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagPopItemDetailSimple.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onDiscard()
        {
            _m_iItem = null;

        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {

        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {

        }

        // 初始化
        public void init(BagItem _item)
        {
            if(null == wnd)
                return;

            _m_iItem = _item;

            ALUGUICommon.setLabelTxt(wnd.itemName, _item.baseItemData.transName);

            ALUGUICommon.setLabelTxt(wnd.itemDesc, _item.baseItemData.transDesc);

        }
    }
}
