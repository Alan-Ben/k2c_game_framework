using ALPackage;

namespace GOE
{
    // 使用物品弹窗- 展示实际获取的物品
    public class GGUIWndBagItemUseShowRealGainItem : _AGGUIWndBagItemUse<GGUIMonoBagItemUseShowRealGainItem>
    {
        private static GGUIWndBagItemUseShowRealGainItem _g_instance = new GGUIWndBagItemUseShowRealGainItem();
        public static GGUIWndBagItemUseShowRealGainItem instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemUseShowRealGainItem();

                return _g_instance;
            }
        }

        //实际获得物品的图片
        private NPGGuiWndTexture _m_realGainItemWnd;

        public GGUIWndBagItemUseShowRealGainItem()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _m_nodeTag { get { return UINodeTagConst.C_ADD_Bag_UseShowRealGainItemNode; } }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemUseShowRealGainItem.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemUseShowRealGainItem.objName; } }

        protected override void _onDiscard()
        {
            base._onDiscard();
            if (null != _m_realGainItemWnd)
                _m_realGainItemWnd.discard();
            _m_realGainItemWnd = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (null != wnd.realGainItemImg)
                _m_realGainItemWnd = new NPGGuiWndTexture(wnd.realGainItemImg);
        }

        protected override void _initEx()
        {
            if (null != _m_iItem.itemUseRefObj.real_gain_Item_Img_Idx)
                _m_realGainItemWnd.setTexture(_m_iItem.itemUseRefObj.real_gain_Item_Img_Idx);

            ALUGUICommon.setLabelTxt(wnd.realGainItemNumTxt, _m_iItem.itemUseRefObj.real_gain_item_count.CalculateVariableResult(null).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }

        protected override void _onCounterChangedEx(long _newCount)
        {
            if (null != _m_iItem && null != _m_iItem.itemUseRefObj)
                ALUGUICommon.setLabelTxt(wnd.realGainItemNumTxt, (_m_iItem.itemUseRefObj.real_gain_item_count.CalculateVariableResult(null) * _newCount).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }
    }
}
