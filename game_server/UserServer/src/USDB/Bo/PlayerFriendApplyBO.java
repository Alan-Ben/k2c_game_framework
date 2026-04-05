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
public class PlayerFriendApplyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_applyCid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "applyCid", comment = "申请玩家CID")
    private long applyCid;

    public static final int FIELD_applyTimeS =2;
    @DataBaseField(type = "int(11)", fieldname = "applyTimeS", comment = "申请时间")
    private int applyTimeS;

    public PlayerFriendApplyBO() {
        id = 0;
        cid = 0L;
        applyCid = 0L;
        applyTimeS = 0;
    }

    public PlayerFriendApplyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        applyCid = rs.getLong(3);
        applyTimeS = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerFriendApplyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `applyCid`, `applyTimeS`";
    }

    @Override
    public String getTableName() {
        return "`player_friend_apply`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(applyCid).append("', ");
        strBuf.append("'").append(applyTimeS).append("', ");
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

    // 申请玩家CID
    public long getApplyCid() { return this.applyCid; }
    public void setApplyCid(BM _bm, long applyCid) {
        if(applyCid==this.applyCid) 
            return;
        this.applyCid = applyCid; 
        markField(_bm, FIELD_applyCid); 
    }
    public void saveApplyCid(BM _bm, long applyCid) {
        if(applyCid==this.applyCid) 
            return;
        this.applyCid = applyCid;
        saveField(_bm, "applyCid", applyCid);
    }

    // 申请时间
    public int getApplyTimeS() { return this.applyTimeS; }
    public void setApplyTimeS(BM _bm, int applyTimeS) {
        if(applyTimeS==this.applyTimeS) 
            return;
        this.applyTimeS = applyTimeS; 
        markField(_bm, FIELD_applyTimeS); 
    }
    public void saveApplyTimeS(BM _bm, int applyTimeS) {
        if(applyTimeS==this.applyTimeS) 
            return;
        this.applyTimeS = applyTimeS;
        saveField(_bm, "applyTimeS", applyTimeS);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `applyCid` = '").append(applyCid).append("',");
        sBuilder.append(" `applyTimeS` = '").append(applyTimeS).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_applyCid)) sBuilder.append(" `applyCid` = '").append(applyCid).append("',");
        if(isFieldMarked(FIELD_applyTimeS)) sBuilder.append(" `applyTimeS` = '").append(applyTimeS).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_friend_apply` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`applyCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '申请玩家CID',"
                + "`applyTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '申请时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Player Friend 玩家好友申请数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//applyCid
        _size+=4;//applyTimeS
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(applyCid);
        buff.putInt(applyTimeS);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        applyCid=buff.getLong();
        applyTimeS=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
