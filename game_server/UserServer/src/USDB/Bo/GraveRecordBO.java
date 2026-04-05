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
public class GraveRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_titleId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "titleId", comment = "称号ID")
    private long titleId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_achieveTs =2;
    @DataBaseField(type = "int(11)", fieldname = "achieveTs", comment = "达成时间，时间戳：秒")
    private int achieveTs;

    public GraveRecordBO() {
        id = 0;
        titleId = 0L;
        cid = 0L;
        achieveTs = 0;
    }

    public GraveRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        titleId = rs.getLong(2);
        cid = rs.getLong(3);
        achieveTs = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GraveRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `titleId`, `cid`, `achieveTs`";
    }

    @Override
    public String getTableName() {
        return "`grave_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(titleId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(achieveTs).append("', ");
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

    // 达成时间，时间戳：秒
    public int getAchieveTs() { return this.achieveTs; }
    public void setAchieveTs(BM _bm, int achieveTs) {
        if(achieveTs==this.achieveTs) 
            return;
        this.achieveTs = achieveTs; 
        markField(_bm, FIELD_achieveTs); 
    }
    public void saveAchieveTs(BM _bm, int achieveTs) {
        if(achieveTs==this.achieveTs) 
            return;
        this.achieveTs = achieveTs;
        saveField(_bm, "achieveTs", achieveTs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `titleId` = '").append(titleId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `achieveTs` = '").append(achieveTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_titleId)) sBuilder.append(" `titleId` = '").append(titleId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_achieveTs)) sBuilder.append(" `achieveTs` = '").append(achieveTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `grave_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`titleId` bigint(20) NOT NULL DEFAULT '0' COMMENT '称号ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`achieveTs` int(11) NOT NULL DEFAULT '0' COMMENT '达成时间，时间戳：秒',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='杰出者记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//titleId
        _size+=8;//cid
        _size+=4;//achieveTs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(titleId);
        buff.putLong(cid);
        buff.putInt(achieveTs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        titleId=buff.getLong();
        cid=buff.getLong();
        achieveTs=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
