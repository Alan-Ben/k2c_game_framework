using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟申请界面
    /// </summary>
    public class GNodeGuildApply : UIQueueBaseNode
    {
        public GNodeGuildApply() : base(EUIQueueStageType.MAIN, UINodeTagConst_Guild.C_GUILD_APPLY)
        {
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
        public override bool isOnlyUINode { get { return false; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            // 打开界面
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneGuildApply.instance);
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
        }

        public override void onEnterQueue()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);//加入联盟消息
        }

        public override void onClose()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);//加入联盟消息
        }

        //加入联盟的推送处理
        private void _onJoinGuild(params object[] _objects)
        {
            if (_objects == null || _objects[0] == null)
                return;

            //是否是创建
            bool isCreate = (bool) _objects[0];

            if (isCreate)
            {
                //创建联盟成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_createGuildSucceed_none);
            }
            else
            {
                //加入联盟成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_reqJoinGuildSuccTip_none);
            }

            //关闭窗口，打开联盟主界面
            QueueMgr.instance.QuitUntilCanStop(_node => _node is GNodeSpaceStation);
            GCommon.enterUIMainNodeShow(ESysSceneType.GUILD);
        }
    }
}
