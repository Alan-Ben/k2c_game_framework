using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟主界面节点
    /// </summary>
    public class GNodeGuildMain : BaseQueueNode
    {
        public GNodeGuildMain() : base(EUIQueueStageType.MAIN, UINodeTagConst_Guild.C_GUILD_MAIN)
        {
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
            WinMsg.RegisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);//退出联盟消息
        }
        
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);//退出联盟消息
        }
        
        public override void EnterNode()
        {
            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(
                () =>
                {
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(2);
                    stepCounter.regAllDoneDelegate(_onAllSceneInited);

                    GTDSceneMain.instance.showMainScene(MainAdditionGuildTDScene.instance, stepCounter.addDoneStepCount);
                    GUISceneMain.instance.showMainScene(GMainGUIAddSceneGuildMain.instance, stepCounter.addDoneStepCount);
                });
        }

        public override void QuitNode()
        {
        }

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
        }

        //退出联盟推送的处理
        private void _onLeaveGuild(params object[] _objects)
        {
            if (_objects == null || _objects[0] == null)
                return;

            //是否被踢出联盟
            bool isKick = (bool) _objects[0];

            if (isKick)
            {
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_leaveGuildTip_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        //关闭窗口，打开联盟申请界面
                        QueueMgr.instance.QuitUntilCanStop(_node=>_node is GNodeSpaceStation);
                        GCommon.enterUIMainNodeShow(ESysSceneType.GUILD);
                    });
            }
            else
            {
                //主动退出联盟，关闭窗口，打开联盟申请界面
                QueueMgr.instance.QuitUntilCanStop(_node => _node is GNodeSpaceStation);
                GCommon.enterUIMainNodeShow(ESysSceneType.GUILD);
            }
        }
    }
}