using System;
using System.Collections.Generic;

namespace GOE
{
    public class GNodeTreasureHuntLab : BaseQueueNode
    {
        private Dictionary<TreasureHuntLabRefObj, MainAdditionTreasureHuntLabTDScene> _m_dLabRefObjToTdSceneDic;
        private MainAdditionTreasureHuntLabTDScene _m_curTdScene;

        private GNodeTreasureHuntLab(TreasureHuntLabRefObj _labRefObj, TreasureHuntGotTreasureInfo _putInTreasureInfo, long _selectedTreasureId) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_LAB)
        {
            if (_labRefObj == null)
                _labRefObj = GRefdataCoreMgr.instance.getFirstUnlockTreasureHuntLabRefObj();
            if (_labRefObj == null)
                _labRefObj = GRefdataCoreMgr.instance.treasureHuntLabRefCore?.refList?.GetFirst();
            
            _m_curTdScene = _getTdScene(_labRefObj);
            GMainGUIAddSceneTreasureHuntLab.instance.setShowLab(_labRefObj, _putInTreasureInfo, _selectedTreasureId);
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
            GMainGUIAddSceneTreasureHuntLab.instance.enterScene();
            _m_curTdScene?.enterScene();
        }

        public override void onClose()
        {
            GMainGUIAddSceneTreasureHuntLab.instance.quitScene();

            dealAllTdScene((_labRefObj, _tdScene) =>
            {
                _tdScene?.quitScene();
            });
            _m_dLabRefObjToTdSceneDic?.Clear();
            _m_dLabRefObjToTdSceneDic = null;
            _m_curTdScene = null;
        }
        
        public override void EnterNode()
        {
            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance);
            GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);
            
            if (_m_curTdScene != null)
            {
                _m_curTdScene.regInitDelegate(() =>
                {
                    _m_curTdScene?.showScene();
                    
                    GMainGUIAddSceneTreasureHuntLab.instance.regEnterDelegate(() =>
                    {
                        GMainGUIAddSceneTreasureHuntLab.instance.showScene();
                    });
                });
            }
        }

        public override void QuitNode()
        {
            GMainGUIAddSceneTreasureHuntLab.instance.hideScene();
            _m_curTdScene?.hideScene();
        }

        /// <summary>
        /// 切换实验室
        /// </summary>
        private void _chgLab(TreasureHuntLabRefObj _labRefObj, TreasureHuntGotTreasureInfo _canPutInTreasureInfo, long _selectedTreasureId)
        {
            if(_labRefObj == null)
                return;

            MainAdditionTreasureHuntLabTDScene perTdScene = _m_curTdScene;
            _m_curTdScene = _getTdScene(_labRefObj);
            if(perTdScene == _m_curTdScene)
                return;

            if (_m_curTdScene != null)
            {
                if(!_m_curTdScene.isEntered)
                    _m_curTdScene.enterScene();
                
                _m_curTdScene.regInitDelegate(() =>
                {
                    perTdScene?.hideScene();
                    _m_curTdScene.showScene();
                    
                    GMainGUIAddSceneTreasureHuntLab.instance.setShowLab(_labRefObj, _canPutInTreasureInfo, _selectedTreasureId);
                });
            }
        }
        
        private void _chgLab(long _labId, TreasureHuntGotTreasureInfo _canPutInTreasureInfo, long _selectedTreasureId)
        {
            TreasureHuntLabRefObj labRefObj = GRefdataCoreMgr.instance.treasureHuntLabRefCore.getRef(_labId);
            _chgLab(labRefObj, _canPutInTreasureInfo, _selectedTreasureId);
        }
        
        private MainAdditionTreasureHuntLabTDScene _getTdScene(TreasureHuntLabRefObj _labRefObj)
        {
            if (_labRefObj == null)
                return null;
            
            if (_m_dLabRefObjToTdSceneDic == null)
                _m_dLabRefObjToTdSceneDic = new Dictionary<TreasureHuntLabRefObj, MainAdditionTreasureHuntLabTDScene>();

            MainAdditionTreasureHuntLabTDScene tdScene = null;
            if (!_m_dLabRefObjToTdSceneDic.TryGetValue(_labRefObj, out tdScene) || tdScene == null)
            {
                tdScene = new MainAdditionTreasureHuntLabTDScene(_labRefObj);
                _m_dLabRefObjToTdSceneDic[_labRefObj] = tdScene;
            }

            return tdScene;
        }

        private void dealAllTdScene(Action<TreasureHuntLabRefObj, MainAdditionTreasureHuntLabTDScene> _action)
        {
            if(_action == null || _m_dLabRefObjToTdSceneDic == null)
                return;

            _m_dLabRefObjToTdSceneDic.ForEach((kv) =>
            {
                _action(kv.Key, kv.Value);
            });
        }
        
        #region 添加Node方法

        public static void addNode(TreasureHuntLabRefObj _labRefObj, TreasureHuntGotTreasureInfo _putInTreasureInfo = null, long _selectedTreasureId = 0)
        {
            if ((QueueMgr.instance.findLastNode(typeof(GNodeTreasureHuntLab)) is GNodeTreasureHuntLab _labNode))
            {
                QueueMgr.instance.QuitUntilCanStop((_node) => _node == _labNode);
                _labNode._chgLab(_labRefObj, _putInTreasureInfo, _selectedTreasureId);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeTreasureHuntLab(_labRefObj, _putInTreasureInfo, _selectedTreasureId));
            }
        }
        
        public static void addNode(long _labId, TreasureHuntGotTreasureInfo _putInTreasureInfo = null, long _selectedTreasureId = 0)
        {
            TreasureHuntLabRefObj labRefObj = GRefdataCoreMgr.instance.treasureHuntLabRefCore.getRef(_labId);
            addNode(labRefObj, _putInTreasureInfo, _selectedTreasureId);
        }
        
        /// <summary>
        /// 根据需要放入的奇物信息添加实验室节点
        /// </summary>
        /// <param name="_putInTreasureInfo"></param>
        public static void addNodeByPutInTreasureInfo(TreasureHuntGotTreasureInfo _putInTreasureInfo)
        {
            addNode(_putInTreasureInfo?.treasureRefObj?.related_lab_id ?? 0, _putInTreasureInfo);
        }
        
        /// <summary>
        /// 根据需要选中的奇物id添加实验室节点
        /// </summary>
        /// <param name="_selectedTreasureId"></param>
        public static void addNodeBySelectedTreasureId(long _selectedTreasureId)
        {
            TreasureHuntTreasureRefObj treasureRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.getRef(_selectedTreasureId);
            addNode(treasureRefObj?.related_lab_id ?? 0, null, _selectedTreasureId);
        }
        
        #endregion
    }
}