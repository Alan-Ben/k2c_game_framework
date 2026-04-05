package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "爬塔相关命令", name = "tower")
public class CmdTower extends UsCmdBase
{
    @ACommand(comment = "爬塔信息")
    public String info()
    {
    	return getOwner().getTowerComponent().toString();
    }

    @ACommand(comment = "修改玩家位置")
    public String chgPos(long _chapterId, int _level)
    {
        return getOwner().getTowerComponent().chgPos(_chapterId, _level, getContext()).toString();
    }

    @ACommand(comment = "设置到达最高级别")
    public String setHighestPosHadReach(long _chapterId, int _level)
    {
        getOwner().getTowerComponent().setHighestPosHadReach(_chapterId, _level, getContext());

        return "ok";
    }

    @ACommand(comment = "设置激活研究进度")
    public String setActiveResearchPos(long _chapterId, int _level)
    {
        return getOwner().getTowerComponent().setActiveResearchPos(_chapterId, _level, getContext()).toString();
    }
}
