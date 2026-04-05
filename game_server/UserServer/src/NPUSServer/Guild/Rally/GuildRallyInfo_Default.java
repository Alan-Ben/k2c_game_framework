package NPUSServer.Guild.Rally;

import Common.Common_Long;
import NPCommon.Log.CommLog;
import USDB.Bo.GuildRallyMainBO;

import java.nio.ByteBuffer;

/**
 * 默认集结类型数据
 *
 * 主要职责：
 * 1. 承接默认集结类型(0)的数据结构
 * 2. 在集结到期时执行默认处理逻辑
 */
public class GuildRallyInfo_Default extends GuildRallyInfo<Common_Long> {

    /**
     * 构造函数
     * @param _rallyMgr 所属集结管理器
     * @param _bo 集结主表BO
     */
    public GuildRallyInfo_Default(GuildRallyMgr _rallyMgr, GuildRallyMainBO _bo) {
        super(_rallyMgr, _bo);
    }

    /**
     * 读取默认集结扩展数据
     */
    @Override
    protected Common_Long _readTxtData(byte[] _data) {
        Common_Long dataObj = new Common_Long(0);
        if (_data == null || _data.length == 0) {
            return dataObj;
        }

        try {
            dataObj.readPackage(ByteBuffer.wrap(_data));
        } catch (Exception e) {
            CommLog.error("GuildRallyInfo_Default.readTxtData - parse ext data failed, guildId={}, rallyId={}, err={}",
                    _getRallyMgr().getGuildInfo().getGuildId(), getRallyId(), e.getMessage());
        }

        return dataObj;
    }

    /**
     * 默认集结到期处理
     * 当前逻辑：先清理成员表，再清理主表，避免遗留脏数据
     */
    @Override
    public void dealRally() {
        try {
            for (RallyMemberInfo memberInfo : cpyMemberInfoList()) {
                if (memberInfo == null) {
                    continue;
                }
                memberInfo.del(_getBM());
            }

            //TODO：遣返所有成员对象
        } catch (Exception e) {
            CommLog.error("GuildRallyInfo_Default.dealRally - deal failed, guildId={}, rallyId={}, err={}",
                    _getRallyMgr().getGuildInfo().getGuildId(), getRallyId(), e.getMessage());
        }
    }
}


