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
public class PlayerEventRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_record_type_id =1;
    @DataBaseField(type = "int(11)", fieldname = "record_type_id", comment = "记录类型id")
    private int record_type_id;

    public static final int FIELD_record_sub_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "record_sub_id", comment = "类型细分id")
    private long record_sub_id;

    public static final int FIELD_count =3;
    @DataBaseField(type = "bigint(20)", fieldname = "count", comment = "计数")
    private long count;

    public PlayerEventRecordBO() {
        id = 0;
        cid = 0L;
        record_type_id = 0;
        record_sub_id = 0L;
        count = 0L;
    }

    public PlayerEventRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        record_type_id = rs.getInt(3);
        record_sub_id = rs.getLong(4);
        count = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerEventRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `record_type_id`, `record_sub_id`, `count`";
    }

    @Override
    public String getTableName() {
        return "`player_event_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(record_type_id).append("', ");
        strBuf.append("'").append(record_sub_id).append("', ");
        strBuf.append("'").append(count).append("', ");
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

    // 记录类型id
    public int getRecordTypeId() { return this.record_type_id; }
    public void setRecordTypeId(BM _bm, int record_type_id) {
        if(record_type_id==this.record_type_id) 
            return;
        this.record_type_id = record_type_id; 
        markField(_bm, FIELD_record_type_id); 
    }
    public void saveRecordTypeId(BM _bm, int record_type_id) {
        if(record_type_id==this.record_type_id) 
            return;
        this.record_type_id = record_type_id;
        saveField(_bm, "record_type_id", record_type_id);
    }

    // 类型细分id
    public long getRecordSubId() { return this.record_sub_id; }
    public void setRecordSubId(BM _bm, long record_sub_id) {
        if(record_sub_id==this.record_sub_id) 
            return;
        this.record_sub_id = record_sub_id; 
        markField(_bm, FIELD_record_sub_id); 
    }
    public void saveRecordSubId(BM _bm, long record_sub_id) {
        if(record_sub_id==this.record_sub_id) 
            return;
        this.record_sub_id = record_sub_id;
        saveField(_bm, "record_sub_id", record_sub_id);
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `record_type_id` = '").append(record_type_id).append("',");
        sBuilder.append(" `record_sub_id` = '").append(record_sub_id).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_record_type_id)) sBuilder.append(" `record_type_id` = '").append(record_type_id).append("',");
        if(isFieldMarked(FIELD_record_sub_id)) sBuilder.append(" `record_sub_id` = '").append(record_sub_id).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_event_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`record_type_id` int(11) NOT NULL DEFAULT '0' COMMENT '记录类型id',"
                + "`record_sub_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '类型细分id',"
                + "`count` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User 玩家事件计数' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//record_type_id
        _size+=8;//record_sub_id
        _size+=8;//count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(record_type_id);
        buff.putLong(record_sub_id);
        buff.putLong(count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        record_type_id=buff.getInt();
        record_sub_id=buff.getLong();
        count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
