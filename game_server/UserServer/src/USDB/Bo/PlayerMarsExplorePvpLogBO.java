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
public class PlayerMarsExplorePvpLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_logType =1;
    @DataBaseField(type = "int(11)", fieldname = "logType", comment = "日志类型")
    private int logType;

    public static final int FIELD_targetCid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "targetCid", comment = "目标玩家CID")
    private long targetCid;

    public static final int FIELD_createdAt =3;
    @DataBaseField(type = "bigint(20)", fieldname = "createdAt", comment = "创建时间（毫秒）")
    private long createdAt;

    public static final int FIELD_logData =4;
    @DataBaseField(type = "blob", fieldname = "logData", comment = "日志数据")
    private byte[] logData;

    public PlayerMarsExplorePvpLogBO() {
        id = 0;
        cid = 0L;
        logType = 0;
        targetCid = 0L;
        createdAt = 0L;
        logData = null;
    }

    public PlayerMarsExplorePvpLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        logType = rs.getInt(3);
        targetCid = rs.getLong(4);
        createdAt = rs.getLong(5);
        logData = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsExplorePvpLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `logType`, `targetCid`, `createdAt`, `logData`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_explore_pvp_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(logType).append("', ");
        strBuf.append("'").append(targetCid).append("', ");
        strBuf.append("'").append(createdAt).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(logData);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_logData)) ret.add(logData);         return ret;
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

    // 日志类型
    public int getLogType() { return this.logType; }
    public void setLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType; 
        markField(_bm, FIELD_logType); 
    }
    public void saveLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType;
        saveField(_bm, "logType", logType);
    }

    // 目标玩家CID
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

    // 创建时间（毫秒）
    public long getCreatedAt() { return this.createdAt; }
    public void setCreatedAt(BM _bm, long createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt; 
        markField(_bm, FIELD_createdAt); 
    }
    public void saveCreatedAt(BM _bm, long createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt;
        saveField(_bm, "createdAt", createdAt);
    }

    // 日志数据
    public byte[] getLogData() { return this.logData; }
    public void setLogData(BM _bm, byte[] logData) {
        if(logData==this.logData) 
            return;
        this.logData = logData; 
        markField(_bm, FIELD_logData); 
    }
    public void saveLogData(BM _bm, byte[] logData) {
        if(logData==this.logData) 
            return;
        this.logData = logData;
        saveFieldBytes(_bm, "logData", logData);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `logType` = '").append(logType).append("',");
        sBuilder.append(" `targetCid` = '").append(targetCid).append("',");
        sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.append(" `logData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_logType)) sBuilder.append(" `logType` = '").append(logType).append("',");
        if(isFieldMarked(FIELD_targetCid)) sBuilder.append(" `targetCid` = '").append(targetCid).append("',");
        if(isFieldMarked(FIELD_createdAt)) sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        if(isFieldMarked(FIELD_logData)) sBuilder.append(" `logData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_explore_pvp_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型',"
                + "`targetCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标玩家CID',"
                + "`createdAt` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间（毫秒）',"
                + "`logData` blob NULL COMMENT '日志数据',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家火星探险PVP日志数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//logType
        _size+=8;//targetCid
        _size+=8;//createdAt
        _size+=2;_size+=logData.length;//logData
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(logType);
        buff.putLong(targetCid);
        buff.putLong(createdAt);
        buff.putShort((short)(logData == null ? 0 : logData.length));if(null != logData){buff.put(logData);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        logType=buff.getInt();
        targetCid=buff.getLong();
        createdAt=buff.getLong();
        int logData_count = buff.getShort();if(logData_count>0){logData = new byte[logData_count];buff.get(logData);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
