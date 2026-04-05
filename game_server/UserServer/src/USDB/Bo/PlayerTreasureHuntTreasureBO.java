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
public class PlayerTreasureHuntTreasureBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_treasure_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "treasure_id", comment = "奇物ID")
    private long treasure_id;

    public static final int FIELD_gain_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "gain_time_ms", comment = "获得时间 ms")
    private long gain_time_ms;

    public static final int FIELD_skill_level =3;
    @DataBaseField(type = "int(11)", fieldname = "skill_level", comment = "技能等级")
    private int skill_level;

    public PlayerTreasureHuntTreasureBO() {
        id = 0;
        cid = 0L;
        treasure_id = 0L;
        gain_time_ms = 0L;
        skill_level = 0;
    }

    public PlayerTreasureHuntTreasureBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        treasure_id = rs.getLong(3);
        gain_time_ms = rs.getLong(4);
        skill_level = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTreasureHuntTreasureBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `treasure_id`, `gain_time_ms`, `skill_level`";
    }

    @Override
    public String getTableName() {
        return "`player_treasure_hunt_treasure`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(treasure_id).append("', ");
        strBuf.append("'").append(gain_time_ms).append("', ");
        strBuf.append("'").append(skill_level).append("', ");
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

    // 奇物ID
    public long getTreasureId() { return this.treasure_id; }
    public void setTreasureId(BM _bm, long treasure_id) {
        if(treasure_id==this.treasure_id) 
            return;
        this.treasure_id = treasure_id; 
        markField(_bm, FIELD_treasure_id); 
    }
    public void saveTreasureId(BM _bm, long treasure_id) {
        if(treasure_id==this.treasure_id) 
            return;
        this.treasure_id = treasure_id;
        saveField(_bm, "treasure_id", treasure_id);
    }

    // 获得时间 ms
    public long getGainTimeMs() { return this.gain_time_ms; }
    public void setGainTimeMs(BM _bm, long gain_time_ms) {
        if(gain_time_ms==this.gain_time_ms) 
            return;
        this.gain_time_ms = gain_time_ms; 
        markField(_bm, FIELD_gain_time_ms); 
    }
    public void saveGainTimeMs(BM _bm, long gain_time_ms) {
        if(gain_time_ms==this.gain_time_ms) 
            return;
        this.gain_time_ms = gain_time_ms;
        saveField(_bm, "gain_time_ms", gain_time_ms);
    }

    // 技能等级
    public int getSkillLevel() { return this.skill_level; }
    public void setSkillLevel(BM _bm, int skill_level) {
        if(skill_level==this.skill_level) 
            return;
        this.skill_level = skill_level; 
        markField(_bm, FIELD_skill_level); 
    }
    public void saveSkillLevel(BM _bm, int skill_level) {
        if(skill_level==this.skill_level) 
            return;
        this.skill_level = skill_level;
        saveField(_bm, "skill_level", skill_level);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `treasure_id` = '").append(treasure_id).append("',");
        sBuilder.append(" `gain_time_ms` = '").append(gain_time_ms).append("',");
        sBuilder.append(" `skill_level` = '").append(skill_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_treasure_id)) sBuilder.append(" `treasure_id` = '").append(treasure_id).append("',");
        if(isFieldMarked(FIELD_gain_time_ms)) sBuilder.append(" `gain_time_ms` = '").append(gain_time_ms).append("',");
        if(isFieldMarked(FIELD_skill_level)) sBuilder.append(" `skill_level` = '").append(skill_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_treasure_hunt_treasure` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`treasure_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奇物ID',"
                + "`gain_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '获得时间 ms',"
                + "`skill_level` int(11) NOT NULL DEFAULT '0' COMMENT '技能等级',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家太空寻宝奇物数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//treasure_id
        _size+=8;//gain_time_ms
        _size+=4;//skill_level
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(treasure_id);
        buff.putLong(gain_time_ms);
        buff.putInt(skill_level);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        treasure_id=buff.getLong();
        gain_time_ms=buff.getLong();
        skill_level=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
