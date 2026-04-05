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
public class PlayerRushExchangeBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_group_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "礼包组ID")
    private long group_id;

    public static final int FIELD_ref_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "当前礼包配置ID")
    private long ref_id;

    public static final int FIELD_active_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "active_time_ms", comment = "当前兑换开始时间 如果没兑换影响刷新礼包时间")
    private long active_time_ms;

    public static final int FIELD_exchange_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "exchange_time_ms", comment = "兑换时间 影响什么时候可以领奖")
    private long exchange_time_ms;

    public static final int FIELD_is_rewarded =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_rewarded", comment = "是否已领奖")
    private boolean is_rewarded;

    public static final int FIELD_today_exchange_count =6;
    @DataBaseField(type = "int(11)", fieldname = "today_exchange_count", comment = "当天兑换次数")
    private int today_exchange_count;

    public static final int FIELD_next_reset_count_time_ms =7;
    @DataBaseField(type = "bigint(20)", fieldname = "next_reset_count_time_ms", comment = "下次刷新兑换次数时间")
    private long next_reset_count_time_ms;

    public static final int FIELD_used_ref_ids =8;
    @DataBaseField(type = "varchar(2048)", fieldname = "used_ref_ids", comment = "已使用的礼包ID列表（分号分隔，用于不放回抽取）")
    private String used_ref_ids;

    public PlayerRushExchangeBO() {
        id = 0;
        cid = 0L;
        group_id = 0L;
        ref_id = 0L;
        active_time_ms = 0L;
        exchange_time_ms = 0L;
        is_rewarded = false;
        today_exchange_count = 0;
        next_reset_count_time_ms = 0L;
        used_ref_ids = "";
    }

    public PlayerRushExchangeBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        group_id = rs.getLong(3);
        ref_id = rs.getLong(4);
        active_time_ms = rs.getLong(5);
        exchange_time_ms = rs.getLong(6);
        is_rewarded = rs.getBoolean(7);
        today_exchange_count = rs.getInt(8);
        next_reset_count_time_ms = rs.getLong(9);
        used_ref_ids = rs.getString(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerRushExchangeBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `group_id`, `ref_id`, `active_time_ms`, `exchange_time_ms`, `is_rewarded`, `today_exchange_count`, `next_reset_count_time_ms`, `used_ref_ids`";
    }

    @Override
    public String getTableName() {
        return "`player_rush_exchange`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(active_time_ms).append("', ");
        strBuf.append("'").append(exchange_time_ms).append("', ");
        strBuf.append("'").append(is_rewarded ? 1 : 0).append("', ");
        strBuf.append("'").append(today_exchange_count).append("', ");
        strBuf.append("'").append(next_reset_count_time_ms).append("', ");
        strBuf.append("'").append(used_ref_ids == null ? null : used_ref_ids.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 礼包组ID
    public long getGroupId() { return this.group_id; }
    public void setGroupId(BM _bm, long group_id) {
        if(group_id==this.group_id) 
            return;
        this.group_id = group_id; 
        markField(_bm, FIELD_group_id); 
    }
    public void saveGroupId(BM _bm, long group_id) {
        if(group_id==this.group_id) 
            return;
        this.group_id = group_id;
        saveField(_bm, "group_id", group_id);
    }

    // 当前礼包配置ID
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 当前兑换开始时间 如果没兑换影响刷新礼包时间
    public long getActiveTimeMs() { return this.active_time_ms; }
    public void setActiveTimeMs(BM _bm, long active_time_ms) {
        if(active_time_ms==this.active_time_ms) 
            return;
        this.active_time_ms = active_time_ms; 
        markField(_bm, FIELD_active_time_ms); 
    }
    public void saveActiveTimeMs(BM _bm, long active_time_ms) {
        if(active_time_ms==this.active_time_ms) 
            return;
        this.active_time_ms = active_time_ms;
        saveField(_bm, "active_time_ms", active_time_ms);
    }

    // 兑换时间 影响什么时候可以领奖
    public long getExchangeTimeMs() { return this.exchange_time_ms; }
    public void setExchangeTimeMs(BM _bm, long exchange_time_ms) {
        if(exchange_time_ms==this.exchange_time_ms) 
            return;
        this.exchange_time_ms = exchange_time_ms; 
        markField(_bm, FIELD_exchange_time_ms); 
    }
    public void saveExchangeTimeMs(BM _bm, long exchange_time_ms) {
        if(exchange_time_ms==this.exchange_time_ms) 
            return;
        this.exchange_time_ms = exchange_time_ms;
        saveField(_bm, "exchange_time_ms", exchange_time_ms);
    }

    // 是否已领奖
    public boolean getIsRewarded() { return this.is_rewarded; }
    public void setIsRewarded(BM _bm, boolean is_rewarded) {
        if(is_rewarded==this.is_rewarded) 
            return;
        this.is_rewarded = is_rewarded; 
        markField(_bm, FIELD_is_rewarded); 
    }
    public void saveIsRewarded(BM _bm, boolean is_rewarded) {
        if(is_rewarded==this.is_rewarded) 
            return;
        this.is_rewarded = is_rewarded;
        saveField(_bm, "is_rewarded", is_rewarded ? 1 : 0);
    }

    // 当天兑换次数
    public int getTodayExchangeCount() { return this.today_exchange_count; }
    public void setTodayExchangeCount(BM _bm, int today_exchange_count) {
        if(today_exchange_count==this.today_exchange_count) 
            return;
        this.today_exchange_count = today_exchange_count; 
        markField(_bm, FIELD_today_exchange_count); 
    }
    public void saveTodayExchangeCount(BM _bm, int today_exchange_count) {
        if(today_exchange_count==this.today_exchange_count) 
            return;
        this.today_exchange_count = today_exchange_count;
        saveField(_bm, "today_exchange_count", today_exchange_count);
    }

    // 下次刷新兑换次数时间
    public long getNextResetCountTimeMs() { return this.next_reset_count_time_ms; }
    public void setNextResetCountTimeMs(BM _bm, long next_reset_count_time_ms) {
        if(next_reset_count_time_ms==this.next_reset_count_time_ms) 
            return;
        this.next_reset_count_time_ms = next_reset_count_time_ms; 
        markField(_bm, FIELD_next_reset_count_time_ms); 
    }
    public void saveNextResetCountTimeMs(BM _bm, long next_reset_count_time_ms) {
        if(next_reset_count_time_ms==this.next_reset_count_time_ms) 
            return;
        this.next_reset_count_time_ms = next_reset_count_time_ms;
        saveField(_bm, "next_reset_count_time_ms", next_reset_count_time_ms);
    }

    // 已使用的礼包ID列表（分号分隔，用于不放回抽取）
    public String getUsedRefIds() { return this.used_ref_ids; }
    public void setUsedRefIds(BM _bm, String used_ref_ids) {
        if(used_ref_ids.equals(this.used_ref_ids)) 
            return;
        this.used_ref_ids = used_ref_ids; 
        markField(_bm, FIELD_used_ref_ids); 
    }
    public void saveUsedRefIds(BM _bm, String used_ref_ids) {
        if(used_ref_ids.equals(this.used_ref_ids)) 
            return;
        this.used_ref_ids = used_ref_ids;
        saveField(_bm, "used_ref_ids", used_ref_ids);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `active_time_ms` = '").append(active_time_ms).append("',");
        sBuilder.append(" `exchange_time_ms` = '").append(exchange_time_ms).append("',");
        sBuilder.append(" `is_rewarded` = '").append(is_rewarded ? 1 : 0).append("',");
        sBuilder.append(" `today_exchange_count` = '").append(today_exchange_count).append("',");
        sBuilder.append(" `next_reset_count_time_ms` = '").append(next_reset_count_time_ms).append("',");
        sBuilder.append(" `used_ref_ids` = '").append(used_ref_ids == null ? null : used_ref_ids.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_active_time_ms)) sBuilder.append(" `active_time_ms` = '").append(active_time_ms).append("',");
        if(isFieldMarked(FIELD_exchange_time_ms)) sBuilder.append(" `exchange_time_ms` = '").append(exchange_time_ms).append("',");
        if(isFieldMarked(FIELD_is_rewarded)) sBuilder.append(" `is_rewarded` = '").append(is_rewarded ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_today_exchange_count)) sBuilder.append(" `today_exchange_count` = '").append(today_exchange_count).append("',");
        if(isFieldMarked(FIELD_next_reset_count_time_ms)) sBuilder.append(" `next_reset_count_time_ms` = '").append(next_reset_count_time_ms).append("',");
        if(isFieldMarked(FIELD_used_ref_ids)) sBuilder.append(" `used_ref_ids` = '").append(used_ref_ids == null ? null : used_ref_ids.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_rush_exchange` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包组ID',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前礼包配置ID',"
                + "`active_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前兑换开始时间 如果没兑换影响刷新礼包时间',"
                + "`exchange_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '兑换时间 影响什么时候可以领奖',"
                + "`is_rewarded` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已领奖',"
                + "`today_exchange_count` int(11) NOT NULL DEFAULT '0' COMMENT '当天兑换次数',"
                + "`next_reset_count_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下次刷新兑换次数时间',"
                + "`used_ref_ids` varchar(2048) NOT NULL DEFAULT '' COMMENT '已使用的礼包ID列表（分号分隔，用于不放回抽取）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家急速兑换数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//group_id
        _size+=8;//ref_id
        _size+=8;//active_time_ms
        _size+=8;//exchange_time_ms
        _size+=1;//is_rewarded
        _size+=4;//today_exchange_count
        _size+=8;//next_reset_count_time_ms
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(used_ref_ids);//used_ref_ids
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(group_id);
        buff.putLong(ref_id);
        buff.putLong(active_time_ms);
        buff.putLong(exchange_time_ms);
        buff.put((byte)(is_rewarded?1:0));
        buff.putInt(today_exchange_count);
        buff.putLong(next_reset_count_time_ms);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, used_ref_ids);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        group_id=buff.getLong();
        ref_id=buff.getLong();
        active_time_ms=buff.getLong();
        exchange_time_ms=buff.getLong();
        is_rewarded=(buff.get()==1);
        today_exchange_count=buff.getInt();
        next_reset_count_time_ms=buff.getLong();
        used_ref_ids=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
