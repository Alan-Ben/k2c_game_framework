package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import USDB.Bo.GuildHeroDispatchBO;
import USDB.Bo.GuildMemberBO;

import java.util.ArrayList;

/**
 * 在player_guild表增加guild_id字段，在User记录玩家的公会id
 *
 * @author claude
 */
public class Update_1_0_1_5_To_1_0_1_6 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_5_To_1_0_1_6.run - start: begin player guild id update");

        // 更新player_guild表中的guild_id
        if (!updatePlayerGuildId(_dbObj)) {
            CommLog.error("Update_1_0_1_5_To_1_0_1_6.run - player guild id update failed");
            return false;
        }

        // 更新player_guild表中的dispatch_hero_id
        if (!updatePlayerHeroDispatch(_dbObj)) {
            CommLog.error("Update_1_0_1_5_To_1_0_1_6.run - player dispatch hero id update failed");
            return false;
        }

        CommLog.info("Update_1_0_1_5_To_1_0_1_6.run - complete: player guild id update success");
        return true;
    }

    /**
     * 更新大臣天赋技能ID
     *
     * 执行流程：
     * 1. 查找 hero_id=1307 且 skill_id=133 的记录
     * 2. 将 skill_id 更新为 143
     * 3. 记录更新日志
     */
    private boolean updatePlayerGuildId(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_5_To_1_0_1_6.updatePlayerGuildId - start");

        //取出所有公会成员数据，用于更新player_guild表
        SelectExceptionDealer guildMemberBoErrDealer = new SelectExceptionDealer();
        GuildMemberBO guildMemberBo = new GuildMemberBO();
        ArrayList<GuildMemberBO> guildMemberBoList
                = (ArrayList<GuildMemberBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), guildMemberBo, guildMemberBoErrDealer);
        if (guildMemberBoErrDealer.isHasException())
        {
            CommLog.error("Update_1_0_1_5_To_1_0_1_6.updatePlayerGuildId - update failed for get all memberList");
            return false;
        }

        //逐个遍历处理
        for(int i = 0; i < guildMemberBoList.size(); i++)
        {
            GuildMemberBO bo = guildMemberBoList.get(i);
            if(null == bo)
                continue;

            //修正玩家数据
            String sql = "UPDATE `player_guild` SET `guild_id` = '" + bo.getGuildId() + "' WHERE `cid` = '"+ bo.getCid() + "';";

            if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
                CommLog.error("Update_1_0_1_5_To_1_0_1_6.updatePlayerGuildId - update failed: guild=" + bo.getGuildId() + ", cid=" + bo.getCid());
            }
            else {
                CommLog.error("update suc: guild=" + bo.getGuildId() + ", cid=" + bo.getCid());
            }
        }

        CommLog.info("Update_1_0_1_5_To_1_0_1_6.updatePlayerGuildId - complete: updated data count: " + guildMemberBoList.size());
        return true;
    }

    private boolean updatePlayerHeroDispatch(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_5_To_1_0_1_6.updatePlayerHeroDispatch - start");

        //取出所有公会成员数据，用于更新player_guild表
        SelectExceptionDealer guildDispatchHeroBoErrDealer = new SelectExceptionDealer();
        GuildHeroDispatchBO guildHeroDispatchBo = new GuildHeroDispatchBO();
        ArrayList<GuildHeroDispatchBO> guildHeroDispatchBoList
                = (ArrayList<GuildHeroDispatchBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), guildHeroDispatchBo, guildDispatchHeroBoErrDealer);
        if (guildDispatchHeroBoErrDealer.isHasException())
        {
            CommLog.error("Update_1_0_1_5_To_1_0_1_6.updatePlayerHeroDispatch - update failed for get all dispatchHeroList");
            return false;
        }

        //逐个遍历处理
        for(int i = 0; i < guildHeroDispatchBoList.size(); i++)
        {
            GuildHeroDispatchBO bo = guildHeroDispatchBoList.get(i);
            if(null == bo)
                continue;

            //修正玩家数据
            String sql = "UPDATE `player_guild` SET `dispatch_hero_id` = '" + bo.getHeroId() + "' WHERE `cid` = '"+ bo.getCid() + "';";

            if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
                CommLog.error("Update_1_0_1_5_To_1_0_1_6.updatePlayerHeroDispatch - update failed: dispatch_hero_id=" + bo.getHeroId() + ", cid=" + bo.getCid());
            }
            else {
                CommLog.error("update suc: dispatch_hero_id=" + bo.getHeroId() + ", cid=" + bo.getCid());
            }
        }

        CommLog.info("Update_1_0_1_5_To_1_0_1_6.updatePlayerHeroDispatch - complete: updated data count: " + guildHeroDispatchBoList.size());
        return true;
    }
}
