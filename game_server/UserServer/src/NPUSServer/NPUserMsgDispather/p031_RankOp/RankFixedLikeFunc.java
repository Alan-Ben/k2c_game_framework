package NPUSServer.NPUserMsgDispather.p031_RankOp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.RankObj.RankFixed_LikeResult;
import Common.RankObj.Rank_BaseItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.RankErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Game.RangeWeight.RangeRandom;
import NPCommon.Game.RangeWeight.RangeRandomWeightList;
import NPCommon.Game.WeightValueList;
import NPCommon.Promise.SerialPromise;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.RefWrap;
import NPEnum.ENPItemType;
import NPEnum.ERankType;
import NPGameRes.Refs.Rank.RefRankFixed;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.RankFixedMgr.RankFixedInfo;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_006_ReqRankFixedLike;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_006_RetRankFixedLike;
import WCGCommon.Enum.NPEnum;

import java.util.List;

public class RankFixedLikeFunc
{
    /**
     * 随机点赞
     * @param _userdata
     * @param _rankFixedInfo
     * @param _needCross
     * @param _callback
     */
    public static void randomLike(NPUSUserData _userdata, RankFixedInfo _rankFixedInfo, boolean _needCross, NPPlayerContext _context, _ICallBackResultT<RankFixed_LikeResult> _callback)
    {
        //此处不再检查玩家是否有体力, 外部调用时已检查

        SerialPromise promise = new SerialPromise(null);
        //获取排行榜玩家人数
        RefWrap<Integer> rankSize = new RefWrap<>(0);
        promise.then(p ->
        {
            _rankFixedInfo.getRankListSize(false, new _ICallBackResultT<Integer>()
            {
                @Override
                public void onRunOver(Result _result, Integer _size)
                {
                    if (!_result.isSucc())
                    {
                        _callback.onRunOver(_result, null);
                        return;
                    }

                    rankSize.set(_size);
                    promise.commit();
                }
            });
        });

        //随机一个点赞的排名
        RefWrap<Integer> randomTargetRank = new RefWrap<>(0);
        promise.then(p ->
        {
            //随机点赞的排名
            int likeRank = randomLikeRank(rankSize.get());
            if (likeRank == 0)
            {
                _callback.onRunOver(RankErr.RANK_FIXED_LIKE_LIST_EMPTY, null);
                return;
            }

            randomTargetRank.set(likeRank);
            promise.commit();
        });

        //获取点赞对象
        RefWrap<Rank_BaseItem> targetContainer = new RefWrap<>(null);
        promise.then(p ->
        {
            _rankFixedInfo.makeRankBaseByRank(randomTargetRank.get(), _needCross, new _ICallBackResultT<Rank_BaseItem>()
            {
                @Override
                public void onRunOver(Result _result, Rank_BaseItem _rankItem)
                {
                    if (!_result.isSucc())
                    {
                        _callback.onRunOver(_result, null);
                        return;
                    }

                    targetContainer.set(_rankItem);
                    promise.commit();
                }
            });
        });

        //消耗点赞体力
        promise.then(p ->
        {
            NPPlayerContext context = NPPlayerContext.createNew(_context);

            long fixedCdId = _needCross ? _rankFixedInfo.getRef().cross_like_fixed_cd_id : _rankFixedInfo.getRef().like_fixed_cd_id;

            boolean consumeResult = _userdata.spendItem(ENPItemType.FIXED_CD, fixedCdId, 1, context);
            if (!consumeResult)
            {
                _callback.onRunOver(RankErr.RANK_FIXED_LIKE_CD_NOT_ENOUGH, null);
                return;
            }

            promise.commit();
        });

        //进行点赞
        promise.over(p ->
        {
            doLike(_userdata, _rankFixedInfo, targetContainer.get(), _needCross, _context, _result ->
            {
                if (_result == null)
                {
                    _callback.onRunOver(CommErr.SYS_ERR, null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _result);
            });
        });
    }

    /**
     * 随机点赞的排名
     * @param _size
     * @return
     */
    public static int randomLikeRank(int _size)
    {
        //获取排行榜玩家人数
        if (_size == 0)
            return 0;

        //新的权重值列表
        RangeRandomWeightList newWeightValueList = new RangeRandomWeightList();

        //遍历原有的点赞权重列表，构造新的点赞权重列表
        List<WeightValueList.WeightValue<RangeRandom>> weightValueList = RefGeneral.Ref().like_range.itemList();
        for (WeightValueList.WeightValue<RangeRandom> weightValue : weightValueList)
        {
            if (_size >= weightValue.value.getRangeStart())
            {
                if (_size >= weightValue.value.getRangeEnd())
                {
                    newWeightValueList.add(weightValue.value, weightValue.weight);
                } else
                {
                    newWeightValueList.add(new RangeRandom(weightValue.value.getRangeStart(), _size), weightValue.weight);
                }
            }
        }

        //如果新的权重值列表为空，返回空
        if (newWeightValueList.isEmpty())
            return 0;

        //随机一个点赞位置
        RangeRandom random = newWeightValueList.random();

        return random.random();
    }

    /**
     * 进行点赞操作
     * @param _likeTarget 点赞对象
     * @param _needCross  是否跨服
     * @param _context    上下文
     */
    public static void doLike(NPUSUserData _userdata, RankFixedInfo _rankFixed, Rank_BaseItem _likeTarget, boolean _needCross, NPPlayerContext _context, _ICallBackT<RankFixed_LikeResult> _callback)
    {
        RefRankFixed _refRankFixed = _rankFixed.getRef();

        long rankFixedId = _rankFixed.getRefId();

        int targetUsId = -1;
        if (_rankFixed.getRankRef().rank_type == ERankType.PLAYER)
        {
            targetUsId = CommonFunc.parseServerTypeIdFromCid(_likeTarget.getKey());
        } else if (_rankFixed.getRankRef().rank_type == ERankType.GUILD)
        {
            targetUsId = CommonFunc.parseServerTypeIdFromInstanced(_likeTarget.getKey());
        }

        if (targetUsId == -1)
        {
            USLog.error(_userdata.getUSServer(), "RankFixedLikeFunc doLike fail, rankFixedId:{} rankId:{} rankType:{} fromCid:{} key:{}",
                    rankFixedId, _rankFixed.getRankId(), _rankFixed.getRankRef().rank_type, _userdata.getCid(), _likeTarget.getKey());
            _callback.onRunOver(null);
            return;
        }

        if (targetUsId == _userdata.getUSServer().getServerTypeId())
        {
            //本服玩家点赞
            long finalScore = _userdata.getUSServer().getRankFixedObjDataListMgr().ensureObj(rankFixedId).incrLike(_likeTarget.getKey(), _needCross);

            //获取点赞奖励
            NPPlayerContext context = NPPlayerContext.createNew(_context);
            _userdata.gainItemList(_refRankFixed.like_reward, context);

            _callback.onRunOver(new RankFixed_LikeResult(rankFixedId, _likeTarget, finalScore, context.getCollector().toProto().getItemList()));
        } else
        {
            //跨服玩家点赞
            int finalTargetUsId = targetUsId;
            _userdata.getUSServer().sendRequestToBSServer(NPEnum.EServerType.USER.ordinal(), targetUsId,
                    new NP2US_R_003_006_ReqRankFixedLike(rankFixedId, _likeTarget.getKey(), _needCross), new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2US_RB_003_006_RetRankFixedLike();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _msg)
                        {
                            NP2US_RB_003_006_RetRankFixedLike retMsg = (NP2US_RB_003_006_RetRankFixedLike) _msg;

                            //获取点赞奖励
                            NPPlayerContext context = NPPlayerContext.createNew(_context);
                            _userdata.gainItemList(_refRankFixed.like_reward, context);

                            _callback.onRunOver(new RankFixed_LikeResult(rankFixedId, _likeTarget, retMsg.getLikeScore(), context.getCollector().toProto().getItemList()));
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            USLog.error(_userdata.getUSServer(), "RankFixedLikeFunc doLike fail, targetUsId:{} rankFixedId:{} fromCid:{} key:{} errCode:{}",
                                    finalTargetUsId, rankFixedId, _userdata.getCid(), _likeTarget.getKey(), _errCode);
                            _callback.onRunOver(null);
                        }
                    });

        }
    }
}
