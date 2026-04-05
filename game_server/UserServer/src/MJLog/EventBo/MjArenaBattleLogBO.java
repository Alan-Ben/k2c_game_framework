package MJLog.EventBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class MjArenaBattleLogBO extends MJEventLogBo {

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

    public static final int FIELD_fid =7;
    @DataBaseField(type = "bigint(20)", fieldname = "fid", comment = "伙伴id")
    private long fid;

    public static final int FIELD_rival_cid =8;
    @DataBaseField(type = "bigint(20)", fieldname = "rival_cid", comment = "对手cid")
    private long rival_cid;

    public static final int FIELD_rival_hp =9;
    @DataBaseField(type = "bigint(20)", fieldname = "rival_hp", comment = "对手总血量")
    private long rival_hp;

    public static final int FIELD_kill_num =10;
    @DataBaseField(type = "int(11)", fieldname = "kill_num", comment = "击败对手伙伴数量")
    private int kill_num;

    public static final int FIELD_initial_hp =11;
    @DataBaseField(type = "bigint(20)", fieldname = "initial_hp", comment = "起始血量")
    private long initial_hp;

    public static final int FIELD_final_hp =12;
    @DataBaseField(type = "bigint(20)", fieldname = "final_hp", comment = "剩余血量")
    private long final_hp;

    public static final int FIELD_buff_list =13;
    @DataBaseField(type = "varchar(2000)", fieldname = "buff_list", comment = "增益明细")
    private String buff_list;

    public static final int FIELD_chg_score =14;
    @DataBaseField(type = "bigint(20)", fieldname = "chg_score", comment = "变更积分")
    private long chg_score;

    public static final int FIELD_timestamp =15;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "事件发生时间戳(10位)")
    private int timestamp;

    public MjArenaBattleLogBO() {
        id = 0;
        cid = 0L;
        uid = "";
        vip_lv = 0;
        server_id = 0;
        platform = 0;
        region = 0;
        create_time = 0;
        fid = 0L;
        rival_cid = 0L;
        rival_hp = 0L;
        kill_num = 0;
        initial_hp = 0L;
        final_hp = 0L;
        buff_list = "";
        chg_score = 0L;
        timestamp = 0;
    }

    public MjArenaBattleLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        uid = rs.getString(3);
        vip_lv = rs.getInt(4);
        server_id = rs.getInt(5);
        platform = rs.getInt(6);
        region = rs.getInt(7);
        create_time = rs.getInt(8);
        fid = rs.getLong(9);
        rival_cid = rs.getLong(10);
        rival_hp = rs.getLong(11);
        kill_num = rs.getInt(12);
        initial_hp = rs.getLong(13);
        final_hp = rs.getLong(14);
        buff_list = rs.getString(15);
        chg_score = rs.getLong(16);
        timestamp = rs.getInt(17);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MjArenaBattleLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `uid`, `vip_lv`, `server_id`, `platform`, `region`, `create_time`, `fid`, `rival_cid`, `rival_hp`, `kill_num`, `initial_hp`, `final_hp`, `buff_list`, `chg_score`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`mj_arena_battle_log`";
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
        strBuf.append("'").append(fid).append("', ");
        strBuf.append("'").append(rival_cid).append("', ");
        strBuf.append("'").append(rival_hp).append("', ");
        strBuf.append("'").append(kill_num).append("', ");
        strBuf.append("'").append(initial_hp).append("', ");
        strBuf.append("'").append(final_hp).append("', ");
        strBuf.append("'").append(buff_list == null ? null : buff_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(chg_score).append("', ");
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

    // 伙伴id
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

    // 对手cid
    public long getRivalCid() { return this.rival_cid; }
    public void setRivalCid(BM _bm, long rival_cid) {
        if(rival_cid==this.rival_cid) 
            return;
        this.rival_cid = rival_cid; 
        markField(_bm, FIELD_rival_cid); 
    }
    public void saveRivalCid(BM _bm, long rival_cid) {
        if(rival_cid==this.rival_cid) 
            return;
        this.rival_cid = rival_cid;
        saveField(_bm, "rival_cid", rival_cid);
    }

    // 对手总血量
    public long getRivalHp() { return this.rival_hp; }
    public void setRivalHp(BM _bm, long rival_hp) {
        if(rival_hp==this.rival_hp) 
            return;
        this.rival_hp = rival_hp; 
        markField(_bm, FIELD_rival_hp); 
    }
    public void saveRivalHp(BM _bm, long rival_hp) {
        if(rival_hp==this.rival_hp) 
            return;
        this.rival_hp = rival_hp;
        saveField(_bm, "rival_hp", rival_hp);
    }

    // 击败对手伙伴数量
    public int getKillNum() { return this.kill_num; }
    public void setKillNum(BM _bm, int kill_num) {
        if(kill_num==this.kill_num) 
            return;
        this.kill_num = kill_num; 
        markField(_bm, FIELD_kill_num); 
    }
    public void saveKillNum(BM _bm, int kill_num) {
        if(kill_num==this.kill_num) 
            return;
        this.kill_num = kill_num;
        saveField(_bm, "kill_num", kill_num);
    }

    // 起始血量
    public long getInitialHp() { return this.initial_hp; }
    public void setInitialHp(BM _bm, long initial_hp) {
        if(initial_hp==this.initial_hp) 
            return;
        this.initial_hp = initial_hp; 
        markField(_bm, FIELD_initial_hp); 
    }
    public void saveInitialHp(BM _bm, long initial_hp) {
        if(initial_hp==this.initial_hp) 
            return;
        this.initial_hp = initial_hp;
        saveField(_bm, "initial_hp", initial_hp);
    }

    // 剩余血量
    public long getFinalHp() { return this.final_hp; }
    public void setFinalHp(BM _bm, long final_hp) {
        if(final_hp==this.final_hp) 
            return;
        this.final_hp = final_hp; 
        markField(_bm, FIELD_final_hp); 
    }
    public void saveFinalHp(BM _bm, long final_hp) {
        if(final_hp==this.final_hp) 
            return;
        this.final_hp = final_hp;
        saveField(_bm, "final_hp", final_hp);
    }

    // 增益明细
    public String getBuffList() { return this.buff_list; }
    public void setBuffList(BM _bm, String buff_list) {
        if(buff_list.equals(this.buff_list)) 
            return;
        this.buff_list = buff_list; 
        markField(_bm, FIELD_buff_list); 
    }
    public void saveBuffList(BM _bm, String buff_list) {
        if(buff_list.equals(this.buff_list)) 
            return;
        this.buff_list = buff_list;
        saveField(_bm, "buff_list", buff_list);
    }

    // 变更积分
    public long getChgScore() { return this.chg_score; }
    public void setChgScore(BM _bm, long chg_score) {
        if(chg_score==this.chg_score) 
            return;
        this.chg_score = chg_score; 
        markField(_bm, FIELD_chg_score); 
    }
    public void saveChgScore(BM _bm, long chg_score) {
        if(chg_score==this.chg_score) 
            return;
        this.chg_score = chg_score;
        saveField(_bm, "chg_score", chg_score);
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
        sBuilder.append(" `fid` = '").append(fid).append("',");
        sBuilder.append(" `rival_cid` = '").append(rival_cid).append("',");
        sBuilder.append(" `rival_hp` = '").append(rival_hp).append("',");
        sBuilder.append(" `kill_num` = '").append(kill_num).append("',");
        sBuilder.append(" `initial_hp` = '").append(initial_hp).append("',");
        sBuilder.append(" `final_hp` = '").append(final_hp).append("',");
        sBuilder.append(" `buff_list` = '").append(buff_list == null ? null : buff_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `chg_score` = '").append(chg_score).append("',");
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
        if(isFieldMarked(FIELD_fid)) sBuilder.append(" `fid` = '").append(fid).append("',");
        if(isFieldMarked(FIELD_rival_cid)) sBuilder.append(" `rival_cid` = '").append(rival_cid).append("',");
        if(isFieldMarked(FIELD_rival_hp)) sBuilder.append(" `rival_hp` = '").append(rival_hp).append("',");
        if(isFieldMarked(FIELD_kill_num)) sBuilder.append(" `kill_num` = '").append(kill_num).append("',");
        if(isFieldMarked(FIELD_initial_hp)) sBuilder.append(" `initial_hp` = '").append(initial_hp).append("',");
        if(isFieldMarked(FIELD_final_hp)) sBuilder.append(" `final_hp` = '").append(final_hp).append("',");
        if(isFieldMarked(FIELD_buff_list)) sBuilder.append(" `buff_list` = '").append(buff_list == null ? null : buff_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_chg_score)) sBuilder.append(" `chg_score` = '").append(chg_score).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mj_arena_battle_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '玩家VIP等级',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` int(11) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角时间',"
                + "`fid` bigint(20) NOT NULL DEFAULT '0' COMMENT '伙伴id',"
                + "`rival_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '对手cid',"
                + "`rival_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '对手总血量',"
                + "`kill_num` int(11) NOT NULL DEFAULT '0' COMMENT '击败对手伙伴数量',"
                + "`initial_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '起始血量',"
                + "`final_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '剩余血量',"
                + "`buff_list` varchar(2000) NOT NULL DEFAULT '' COMMENT '增益明细',"
                + "`chg_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '变更积分',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时间戳(10位)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-碰撞测试挑战记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//fid
        _size+=8;//rival_cid
        _size+=8;//rival_hp
        _size+=4;//kill_num
        _size+=8;//initial_hp
        _size+=8;//final_hp
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(buff_list);//buff_list
        _size+=8;//chg_score
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
        buff.putLong(fid);
        buff.putLong(rival_cid);
        buff.putLong(rival_hp);
        buff.putInt(kill_num);
        buff.putLong(initial_hp);
        buff.putLong(final_hp);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, buff_list);
        buff.putLong(chg_score);
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
        fid=buff.getLong();
        rival_cid=buff.getLong();
        rival_hp=buff.getLong();
        kill_num=buff.getInt();
        initial_hp=buff.getLong();
        final_hp=buff.getLong();
        buff_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        chg_score=buff.getLong();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
