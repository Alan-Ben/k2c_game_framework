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
public class PlayerBuffBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_buffId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "buffId", comment = "buff id")
    private long buffId;

    public static final int FIELD_layer =2;
    @DataBaseField(type = "int(11)", fieldname = "layer", comment = "层数")
    private int layer;

    public static final int FIELD_endTimeMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "endTimeMs", comment = "结束时间")
    private long endTimeMs;

    public static final int FIELD_startTimeMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "startTimeMs", comment = "开始时间")
    private long startTimeMs;

    public PlayerBuffBO() {
        id = 0;
        cid = 0L;
        buffId = 0L;
        layer = 0;
        endTimeMs = 0L;
        startTimeMs = 0L;
    }

    public PlayerBuffBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        buffId = rs.getLong(3);
        layer = rs.getInt(4);
        endTimeMs = rs.getLong(5);
        startTimeMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerBuffBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `buffId`, `layer`, `endTimeMs`, `startTimeMs`";
    }

    @Override
    public String getTableName() {
        return "`player_buff`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(buffId).append("', ");
        strBuf.append("'").append(layer).append("', ");
        strBuf.append("'").append(endTimeMs).append("', ");
        strBuf.append("'").append(startTimeMs).append("', ");
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

    // 玩家账号ID
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

    // buff id
    public long getBuffId() { return this.buffId; }
    public void setBuffId(BM _bm, long buffId) {
        if(buffId==this.buffId) 
            return;
        this.buffId = buffId; 
        markField(_bm, FIELD_buffId); 
    }
    public void saveBuffId(BM _bm, long buffId) {
        if(buffId==this.buffId) 
            return;
        this.buffId = buffId;
        saveField(_bm, "buffId", buffId);
    }

    // 层数
    public int getLayer() { return this.layer; }
    public void setLayer(BM _bm, int layer) {
        if(layer==this.layer) 
            return;
        this.layer = layer; 
        markField(_bm, FIELD_layer); 
    }
    public void saveLayer(BM _bm, int layer) {
        if(layer==this.layer) 
            return;
        this.layer = layer;
        saveField(_bm, "layer", layer);
    }

    // 结束时间
    public long getEndTimeMs() { return this.endTimeMs; }
    public void setEndTimeMs(BM _bm, long endTimeMs) {
        if(endTimeMs==this.endTimeMs) 
            return;
        this.endTimeMs = endTimeMs; 
        markField(_bm, FIELD_endTimeMs); 
    }
    public void saveEndTimeMs(BM _bm, long endTimeMs) {
        if(endTimeMs==this.endTimeMs) 
            return;
        this.endTimeMs = endTimeMs;
        saveField(_bm, "endTimeMs", endTimeMs);
    }

    // 开始时间
    public long getStartTimeMs() { return this.startTimeMs; }
    public void setStartTimeMs(BM _bm, long startTimeMs) {
        if(startTimeMs==this.startTimeMs) 
            return;
        this.startTimeMs = startTimeMs; 
        markField(_bm, FIELD_startTimeMs); 
    }
    public void saveStartTimeMs(BM _bm, long startTimeMs) {
        if(startTimeMs==this.startTimeMs) 
            return;
        this.startTimeMs = startTimeMs;
        saveField(_bm, "startTimeMs", startTimeMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `buffId` = '").append(buffId).append("',");
        sBuilder.append(" `layer` = '").append(layer).append("',");
        sBuilder.append(" `endTimeMs` = '").append(endTimeMs).append("',");
        sBuilder.append(" `startTimeMs` = '").append(startTimeMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_buffId)) sBuilder.append(" `buffId` = '").append(buffId).append("',");
        if(isFieldMarked(FIELD_layer)) sBuilder.append(" `layer` = '").append(layer).append("',");
        if(isFieldMarked(FIELD_endTimeMs)) sBuilder.append(" `endTimeMs` = '").append(endTimeMs).append("',");
        if(isFieldMarked(FIELD_startTimeMs)) sBuilder.append(" `startTimeMs` = '").append(startTimeMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_buff` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`buffId` bigint(20) NOT NULL DEFAULT '0' COMMENT 'buff id',"
                + "`layer` int(11) NOT NULL DEFAULT '0' COMMENT '层数',"
                + "`endTimeMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束时间',"
                + "`startTimeMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Buff 玩家Buff数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//buffId
        _size+=4;//layer
        _size+=8;//endTimeMs
        _size+=8;//startTimeMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(buffId);
        buff.putInt(layer);
        buff.putLong(endTimeMs);
        buff.putLong(startTimeMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        buffId=buff.getLong();
        layer=buff.getInt();
        endTimeMs=buff.getLong();
        startTimeMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
