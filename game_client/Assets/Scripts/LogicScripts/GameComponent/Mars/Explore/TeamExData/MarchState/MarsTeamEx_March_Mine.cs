using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 行军状态下的前往矿区的特殊状态
    /// </summary>
    public class MarsTeamEx_March_Mine : _IMarsTeamExDataInterface, _IMarsExploreMineItem, _IMarsExplorePosItem
    {
        private MarsTeamEx_March _m_mMarchExData;

        //数据序列号，确保操作有效性
        private long _m_lDataSerialize;
        //是否扩展矿点
        private bool _m_bIsExMine;
        //在获取具体数据之后会初始化的矿信息
        private MarsExploreMineRefObj _m_rMineRef;

        //矿的动态信息
        private long _m_lServerMineSerialize;
        private long _m_occupiedCid;
        private long _m_occupiedTeamId;
        private long _m_occupiedMs;
        private long _m_collectSpeed;
        private long _m_occupiedResourceNum;
        public MarsTeamEx_March_Mine(MarsTeamEx_March _marchExData)
        {
            _m_mMarchExData = _marchExData;

            _m_lDataSerialize = ALSerializeOpMgr.next();
            //判断是否是扩展矿点，不在玩家自身列表管理的是扩展矿点
            _m_bIsExMine = (null == NPPlayer.instance.marsComp.exploreSubComponent.getMineInfoByInstanceId(instanceId));
        }

        /// <summary>
        /// 初始化数据的处理
        /// </summary>
        /// <param name="_serverData"></param>
        protected void _updateData(Mars_MineDynamic _serverData)
        {
            _m_rMineRef = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_serverData.getRefId());
            if (_m_rMineRef == null)
            {
                ALLog.Error($"[MarsTeamEx_Collect::_initData] 未找到火星探索矿点配置, refId={_serverData.getRefId()}");
                return;
            }

            _m_lServerMineSerialize = _serverData.getSerialize();
            _m_occupiedCid = _serverData.getOccupiedCid();
            _m_occupiedTeamId = _serverData.getOccupiedTeamId();
            _m_occupiedMs = _serverData.getOccupiedMs();
            _m_collectSpeed = _serverData.getCollectSpeed();
            _m_occupiedResourceNum = _serverData.getRemainNum();
        }
        protected void _initData(Mars_MineDynamic _serverData)
        {
            _updateData(_serverData);

            //如数据无效不做后续处理
            if (null == _m_rMineRef)
                return;

            //注册展示
            NPPlayer.instance.marsComp.exploreSubComponent.addExPosItem(this);
        }

        /****************** 扩展数据相关接口 **************/

        /// <summary>
        /// 额外可以减少的存在时间，根据不同状态额外数据做处理
        /// 一般为0
        /// </summary>
        public long exReduceTimeMS { get { return 0; } }
        /// <summary>
        /// 更新扩展数据，如果状态没变化，则调用此接口
        /// </summary>
        /// <param name="_exData"></param>
        public void onUpdateExData(_IMarsTeamExDataInterface _exData)
        {
            //矿点更新不需要处理
        }

        /// <summary>
        /// 进入本状态类的处理
        /// </summary>
        public void onEnterExData()
        {
            //判断是否在已经监控了本矿对象，如果不是需要注册一个新的关注对象到相关数据类中
            _m_lDataSerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lDataSerialize;

            //判断本矿是否扩展矿点，是则需要发送消息请求数据，并在返回的数据中对数据进行初始化
            //需要发送请求时由于扩展点不在数据管理类中进行管理，因此需要本对象自行请求
            if (_m_bIsExMine)
            {
                //开始申请矿数据
                NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(instanceId, _m_lServerMineSerialize),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>((_isSuc, _msg) =>
                    {
                        if (!_isSuc)
                            return;

                        //数据无效则不处理
                        if (tmpSerialize != _m_lDataSerialize)
                            return;

                        Mars_MineDynamic mineDynamic = _msg?.getInfo();
                        _initData(mineDynamic);
                    }, null, true, true));
            }
        }

        /// <summary>
        /// 退出本状态类的处理
        /// </summary>
        public void onExitExData()
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
        }
        /****************** 扩展数据相关接口 End **************/

        /******************* 火星探索相关接口 ******************/
        public long instanceId { get { return _m_mMarchExData.data.getInstanceId(); } }
        public MarsExploreMineRefObj refObj { get { return _m_rMineRef; } }
        public long occupiedCid { get{ return _m_occupiedCid; } }
        public bool isMe { get{ return true; } }
        public long occupiedTeamId { get { return _m_occupiedTeamId; } }
        public long posId { get { return _m_mMarchExData.teamInfo.statePosId; } }
        public long startTime { get { return _m_occupiedMs; } }
        public long remainNum
        {
            get
            {
                long result = _m_occupiedResourceNum;
                result = Mathf.FloorToInt(_m_occupiedResourceNum - (FpsAndPingMgr.instance.serverTimeTag - _m_occupiedMs) * _m_collectSpeed / 1000f);

                return Math.Max(0, result);
            }
        }
        /// <summary>
        /// 尝试检查信息，通过41-13发送矿消息，附带状态序列号可以检查
        /// </summary>
        public void checkInfo()
        {
            if (null == _m_rMineRef)
                return;

            //不需要关注的不刷新
            if (_m_bIsExMine)
                return;

            long tmpSerialize = _m_lDataSerialize;
            //发送请求更新，如有变化需要调用相关接口
            //发送请求并在返回数据时刷新
            NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(instanceId)
                , new CommonRequestCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>(
                    (_res) =>
                    {
                        //序列号不一致则不处理
                        if (tmpSerialize != _m_lDataSerialize)
                            return;

                        //刷新数据
                        _updateData(_res.getInfo());

                        //如数据无效不做后续处理
                        if (null == _m_rMineRef)
                            return;
                    }, (_err) =>
                    {
                        //错误不做处理
                    }, 
                    true // 同时让回包正常处理
                ));
        }
        public EMarsBagItemUseTimeType timeType { get { return EMarsBagItemUseTimeType.NONE; } }
        public long guildHelpId { get { return 0; } }
        public void reqCompleteNow(Action<bool> _complete)
        {
            _complete?.Invoke(false);
        }
        /******************* 火星探索相关接口End ******************/
    }
}