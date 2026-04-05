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
public class PlayerChapterEventBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_event_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "event_id", comment = "事件id")
    private long event_id;

    public static final int FIELD_extra_data =2;
    @DataBaseField(type = "blob", fieldname = "extra_data", comment = "额外数据")
    private byte[] extra_data;

    public PlayerChapterEventBO() {
        id = 0;
        cid = 0L;
        event_id = 0L;
        extra_data = null;
    }

    public PlayerChapterEventBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getLong(3);
        extra_data = rs.getBytes(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerChapterEventBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `extra_data`";
    }

    @Override
    public String getTableName() {
        return "`player_chapter_event`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(extra_data);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_extra_data)) ret.add(extra_data);         return ret;
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

    // 事件id
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

    // 额外数据
    public byte[] getExtraData() { return this.extra_data; }
    public void setExtraData(BM _bm, byte[] extra_data) {
        if(extra_data==this.extra_data) 
            return;
        this.extra_data = extra_data; 
        markField(_bm, FIELD_extra_data); 
    }
    public void saveExtraData(BM _bm, byte[] extra_data) {
        if(extra_data==this.extra_data) 
            return;
        this.extra_data = extra_data;
        saveFieldBytes(_bm, "extra_data", extra_data);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `extra_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_extra_data)) sBuilder.append(" `extra_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_chapter_event` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件id',"
                + "`extra_data` blob NULL COMMENT '额外数据',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家关卡事件数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=2;_size+=extra_data.length;//extra_data
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
        buff.putShort((short)(extra_data == null ? 0 : extra_data.length));if(null != extra_data){buff.put(extra_data);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        event_id=buff.getLong();
        int extra_data_count = buff.getShort();if(extra_data_count>0){extra_data = new byte[extra_data_count];buff.get(extra_data);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
