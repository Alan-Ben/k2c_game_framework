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
public class PlayerConsortCgBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_cgId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cgId", comment = "CG")
    private long cgId;

    public static final int FIELD_rewarded =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "rewarded", comment = "是否领取奖励")
    private boolean rewarded;

    public PlayerConsortCgBO() {
        id = 0;
        cid = 0L;
        cgId = 0L;
        rewarded = false;
    }

    public PlayerConsortCgBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        cgId = rs.getLong(3);
        rewarded = rs.getBoolean(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortCgBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `cgId`, `rewarded`";
    }

    @Override
    public String getTableName() {
        return "`player_consort_cg`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(cgId).append("', ");
        strBuf.append("'").append(rewarded ? 1 : 0).append("', ");
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

    // CG
    public long getCgId() { return this.cgId; }
    public void setCgId(BM _bm, long cgId) {
        if(cgId==this.cgId) 
            return;
        this.cgId = cgId; 
        markField(_bm, FIELD_cgId); 
    }
    public void saveCgId(BM _bm, long cgId) {
        if(cgId==this.cgId) 
            return;
        this.cgId = cgId;
        saveField(_bm, "cgId", cgId);
    }

    // 是否领取奖励
    public boolean getRewarded() { return this.rewarded; }
    public void setRewarded(BM _bm, boolean rewarded) {
        if(rewarded==this.rewarded) 
            return;
        this.rewarded = rewarded; 
        markField(_bm, FIELD_rewarded); 
    }
    public void saveRewarded(BM _bm, boolean rewarded) {
        if(rewarded==this.rewarded) 
            return;
        this.rewarded = rewarded;
        saveField(_bm, "rewarded", rewarded ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `cgId` = '").append(cgId).append("',");
        sBuilder.append(" `rewarded` = '").append(rewarded ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_cgId)) sBuilder.append(" `cgId` = '").append(cgId).append("',");
        if(isFieldMarked(FIELD_rewarded)) sBuilder.append(" `rewarded` = '").append(rewarded ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort_cg` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`cgId` bigint(20) NOT NULL DEFAULT '0' COMMENT 'CG',"
                + "`rewarded` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否领取奖励',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家妃子CG数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cgId
        _size+=1;//rewarded
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(cgId);
        buff.put((byte)(rewarded?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        cgId=buff.getLong();
        rewarded=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
