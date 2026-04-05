using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using CommonEnum;
using GOE;
using Hotfix.Common.NumMergeObj;
using Hotfix.GC2GS.p202_NumMergeOp;
using Hotfix.GS2GC.p202_NumMergeOp;
using Hotfix.NumMergeEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏组件
    /// </summary>
    public class NumMergeComponent : HotfixActivityBasedComponent<GC2GS_202_001_ReqNumMergeInit, GS2GC_202_001_RetNumMergeInit>
    {
        [NotNull] public static readonly Vector2Int[] allBoardPositions = new Vector2Int[]
        {
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3),
            new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 3),
        };


        // 棋盘数据
        [ItemNotNull, NotNull] private readonly NumMergeTileInfo[][] _m_tileInfos;
        // 这局游戏走了多少步
        private int _m_currentStep;
        // 当前游戏的得分
        private long _m_currentScore;
        // 历史最高单局分数
        private long _m_maxScore;
        // 累计总得分
        private long _m_totalScore;

        // 宝箱信息
        [NotNull] private readonly NumMergeBoxInfo _m_boxInfo;


        public NumMergeComponent()
        {
            _m_tileInfos = new NumMergeTileInfo[4][];
            for (int x = 0; x < 4; x++)
            {
                _m_tileInfos[x] = new NumMergeTileInfo[4];
            }

            foreach (Vector2Int pos in allBoardPositions)
            {
                 _m_tileInfos[pos.x][pos.y] = new NumMergeTileInfo(pos);
            }

            _m_boxInfo = new NumMergeBoxInfo();
        }


        /// <summary>
        /// 分数变化事件
        /// </summary>
        public event Action<bool> onScoreChg;
        /// <summary>
        /// 棋盘数据变化事件
        /// </summary>
        public event Action onBoardDataChg;
        /// <summary>
        /// 宝箱数据变化事件
        /// </summary>
        public event Action onBoxDataChg;
        
        /// <summary>
        /// 格子数据，4x4 数组
        /// </summary>
        [ItemNotNull, NotNull] public NumMergeTileInfo[][] tileInfos { get { return _m_tileInfos; } }
        /// <summary>
        /// 这局游戏走了多少步
        /// </summary>
        public int currentStep { get { return _m_currentStep; } }
        /// <summary>
        /// 当前游戏的得分
        /// </summary>
        public long currentScore { get { return _m_currentScore; } }
        /// <summary>
        /// 历史最高单局分数
        /// </summary>
        public long maxScore { get { return _m_maxScore; } }
        /// <summary>
        /// 累计总得分
        /// </summary>
        public long totalScore { get { return _m_totalScore; } }
        /// <summary>
        /// 宝箱信息
        /// </summary>
        [NotNull] public NumMergeBoxInfo boxInfo { get { return _m_boxInfo; } }

        /// <summary>
        /// 数据关联的活动类型
        /// </summary>
        protected override ECommonActivityType activityType { get { return ECommonActivityType.NUM_MERGE; } }


        protected override void _dealInit()
        {
            // 清空红点
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_LAZY_CD, 0);
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_BOX_CAN_CLAIM, 0);
            
            base._dealInit();
        }
        /// <summary>
        /// 从服务器数据初始化组件数据
        /// </summary>
        protected override void _initData(GS2GC_202_001_RetNumMergeInit _data)
        {
            if (_data?.getInfo() == null)
            {
                Debug.LogError("[NumMergeComponent _initData] data or info is null");
                return;
            }

            NumMerge_Info info = _data.getInfo();
            _updateBoardData(info?.getBoardData(), false);
            _updateBoxData(info?.getBoxInfo());

            WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyChg);

            _refreshRed();
        }
        /// <summary>
        /// 清除组件数据
        /// </summary>
        protected override void _clearData()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyChg);

            // 清空棋盘
            foreach (Vector2Int pos in allBoardPositions)
            {
                _m_tileInfos[pos.x][pos.y].clear();
            }

            // 清空分数和步数
            _m_currentStep = 0;
            _m_currentScore = 0;
            _m_maxScore = 0;
            _m_totalScore = 0;

            // 清空宝箱信息
            _m_boxInfo.clear();

            // 清空红点
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_LAZY_CD, 0);
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_BOX_CAN_CLAIM, 0);
        }
        
        
        /// <summary>
        /// 更新棋盘数据
        /// </summary>
        private void _updateBoardData(NumMerge_BoardData _serverBoardData, bool _isByBuff)
        {
            if (_serverBoardData == null)
            {
                Debug.LogError("[NumMergeComponent _updateBoardData] serverBoardData is null");
                return;
            }

            // 更新棋盘格子数据
            // 服务器数据从左上到右下(top-left to bottom-right)，需要转换为左下到右上(bottom-left to top-right)
            List<NumMerge_BlockBase> blockList = _serverBoardData.getBlocks();
            if (blockList != null && blockList.Count == 16)
            {
                for (int i = 0; i < blockList.Count; i++)
                {
                    int x = i % 4;
                    int y = 3 - (i / 4); // 反转Y坐标：服务器y=0(顶部)映射到Unity y=3(顶部)
                    _m_tileInfos[x][y].updateFromBlockBase(blockList[i]);
                }
            }
            
            // 记录旧分数
            long oldScore = _m_currentScore;

            // 更新分数和步数
            _m_currentStep = _serverBoardData.getCurrentStep();
            _m_currentScore = _serverBoardData.getCurrentScore();
            _m_maxScore = _serverBoardData.getMaxScore();
            _m_totalScore = _serverBoardData.getTotalScore();
            
            // 触发数据变化事件
            onBoardDataChg?.Invoke();
            if (oldScore != _m_currentScore)
                onScoreChg?.Invoke(_isByBuff);
        }
        private void _updateBoxData(NumMerge_BoxInfo _serverBoxInfo)
        {
            if (_serverBoxInfo == null)
            {
                Debug.LogError("[NumMergeComponent _updateBoxData] serverBoxInfo is null");
                return;
            }

            // 更新宝箱数据
            _m_boxInfo.updateFromServer(_serverBoxInfo);

            // 触发宝箱数据变化事件
            onBoxDataChg?.Invoke();

            // 刷新宝箱红点
            _refreshBoxCanClaimRedTip();
        }
        /// <summary>
        /// 请求移动棋子
        /// </summary>
        public void reqNumMergeMove(ENumMerge_ModeType _modeType, ENumMerge_MoveDir _moveDir, Action<bool, GS2GC_202_003_RetNumMergeMove> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_202_003_ReqNumMergeMove(_modeType, _moveDir),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_202_003_RetNumMergeMove>(_reqDone));
        }
        /// <summary>
        /// 2048游戏结束/重新开始
        /// </summary>
        public void reqNumMergeGameOver(Action<bool, GS2GC_202_004_RetNumMergeGameOver> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_202_004_ReqNumMergeGameOver(),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_202_004_RetNumMergeGameOver>(_reqDone));
        }
        /// <summary>
        /// 请求使用重排道具
        /// </summary>
        public void reqNumMergeOrganize(Action<bool, GS2GC_202_005_RetNumMergeUseOrganizeItem> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_202_005_ReqNumMergeUseOrganizeItem(),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_202_005_RetNumMergeUseOrganizeItem>(_reqDone));
        }
        /// <summary>
        /// 请求使用消除道具
        /// </summary>
        public void reqNumMergeEliminate(Vector2Int _gridPos, Action<bool, GS2GC_202_006_RetNumMergeUseEliminateItem> _reqDone)
        {
            // 将 Unity 坐标转换为服务器索引：x + (3 - y) * 4
            int blockIndex = _gridPos.x + (3 - _gridPos.y) * 4;

            NPGSClientListener.sendRequestByLog(new GC2GS_202_006_ReqNumMergeUseEliminateItem(blockIndex),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_202_006_RetNumMergeUseEliminateItem>(_reqDone));
        }
        /// <summary>
        /// 领取宝箱奖励
        /// </summary>
        public void reqBoxReward(Action<bool, GS2GC_202_007_RetNumMergeDrawBox> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_202_007_ReqNumMergeDrawBox(), 
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_202_007_RetNumMergeDrawBox>(_reqDone));
        }
        /// <summary>
        /// 更新棋盘数据（供外部调用，如推送协议）
        /// </summary>
        public void updateBoardData(GS2GC_202_050_OnNumMergeBoardChg _msg)
        {
            if (_msg == null)
                return;
            
            _updateBoardData(_msg.getBoardData(), _msg.getIsBuffTriggered());
        }
        public void updateBoxData(GS2GC_202_051_OnNumMergeBoxChg _msg)
        {
            _updateBoxData(_msg?.getBoxInfo());
        }


        #region 红点

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRed()
        {
            _refreshLazyCDRedTip();
            _refreshBoxCanClaimRedTip();
        }

        /// <summary>
        /// 刷新2048体力红点(当前体力值达到体力上限的配置百分比时显示)
        /// </summary>
        private void _refreshLazyCDRedTip()
        {
            long lazyCDId = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id;
            PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(lazyCDId);
            if (lazyCdInfo == null)
            {
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_LAZY_CD, 0);
                return;
            }

            float showRedTipPer = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_red_tip_show_per / 100f;
            float nowLazyCdCountPer = 1f * lazyCdInfo.getCount() / lazyCdInfo.MaxCount;
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_LAZY_CD, nowLazyCdCountPer >= showRedTipPer ? 1 : 0);
        }

        /// <summary>
        /// 刷新2048宝箱可领取红点
        /// </summary>
        private void _refreshBoxCanClaimRedTip()
        {
            bool canClaim = _m_boxInfo.hasClaimableBox();
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.NUMMERGE_BOX_CAN_CLAIM, canClaim ? 1 : 0);
        }

        /// <summary>
        /// 体力变化回调
        /// </summary>
        private void _onLazyChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 1 || !(_objects[0] is long _lazyCdId) || _lazyCdId != HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id)
                return;

            _refreshLazyCDRedTip();
        }

        #endregion
    }
}
