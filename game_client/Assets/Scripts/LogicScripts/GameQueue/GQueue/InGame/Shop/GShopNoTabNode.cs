using System;

namespace GOE
{
    /// <summary>
    /// 商店NoTab Node
    /// </summary>
    public class GShopNoTabNode : UIQueueBaseNode
    {
        private long _m_shopMainRefId;
        private long _m_shopRefId;
        //进入回调
        private Action _m_onEnter;

        public GShopNoTabNode(long _shopMainRefId = 0, long _shopId = 0, Action _onEnter = null) : base(EUIQueueStageType.MAIN, UINodeTagConst_Shop.C_MAIN_SHOP_NO_TAB_NODE)
        {
            _m_shopMainRefId = _shopMainRefId;
            _m_shopRefId = _shopId;
            _m_onEnter = _onEnter;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance);

            GGUIWndShopMain_NoTab.instance.regLoadDoneDelegate(() => {
                GGUIWndShopMain_NoTab.instance.showWnd();
                GGUIWndShopMain_NoTab.instance.setInfo(_m_shopMainRefId, _m_shopRefId);
            });

            _m_onEnter?.Invoke();
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GGUIWndShopMain_NoTab.instance.hideWnd();
        }

        public override void onEnterQueue()
        {
            GGUIWndShopMain_NoTab.instance.load();
        }

        public override void onClose()
        {
            GGUIWndShopMain_NoTab.instance.discard();

        }
    }
}
