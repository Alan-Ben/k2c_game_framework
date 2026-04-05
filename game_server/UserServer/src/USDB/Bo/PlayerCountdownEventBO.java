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
public class PlayerCountdownEventBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_event_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "event_id", comment = "事件ID")
    private long event_id;

    public static final int FIELD_trigger_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "trigger_time_ms", comment = "触发时间")
    private long trigger_time_ms;

    public static final int FIELD_reset_count =3;
    @DataBaseField(type = "int(11)", fieldname = "reset_count", comment = "重置次数")
    private int reset_count;

    public PlayerCountdownEventBO() {
        id = 0;
        cid = 0L;
        event_id = 0L;
        trigger_time_ms = 0L;
        reset_count = 0;
    }

    public PlayerCountdownEventBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getLong(3);
        trigger_time_ms = rs.getLong(4);
        reset_count = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerCountdownEventBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `trigger_time_ms`, `reset_count`";
    }

    @Override
    public String getTableName() {
        return "`player_countdown_event`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(trigger_time_ms).append("', ");
        strBuf.append("'").append(reset_count).append("', ");
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

    // 事件ID
    public long getEventId() { return this.event_id; }
    public void setEventId(BM _bm, long event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, long event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 触发时间
    public long getTriggerTimeMs() { return this.trigger_time_ms; }
    public void setTriggerTimeMs(BM _bm, long trigger_time_ms) {
        if(trigger_time_ms==this.trigger_time_ms) 
            return;
        this.trigger_time_ms = trigger_time_ms; 
        markField(_bm, FIELD_trigger_time_ms); 
    }
    public void saveTriggerTimeMs(BM _bm, long trigger_time_ms) {
        if(trigger_time_ms==this.trigger_time_ms) 
            return;
        this.trigger_time_ms = trigger_time_ms;
        saveField(_bm, "trigger_time_ms", trigger_time_ms);
    }

    // 重置次数
    public int getResetCount() { return this.reset_count; }
    public void setResetCount(BM _bm, int reset_count) {
        if(reset_count==this.reset_count) 
            return;
        this.reset_count = reset_count; 
        markField(_bm, FIELD_reset_count); 
    }
    public void saveResetCount(BM _bm, int reset_count) {
        if(reset_count==this.reset_count) 
            return;
        this.reset_count = reset_count;
        saveField(_bm, "reset_count", reset_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `trigger_time_ms` = '").append(trigger_time_ms).append("',");
        sBuilder.append(" `reset_count` = '").append(reset_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_trigger_time_ms)) sBuilder.append(" `trigger_time_ms` = '").append(trigger_time_ms).append("',");
        if(isFieldMarked(FIELD_reset_count)) sBuilder.append(" `reset_count` = '").append(reset_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_countdown_event` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件ID',"
                + "`trigger_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '触发时间',"
                + "`reset_count` int(11) NOT NULL DEFAULT '0' COMMENT '重置次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家倒计时事件数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//event_id
        _size+=8;//trigger_time_ms
        _size+=4;//reset_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(event_id);
        buff.putLong(trigger_time_ms);
        buff.putInt(reset_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        event_id=buff.getLong();
        trigger_time_ms=buff.getLong();
        reset_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
