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
public class PlayerMarsPeopleIntelligentBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_refId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "配置ID")
    private long refId;

    public static final int FIELD_endMs =2;
    @DataBaseField(type = "bigint(20)", fieldname = "endMs", comment = "CD结束时间（毫秒）")
    private long endMs;

    public PlayerMarsPeopleIntelligentBO() {
        id = 0;
        cid = 0L;
        refId = 0L;
        endMs = 0L;
    }

    public PlayerMarsPeopleIntelligentBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        refId = rs.getLong(3);
        endMs = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsPeopleIntelligentBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `refId`, `endMs`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_people_intelligent`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(endMs).append("', ");
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

    // 配置ID
    public long getRefId() { return this.refId; }
    public void setRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId; 
        markField(_bm, FIELD_refId); 
    }
    public void saveRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId;
        saveField(_bm, "refId", refId);
    }

    // CD结束时间（毫秒）
    public long getEndMs() { return this.endMs; }
    public void setEndMs(BM _bm, long endMs) {
        if(endMs==this.endMs) 
            return;
        this.endMs = endMs; 
        markField(_bm, FIELD_endMs); 
    }
    public void saveEndMs(BM _bm, long endMs) {
        if(endMs==this.endMs) 
            return;
        this.endMs = endMs;
        saveField(_bm, "endMs", endMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `endMs` = '").append(endMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_endMs)) sBuilder.append(" `endMs` = '").append(endMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_people_intelligent` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`endMs` bigint(20) NOT NULL DEFAULT '0' COMMENT 'CD结束时间（毫秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-火星居民决策数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//refId
        _size+=8;//endMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(refId);
        buff.putLong(endMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        refId=buff.getLong();
        endMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
