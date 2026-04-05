package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogActivityShopBuyBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "活动实例ID")
    private long instance_id;

    public static final int FIELD_shop_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_id", comment = "商店ID")
    private long shop_id;

    public static final int FIELD_item_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "item_id", comment = "物品ID")
    private long item_id;

    public static final int FIELD_num =4;
    @DataBaseField(type = "int(11)", fieldname = "num", comment = "购买数量")
    private int num;

    public static final int FIELD_event_id =5;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =6;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =7;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =8;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public LogActivityShopBuyBO() {
        id = 0;
        cid = 0L;
        instance_id = 0L;
        shop_id = 0L;
        item_id = 0L;
        num = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
    }

    public LogActivityShopBuyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        instance_id = rs.getLong(3);
        shop_id = rs.getLong(4);
        item_id = rs.getLong(5);
        num = rs.getInt(6);
        event_id = rs.getInt(7);
        guid = rs.getLong(8);
        date_time = rs.getInt(9);
        timestamp = rs.getInt(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogActivityShopBuyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `instance_id`, `shop_id`, `item_id`, `num`, `event_id`, `guid`, `date_time`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`log_activity_shop_buy`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(shop_id).append("', ");
        strBuf.append("'").append(item_id).append("', ");
        strBuf.append("'").append(num).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
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

    // 商店ID
    public long getShopId() { return this.shop_id; }
    public void setShopId(BM _bm, long shop_id) {
        if(shop_id==this.shop_id) 
            return;
        this.shop_id = shop_id; 
        markField(_bm, FIELD_shop_id); 
    }
    public void saveShopId(BM _bm, long shop_id) {
        if(shop_id==this.shop_id) 
            return;
        this.shop_id = shop_id;
        saveField(_bm, "shop_id", shop_id);
    }

    // 物品ID
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

    // 购买数量
    public int getNum() { return this.num; }
    public void setNum(BM _bm, int num) {
        if(num==this.num) 
            return;
        this.num = num; 
        markField(_bm, FIELD_num); 
    }
    public void saveNum(BM _bm, int num) {
        if(num==this.num) 
            return;
        this.num = num;
        saveField(_bm, "num", num);
    }

    // 事件类型
    public int getEventId() { return this.event_id; }
    public void setEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 事件唯一id
    public long getGuid() { return this.guid; }
    public void setGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid; 
        markField(_bm, FIELD_guid); 
    }
    public void saveGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid;
        saveField(_bm, "guid", guid);
    }

    // 日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }

    // 时间戳
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `shop_id` = '").append(shop_id).append("',");
        sBuilder.append(" `item_id` = '").append(item_id).append("',");
        sBuilder.append(" `num` = '").append(num).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_shop_id)) sBuilder.append(" `shop_id` = '").append(shop_id).append("',");
        if(isFieldMarked(FIELD_item_id)) sBuilder.append(" `item_id` = '").append(item_id).append("',");
        if(isFieldMarked(FIELD_num)) sBuilder.append(" `num` = '").append(num).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_activity_shop_buy` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`shop_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商店ID',"
                + "`item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品ID',"
                + "`num` int(11) NOT NULL DEFAULT '0' COMMENT '购买数量',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动商店购买日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cid
        _size+=8;//instance_id
        _size+=8;//shop_id
        _size+=8;//item_id
        _size+=4;//num
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(instance_id);
        buff.putLong(shop_id);
        buff.putLong(item_id);
        buff.putInt(num);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        instance_id=buff.getLong();
        shop_id=buff.getLong();
        item_id=buff.getLong();
        num=buff.getInt();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
