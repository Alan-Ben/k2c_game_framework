using System;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 游戏主页面Node
    /// </summary>
    public class GNodeTreasureHuntGameMain : BaseQueueNode
    {
        [NotNull] private readonly TreasureHuntGameLogic _m_gameLogic;
        
        private int _m_enterSerialize = 0; 
        
        
        private GNodeTreasureHuntGameMain() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_GAME_MAIN)
        {
            _m_gameLogic = new TreasureHuntGameLogic();
            
            TreasureHuntUtil.refreshInArea();
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
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }
        
        public override void onEnterQueue()
        {
        }

        public override void onClose()
        {
        }
        
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            int serialize = _m_enterSerialize;
            GTDSceneMain.instance.showMainScene(MainAdditionTreasureHuntGameTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;

                GUISceneMain.instance.showMainScene(GMainGUIAddSceneTreasureHuntGame.instance, () => _m_gameLogic.start(_triggerEnterDone, null));
            });
        }

        public override void EnterNode()
        {
        }

        public override void QuitNode()
        {
            _m_gameLogic.stop();
        }

        #region 添加Node方法

        public static void addNode()
        {
            if ((QueueMgr.instance.findLastNode(typeof(GNodeTreasureHuntGameMain)) is GNodeTreasureHuntGameMain treasureHuntGameMainNode))
            {
                QueueMgr.instance.QuitUntilCanStop((_node) => _node == treasureHuntGameMainNode);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeTreasureHuntGameMain());
            }
        }
        
        public static void addNode(long _areaId)
        {
            NPPlayer.instance.treasureHuntComponent.saver?.setAreaId(_areaId);
            addNode();
        }

        public static void addNode(TreasureHuntAreaRefObj _areaRefObj)
        {
            if (_areaRefObj != null && _areaRefObj.isUnlock())
            {
                NPPlayer.instance.treasureHuntComponent.saver?.setAreaId(_areaRefObj.area_id);
            }
            addNode();
        }
        
        #endregion
    }
}