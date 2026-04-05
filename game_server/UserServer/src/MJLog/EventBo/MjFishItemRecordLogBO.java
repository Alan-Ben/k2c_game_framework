package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjFishItemRecordLogBO extends MJEventLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_uid =1;
    @DataBaseField(type = "varchar(64)", fieldname = "uid", comment = "平台用户id")
    private String uid;

    public static final int FIELD_vip_lv =2;
    @DataBaseField(type = "int(11)", fieldname = "vip_lv", comment = "玩家VIP等级")
    private int vip_lv;

    public static final int FIELD_server_id =3;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "服务器id")
    private int server_id;

    public static final int FIELD_platform =4;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "平台id")
    private int platform;

    public static final int FIELD_region =5;
    @DataBaseField(type = "int(11)", fieldname = "region", comment = "区域id")
    private int region;

    public static final int FIELD_create_time =6;
    @DataBaseField(type = "int(11)", fieldname = "create_time", comment = "玩家创角时间")
    private int create_time;

    public static final int FIELD_type =7;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "物品类型：1=矿石；2=奇物；3=组合")
    private int type;

    public static final int FIELD_item_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "item_id", comment = "物品id")
    private long item_id;

    public static final int FIELD_is_advanced =9;
    @DataBaseField(type = "int(11)", fieldname = "is_advanced", comment = "是否高级：1=是；2=否")
    private int is_advanced;

    public static final int FIELD_action_type =10;
    @DataBaseField(type = "int(11)", fieldname = "action_type", comment = "操作类型：1=解锁；2=升级")
    private int action_type;

    public static final int FIELD_before_level =11;
    @DataBaseField(type = "varchar(128)", fieldname = "before_level", comment = "升级前等级，未解锁为0")
    private String before_level;

    public static final int FIELD_new_level =12;
    @DataBaseField(type = "varchar(128)", fieldname = "new_level", comment = "升级后等级")
    private String new_level;

    public static final int FIELD_event =13;
    @DataBaseField(type = "int(11)", fieldname = "event", comment = "事件id")
    private int event;

    public static final int FIELD_timestamp =14;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjFishItemRecordLogBO() {
        id = 0;
        cid = 0L;
        uid = "";
        vip_lv = 0;
        server_id = 0;
        platform = 0;
        region = 0;
        create_time = 0;
        type = 0;
        item_id = 0L;
        is_advanced = 0;
        action_type = 0;
        before_level = "";
        new_level = "";
        event = 0;
        timestamp = 0;
    }

    public MjFishItemRecordLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        uid = rs.getString(3);
        vip_lv = rs.getInt(4);
        server_id = rs.getInt(5);
        platform = rs.getInt(6);
        region = rs.getInt(7);
        create_time = rs.getInt(8);
        type = rs.getInt(9);
        item_id = rs.getLong(10);
        is_advanced = rs.getInt(11);
        action_type = rs.getInt(12);
        before_level = rs.getString(13);
        new_level = rs.getString(14);
        event = rs.getInt(15);
        timestamp = rs.getInt(16);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjFishItemRecordLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `type`, `item_id`, `is_advanced`, `action_type`, `before_level`, `new_level`, `event`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_fish_item_record_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(vip_lv).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region).append("', ");
        strBuf.append("'").append(create_time).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("'").append(item_id).append("', ");
        strBuf.append("'").append(is_advanced).append("', ");
        strBuf.append("'").append(action_type).append("', ");
        strBuf.append("'").append(before_level == null ? null : before_level.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(new_level == null ? null : new_level.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(event).append("', ");
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

    // 平台用户id
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

    // 玩家VIP等级
    public int getVipLv() { return this.vip_lv; }
    public void setVipLv(BM _bm, int vip_lv) {
        if(vip_lv==this.vip_lv) 
            return;
        this.vip_lv = vip_lv; 
        markField(_bm, FIELD_vip_lv); 
    }
    public void saveVipLv(BM _bm, int vip_lv) {
        if(vip_lv==this.vip_lv) 
            return;
        this.vip_lv = vip_lv;
        saveField(_bm, "vip_lv", vip_lv);
    }

    // 服务器id
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

    // 平台id
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

    // 区域id
    public int getRegion() { return this.region; }
    public void setRegion(BM _bm, int region) {
        if(region==this.region) 
            return;
        this.region = region; 
        markField(_bm, FIELD_region); 
    }
    public void saveRegion(BM _bm, int region) {
        if(region==this.region) 
            return;
        this.region = region;
        saveField(_bm, "region", region);
    }

    // 玩家创角时间
    public int getCreateTime() { return this.create_time; }
    public void setCreateTime(BM _bm, int create_time) {
        if(create_time==this.create_time) 
            return;
        this.create_time = create_time; 
        markField(_bm, FIELD_create_time); 
    }
    public void saveCreateTime(BM _bm, int create_time) {
        if(create_time==this.create_time) 
            return;
        this.create_time = create_time;
        saveField(_bm, "create_time", create_time);
    }

    // 物品类型：1=矿石；2=奇物；3=组合
    public int getType() { return this.type; }
    public void setType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type; 
        markField(_bm, FIELD_type); 
    }
    public void saveType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type;
        saveField(_bm, "type", type);
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

    // 是否高级：1=是；2=否
    public int getIsAdvanced() { return this.is_advanced; }
    public void setIsAdvanced(BM _bm, int is_advanced) {
        if(is_advanced==this.is_advanced) 
            return;
        this.is_advanced = is_advanced; 
        markField(_bm, FIELD_is_advanced); 
    }
    public void saveIsAdvanced(BM _bm, int is_advanced) {
        if(is_advanced==this.is_advanced) 
            return;
        this.is_advanced = is_advanced;
        saveField(_bm, "is_advanced", is_advanced);
    }

    // 操作类型：1=解锁；2=升级
    public int getActionType() { return this.action_type; }
    public void setActionType(BM _bm, int action_type) {
        if(action_type==this.action_type) 
            return;
        this.action_type = action_type; 
        markField(_bm, FIELD_action_type); 
    }
    public void saveActionType(BM _bm, int action_type) {
        if(action_type==this.action_type) 
            return;
        this.action_type = action_type;
        saveField(_bm, "action_type", action_type);
    }

    // 升级前等级，未解锁为0
    public String getBeforeLevel() { return this.before_level; }
    public void setBeforeLevel(BM _bm, String before_level) {
        if(before_level.equals(this.before_level)) 
            return;
        this.before_level = before_level; 
        markField(_bm, FIELD_before_level); 
    }
    public void saveBeforeLevel(BM _bm, String before_level) {
        if(before_level.equals(this.before_level)) 
            return;
        this.before_level = before_level;
        saveField(_bm, "before_level", before_level);
    }

    // 升级后等级
    public String getNewLevel() { return this.new_level; }
    public void setNewLevel(BM _bm, String new_level) {
        if(new_level.equals(this.new_level)) 
            return;
        this.new_level = new_level; 
        markField(_bm, FIELD_new_level); 
    }
    public void saveNewLevel(BM _bm, String new_level) {
        if(new_level.equals(this.new_level)) 
            return;
        this.new_level = new_level;
        saveField(_bm, "new_level", new_level);
    }

    // 事件id
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



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `vip_lv` = '").append(vip_lv).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region).append("',");
        sBuilder.append(" `create_time` = '").append(create_time).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `item_id` = '").append(item_id).append("',");
        sBuilder.append(" `is_advanced` = '").append(is_advanced).append("',");
        sBuilder.append(" `action_type` = '").append(action_type).append("',");
        sBuilder.append(" `before_level` = '").append(before_level == null ? null : before_level.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `new_level` = '").append(new_level == null ? null : new_level.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `event` = '").append(event).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_vip_lv)) sBuilder.append(" `vip_lv` = '").append(vip_lv).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region).append("',");
        if(isFieldMarked(FIELD_create_time)) sBuilder.append(" `create_time` = '").append(create_time).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_item_id)) sBuilder.append(" `item_id` = '").append(item_id).append("',");
        if(isFieldMarked(FIELD_is_advanced)) sBuilder.append(" `is_advanced` = '").append(is_advanced).append("',");
        if(isFieldMarked(FIELD_action_type)) sBuilder.append(" `action_type` = '").append(action_type).append("',");
        if(isFieldMarked(FIELD_before_level)) sBuilder.append(" `before_level` = '").append(before_level == null ? null : before_level.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_new_level)) sBuilder.append(" `new_level` = '").append(new_level == null ? null : new_level.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_event)) sBuilder.append(" `event` = '").append(event).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_fish_item_record_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '物品类型：1=矿石；2=奇物；3=组合',"
                + "`item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品id',"
                + "`is_advanced` int(11) NOT NULL DEFAULT '0' COMMENT '是否高级：1=是；2=否',"
                + "`action_type` int(11) NOT NULL DEFAULT '0' COMMENT '操作类型：1=解锁；2=升级',"
                + "`before_level` varchar(128) NOT NULL DEFAULT '' COMMENT '升级前等级，未解锁为0',"
                + "`new_level` varchar(128) NOT NULL DEFAULT '' COMMENT '升级后等级',"
                + "`event` int(11) NOT NULL DEFAULT '0' COMMENT '事件id',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-寻宝图鉴记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size+=4;//vip_lv
        _size+=4;//server_id
        _size+=4;//platform
        _size+=4;//region
        _size+=4;//create_time
        _size+=4;//type
        _size+=8;//item_id
        _size+=4;//is_advanced
        _size+=4;//action_type
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(before_level);//before_level
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(new_level);//new_level
        _size+=4;//event
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
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        buff.putInt(vip_lv);
        buff.putInt(server_id);
        buff.putInt(platform);
        buff.putInt(region);
        buff.putInt(create_time);
        buff.putInt(type);
        buff.putLong(item_id);
        buff.putInt(is_advanced);
        buff.putInt(action_type);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, before_level);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, new_level);
        buff.putInt(event);
        buff.putInt(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        vip_lv=buff.getInt();
        server_id=buff.getInt();
        platform=buff.getInt();
        region=buff.getInt();
        create_time=buff.getInt();
        type=buff.getInt();
        item_id=buff.getLong();
        is_advanced=buff.getInt();
        action_type=buff.getInt();
        before_level=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        new_level=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        event=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
