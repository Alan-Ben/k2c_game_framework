package SSDB.Bo;
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
public class CrossServerGroupItemBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_group_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "分组id")
    private long group_id;

    public static final int FIELD_us_id_list =1;
    @DataBaseField(type = "varchar(2048)", fieldname = "us_id_list", comment = "服务器列表")
    private String us_id_list;

    public static final int FIELD_crs_group_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "crs_group_instance_id", comment = "跨服分组排行实例")
    private long crs_group_instance_id;

    public static final int FIELD_has_push =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_push", comment = "是否推送")
    private boolean has_push;

    public CrossServerGroupItemBO() {
        id = 0;
        group_id = 0L;
        us_id_list = "";
        crs_group_instance_id = 0L;
        has_push = false;
    }

    public CrossServerGroupItemBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        group_id = rs.getLong(2);
        us_id_list = rs.getString(3);
        crs_group_instance_id = rs.getLong(4);
        has_push = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossServerGroupItemBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `group_id`, `us_id_list`, `crs_group_instance_id`, `has_push`";
    }

    @Override
    public String getTableName() {
        return "`cross_server_group_item`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(us_id_list == null ? null : us_id_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(crs_group_instance_id).append("', ");
        strBuf.append("'").append(has_push ? 1 : 0).append("', ");
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

    // 分组id
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

    // 服务器列表
    public String getUsIdList() { return this.us_id_list; }
    public void setUsIdList(BM _bm, String us_id_list) {
        if(us_id_list.equals(this.us_id_list)) 
            return;
        this.us_id_list = us_id_list; 
        markField(_bm, FIELD_us_id_list); 
    }
    public void saveUsIdList(BM _bm, String us_id_list) {
        if(us_id_list.equals(this.us_id_list)) 
            return;
        this.us_id_list = us_id_list;
        saveField(_bm, "us_id_list", us_id_list);
    }

    // 跨服分组排行实例
    public long getCrsGroupInstanceId() { return this.crs_group_instance_id; }
    public void setCrsGroupInstanceId(BM _bm, long crs_group_instance_id) {
        if(crs_group_instance_id==this.crs_group_instance_id) 
            return;
        this.crs_group_instance_id = crs_group_instance_id; 
        markField(_bm, FIELD_crs_group_instance_id); 
    }
    public void saveCrsGroupInstanceId(BM _bm, long crs_group_instance_id) {
        if(crs_group_instance_id==this.crs_group_instance_id) 
            return;
        this.crs_group_instance_id = crs_group_instance_id;
        saveField(_bm, "crs_group_instance_id", crs_group_instance_id);
    }

    // 是否推送
    public boolean getHasPush() { return this.has_push; }
    public void setHasPush(BM _bm, boolean has_push) {
        if(has_push==this.has_push) 
            return;
        this.has_push = has_push; 
        markField(_bm, FIELD_has_push); 
    }
    public void saveHasPush(BM _bm, boolean has_push) {
        if(has_push==this.has_push) 
            return;
        this.has_push = has_push;
        saveField(_bm, "has_push", has_push ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `us_id_list` = '").append(us_id_list == null ? null : us_id_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `crs_group_instance_id` = '").append(crs_group_instance_id).append("',");
        sBuilder.append(" `has_push` = '").append(has_push ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_us_id_list)) sBuilder.append(" `us_id_list` = '").append(us_id_list == null ? null : us_id_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_crs_group_instance_id)) sBuilder.append(" `crs_group_instance_id` = '").append(crs_group_instance_id).append("',");
        if(isFieldMarked(FIELD_has_push)) sBuilder.append(" `has_push` = '").append(has_push ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_server_group_item` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '分组id',"
                + "`us_id_list` varchar(2048) NOT NULL DEFAULT '' COMMENT '服务器列表',"
                + "`crs_group_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服分组排行实例',"
                + "`has_push` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否推送',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服分组对象' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.ss_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//group_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(us_id_list);//us_id_list
        _size+=8;//crs_group_instance_id
        _size+=1;//has_push
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(group_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, us_id_list);
        buff.putLong(crs_group_instance_id);
        buff.put((byte)(has_push?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        group_id=buff.getLong();
        us_id_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        crs_group_instance_id=buff.getLong();
        has_push=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
