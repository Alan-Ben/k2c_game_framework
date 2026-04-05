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
public class PlayerInnBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_level =1;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_medal_level =2;
    @DataBaseField(type = "int(11)", fieldname = "medal_level", comment = "奖牌等级")
    private int medal_level;

    public static final int FIELD_popularity =3;
    @DataBaseField(type = "bigint(20)", fieldname = "popularity", comment = "人气值")
    private long popularity;

    public static final int FIELD_range_had_settle_count =4;
    @DataBaseField(type = "int(11)", fieldname = "range_had_settle_count", comment = "区间内已处理次数")
    private int range_had_settle_count;

    public static final int FIELD_range_had_gain_count =5;
    @DataBaseField(type = "int(11)", fieldname = "range_had_gain_count", comment = "区间内已获得数量")
    private int range_had_gain_count;

    public static final int FIELD_first_time_upgrade_time_ms =6;
    @DataBaseField(type = "bigint(20)", fieldname = "first_time_upgrade_time_ms", comment = "首次升级时间")
    private long first_time_upgrade_time_ms;

    public static final int FIELD_receive_list =7;
    @DataBaseField(type = "blob", fieldname = "receive_list", comment = "接待列表")
    private byte[] receive_list;

    public PlayerInnBO() {
        id = 0;
        cid = 0L;
        level = 0;
        medal_level = 0;
        popularity = 0L;
        range_had_settle_count = 0;
        range_had_gain_count = 0;
        first_time_upgrade_time_ms = 0L;
        receive_list = null;
    }

    public PlayerInnBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        level = rs.getInt(3);
        medal_level = rs.getInt(4);
        popularity = rs.getLong(5);
        range_had_settle_count = rs.getInt(6);
        range_had_gain_count = rs.getInt(7);
        first_time_upgrade_time_ms = rs.getLong(8);
        receive_list = rs.getBytes(9);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerInnBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `level`, `medal_level`, `popularity`, `range_had_settle_count`, `range_had_gain_count`, `first_time_upgrade_time_ms`, `receive_list`";
    }

    @Override
    public String getTableName() {
        return "`player_inn`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(medal_level).append("', ");
        strBuf.append("'").append(popularity).append("', ");
        strBuf.append("'").append(range_had_settle_count).append("', ");
        strBuf.append("'").append(range_had_gain_count).append("', ");
        strBuf.append("'").append(first_time_upgrade_time_ms).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(receive_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_receive_list)) ret.add(receive_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家CID
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

    // 等级
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

    // 奖牌等级
    public int getMedalLevel() { return this.medal_level; }
    public void setMedalLevel(BM _bm, int medal_level) {
        if(medal_level==this.medal_level) 
            return;
        this.medal_level = medal_level; 
        markField(_bm, FIELD_medal_level); 
    }
    public void saveMedalLevel(BM _bm, int medal_level) {
        if(medal_level==this.medal_level) 
            return;
        this.medal_level = medal_level;
        saveField(_bm, "medal_level", medal_level);
    }

    // 人气值
    public long getPopularity() { return this.popularity; }
    public void setPopularity(BM _bm, long popularity) {
        if(popularity==this.popularity) 
            return;
        this.popularity = popularity; 
        markField(_bm, FIELD_popularity); 
    }
    public void savePopularity(BM _bm, long popularity) {
        if(popularity==this.popularity) 
            return;
        this.popularity = popularity;
        saveField(_bm, "popularity", popularity);
    }

    // 区间内已处理次数
    public int getRangeHadSettleCount() { return this.range_had_settle_count; }
    public void setRangeHadSettleCount(BM _bm, int range_had_settle_count) {
        if(range_had_settle_count==this.range_had_settle_count) 
            return;
        this.range_had_settle_count = range_had_settle_count; 
        markField(_bm, FIELD_range_had_settle_count); 
    }
    public void saveRangeHadSettleCount(BM _bm, int range_had_settle_count) {
        if(range_had_settle_count==this.range_had_settle_count) 
            return;
        this.range_had_settle_count = range_had_settle_count;
        saveField(_bm, "range_had_settle_count", range_had_settle_count);
    }

    // 区间内已获得数量
    public int getRangeHadGainCount() { return this.range_had_gain_count; }
    public void setRangeHadGainCount(BM _bm, int range_had_gain_count) {
        if(range_had_gain_count==this.range_had_gain_count) 
            return;
        this.range_had_gain_count = range_had_gain_count; 
        markField(_bm, FIELD_range_had_gain_count); 
    }
    public void saveRangeHadGainCount(BM _bm, int range_had_gain_count) {
        if(range_had_gain_count==this.range_had_gain_count) 
            return;
        this.range_had_gain_count = range_had_gain_count;
        saveField(_bm, "range_had_gain_count", range_had_gain_count);
    }

    // 首次升级时间
    public long getFirstTimeUpgradeTimeMs() { return this.first_time_upgrade_time_ms; }
    public void setFirstTimeUpgradeTimeMs(BM _bm, long first_time_upgrade_time_ms) {
        if(first_time_upgrade_time_ms==this.first_time_upgrade_time_ms) 
            return;
        this.first_time_upgrade_time_ms = first_time_upgrade_time_ms; 
        markField(_bm, FIELD_first_time_upgrade_time_ms); 
    }
    public void saveFirstTimeUpgradeTimeMs(BM _bm, long first_time_upgrade_time_ms) {
        if(first_time_upgrade_time_ms==this.first_time_upgrade_time_ms) 
            return;
        this.first_time_upgrade_time_ms = first_time_upgrade_time_ms;
        saveField(_bm, "first_time_upgrade_time_ms", first_time_upgrade_time_ms);
    }

    // 接待列表
    public byte[] getReceiveList() { return this.receive_list; }
    public void setReceiveList(BM _bm, byte[] receive_list) {
        if(receive_list==this.receive_list) 
            return;
        this.receive_list = receive_list; 
        markField(_bm, FIELD_receive_list); 
    }
    public void saveReceiveList(BM _bm, byte[] receive_list) {
        if(receive_list==this.receive_list) 
            return;
        this.receive_list = receive_list;
        saveFieldBytes(_bm, "receive_list", receive_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `medal_level` = '").append(medal_level).append("',");
        sBuilder.append(" `popularity` = '").append(popularity).append("',");
        sBuilder.append(" `range_had_settle_count` = '").append(range_had_settle_count).append("',");
        sBuilder.append(" `range_had_gain_count` = '").append(range_had_gain_count).append("',");
        sBuilder.append(" `first_time_upgrade_time_ms` = '").append(first_time_upgrade_time_ms).append("',");
        sBuilder.append(" `receive_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_medal_level)) sBuilder.append(" `medal_level` = '").append(medal_level).append("',");
        if(isFieldMarked(FIELD_popularity)) sBuilder.append(" `popularity` = '").append(popularity).append("',");
        if(isFieldMarked(FIELD_range_had_settle_count)) sBuilder.append(" `range_had_settle_count` = '").append(range_had_settle_count).append("',");
        if(isFieldMarked(FIELD_range_had_gain_count)) sBuilder.append(" `range_had_gain_count` = '").append(range_had_gain_count).append("',");
        if(isFieldMarked(FIELD_first_time_upgrade_time_ms)) sBuilder.append(" `first_time_upgrade_time_ms` = '").append(first_time_upgrade_time_ms).append("',");
        if(isFieldMarked(FIELD_receive_list)) sBuilder.append(" `receive_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_inn` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`medal_level` int(11) NOT NULL DEFAULT '0' COMMENT '奖牌等级',"
                + "`popularity` bigint(20) NOT NULL DEFAULT '0' COMMENT '人气值',"
                + "`range_had_settle_count` int(11) NOT NULL DEFAULT '0' COMMENT '区间内已处理次数',"
                + "`range_had_gain_count` int(11) NOT NULL DEFAULT '0' COMMENT '区间内已获得数量',"
                + "`first_time_upgrade_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '首次升级时间',"
                + "`receive_list` blob NULL COMMENT '接待列表',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家旅店数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//level
        _size+=4;//medal_level
        _size+=8;//popularity
        _size+=4;//range_had_settle_count
        _size+=4;//range_had_gain_count
        _size+=8;//first_time_upgrade_time_ms
        _size+=2;_size+=receive_list.length;//receive_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(level);
        buff.putInt(medal_level);
        buff.putLong(popularity);
        buff.putInt(range_had_settle_count);
        buff.putInt(range_had_gain_count);
        buff.putLong(first_time_upgrade_time_ms);
        buff.putShort((short)(receive_list == null ? 0 : receive_list.length));if(null != receive_list){buff.put(receive_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        level=buff.getInt();
        medal_level=buff.getInt();
        popularity=buff.getLong();
        range_had_settle_count=buff.getInt();
        range_had_gain_count=buff.getInt();
        first_time_upgrade_time_ms=buff.getLong();
        int receive_list_count = buff.getShort();if(receive_list_count>0){receive_list = new byte[receive_list_count];buff.get(receive_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
