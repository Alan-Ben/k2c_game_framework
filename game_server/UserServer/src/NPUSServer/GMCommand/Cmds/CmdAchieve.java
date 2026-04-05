package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.AchieveComp.AchieveInfo;
import NPUSServer.NPUSUserMgr.UserComp.AchieveComp.AchievePointInfo;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "achieve相关命令", name = "achieve")
public class CmdAchieve extends UsCmdBase
{
    @ACommand(comment = "设置成就计数(成就ID，成就计数)")
    public String setCounter(long _achieveId, long _counter)
    {
        AchieveInfo info = getOwner().getAchieveComponent().lookupAchieve(_achieveId);
        if (null == info)
            return "fail, not find achieve";

        info.setCount(_counter, getContext());
        return "ok";
    }

    @ACommand(comment = "领取成就阶段奖励(成就ID，阶段)")
    public String drawStep(long _achieveId, int _step)
    {
        return getOwner().getAchieveComponent().drawAchieveStepReward(_achieveId, _step, getContext()).toString();
    }

    @ACommand(comment = "重置成就(成就ID)")
    public String resetAchieve(long _achieveId)
    {
        return getOwner().getAchieveComponent().resetAchieve(_achieveId) ? "ok" : "achieve not found";
    }

    @ACommand(comment = "领取成就点阶段奖励(成就点阶段ID)")
    public String drawPointStep(long _pointStepId)
    {
        return getOwner().getAchieveComponent().drawAchievePointStepReward(_pointStepId, getContext()).toString();
    }

    //增加成就点
    @ACommand(comment = "增加成就点(成就点类型id，成就点数量)")
    public String addPoint(int _type, long _count)
    {
        AchievePointInfo achievePointInfo = getOwner().getAchieveComponent().lookupAchievePoint(_type);
        if (achievePointInfo == null)
            return "point not found";
        achievePointInfo.addCount(_count, getContext());
        return "ok";
    }

    //减少成就点
    @ACommand(comment = "减少成就点(成就点类型id，成就点数量)")
    public String subPoint(int _type, long _count)
    {
        AchievePointInfo achievePointInfo = getOwner().getAchieveComponent().lookupAchievePoint(_type);
        if (achievePointInfo == null)
            return "point not found";
        achievePointInfo.reduceCount(_count, getContext());
        return "ok";
    }

    //重置成就点领取记录
    @ACommand(comment = "重置成就点领取记录(成就点类型id)")
    public String resetPoint(int _type)
    {
        AchievePointInfo achievePointInfo = getOwner().getAchieveComponent().lookupAchievePoint(_type);
        if (achievePointInfo == null)
            return "point not found";
        achievePointInfo.reset();
        return "ok";
    }
}
