namespace GOE
{
    /// <summary>
    /// 妃子招募窗口
    /// </summary>
    public class GNodeHeroRecruit : BaseQueueNode
    {
        private RecruitShopInfo _m_rRecruitShopInfo;
        private RecruitHeroItemInfo _m_iRecruitHeroInfo;

        public GNodeHeroRecruit(RecruitShopInfo _recruitShopInfo, RecruitHeroItemInfo _recruitHeroItemInfo) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_RECRUIT_HERO)
        {
            _m_rRecruitShopInfo = _recruitShopInfo;
            _m_iRecruitHeroInfo = _recruitHeroItemInfo;
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
        
        public override void onEnterQueue()
        {
        }

        public override void onClose()
        {
        }

        public override void EnterNode()
        {
            GUISceneMain.instance.showMainWnd(GGUIWndRecruitHero.instance, () =>
            {
                GGUIWndRecruitHero.instance.setData(_m_rRecruitShopInfo, _m_iRecruitHeroInfo);
            });
        }

        public override void QuitNode()
        {
        }
    }
}