package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.EQuality;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaPoolInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

@ACommander(comment = "抽卡命令", name = "gacha")
public class CmdGacha extends UsCmdBase
{
    @ACommand(comment = "增加累计抽卡次数[卡池id][次数]")
    public String incCumulativePoint(long _poolId, int _num)
    {
        RefGachaPool refPool = RefGachaPool.getMgr().get(_poolId);
        if (refPool == null)
            return "pool ref not found";

        GachaPoolInfo poolInfo = getOwner().getGachaComponent().ensurePool(refPool);
        if (poolInfo == null)
            return "pool not exist";

        poolInfo.incCumulativeRewardPoint(_num);

        getOwner().sendMsgToGC(US2GCWriter_007_CommOp.make_074_OnGachaPoolChg(poolInfo.makeProto()));

        return poolInfo.toString();
    }

    @ACommand(comment = "打印抽卡卡池信息[卡池id]")
    public String printGachaInfo(long _poolId)
    {
        RefGachaPool refPool = RefGachaPool.getMgr().get(_poolId);
        if (refPool == null)
            return "pool ref not found";

        GachaPoolInfo poolInfo = getOwner().getGachaComponent().ensurePool(refPool);
        if (poolInfo == null)
            return "pool not exist";

        return poolInfo.toString();
    }

    @ACommand(comment = "打印指定卡池卡牌权重信息[卡池id]")
    public String printGachaPoolItemInfo(long _poolId)
    {
        RefGachaPool refPool = RefGachaPool.getMgr().get(_poolId);
        if (refPool == null)
            return "pool ref not found";

        GachaPoolInfo poolInfo = getOwner().getGachaComponent().ensurePool(refPool);
        if (poolInfo == null)
            return "pool not exist";

        return poolInfo.getAllItemWeightInfo();
    }

    @ACommand(comment = "抽卡[卡池id][次数上限1000次]")
    public String rollAvatar(long _poolId, int _times)
    {
        RefGachaPool refPool = RefGachaPool.getMgr().get(_poolId);
        if (refPool == null)
            return "pool ref not found";

        GachaPoolInfo poolInfo = getOwner().getGachaComponent().ensurePool(refPool);
        if (poolInfo == null)
            return "pool not exist";

        int[] qualityNumCollect = new int[EQuality.values().length];

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _times; i++)
        {
            String oriWeightInfo = poolInfo.getWeightInfo();
            WCGPairLong randomResult = new WCGPairLong();
            RefGachaItem rollItem = poolInfo.roll(randomResult);
            //打印抽卡的结果，结构为：抽卡第几次-卡牌id-卡牌品质-权重信息，如果rollItem为空，则输出:抽卡第几次-空
            sb.append(i + 1).append("-").append(rollItem == null ? "空" : rollItem.id).append("-").append(rollItem == null ? "空" : rollItem.quality)
                    .append(" qualityValue:").append(rollItem == null ? "空" : randomResult.first()).append(" itemValue:").append(rollItem == null ? "空" : randomResult.second())
                    .append("\n").append(oriWeightInfo).append("\n");

            //累计品质数量
            if (rollItem != null)
                qualityNumCollect[rollItem.quality.ordinal()]++;
        }

        sb.append("品质数量统计:\n");
        for (int i = 0; i < qualityNumCollect.length; i++)
        {
            sb.append(EQuality.values()[i]).append(":").append(qualityNumCollect[i]).append(";");
        }

        return sb.toString();
    }
}
