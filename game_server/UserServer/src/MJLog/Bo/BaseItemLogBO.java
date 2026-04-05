package MJLog.Bo;
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
public class BaseItemLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_sole_id =0;
    @DataBaseField(type = "varchar(32)", fieldname = "sole_id", comment = "项目唯一角色id")
    private String sole_id;

    public static final int FIELD_uid =1;
    @DataBaseField(type = "varchar(64)", fieldname = "uid", comment = "用户id")
    private String uid;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_itemType =3;
    @DataBaseField(type = "int(11)", fieldname = "itemType", comment = "物品类型")
    private int itemType;

    public static final int FIELD_itemId =4;
    @DataBaseField(type = "int(11)", fieldname = "itemId", comment = "物品id")
    private int itemId;

    public static final int FIELD_action =5;
    @DataBaseField(type = "int(11)", fieldname = "action", comment = "1获得,2消耗,3无效丢失,4过期")
    private int action;

    public static final int FIELD_oldNumber =6;
    @DataBaseField(type = "double", fieldname = "oldNumber", comment = "旧值")
    private double oldNumber;

    public static final int FIELD_exchange =7;
    @DataBaseField(type = "double", fieldname = "exchange", comment = "改变值")
    private double exchange;

    public static final int FIELD_finalNumber =8;
    @DataBaseField(type = "double", fieldname = "finalNumber", comment = "最终值")
    private double finalNumber;

    public static final int FIELD_item =9;
    @DataBaseField(type = "varchar(100)", fieldname = "item", comment = "物品")
    private String item;

    public static final int FIELD_event =10;
    @DataBaseField(type = "int(11)", fieldname = "event", comment = "事件")
    private int event;

    public static final int FIELD_timestamp =11;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public static final int FIELD_server_id =12;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "所在的服务器id")
    private int server_id;

    public static final int FIELD_platform =13;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "所在的平台id")
    private int platform;

    public static final int FIELD_region =14;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "所在的区域id")
    private String region;

    public static final int FIELD_level =15;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "玩家等级")
    private int level;

    public static final int FIELD_ext =16;
    @DataBaseField(type = "text", fieldname = "ext", comment = "扩展字段：json格式")
    private String ext;

    public static final int FIELD_date_time =17;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public BaseItemLogBO() {
        id = 0;
        sole_id = "";
        uid = "";
        cid = 0L;
        itemType = 0;
        itemId = 0;
        action = 0;
        oldNumber = 0.0d;
        exchange = 0.0d;
        finalNumber = 0.0d;
        item = "";
        event = 0;
        timestamp = 0;
        server_id = 0;
        platform = 0;
        region = "";
        level = 0;
        ext = "";
        date_time = 0;
    }

    public BaseItemLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sole_id = rs.getString(2);
        uid = rs.getString(3);
        cid = rs.getLong(4);
        itemType = rs.getInt(5);
        itemId = rs.getInt(6);
        action = rs.getInt(7);
        oldNumber = rs.getDouble(8);
        exchange = rs.getDouble(9);
        finalNumber = rs.getDouble(10);
        item = rs.getString(11);
        event = rs.getInt(12);
        timestamp = rs.getInt(13);
        server_id = rs.getInt(14);
        platform = rs.getInt(15);
        region = rs.getString(16);
        level = rs.getInt(17);
        ext = rs.getString(18);
        date_time = rs.getInt(19);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new BaseItemLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sole_id`, `uid`, `cid`, `itemType`, `itemId`, `action`, `oldNumber`, `exchange`, `finalNumber`, `item`, `event`, `timestamp`, `server_id`, `platform`, `region`, `level`, `ext`, `date_time`";
    }

    @Override
    public String getTableName() {
        return "`base_item_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(itemType).append("', ");
        strBuf.append("'").append(itemId).append("', ");
        strBuf.append("'").append(action).append("', ");
        strBuf.append("'").append(oldNumber).append("', ");
        strBuf.append("'").append(exchange).append("', ");
        strBuf.append("'").append(finalNumber).append("', ");
        strBuf.append("'").append(item == null ? null : item.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(event).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(date_time).append("', ");
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

    // 项目唯一角色id
    public String getSoleId() { return this.sole_id; }
    public void setSoleId(BM _bm, String sole_id) {
        if(sole_id.equals(this.sole_id)) 
            return;
        this.sole_id = sole_id; 
        markField(_bm, FIELD_sole_id); 
    }
    public void saveSoleId(BM _bm, String sole_id) {
        if(sole_id.equals(this.sole_id)) 
            return;
        this.sole_id = sole_id;
        saveField(_bm, "sole_id", sole_id);
    }

    // 用户id
    public String getUid() { return this.uid; }
    public void setUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 角色id
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
    public int getItemType() { return this.itemType; }
    public void setItemType(BM _bm, int itemType) {
        if(itemType==this.itemType) 
            return;
        this.itemType = itemType; 
        markField(_bm, FIELD_itemType); 
    }
    public void saveItemType(BM _bm, int itemType) {
        if(itemType==this.itemType) 
            return;
        this.itemType = itemType;
        saveField(_bm, "itemType", itemType);
    }

    // 物品id
    public int getItemId() { return this.itemId; }
    public void setItemId(BM _bm, int itemId) {
        if(itemId==this.itemId) 
            return;
        this.itemId = itemId; 
        markField(_bm, FIELD_itemId); 
    }
    public void saveItemId(BM _bm, int itemId) {
        if(itemId==this.itemId) 
            return;
        this.itemId = itemId;
        saveField(_bm, "itemId", itemId);
    }

    // 1获得,2消耗,3无效丢失,4过期
    public int getAction() { return this.action; }
    public void setAction(BM _bm, int action) {
        if(action==this.action) 
            return;
        this.action = action; 
        markField(_bm, FIELD_action); 
    }
    public void saveAction(BM _bm, int action) {
        if(action==this.action) 
            return;
        this.action = action;
        saveField(_bm, "action", action);
    }

    // 旧值
    public double getOldNumber() { return this.oldNumber; }
    public void setOldNumber(BM _bm, double oldNumber) {
        if(oldNumber==this.oldNumber) 
            return;
        this.oldNumber = oldNumber; 
        markField(_bm, FIELD_oldNumber); 
    }
    public void saveOldNumber(BM _bm, double oldNumber) {
        if(oldNumber==this.oldNumber) 
            return;
        this.oldNumber = oldNumber;
        saveField(_bm, "oldNumber", oldNumber);
    }

    // 改变值
    public double getExchange() { return this.exchange; }
    public void setExchange(BM _bm, double exchange) {
        if(exchange==this.exchange) 
            return;
        this.exchange = exchange; 
        markField(_bm, FIELD_exchange); 
    }
    public void saveExchange(BM _bm, double exchange) {
        if(exchange==this.exchange) 
            return;
        this.exchange = exchange;
        saveField(_bm, "exchange", exchange);
    }

    // 最终值
    public double getFinalNumber() { return this.finalNumber; }
    public void setFinalNumber(BM _bm, double finalNumber) {
        if(finalNumber==this.finalNumber) 
            return;
        this.finalNumber = finalNumber; 
        markField(_bm, FIELD_finalNumber); 
    }
    public void saveFinalNumber(BM _bm, double finalNumber) {
        if(finalNumber==this.finalNumber) 
            return;
        this.finalNumber = finalNumber;
        saveField(_bm, "finalNumber", finalNumber);
    }

    // 物品
    public String getItem() { return this.item; }
    public void setItem(BM _bm, String item) {
        if(item.equals(this.item)) 
            return;
        this.item = item; 
        markField(_bm, FIELD_item); 
    }
    public void saveItem(BM _bm, String item) {
        if(item.equals(this.item)) 
            return;
        this.item = item;
        saveField(_bm, "item", item);
    }

    // 事件
    public int getEvent() { return this.event; }
    public void setEvent(BM _bm, int event) {
        if(event==this.event) 
            return;
        this.event = event; 
        markField(_bm, FIELD_event); 
    }
    public void saveEvent(BM _bm, int event) {
        if(event==this.event) 
            return;
        this.event = event;
        saveField(_bm, "event", event);
    }

    // 事件发生时间戳(10位)
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

    // 所在的服务器id
    public int getServerId() { return this.server_id; }
    public void setServerId(BM _bm, int server_id) {
        if(server_id==this.server_id) 
            return;
        this.server_id = server_id; 
        markField(_bm, FIELD_server_id); 
    }
    public void saveServerId(BM _bm, int server_id) {
        if(server_id==this.server_id) 
            return;
        this.server_id = server_id;
        saveField(_bm, "server_id", server_id);
    }

    // 所在的平台id
    public int getPlatform() { return this.platform; }
    public void setPlatform(BM _bm, int platform) {
        if(platform==this.platform) 
            return;
        this.platform = platform; 
        markField(_bm, FIELD_platform); 
    }
    public void savePlatform(BM _bm, int platform) {
        if(platform==this.platform) 
            return;
        this.platform = platform;
        saveField(_bm, "platform", platform);
    }

    // 所在的区域id
    public String getRegion() { return this.region; }
    public void setRegion(BM _bm, String region) {
        if(region.equals(this.region)) 
            return;
        this.region = region; 
        markField(_bm, FIELD_region); 
    }
    public void saveRegion(BM _bm, String region) {
        if(region.equals(this.region)) 
            return;
        this.region = region;
        saveField(_bm, "region", region);
    }

    // 玩家等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 扩展字段：json格式
    public String getExt() { return this.ext; }
    public void setExt(BM _bm, String ext) {
        if(ext.equals(this.ext)) 
            return;
        this.ext = ext; 
        markField(_bm, FIELD_ext); 
    }
    public void saveExt(BM _bm, String ext) {
        if(ext.equals(this.ext)) 
            return;
        this.ext = ext;
        saveField(_bm, "ext", ext);
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `sole_id` = '").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `itemType` = '").append(itemType).append("',");
        sBuilder.append(" `itemId` = '").append(itemId).append("',");
        sBuilder.append(" `action` = '").append(action).append("',");
        sBuilder.append(" `oldNumber` = '").append(oldNumber).append("',");
        sBuilder.append(" `exchange` = '").append(exchange).append("',");
        sBuilder.append(" `finalNumber` = '").append(finalNumber).append("',");
        sBuilder.append(" `item` = '").append(item == null ? null : item.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `event` = '").append(event).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_sole_id)) sBuilder.append(" `sole_id` = '").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_itemType)) sBuilder.append(" `itemType` = '").append(itemType).append("',");
        if(isFieldMarked(FIELD_itemId)) sBuilder.append(" `itemId` = '").append(itemId).append("',");
        if(isFieldMarked(FIELD_action)) sBuilder.append(" `action` = '").append(action).append("',");
        if(isFieldMarked(FIELD_oldNumber)) sBuilder.append(" `oldNumber` = '").append(oldNumber).append("',");
        if(isFieldMarked(FIELD_exchange)) sBuilder.append(" `exchange` = '").append(exchange).append("',");
        if(isFieldMarked(FIELD_finalNumber)) sBuilder.append(" `finalNumber` = '").append(finalNumber).append("',");
        if(isFieldMarked(FIELD_item)) sBuilder.append(" `item` = '").append(item == null ? null : item.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_event)) sBuilder.append(" `event` = '").append(event).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `base_item_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sole_id` varchar(32) NOT NULL DEFAULT '' COMMENT '项目唯一角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '用户id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`itemType` int(11) NOT NULL DEFAULT '0' COMMENT '物品类型',"
                + "`itemId` int(11) NOT NULL DEFAULT '0' COMMENT '物品id',"
                + "`action` int(11) NOT NULL DEFAULT '0' COMMENT '1获得,2消耗,3无效丢失,4过期',"
                + "`oldNumber` double NOT NULL DEFAULT '0.0' COMMENT '旧值',"
                + "`exchange` double NOT NULL DEFAULT '0.0' COMMENT '改变值',"
                + "`finalNumber` double NOT NULL DEFAULT '0.0' COMMENT '最终值',"
                + "`item` varchar(100) NOT NULL DEFAULT '' COMMENT '物品',"
                + "`event` int(11) NOT NULL DEFAULT '0' COMMENT '事件',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '所在的服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '所在的平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '所在的区域id',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`ext` text NULL COMMENT '扩展字段：json格式',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-物品日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sole_id);//sole_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size+=8;//cid
        _size+=4;//itemType
        _size+=4;//itemId
        _size+=4;//action
        _size+=8;//oldNumber
        _size+=8;//exchange
        _size+=8;//finalNumber
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(item);//item
        _size+=4;//event
        _size+=4;//timestamp
        _size+=4;//server_id
        _size+=4;//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(region);//region
        _size+=4;//level
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);//ext
        _size+=4;//date_time
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sole_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        buff.putLong(cid);
        buff.putInt(itemType);
        buff.putInt(itemId);
        buff.putInt(action);
        buff.putDouble(oldNumber);
        buff.putDouble(exchange);
        buff.putDouble(finalNumber);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, item);
        buff.putInt(event);
        buff.putInt(timestamp);
        buff.putInt(server_id);
        buff.putInt(platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, region);
        buff.putInt(level);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ext);
        buff.putInt(date_time);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        sole_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cid=buff.getLong();
        itemType=buff.getInt();
        itemId=buff.getInt();
        action=buff.getInt();
        oldNumber=buff.getDouble();
        exchange=buff.getDouble();
        finalNumber=buff.getDouble();
        item=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        event=buff.getInt();
        timestamp=buff.getInt();
        server_id=buff.getInt();
        platform=buff.getInt();
        region=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        level=buff.getInt();
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        date_time=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
