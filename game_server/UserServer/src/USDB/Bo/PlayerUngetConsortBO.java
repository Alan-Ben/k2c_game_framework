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
public class PlayerUngetConsortBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_consortId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "情人ID")
    private long consortId;

    public static final int FIELD_consortLike =2;
    @DataBaseField(type = "int(11)", fieldname = "consortLike", comment = "情人好感度")
    private int consortLike;

    public static final int FIELD_travelCount =3;
    @DataBaseField(type = "int(11)", fieldname = "travelCount", comment = "情人游历次数")
    private int travelCount;

    public PlayerUngetConsortBO() {
        id = 0;
        cid = 0L;
        consortId = 0L;
        consortLike = 0;
        travelCount = 0;
    }

    public PlayerUngetConsortBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        consortId = rs.getLong(3);
        consortLike = rs.getInt(4);
        travelCount = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerUngetConsortBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `consortId`, `consortLike`, `travelCount`";
    }

    @Override
    public String getTableName() {
        return "`player_unget_consort`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(consortLike).append("', ");
        strBuf.append("'").append(travelCount).append("', ");
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

    // 情人ID
    public long getConsortId() { return this.consortId; }
    public void setConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId; 
        markField(_bm, FIELD_consortId); 
    }
    public void saveConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId;
        saveField(_bm, "consortId", consortId);
    }

    // 情人好感度
    public int getConsortLike() { return this.consortLike; }
    public void setConsortLike(BM _bm, int consortLike) {
        if(consortLike==this.consortLike) 
            return;
        this.consortLike = consortLike; 
        markField(_bm, FIELD_consortLike); 
    }
    public void saveConsortLike(BM _bm, int consortLike) {
        if(consortLike==this.consortLike) 
            return;
        this.consortLike = consortLike;
        saveField(_bm, "consortLike", consortLike);
    }

    // 情人游历次数
    public int getTravelCount() { return this.travelCount; }
    public void setTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount; 
        markField(_bm, FIELD_travelCount); 
    }
    public void saveTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount;
        saveField(_bm, "travelCount", travelCount);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `consortLike` = '").append(consortLike).append("',");
        sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_consortLike)) sBuilder.append(" `consortLike` = '").append(consortLike).append("',");
        if(isFieldMarked(FIELD_travelCount)) sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_unget_consort` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '情人ID',"
                + "`consortLike` int(11) NOT NULL DEFAULT '0' COMMENT '情人好感度',"
                + "`travelCount` int(11) NOT NULL DEFAULT '0' COMMENT '情人游历次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家未获得情人数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//consortId
        _size+=4;//consortLike
        _size+=4;//travelCount
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(consortId);
        buff.putInt(consortLike);
        buff.putInt(travelCount);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        consortId=buff.getLong();
        consortLike=buff.getInt();
        travelCount=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
