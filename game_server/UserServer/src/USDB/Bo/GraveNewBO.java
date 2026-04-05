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
public class GraveNewBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_titleId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "titleId", comment = "称号ID")
    private long titleId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_endMs =2;
    @DataBaseField(type = "bigint(20)", fieldname = "endMs", comment = "结束时间，时间戳：毫秒")
    private long endMs;

    public static final int FIELD_congCids =3;
    @DataBaseField(type = "blob", fieldname = "congCids", comment = "注册玩家CID集合")
    private byte[] congCids;

    public GraveNewBO() {
        id = 0;
        titleId = 0L;
        cid = 0L;
        endMs = 0L;
        congCids = null;
    }

    public GraveNewBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        titleId = rs.getLong(2);
        cid = rs.getLong(3);
        endMs = rs.getLong(4);
        congCids = rs.getBytes(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GraveNewBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `titleId`, `cid`, `endMs`, `congCids`";
    }

    @Override
    public String getTableName() {
        return "`grave_new`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(titleId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(endMs).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(congCids);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_congCids)) ret.add(congCids);         return ret;
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

    // 结束时间，时间戳：毫秒
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

    // 注册玩家CID集合
    public byte[] getCongCids() { return this.congCids; }
    public void setCongCids(BM _bm, byte[] congCids) {
        if(congCids==this.congCids) 
            return;
        this.congCids = congCids; 
        markField(_bm, FIELD_congCids); 
    }
    public void saveCongCids(BM _bm, byte[] congCids) {
        if(congCids==this.congCids) 
            return;
        this.congCids = congCids;
        saveFieldBytes(_bm, "congCids", congCids);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `titleId` = '").append(titleId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `endMs` = '").append(endMs).append("',");
        sBuilder.append(" `congCids` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_titleId)) sBuilder.append(" `titleId` = '").append(titleId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_endMs)) sBuilder.append(" `endMs` = '").append(endMs).append("',");
        if(isFieldMarked(FIELD_congCids)) sBuilder.append(" `congCids` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `grave_new` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`titleId` bigint(20) NOT NULL DEFAULT '0' COMMENT '称号ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`endMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束时间，时间戳：毫秒',"
                + "`congCids` blob NULL COMMENT '注册玩家CID集合',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='新晋杰出者数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//endMs
        _size+=2;_size+=congCids.length;//congCids
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
        buff.putLong(endMs);
        buff.putShort((short)(congCids == null ? 0 : congCids.length));if(null != congCids){buff.put(congCids);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        titleId=buff.getLong();
        cid=buff.getLong();
        endMs=buff.getLong();
        int congCids_count = buff.getShort();if(congCids_count>0){congCids = new byte[congCids_count];buff.get(congCids);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
