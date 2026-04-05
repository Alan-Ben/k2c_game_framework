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
public class PlayerMarsLetterBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_refId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "配置ID")
    private long refId;

    public static final int FIELD_npcId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "npcId", comment = "NPC ID")
    private long npcId;

    public static final int FIELD_isDealed =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isDealed", comment = "已处理")
    private boolean isDealed;

    public PlayerMarsLetterBO() {
        id = 0;
        cid = 0L;
        refId = 0L;
        npcId = 0L;
        isDealed = false;
    }

    public PlayerMarsLetterBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        refId = rs.getLong(3);
        npcId = rs.getLong(4);
        isDealed = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsLetterBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `refId`, `npcId`, `isDealed`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_letter`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(npcId).append("', ");
        strBuf.append("'").append(isDealed ? 1 : 0).append("', ");
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

    // NPC ID
    public long getNpcId() { return this.npcId; }
    public void setNpcId(BM _bm, long npcId) {
        if(npcId==this.npcId) 
            return;
        this.npcId = npcId; 
        markField(_bm, FIELD_npcId); 
    }
    public void saveNpcId(BM _bm, long npcId) {
        if(npcId==this.npcId) 
            return;
        this.npcId = npcId;
        saveField(_bm, "npcId", npcId);
    }

    // 已处理
    public boolean getIsDealed() { return this.isDealed; }
    public void setIsDealed(BM _bm, boolean isDealed) {
        if(isDealed==this.isDealed) 
            return;
        this.isDealed = isDealed; 
        markField(_bm, FIELD_isDealed); 
    }
    public void saveIsDealed(BM _bm, boolean isDealed) {
        if(isDealed==this.isDealed) 
            return;
        this.isDealed = isDealed;
        saveField(_bm, "isDealed", isDealed ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `npcId` = '").append(npcId).append("',");
        sBuilder.append(" `isDealed` = '").append(isDealed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_npcId)) sBuilder.append(" `npcId` = '").append(npcId).append("',");
        if(isFieldMarked(FIELD_isDealed)) sBuilder.append(" `isDealed` = '").append(isDealed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_letter` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`npcId` bigint(20) NOT NULL DEFAULT '0' COMMENT 'NPC ID',"
                + "`isDealed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '已处理',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-火星居民信件数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//npcId
        _size+=1;//isDealed
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
        buff.putLong(npcId);
        buff.put((byte)(isDealed?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        refId=buff.getLong();
        npcId=buff.getLong();
        isDealed=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
