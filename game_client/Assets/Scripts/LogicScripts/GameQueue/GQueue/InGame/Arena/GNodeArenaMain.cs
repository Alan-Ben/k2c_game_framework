using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 竞技场主界面节点
    /// </summary>
    public class GNodeArenaMain : BaseQueueNode
    {
        /// <summary>
        /// 进入完成之后调用的函数
        /// </summary>
        private Action _m_dOnEnterDone;

        public GNodeArenaMain() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_ARENA_MAIN)
        {
            _m_dOnEnterDone = () =>
            {
                //进入竞技场主界面时，检查是否正在战斗中，如果正在战斗中则直接进入战斗界面
                GGUIWndArenaMain.instance.checkIsInBattle();
            };
        }
        
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
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
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            // 退出竞技场主界面时，清除竞技场名人堂机器人数据
            NPPlayer.instance.arenaComp.clearCelebrityBot();
        }
        
        public override void EnterNode()
        {
            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(
                () =>
                {
                    GUISceneMain.instance.showMainScene(GMainGUIMainSceneArenaMain.instance, _onEnterDone);
                });
        }

        public override void QuitNode()
        {
        }


        /// <summary>
        /// 切换进入视图时完成的处理函数
        /// </summary>
        protected void _onEnterDone()
        {
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
            _m_dOnEnterDone = null;
        }
    }
}