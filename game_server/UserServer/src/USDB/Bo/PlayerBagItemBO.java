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
public class PlayerBagItemBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_itemId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "itemId", comment = "物品ID")
    private long itemId;

    public static final int FIELD_itemCount =2;
    @DataBaseField(type = "bigint(20)", fieldname = "itemCount", comment = "物品数量")
    private long itemCount;

    public static final int FIELD_lastGetTimeS =3;
    @DataBaseField(type = "int(11)", fieldname = "lastGetTimeS", comment = "最后一次获取时间")
    private int lastGetTimeS;

    public static final int FIELD_newGetTimeS =4;
    @DataBaseField(type = "int(11)", fieldname = "newGetTimeS", comment = "首次获取时间")
    private int newGetTimeS;

    public static final int FIELD_lastClickTimeS =5;
    @DataBaseField(type = "int(11)", fieldname = "lastClickTimeS", comment = "最后一次点击时间")
    private int lastClickTimeS;

    public static final int FIELD_total_gain_count =6;
    @DataBaseField(type = "bigint(20)", fieldname = "total_gain_count", comment = "总获得数量")
    private long total_gain_count;

    public static final int FIELD_total_consume_count =7;
    @DataBaseField(type = "bigint(20)", fieldname = "total_consume_count", comment = "总消耗数量")
    private long total_consume_count;

    public static final int FIELD_relative_activity_instance_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "relative_activity_instance_id", comment = "关联的活动实例ID")
    private long relative_activity_instance_id;

    public PlayerBagItemBO() {
        id = 0;
        cid = 0L;
        itemId = 0L;
        itemCount = 0L;
        lastGetTimeS = 0;
        newGetTimeS = 0;
        lastClickTimeS = 0;
        total_gain_count = 0L;
        total_consume_count = 0L;
        relative_activity_instance_id = 0L;
    }

    public PlayerBagItemBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        itemId = rs.getLong(3);
        itemCount = rs.getLong(4);
        lastGetTimeS = rs.getInt(5);
        newGetTimeS = rs.getInt(6);
        lastClickTimeS = rs.getInt(7);
        total_gain_count = rs.getLong(8);
        total_consume_count = rs.getLong(9);
        relative_activity_instance_id = rs.getLong(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerBagItemBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `itemId`, `itemCount`, `lastGetTimeS`, `newGetTimeS`, `lastClickTimeS`, `total_gain_count`, `total_consume_count`, `relative_activity_instance_id`";
    }

    @Override
    public String getTableName() {
        return "`player_bag_item`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(itemId).append("', ");
        strBuf.append("'").append(itemCount).append("', ");
        strBuf.append("'").append(lastGetTimeS).append("', ");
        strBuf.append("'").append(newGetTimeS).append("', ");
        strBuf.append("'").append(lastClickTimeS).append("', ");
        strBuf.append("'").append(total_gain_count).append("', ");
        strBuf.append("'").append(total_consume_count).append("', ");
        strBuf.append("'").append(relative_activity_instance_id).append("', ");
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

    // 玩家账号ID
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

    // 物品ID
    public long getItemId() { return this.itemId; }
    public void setItemId(BM _bm, long itemId) {
        if(itemId==this.itemId) 
            return;
        this.itemId = itemId; 
        markField(_bm, FIELD_itemId); 
    }
    public void saveItemId(BM _bm, long itemId) {
        if(itemId==this.itemId) 
            return;
        this.itemId = itemId;
        saveField(_bm, "itemId", itemId);
    }

    // 物品数量
    public long getItemCount() { return this.itemCount; }
    public void setItemCount(BM _bm, long itemCount) {
        if(itemCount==this.itemCount) 
            return;
        this.itemCount = itemCount; 
        markField(_bm, FIELD_itemCount); 
    }
    public void saveItemCount(BM _bm, long itemCount) {
        if(itemCount==this.itemCount) 
            return;
        this.itemCount = itemCount;
        saveField(_bm, "itemCount", itemCount);
    }

    // 最后一次获取时间
    public int getLastGetTimeS() { return this.lastGetTimeS; }
    public void setLastGetTimeS(BM _bm, int lastGetTimeS) {
        if(lastGetTimeS==this.lastGetTimeS) 
            return;
        this.lastGetTimeS = lastGetTimeS; 
        markField(_bm, FIELD_lastGetTimeS); 
    }
    public void saveLastGetTimeS(BM _bm, int lastGetTimeS) {
        if(lastGetTimeS==this.lastGetTimeS) 
            return;
        this.lastGetTimeS = lastGetTimeS;
        saveField(_bm, "lastGetTimeS", lastGetTimeS);
    }

    // 首次获取时间
    public int getNewGetTimeS() { return this.newGetTimeS; }
    public void setNewGetTimeS(BM _bm, int newGetTimeS) {
        if(newGetTimeS==this.newGetTimeS) 
            return;
        this.newGetTimeS = newGetTimeS; 
        markField(_bm, FIELD_newGetTimeS); 
    }
    public void saveNewGetTimeS(BM _bm, int newGetTimeS) {
        if(newGetTimeS==this.newGetTimeS) 
            return;
        this.newGetTimeS = newGetTimeS;
        saveField(_bm, "newGetTimeS", newGetTimeS);
    }

    // 最后一次点击时间
    public int getLastClickTimeS() { return this.lastClickTimeS; }
    public void setLastClickTimeS(BM _bm, int lastClickTimeS) {
        if(lastClickTimeS==this.lastClickTimeS) 
            return;
        this.lastClickTimeS = lastClickTimeS; 
        markField(_bm, FIELD_lastClickTimeS); 
    }
    public void saveLastClickTimeS(BM _bm, int lastClickTimeS) {
        if(lastClickTimeS==this.lastClickTimeS) 
            return;
        this.lastClickTimeS = lastClickTimeS;
        saveField(_bm, "lastClickTimeS", lastClickTimeS);
    }

    // 总获得数量
    public long getTotalGainCount() { return this.total_gain_count; }
    public void setTotalGainCount(BM _bm, long total_gain_count) {
        if(total_gain_count==this.total_gain_count) 
            return;
        this.total_gain_count = total_gain_count; 
        markField(_bm, FIELD_total_gain_count); 
    }
    public void saveTotalGainCount(BM _bm, long total_gain_count) {
        if(total_gain_count==this.total_gain_count) 
            return;
        this.total_gain_count = total_gain_count;
        saveField(_bm, "total_gain_count", total_gain_count);
    }

    // 总消耗数量
    public long getTotalConsumeCount() { return this.total_consume_count; }
    public void setTotalConsumeCount(BM _bm, long total_consume_count) {
        if(total_consume_count==this.total_consume_count) 
            return;
        this.total_consume_count = total_consume_count; 
        markField(_bm, FIELD_total_consume_count); 
    }
    public void saveTotalConsumeCount(BM _bm, long total_consume_count) {
        if(total_consume_count==this.total_consume_count) 
            return;
        this.total_consume_count = total_consume_count;
        saveField(_bm, "total_consume_count", total_consume_count);
    }

    // 关联的活动实例ID
    public long getRelativeActivityInstanceId() { return this.relative_activity_instance_id; }
    public void setRelativeActivityInstanceId(BM _bm, long relative_activity_instance_id) {
        if(relative_activity_instance_id==this.relative_activity_instance_id) 
            return;
        this.relative_activity_instance_id = relative_activity_instance_id; 
        markField(_bm, FIELD_relative_activity_instance_id); 
    }
    public void saveRelativeActivityInstanceId(BM _bm, long relative_activity_instance_id) {
        if(relative_activity_instance_id==this.relative_activity_instance_id) 
            return;
        this.relative_activity_instance_id = relative_activity_instance_id;
        saveField(_bm, "relative_activity_instance_id", relative_activity_instance_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `itemId` = '").append(itemId).append("',");
        sBuilder.append(" `itemCount` = '").append(itemCount).append("',");
        sBuilder.append(" `lastGetTimeS` = '").append(lastGetTimeS).append("',");
        sBuilder.append(" `newGetTimeS` = '").append(newGetTimeS).append("',");
        sBuilder.append(" `lastClickTimeS` = '").append(lastClickTimeS).append("',");
        sBuilder.append(" `total_gain_count` = '").append(total_gain_count).append("',");
        sBuilder.append(" `total_consume_count` = '").append(total_consume_count).append("',");
        sBuilder.append(" `relative_activity_instance_id` = '").append(relative_activity_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_itemId)) sBuilder.append(" `itemId` = '").append(itemId).append("',");
        if(isFieldMarked(FIELD_itemCount)) sBuilder.append(" `itemCount` = '").append(itemCount).append("',");
        if(isFieldMarked(FIELD_lastGetTimeS)) sBuilder.append(" `lastGetTimeS` = '").append(lastGetTimeS).append("',");
        if(isFieldMarked(FIELD_newGetTimeS)) sBuilder.append(" `newGetTimeS` = '").append(newGetTimeS).append("',");
        if(isFieldMarked(FIELD_lastClickTimeS)) sBuilder.append(" `lastClickTimeS` = '").append(lastClickTimeS).append("',");
        if(isFieldMarked(FIELD_total_gain_count)) sBuilder.append(" `total_gain_count` = '").append(total_gain_count).append("',");
        if(isFieldMarked(FIELD_total_consume_count)) sBuilder.append(" `total_consume_count` = '").append(total_consume_count).append("',");
        if(isFieldMarked(FIELD_relative_activity_instance_id)) sBuilder.append(" `relative_activity_instance_id` = '").append(relative_activity_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_bag_item` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`itemId` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品ID',"
                + "`itemCount` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品数量',"
                + "`lastGetTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '最后一次获取时间',"
                + "`newGetTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '首次获取时间',"
                + "`lastClickTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '最后一次点击时间',"
                + "`total_gain_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '总获得数量',"
                + "`total_consume_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '总消耗数量',"
                + "`relative_activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '关联的活动实例ID',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Bag Item Info 玩家背包物品信息表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//itemId
        _size+=8;//itemCount
        _size+=4;//lastGetTimeS
        _size+=4;//newGetTimeS
        _size+=4;//lastClickTimeS
        _size+=8;//total_gain_count
        _size+=8;//total_consume_count
        _size+=8;//relative_activity_instance_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(itemId);
        buff.putLong(itemCount);
        buff.putInt(lastGetTimeS);
        buff.putInt(newGetTimeS);
        buff.putInt(lastClickTimeS);
        buff.putLong(total_gain_count);
        buff.putLong(total_consume_count);
        buff.putLong(relative_activity_instance_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        itemId=buff.getLong();
        itemCount=buff.getLong();
        lastGetTimeS=buff.getInt();
        newGetTimeS=buff.getInt();
        lastClickTimeS=buff.getInt();
        total_gain_count=buff.getLong();
        total_consume_count=buff.getLong();
        relative_activity_instance_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
