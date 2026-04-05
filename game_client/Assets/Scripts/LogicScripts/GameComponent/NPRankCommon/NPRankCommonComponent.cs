using System.Collections.Generic;
using System;
using GS2GC.p004_PlayerOp;
using GS2GC.p031_RankOp;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;

namespace GOE
{
    //通用排行榜管理类
    public class NPRankCommonComponent : _ANPBasicPlayerComponent
    {
        //是否有公会在排行榜中
        [NotNull] private Dictionary<long,bool> _m_bHaveGuildInRankDic = new Dictionary<long, bool>();

        public NPRankCommonComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //是否必要型初始化组件
        public override bool isMustInit { get { return true; } }
        //获取组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RANK_COMMON; } }
        //依赖的组件队列
        public override ENPPlayerCompType[] dependCompList { get { return new ENPPlayerCompType[]{ ENPPlayerCompType.FIXED_CD }; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
        }

        //处理初始化操作
        protected override void _dealInit()
        {
            setInitDone();
        }
        //初始化结果操作
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCDChg);
        }
        protected override void _onInitFail()
        {

        }

        public override void onAllCompInited()
        {
            //刷新入口红点
            refreshRedTip();
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_bHaveGuildInRankDic?.Clear();
            WinMsg.UnregisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCDChg);
        }

        //计算时间差
        public long getNextDayMargin()
        {
            //服务端时间
            long nowTime = FpsAndPingMgr.instance.serverTimeTag;
            DateTime dt = TimeUtil.FromUTCMilliseconds(nowTime);
            DateTime dt2 = dt.AddDays(1).Date;
            long nextTime = TimeUtil.dateTime2Milliseconds(dt2);
            return nextTime - nowTime;
        }

        /// <summary>
        /// 刷新排行榜入口红点
        /// </summary>
        public void refreshRedTip()
        {
            for (int i = 0; i < GRefdataCoreMgr.instance.rankFixedRefCore.refList.Count; i++)
            {
                NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.refList[i];
                if (rankFixedRef != null && NPPlayer.instance.fixedCdComp.getCount(rankFixedRef.like_fixed_cd_id) > 0)
                {
                    NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixedRef.rank_id);

                    //如果是联盟排行榜，则需要判断是否有联盟在排行榜中
                    if (rankRef != null && rankRef.rank_type == ERankType.GUILD)
                    {
                        if (getHaveGuildInRank(rankFixedRef.id))
                        {
                            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RANK_ENTRANCE, 1);
                            return;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RANK_ENTRANCE, 1);
                        return;
                    }
                }
            }

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RANK_ENTRANCE, 0);
        }

        /// <summary>
        /// 是否有联盟在排行榜中
        /// </summary>
        /// <param name="_rankFixId"></param>
        /// <returns></returns>
        public bool getHaveGuildInRank(long _rankFixId)
        {
            if (_m_bHaveGuildInRankDic.TryGetValue(_rankFixId, out bool hasGuild))
                return hasGuild;
            return true;
        }

        /// <summary>
        /// 记录是否有联盟在排行榜中
        /// </summary>
        /// <param name="_rankFixId"></param>
        /// <param name="_hasGuild"></param>
        public void recordHaveGuildInRank(long _rankFixId, bool _hasGuild)
        {
            _m_bHaveGuildInRankDic[_rankFixId] = _hasGuild;
            refreshRedTip();
        }

        //fixed cd 数量变化
        private void _onFixedCDChg(params object[] _objects)
        {
            refreshRedTip();
        }

        #region S2C


        #endregion

        #region C2S

        /// <summary>
        /// 请求常驻排行榜列表基础信息  这里应该是根据排行榜实例id去请求排行榜数据
        /// </summary>
        /// <param name="_rankId"></param>
        /// <param name="_callBack"></param>
        public void reqRankFixedBaseList(long _rankFixedId, Action<GS2GC_031_001_RetRankFixedBaseList> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_031_RankOp.make_001_ReqRankFixedBaseList(_rankFixedId),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_031_001_RetRankFixedBaseList>((info) =>
               {
                   if (null != info && null != _callBack)
                       _callBack(info);
               }));
        }

        /// <summary>
        /// 根据排名请求玩家排行榜相关数据
        /// </summary>
        public void reqInRankPlayerInfoByRank(long _rankFixedId, int _rank, Action<Common.RankObj.Rank_BaseItem> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_031_RankOp.make_005_ReqRankFixedInfoByRank(_rankFixedId, _rank),
              new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_031_005_RetRankFixedInfoByRank>((info) =>
              {
                  if (null != info && null != _callBack)
                      _callBack(info.getInfo());
              }));
        }

        /// <summary>
        /// 根据cid请求玩家排行榜相关数据
        /// </summary>
        /// <param name="_rankId"></param>
        /// <param name="_cid"></param>
        /// <param name="_callBack"></param>
        public void reqInRankPlayerInfoByCid(long _rankFixedId, long _cid, Action<Common.RankObj.Rank_BaseItem> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_031_RankOp.make_006_ReqRankFixedInfoByKey(_rankFixedId, _cid),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_031_006_RetRankFixedInfoByKey>((info) =>
               {
                   if (null != info && null != _callBack)
                       _callBack(info.getInfo());
               }));
        }

        /// <summary>
        /// 对排行榜点赞
        /// </summary>
        /// <param name="_rankFixedId"></param>
        /// <param name="_callBack"></param>
        public void reqRankFixedLike(long _rankFixedId, Action<Common.RankObj.RankFixed_LikeResult> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_031_RankOp.make_002_ReqRankFixedLike(_rankFixedId),
              new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_031_002_RetRankFixedLike>((info) =>
              {
                  if (null != info && null != _callBack)
                      _callBack(info.getLikeResult());
              }));
        }

        /// <summary>
        /// 请求点赞积分
        /// </summary>
        /// <param name="_rankId"></param>
        /// <param name="_cid"></param>
        /// <param name="_callBack"></param>
        public void reqRankLikeScore(long _rankFixedId, long _cid, Action<long> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_031_RankOp.make_003_ReqRankFixedLikeScore(_rankFixedId, _cid),
              new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_031_003_RetRankFixedLikeScore>((info) =>
              {
                  if (null != info && null != _callBack)
                      _callBack(info.getLikeScore());
              }));
        }

        /// <summary>
        /// 对排行榜一键点赞
        /// </summary>
        /// <param name="_rankFixedId"></param>
        /// <param name="_callBack"></param>
        public void reqRankFixedAKeyLike(List<long> _rankFixedIdList, Action<List<Common.RankObj.RankFixed_LikeResult>> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_031_RankOp.make_004_ReqRankFixedAKeyLike(_rankFixedIdList),
              new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_031_004_RetRankFixedAKeyLike>((info) =>
              {
                  if (null != info && null != _callBack)
                      _callBack(info.getLikeResultList());
              }));
        }

        /// <summary>
        /// 请求玩家简要信息
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_callBack"></param>
        public void reqPlayerBriefInfo(long _cid, Action<PlayerInfo_IconShow> _callBack)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_cid),
             new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((info) =>
             {
                 if (null != info && null != _callBack)
                     _callBack(info.getPlayerBrief());
             }));
        }
        #endregion
    }
}
