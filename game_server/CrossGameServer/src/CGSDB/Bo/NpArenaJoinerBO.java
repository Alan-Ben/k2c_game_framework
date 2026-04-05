package CGSDB.Bo;
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
public class NpArenaJoinerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "聚会实例ID")
    private long instanceId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_rank =2;
    @DataBaseField(type = "int(11)", fieldname = "rank", comment = "排名")
    private int rank;

    public static final int FIELD_fight_info =3;
    @DataBaseField(type = "blob", fieldname = "fight_info", comment = "玩家战斗数据")
    private byte[] fight_info;

    public static final int FIELD_last_settle_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "last_settle_time_ms", comment = "最后一次结算奖励的时间戳")
    private long last_settle_time_ms;

    public NpArenaJoinerBO() {
        id = 0;
        instanceId = 0L;
        cid = 0L;
        rank = 0;
        fight_info = null;
        last_settle_time_ms = 0L;
    }

    public NpArenaJoinerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        cid = rs.getLong(3);
        rank = rs.getInt(4);
        fight_info = rs.getBytes(5);
        last_settle_time_ms = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpArenaJoinerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `cid`, `rank`, `fight_info`, `last_settle_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`np_arena_joiner`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(rank).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(last_settle_time_ms).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(fight_info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_fight_info)) ret.add(fight_info);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 聚会实例ID
    public long getInstanceId() { return this.instanceId; }
    public void setInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId; 
        markField(_bm, FIELD_instanceId); 
    }
    public void saveInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId;
        saveField(_bm, "instanceId", instanceId);
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

    // 排名
    public int getRank() { return this.rank; }
    public void setRank(BM _bm, int rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank; 
        markField(_bm, FIELD_rank); 
    }
    public void saveRank(BM _bm, int rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank;
        saveField(_bm, "rank", rank);
    }

    // 玩家战斗数据
    public byte[] getFightInfo() { return this.fight_info; }
    public void setFightInfo(BM _bm, byte[] fight_info) {
        if(fight_info==this.fight_info) 
            return;
        this.fight_info = fight_info; 
        markField(_bm, FIELD_fight_info); 
    }
    public void saveFightInfo(BM _bm, byte[] fight_info) {
        if(fight_info==this.fight_info) 
            return;
        this.fight_info = fight_info;
        saveFieldBytes(_bm, "fight_info", fight_info);
    }

    // 最后一次结算奖励的时间戳
    public long getLastSettleTimeMs() { return this.last_settle_time_ms; }
    public void setLastSettleTimeMs(BM _bm, long last_settle_time_ms) {
        if(last_settle_time_ms==this.last_settle_time_ms) 
            return;
        this.last_settle_time_ms = last_settle_time_ms; 
        markField(_bm, FIELD_last_settle_time_ms); 
    }
    public void saveLastSettleTimeMs(BM _bm, long last_settle_time_ms) {
        if(last_settle_time_ms==this.last_settle_time_ms) 
            return;
        this.last_settle_time_ms = last_settle_time_ms;
        saveField(_bm, "last_settle_time_ms", last_settle_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `rank` = '").append(rank).append("',");
        sBuilder.append(" `fight_info` = ?,");
        sBuilder.append(" `last_settle_time_ms` = '").append(last_settle_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_rank)) sBuilder.append(" `rank` = '").append(rank).append("',");
        if(isFieldMarked(FIELD_fight_info)) sBuilder.append(" `fight_info` = ?,");
        if(isFieldMarked(FIELD_last_settle_time_ms)) sBuilder.append(" `last_settle_time_ms` = '").append(last_settle_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_arena_joiner` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '聚会实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`rank` int(11) NOT NULL DEFAULT '0' COMMENT '排名',"
                + "`fight_info` blob NULL COMMENT '玩家战斗数据',"
                + "`last_settle_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一次结算奖励的时间戳',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Arena 比武擂台上榜玩家' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossgame_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//instanceId
        _size+=8;//cid
        _size+=4;//rank
        _size+=2;_size+=fight_info.length;//fight_info
        _size+=8;//last_settle_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putLong(cid);
        buff.putInt(rank);
        buff.putShort((short)(fight_info == null ? 0 : fight_info.length));if(null != fight_info){buff.put(fight_info);}
        buff.putLong(last_settle_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        cid=buff.getLong();
        rank=buff.getInt();
        int fight_info_count = buff.getShort();if(fight_info_count>0){fight_info = new byte[fight_info_count];buff.get(fight_info);}
        last_settle_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
