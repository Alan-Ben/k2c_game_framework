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
public class PlayerDinnerEachLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_targetCid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "targetCid", comment = "对方玩家CID")
    private long targetCid;

    public static final int FIELD_joinedCount =2;
    @DataBaseField(type = "int(11)", fieldname = "joinedCount", comment = "参加对方宴会次数")
    private int joinedCount;

    public static final int FIELD_beJoinedCount =3;
    @DataBaseField(type = "int(11)", fieldname = "beJoinedCount", comment = "对方参加己方宴会次数")
    private int beJoinedCount;

    public static final int FIELD_lastTs =4;
    @DataBaseField(type = "int(11)", fieldname = "lastTs", comment = "最后一次更新时间")
    private int lastTs;

    public PlayerDinnerEachLogBO() {
        id = 0;
        cid = 0L;
        targetCid = 0L;
        joinedCount = 0;
        beJoinedCount = 0;
        lastTs = 0;
    }

    public PlayerDinnerEachLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        targetCid = rs.getLong(3);
        joinedCount = rs.getInt(4);
        beJoinedCount = rs.getInt(5);
        lastTs = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDinnerEachLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `targetCid`, `joinedCount`, `beJoinedCount`, `lastTs`";
    }

    @Override
    public String getTableName() {
        return "`player_dinner_each_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(targetCid).append("', ");
        strBuf.append("'").append(joinedCount).append("', ");
        strBuf.append("'").append(beJoinedCount).append("', ");
        strBuf.append("'").append(lastTs).append("', ");
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

    // 对方玩家CID
    public long getTargetCid() { return this.targetCid; }
    public void setTargetCid(BM _bm, long targetCid) {
        if(targetCid==this.targetCid) 
            return;
        this.targetCid = targetCid; 
        markField(_bm, FIELD_targetCid); 
    }
    public void saveTargetCid(BM _bm, long targetCid) {
        if(targetCid==this.targetCid) 
            return;
        this.targetCid = targetCid;
        saveField(_bm, "targetCid", targetCid);
    }

    // 参加对方宴会次数
    public int getJoinedCount() { return this.joinedCount; }
    public void setJoinedCount(BM _bm, int joinedCount) {
        if(joinedCount==this.joinedCount) 
            return;
        this.joinedCount = joinedCount; 
        markField(_bm, FIELD_joinedCount); 
    }
    public void saveJoinedCount(BM _bm, int joinedCount) {
        if(joinedCount==this.joinedCount) 
            return;
        this.joinedCount = joinedCount;
        saveField(_bm, "joinedCount", joinedCount);
    }

    // 对方参加己方宴会次数
    public int getBeJoinedCount() { return this.beJoinedCount; }
    public void setBeJoinedCount(BM _bm, int beJoinedCount) {
        if(beJoinedCount==this.beJoinedCount) 
            return;
        this.beJoinedCount = beJoinedCount; 
        markField(_bm, FIELD_beJoinedCount); 
    }
    public void saveBeJoinedCount(BM _bm, int beJoinedCount) {
        if(beJoinedCount==this.beJoinedCount) 
            return;
        this.beJoinedCount = beJoinedCount;
        saveField(_bm, "beJoinedCount", beJoinedCount);
    }

    // 最后一次更新时间
    public int getLastTs() { return this.lastTs; }
    public void setLastTs(BM _bm, int lastTs) {
        if(lastTs==this.lastTs) 
            return;
        this.lastTs = lastTs; 
        markField(_bm, FIELD_lastTs); 
    }
    public void saveLastTs(BM _bm, int lastTs) {
        if(lastTs==this.lastTs) 
            return;
        this.lastTs = lastTs;
        saveField(_bm, "lastTs", lastTs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `targetCid` = '").append(targetCid).append("',");
        sBuilder.append(" `joinedCount` = '").append(joinedCount).append("',");
        sBuilder.append(" `beJoinedCount` = '").append(beJoinedCount).append("',");
        sBuilder.append(" `lastTs` = '").append(lastTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_targetCid)) sBuilder.append(" `targetCid` = '").append(targetCid).append("',");
        if(isFieldMarked(FIELD_joinedCount)) sBuilder.append(" `joinedCount` = '").append(joinedCount).append("',");
        if(isFieldMarked(FIELD_beJoinedCount)) sBuilder.append(" `beJoinedCount` = '").append(beJoinedCount).append("',");
        if(isFieldMarked(FIELD_lastTs)) sBuilder.append(" `lastTs` = '").append(lastTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_dinner_each_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`targetCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '对方玩家CID',"
                + "`joinedCount` int(11) NOT NULL DEFAULT '0' COMMENT '参加对方宴会次数',"
                + "`beJoinedCount` int(11) NOT NULL DEFAULT '0' COMMENT '对方参加己方宴会次数',"
                + "`lastTs` int(11) NOT NULL DEFAULT '0' COMMENT '最后一次更新时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家宴会交互数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//targetCid
        _size+=4;//joinedCount
        _size+=4;//beJoinedCount
        _size+=4;//lastTs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(targetCid);
        buff.putInt(joinedCount);
        buff.putInt(beJoinedCount);
        buff.putInt(lastTs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        targetCid=buff.getLong();
        joinedCount=buff.getInt();
        beJoinedCount=buff.getInt();
        lastTs=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
