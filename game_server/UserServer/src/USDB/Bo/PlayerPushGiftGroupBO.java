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
public class PlayerPushGiftGroupBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_group_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "礼包组id")
    private long group_id;

    public static final int FIELD_trigger_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "trigger_time_ms", comment = "触发时间毫秒")
    private long trigger_time_ms;

    public static final int FIELD_push_gift_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "push_gift_id", comment = "推送礼包id")
    private long push_gift_id;

    public static final int FIELD_active_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "active_time_ms", comment = "激活时间毫秒")
    private long active_time_ms;

    public static final int FIELD_has_read =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_read", comment = "是否已读")
    private boolean has_read;

    public PlayerPushGiftGroupBO() {
        id = 0;
        cid = 0L;
        group_id = 0L;
        trigger_time_ms = 0L;
        push_gift_id = 0L;
        active_time_ms = 0L;
        has_read = false;
    }

    public PlayerPushGiftGroupBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        group_id = rs.getLong(3);
        trigger_time_ms = rs.getLong(4);
        push_gift_id = rs.getLong(5);
        active_time_ms = rs.getLong(6);
        has_read = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerPushGiftGroupBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `group_id`, `trigger_time_ms`, `push_gift_id`, `active_time_ms`, `has_read`";
    }

    @Override
    public String getTableName() {
        return "`player_push_gift_group`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(trigger_time_ms).append("', ");
        strBuf.append("'").append(push_gift_id).append("', ");
        strBuf.append("'").append(active_time_ms).append("', ");
        strBuf.append("'").append(has_read ? 1 : 0).append("', ");
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

    // 礼包组id
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

    // 触发时间毫秒
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

    // 推送礼包id
    public long getPushGiftId() { return this.push_gift_id; }
    public void setPushGiftId(BM _bm, long push_gift_id) {
        if(push_gift_id==this.push_gift_id) 
            return;
        this.push_gift_id = push_gift_id; 
        markField(_bm, FIELD_push_gift_id); 
    }
    public void savePushGiftId(BM _bm, long push_gift_id) {
        if(push_gift_id==this.push_gift_id) 
            return;
        this.push_gift_id = push_gift_id;
        saveField(_bm, "push_gift_id", push_gift_id);
    }

    // 激活时间毫秒
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

    // 是否已读
    public boolean getHasRead() { return this.has_read; }
    public void setHasRead(BM _bm, boolean has_read) {
        if(has_read==this.has_read) 
            return;
        this.has_read = has_read; 
        markField(_bm, FIELD_has_read); 
    }
    public void saveHasRead(BM _bm, boolean has_read) {
        if(has_read==this.has_read) 
            return;
        this.has_read = has_read;
        saveField(_bm, "has_read", has_read ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `trigger_time_ms` = '").append(trigger_time_ms).append("',");
        sBuilder.append(" `push_gift_id` = '").append(push_gift_id).append("',");
        sBuilder.append(" `active_time_ms` = '").append(active_time_ms).append("',");
        sBuilder.append(" `has_read` = '").append(has_read ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_trigger_time_ms)) sBuilder.append(" `trigger_time_ms` = '").append(trigger_time_ms).append("',");
        if(isFieldMarked(FIELD_push_gift_id)) sBuilder.append(" `push_gift_id` = '").append(push_gift_id).append("',");
        if(isFieldMarked(FIELD_active_time_ms)) sBuilder.append(" `active_time_ms` = '").append(active_time_ms).append("',");
        if(isFieldMarked(FIELD_has_read)) sBuilder.append(" `has_read` = '").append(has_read ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_push_gift_group` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包组id',"
                + "`trigger_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '触发时间毫秒',"
                + "`push_gift_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '推送礼包id',"
                + "`active_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '激活时间毫秒',"
                + "`has_read` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已读',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家推送礼包组状态' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//trigger_time_ms
        _size+=8;//push_gift_id
        _size+=8;//active_time_ms
        _size+=1;//has_read
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
        buff.putLong(trigger_time_ms);
        buff.putLong(push_gift_id);
        buff.putLong(active_time_ms);
        buff.put((byte)(has_read?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        group_id=buff.getLong();
        trigger_time_ms=buff.getLong();
        push_gift_id=buff.getLong();
        active_time_ms=buff.getLong();
        has_read=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
