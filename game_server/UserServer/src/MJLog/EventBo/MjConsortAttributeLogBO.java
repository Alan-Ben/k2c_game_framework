package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjConsortAttributeLogBO extends MJEventLogBo {

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

    public static final int FIELD_lid =7;
    @DataBaseField(type = "bigint(20)", fieldname = "lid", comment = "情人id")
    private long lid;

    public static final int FIELD_train_type =8;
    @DataBaseField(type = "int(11)", fieldname = "train_type", comment = "培养类型：1=亲密度；2=吸引力；3=加护力；4=羁绊升级；5=星辉升级")
    private int train_type;

    public static final int FIELD_event =9;
    @DataBaseField(type = "int(11)", fieldname = "event", comment = "事件id")
    private int event;

    public static final int FIELD_before_attr =10;
    @DataBaseField(type = "int(11)", fieldname = "before_attr", comment = "变更前的属性值")
    private int before_attr;

    public static final int FIELD_final_attr =11;
    @DataBaseField(type = "int(11)", fieldname = "final_attr", comment = "变更后的属性值")
    private int final_attr;

    public static final int FIELD_timestamp =12;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjConsortAttributeLogBO() {
        id = 0;
        cid = 0L;
        uid = "";
        vip_lv = 0;
        server_id = 0;
        platform = 0;
        region = 0;
        create_time = 0;
        lid = 0L;
        train_type = 0;
        event = 0;
        before_attr = 0;
        final_attr = 0;
        timestamp = 0;
    }

    public MjConsortAttributeLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        uid = rs.getString(3);
        vip_lv = rs.getInt(4);
        server_id = rs.getInt(5);
        platform = rs.getInt(6);
        region = rs.getInt(7);
        create_time = rs.getInt(8);
        lid = rs.getLong(9);
        train_type = rs.getInt(10);
        event = rs.getInt(11);
        before_attr = rs.getInt(12);
        final_attr = rs.getInt(13);
        timestamp = rs.getInt(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjConsortAttributeLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `lid`, `train_type`, `event`, `before_attr`, `final_attr`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_consort_attribute_log`";
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
        strBuf.append("'").append(lid).append("', ");
        strBuf.append("'").append(train_type).append("', ");
        strBuf.append("'").append(event).append("', ");
        strBuf.append("'").append(before_attr).append("', ");
        strBuf.append("'").append(final_attr).append("', ");
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

    // 情人id
    public long getLid() { return this.lid; }
    public void setLid(BM _bm, long lid) {
        if(lid==this.lid) 
            return;
        this.lid = lid; 
        markField(_bm, FIELD_lid); 
    }
    public void saveLid(BM _bm, long lid) {
        if(lid==this.lid) 
            return;
        this.lid = lid;
        saveField(_bm, "lid", lid);
    }

    // 培养类型：1=亲密度；2=吸引力；3=加护力；4=羁绊升级；5=星辉升级
    public int getTrainType() { return this.train_type; }
    public void setTrainType(BM _bm, int train_type) {
        if(train_type==this.train_type) 
            return;
        this.train_type = train_type; 
        markField(_bm, FIELD_train_type); 
    }
    public void saveTrainType(BM _bm, int train_type) {
        if(train_type==this.train_type) 
            return;
        this.train_type = train_type;
        saveField(_bm, "train_type", train_type);
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

    // 变更前的属性值
    public int getBeforeAttr() { return this.before_attr; }
    public void setBeforeAttr(BM _bm, int before_attr) {
        if(before_attr==this.before_attr) 
            return;
        this.before_attr = before_attr; 
        markField(_bm, FIELD_before_attr); 
    }
    public void saveBeforeAttr(BM _bm, int before_attr) {
        if(before_attr==this.before_attr) 
            return;
        this.before_attr = before_attr;
        saveField(_bm, "before_attr", before_attr);
    }

    // 变更后的属性值
    public int getFinalAttr() { return this.final_attr; }
    public void setFinalAttr(BM _bm, int final_attr) {
        if(final_attr==this.final_attr) 
            return;
        this.final_attr = final_attr; 
        markField(_bm, FIELD_final_attr); 
    }
    public void saveFinalAttr(BM _bm, int final_attr) {
        if(final_attr==this.final_attr) 
            return;
        this.final_attr = final_attr;
        saveField(_bm, "final_attr", final_attr);
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
        sBuilder.append(" `lid` = '").append(lid).append("',");
        sBuilder.append(" `train_type` = '").append(train_type).append("',");
        sBuilder.append(" `event` = '").append(event).append("',");
        sBuilder.append(" `before_attr` = '").append(before_attr).append("',");
        sBuilder.append(" `final_attr` = '").append(final_attr).append("',");
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
        if(isFieldMarked(FIELD_lid)) sBuilder.append(" `lid` = '").append(lid).append("',");
        if(isFieldMarked(FIELD_train_type)) sBuilder.append(" `train_type` = '").append(train_type).append("',");
        if(isFieldMarked(FIELD_event)) sBuilder.append(" `event` = '").append(event).append("',");
        if(isFieldMarked(FIELD_before_attr)) sBuilder.append(" `before_attr` = '").append(before_attr).append("',");
        if(isFieldMarked(FIELD_final_attr)) sBuilder.append(" `final_attr` = '").append(final_attr).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_consort_attribute_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`lid` bigint(20) NOT NULL DEFAULT '0' COMMENT '情人id',"
                + "`train_type` int(11) NOT NULL DEFAULT '0' COMMENT '培养类型：1=亲密度；2=吸引力；3=加护力；4=羁绊升级；5=星辉升级',"
                + "`event` int(11) NOT NULL DEFAULT '0' COMMENT '事件id',"
                + "`before_attr` int(11) NOT NULL DEFAULT '0' COMMENT '变更前的属性值',"
                + "`final_attr` int(11) NOT NULL DEFAULT '0' COMMENT '变更后的属性值',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-情人亲密度&吸引力&加护力变更记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//lid
        _size+=4;//train_type
        _size+=4;//event
        _size+=4;//before_attr
        _size+=4;//final_attr
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
        buff.putLong(lid);
        buff.putInt(train_type);
        buff.putInt(event);
        buff.putInt(before_attr);
        buff.putInt(final_attr);
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
        lid=buff.getLong();
        train_type=buff.getInt();
        event=buff.getInt();
        before_attr=buff.getInt();
        final_attr=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
