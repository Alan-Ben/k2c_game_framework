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
public class PlayerMarsMineBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_instanceId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "矿产实例ID")
    private long instanceId;

    public static final int FIELD_refId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "配置ID")
    private long refId;

    public static final int FIELD_startShowMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "startShowMs", comment = "开始展示时间（毫秒）")
    private long startShowMs;

    public static final int FIELD_endShowMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "endShowMs", comment = "结束展示时间（毫秒）")
    private long endShowMs;

    public static final int FIELD_pos =5;
    @DataBaseField(type = "bigint(20)", fieldname = "pos", comment = "矿产位置ID")
    private long pos;

    public PlayerMarsMineBO() {
        id = 0;
        cid = 0L;
        instanceId = 0L;
        refId = 0L;
        startShowMs = 0L;
        endShowMs = 0L;
        pos = 0L;
    }

    public PlayerMarsMineBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        instanceId = rs.getLong(3);
        refId = rs.getLong(4);
        startShowMs = rs.getLong(5);
        endShowMs = rs.getLong(6);
        pos = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsMineBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `instanceId`, `refId`, `startShowMs`, `endShowMs`, `pos`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_mine`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(startShowMs).append("', ");
        strBuf.append("'").append(endShowMs).append("', ");
        strBuf.append("'").append(pos).append("', ");
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

    // 矿产实例ID
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

    // 开始展示时间（毫秒）
    public long getStartShowMs() { return this.startShowMs; }
    public void setStartShowMs(BM _bm, long startShowMs) {
        if(startShowMs==this.startShowMs) 
            return;
        this.startShowMs = startShowMs; 
        markField(_bm, FIELD_startShowMs); 
    }
    public void saveStartShowMs(BM _bm, long startShowMs) {
        if(startShowMs==this.startShowMs) 
            return;
        this.startShowMs = startShowMs;
        saveField(_bm, "startShowMs", startShowMs);
    }

    // 结束展示时间（毫秒）
    public long getEndShowMs() { return this.endShowMs; }
    public void setEndShowMs(BM _bm, long endShowMs) {
        if(endShowMs==this.endShowMs) 
            return;
        this.endShowMs = endShowMs; 
        markField(_bm, FIELD_endShowMs); 
    }
    public void saveEndShowMs(BM _bm, long endShowMs) {
        if(endShowMs==this.endShowMs) 
            return;
        this.endShowMs = endShowMs;
        saveField(_bm, "endShowMs", endShowMs);
    }

    // 矿产位置ID
    public long getPos() { return this.pos; }
    public void setPos(BM _bm, long pos) {
        if(pos==this.pos) 
            return;
        this.pos = pos; 
        markField(_bm, FIELD_pos); 
    }
    public void savePos(BM _bm, long pos) {
        if(pos==this.pos) 
            return;
        this.pos = pos;
        saveField(_bm, "pos", pos);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `startShowMs` = '").append(startShowMs).append("',");
        sBuilder.append(" `endShowMs` = '").append(endShowMs).append("',");
        sBuilder.append(" `pos` = '").append(pos).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_startShowMs)) sBuilder.append(" `startShowMs` = '").append(startShowMs).append("',");
        if(isFieldMarked(FIELD_endShowMs)) sBuilder.append(" `endShowMs` = '").append(endShowMs).append("',");
        if(isFieldMarked(FIELD_pos)) sBuilder.append(" `pos` = '").append(pos).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_mine` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿产实例ID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`startShowMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始展示时间（毫秒）',"
                + "`endShowMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束展示时间（毫秒）',"
                + "`pos` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿产位置ID',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-玩家火星矿产数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instanceId
        _size+=8;//refId
        _size+=8;//startShowMs
        _size+=8;//endShowMs
        _size+=8;//pos
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(instanceId);
        buff.putLong(refId);
        buff.putLong(startShowMs);
        buff.putLong(endShowMs);
        buff.putLong(pos);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        instanceId=buff.getLong();
        refId=buff.getLong();
        startShowMs=buff.getLong();
        endShowMs=buff.getLong();
        pos=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
