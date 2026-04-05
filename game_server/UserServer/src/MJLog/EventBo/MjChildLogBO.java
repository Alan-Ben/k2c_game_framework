package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjChildLogBO extends MJEventLogBo {

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

    public static final int FIELD_uniqe_id =7;
    @DataBaseField(type = "bigint(20)", fieldname = "uniqe_id", comment = "子嗣唯一ID")
    private long uniqe_id;

    public static final int FIELD_child_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "child_id", comment = "子嗣ID")
    private long child_id;

    public static final int FIELD_attribute =9;
    @DataBaseField(type = "bigint(20)", fieldname = "attribute", comment = "子嗣属性")
    private long attribute;

    public static final int FIELD_loversid =10;
    @DataBaseField(type = "bigint(20)", fieldname = "loversid", comment = "子嗣对应情人")
    private long loversid;

    public static final int FIELD_child_ep =11;
    @DataBaseField(type = "bigint(20)", fieldname = "child_ep", comment = "子嗣培养进度")
    private long child_ep;

    public static final int FIELD_stateid =12;
    @DataBaseField(type = "int(11)", fieldname = "stateid", comment = "子嗣状态")
    private int stateid;

    public static final int FIELD_sex_id =13;
    @DataBaseField(type = "int(11)", fieldname = "sex_id", comment = "子嗣性别")
    private int sex_id;

    public static final int FIELD_quality =14;
    @DataBaseField(type = "bigint(20)", fieldname = "quality", comment = "子嗣品质")
    private long quality;

    public static final int FIELD_time =15;
    @DataBaseField(type = "int(11)", fieldname = "time", comment = "获得时间戳")
    private int time;

    public static final int FIELD_timestamp =16;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjChildLogBO() {
        id = 0;
        cid = 0L;
        uid = "";
        vip_lv = 0;
        server_id = 0;
        platform = 0;
        region = 0;
        create_time = 0;
        uniqe_id = 0L;
        child_id = 0L;
        attribute = 0L;
        loversid = 0L;
        child_ep = 0L;
        stateid = 0;
        sex_id = 0;
        quality = 0L;
        time = 0;
        timestamp = 0;
    }

    public MjChildLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        uid = rs.getString(3);
        vip_lv = rs.getInt(4);
        server_id = rs.getInt(5);
        platform = rs.getInt(6);
        region = rs.getInt(7);
        create_time = rs.getInt(8);
        uniqe_id = rs.getLong(9);
        child_id = rs.getLong(10);
        attribute = rs.getLong(11);
        loversid = rs.getLong(12);
        child_ep = rs.getLong(13);
        stateid = rs.getInt(14);
        sex_id = rs.getInt(15);
        quality = rs.getLong(16);
        time = rs.getInt(17);
        timestamp = rs.getInt(18);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjChildLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `uniqe_id`, `child_id`, `attribute`, `loversid`, `child_ep`, `stateid`, `sex_id`, `quality`, `time`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_child_log`";
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
        strBuf.append("'").append(uniqe_id).append("', ");
        strBuf.append("'").append(child_id).append("', ");
        strBuf.append("'").append(attribute).append("', ");
        strBuf.append("'").append(loversid).append("', ");
        strBuf.append("'").append(child_ep).append("', ");
        strBuf.append("'").append(stateid).append("', ");
        strBuf.append("'").append(sex_id).append("', ");
        strBuf.append("'").append(quality).append("', ");
        strBuf.append("'").append(time).append("', ");
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

    // 子嗣唯一ID
    public long getUniqeId() { return this.uniqe_id; }
    public void setUniqeId(BM _bm, long uniqe_id) {
        if(uniqe_id==this.uniqe_id) 
            return;
        this.uniqe_id = uniqe_id; 
        markField(_bm, FIELD_uniqe_id); 
    }
    public void saveUniqeId(BM _bm, long uniqe_id) {
        if(uniqe_id==this.uniqe_id) 
            return;
        this.uniqe_id = uniqe_id;
        saveField(_bm, "uniqe_id", uniqe_id);
    }

    // 子嗣ID
    public long getChildId() { return this.child_id; }
    public void setChildId(BM _bm, long child_id) {
        if(child_id==this.child_id) 
            return;
        this.child_id = child_id; 
        markField(_bm, FIELD_child_id); 
    }
    public void saveChildId(BM _bm, long child_id) {
        if(child_id==this.child_id) 
            return;
        this.child_id = child_id;
        saveField(_bm, "child_id", child_id);
    }

    // 子嗣属性
    public long getAttribute() { return this.attribute; }
    public void setAttribute(BM _bm, long attribute) {
        if(attribute==this.attribute) 
            return;
        this.attribute = attribute; 
        markField(_bm, FIELD_attribute); 
    }
    public void saveAttribute(BM _bm, long attribute) {
        if(attribute==this.attribute) 
            return;
        this.attribute = attribute;
        saveField(_bm, "attribute", attribute);
    }

    // 子嗣对应情人
    public long getLoversid() { return this.loversid; }
    public void setLoversid(BM _bm, long loversid) {
        if(loversid==this.loversid) 
            return;
        this.loversid = loversid; 
        markField(_bm, FIELD_loversid); 
    }
    public void saveLoversid(BM _bm, long loversid) {
        if(loversid==this.loversid) 
            return;
        this.loversid = loversid;
        saveField(_bm, "loversid", loversid);
    }

    // 子嗣培养进度
    public long getChildEp() { return this.child_ep; }
    public void setChildEp(BM _bm, long child_ep) {
        if(child_ep==this.child_ep) 
            return;
        this.child_ep = child_ep; 
        markField(_bm, FIELD_child_ep); 
    }
    public void saveChildEp(BM _bm, long child_ep) {
        if(child_ep==this.child_ep) 
            return;
        this.child_ep = child_ep;
        saveField(_bm, "child_ep", child_ep);
    }

    // 子嗣状态
    public int getStateid() { return this.stateid; }
    public void setStateid(BM _bm, int stateid) {
        if(stateid==this.stateid) 
            return;
        this.stateid = stateid; 
        markField(_bm, FIELD_stateid); 
    }
    public void saveStateid(BM _bm, int stateid) {
        if(stateid==this.stateid) 
            return;
        this.stateid = stateid;
        saveField(_bm, "stateid", stateid);
    }

    // 子嗣性别
    public int getSexId() { return this.sex_id; }
    public void setSexId(BM _bm, int sex_id) {
        if(sex_id==this.sex_id) 
            return;
        this.sex_id = sex_id; 
        markField(_bm, FIELD_sex_id); 
    }
    public void saveSexId(BM _bm, int sex_id) {
        if(sex_id==this.sex_id) 
            return;
        this.sex_id = sex_id;
        saveField(_bm, "sex_id", sex_id);
    }

    // 子嗣品质
    public long getQuality() { return this.quality; }
    public void setQuality(BM _bm, long quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality; 
        markField(_bm, FIELD_quality); 
    }
    public void saveQuality(BM _bm, long quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality;
        saveField(_bm, "quality", quality);
    }

    // 获得时间戳
    public int getTime() { return this.time; }
    public void setTime(BM _bm, int time) {
        if(time==this.time) 
            return;
        this.time = time; 
        markField(_bm, FIELD_time); 
    }
    public void saveTime(BM _bm, int time) {
        if(time==this.time) 
            return;
        this.time = time;
        saveField(_bm, "time", time);
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
        sBuilder.append(" `uniqe_id` = '").append(uniqe_id).append("',");
        sBuilder.append(" `child_id` = '").append(child_id).append("',");
        sBuilder.append(" `attribute` = '").append(attribute).append("',");
        sBuilder.append(" `loversid` = '").append(loversid).append("',");
        sBuilder.append(" `child_ep` = '").append(child_ep).append("',");
        sBuilder.append(" `stateid` = '").append(stateid).append("',");
        sBuilder.append(" `sex_id` = '").append(sex_id).append("',");
        sBuilder.append(" `quality` = '").append(quality).append("',");
        sBuilder.append(" `time` = '").append(time).append("',");
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
        if(isFieldMarked(FIELD_uniqe_id)) sBuilder.append(" `uniqe_id` = '").append(uniqe_id).append("',");
        if(isFieldMarked(FIELD_child_id)) sBuilder.append(" `child_id` = '").append(child_id).append("',");
        if(isFieldMarked(FIELD_attribute)) sBuilder.append(" `attribute` = '").append(attribute).append("',");
        if(isFieldMarked(FIELD_loversid)) sBuilder.append(" `loversid` = '").append(loversid).append("',");
        if(isFieldMarked(FIELD_child_ep)) sBuilder.append(" `child_ep` = '").append(child_ep).append("',");
        if(isFieldMarked(FIELD_stateid)) sBuilder.append(" `stateid` = '").append(stateid).append("',");
        if(isFieldMarked(FIELD_sex_id)) sBuilder.append(" `sex_id` = '").append(sex_id).append("',");
        if(isFieldMarked(FIELD_quality)) sBuilder.append(" `quality` = '").append(quality).append("',");
        if(isFieldMarked(FIELD_time)) sBuilder.append(" `time` = '").append(time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_child_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`uniqe_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣唯一ID',"
                + "`child_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣ID',"
                + "`attribute` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣属性',"
                + "`loversid` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣对应情人',"
                + "`child_ep` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣培养进度',"
                + "`stateid` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣状态',"
                + "`sex_id` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣性别',"
                + "`quality` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣品质',"
                + "`time` int(11) NOT NULL DEFAULT '0' COMMENT '获得时间戳',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-子嗣数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//uniqe_id
        _size+=8;//child_id
        _size+=8;//attribute
        _size+=8;//loversid
        _size+=8;//child_ep
        _size+=4;//stateid
        _size+=4;//sex_id
        _size+=8;//quality
        _size+=4;//time
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
        buff.putLong(uniqe_id);
        buff.putLong(child_id);
        buff.putLong(attribute);
        buff.putLong(loversid);
        buff.putLong(child_ep);
        buff.putInt(stateid);
        buff.putInt(sex_id);
        buff.putLong(quality);
        buff.putInt(time);
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
        uniqe_id=buff.getLong();
        child_id=buff.getLong();
        attribute=buff.getLong();
        loversid=buff.getLong();
        child_ep=buff.getLong();
        stateid=buff.getInt();
        sex_id=buff.getInt();
        quality=buff.getLong();
        time=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
