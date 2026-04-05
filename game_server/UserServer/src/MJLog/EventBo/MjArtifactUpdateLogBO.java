package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjArtifactUpdateLogBO extends MJEventLogBo {

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
    @DataBaseField(type = "bigint(20)", fieldname = "collection_id", comment = "藏品实例id")
    private long collection_id;

    public static final int FIELD_fid =9;
    @DataBaseField(type = "bigint(20)", fieldname = "fid", comment = "伙伴id，无伙伴则记0")
    private long fid;

    public static final int FIELD_initial_lv =10;
    @DataBaseField(type = "int(11)", fieldname = "initial_lv", comment = "升级前的等级")
    private int initial_lv;

    public static final int FIELD_initial_aptitude =11;
    @DataBaseField(type = "int(11)", fieldname = "initial_aptitude", comment = "升级前的资质点")
    private int initial_aptitude;

    public static final int FIELD_final_lv =12;
    @DataBaseField(type = "int(11)", fieldname = "final_lv", comment = "升级后的等级")
    private int final_lv;

    public static final int FIELD_final_aptitude =13;
    @DataBaseField(type = "int(11)", fieldname = "final_aptitude", comment = "升级后的资质点")
    private int final_aptitude;

    public static final int FIELD_timestamp =14;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjArtifactUpdateLogBO() {
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
        fid = 0L;
        initial_lv = 0;
        initial_aptitude = 0;
        final_lv = 0;
        final_aptitude = 0;
        timestamp = 0;
    }

    public MjArtifactUpdateLogBO(ResultSet rs) throws Exception {
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
        fid = rs.getLong(11);
        initial_lv = rs.getInt(12);
        initial_aptitude = rs.getInt(13);
        final_lv = rs.getInt(14);
        final_aptitude = rs.getInt(15);
        timestamp = rs.getInt(16);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjArtifactUpdateLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `aid`, `collection_id`, `fid`, `initial_lv`, `initial_aptitude`, `final_lv`, `final_aptitude`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_artifact_update_log`";
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
        strBuf.append("'").append(fid).append("', ");
        strBuf.append("'").append(initial_lv).append("', ");
        strBuf.append("'").append(initial_aptitude).append("', ");
        strBuf.append("'").append(final_lv).append("', ");
        strBuf.append("'").append(final_aptitude).append("', ");
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

    // 藏品实例id
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

    // 升级前的等级
    public int getInitialLv() { return this.initial_lv; }
    public void setInitialLv(BM _bm, int initial_lv) {
        if(initial_lv==this.initial_lv) 
            return;
        this.initial_lv = initial_lv; 
        markField(_bm, FIELD_initial_lv); 
    }
    public void saveInitialLv(BM _bm, int initial_lv) {
        if(initial_lv==this.initial_lv) 
            return;
        this.initial_lv = initial_lv;
        saveField(_bm, "initial_lv", initial_lv);
    }

    // 升级前的资质点
    public int getInitialAptitude() { return this.initial_aptitude; }
    public void setInitialAptitude(BM _bm, int initial_aptitude) {
        if(initial_aptitude==this.initial_aptitude) 
            return;
        this.initial_aptitude = initial_aptitude; 
        markField(_bm, FIELD_initial_aptitude); 
    }
    public void saveInitialAptitude(BM _bm, int initial_aptitude) {
        if(initial_aptitude==this.initial_aptitude) 
            return;
        this.initial_aptitude = initial_aptitude;
        saveField(_bm, "initial_aptitude", initial_aptitude);
    }

    // 升级后的等级
    public int getFinalLv() { return this.final_lv; }
    public void setFinalLv(BM _bm, int final_lv) {
        if(final_lv==this.final_lv) 
            return;
        this.final_lv = final_lv; 
        markField(_bm, FIELD_final_lv); 
    }
    public void saveFinalLv(BM _bm, int final_lv) {
        if(final_lv==this.final_lv) 
            return;
        this.final_lv = final_lv;
        saveField(_bm, "final_lv", final_lv);
    }

    // 升级后的资质点
    public int getFinalAptitude() { return this.final_aptitude; }
    public void setFinalAptitude(BM _bm, int final_aptitude) {
        if(final_aptitude==this.final_aptitude) 
            return;
        this.final_aptitude = final_aptitude; 
        markField(_bm, FIELD_final_aptitude); 
    }
    public void saveFinalAptitude(BM _bm, int final_aptitude) {
        if(final_aptitude==this.final_aptitude) 
            return;
        this.final_aptitude = final_aptitude;
        saveField(_bm, "final_aptitude", final_aptitude);
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
        sBuilder.append(" `fid` = '").append(fid).append("',");
        sBuilder.append(" `initial_lv` = '").append(initial_lv).append("',");
        sBuilder.append(" `initial_aptitude` = '").append(initial_aptitude).append("',");
        sBuilder.append(" `final_lv` = '").append(final_lv).append("',");
        sBuilder.append(" `final_aptitude` = '").append(final_aptitude).append("',");
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
        if(isFieldMarked(FIELD_fid)) sBuilder.append(" `fid` = '").append(fid).append("',");
        if(isFieldMarked(FIELD_initial_lv)) sBuilder.append(" `initial_lv` = '").append(initial_lv).append("',");
        if(isFieldMarked(FIELD_initial_aptitude)) sBuilder.append(" `initial_aptitude` = '").append(initial_aptitude).append("',");
        if(isFieldMarked(FIELD_final_lv)) sBuilder.append(" `final_lv` = '").append(final_lv).append("',");
        if(isFieldMarked(FIELD_final_aptitude)) sBuilder.append(" `final_aptitude` = '").append(final_aptitude).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_artifact_update_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`aid` bigint(20) NOT NULL DEFAULT '0' COMMENT '藏品id',"
                + "`collection_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '藏品实例id',"
                + "`fid` bigint(20) NOT NULL DEFAULT '0' COMMENT '伙伴id，无伙伴则记0',"
                + "`initial_lv` int(11) NOT NULL DEFAULT '0' COMMENT '升级前的等级',"
                + "`initial_aptitude` int(11) NOT NULL DEFAULT '0' COMMENT '升级前的资质点',"
                + "`final_lv` int(11) NOT NULL DEFAULT '0' COMMENT '升级后的等级',"
                + "`final_aptitude` int(11) NOT NULL DEFAULT '0' COMMENT '升级后的资质点',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-藏品升级记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//fid
        _size+=4;//initial_lv
        _size+=4;//initial_aptitude
        _size+=4;//final_lv
        _size+=4;//final_aptitude
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
        buff.putLong(fid);
        buff.putInt(initial_lv);
        buff.putInt(initial_aptitude);
        buff.putInt(final_lv);
        buff.putInt(final_aptitude);
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
        fid=buff.getLong();
        initial_lv=buff.getInt();
        initial_aptitude=buff.getInt();
        final_lv=buff.getInt();
        final_aptitude=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
