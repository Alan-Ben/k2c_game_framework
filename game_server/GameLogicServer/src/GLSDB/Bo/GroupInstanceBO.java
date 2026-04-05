package GLSDB.Bo;
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
public class GroupInstanceBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_group_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "分组ID")
    private long group_id;

    public static final int FIELD_activity_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_id", comment = "活动ID")
    private long activity_id;

    public static final int FIELD_us_ids =2;
    @DataBaseField(type = "blob", fieldname = "us_ids", comment = "参与分组的US服务器ID集合")
    private byte[] us_ids;

    public static final int FIELD_need_discard =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "need_discard", comment = "是否需要删除")
    private boolean need_discard;

    public GroupInstanceBO() {
        id = 0;
        group_id = 0L;
        activity_id = 0L;
        us_ids = null;
        need_discard = false;
    }

    public GroupInstanceBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        group_id = rs.getLong(2);
        activity_id = rs.getLong(3);
        us_ids = rs.getBytes(4);
        need_discard = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GroupInstanceBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `group_id`, `activity_id`, `us_ids`, `need_discard`";
    }

    @Override
    public String getTableName() {
        return "`group_instance`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(activity_id).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(need_discard ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(us_ids);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_us_ids)) ret.add(us_ids);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 分组ID
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

    // 活动ID
    public long getActivityId() { return this.activity_id; }
    public void setActivityId(BM _bm, long activity_id) {
        if(activity_id==this.activity_id) 
            return;
        this.activity_id = activity_id; 
        markField(_bm, FIELD_activity_id); 
    }
    public void saveActivityId(BM _bm, long activity_id) {
        if(activity_id==this.activity_id) 
            return;
        this.activity_id = activity_id;
        saveField(_bm, "activity_id", activity_id);
    }

    // 参与分组的US服务器ID集合
    public byte[] getUsIds() { return this.us_ids; }
    public void setUsIds(BM _bm, byte[] us_ids) {
        if(us_ids==this.us_ids) 
            return;
        this.us_ids = us_ids; 
        markField(_bm, FIELD_us_ids); 
    }
    public void saveUsIds(BM _bm, byte[] us_ids) {
        if(us_ids==this.us_ids) 
            return;
        this.us_ids = us_ids;
        saveFieldBytes(_bm, "us_ids", us_ids);
    }

    // 是否需要删除
    public boolean getNeedDiscard() { return this.need_discard; }
    public void setNeedDiscard(BM _bm, boolean need_discard) {
        if(need_discard==this.need_discard) 
            return;
        this.need_discard = need_discard; 
        markField(_bm, FIELD_need_discard); 
    }
    public void saveNeedDiscard(BM _bm, boolean need_discard) {
        if(need_discard==this.need_discard) 
            return;
        this.need_discard = need_discard;
        saveField(_bm, "need_discard", need_discard ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        sBuilder.append(" `us_ids` = ?,");
        sBuilder.append(" `need_discard` = '").append(need_discard ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_activity_id)) sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        if(isFieldMarked(FIELD_us_ids)) sBuilder.append(" `us_ids` = ?,");
        if(isFieldMarked(FIELD_need_discard)) sBuilder.append(" `need_discard` = '").append(need_discard ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `group_instance` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '分组ID',"
                + "`activity_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动ID',"
                + "`us_ids` blob NULL COMMENT '参与分组的US服务器ID集合',"
                + "`need_discard` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否需要删除',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='分组实例数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.gamelogic_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//group_id
        _size+=8;//activity_id
        _size+=2;_size+=us_ids.length;//us_ids
        _size+=1;//need_discard
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(group_id);
        buff.putLong(activity_id);
        buff.putShort((short)(us_ids == null ? 0 : us_ids.length));if(null != us_ids){buff.put(us_ids);}
        buff.put((byte)(need_discard?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        group_id=buff.getLong();
        activity_id=buff.getLong();
        int us_ids_count = buff.getShort();if(us_ids_count>0){us_ids = new byte[us_ids_count];buff.get(us_ids);}
        need_discard=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
