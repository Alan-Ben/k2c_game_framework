using System;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 采集状态的额外数据
    /// </summary>
    public class MarsTeamEx_Collect : _ATMarsTeamBasicExData<MarsTeamState_Collect>, _IMarsExploreMineItem, _IMarsExplorePosItem
    {
        //数据序列号，确保操作有效性
        private long _m_lDataSerialize;
        //是否扩展矿点
        private bool _m_bIsExMine;
        //在获取具体数据之后会初始化的矿信息
        private MarsExploreMineRefObj _m_rMineRef;

        //矿的动态信息
        private long _m_lServerMineSerialize;
        private long _m_occupiedMs;
        private long _m_collectSpeed;
        private long _m_occupiedResourceNum;
        public MarsTeamEx_Collect(MarsExploreTeamInfo _tiTeamInfo, byte[] _exData)
           : base(_tiTeamInfo, _exData)
        {
            _m_lDataSerialize = ALSerializeOpMgr.next();
            //判断是否是扩展矿点，不在玩家自身列表管理的是扩展矿点
            _m_bIsExMine = (null == NPPlayer.instance.marsComp.exploreSubComponent.getMineInfoByInstanceId(instanceId));
        }
        /// <summary>
        /// 读取数据，并返回结构体
        /// </summary>
        /// <param name="_exData"></param>
        /// <returns></returns>
        protected override MarsTeamState_Collect _readExData(byte[] _exData)
        {
            MarsTeamState_Collect data = new MarsTeamState_Collect();
            data.readPackage(_exData);

            return data;
        }

        /// <summary>
        /// 初始化数据的处理
        /// </summary>
        /// <param name="_serverData"></param>
        protected void _initData(Mars_MineDynamic _serverData)
        {
            _m_rMineRef = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_serverData.getRefId());
            if (_m_rMineRef == null)
            {
                ALLog.Error($"[MarsTeamEx_Collect::_initData] 未找到火星探索矿点配置, refId={_serverData.getRefId()}");
                return;
            }

            _m_lServerMineSerialize = _serverData.getSerialize();
            _m_occupiedMs = _serverData.getOccupiedMs();
            _m_collectSpeed = _serverData.getCollectSpeed();
            _m_occupiedResourceNum = _serverData.getRemainNum();

            //注册展示
            NPPlayer.instance.marsComp.exploreSubComponent.addExPosItem(this);
        }

        /****************** 扩展数据相关接口 **************/
        /// <summary>
        /// 更新扩展数据，如果状态没变化，则调用此接口
        /// </summary>
        /// <param name="_exData"></param>
        public override void onUpdateExData(_IMarsTeamExDataInterface _exData)
        {
            //矿点更新不需要处理
        }

        /// <summary>
        /// 进入本状态类的处理
        /// </summary>
        public override void onEnterExData()
        {
            //判断是否在已经监控了本矿对象，如果不是需要注册一个新的关注对象到相关数据类中
            _m_lDataSerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lDataSerialize;

            //判断本矿是否扩展矿点
            if (_m_bIsExMine)
            {
                //开始申请矿数据
                NPGSClientListener.sendRequestByLog(new GC2GS_041_011_ReqMarsMineInfo(instanceId),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_011_RetMarsMineInfo>((_isSuc, _msg) =>
                    {
                        if (!_isSuc)
                            return;

                        //数据无效则不处理
                        if (tmpSerialize != _m_lDataSerialize)
                            return;

                        Mars_MineDynamic mineDynamic = _msg?.getInfo();
                        _initData(mineDynamic);
                    }));
            }
            else
            {
                //如果是关注的数据，需要发消息刷新
                NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(instanceId, _m_lServerMineSerialize),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>((_isSuc, _msg) =>
                    {
                    }, _errorCode =>
                    {
                        if (_errorCode == ErrorCodeConst.MARS_MINE_NOT_FOUND)
                            return;
                    
                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                    }, false, true));
            }
        }

        /// <summary>
        /// 退出本状态类的处理
        /// </summary>
        public override void onExitExData()
        {
            //退出的时候需要从相关数据类中移除对本矿对象的监控
            //刷新序列号
            _m_lDataSerialize = ALSerializeOpMgr.next();

            //判断本矿是否扩展矿点
            if (_m_bIsExMine)
            {
                //移除展示
                NPPlayer.instance.marsComp.exploreSubComponent.removeExPosItem(this);
            }
            else
            {
                //如果是关注的数据，需要发消息刷新
                NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(instanceId, _m_lServerMineSerialize), 
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>((_isSuc, _msg) =>
                    {
                        
                    }, _errorCode =>
                    {
                        if (_errorCode == ErrorCodeConst.MARS_MINE_NOT_FOUND)
                            return;

                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                    }, false, true));
            }
        }
        /****************** 扩展数据相关接口 End **************/

        /******************* 火星探索相关接口 ******************/
        public long instanceId { get { return data.getMineInstanceId(); } }
        public MarsExploreMineRefObj refObj { get { return _m_rMineRef; } }
        public long occupiedCid { get{ return NPPlayer.instance.playerInfo.CID; } }
        public bool isMe { get{ return true; } }
        public long occupiedTeamId { get { return teamInfo.teamId; } }
        public long posId { get { return data.getPos(); } }
        public long startTime { get { return data.getStartCollectMs(); } }
        public long remainNum
        {
            get
            {
                long result = _m_occupiedResourceNum;
                result = Mathf.FloorToInt(_m_occupiedResourceNum - (FpsAndPingMgr.instance.serverTimeTag - _m_occupiedMs) * _m_collectSpeed / 1000f);

                return Math.Max(0, result);
            }
        }
        public override EMarsBagItemUseTimeType timeType { get { return EMarsBagItemUseTimeType.NONE; } }
        public override long guildHelpId { get { return 0; } }
        /// <summary>
        /// 尝试检查信息，通过41-13发送矿消息，附带状态序列号可以检查
        /// </summary>
        public void checkInfo()
        {
            //挖矿的数据不需要做更新
        }
        public override void reqCompleteNow(Action<bool> _complete)
        {
            _complete?.Invoke(false);
        }
        /******************* 火星探索相关接口End ******************/
    }
}