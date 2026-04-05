using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    // 背包弹窗:使用物品
    public class GGUIWndBagItemUse : _AGGUIWndBagItemUse<GGUIMonoBagItemUse>
    {
        private static GGUIWndBagItemUse _g_instance = new GGUIWndBagItemUse();
        public static GGUIWndBagItemUse instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemUse();

                return _g_instance;
            }
        }

        protected override string _m_nodeTag { get { return UINodeTagConst.C_ADD_Bag_UseItemNode; } }

        public GGUIWndBagItemUse()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemUse.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemUse.objName; } }

        protected override void _initEx()
        {

        }

        protected override void _onCounterChangedEx(long _newCount)
        {

        }
    }
}
