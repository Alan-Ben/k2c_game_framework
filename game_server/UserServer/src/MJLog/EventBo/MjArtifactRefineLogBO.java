package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjArtifactRefineLogBO extends MJEventLogBo {

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

    public static final int FIELD_aid =7;
    @DataBaseField(type = "bigint(20)", fieldname = "aid", comment = "藏品id")
    private long aid;

    public static final int FIELD_collection_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "collection_id", comment = "藏品唯一id（数据库id）")
    private long collection_id;

    public static final int FIELD_a_level =9;
    @DataBaseField(type = "int(11)", fieldname = "a_level", comment = "藏品等级")
    private int a_level;

    public static final int FIELD_a_aptitude =10;
    @DataBaseField(type = "int(11)", fieldname = "a_aptitude", comment = "藏品资质点")
    private int a_aptitude;

    public static final int FIELD_fid =11;
    @DataBaseField(type = "bigint(20)", fieldname = "fid", comment = "伙伴id，无伙伴则记0")
    private long fid;

    public static final int FIELD_op_type =12;
    @DataBaseField(type = "int(11)", fieldname = "op_type", comment = "操作类型：1=洗练")
    private int op_type;

    public static final int FIELD_is_succeed =13;
    @DataBaseField(type = "int(11)", fieldname = "is_succeed", comment = "本次洗练是否成功：1=成功；0=未成功")
    private int is_succeed;

    public static final int FIELD_initial_attr =14;
    @DataBaseField(type = "int(11)", fieldname = "initial_attr", comment = "变更前的属性值（加成万分比）")
    private int initial_attr;

    public static final int FIELD_final_attr =15;
    @DataBaseField(type = "int(11)", fieldname = "final_attr", comment = "变更后的属性值（加成万分比）")
    private int final_attr;

    public static final int FIELD_timestamp =16;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjArtifactRefineLogBO() {
        id = 0;
        cid = 0L;
        uid = "";
        vip_lv = 0;
        server_id = 0;
        platform = 0;
        region = 0;
        create_time = 0;
        aid = 0L;
        collection_id = 0L;
        a_level = 0;
        a_aptitude = 0;
        fid = 0L;
        op_type = 0;
        is_succeed = 0;
        initial_attr = 0;
        final_attr = 0;
        timestamp = 0;
    }

    public MjArtifactRefineLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        uid = rs.getString(3);
        vip_lv = rs.getInt(4);
        server_id = rs.getInt(5);
        platform = rs.getInt(6);
        region = rs.getInt(7);
        create_time = rs.getInt(8);
        aid = rs.getLong(9);
        collection_id = rs.getLong(10);
        a_level = rs.getInt(11);
        a_aptitude = rs.getInt(12);
        fid = rs.getLong(13);
        op_type = rs.getInt(14);
        is_succeed = rs.getInt(15);
        initial_attr = rs.getInt(16);
        final_attr = rs.getInt(17);
        timestamp = rs.getInt(18);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjArtifactRefineLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `aid`, `collection_id`, `a_level`, `a_aptitude`, `fid`, `op_type`, `is_succeed`, `initial_attr`, `final_attr`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_artifact_refine_log`";
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
        strBuf.append("'").append(aid).append("', ");
        strBuf.append("'").append(collection_id).append("', ");
        strBuf.append("'").append(a_level).append("', ");
        strBuf.append("'").append(a_aptitude).append("', ");
        strBuf.append("'").append(fid).append("', ");
        strBuf.append("'").append(op_type).append("', ");
        strBuf.append("'").append(is_succeed).append("', ");
        strBuf.append("'").append(initial_attr).append("', ");
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

    // 藏品id
    public long getAid() { return this.aid; }
    public void setAid(BM _bm, long aid) {
        if(aid==this.aid) 
            return;
        this.aid = aid; 
        markField(_bm, FIELD_aid); 
    }
    public void saveAid(BM _bm, long aid) {
        if(aid==this.aid) 
            return;
        this.aid = aid;
        saveField(_bm, "aid", aid);
    }

    // 藏品唯一id（数据库id）
    public long getCollectionId() { return this.collection_id; }
    public void setCollectionId(BM _bm, long collection_id) {
        if(collection_id==this.collection_id) 
            return;
        this.collection_id = collection_id; 
        markField(_bm, FIELD_collection_id); 
    }
    public void saveCollectionId(BM _bm, long collection_id) {
        if(collection_id==this.collection_id) 
            return;
        this.collection_id = collection_id;
        saveField(_bm, "collection_id", collection_id);
    }

    // 藏品等级
    public int getALevel() { return this.a_level; }
    public void setALevel(BM _bm, int a_level) {
        if(a_level==this.a_level) 
            return;
        this.a_level = a_level; 
        markField(_bm, FIELD_a_level); 
    }
    public void saveALevel(BM _bm, int a_level) {
        if(a_level==this.a_level) 
            return;
        this.a_level = a_level;
        saveField(_bm, "a_level", a_level);
    }

    // 藏品资质点
    public int getAAptitude() { return this.a_aptitude; }
    public void setAAptitude(BM _bm, int a_aptitude) {
        if(a_aptitude==this.a_aptitude) 
            return;
        this.a_aptitude = a_aptitude; 
        markField(_bm, FIELD_a_aptitude); 
    }
    public void saveAAptitude(BM _bm, int a_aptitude) {
        if(a_aptitude==this.a_aptitude) 
            return;
        this.a_aptitude = a_aptitude;
        saveField(_bm, "a_aptitude", a_aptitude);
    }

    // 伙伴id，无伙伴则记0
    public long getFid() { return this.fid; }
    public void setFid(BM _bm, long fid) {
        if(fid==this.fid) 
            return;
        this.fid = fid; 
        markField(_bm, FIELD_fid); 
    }
    public void saveFid(BM _bm, long fid) {
        if(fid==this.fid) 
            return;
        this.fid = fid;
        saveField(_bm, "fid", fid);
    }

    // 操作类型：1=洗练
    public int getOpType() { return this.op_type; }
    public void setOpType(BM _bm, int op_type) {
        if(op_type==this.op_type) 
            return;
        this.op_type = op_type; 
        markField(_bm, FIELD_op_type); 
    }
    public void saveOpType(BM _bm, int op_type) {
        if(op_type==this.op_type) 
            return;
        this.op_type = op_type;
        saveField(_bm, "op_type", op_type);
    }

    // 本次洗练是否成功：1=成功；0=未成功
    public int getIsSucceed() { return this.is_succeed; }
    public void setIsSucceed(BM _bm, int is_succeed) {
        if(is_succeed==this.is_succeed) 
            return;
        this.is_succeed = is_succeed; 
        markField(_bm, FIELD_is_succeed); 
    }
    public void saveIsSucceed(BM _bm, int is_succeed) {
        if(is_succeed==this.is_succeed) 
            return;
        this.is_succeed = is_succeed;
        saveField(_bm, "is_succeed", is_succeed);
    }

    // 变更前的属性值（加成万分比）
    public int getInitialAttr() { return this.initial_attr; }
    public void setInitialAttr(BM _bm, int initial_attr) {
        if(initial_attr==this.initial_attr) 
            return;
        this.initial_attr = initial_attr; 
        markField(_bm, FIELD_initial_attr); 
    }
    public void saveInitialAttr(BM _bm, int initial_attr) {
        if(initial_attr==this.initial_attr) 
            return;
        this.initial_attr = initial_attr;
        saveField(_bm, "initial_attr", initial_attr);
    }

    // 变更后的属性值（加成万分比）
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
        sBuilder.append(" `aid` = '").append(aid).append("',");
        sBuilder.append(" `collection_id` = '").append(collection_id).append("',");
        sBuilder.append(" `a_level` = '").append(a_level).append("',");
        sBuilder.append(" `a_aptitude` = '").append(a_aptitude).append("',");
        sBuilder.append(" `fid` = '").append(fid).append("',");
        sBuilder.append(" `op_type` = '").append(op_type).append("',");
        sBuilder.append(" `is_succeed` = '").append(is_succeed).append("',");
        sBuilder.append(" `initial_attr` = '").append(initial_attr).append("',");
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
        if(isFieldMarked(FIELD_aid)) sBuilder.append(" `aid` = '").append(aid).append("',");
        if(isFieldMarked(FIELD_collection_id)) sBuilder.append(" `collection_id` = '").append(collection_id).append("',");
        if(isFieldMarked(FIELD_a_level)) sBuilder.append(" `a_level` = '").append(a_level).append("',");
        if(isFieldMarked(FIELD_a_aptitude)) sBuilder.append(" `a_aptitude` = '").append(a_aptitude).append("',");
        if(isFieldMarked(FIELD_fid)) sBuilder.append(" `fid` = '").append(fid).append("',");
        if(isFieldMarked(FIELD_op_type)) sBuilder.append(" `op_type` = '").append(op_type).append("',");
        if(isFieldMarked(FIELD_is_succeed)) sBuilder.append(" `is_succeed` = '").append(is_succeed).append("',");
        if(isFieldMarked(FIELD_initial_attr)) sBuilder.append(" `initial_attr` = '").append(initial_attr).append("',");
        if(isFieldMarked(FIELD_final_attr)) sBuilder.append(" `final_attr` = '").append(final_attr).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_artifact_refine_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`aid` bigint(20) NOT NULL DEFAULT '0' COMMENT '藏品id',"
                + "`collection_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '藏品唯一id（数据库id）',"
                + "`a_level` int(11) NOT NULL DEFAULT '0' COMMENT '藏品等级',"
                + "`a_aptitude` int(11) NOT NULL DEFAULT '0' COMMENT '藏品资质点',"
                + "`fid` bigint(20) NOT NULL DEFAULT '0' COMMENT '伙伴id，无伙伴则记0',"
                + "`op_type` int(11) NOT NULL DEFAULT '0' COMMENT '操作类型：1=洗练',"
                + "`is_succeed` int(11) NOT NULL DEFAULT '0' COMMENT '本次洗练是否成功：1=成功；0=未成功',"
                + "`initial_attr` int(11) NOT NULL DEFAULT '0' COMMENT '变更前的属性值（加成万分比）',"
                + "`final_attr` int(11) NOT NULL DEFAULT '0' COMMENT '变更后的属性值（加成万分比）',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-藏品洗练记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//aid
        _size+=8;//collection_id
        _size+=4;//a_level
        _size+=4;//a_aptitude
        _size+=8;//fid
        _size+=4;//op_type
        _size+=4;//is_succeed
        _size+=4;//initial_attr
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
        buff.putLong(aid);
        buff.putLong(collection_id);
        buff.putInt(a_level);
        buff.putInt(a_aptitude);
        buff.putLong(fid);
        buff.putInt(op_type);
        buff.putInt(is_succeed);
        buff.putInt(initial_attr);
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
        aid=buff.getLong();
        collection_id=buff.getLong();
        a_level=buff.getInt();
        a_aptitude=buff.getInt();
        fid=buff.getLong();
        op_type=buff.getInt();
        is_succeed=buff.getInt();
        initial_attr=buff.getInt();
        final_attr=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
