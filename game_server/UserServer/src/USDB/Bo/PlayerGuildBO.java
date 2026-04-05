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
public class PlayerGuildBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public static final int FIELD_guild_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "玩家所属公会Id")
    private long guild_id;

    public static final int FIELD_dispatch_hero_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "dispatch_hero_id", comment = "玩家向公会派遣的大臣Id")
    private long dispatch_hero_id;

    public static final int FIELD_free_cd_join_guild_num =3;
    @DataBaseField(type = "int(11)", fieldname = "free_cd_join_guild_num", comment = "免cd加入联盟次数")
    private int free_cd_join_guild_num;

    public static final int FIELD_join_guild_cd_end_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "join_guild_cd_end_time_ms", comment = "加入联盟cd结束时间戳")
    private long join_guild_cd_end_time_ms;

    public static final int FIELD_day_tag =5;
    @DataBaseField(type = "int(11)", fieldname = "day_tag", comment = "上次重置每日数据的日期")
    private int day_tag;

    public static final int FIELD_day_had_draw_construct_reward_list =6;
    @DataBaseField(type = "blob", fieldname = "day_had_draw_construct_reward_list", comment = "今天已领取建设奖励列表")
    private byte[] day_had_draw_construct_reward_list;

    public PlayerGuildBO() {
        id = 0;
        cid = 0L;
        guild_id = 0L;
        dispatch_hero_id = 0L;
        free_cd_join_guild_num = 0;
        join_guild_cd_end_time_ms = 0L;
        day_tag = 0;
        day_had_draw_construct_reward_list = null;
    }

    public PlayerGuildBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        guild_id = rs.getLong(3);
        dispatch_hero_id = rs.getLong(4);
        free_cd_join_guild_num = rs.getInt(5);
        join_guild_cd_end_time_ms = rs.getLong(6);
        day_tag = rs.getInt(7);
        day_had_draw_construct_reward_list = rs.getBytes(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGuildBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `guild_id`, `dispatch_hero_id`, `free_cd_join_guild_num`, `join_guild_cd_end_time_ms`, `day_tag`, `day_had_draw_construct_reward_list`";
    }

    @Override
    public String getTableName() {
        return "`player_guild`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(dispatch_hero_id).append("', ");
        strBuf.append("'").append(free_cd_join_guild_num).append("', ");
        strBuf.append("'").append(join_guild_cd_end_time_ms).append("', ");
        strBuf.append("'").append(day_tag).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(day_had_draw_construct_reward_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_day_had_draw_construct_reward_list)) ret.add(day_had_draw_construct_reward_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家id
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

    // 玩家所属公会Id
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 玩家向公会派遣的大臣Id
    public long getDispatchHeroId() { return this.dispatch_hero_id; }
    public void setDispatchHeroId(BM _bm, long dispatch_hero_id) {
        if(dispatch_hero_id==this.dispatch_hero_id) 
            return;
        this.dispatch_hero_id = dispatch_hero_id; 
        markField(_bm, FIELD_dispatch_hero_id); 
    }
    public void saveDispatchHeroId(BM _bm, long dispatch_hero_id) {
        if(dispatch_hero_id==this.dispatch_hero_id) 
            return;
        this.dispatch_hero_id = dispatch_hero_id;
        saveField(_bm, "dispatch_hero_id", dispatch_hero_id);
    }

    // 免cd加入联盟次数
    public int getFreeCdJoinGuildNum() { return this.free_cd_join_guild_num; }
    public void setFreeCdJoinGuildNum(BM _bm, int free_cd_join_guild_num) {
        if(free_cd_join_guild_num==this.free_cd_join_guild_num) 
            return;
        this.free_cd_join_guild_num = free_cd_join_guild_num; 
        markField(_bm, FIELD_free_cd_join_guild_num); 
    }
    public void saveFreeCdJoinGuildNum(BM _bm, int free_cd_join_guild_num) {
        if(free_cd_join_guild_num==this.free_cd_join_guild_num) 
            return;
        this.free_cd_join_guild_num = free_cd_join_guild_num;
        saveField(_bm, "free_cd_join_guild_num", free_cd_join_guild_num);
    }

    // 加入联盟cd结束时间戳
    public long getJoinGuildCdEndTimeMs() { return this.join_guild_cd_end_time_ms; }
    public void setJoinGuildCdEndTimeMs(BM _bm, long join_guild_cd_end_time_ms) {
        if(join_guild_cd_end_time_ms==this.join_guild_cd_end_time_ms) 
            return;
        this.join_guild_cd_end_time_ms = join_guild_cd_end_time_ms; 
        markField(_bm, FIELD_join_guild_cd_end_time_ms); 
    }
    public void saveJoinGuildCdEndTimeMs(BM _bm, long join_guild_cd_end_time_ms) {
        if(join_guild_cd_end_time_ms==this.join_guild_cd_end_time_ms) 
            return;
        this.join_guild_cd_end_time_ms = join_guild_cd_end_time_ms;
        saveField(_bm, "join_guild_cd_end_time_ms", join_guild_cd_end_time_ms);
    }

    // 上次重置每日数据的日期
    public int getDayTag() { return this.day_tag; }
    public void setDayTag(BM _bm, int day_tag) {
        if(day_tag==this.day_tag) 
            return;
        this.day_tag = day_tag; 
        markField(_bm, FIELD_day_tag); 
    }
    public void saveDayTag(BM _bm, int day_tag) {
        if(day_tag==this.day_tag) 
            return;
        this.day_tag = day_tag;
        saveField(_bm, "day_tag", day_tag);
    }

    // 今天已领取建设奖励列表
    public byte[] getDayHadDrawConstructRewardList() { return this.day_had_draw_construct_reward_list; }
    public void setDayHadDrawConstructRewardList(BM _bm, byte[] day_had_draw_construct_reward_list) {
        if(day_had_draw_construct_reward_list==this.day_had_draw_construct_reward_list) 
            return;
        this.day_had_draw_construct_reward_list = day_had_draw_construct_reward_list; 
        markField(_bm, FIELD_day_had_draw_construct_reward_list); 
    }
    public void saveDayHadDrawConstructRewardList(BM _bm, byte[] day_had_draw_construct_reward_list) {
        if(day_had_draw_construct_reward_list==this.day_had_draw_construct_reward_list) 
            return;
        this.day_had_draw_construct_reward_list = day_had_draw_construct_reward_list;
        saveFieldBytes(_bm, "day_had_draw_construct_reward_list", day_had_draw_construct_reward_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `dispatch_hero_id` = '").append(dispatch_hero_id).append("',");
        sBuilder.append(" `free_cd_join_guild_num` = '").append(free_cd_join_guild_num).append("',");
        sBuilder.append(" `join_guild_cd_end_time_ms` = '").append(join_guild_cd_end_time_ms).append("',");
        sBuilder.append(" `day_tag` = '").append(day_tag).append("',");
        sBuilder.append(" `day_had_draw_construct_reward_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_dispatch_hero_id)) sBuilder.append(" `dispatch_hero_id` = '").append(dispatch_hero_id).append("',");
        if(isFieldMarked(FIELD_free_cd_join_guild_num)) sBuilder.append(" `free_cd_join_guild_num` = '").append(free_cd_join_guild_num).append("',");
        if(isFieldMarked(FIELD_join_guild_cd_end_time_ms)) sBuilder.append(" `join_guild_cd_end_time_ms` = '").append(join_guild_cd_end_time_ms).append("',");
        if(isFieldMarked(FIELD_day_tag)) sBuilder.append(" `day_tag` = '").append(day_tag).append("',");
        if(isFieldMarked(FIELD_day_had_draw_construct_reward_list)) sBuilder.append(" `day_had_draw_construct_reward_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_guild` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家所属公会Id',"
                + "`dispatch_hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家向公会派遣的大臣Id',"
                + "`free_cd_join_guild_num` int(11) NOT NULL DEFAULT '0' COMMENT '免cd加入联盟次数',"
                + "`join_guild_cd_end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '加入联盟cd结束时间戳',"
                + "`day_tag` int(11) NOT NULL DEFAULT '0' COMMENT '上次重置每日数据的日期',"
                + "`day_had_draw_construct_reward_list` blob NULL COMMENT '今天已领取建设奖励列表',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家联盟数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//guild_id
        _size+=8;//dispatch_hero_id
        _size+=4;//free_cd_join_guild_num
        _size+=8;//join_guild_cd_end_time_ms
        _size+=4;//day_tag
        _size+=2;_size+=day_had_draw_construct_reward_list.length;//day_had_draw_construct_reward_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(guild_id);
        buff.putLong(dispatch_hero_id);
        buff.putInt(free_cd_join_guild_num);
        buff.putLong(join_guild_cd_end_time_ms);
        buff.putInt(day_tag);
        buff.putShort((short)(day_had_draw_construct_reward_list == null ? 0 : day_had_draw_construct_reward_list.length));if(null != day_had_draw_construct_reward_list){buff.put(day_had_draw_construct_reward_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        guild_id=buff.getLong();
        dispatch_hero_id=buff.getLong();
        free_cd_join_guild_num=buff.getInt();
        join_guild_cd_end_time_ms=buff.getLong();
        day_tag=buff.getInt();
        int day_had_draw_construct_reward_list_count = buff.getShort();if(day_had_draw_construct_reward_list_count>0){day_had_draw_construct_reward_list = new byte[day_had_draw_construct_reward_list_count];buff.get(day_had_draw_construct_reward_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
