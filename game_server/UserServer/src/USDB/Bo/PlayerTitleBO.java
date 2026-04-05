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
public class PlayerTitleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_titleId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "titleId", comment = "称号ID")
    private long titleId;

    public static final int FIELD_expireTimeS =2;
    @DataBaseField(type = "int(11)", fieldname = "expireTimeS", comment = "超时时间戳，0一下表示永久")
    private int expireTimeS;

    public static final int FIELD_viewed =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "viewed", comment = "是否已查看")
    private boolean viewed;

    public static final int FIELD_lastGainTs =4;
    @DataBaseField(type = "int(11)", fieldname = "lastGainTs", comment = "上次获取时间")
    private int lastGainTs;

    public static final int FIELD_gainCount =5;
    @DataBaseField(type = "int(11)", fieldname = "gainCount", comment = "获取次数")
    private int gainCount;

    public PlayerTitleBO() {
        id = 0;
        cid = 0L;
        titleId = 0L;
        expireTimeS = 0;
        viewed = false;
        lastGainTs = 0;
        gainCount = 0;
    }

    public PlayerTitleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        titleId = rs.getLong(3);
        expireTimeS = rs.getInt(4);
        viewed = rs.getBoolean(5);
        lastGainTs = rs.getInt(6);
        gainCount = rs.getInt(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTitleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `titleId`, `expireTimeS`, `viewed`, `lastGainTs`, `gainCount`";
    }

    @Override
    public String getTableName() {
        return "`player_title`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(titleId).append("', ");
        strBuf.append("'").append(expireTimeS).append("', ");
        strBuf.append("'").append(viewed ? 1 : 0).append("', ");
        strBuf.append("'").append(lastGainTs).append("', ");
        strBuf.append("'").append(gainCount).append("', ");
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

    // 称号ID
    public long getTitleId() { return this.titleId; }
    public void setTitleId(BM _bm, long titleId) {
        if(titleId==this.titleId) 
            return;
        this.titleId = titleId; 
        markField(_bm, FIELD_titleId); 
    }
    public void saveTitleId(BM _bm, long titleId) {
        if(titleId==this.titleId) 
            return;
        this.titleId = titleId;
        saveField(_bm, "titleId", titleId);
    }

    // 超时时间戳，0一下表示永久
    public int getExpireTimeS() { return this.expireTimeS; }
    public void setExpireTimeS(BM _bm, int expireTimeS) {
        if(expireTimeS==this.expireTimeS) 
            return;
        this.expireTimeS = expireTimeS; 
        markField(_bm, FIELD_expireTimeS); 
    }
    public void saveExpireTimeS(BM _bm, int expireTimeS) {
        if(expireTimeS==this.expireTimeS) 
            return;
        this.expireTimeS = expireTimeS;
        saveField(_bm, "expireTimeS", expireTimeS);
    }

    // 是否已查看
    public boolean getViewed() { return this.viewed; }
    public void setViewed(BM _bm, boolean viewed) {
        if(viewed==this.viewed) 
            return;
        this.viewed = viewed; 
        markField(_bm, FIELD_viewed); 
    }
    public void saveViewed(BM _bm, boolean viewed) {
        if(viewed==this.viewed) 
            return;
        this.viewed = viewed;
        saveField(_bm, "viewed", viewed ? 1 : 0);
    }

    // 上次获取时间
    public int getLastGainTs() { return this.lastGainTs; }
    public void setLastGainTs(BM _bm, int lastGainTs) {
        if(lastGainTs==this.lastGainTs) 
            return;
        this.lastGainTs = lastGainTs; 
        markField(_bm, FIELD_lastGainTs); 
    }
    public void saveLastGainTs(BM _bm, int lastGainTs) {
        if(lastGainTs==this.lastGainTs) 
            return;
        this.lastGainTs = lastGainTs;
        saveField(_bm, "lastGainTs", lastGainTs);
    }

    // 获取次数
    public int getGainCount() { return this.gainCount; }
    public void setGainCount(BM _bm, int gainCount) {
        if(gainCount==this.gainCount) 
            return;
        this.gainCount = gainCount; 
        markField(_bm, FIELD_gainCount); 
    }
    public void saveGainCount(BM _bm, int gainCount) {
        if(gainCount==this.gainCount) 
            return;
        this.gainCount = gainCount;
        saveField(_bm, "gainCount", gainCount);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `titleId` = '").append(titleId).append("',");
        sBuilder.append(" `expireTimeS` = '").append(expireTimeS).append("',");
        sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        sBuilder.append(" `lastGainTs` = '").append(lastGainTs).append("',");
        sBuilder.append(" `gainCount` = '").append(gainCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_titleId)) sBuilder.append(" `titleId` = '").append(titleId).append("',");
        if(isFieldMarked(FIELD_expireTimeS)) sBuilder.append(" `expireTimeS` = '").append(expireTimeS).append("',");
        if(isFieldMarked(FIELD_viewed)) sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_lastGainTs)) sBuilder.append(" `lastGainTs` = '").append(lastGainTs).append("',");
        if(isFieldMarked(FIELD_gainCount)) sBuilder.append(" `gainCount` = '").append(gainCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_title` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`titleId` bigint(20) NOT NULL DEFAULT '0' COMMENT '称号ID',"
                + "`expireTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '超时时间戳，0一下表示永久',"
                + "`viewed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已查看',"
                + "`lastGainTs` int(11) NOT NULL DEFAULT '0' COMMENT '上次获取时间',"
                + "`gainCount` int(11) NOT NULL DEFAULT '0' COMMENT '获取次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Info 玩家普通称号数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//titleId
        _size+=4;//expireTimeS
        _size+=1;//viewed
        _size+=4;//lastGainTs
        _size+=4;//gainCount
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(titleId);
        buff.putInt(expireTimeS);
        buff.put((byte)(viewed?1:0));
        buff.putInt(lastGainTs);
        buff.putInt(gainCount);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        titleId=buff.getLong();
        expireTimeS=buff.getInt();
        viewed=(buff.get()==1);
        lastGainTs=buff.getInt();
        gainCount=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
