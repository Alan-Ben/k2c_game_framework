using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴详情界面节点
    /// </summary>
    public class GMainQueueHeroInfoNode : UIQueueBaseNode
    {
        private HeroCardShowInfo _m_heroShowInfo;
        private List<HeroCardShowInfo> _m_lHeroShowInfoList;
        private bool _m_bShowEnterAni = false;

        public GMainQueueHeroInfoNode(HeroCardShowInfo _heroShowInfo, List<HeroCardShowInfo> _heroShowInfoList, bool _showEnterAni = true) : base(EUIQueueStageType.MAIN,UINodeTagConst.C_HERO_INFO)
        {
            _m_heroShowInfo = _heroShowInfo;
            _m_lHeroShowInfoList = new List<HeroCardShowInfo>();
            if(_heroShowInfoList != null)
                _m_lHeroShowInfoList.AddRange(_heroShowInfoList);
            _m_bShowEnterAni = _showEnterAni;
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
            GMainGUIAddSceneHeroInfo.instance.regInitDelegate(
                () =>
                {
                    GMainGUIAddSceneHeroInfo.instance.setInfo(_m_heroShowInfo, _m_lHeroShowInfoList, _m_bShowEnterAni);
                    GUISceneMain.instance.showMainScene(GMainGUIAddSceneHeroInfo.instance);
                });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            //记录一下当前展示的骑士数据
            _m_heroShowInfo = GGUIWndHeroInfo.instance.curHeroCardShowInfo;
            GMainGUIAddSceneHeroInfo.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GMainGUIAddSceneHeroInfo.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GMainGUIAddSceneHeroInfo.instance.quitScene();
        }
    }
}
