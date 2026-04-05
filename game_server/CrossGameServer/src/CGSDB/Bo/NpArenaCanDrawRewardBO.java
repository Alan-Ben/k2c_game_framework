package CGSDB.Bo;
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
public class NpArenaCanDrawRewardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "实例id")
    private long instanceId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_item_type =2;
    @DataBaseField(type = "int(11)", fieldname = "item_type", comment = "物品类型")
    private int item_type;

    public static final int FIELD_item_sub_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "item_sub_id", comment = "物品子类型id")
    private long item_sub_id;

    public static final int FIELD_item_count =4;
    @DataBaseField(type = "bigint(20)", fieldname = "item_count", comment = "物品数量")
    private long item_count;

    public NpArenaCanDrawRewardBO() {
        id = 0;
        instanceId = 0L;
        cid = 0L;
        item_type = 0;
        item_sub_id = 0L;
        item_count = 0L;
    }

    public NpArenaCanDrawRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        cid = rs.getLong(3);
        item_type = rs.getInt(4);
        item_sub_id = rs.getLong(5);
        item_count = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpArenaCanDrawRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `cid`, `item_type`, `item_sub_id`, `item_count`";
    }

    @Override
    public String getTableName() {
        return "`np_arena_can_draw_reward`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(item_type).append("', ");
        strBuf.append("'").append(item_sub_id).append("', ");
        strBuf.append("'").append(item_count).append("', ");
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

    // 实例id
    public long getInstanceId() { return this.instanceId; }
    public void setInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId; 
        markField(_bm, FIELD_instanceId); 
    }
    public void saveInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId;
        saveField(_bm, "instanceId", instanceId);
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

    // 物品类型
    public int getItemType() { return this.item_type; }
    public void setItemType(BM _bm, int item_type) {
        if(item_type==this.item_type) 
            return;
        this.item_type = item_type; 
        markField(_bm, FIELD_item_type); 
    }
    public void saveItemType(BM _bm, int item_type) {
        if(item_type==this.item_type) 
            return;
        this.item_type = item_type;
        saveField(_bm, "item_type", item_type);
    }

    // 物品子类型id
    public long getItemSubId() { return this.item_sub_id; }
    public void setItemSubId(BM _bm, long item_sub_id) {
        if(item_sub_id==this.item_sub_id) 
            return;
        this.item_sub_id = item_sub_id; 
        markField(_bm, FIELD_item_sub_id); 
    }
    public void saveItemSubId(BM _bm, long item_sub_id) {
        if(item_sub_id==this.item_sub_id) 
            return;
        this.item_sub_id = item_sub_id;
        saveField(_bm, "item_sub_id", item_sub_id);
    }

    // 物品数量
    public long getItemCount() { return this.item_count; }
    public void setItemCount(BM _bm, long item_count) {
        if(item_count==this.item_count) 
            return;
        this.item_count = item_count; 
        markField(_bm, FIELD_item_count); 
    }
    public void saveItemCount(BM _bm, long item_count) {
        if(item_count==this.item_count) 
            return;
        this.item_count = item_count;
        saveField(_bm, "item_count", item_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `item_type` = '").append(item_type).append("',");
        sBuilder.append(" `item_sub_id` = '").append(item_sub_id).append("',");
        sBuilder.append(" `item_count` = '").append(item_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_item_type)) sBuilder.append(" `item_type` = '").append(item_type).append("',");
        if(isFieldMarked(FIELD_item_sub_id)) sBuilder.append(" `item_sub_id` = '").append(item_sub_id).append("',");
        if(isFieldMarked(FIELD_item_count)) sBuilder.append(" `item_count` = '").append(item_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_arena_can_draw_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '实例id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`item_type` int(11) NOT NULL DEFAULT '0' COMMENT '物品类型',"
                + "`item_sub_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品子类型id',"
                + "`item_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品数量',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='比武擂台 聚会数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossgame_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//instanceId
        _size+=8;//cid
        _size+=4;//item_type
        _size+=8;//item_sub_id
        _size+=8;//item_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putLong(cid);
        buff.putInt(item_type);
        buff.putLong(item_sub_id);
        buff.putLong(item_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        cid=buff.getLong();
        item_type=buff.getInt();
        item_sub_id=buff.getLong();
        item_count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
