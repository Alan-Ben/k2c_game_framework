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
public class PlayerArenaFightReportBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_opponent_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "opponent_cid", comment = "对手cid")
    private long opponent_cid;

    public static final int FIELD_defeat_hero_num =2;
    @DataBaseField(type = "int(11)", fieldname = "defeat_hero_num", comment = "击败我方大臣数量")
    private int defeat_hero_num;

    public static final int FIELD_deduct_influence =3;
    @DataBaseField(type = "int(11)", fieldname = "deduct_influence", comment = "扣除影响力")
    private int deduct_influence;

    public static final int FIELD_timestamp =4;
    @DataBaseField(type = "bigint(20)", fieldname = "timestamp", comment = "时间戳")
    private long timestamp;

    public static final int FIELD_from_celebrity_rand =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "from_celebrity_rand", comment = "是否来自名人榜")
    private boolean from_celebrity_rand;

    public PlayerArenaFightReportBO() {
        id = 0;
        cid = 0L;
        opponent_cid = 0L;
        defeat_hero_num = 0;
        deduct_influence = 0;
        timestamp = 0L;
        from_celebrity_rand = false;
    }

    public PlayerArenaFightReportBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        opponent_cid = rs.getLong(3);
        defeat_hero_num = rs.getInt(4);
        deduct_influence = rs.getInt(5);
        timestamp = rs.getLong(6);
        from_celebrity_rand = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerArenaFightReportBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `opponent_cid`, `defeat_hero_num`, `deduct_influence`, `timestamp`, `from_celebrity_rand`";
    }

    @Override
    public String getTableName() {
        return "`player_arena_fight_report`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(opponent_cid).append("', ");
        strBuf.append("'").append(defeat_hero_num).append("', ");
        strBuf.append("'").append(deduct_influence).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(from_celebrity_rand ? 1 : 0).append("', ");
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

    // 对手cid
    public long getOpponentCid() { return this.opponent_cid; }
    public void setOpponentCid(BM _bm, long opponent_cid) {
        if(opponent_cid==this.opponent_cid) 
            return;
        this.opponent_cid = opponent_cid; 
        markField(_bm, FIELD_opponent_cid); 
    }
    public void saveOpponentCid(BM _bm, long opponent_cid) {
        if(opponent_cid==this.opponent_cid) 
            return;
        this.opponent_cid = opponent_cid;
        saveField(_bm, "opponent_cid", opponent_cid);
    }

    // 击败我方大臣数量
    public int getDefeatHeroNum() { return this.defeat_hero_num; }
    public void setDefeatHeroNum(BM _bm, int defeat_hero_num) {
        if(defeat_hero_num==this.defeat_hero_num) 
            return;
        this.defeat_hero_num = defeat_hero_num; 
        markField(_bm, FIELD_defeat_hero_num); 
    }
    public void saveDefeatHeroNum(BM _bm, int defeat_hero_num) {
        if(defeat_hero_num==this.defeat_hero_num) 
            return;
        this.defeat_hero_num = defeat_hero_num;
        saveField(_bm, "defeat_hero_num", defeat_hero_num);
    }

    // 扣除影响力
    public int getDeductInfluence() { return this.deduct_influence; }
    public void setDeductInfluence(BM _bm, int deduct_influence) {
        if(deduct_influence==this.deduct_influence) 
            return;
        this.deduct_influence = deduct_influence; 
        markField(_bm, FIELD_deduct_influence); 
    }
    public void saveDeductInfluence(BM _bm, int deduct_influence) {
        if(deduct_influence==this.deduct_influence) 
            return;
        this.deduct_influence = deduct_influence;
        saveField(_bm, "deduct_influence", deduct_influence);
    }

    // 时间戳
    public long getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }

    // 是否来自名人榜
    public boolean getFromCelebrityRand() { return this.from_celebrity_rand; }
    public void setFromCelebrityRand(BM _bm, boolean from_celebrity_rand) {
        if(from_celebrity_rand==this.from_celebrity_rand) 
            return;
        this.from_celebrity_rand = from_celebrity_rand; 
        markField(_bm, FIELD_from_celebrity_rand); 
    }
    public void saveFromCelebrityRand(BM _bm, boolean from_celebrity_rand) {
        if(from_celebrity_rand==this.from_celebrity_rand) 
            return;
        this.from_celebrity_rand = from_celebrity_rand;
        saveField(_bm, "from_celebrity_rand", from_celebrity_rand ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `opponent_cid` = '").append(opponent_cid).append("',");
        sBuilder.append(" `defeat_hero_num` = '").append(defeat_hero_num).append("',");
        sBuilder.append(" `deduct_influence` = '").append(deduct_influence).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `from_celebrity_rand` = '").append(from_celebrity_rand ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_opponent_cid)) sBuilder.append(" `opponent_cid` = '").append(opponent_cid).append("',");
        if(isFieldMarked(FIELD_defeat_hero_num)) sBuilder.append(" `defeat_hero_num` = '").append(defeat_hero_num).append("',");
        if(isFieldMarked(FIELD_deduct_influence)) sBuilder.append(" `deduct_influence` = '").append(deduct_influence).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_from_celebrity_rand)) sBuilder.append(" `from_celebrity_rand` = '").append(from_celebrity_rand ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_arena_fight_report` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`opponent_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '对手cid',"
                + "`defeat_hero_num` int(11) NOT NULL DEFAULT '0' COMMENT '击败我方大臣数量',"
                + "`deduct_influence` int(11) NOT NULL DEFAULT '0' COMMENT '扣除影响力',"
                + "`timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`from_celebrity_rand` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否来自名人榜',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家竞技场战报数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//opponent_cid
        _size+=4;//defeat_hero_num
        _size+=4;//deduct_influence
        _size+=8;//timestamp
        _size+=1;//from_celebrity_rand
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(opponent_cid);
        buff.putInt(defeat_hero_num);
        buff.putInt(deduct_influence);
        buff.putLong(timestamp);
        buff.put((byte)(from_celebrity_rand?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        opponent_cid=buff.getLong();
        defeat_hero_num=buff.getInt();
        deduct_influence=buff.getInt();
        timestamp=buff.getLong();
        from_celebrity_rand=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
