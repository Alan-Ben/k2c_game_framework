using System;
using ALPackage;

namespace GOE
{
    public class GNodeBag : _AGNodeMainSub
    {
        private bool _m_bSelectDefaultTab;//是否选中默认页签
        private EBagMainTabMonoType _m_eSelectTabType;//(在_m_bSelectDefaultTab为false时)指定选中的页签类型
        
        public GNodeBag(bool _selectDefaultTab = true, EBagMainTabMonoType _selectTabType = EBagMainTabMonoType.BAG_ITEM)
            : base(EMainFunctionTabType.BAG, UINodeTagConst.C_Main_BagNode)
        {
            _m_bSelectDefaultTab = _selectDefaultTab;
            _m_eSelectTabType = _selectTabType;
        }

        public override bool isOnlyUINode => true;

        public override void onEnterQueue()
        {
        }
        public override void onClose()
        {
        }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            NPGMainGUIAddSceneBag.instance.setData(_m_bSelectDefaultTab, _m_eSelectTabType);
            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneBag.instance, _triggerEnterDone);
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        protected override void _doQuitNode()
        {
        }
    }
}