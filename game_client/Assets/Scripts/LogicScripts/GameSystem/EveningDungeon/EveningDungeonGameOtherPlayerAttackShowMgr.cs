using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.EveningDungeon
{
    public class EveningDungeonGameOtherPlayerAttackShowMgr
    {
        [NotNull] private EveningDungeonGameLogic _m_gameLogic;

        [NotNull] private List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _m_lAllAttackAreaList = new List<GGUISubWndEveningDungeonOtherPlayerAttackArea>();//其他玩家攻击表现区域列表
        [NotNull] private List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _m_lEnableAttackAreaList = new List<GGUISubWndEveningDungeonOtherPlayerAttackArea>();//可用的其他玩家攻击表现区域列表
        [NotNull] private List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _m_lShowingAttackAreaList = new List<GGUISubWndEveningDungeonOtherPlayerAttackArea>();//正在表现的其他玩家攻击表现区域列表
        [NotNull] private List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _m_lHidingAttackAreaList = new List<GGUISubWndEveningDungeonOtherPlayerAttackArea>();//正在隐藏的其他玩家攻击表现区域列表
        private int _m_iAttackAreaCount = 0;//攻击表现区域数量
        private bool _m_bIsFirstReq;//是否第一次请求, 第一次请求到的攻击数据是历史攻击数据, 只进行序列号更新, 不进行展示
        private long _m_lPreReqLogSerialize;//上一次请求到的攻击日志序列号

        [NotNull] private List<Common.DungeonObj.EveningDungeon_AttackLog> _m_lWaittingShowLogList = new List<Common.DungeonObj.EveningDungeon_AttackLog>();//等待表现的攻击日志列表
        
        private ALCommonEnableTaskController _m_reqAttackLogTask;//请求攻击日志的任务
        
        public EveningDungeonGameOtherPlayerAttackShowMgr ([NotNull] EveningDungeonGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }

        public void init()
        {
            _m_gameLogic.uiScene.getOtherPlayerAttackArea(_m_lAllAttackAreaList);
            _m_iAttackAreaCount = _m_lAllAttackAreaList.Count;
            foreach (var attackArea in _m_lAllAttackAreaList)
            {
                if(attackArea == null)
                    continue;

                attackArea.onOnceAttackShowDone += _onItemOnceAttackShowDone;
                attackArea.onTotalAttackShowDone += _onItemTotalAttackShowDone;
            }
            
            _m_lEnableAttackAreaList.AddRange(_m_lAllAttackAreaList);
            _m_lShowingAttackAreaList.Clear();
            _m_lHidingAttackAreaList.Clear();

            _m_lWaittingShowLogList.Clear();
            
            _initReqAttackLogTask();
        }

        public void discrad()
        {
            _discardReqAttackLogTask();
            
            foreach (var attackArea in _m_lAllAttackAreaList)
            {
                if(attackArea == null)
                    continue;

                attackArea.onOnceAttackShowDone -= _onItemOnceAttackShowDone;
                attackArea.onTotalAttackShowDone -= _onItemTotalAttackShowDone;
            }
            
            _m_lAllAttackAreaList.Clear();
            _m_lEnableAttackAreaList.Clear();
            _m_lShowingAttackAreaList.Clear();
            _m_lHidingAttackAreaList.Clear();
            
            _m_lWaittingShowLogList.Clear();
        }
        
        /// <summary>
        /// 当某个item一次攻击表现完成时
        /// </summary>
        /// <param name="_area"></param>
        private void _onItemOnceAttackShowDone(GGUISubWndEveningDungeonOtherPlayerAttackArea _area)
        {
            if(_area == null)
                return;

            if (_area.attackLog != null)
            {
                // 遍历等待显示的log数据列表
                Common.DungeonObj.EveningDungeon_AttackLog attackLog = null;
                for (int i = _m_lWaittingShowLogList.Count - 1; i >= 0; i--)
                {
                    attackLog = _m_lWaittingShowLogList[i];
                    // 若当前显示的攻击日志在该表现区域窗口中中, 则移除该日志, 直接在该表现区域进行展示
                    if (attackLog != null && attackLog.getCid() == _area.attackLog.getCid())
                    {
                        _m_lWaittingShowLogList.RemoveAt(i);
                        _area.showAttack(attackLog);
                        return;
                    }
                }
            }

            // 到这里时, 说明该攻击区域没有需要继续展示的表现, 从正在表现列表中移除, 加入隐藏窗口中列表
            _m_lShowingAttackAreaList.Remove(_area);
            _m_lHidingAttackAreaList.Add(_area);
        }
        
        /// <summary>
        /// 当某个item所有攻击表现显示完成时
        /// </summary>
        /// <param name="_area"></param>
        private void _onItemTotalAttackShowDone(GGUISubWndEveningDungeonOtherPlayerAttackArea _area)
        {
            if(_area == null)
                return;
            
            // 从隐藏中列表中, 加入可用列表中
            _m_lHidingAttackAreaList.Remove(_area);
            _m_lEnableAttackAreaList.Add(_area);
            
            // 尝试进行展示攻击表现
            _tryShowAttackLog();
        }

        /// <summary>
        /// 进行攻击log表现
        /// </summary>
        private void _tryShowAttackLog()
        {
            GGUISubWndEveningDungeonOtherPlayerAttackArea enableAttackArea = null;
            Common.DungeonObj.EveningDungeon_AttackLog attackLog = null;
            
            // 遍历等待展示log数据
            for (int i = _m_lWaittingShowLogList.Count - 1; i >= 0; i--)
            {
                // 若在遍历过程中, 发现没有可用的表现区域, 直接返回
                if(_m_lEnableAttackAreaList.Count <= 0)
                    return;
                
                attackLog = _m_lWaittingShowLogList[i];
                if (attackLog == null)
                {
                    _m_lWaittingShowLogList.RemoveAt(i);
                    continue;
                }
                
                // 若该attackLog对应的玩家 在 _m_lShowingAttackAreaList正在表现列表中, 则跳过
                if (_m_lShowingAttackAreaList.Find((_showingAttackArea) =>
                    {
                        if (_showingAttackArea != null && _showingAttackArea.attackLog != null && _showingAttackArea.attackLog.getCid() == attackLog.getCid())
                            return true;
                        return false;
                    }) != null)
                {
                    continue;
                }
                
                // 若该attackLog对应的玩家 在 _m_lHidingAttackAreaList正在隐藏窗口中, 则跳过
                if (_m_lHidingAttackAreaList.Find((_showingAttackArea) =>
                    {
                        if (_showingAttackArea != null && _showingAttackArea.attackLog != null && _showingAttackArea.attackLog.getCid() == attackLog.getCid())
                            return true;
                        return false;
                    }) != null)
                {
                    continue;
                }
                
                // 遍历可用的表现区域列表, 找到一个可用的表现区域, 并从可用列表中移除
                while (_m_lEnableAttackAreaList.Count > 0 && enableAttackArea == null)
                {
                    enableAttackArea = _m_lEnableAttackAreaList.GetLastAndRemove();
                }
                
                if(enableAttackArea != null)
                {
                    // 移除表现数据
                    _m_lWaittingShowLogList.RemoveAt(i);
                    // enableAttackArea加入表现中的区域
                    _m_lShowingAttackAreaList.Add(enableAttackArea);
                    enableAttackArea.showAttack(attackLog);
                }
            }
        }
        
        #region 请求攻击日志任务

        /// <summary>
        /// 销毁请求攻击日志任务
        /// </summary>
        private void _discardReqAttackLogTask()
        {
            _m_reqAttackLogTask.setDisable();
        }

        /// <summary>
        /// 初始化请求攻击日志任务
        /// </summary>
        private void _initReqAttackLogTask()
        {
            _discardReqAttackLogTask();

            float refreshOtherPlayerAttackLogTimeInterval = _m_gameLogic.uiConfig?.refreshOtherPlayerAttackLogTimeInterval ?? 0;
            if (refreshOtherPlayerAttackLogTimeInterval <= 0)
                refreshOtherPlayerAttackLogTimeInterval = 0.1f;
            
            _m_bIsFirstReq = true;
            _m_lPreReqLogSerialize = -1;//上一次请求到的攻击日志序列号重置
            _m_reqAttackLogTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_taskDeal, refreshOtherPlayerAttackLogTimeInterval);
        }

        /// <summary>
        /// 任务处理
        /// </summary>
        private void _taskDeal()
        {
            // 只有在游戏处于idle状态时才请求攻击日志
            if(_m_gameLogic.gameState != EEveningDungeonGameState.IDLE)
                return;
            
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonAttackLog(_m_lPreReqLogSerialize, _m_iAttackAreaCount, _onRetAttackLog);
        }
        
        /// <summary>
        /// 收到回包
        /// </summary>
        /// <param name="_retMsg"></param>
        private void _onRetAttackLog(GC2GS.p024_DungeonOp.GC2GS_024_013_ReqEveningDungeonAttackLog _reqMsg, GS2GC.p024_DungeonOp.GS2GC_024_013_RetEveningDungeonAttackLog _retMsg)
        {
            if(_retMsg == null || _retMsg.getLogList() == null || _retMsg.getLogList().Count <= 0)
                return;

            // 因为在请求收到回包时没有打印协议, 这里进行打印
            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                // 打印请求协议
                if (_reqMsg != null)
                {
                    if (_reqMsg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                    {
                        GCommon.NetSend(string.Format("<color=red>C -> S: {0} ; </color> {1}", _reqMsg.GetType().Name, GCommon.GetInfoPropertys(_reqMsg)));
                    }
                    else
                    {
                        GCommon.NetWaring($"协议{_reqMsg.GetType().Name}过大，大小：{_reqMsg.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                    }
                }
                
                //打印回包
                if (_retMsg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", _retMsg.GetType().Name, GCommon.GetInfoPropertys(_retMsg)));
                }
                else
                {
                    GCommon.NetWaring($"协议{_retMsg.GetType().Name}过大，大小：{_retMsg.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }

            long preReqLogSerialize = _m_lPreReqLogSerialize;//
            foreach (var item in _retMsg.getLogList())
            {
                if(item == null)
                    return;

                // 记录请求到的最大序列号
                if (_m_lPreReqLogSerialize < item.getSerial())
                    _m_lPreReqLogSerialize = item.getSerial();
                
                // 只有非自己的log 和 序列号大于上次请求到的最大序列号的log才加入等待表现列表
                if(item.getCid() != NPPlayer.instance.playerInfo.CID && item.getSerial() > preReqLogSerialize)
                    _m_lWaittingShowLogList.Add(item);
            }
            // 按照 序列号小->大, 触发时间早->晚 排序
            _m_lWaittingShowLogList.Sort((_log1, _log2) =>
            {
                if (_log2 == null)
                    return -1;
                if (_log1 == null)
                    return 1;
                if (object.ReferenceEquals(_log1, _log2))
                    return 0;

                if (_log1.getSerial().CompareTo(_log2.getSerial()) != 0)
                    return _log1.getSerial().CompareTo(_log2.getSerial());

                return _log1.getTimestamp().CompareTo(_log2.getTimestamp());
            });

            if (_m_bIsFirstReq)//第一次请求到的攻击数据是历史攻击数据, 只进行序列号更新, 不进行展示
            {
                _m_bIsFirstReq = false;
                _m_lWaittingShowLogList.Clear();
            }
            else
            {
                // 若等待表现的日志数量大于可用的表现区域数量, 则移除多余的日志
                if (_m_lWaittingShowLogList.Count > _m_iAttackAreaCount)
                    _m_lWaittingShowLogList.RemoveRange(0, _m_lWaittingShowLogList.Count - _m_iAttackAreaCount);
            }
            
            _tryShowAttackLog();
        }
        
        #endregion
    }
}