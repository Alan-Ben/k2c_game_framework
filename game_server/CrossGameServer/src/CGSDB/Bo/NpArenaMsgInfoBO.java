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
public class NpArenaMsgInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "实例id")
    private long instanceId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public static final int FIELD_type =2;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "公告类型")
    private int type;

    public static final int FIELD_timeMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "timeMs", comment = "发布时间戳")
    private long timeMs;

    public static final int FIELD_paramList =4;
    @DataBaseField(type = "varchar(128)", fieldname = "paramList", comment = "公告参数")
    private String paramList;

    public static final int FIELD_rankBefore =5;
    @DataBaseField(type = "int(11)", fieldname = "rankBefore", comment = "挑战前排位")
    private int rankBefore;

    public static final int FIELD_rankNow =6;
    @DataBaseField(type = "int(11)", fieldname = "rankNow", comment = "现在排位")
    private int rankNow;

    public NpArenaMsgInfoBO() {
        id = 0;
        instanceId = 0L;
        cid = 0L;
        type = 0;
        timeMs = 0L;
        paramList = "";
        rankBefore = 0;
        rankNow = 0;
    }

    public NpArenaMsgInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        cid = rs.getLong(3);
        type = rs.getInt(4);
        timeMs = rs.getLong(5);
        paramList = rs.getString(6);
        rankBefore = rs.getInt(7);
        rankNow = rs.getInt(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpArenaMsgInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `cid`, `type`, `timeMs`, `paramList`, `rankBefore`, `rankNow`";
    }

    @Override
    public String getTableName() {
        return "`np_arena_msg_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("'").append(timeMs).append("', ");
        strBuf.append("'").append(paramList == null ? null : paramList.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(rankBefore).append("', ");
        strBuf.append("'").append(rankNow).append("', ");
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

    // 实例id
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

    // 玩家id
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

    // 公告类型
    public int getType() { return this.type; }
    public void setType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type; 
        markField(_bm, FIELD_type); 
    }
    public void saveType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type;
        saveField(_bm, "type", type);
    }

    // 发布时间戳
    public long getTimeMs() { return this.timeMs; }
    public void setTimeMs(BM _bm, long timeMs) {
        if(timeMs==this.timeMs) 
            return;
        this.timeMs = timeMs; 
        markField(_bm, FIELD_timeMs); 
    }
    public void saveTimeMs(BM _bm, long timeMs) {
        if(timeMs==this.timeMs) 
            return;
        this.timeMs = timeMs;
        saveField(_bm, "timeMs", timeMs);
    }

    // 公告参数
    public String getParamList() { return this.paramList; }
    public void setParamList(BM _bm, String paramList) {
        if(paramList.equals(this.paramList)) 
            return;
        this.paramList = paramList; 
        markField(_bm, FIELD_paramList); 
    }
    public void saveParamList(BM _bm, String paramList) {
        if(paramList.equals(this.paramList)) 
            return;
        this.paramList = paramList;
        saveField(_bm, "paramList", paramList);
    }

    // 挑战前排位
    public int getRankBefore() { return this.rankBefore; }
    public void setRankBefore(BM _bm, int rankBefore) {
        if(rankBefore==this.rankBefore) 
            return;
        this.rankBefore = rankBefore; 
        markField(_bm, FIELD_rankBefore); 
    }
    public void saveRankBefore(BM _bm, int rankBefore) {
        if(rankBefore==this.rankBefore) 
            return;
        this.rankBefore = rankBefore;
        saveField(_bm, "rankBefore", rankBefore);
    }

    // 现在排位
    public int getRankNow() { return this.rankNow; }
    public void setRankNow(BM _bm, int rankNow) {
        if(rankNow==this.rankNow) 
            return;
        this.rankNow = rankNow; 
        markField(_bm, FIELD_rankNow); 
    }
    public void saveRankNow(BM _bm, int rankNow) {
        if(rankNow==this.rankNow) 
            return;
        this.rankNow = rankNow;
        saveField(_bm, "rankNow", rankNow);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `timeMs` = '").append(timeMs).append("',");
        sBuilder.append(" `paramList` = '").append(paramList == null ? null : paramList.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `rankBefore` = '").append(rankBefore).append("',");
        sBuilder.append(" `rankNow` = '").append(rankNow).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_timeMs)) sBuilder.append(" `timeMs` = '").append(timeMs).append("',");
        if(isFieldMarked(FIELD_paramList)) sBuilder.append(" `paramList` = '").append(paramList == null ? null : paramList.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_rankBefore)) sBuilder.append(" `rankBefore` = '").append(rankBefore).append("',");
        if(isFieldMarked(FIELD_rankNow)) sBuilder.append(" `rankNow` = '").append(rankNow).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_arena_msg_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '实例id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '公告类型',"
                + "`timeMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '发布时间戳',"
                + "`paramList` varchar(128) NOT NULL DEFAULT '' COMMENT '公告参数',"
                + "`rankBefore` int(11) NOT NULL DEFAULT '0' COMMENT '挑战前排位',"
                + "`rankNow` int(11) NOT NULL DEFAULT '0' COMMENT '现在排位',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='比武擂台 战斗公告表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//type
        _size+=8;//timeMs
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList);//paramList
        _size+=4;//rankBefore
        _size+=4;//rankNow
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putLong(cid);
        buff.putInt(type);
        buff.putLong(timeMs);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, paramList);
        buff.putInt(rankBefore);
        buff.putInt(rankNow);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        cid=buff.getLong();
        type=buff.getInt();
        timeMs=buff.getLong();
        paramList=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        rankBefore=buff.getInt();
        rankNow=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
