package USDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class PlayerDailyQuestActiveRewardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_active_reward_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "active_reward_id", comment = "活跃度refId")
    private long active_reward_id;

    public static final int FIELD_daily_quest_type =2;
    @DataBaseField(type = "int(11)", fieldname = "daily_quest_type", comment = "日常任务类型枚举")
    private int daily_quest_type;

    public static final int FIELD_fresh_serial =3;
    @DataBaseField(type = "bigint(20)", fieldname = "fresh_serial", comment = "刷新序列号")
    private long fresh_serial;

    public PlayerDailyQuestActiveRewardBO() {
        id = 0;
        cid = 0L;
        active_reward_id = 0L;
        daily_quest_type = 0;
        fresh_serial = 0L;
    }

    public PlayerDailyQuestActiveRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        active_reward_id = rs.getLong(3);
        daily_quest_type = rs.getInt(4);
        fresh_serial = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDailyQuestActiveRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `active_reward_id`, `daily_quest_type`, `fresh_serial`";
    }

    @Override
    public String getTableName() {
        return "`player_daily_quest_active_reward`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(active_reward_id).append("', ");
        strBuf.append("'").append(daily_quest_type).append("', ");
        strBuf.append("'").append(fresh_serial).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家CID
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 活跃度refId
    public long getActiveRewardId() { return this.active_reward_id; }
    public void setActiveRewardId(BM _bm, long active_reward_id) {
        if(active_reward_id==this.active_reward_id) 
            return;
        this.active_reward_id = active_reward_id; 
        markField(_bm, FIELD_active_reward_id); 
    }
    public void saveActiveRewardId(BM _bm, long active_reward_id) {
        if(active_reward_id==this.active_reward_id) 
            return;
        this.active_reward_id = active_reward_id;
        saveField(_bm, "active_reward_id", active_reward_id);
    }

    // 日常任务类型枚举
    public int getDailyQuestType() { return this.daily_quest_type; }
    public void setDailyQuestType(BM _bm, int daily_quest_type) {
        if(daily_quest_type==this.daily_quest_type) 
            return;
        this.daily_quest_type = daily_quest_type; 
        markField(_bm, FIELD_daily_quest_type); 
    }
    public void saveDailyQuestType(BM _bm, int daily_quest_type) {
        if(daily_quest_type==this.daily_quest_type) 
            return;
        this.daily_quest_type = daily_quest_type;
        saveField(_bm, "daily_quest_type", daily_quest_type);
    }

    // 刷新序列号
    public long getFreshSerial() { return this.fresh_serial; }
    public void setFreshSerial(BM _bm, long fresh_serial) {
        if(fresh_serial==this.fresh_serial) 
            return;
        this.fresh_serial = fresh_serial; 
        markField(_bm, FIELD_fresh_serial); 
    }
    public void saveFreshSerial(BM _bm, long fresh_serial) {
        if(fresh_serial==this.fresh_serial) 
            return;
        this.fresh_serial = fresh_serial;
        saveField(_bm, "fresh_serial", fresh_serial);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `active_reward_id` = '").append(active_reward_id).append("',");
        sBuilder.append(" `daily_quest_type` = '").append(daily_quest_type).append("',");
        sBuilder.append(" `fresh_serial` = '").append(fresh_serial).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_active_reward_id)) sBuilder.append(" `active_reward_id` = '").append(active_reward_id).append("',");
        if(isFieldMarked(FIELD_daily_quest_type)) sBuilder.append(" `daily_quest_type` = '").append(daily_quest_type).append("',");
        if(isFieldMarked(FIELD_fresh_serial)) sBuilder.append(" `fresh_serial` = '").append(fresh_serial).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_daily_quest_active_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`active_reward_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活跃度refId',"
                + "`daily_quest_type` int(11) NOT NULL DEFAULT '0' COMMENT '日常任务类型枚举',"
                + "`fresh_serial` bigint(20) NOT NULL DEFAULT '0' COMMENT '刷新序列号',"
                + "KEY `cid` (`cid`),"
                + "KEY `fresh_serial` (`fresh_serial`),"
                + "KEY `daily_quest_type` (`daily_quest_type`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家活跃奖励领取记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cid
        _size+=8;//active_reward_id
        _size+=4;//daily_quest_type
        _size+=8;//fresh_serial
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(active_reward_id);
        buff.putInt(daily_quest_type);
        buff.putLong(fresh_serial);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        active_reward_id=buff.getLong();
        daily_quest_type=buff.getInt();
        fresh_serial=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
