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
public class ActivityPlayerCrystalGiftPackBuyRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "活动实例ID")
    private long instance_id;

    public static final int FIELD_group_db_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "group_db_id", comment = "礼包组实例id")
    private long group_db_id;

    public static final int FIELD_group_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "礼包组id")
    private long group_id;

    public static final int FIELD_cid =3;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public static final int FIELD_item_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "item_id", comment = "物品id")
    private long item_id;

    public static final int FIELD_had_buy_count =5;
    @DataBaseField(type = "bigint(20)", fieldname = "had_buy_count", comment = "已经购买的次数")
    private long had_buy_count;

    public ActivityPlayerCrystalGiftPackBuyRecordBO() {
        id = 0;
        instance_id = 0L;
        group_db_id = 0L;
        group_id = 0L;
        cid = 0L;
        item_id = 0L;
        had_buy_count = 0L;
    }

    public ActivityPlayerCrystalGiftPackBuyRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instance_id = rs.getLong(2);
        group_db_id = rs.getLong(3);
        group_id = rs.getLong(4);
        cid = rs.getLong(5);
        item_id = rs.getLong(6);
        had_buy_count = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityPlayerCrystalGiftPackBuyRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instance_id`, `group_db_id`, `group_id`, `cid`, `item_id`, `had_buy_count`";
    }

    @Override
    public String getTableName() {
        return "`activity_player_crystal_gift_pack_buy_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(group_db_id).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(item_id).append("', ");
        strBuf.append("'").append(had_buy_count).append("', ");
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

    // 礼包组实例id
    public long getGroupDbId() { return this.group_db_id; }
    public void setGroupDbId(BM _bm, long group_db_id) {
        if(group_db_id==this.group_db_id) 
            return;
        this.group_db_id = group_db_id; 
        markField(_bm, FIELD_group_db_id); 
    }
    public void saveGroupDbId(BM _bm, long group_db_id) {
        if(group_db_id==this.group_db_id) 
            return;
        this.group_db_id = group_db_id;
        saveField(_bm, "group_db_id", group_db_id);
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

    // 物品id
    public long getItemId() { return this.item_id; }
    public void setItemId(BM _bm, long item_id) {
        if(item_id==this.item_id) 
            return;
        this.item_id = item_id; 
        markField(_bm, FIELD_item_id); 
    }
    public void saveItemId(BM _bm, long item_id) {
        if(item_id==this.item_id) 
            return;
        this.item_id = item_id;
        saveField(_bm, "item_id", item_id);
    }

    // 已经购买的次数
    public long getHadBuyCount() { return this.had_buy_count; }
    public void setHadBuyCount(BM _bm, long had_buy_count) {
        if(had_buy_count==this.had_buy_count) 
            return;
        this.had_buy_count = had_buy_count; 
        markField(_bm, FIELD_had_buy_count); 
    }
    public void saveHadBuyCount(BM _bm, long had_buy_count) {
        if(had_buy_count==this.had_buy_count) 
            return;
        this.had_buy_count = had_buy_count;
        saveField(_bm, "had_buy_count", had_buy_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `group_db_id` = '").append(group_db_id).append("',");
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `item_id` = '").append(item_id).append("',");
        sBuilder.append(" `had_buy_count` = '").append(had_buy_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_group_db_id)) sBuilder.append(" `group_db_id` = '").append(group_db_id).append("',");
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_item_id)) sBuilder.append(" `item_id` = '").append(item_id).append("',");
        if(isFieldMarked(FIELD_had_buy_count)) sBuilder.append(" `had_buy_count` = '").append(had_buy_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_player_crystal_gift_pack_buy_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`group_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包组实例id',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包组id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品id',"
                + "`had_buy_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '已经购买的次数',"
                + "KEY `group_db_id` (`group_db_id`),"
                + "KEY `instance_id` (`instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动玩家礼包购买记录数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//group_db_id
        _size+=8;//group_id
        _size+=8;//cid
        _size+=8;//item_id
        _size+=8;//had_buy_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instance_id);
        buff.putLong(group_db_id);
        buff.putLong(group_id);
        buff.putLong(cid);
        buff.putLong(item_id);
        buff.putLong(had_buy_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instance_id=buff.getLong();
        group_db_id=buff.getLong();
        group_id=buff.getLong();
        cid=buff.getLong();
        item_id=buff.getLong();
        had_buy_count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
