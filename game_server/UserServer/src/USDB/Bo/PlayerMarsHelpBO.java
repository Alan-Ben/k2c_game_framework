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
public class PlayerMarsHelpBO extends BaseBO {
    
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

    public static final int FIELD_chooseIdx =3;
    @DataBaseField(type = "int(11)", fieldname = "chooseIdx", comment = "选项下标")
    private int chooseIdx;

    public static final int FIELD_isDealed =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "isDealed", comment = "已处理")
    private boolean isDealed;

    public static final int FIELD_createdAt =5;
    @DataBaseField(type = "int(11)", fieldname = "createdAt", comment = "创建时间（秒）")
    private int createdAt;

    public PlayerMarsHelpBO() {
        id = 0;
        cid = 0L;
        refId = 0L;
        npcId = 0L;
        chooseIdx = 0;
        isDealed = false;
        createdAt = 0;
    }

    public PlayerMarsHelpBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        refId = rs.getLong(3);
        npcId = rs.getLong(4);
        chooseIdx = rs.getInt(5);
        isDealed = rs.getBoolean(6);
        createdAt = rs.getInt(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsHelpBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `refId`, `npcId`, `chooseIdx`, `isDealed`, `createdAt`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_help`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(npcId).append("', ");
        strBuf.append("'").append(chooseIdx).append("', ");
        strBuf.append("'").append(isDealed ? 1 : 0).append("', ");
        strBuf.append("'").append(createdAt).append("', ");
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

    // 选项下标
    public int getChooseIdx() { return this.chooseIdx; }
    public void setChooseIdx(BM _bm, int chooseIdx) {
        if(chooseIdx==this.chooseIdx) 
            return;
        this.chooseIdx = chooseIdx; 
        markField(_bm, FIELD_chooseIdx); 
    }
    public void saveChooseIdx(BM _bm, int chooseIdx) {
        if(chooseIdx==this.chooseIdx) 
            return;
        this.chooseIdx = chooseIdx;
        saveField(_bm, "chooseIdx", chooseIdx);
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

    // 创建时间（秒）
    public int getCreatedAt() { return this.createdAt; }
    public void setCreatedAt(BM _bm, int createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt; 
        markField(_bm, FIELD_createdAt); 
    }
    public void saveCreatedAt(BM _bm, int createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt;
        saveField(_bm, "createdAt", createdAt);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `npcId` = '").append(npcId).append("',");
        sBuilder.append(" `chooseIdx` = '").append(chooseIdx).append("',");
        sBuilder.append(" `isDealed` = '").append(isDealed ? 1 : 0).append("',");
        sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_npcId)) sBuilder.append(" `npcId` = '").append(npcId).append("',");
        if(isFieldMarked(FIELD_chooseIdx)) sBuilder.append(" `chooseIdx` = '").append(chooseIdx).append("',");
        if(isFieldMarked(FIELD_isDealed)) sBuilder.append(" `isDealed` = '").append(isDealed ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_createdAt)) sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_help` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`npcId` bigint(20) NOT NULL DEFAULT '0' COMMENT 'NPC ID',"
                + "`chooseIdx` int(11) NOT NULL DEFAULT '0' COMMENT '选项下标',"
                + "`isDealed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '已处理',"
                + "`createdAt` int(11) NOT NULL DEFAULT '0' COMMENT '创建时间（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-火星居民帮助数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//chooseIdx
        _size+=1;//isDealed
        _size+=4;//createdAt
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
        buff.putInt(chooseIdx);
        buff.put((byte)(isDealed?1:0));
        buff.putInt(createdAt);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        refId=buff.getLong();
        npcId=buff.getLong();
        chooseIdx=buff.getInt();
        isDealed=(buff.get()==1);
        createdAt=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
