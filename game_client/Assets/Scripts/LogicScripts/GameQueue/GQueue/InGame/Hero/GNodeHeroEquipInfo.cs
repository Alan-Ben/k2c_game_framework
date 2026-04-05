using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴当前佩戴藏品展示界面节点
    /// </summary>
    public class GNodeHeroEquipInfo : UIQueueBaseNode
    {
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //操作序列号
        private long _m_lSerialize;
        public GNodeHeroEquipInfo(HeroInfo _heroInfo) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_HERO_EQUIP_INFO)
        {
            _m_heroInfo = _heroInfo;
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
        public override bool IsMainViewNode { get { return false; } }
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
            if (null != _m_wTransBk)
            {
                _m_wTransBk.showWnd();
                _afterTransBkAction();
            }
            else
            {
                _m_lSerialize = ALSerializeOpMgr.next();
                long serialize = _m_lSerialize;
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    GGUIWndHeroEquipInfo.instance,
                    () =>
                    {
                        QueueMgr.instance.forceCloseNode(this);
                    },
                    () =>
                    {
                        if (serialize != _m_lSerialize)
                            return;

                        _afterTransBkAction();
                    });
            }
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;

            //隐藏窗口
            GGUIWndHeroEquipInfo.instance.hideWnd();
        }

        public override void onEnterQueue()
        {
            GGUIWndHeroEquipInfo.instance.load();
        }

        public override void onClose()
        {
            //释放窗口
            GGUIWndHeroEquipInfo.instance.discard();
        }
        
        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            GGUIWndHeroEquipInfo.instance.regLoadDoneDelegate(() =>
            {
                //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if (_m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), GGUIWndHeroEquipInfo.instance.getGameObj());
                else
                    //将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndHeroEquipInfo.instance.getGameObj());

                GGUIWndHeroEquipInfo.instance.showWnd();
                GGUIWndHeroEquipInfo.instance.setInfo(_m_heroInfo);
            });
        }
    }
}
