package ActivitiesV01.Bo;
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
public class RegularActivityPlayerInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "活动实例ID")
    private long instance_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public static final int FIELD_next_refresh_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "next_refresh_time_ms", comment = "下次刷新时间")
    private long next_refresh_time_ms;

    public RegularActivityPlayerInfoBO() {
        id = 0;
        instance_id = 0L;
        cid = 0L;
        next_refresh_time_ms = 0L;
    }

    public RegularActivityPlayerInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instance_id = rs.getLong(2);
        cid = rs.getLong(3);
        next_refresh_time_ms = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RegularActivityPlayerInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instance_id`, `cid`, `next_refresh_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`regular_activity_player_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(next_refresh_time_ms).append("', ");
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

    // 活动实例ID
    public long getInstanceId() { return this.instance_id; }
    public void setInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id; 
        markField(_bm, FIELD_instance_id); 
    }
    public void saveInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id;
        saveField(_bm, "instance_id", instance_id);
    }

    // 玩家id
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

    // 下次刷新时间
    public long getNextRefreshTimeMs() { return this.next_refresh_time_ms; }
    public void setNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms; 
        markField(_bm, FIELD_next_refresh_time_ms); 
    }
    public void saveNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms;
        saveField(_bm, "next_refresh_time_ms", next_refresh_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_next_refresh_time_ms)) sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `regular_activity_player_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`next_refresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下次刷新时间',"
                + "KEY `instance_id` (`instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='万能活动玩家数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instance_id
        _size+=8;//cid
        _size+=8;//next_refresh_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instance_id);
        buff.putLong(cid);
        buff.putLong(next_refresh_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instance_id=buff.getLong();
        cid=buff.getLong();
        next_refresh_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
