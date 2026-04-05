package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import Common.CachedObj.CachedObj_CachedGuildInfo;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import USDB.Bo.GuildBO;
import USDB.Bo.GuildMemberBO;

import java.util.ArrayList;
import java.util.Hashtable;

/**
 * 在player_cache表增加guild_info字段，在User记录玩家的公会id
 *
 * @author claude
 */
public class Update_1_0_1_7_To_1_0_1_8 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_7_To_1_0_1_8.run - start: begin player cache guild info update");

        // 更新player_cache表中的guild_info
        if (!updatePlayerCacheGuildInfo(_dbObj)) {
            CommLog.error("Update_1_0_1_7_To_1_0_1_8.run - player cache guild info update failed");
            return false;
        }

        CommLog.info("Update_1_0_1_7_To_1_0_1_8.run - complete: player cache guild info update success");
        return true;
    }

    /**
     * 更新玩家缓存数据中的公会信息
     *
     */
    private boolean updatePlayerCacheGuildInfo(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_7_To_1_0_1_8.updatePlayerCacheGuildInfo - start");

        //取出所有公会数据，
        SelectExceptionDealer guildBoErrDealer = new SelectExceptionDealer();
        GuildBO guildBo = new GuildBO();
        ArrayList<GuildBO> guildBoList
                = (ArrayList<GuildBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), guildBo, guildBoErrDealer);
        if (guildBoErrDealer.isHasException())
        {
            CommLog.error("Update_1_0_1_7_To_1_0_1_8.updatePlayerCacheGuildInfo - update failed for get all guildList");
            return false;
        }

        Hashtable<Long, CachedObj_CachedGuildInfo> guildTable = new Hashtable<Long, CachedObj_CachedGuildInfo>();
        for(GuildBO gBo : guildBoList)
        {
            //构造存储公会信息的结构体
            CachedObj_CachedGuildInfo guildInfo = new CachedObj_CachedGuildInfo();
            guildInfo.setGuildId(gBo.getGuildId());
            guildInfo.setGuildName(gBo.getName());
            guildInfo.setGuildSimpleName(gBo.getSimpleName());

            guildTable.put(gBo.getGuildId(), guildInfo);
        }

        //取出所有公会成员数据，用于更新player_guild表
        SelectExceptionDealer guildMemberBoErrDealer = new SelectExceptionDealer();
        GuildMemberBO guildMemberBo = new GuildMemberBO();
        ArrayList<GuildMemberBO> guildMemberBoList
                = (ArrayList<GuildMemberBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), guildMemberBo, guildMemberBoErrDealer);
        if (guildMemberBoErrDealer.isHasException())
        {
            CommLog.error("Update_1_0_1_7_To_1_0_1_8.updatePlayerCacheGuildInfo - update failed for get all memberList");
            return false;
        }

        //逐个遍历处理
        for(int i = 0; i < guildMemberBoList.size(); i++)
        {
            GuildMemberBO bo = guildMemberBoList.get(i);
            if(null == bo)
                continue;

            //查询联盟信息
            CachedObj_CachedGuildInfo guildInfo = guildTable.get(bo.getGuildId());
            if(null == guildInfo)
                continue;

            ALMySqlDBConditionObj cnd = new ALMySqlDBConditionObj();
            cnd.setTablesName("player_cache");
            cnd.addAndEquals("cid", bo.getCid());

            ArrayList<byte[]> byteList = new ArrayList<>();
            byteList.add(guildInfo.makePackage().array());

            //执行update
            int count = ALMySqlDBExcutor.updateByCondition(_dbObj, cnd, "guild_info=? ", byteList);
            if(count <= 0)
            {
                CommLog.error("Update_1_0_1_7_To_1_0_1_8.updatePlayerCacheGuildInfo - update failed: guild=" + bo.getGuildId() + ", cid=" + bo.getCid());
            }
            else {
                CommLog.error("update playerCacheGuildInfo suc: guild=" + bo.getGuildId() + ", cid=" + bo.getCid());
            }
        }

        CommLog.info("Update_1_0_1_7_To_1_0_1_8.updatePlayerCacheGuildInfo - complete: updated data count: " + guildMemberBoList.size());
        return true;
    }
}
