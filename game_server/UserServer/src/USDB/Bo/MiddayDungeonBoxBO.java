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
public class MiddayDungeonBoxBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_box_ref_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "box_ref_id", comment = "宝箱ID")
    private long box_ref_id;

    public static final int FIELD_sender_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "sender_cid", comment = "发送者ID")
    private long sender_cid;

    public static final int FIELD_sender_name =2;
    @DataBaseField(type = "varchar(500)", fieldname = "sender_name", comment = "发送者名称")
    private String sender_name;

    public static final int FIELD_expired_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "expired_ms", comment = "过期时间")
    private long expired_ms;

    public static final int FIELD_had_draw_cid_list =4;
    @DataBaseField(type = "blob", fieldname = "had_draw_cid_list", comment = "已领取的玩家列表")
    private byte[] had_draw_cid_list;

    public MiddayDungeonBoxBO() {
        id = 0;
        box_ref_id = 0L;
        sender_cid = 0L;
        sender_name = "";
        expired_ms = 0L;
        had_draw_cid_list = null;
    }

    public MiddayDungeonBoxBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        box_ref_id = rs.getLong(2);
        sender_cid = rs.getLong(3);
        sender_name = rs.getString(4);
        expired_ms = rs.getLong(5);
        had_draw_cid_list = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MiddayDungeonBoxBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `box_ref_id`, `sender_cid`, `sender_name`, `expired_ms`, `had_draw_cid_list`";
    }

    @Override
    public String getTableName() {
        return "`midday_dungeon_box`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(box_ref_id).append("', ");
        strBuf.append("'").append(sender_cid).append("', ");
        strBuf.append("'").append(sender_name == null ? null : sender_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(expired_ms).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(had_draw_cid_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_had_draw_cid_list)) ret.add(had_draw_cid_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 宝箱ID
    public long getBoxRefId() { return this.box_ref_id; }
    public void setBoxRefId(BM _bm, long box_ref_id) {
        if(box_ref_id==this.box_ref_id) 
            return;
        this.box_ref_id = box_ref_id; 
        markField(_bm, FIELD_box_ref_id); 
    }
    public void saveBoxRefId(BM _bm, long box_ref_id) {
        if(box_ref_id==this.box_ref_id) 
            return;
        this.box_ref_id = box_ref_id;
        saveField(_bm, "box_ref_id", box_ref_id);
    }

    // 发送者ID
    public long getSenderCid() { return this.sender_cid; }
    public void setSenderCid(BM _bm, long sender_cid) {
        if(sender_cid==this.sender_cid) 
            return;
        this.sender_cid = sender_cid; 
        markField(_bm, FIELD_sender_cid); 
    }
    public void saveSenderCid(BM _bm, long sender_cid) {
        if(sender_cid==this.sender_cid) 
            return;
        this.sender_cid = sender_cid;
        saveField(_bm, "sender_cid", sender_cid);
    }

    // 发送者名称
    public String getSenderName() { return this.sender_name; }
    public void setSenderName(BM _bm, String sender_name) {
        if(sender_name.equals(this.sender_name)) 
            return;
        this.sender_name = sender_name; 
        markField(_bm, FIELD_sender_name); 
    }
    public void saveSenderName(BM _bm, String sender_name) {
        if(sender_name.equals(this.sender_name)) 
            return;
        this.sender_name = sender_name;
        saveField(_bm, "sender_name", sender_name);
    }

    // 过期时间
    public long getExpiredMs() { return this.expired_ms; }
    public void setExpiredMs(BM _bm, long expired_ms) {
        if(expired_ms==this.expired_ms) 
            return;
        this.expired_ms = expired_ms; 
        markField(_bm, FIELD_expired_ms); 
    }
    public void saveExpiredMs(BM _bm, long expired_ms) {
        if(expired_ms==this.expired_ms) 
            return;
        this.expired_ms = expired_ms;
        saveField(_bm, "expired_ms", expired_ms);
    }

    // 已领取的玩家列表
    public byte[] getHadDrawCidList() { return this.had_draw_cid_list; }
    public void setHadDrawCidList(BM _bm, byte[] had_draw_cid_list) {
        if(had_draw_cid_list==this.had_draw_cid_list) 
            return;
        this.had_draw_cid_list = had_draw_cid_list; 
        markField(_bm, FIELD_had_draw_cid_list); 
    }
    public void saveHadDrawCidList(BM _bm, byte[] had_draw_cid_list) {
        if(had_draw_cid_list==this.had_draw_cid_list) 
            return;
        this.had_draw_cid_list = had_draw_cid_list;
        saveFieldBytes(_bm, "had_draw_cid_list", had_draw_cid_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `box_ref_id` = '").append(box_ref_id).append("',");
        sBuilder.append(" `sender_cid` = '").append(sender_cid).append("',");
        sBuilder.append(" `sender_name` = '").append(sender_name == null ? null : sender_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `expired_ms` = '").append(expired_ms).append("',");
        sBuilder.append(" `had_draw_cid_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_box_ref_id)) sBuilder.append(" `box_ref_id` = '").append(box_ref_id).append("',");
        if(isFieldMarked(FIELD_sender_cid)) sBuilder.append(" `sender_cid` = '").append(sender_cid).append("',");
        if(isFieldMarked(FIELD_sender_name)) sBuilder.append(" `sender_name` = '").append(sender_name == null ? null : sender_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_expired_ms)) sBuilder.append(" `expired_ms` = '").append(expired_ms).append("',");
        if(isFieldMarked(FIELD_had_draw_cid_list)) sBuilder.append(" `had_draw_cid_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `midday_dungeon_box` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`box_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宝箱ID',"
                + "`sender_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发送者ID',"
                + "`sender_name` varchar(500) NOT NULL DEFAULT '' COMMENT '发送者名称',"
                + "`expired_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '过期时间',"
                + "`had_draw_cid_list` blob NULL COMMENT '已领取的玩家列表',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='副本宝箱信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//box_ref_id
        _size+=8;//sender_cid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sender_name);//sender_name
        _size+=8;//expired_ms
        _size+=2;_size+=had_draw_cid_list.length;//had_draw_cid_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(box_ref_id);
        buff.putLong(sender_cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sender_name);
        buff.putLong(expired_ms);
        buff.putShort((short)(had_draw_cid_list == null ? 0 : had_draw_cid_list.length));if(null != had_draw_cid_list){buff.put(had_draw_cid_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        box_ref_id=buff.getLong();
        sender_cid=buff.getLong();
        sender_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        expired_ms=buff.getLong();
        int had_draw_cid_list_count = buff.getShort();if(had_draw_cid_list_count>0){had_draw_cid_list = new byte[had_draw_cid_list_count];buff.get(had_draw_cid_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
