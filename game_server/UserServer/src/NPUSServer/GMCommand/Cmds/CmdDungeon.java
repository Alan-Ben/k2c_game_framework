package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Dungeon.Midday.RefMiddayDungeonBox;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp.PlayerFixedCD;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "副本相关命令", name = "dungeon")
public class CmdDungeon extends UsCmdBase
{
    @ACommand(comment = "清除午间大臣使用记录")
    public String cleanMiddayRecord()
    {
        getOwner().getMiddayDungeonComponent().getDungeonInfo().cleanRecord();
        //重置借用大臣次数
        PlayerFixedCD playerFixedCD = getOwner().getFixedCdComponent().lookupByRefId(RefGeneral.Ref().midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id);
        if (playerFixedCD != null)
            playerFixedCD.fill();

        return "ok";
    }

    @ACommand(comment = "修改午间boss时间[预告][开始][结束] 24小时制")
    public String chgMiddayBossTime(int _preview, int _start, int _end)
    {
        //限制时间是由小到大, 且在24小时制内
        if (_preview < 0 || _start < 0 || _end < 0 || _preview > _start || _start > _end || _end >= 24)
            return "fail: invalid time parameters";

        boolean isSucc = getUserServer().getMiddayDungeonMgr().cmdChgBossTime(_preview, _start, _end);
        return isSucc ? "ok" : "fail";
    }

    @ACommand(comment = "重置午间boss时间")
    public String resetMiddayBossTime()
    {
        return chgMiddayBossTime(0, 0, 0);
    }

    @ACommand(comment = "清除晚间大臣使用记录")
    public String cleanEveningRecord()
    {
        getOwner().getEveningDungeonComponent().getDungeonInfo().cleanRecord();
        return "ok";
    }

    @ACommand(comment = "设置晚间boss血量")
    public String setEveningBossBlood(long _blood)
    {
        getUserServer().getEveningDungeonMgr().getInfo().cmdChgBossBlood(_blood);
        return "ok";
    }

    @ACommand(comment = "马上复活晚间boss")
    public String rebornEveningBoss()
    {
        getUserServer().getEveningDungeonMgr().getInfo().cmdRebornBoss();
        return "ok";
    }

    @ACommand(comment = "重置晚间boss复活次数")
    public String resetEveningBossRebornTimes()
    {
        getUserServer().getEveningDungeonMgr().getInfo().cmdResetRebornTimes();
        return "ok";
    }

    @ACommand(comment = "修改晚间boss时间[预告][开始][结束][关闭] 24小时制")
    public String chgEveningBossTime(int _preview, int _start, int _end, int _close)
    {
        //限制时间是由小到大, 且在24小时制内
        if (_preview < 0 || _start < 0 || _end < 0 || _close < 0 || _preview > _start || _start > _end || _end > _close || _close >= 24)
            return "fail: invalid time parameters";

        boolean isSucc = getUserServer().getEveningDungeonMgr().getInfo().cmdChgBossTime(_preview, _start, _end, _close);
        return isSucc ? "ok" : "fail";
    }
    
    @ACommand(comment = "重置晚间boss时间")
    public String resetEveningBossTime()
    {
        return chgEveningBossTime(0, 0, 0, 0);
    }

    @ACommand(comment = "打印信息")
    public String info()
    {
        String middayDungeonInfo = getUserServer().getMiddayDungeonMgr().toString();
        String eveningDungeonInfo = getUserServer().getEveningDungeonMgr().toString();
        return middayDungeonInfo + "\n" + eveningDungeonInfo;
    }

    @ACommand(comment = "添加午间副本宝箱[宝箱配表ID]")
    public String addMiddayBox(int boxRefId)
    {
        // 获取宝箱配表对象
        RefMiddayDungeonBox ref = RefMiddayDungeonBox.getMgr().get(boxRefId);

        if (ref == null)
        {
            return "fail: box ref not found, boxRefId=" + boxRefId;
        }

        // 获取当前时间
        long currentTimeMs = System.currentTimeMillis();

        // 调用 addBox 方法
        try
        {
            getUserServer().getMiddayDungeonMgr().getBoxMgr().addBox(ref, getOwner(), currentTimeMs);
            return "ok: midday dungeon box added, boxRefId=" + boxRefId;
        }
        catch (Exception e)
        {
            return "fail: " + e.getMessage();
        }
    }

}
