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
public class NpPartyLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "聚会实例ID")
    private long instanceId;

    public static final int FIELD_logType =1;
    @DataBaseField(type = "int(11)", fieldname = "logType", comment = "日志类型")
    private int logType;

    public static final int FIELD_paramList =2;
    @DataBaseField(type = "blob(1024)", fieldname = "paramList", comment = "参数列表")
    private byte[] paramList;

    public static final int FIELD_logTs =3;
    @DataBaseField(type = "int(11)", fieldname = "logTs", comment = "日志时间戳")
    private int logTs;

    public NpPartyLogBO() {
        id = 0;
        instanceId = 0L;
        logType = 0;
        paramList = null;
        logTs = 0;
    }

    public NpPartyLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        logType = rs.getInt(3);
        paramList = rs.getBytes(4);
        logTs = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpPartyLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `logType`, `paramList`, `logTs`";
    }

    @Override
    public String getTableName() {
        return "`np_party_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(logType).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(logTs).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(paramList);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_paramList)) ret.add(paramList);         return ret;
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

    // 参数列表
    public byte[] getParamList() { return this.paramList; }
    public void setParamList(BM _bm, byte[] paramList) {
        if(paramList==this.paramList) 
            return;
        this.paramList = paramList; 
        markField(_bm, FIELD_paramList); 
    }
    public void saveParamList(BM _bm, byte[] paramList) {
        if(paramList==this.paramList) 
            return;
        this.paramList = paramList;
        saveFieldBytes(_bm, "paramList", paramList);
    }

    // 日志时间戳
    public int getLogTs() { return this.logTs; }
    public void setLogTs(BM _bm, int logTs) {
        if(logTs==this.logTs) 
            return;
        this.logTs = logTs; 
        markField(_bm, FIELD_logTs); 
    }
    public void saveLogTs(BM _bm, int logTs) {
        if(logTs==this.logTs) 
            return;
        this.logTs = logTs;
        saveField(_bm, "logTs", logTs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `logType` = '").append(logType).append("',");
        sBuilder.append(" `paramList` = ?,");
        sBuilder.append(" `logTs` = '").append(logTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_logType)) sBuilder.append(" `logType` = '").append(logType).append("',");
        if(isFieldMarked(FIELD_paramList)) sBuilder.append(" `paramList` = ?,");
        if(isFieldMarked(FIELD_logTs)) sBuilder.append(" `logTs` = '").append(logTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_party_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '聚会实例ID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型',"
                + "`paramList` blob NULL COMMENT '参数列表',"
                + "`logTs` int(11) NOT NULL DEFAULT '0' COMMENT '日志时间戳',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Party 聚会日志数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//logType
        _size+=2;_size+=paramList.length;//paramList
        _size+=4;//logTs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putInt(logType);
        buff.putShort((short)(paramList == null ? 0 : paramList.length));if(null != paramList){buff.put(paramList);}
        buff.putInt(logTs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        logType=buff.getInt();
        int paramList_count = buff.getShort();if(paramList_count>0){paramList = new byte[paramList_count];buff.get(paramList);}
        logTs=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
