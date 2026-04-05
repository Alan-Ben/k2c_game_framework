package CSDB.Bo;
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
public class PlayerFreezeInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_uid =0;
    @DataBaseField(type = "varchar(32)", fieldname = "uid", comment = "玩家uid")
    private String uid;

    public static final int FIELD_freeze_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "freeze_time_ms", comment = "冻结结束时间戳")
    private long freeze_time_ms;

    public PlayerFreezeInfoBO() {
        id = 0;
        uid = "";
        freeze_time_ms = 0L;
    }

    public PlayerFreezeInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        uid = rs.getString(2);
        freeze_time_ms = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerFreezeInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `uid`, `freeze_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_freeze_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(freeze_time_ms).append("', ");
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

    // 玩家uid
    public String getUid() { return this.uid; }
    public void setUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 冻结结束时间戳
    public long getFreezeTimeMs() { return this.freeze_time_ms; }
    public void setFreezeTimeMs(BM _bm, long freeze_time_ms) {
        if(freeze_time_ms==this.freeze_time_ms) 
            return;
        this.freeze_time_ms = freeze_time_ms; 
        markField(_bm, FIELD_freeze_time_ms); 
    }
    public void saveFreezeTimeMs(BM _bm, long freeze_time_ms) {
        if(freeze_time_ms==this.freeze_time_ms) 
            return;
        this.freeze_time_ms = freeze_time_ms;
        saveField(_bm, "freeze_time_ms", freeze_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `freeze_time_ms` = '").append(freeze_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_freeze_time_ms)) sBuilder.append(" `freeze_time_ms` = '").append(freeze_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_freeze_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`uid` varchar(32) NOT NULL DEFAULT '' COMMENT '玩家uid',"
                + "`freeze_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '冻结结束时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='冻结用户' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size+=8;//freeze_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        buff.putLong(freeze_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        freeze_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
