package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjFishRecordLogBO extends MJEventLogBo {

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

    public static final int FIELD_is_advanced =7;
    @DataBaseField(type = "int(11)", fieldname = "is_advanced", comment = "是否高级寻宝,1=是,2=否")
    private int is_advanced;

    public static final int FIELD_area_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "area_id", comment = "本次飞行所在的飞行点")
    private long area_id;

    public static final int FIELD_fly_distance =9;
    @DataBaseField(type = "bigint(20)", fieldname = "fly_distance", comment = "飞行距离")
    private long fly_distance;

    public static final int FIELD_result_list =10;
    @DataBaseField(type = "varchar(2000)", fieldname = "result_list", comment = "寻宝结果JSON字符串")
    private String result_list;

    public static final int FIELD_total_num =11;
    @DataBaseField(type = "bigint(20)", fieldname = "total_num", comment = "累计获得该矿石数量")
    private long total_num;

    public static final int FIELD_timestamp =12;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjFishRecordLogBO() {
        id = 0;
        cid = 0L;
        uid = "";
        vip_lv = 0;
        server_id = 0;
        platform = 0;
        region = 0;
        create_time = 0;
        is_advanced = 0;
        area_id = 0L;
        fly_distance = 0L;
        result_list = "";
        total_num = 0L;
        timestamp = 0;
    }

    public MjFishRecordLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        uid = rs.getString(3);
        vip_lv = rs.getInt(4);
        server_id = rs.getInt(5);
        platform = rs.getInt(6);
        region = rs.getInt(7);
        create_time = rs.getInt(8);
        is_advanced = rs.getInt(9);
        area_id = rs.getLong(10);
        fly_distance = rs.getLong(11);
        result_list = rs.getString(12);
        total_num = rs.getLong(13);
        timestamp = rs.getInt(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjFishRecordLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `is_advanced`, `area_id`, `fly_distance`, `result_list`, `total_num`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_fish_record_log`";
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
        strBuf.append("'").append(is_advanced).append("', ");
        strBuf.append("'").append(area_id).append("', ");
        strBuf.append("'").append(fly_distance).append("', ");
        strBuf.append("'").append(result_list == null ? null : result_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(total_num).append("', ");
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

    // 是否高级寻宝,1=是,2=否
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

    // 本次飞行所在的飞行点
    public long getAreaId() { return this.area_id; }
    public void setAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id; 
        markField(_bm, FIELD_area_id); 
    }
    public void saveAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id;
        saveField(_bm, "area_id", area_id);
    }

    // 飞行距离
    public long getFlyDistance() { return this.fly_distance; }
    public void setFlyDistance(BM _bm, long fly_distance) {
        if(fly_distance==this.fly_distance) 
            return;
        this.fly_distance = fly_distance; 
        markField(_bm, FIELD_fly_distance); 
    }
    public void saveFlyDistance(BM _bm, long fly_distance) {
        if(fly_distance==this.fly_distance) 
            return;
        this.fly_distance = fly_distance;
        saveField(_bm, "fly_distance", fly_distance);
    }

    // 寻宝结果JSON字符串
    public String getResultList() { return this.result_list; }
    public void setResultList(BM _bm, String result_list) {
        if(result_list.equals(this.result_list)) 
            return;
        this.result_list = result_list; 
        markField(_bm, FIELD_result_list); 
    }
    public void saveResultList(BM _bm, String result_list) {
        if(result_list.equals(this.result_list)) 
            return;
        this.result_list = result_list;
        saveField(_bm, "result_list", result_list);
    }

    // 累计获得该矿石数量
    public long getTotalNum() { return this.total_num; }
    public void setTotalNum(BM _bm, long total_num) {
        if(total_num==this.total_num) 
            return;
        this.total_num = total_num; 
        markField(_bm, FIELD_total_num); 
    }
    public void saveTotalNum(BM _bm, long total_num) {
        if(total_num==this.total_num) 
            return;
        this.total_num = total_num;
        saveField(_bm, "total_num", total_num);
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
        sBuilder.append(" `is_advanced` = '").append(is_advanced).append("',");
        sBuilder.append(" `area_id` = '").append(area_id).append("',");
        sBuilder.append(" `fly_distance` = '").append(fly_distance).append("',");
        sBuilder.append(" `result_list` = '").append(result_list == null ? null : result_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `total_num` = '").append(total_num).append("',");
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
        if(isFieldMarked(FIELD_is_advanced)) sBuilder.append(" `is_advanced` = '").append(is_advanced).append("',");
        if(isFieldMarked(FIELD_area_id)) sBuilder.append(" `area_id` = '").append(area_id).append("',");
        if(isFieldMarked(FIELD_fly_distance)) sBuilder.append(" `fly_distance` = '").append(fly_distance).append("',");
        if(isFieldMarked(FIELD_result_list)) sBuilder.append(" `result_list` = '").append(result_list == null ? null : result_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_total_num)) sBuilder.append(" `total_num` = '").append(total_num).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_fish_record_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`is_advanced` int(11) NOT NULL DEFAULT '0' COMMENT '是否高级寻宝,1=是,2=否',"
                + "`area_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '本次飞行所在的飞行点',"
                + "`fly_distance` bigint(20) NOT NULL DEFAULT '0' COMMENT '飞行距离',"
                + "`result_list` varchar(2000) NOT NULL DEFAULT '' COMMENT '寻宝结果JSON字符串',"
                + "`total_num` bigint(20) NOT NULL DEFAULT '0' COMMENT '累计获得该矿石数量',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-寻宝记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//is_advanced
        _size+=8;//area_id
        _size+=8;//fly_distance
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result_list);//result_list
        _size+=8;//total_num
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
        buff.putInt(is_advanced);
        buff.putLong(area_id);
        buff.putLong(fly_distance);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, result_list);
        buff.putLong(total_num);
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
        is_advanced=buff.getInt();
        area_id=buff.getLong();
        fly_distance=buff.getLong();
        result_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        total_num=buff.getLong();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
