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
public class PlayerDailyQuestBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_quest_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_id", comment = "任务id")
    private long quest_id;

    public static final int FIELD_daily_quest_type =2;
    @DataBaseField(type = "int(11)", fieldname = "daily_quest_type", comment = "日常任务类型枚举")
    private int daily_quest_type;

    public static final int FIELD_fresh_serial =3;
    @DataBaseField(type = "bigint(20)", fieldname = "fresh_serial", comment = "刷新序列号")
    private long fresh_serial;

    public static final int FIELD_count =4;
    @DataBaseField(type = "bigint(20)", fieldname = "count", comment = "计数")
    private long count;

    public static final int FIELD_has_taken =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_taken", comment = "是否已领取奖励")
    private boolean has_taken;

    public static final int FIELD_is_random =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_random", comment = "是随机任务")
    private boolean is_random;

    public PlayerDailyQuestBO() {
        id = 0;
        cid = 0L;
        quest_id = 0L;
        daily_quest_type = 0;
        fresh_serial = 0L;
        count = 0L;
        has_taken = false;
        is_random = false;
    }

    public PlayerDailyQuestBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        quest_id = rs.getLong(3);
        daily_quest_type = rs.getInt(4);
        fresh_serial = rs.getLong(5);
        count = rs.getLong(6);
        has_taken = rs.getBoolean(7);
        is_random = rs.getBoolean(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDailyQuestBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `quest_id`, `daily_quest_type`, `fresh_serial`, `count`, `has_taken`, `is_random`";
    }

    @Override
    public String getTableName() {
        return "`player_daily_quest`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(quest_id).append("', ");
        strBuf.append("'").append(daily_quest_type).append("', ");
        strBuf.append("'").append(fresh_serial).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.append("'").append(has_taken ? 1 : 0).append("', ");
        strBuf.append("'").append(is_random ? 1 : 0).append("', ");
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

    // 任务id
    public long getQuestId() { return this.quest_id; }
    public void setQuestId(BM _bm, long quest_id) {
        if(quest_id==this.quest_id) 
            return;
        this.quest_id = quest_id; 
        markField(_bm, FIELD_quest_id); 
    }
    public void saveQuestId(BM _bm, long quest_id) {
        if(quest_id==this.quest_id) 
            return;
        this.quest_id = quest_id;
        saveField(_bm, "quest_id", quest_id);
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

    // 计数
    public long getCount() { return this.count; }
    public void setCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }

    // 是否已领取奖励
    public boolean getHasTaken() { return this.has_taken; }
    public void setHasTaken(BM _bm, boolean has_taken) {
        if(has_taken==this.has_taken) 
            return;
        this.has_taken = has_taken; 
        markField(_bm, FIELD_has_taken); 
    }
    public void saveHasTaken(BM _bm, boolean has_taken) {
        if(has_taken==this.has_taken) 
            return;
        this.has_taken = has_taken;
        saveField(_bm, "has_taken", has_taken ? 1 : 0);
    }

    // 是随机任务
    public boolean getIsRandom() { return this.is_random; }
    public void setIsRandom(BM _bm, boolean is_random) {
        if(is_random==this.is_random) 
            return;
        this.is_random = is_random; 
        markField(_bm, FIELD_is_random); 
    }
    public void saveIsRandom(BM _bm, boolean is_random) {
        if(is_random==this.is_random) 
            return;
        this.is_random = is_random;
        saveField(_bm, "is_random", is_random ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        sBuilder.append(" `daily_quest_type` = '").append(daily_quest_type).append("',");
        sBuilder.append(" `fresh_serial` = '").append(fresh_serial).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.append(" `has_taken` = '").append(has_taken ? 1 : 0).append("',");
        sBuilder.append(" `is_random` = '").append(is_random ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_quest_id)) sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        if(isFieldMarked(FIELD_daily_quest_type)) sBuilder.append(" `daily_quest_type` = '").append(daily_quest_type).append("',");
        if(isFieldMarked(FIELD_fresh_serial)) sBuilder.append(" `fresh_serial` = '").append(fresh_serial).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        if(isFieldMarked(FIELD_has_taken)) sBuilder.append(" `has_taken` = '").append(has_taken ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_is_random)) sBuilder.append(" `is_random` = '").append(is_random ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_daily_quest` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`quest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务id',"
                + "`daily_quest_type` int(11) NOT NULL DEFAULT '0' COMMENT '日常任务类型枚举',"
                + "`fresh_serial` bigint(20) NOT NULL DEFAULT '0' COMMENT '刷新序列号',"
                + "`count` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数',"
                + "`has_taken` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已领取奖励',"
                + "`is_random` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是随机任务',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家日常任务数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//quest_id
        _size+=4;//daily_quest_type
        _size+=8;//fresh_serial
        _size+=8;//count
        _size+=1;//has_taken
        _size+=1;//is_random
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(quest_id);
        buff.putInt(daily_quest_type);
        buff.putLong(fresh_serial);
        buff.putLong(count);
        buff.put((byte)(has_taken?1:0));
        buff.put((byte)(is_random?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        quest_id=buff.getLong();
        daily_quest_type=buff.getInt();
        fresh_serial=buff.getLong();
        count=buff.getLong();
        has_taken=(buff.get()==1);
        is_random=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
