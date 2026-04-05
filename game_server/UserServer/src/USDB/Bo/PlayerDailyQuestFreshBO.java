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
public class PlayerDailyQuestFreshBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_daily_quest_type =1;
    @DataBaseField(type = "int(11)", fieldname = "daily_quest_type", comment = "日常任务类型枚举")
    private int daily_quest_type;

    public static final int FIELD_fresh_serial =2;
    @DataBaseField(type = "bigint(20)", fieldname = "fresh_serial", comment = "刷新序列号")
    private long fresh_serial;

    public static final int FIELD_next_fresh_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "next_fresh_time_ms", comment = "下一次刷新时间戳")
    private long next_fresh_time_ms;

    public PlayerDailyQuestFreshBO() {
        id = 0;
        cid = 0L;
        daily_quest_type = 0;
        fresh_serial = 0L;
        next_fresh_time_ms = 0L;
    }

    public PlayerDailyQuestFreshBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        daily_quest_type = rs.getInt(3);
        fresh_serial = rs.getLong(4);
        next_fresh_time_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDailyQuestFreshBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `daily_quest_type`, `fresh_serial`, `next_fresh_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_daily_quest_fresh`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(daily_quest_type).append("', ");
        strBuf.append("'").append(fresh_serial).append("', ");
        strBuf.append("'").append(next_fresh_time_ms).append("', ");
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

    // 下一次刷新时间戳
    public long getNextFreshTimeMs() { return this.next_fresh_time_ms; }
    public void setNextFreshTimeMs(BM _bm, long next_fresh_time_ms) {
        if(next_fresh_time_ms==this.next_fresh_time_ms) 
            return;
        this.next_fresh_time_ms = next_fresh_time_ms; 
        markField(_bm, FIELD_next_fresh_time_ms); 
    }
    public void saveNextFreshTimeMs(BM _bm, long next_fresh_time_ms) {
        if(next_fresh_time_ms==this.next_fresh_time_ms) 
            return;
        this.next_fresh_time_ms = next_fresh_time_ms;
        saveField(_bm, "next_fresh_time_ms", next_fresh_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `daily_quest_type` = '").append(daily_quest_type).append("',");
        sBuilder.append(" `fresh_serial` = '").append(fresh_serial).append("',");
        sBuilder.append(" `next_fresh_time_ms` = '").append(next_fresh_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_daily_quest_type)) sBuilder.append(" `daily_quest_type` = '").append(daily_quest_type).append("',");
        if(isFieldMarked(FIELD_fresh_serial)) sBuilder.append(" `fresh_serial` = '").append(fresh_serial).append("',");
        if(isFieldMarked(FIELD_next_fresh_time_ms)) sBuilder.append(" `next_fresh_time_ms` = '").append(next_fresh_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_daily_quest_fresh` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`daily_quest_type` int(11) NOT NULL DEFAULT '0' COMMENT '日常任务类型枚举',"
                + "`fresh_serial` bigint(20) NOT NULL DEFAULT '0' COMMENT '刷新序列号',"
                + "`next_fresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下一次刷新时间戳',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家日常任务数据刷新组' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//daily_quest_type
        _size+=8;//fresh_serial
        _size+=8;//next_fresh_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(daily_quest_type);
        buff.putLong(fresh_serial);
        buff.putLong(next_fresh_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        daily_quest_type=buff.getInt();
        fresh_serial=buff.getLong();
        next_fresh_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
