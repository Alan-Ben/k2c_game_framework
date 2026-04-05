using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using Hotfix.TileMatchEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 游戏逻辑 统筹数据和表现部分
    /// </summary>
    public partial class TileMatchGameLogic : _AGameLogic
    {
        [NotNull] private TileMatchGameConfig _m_gameConfig;
        [NotNull] TileMatchGameStateMachine _m_stateMachine;

        private TileMatchModeRefObj _m_rNowGameModeRefObj;
        private int _m_errorSwitchCount = 0;//错误交换次数
        
        public TileMatchGameLogic([NotNull] GGUIWndTileMatchGamePlay _gameShow)
        {
            _m_gameShow = _gameShow;
            
            _m_gameConfig = new TileMatchGameConfig();
            _m_stateMachine = new TileMatchGameStateMachine(this);
            _m_stateMachine.setState<TileMatchGameStateMachineState_None>();

            _initGameConfig();
        }

        public TileMatchModeRefObj nowGameModeRefObj
        {
            get
            {
                ETileMatch_ModeType modeType = HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType();
                if (_m_rNowGameModeRefObj == null || _m_rNowGameModeRefObj.type != modeType)
                {
                    _m_rNowGameModeRefObj = HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.getRef((long) modeType);
                }

                return _m_rNowGameModeRefObj;
            }
        }
        
        public ETileMatchGameState curState => _m_stateMachine.curState.state;
        /// <summary>
        /// 格子被消除回调
        /// </summary>
        public event Action<Vector3, TileMatchBlockShowRefObj, int> onBlockClear;

        /// <summary>
        /// 开始进行服务器处理时调用
        /// </summary>
        public event Action<TileMatchGameStateMachineState_DealServer> onStartDealServer;
        
        /// <summary>
        /// 处理完服务器数据和表现后调用
        /// </summary>
        public event Action onDealServerDone;

        protected override void _startGameOp(Action _complete, Action _failed)
        {
            _m_errorSwitchCount = 0;
            
            // 初始化方块信息数组
            _initData();
            
            _complete?.Invoke();
        }

        protected override void _onStart()
        {
            _m_stateMachine.setState((TileMatchGameStateMachineState_Init _state) =>
            {
                _state?.init(() =>
                {
                    _m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
                }, () =>
                {
                    Debug.LogError($"[TileMatchGameLogic _onStart] Failed to initialize Tile Match blocks. Starting game failed.");
                    _m_stateMachine.setState<TileMatchGameStateMachineState_None>();
                });
            });

            ALMsgSys.RegisterMsg(HotfixMsgType.GET_TILEMATCH_TASK_CHG, _getTileMatchTaskChg);
        }

        protected override void _onStop()
        {
            ALMsgSys.UnregisterMsg(HotfixMsgType.GET_TILEMATCH_TASK_CHG, _getTileMatchTaskChg);

            onBlockClear = null;
            onDealServerDone = null;

            _m_rNowGameModeRefObj = null;
            _m_errorSwitchCount = 0;
            
            _m_stateMachine.setState<TileMatchGameStateMachineState_None>();
            _discardData();
        }

        protected override void _onTick(float _deltaTime)
        {
            _m_stateMachine.tick(_deltaTime);
        }

        private void _initGameConfig()
        {
            _m_gameConfig.errorSwitchShowTipCount = _m_gameShow.gameUIConfig?.errorSwitchShowTipCount ?? TileMatchGameConfig.defaultErrorSwitchShowTipCount;
            _m_gameConfig.idleToTipTime = _m_gameShow.gameUIConfig?.idleToTipTime ?? TileMatchGameConfig.defaultIdleToTipTime;
            _m_gameConfig.errorSwitchResetDelayTime = _m_gameShow.gameUIConfig?.errorSwitchResetDelayTime ?? TileMatchGameConfig.defaultErrorSwitchResetDelayTime;
            _m_gameConfig.exchangeBlockAudioId = _m_gameShow.gameUIConfig?.exchangeBlockAudioId ?? 0;
        }
        
        /// <summary>
        /// 检查当前是否是可操作的状态
        /// </summary>
        /// <returns></returns>
        private bool _checkCurStateCanOp()
        {
            //处于以下状态不处理用户输入
            return curState == ETileMatchGameState.Idle ||
                   curState == ETileMatchGameState.Tips;
        }

        /// <summary>
        /// 使用数据填充方块信息数组
        /// </summary>
        /// <param name="_blockBaseInfoList"></param>
        private void _fillBlockInfoArray(List<Common.TileMatchObj.TileMatch_BlockBaseInfo> _blockBaseInfoList)
        {
            if (_m_blockInfoArray == null)
                _initBlockInfoArray();
            
            if(_blockBaseInfoList == null)
                return;
            
            int blockCount = _blockBaseInfoList.Count;
            for (int i = 0; i < blockCount; i++)
            {
                Common.TileMatchObj.TileMatch_BlockBaseInfo blockBaseInfo = _blockBaseInfoList[i];
                if (blockBaseInfo == null)
                {
                    Debug.LogError($"[TileMatchGameLogic _initBlockInfoArray] : blockBaseInfo is null at index {i}");
                    continue;
                }

                Vector2Int logicPos = TileMatchUtil.indexToVector2(i);
                TileMatchBlockInfo blockInfo = _getBlockInfoFormArray(logicPos);
                if (blockInfo == null)
                {
                    Debug.LogError($"[TileMatchGameLogic _initBlockInfoArray] : blockInfo is null at logicPos {logicPos}");
                    continue;
                }

                _fillBlock(blockInfo, HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType(), blockBaseInfo.getBlockId());
                blockInfo.blockShow?.playIdleAnimation();
            }
        }

        /// <summary>
        /// 使用数据填充格子
        /// </summary>
        private void _fillBlock(TileMatchBlockInfo _blockInfo, long _blockRefId)
        {
            if(_blockInfo == null)
                return;

            //先重置原来格子信息, 这里主要是做个容错, 正常逻辑应该在外部先调用_clearBlock然后再进行填充
            _clearBlock(_blockInfo, false, 0, null);
            
            _blockInfo.setBlockRefId(_blockRefId);
            
            GGUIWndTileMatchChecker blockShow = _popBlockShow(_blockInfo.tileMatchBlockShowRefObj);
            if (blockShow != null)
            {
                blockShow.setUILocalPos(_m_gameShow.getBlockUiLocalPosition(_blockInfo.logicPos));
                blockShow.showWnd();
            }
            
            _blockInfo.setBlockShow(blockShow);
        }
        
        /// <summary>
        /// 使用数据填充格子
        /// </summary>
        /// <param name="_blockInfo"></param>
        /// <param name="_modeType"></param>
        /// <param name="_blockRefId"></param>
        private void _fillBlock(TileMatchBlockInfo _blockInfo, TileMatchEnum.ETileMatch_ModeType _modeType, long _blockRefId)
        {
            if(_blockInfo == null)
                return;

            //先重置原来格子信息, 这里主要是做个容错, 正常逻辑应该在外部先调用_clearBlock然后再进行填充
            _clearBlock(_blockInfo, false, 0, null);
            
            _blockInfo.setBlockRefId(_modeType, _blockRefId);
            
            GGUIWndTileMatchChecker blockShow = _popBlockShow(_blockInfo.tileMatchBlockShowRefObj);
            _blockInfo.setBlockShow(blockShow);
            if (blockShow != null)
            {
                blockShow.setUILocalPos(_m_gameShow.getBlockUiLocalPosition(_blockInfo.logicPos));
                blockShow.showWnd();
            }
        }
        
        /// <summary>
        /// 清除格子
        /// </summary>
        /// <param name="_blockInfo"></param>
        /// <param name="_dealBeforeBlockShowPushBack">在格子表现回收前调用的, Action:完成回调</param>
        /// <param name="_onPushBackDone">回收完成回调</param>
        private void _clearBlock(TileMatchBlockInfo _blockInfo, bool _isClearByGameProcessLogic, int _belongTaskSeriaId, Action<GGUIWndTileMatchChecker, Action> _dealBeforeBlockShowPushBack, Action _onPushBackDone = null)
        {
            if (_blockInfo == null)
            {
                _dealBeforeBlockShowPushBack?.Invoke(null, null);
                _onPushBackDone?.Invoke();
                return;
            }

            TileMatchBlockShowRefObj blockShowRefObj = _blockInfo.tileMatchBlockShowRefObj;
            // 重置格子数据
            _blockInfo.resetBlockInfo();
            
            // 重置格子show
            GGUIWndTileMatchChecker blockShow = _blockInfo.blockShow;
            _blockInfo.resetBlockShow();

            if (_dealBeforeBlockShowPushBack == null)
            {
                _pushBackBlockShow(blockShow);
                _onPushBackDone?.Invoke();
            }
            else
            {
                _dealBeforeBlockShowPushBack(blockShow, () =>
                {
                    _pushBackBlockShow(blockShow);
                    _onPushBackDone?.Invoke();
                });
            }

            if (_isClearByGameProcessLogic)
            {
                onBlockClear?.Invoke(_m_gameShow.getBlockUiWorldPosition(_blockInfo.logicPos), blockShowRefObj, _belongTaskSeriaId);
            }
        }
        
        /// <summary>
        /// 交换格子
        /// </summary>
        private void _exchangeBlock(TileMatchBlockInfo _blockInfo1, TileMatchBlockInfo _blockInfo2, Action _onCompleted = null)
        {
            if (!isRunning || _blockInfo1 == null || _blockInfo2 == null)
            {
                _onCompleted?.Invoke();
                return;
            }
            
            GGUIWndTileMatchChecker blockShow1 = _blockInfo1.blockShow;
            GGUIWndTileMatchChecker blockShow2 = _blockInfo2.blockShow;
            
            // 交换两个格子的数据
            long tmpBlockId = _blockInfo1.blockRefId;
            _blockInfo1.setBlockRefId(_blockInfo2.blockRefId);
            _blockInfo2.setBlockRefId(tmpBlockId);
                
            // 交换两个格子的表现
            _blockInfo1.setBlockShow(blockShow2);
            _blockInfo2.setBlockShow(blockShow1);
            
            ALStepCounter step = new ALStepCounter();
            step.chgTotalStepCount(2);
            step.regAllDoneDelegate(() =>
            {
                if(!isRunning)
                    return;
                
                _onCompleted?.Invoke();
            });
            
            if(blockShow1 != null)
                blockShow1.exChangeMoveTo(_m_gameShow.getBlockUiLocalPosition(_blockInfo2.logicPos), step.addDoneStepCount);
            else
                step.addDoneStepCount();
            
            if(blockShow2 != null)
                blockShow2.exChangeMoveTo(_m_gameShow.getBlockUiLocalPosition(_blockInfo1.logicPos), step.addDoneStepCount);
            else
                step.addDoneStepCount();
        }

        /// <summary>
        /// 开始进行服务器处理时调用
        /// </summary>
        /// <param name="_state"></param>
        private void _onStartDealServer(TileMatchGameStateMachineState_DealServer _state)
        {
            onStartDealServer?.Invoke(_state);
        }
        
        /// <summary>
        /// 当处理完服务器数据和表现后调用
        /// </summary>
        private void _onDealServerDone()
        {
            onDealServerDone?.Invoke();
            
            // 进入idle状态
            _m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
        }
        
        #region 处理ALMsgSys函数

        /// <summary>
        /// 当收到三消任务数据变化推送时
        /// </summary>
        /// <param name="_objs"></param>
        private void _getTileMatchTaskChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length <= 0 || !(_objs[0] is Common.TileMatchObj.TileMatch_TaskInfo tileMatchInfo))
                return;

            _updateTaskInfo(tileMatchInfo);
        }

        #endregion
    }
}