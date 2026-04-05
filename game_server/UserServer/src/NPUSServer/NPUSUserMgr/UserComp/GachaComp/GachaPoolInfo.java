package NPUSServer.NPUSUserMgr.UserComp.GachaComp;

import Common.GachaObj.Gacha_PoolInfo;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Game.WeightQualityValueList;
import NPCommon.Game.WeightValueList;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPCommon.Util.RefWrap;
import NPEnum.EQuality;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import NPGameRes.Refs.AvatarGacha.RefGachaPoolStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.GachaConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Guarantee.GachaGuaranteeInfo;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Guarantee.GachaGuaranteeList;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Weight.GachaWeightList;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerGachaBO;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class GachaPoolInfo
{
    private GachaComponent _m_comp;
    private RefGachaPool _m_refPool;
    private RefGachaPoolStep _m_refPoolStep;
    private PlayerGachaBO _m_bo;

    private GachaWeightList _m_weightInfo;
    private GachaGuaranteeList _m_guaranteeInfo;

    public GachaPoolInfo(GachaComponent _comp, RefGachaPool _refPool, RefGachaPoolStep _refPoolStep, PlayerGachaBO _bo)
    {
        _m_comp = _comp;
        _m_refPool = _refPool;
        _m_refPoolStep = _refPoolStep;
        _m_bo = _bo;

        //初始化权重信息
        _m_weightInfo = new GachaWeightList(_refPool.weightRefList);
        _m_weightInfo.initFromDb(_m_comp, _bo.getWeightInfo());

        //初始化保底信息
        _m_guaranteeInfo = new GachaGuaranteeList(_refPoolStep.guaranteeRuleRefList);
        _m_guaranteeInfo.initFromDb(_m_comp, _bo.getGuaranteeInfo());
    }

    public long getPoolId()
    {
        return _m_refPool.Id();
    }

    /**
     * 随机一个卡牌
     */
    public RefGachaItem roll(WCGPairLong _randomResult)
    {
        //抽取到的卡牌
        RefGachaItem rollResultItemRef;


        //判断是否需要走保底逻辑
        GachaGuaranteeInfo needOpGuaranteeRule = _m_guaranteeInfo.getNeedOpGuaranteeRule();
        if (needOpGuaranteeRule != null)
        {
            rollResultItemRef = guaranteeRoll(needOpGuaranteeRule);

            //如果抽取失败，则直接返回
            if (rollResultItemRef == null)
            {
                USLog.error(_m_comp.getUSServer(), "AvatarGachaPoolInfo guaranteeRoll failed poolId:{} guaranteeRuleId:{}",
                        getPoolId(), needOpGuaranteeRule.getGuaranteeRuleId());
                return null;
            }

            needOpGuaranteeRule.resetGuaranteeTimes();
            _m_bo.setGuaranteeInfo(_m_comp.getUSServer().getBM(), _m_guaranteeInfo.toString());
        } else
        {
            rollResultItemRef = normalRoll(_randomResult);

            //如果抽取失败，则直接返回
            if (rollResultItemRef == null)
            {
                USLog.error(_m_comp.getUSServer(), "AvatarGachaPoolInfo normalRoll failed poolId:{}", getPoolId());
                return null;
            }

            //变更保底记录
            _m_guaranteeInfo.dealRollResult(_m_comp.getUserData(), rollResultItemRef);
            _m_bo.setGuaranteeInfo(_m_comp.getUSServer().getBM(), _m_guaranteeInfo.toString());
        }

        //处理抽取结果
        EQuality quality = rollResultItemRef.getQuality();
        //变更权重记录
        _m_weightInfo.dealRollResult(quality);
        _m_bo.setWeightInfo(_m_comp.getUSServer().getBM(), _m_weightInfo.toString());

        //记录抽卡次数
        _m_bo.setCurStepRollNum(_m_comp.getUSServer().getBM(), _m_bo.getCurStepRollNum() + 1);
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        //检查卡池升级
        checkPoolUpgrade();

        return rollResultItemRef;
    }

    /**
     * 检查卡池升阶
     */
    public void checkPoolUpgrade()
    {
        //无下一阶段
        if (_m_refPoolStep.upgrade_step_num == -1)
            return;

        //没达到升阶次数
        if (_m_bo.getCurStepRollNum() < _m_refPoolStep.upgrade_step_num)
            return;

        //查询下一阶段配置
        RefGachaPoolStep refPoolStep = RefGachaPoolStep.getMgr().lookupByPoolStep(_m_refPool.Id(), _m_refPoolStep.step + 1);
        if (refPoolStep == null)
        {
            USLog.error(_m_comp.getUSServer(), "AvatarGachaPoolInfo checkPoolUpgrade next step not found poolId:{} step:{}",
                    _m_refPool.Id(), _m_refPoolStep.step + 1);
            return;
        }

        _m_refPoolStep = refPoolStep;

        //升阶后需要重置相关数据
        _m_bo.setStep(_m_comp.getUSServer().getBM(), _m_refPoolStep.step);
        _m_bo.setCurStepRollNum(_m_comp.getUSServer().getBM(), 0);
        _m_bo.setGuaranteeInfo(_m_comp.getUSServer().getBM(), "");
        _m_bo.setWeightInfo(_m_comp.getUSServer().getBM(), "");
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        _m_guaranteeInfo = new GachaGuaranteeList(_m_refPoolStep.guaranteeRuleRefList);
        _m_weightInfo = new GachaWeightList(_m_refPool.weightRefList);
    }

    /**
     * 保底抽取
     * @param _guaranteeInfo 保底规则
     * @return 抽取到的卡牌
     */
    public RefGachaItem guaranteeRoll(GachaGuaranteeInfo _guaranteeInfo)
    {
        //构造过滤后的列表(完全符合要求的卡牌)
        List<RefGachaItem> itemList = new ArrayList<>();
        //构造过滤后的列表(已拥有过的卡牌)
        List<RefGachaItem> itemListWithHas = new ArrayList<>();
        //遍历卡牌列表
        for (RefGachaItem refItem : _m_refPool.itemRefList)
        {
            boolean isEnable = GachaConditionDealerMgr.getInstance().isEnable(
                    _m_comp.getUserData(), _guaranteeInfo.getRef().guarantee_roll_cond_list, refItem);
            if (!isEnable)
                continue;

            //判断是否需要新卡
            if (_guaranteeInfo.getRef().guarantee_need_new)
            {
                //判断是否是新卡
                boolean alreadyHas = _m_comp.getUserData().hasItem(refItem.item.getItemType(), refItem.item.getItemId(), 1);
                if (alreadyHas)
                {
                    itemListWithHas.add(refItem);
                    continue;
                }
            }

            itemList.add(refItem);
        }

        //如果完全符合要求的卡牌列表不为空，则随机一个
        if (!itemList.isEmpty())
            return CommonFunc.randSelect(itemList);

        //如果完全符合要求的卡牌列表为空，则随机一个已拥有过的卡牌
        return CommonFunc.randSelect(itemListWithHas);
    }

    /**
     * 普通抽取
     */
    public RefGachaItem normalRoll(WCGPairLong _randomResult)
    {
        //构造权重列表
        WeightQualityValueList qualityWeightList = _m_weightInfo.genWeightList();

        Random random = new Random();

        RefWrap<Long> qualityRandomResult = new RefWrap<>(0L);
        //随机品质
        EQuality quality = qualityWeightList.random(random, qualityRandomResult);
        if (quality == null)
        {
            USLog.error(_m_comp.getUSServer(), "AvatarGachaPoolInfo normalRoll randomQuality failed poolId:{} qualityWeightList:{}", getPoolId(), qualityWeightList);
            return null;
        }

        //随机卡牌
        //构造同品质卡牌的权重列表
        WeightValueList<RefGachaItem> weightList = new WeightValueList<>();
        for (RefGachaItem itemRef : _m_refPool.itemRefList)
        {
            if (itemRef.getQuality() != quality)
                continue;

            //需要判断是否满足抽取条件
            if (!NPPlayerConditionDealerMgr.IsEnable(itemRef.roll_condition, _m_comp.getUserData(), null))
                continue;

            //判断额外权重条件
            boolean needAddExtra = NPPlayerConditionDealerMgr.IsEnable(itemRef.extra_weight_condition, _m_comp.getUserData(), null);
            //计算总权重
            int totalWeight = itemRef.weight + (needAddExtra ? itemRef.extra_weight : 0);
            //添加到权重列表
            weightList.add(itemRef, totalWeight);
        }

        //随机一个卡牌
        RefWrap<Long> itemRandomResult = new RefWrap<>(0L);
        RefGachaItem itemRef = weightList.random(random, itemRandomResult);
        if (itemRef == null)
        {
            USLog.error(_m_comp.getUSServer(), "AvatarGachaPoolInfo normalRoll build quality weightList failed poolId:{} quality:{}", getPoolId(), quality);
            return null;
        }

        if (_randomResult != null)
        {
            _randomResult.setFirst(qualityRandomResult.get());
            _randomResult.setSecond(itemRandomResult.get());
        }

        return itemRef;
    }

    /**
     * 获取权重信息
     * @return
     */
    public String getWeightInfo()
    {
        return _m_weightInfo.getWeightInfo();
    }

    /**
     * 获取所有卡牌权重信息
     * @return
     */
    public String getAllItemWeightInfo()
    {
        StringBuilder sb = new StringBuilder();
        for (RefGachaItem itemRef : _m_refPool.itemRefList)
        {
            //判断额外权重条件
            boolean needAddExtra = NPPlayerConditionDealerMgr.IsEnable(itemRef.extra_weight_condition, _m_comp.getUserData(), null);
            //计算总权重
            int totalWeight = itemRef.weight + (needAddExtra ? itemRef.extra_weight : 0);

            sb.append(itemRef.id).append("-").append(itemRef.quality).append("-").append(totalWeight).append("\n");
        }
        return sb.toString();
    }

    /**
     * 领取累计奖励
     * @return
     */
    public Result drawCumulativeReward(NPPlayerContext context)
    {
        //判断是否达到要求
        int canDrawNum = _m_bo.getCumulativeRewardPoint() / _m_refPool.cumulative_reward_need_num;
        if (canDrawNum <= 0)
            return PlayerErr.GACHA_CUMULATIVE_REWARD_POINT_NOT_ENOUGH;

        //扣去累计奖励点数
        _m_bo.saveCumulativeRewardPoint(_m_comp.getUSServer().getBM(), _m_bo.getCumulativeRewardPoint() - _m_refPool.cumulative_reward_need_num * canDrawNum);

        //发放奖励
        _m_comp.getUserData().gainItem(_m_refPool.cumulative_reward_item.getItemType(), _m_refPool.cumulative_reward_item.getItemId(),
                _m_refPool.cumulative_reward_item.getCount() * canDrawNum, context);

        //推送变动
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_074_OnGachaPoolChg(makeProto()));

        return Result.SUCC;
    }

    /**
     * 增加累计奖励点数
     * @param _rollTimes
     */
    public void incCumulativeRewardPoint(int _rollTimes)
    {
        _m_bo.saveCumulativeRewardPoint(_m_comp.getUSServer().getBM(), _m_bo.getCumulativeRewardPoint() + _rollTimes);
        //此处不需要推送，因为在抽卡时会推送
    }

    @Override
    public String toString()
    {
        return "卡池id:" + getPoolId() + "\n" +
                "权重信息:" + _m_weightInfo.getWeightInfo() + "\n" +
                "保底待触发次数信息:" + _m_guaranteeInfo.getPrepareTriggerInfo() + "\n";
    }

    public Gacha_PoolInfo makeProto()
    {
        Gacha_PoolInfo info = new Gacha_PoolInfo();
        info.setPoolId(getPoolId());
        _m_guaranteeInfo.fillProto(info.getGuaranteeList());
        info.setCumulativeRewardTimes(_m_bo.getCumulativeRewardPoint());
        return info;
    }
}
