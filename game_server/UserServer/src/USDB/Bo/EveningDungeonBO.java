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
public class EveningDungeonBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_round_preview_time_ms =0;
    @DataBaseField(type = "bigint(20)", fieldname = "round_preview_time_ms", comment = "本轮预告时间")
    private long round_preview_time_ms;

    public static final int FIELD_round_start_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "round_start_time_ms", comment = "本轮开始时间")
    private long round_start_time_ms;

    public static final int FIELD_round_end_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "round_end_time_ms", comment = "本轮结束时间")
    private long round_end_time_ms;

    public static final int FIELD_pre_round_close_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "pre_round_close_time_ms", comment = "上轮结束时间")
    private long pre_round_close_time_ms;

    public static final int FIELD_had_settle =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_settle", comment = "本轮是否结算")
    private boolean had_settle;

    public static final int FIELD_reborn_times =5;
    @DataBaseField(type = "int(11)", fieldname = "reborn_times", comment = "复活次数")
    private int reborn_times;

    public static final int FIELD_base_hp =6;
    @DataBaseField(type = "bigint(20)", fieldname = "base_hp", comment = "基准血量")
    private long base_hp;

    public static final int FIELD_total_hp =7;
    @DataBaseField(type = "bigint(20)", fieldname = "total_hp", comment = "总血量")
    private long total_hp;

    public static final int FIELD_deducted_hp =8;
    @DataBaseField(type = "bigint(20)", fieldname = "deducted_hp", comment = "扣除血量")
    private long deducted_hp;

    public static final int FIELD_defeat_cid =9;
    @DataBaseField(type = "bigint(20)", fieldname = "defeat_cid", comment = "击杀玩家ID")
    private long defeat_cid;

    public static final int FIELD_defeat_time_ms =10;
    @DataBaseField(type = "bigint(20)", fieldname = "defeat_time_ms", comment = "击杀时间")
    private long defeat_time_ms;

    public static final int FIELD_rank_instance_id =11;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_instance_id", comment = "排行榜实例id")
    private long rank_instance_id;

    public static final int FIELD_had_reset_rank =12;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_reset_rank", comment = "是否重置 排行榜")
    private boolean had_reset_rank;

    public static final int FIELD_server_start_day =13;
    @DataBaseField(type = "int(11)", fieldname = "server_start_day", comment = "服务器开服天数")
    private int server_start_day;

    public EveningDungeonBO() {
        id = 0;
        round_preview_time_ms = 0L;
        round_start_time_ms = 0L;
        round_end_time_ms = 0L;
        pre_round_close_time_ms = 0L;
        had_settle = false;
        reborn_times = 0;
        base_hp = 0L;
        total_hp = 0L;
        deducted_hp = 0L;
        defeat_cid = 0L;
        defeat_time_ms = 0L;
        rank_instance_id = 0L;
        had_reset_rank = false;
        server_start_day = 0;
    }

    public EveningDungeonBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        round_preview_time_ms = rs.getLong(2);
        round_start_time_ms = rs.getLong(3);
        round_end_time_ms = rs.getLong(4);
        pre_round_close_time_ms = rs.getLong(5);
        had_settle = rs.getBoolean(6);
        reborn_times = rs.getInt(7);
        base_hp = rs.getLong(8);
        total_hp = rs.getLong(9);
        deducted_hp = rs.getLong(10);
        defeat_cid = rs.getLong(11);
        defeat_time_ms = rs.getLong(12);
        rank_instance_id = rs.getLong(13);
        had_reset_rank = rs.getBoolean(14);
        server_start_day = rs.getInt(15);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new EveningDungeonBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `round_preview_time_ms`, `round_start_time_ms`, `round_end_time_ms`, `pre_round_close_time_ms`, `had_settle`, `reborn_times`, `base_hp`, `total_hp`, `deducted_hp`, `defeat_cid`, `defeat_time_ms`, `rank_instance_id`, `had_reset_rank`, `server_start_day`";
    }

    @Override
    public String getTableName() {
        return "`evening_dungeon`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(round_preview_time_ms).append("', ");
        strBuf.append("'").append(round_start_time_ms).append("', ");
        strBuf.append("'").append(round_end_time_ms).append("', ");
        strBuf.append("'").append(pre_round_close_time_ms).append("', ");
        strBuf.append("'").append(had_settle ? 1 : 0).append("', ");
        strBuf.append("'").append(reborn_times).append("', ");
        strBuf.append("'").append(base_hp).append("', ");
        strBuf.append("'").append(total_hp).append("', ");
        strBuf.append("'").append(deducted_hp).append("', ");
        strBuf.append("'").append(defeat_cid).append("', ");
        strBuf.append("'").append(defeat_time_ms).append("', ");
        strBuf.append("'").append(rank_instance_id).append("', ");
        strBuf.append("'").append(had_reset_rank ? 1 : 0).append("', ");
        strBuf.append("'").append(server_start_day).append("', ");
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

    // 本轮预告时间
    public long getRoundPreviewTimeMs() { return this.round_preview_time_ms; }
    public void setRoundPreviewTimeMs(BM _bm, long round_preview_time_ms) {
        if(round_preview_time_ms==this.round_preview_time_ms) 
            return;
        this.round_preview_time_ms = round_preview_time_ms; 
        markField(_bm, FIELD_round_preview_time_ms); 
    }
    public void saveRoundPreviewTimeMs(BM _bm, long round_preview_time_ms) {
        if(round_preview_time_ms==this.round_preview_time_ms) 
            return;
        this.round_preview_time_ms = round_preview_time_ms;
        saveField(_bm, "round_preview_time_ms", round_preview_time_ms);
    }

    // 本轮开始时间
    public long getRoundStartTimeMs() { return this.round_start_time_ms; }
    public void setRoundStartTimeMs(BM _bm, long round_start_time_ms) {
        if(round_start_time_ms==this.round_start_time_ms) 
            return;
        this.round_start_time_ms = round_start_time_ms; 
        markField(_bm, FIELD_round_start_time_ms); 
    }
    public void saveRoundStartTimeMs(BM _bm, long round_start_time_ms) {
        if(round_start_time_ms==this.round_start_time_ms) 
            return;
        this.round_start_time_ms = round_start_time_ms;
        saveField(_bm, "round_start_time_ms", round_start_time_ms);
    }

    // 本轮结束时间
    public long getRoundEndTimeMs() { return this.round_end_time_ms; }
    public void setRoundEndTimeMs(BM _bm, long round_end_time_ms) {
        if(round_end_time_ms==this.round_end_time_ms) 
            return;
        this.round_end_time_ms = round_end_time_ms; 
        markField(_bm, FIELD_round_end_time_ms); 
    }
    public void saveRoundEndTimeMs(BM _bm, long round_end_time_ms) {
        if(round_end_time_ms==this.round_end_time_ms) 
            return;
        this.round_end_time_ms = round_end_time_ms;
        saveField(_bm, "round_end_time_ms", round_end_time_ms);
    }

    // 上轮结束时间
    public long getPreRoundCloseTimeMs() { return this.pre_round_close_time_ms; }
    public void setPreRoundCloseTimeMs(BM _bm, long pre_round_close_time_ms) {
        if(pre_round_close_time_ms==this.pre_round_close_time_ms) 
            return;
        this.pre_round_close_time_ms = pre_round_close_time_ms; 
        markField(_bm, FIELD_pre_round_close_time_ms); 
    }
    public void savePreRoundCloseTimeMs(BM _bm, long pre_round_close_time_ms) {
        if(pre_round_close_time_ms==this.pre_round_close_time_ms) 
            return;
        this.pre_round_close_time_ms = pre_round_close_time_ms;
        saveField(_bm, "pre_round_close_time_ms", pre_round_close_time_ms);
    }

    // 本轮是否结算
    public boolean getHadSettle() { return this.had_settle; }
    public void setHadSettle(BM _bm, boolean had_settle) {
        if(had_settle==this.had_settle) 
            return;
        this.had_settle = had_settle; 
        markField(_bm, FIELD_had_settle); 
    }
    public void saveHadSettle(BM _bm, boolean had_settle) {
        if(had_settle==this.had_settle) 
            return;
        this.had_settle = had_settle;
        saveField(_bm, "had_settle", had_settle ? 1 : 0);
    }

    // 复活次数
    public int getRebornTimes() { return this.reborn_times; }
    public void setRebornTimes(BM _bm, int reborn_times) {
        if(reborn_times==this.reborn_times) 
            return;
        this.reborn_times = reborn_times; 
        markField(_bm, FIELD_reborn_times); 
    }
    public void saveRebornTimes(BM _bm, int reborn_times) {
        if(reborn_times==this.reborn_times) 
            return;
        this.reborn_times = reborn_times;
        saveField(_bm, "reborn_times", reborn_times);
    }

    // 基准血量
    public long getBaseHp() { return this.base_hp; }
    public void setBaseHp(BM _bm, long base_hp) {
        if(base_hp==this.base_hp) 
            return;
        this.base_hp = base_hp; 
        markField(_bm, FIELD_base_hp); 
    }
    public void saveBaseHp(BM _bm, long base_hp) {
        if(base_hp==this.base_hp) 
            return;
        this.base_hp = base_hp;
        saveField(_bm, "base_hp", base_hp);
    }

    // 总血量
    public long getTotalHp() { return this.total_hp; }
    public void setTotalHp(BM _bm, long total_hp) {
        if(total_hp==this.total_hp) 
            return;
        this.total_hp = total_hp; 
        markField(_bm, FIELD_total_hp); 
    }
    public void saveTotalHp(BM _bm, long total_hp) {
        if(total_hp==this.total_hp) 
            return;
        this.total_hp = total_hp;
        saveField(_bm, "total_hp", total_hp);
    }

    // 扣除血量
    public long getDeductedHp() { return this.deducted_hp; }
    public void setDeductedHp(BM _bm, long deducted_hp) {
        if(deducted_hp==this.deducted_hp) 
            return;
        this.deducted_hp = deducted_hp; 
        markField(_bm, FIELD_deducted_hp); 
    }
    public void saveDeductedHp(BM _bm, long deducted_hp) {
        if(deducted_hp==this.deducted_hp) 
            return;
        this.deducted_hp = deducted_hp;
        saveField(_bm, "deducted_hp", deducted_hp);
    }

    // 击杀玩家ID
    public long getDefeatCid() { return this.defeat_cid; }
    public void setDefeatCid(BM _bm, long defeat_cid) {
        if(defeat_cid==this.defeat_cid) 
            return;
        this.defeat_cid = defeat_cid; 
        markField(_bm, FIELD_defeat_cid); 
    }
    public void saveDefeatCid(BM _bm, long defeat_cid) {
        if(defeat_cid==this.defeat_cid) 
            return;
        this.defeat_cid = defeat_cid;
        saveField(_bm, "defeat_cid", defeat_cid);
    }

    // 击杀时间
    public long getDefeatTimeMs() { return this.defeat_time_ms; }
    public void setDefeatTimeMs(BM _bm, long defeat_time_ms) {
        if(defeat_time_ms==this.defeat_time_ms) 
            return;
        this.defeat_time_ms = defeat_time_ms; 
        markField(_bm, FIELD_defeat_time_ms); 
    }
    public void saveDefeatTimeMs(BM _bm, long defeat_time_ms) {
        if(defeat_time_ms==this.defeat_time_ms) 
            return;
        this.defeat_time_ms = defeat_time_ms;
        saveField(_bm, "defeat_time_ms", defeat_time_ms);
    }

    // 排行榜实例id
    public long getRankInstanceId() { return this.rank_instance_id; }
    public void setRankInstanceId(BM _bm, long rank_instance_id) {
        if(rank_instance_id==this.rank_instance_id) 
            return;
        this.rank_instance_id = rank_instance_id; 
        markField(_bm, FIELD_rank_instance_id); 
    }
    public void saveRankInstanceId(BM _bm, long rank_instance_id) {
        if(rank_instance_id==this.rank_instance_id) 
            return;
        this.rank_instance_id = rank_instance_id;
        saveField(_bm, "rank_instance_id", rank_instance_id);
    }

    // 是否重置 排行榜
    public boolean getHadResetRank() { return this.had_reset_rank; }
    public void setHadResetRank(BM _bm, boolean had_reset_rank) {
        if(had_reset_rank==this.had_reset_rank) 
            return;
        this.had_reset_rank = had_reset_rank; 
        markField(_bm, FIELD_had_reset_rank); 
    }
    public void saveHadResetRank(BM _bm, boolean had_reset_rank) {
        if(had_reset_rank==this.had_reset_rank) 
            return;
        this.had_reset_rank = had_reset_rank;
        saveField(_bm, "had_reset_rank", had_reset_rank ? 1 : 0);
    }

    // 服务器开服天数
    public int getServerStartDay() { return this.server_start_day; }
    public void setServerStartDay(BM _bm, int server_start_day) {
        if(server_start_day==this.server_start_day) 
            return;
        this.server_start_day = server_start_day; 
        markField(_bm, FIELD_server_start_day); 
    }
    public void saveServerStartDay(BM _bm, int server_start_day) {
        if(server_start_day==this.server_start_day) 
            return;
        this.server_start_day = server_start_day;
        saveField(_bm, "server_start_day", server_start_day);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `round_preview_time_ms` = '").append(round_preview_time_ms).append("',");
        sBuilder.append(" `round_start_time_ms` = '").append(round_start_time_ms).append("',");
        sBuilder.append(" `round_end_time_ms` = '").append(round_end_time_ms).append("',");
        sBuilder.append(" `pre_round_close_time_ms` = '").append(pre_round_close_time_ms).append("',");
        sBuilder.append(" `had_settle` = '").append(had_settle ? 1 : 0).append("',");
        sBuilder.append(" `reborn_times` = '").append(reborn_times).append("',");
        sBuilder.append(" `base_hp` = '").append(base_hp).append("',");
        sBuilder.append(" `total_hp` = '").append(total_hp).append("',");
        sBuilder.append(" `deducted_hp` = '").append(deducted_hp).append("',");
        sBuilder.append(" `defeat_cid` = '").append(defeat_cid).append("',");
        sBuilder.append(" `defeat_time_ms` = '").append(defeat_time_ms).append("',");
        sBuilder.append(" `rank_instance_id` = '").append(rank_instance_id).append("',");
        sBuilder.append(" `had_reset_rank` = '").append(had_reset_rank ? 1 : 0).append("',");
        sBuilder.append(" `server_start_day` = '").append(server_start_day).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_round_preview_time_ms)) sBuilder.append(" `round_preview_time_ms` = '").append(round_preview_time_ms).append("',");
        if(isFieldMarked(FIELD_round_start_time_ms)) sBuilder.append(" `round_start_time_ms` = '").append(round_start_time_ms).append("',");
        if(isFieldMarked(FIELD_round_end_time_ms)) sBuilder.append(" `round_end_time_ms` = '").append(round_end_time_ms).append("',");
        if(isFieldMarked(FIELD_pre_round_close_time_ms)) sBuilder.append(" `pre_round_close_time_ms` = '").append(pre_round_close_time_ms).append("',");
        if(isFieldMarked(FIELD_had_settle)) sBuilder.append(" `had_settle` = '").append(had_settle ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_reborn_times)) sBuilder.append(" `reborn_times` = '").append(reborn_times).append("',");
        if(isFieldMarked(FIELD_base_hp)) sBuilder.append(" `base_hp` = '").append(base_hp).append("',");
        if(isFieldMarked(FIELD_total_hp)) sBuilder.append(" `total_hp` = '").append(total_hp).append("',");
        if(isFieldMarked(FIELD_deducted_hp)) sBuilder.append(" `deducted_hp` = '").append(deducted_hp).append("',");
        if(isFieldMarked(FIELD_defeat_cid)) sBuilder.append(" `defeat_cid` = '").append(defeat_cid).append("',");
        if(isFieldMarked(FIELD_defeat_time_ms)) sBuilder.append(" `defeat_time_ms` = '").append(defeat_time_ms).append("',");
        if(isFieldMarked(FIELD_rank_instance_id)) sBuilder.append(" `rank_instance_id` = '").append(rank_instance_id).append("',");
        if(isFieldMarked(FIELD_had_reset_rank)) sBuilder.append(" `had_reset_rank` = '").append(had_reset_rank ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_server_start_day)) sBuilder.append(" `server_start_day` = '").append(server_start_day).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `evening_dungeon` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`round_preview_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮预告时间',"
                + "`round_start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮开始时间',"
                + "`round_end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮结束时间',"
                + "`pre_round_close_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上轮结束时间',"
                + "`had_settle` tinyint(1) NOT NULL DEFAULT '0' COMMENT '本轮是否结算',"
                + "`reborn_times` int(11) NOT NULL DEFAULT '0' COMMENT '复活次数',"
                + "`base_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '基准血量',"
                + "`total_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '总血量',"
                + "`deducted_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '扣除血量',"
                + "`defeat_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '击杀玩家ID',"
                + "`defeat_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '击杀时间',"
                + "`rank_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜实例id',"
                + "`had_reset_rank` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否重置 排行榜',"
                + "`server_start_day` int(11) NOT NULL DEFAULT '0' COMMENT '服务器开服天数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='晚间副本' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//round_preview_time_ms
        _size+=8;//round_start_time_ms
        _size+=8;//round_end_time_ms
        _size+=8;//pre_round_close_time_ms
        _size+=1;//had_settle
        _size+=4;//reborn_times
        _size+=8;//base_hp
        _size+=8;//total_hp
        _size+=8;//deducted_hp
        _size+=8;//defeat_cid
        _size+=8;//defeat_time_ms
        _size+=8;//rank_instance_id
        _size+=1;//had_reset_rank
        _size+=4;//server_start_day
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(round_preview_time_ms);
        buff.putLong(round_start_time_ms);
        buff.putLong(round_end_time_ms);
        buff.putLong(pre_round_close_time_ms);
        buff.put((byte)(had_settle?1:0));
        buff.putInt(reborn_times);
        buff.putLong(base_hp);
        buff.putLong(total_hp);
        buff.putLong(deducted_hp);
        buff.putLong(defeat_cid);
        buff.putLong(defeat_time_ms);
        buff.putLong(rank_instance_id);
        buff.put((byte)(had_reset_rank?1:0));
        buff.putInt(server_start_day);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        round_preview_time_ms=buff.getLong();
        round_start_time_ms=buff.getLong();
        round_end_time_ms=buff.getLong();
        pre_round_close_time_ms=buff.getLong();
        had_settle=(buff.get()==1);
        reborn_times=buff.getInt();
        base_hp=buff.getLong();
        total_hp=buff.getLong();
        deducted_hp=buff.getLong();
        defeat_cid=buff.getLong();
        defeat_time_ms=buff.getLong();
        rank_instance_id=buff.getLong();
        had_reset_rank=(buff.get()==1);
        server_start_day=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
