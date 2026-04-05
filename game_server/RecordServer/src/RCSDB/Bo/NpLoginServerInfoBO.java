package RCSDB.Bo;
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
public class NpLoginServerInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_account_id =0;
    @DataBaseField(type = "varchar(50)", fieldname = "account_id", comment = "玩家用户名")
    private String account_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家角色cid")
    private long cid;

    public static final int FIELD_last_login_server_id =2;
    @DataBaseField(type = "int(11)", fieldname = "last_login_server_id", comment = "上一次的登录服务器id")
    private int last_login_server_id;

    public static final int FIELD_last_login_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "last_login_time_ms", comment = "上一次登录时间戳")
    private long last_login_time_ms;

    public NpLoginServerInfoBO() {
        id = 0;
        account_id = "";
        cid = 0L;
        last_login_server_id = 0;
        last_login_time_ms = 0L;
    }

    public NpLoginServerInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        account_id = rs.getString(2);
        cid = rs.getLong(3);
        last_login_server_id = rs.getInt(4);
        last_login_time_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpLoginServerInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `account_id`, `cid`, `last_login_server_id`, `last_login_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`np_login_server_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(account_id == null ? null : account_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(last_login_server_id).append("', ");
        strBuf.append("'").append(last_login_time_ms).append("', ");
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

    // 玩家用户名
    public String getAccountId() { return this.account_id; }
    public void setAccountId(BM _bm, String account_id) {
        if(account_id.equals(this.account_id)) 
            return;
        this.account_id = account_id; 
        markField(_bm, FIELD_account_id); 
    }
    public void saveAccountId(BM _bm, String account_id) {
        if(account_id.equals(this.account_id)) 
            return;
        this.account_id = account_id;
        saveField(_bm, "account_id", account_id);
    }

    // 玩家角色cid
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

    // 上一次的登录服务器id
    public int getLastLoginServerId() { return this.last_login_server_id; }
    public void setLastLoginServerId(BM _bm, int last_login_server_id) {
        if(last_login_server_id==this.last_login_server_id) 
            return;
        this.last_login_server_id = last_login_server_id; 
        markField(_bm, FIELD_last_login_server_id); 
    }
    public void saveLastLoginServerId(BM _bm, int last_login_server_id) {
        if(last_login_server_id==this.last_login_server_id) 
            return;
        this.last_login_server_id = last_login_server_id;
        saveField(_bm, "last_login_server_id", last_login_server_id);
    }

    // 上一次登录时间戳
    public long getLastLoginTimeMs() { return this.last_login_time_ms; }
    public void setLastLoginTimeMs(BM _bm, long last_login_time_ms) {
        if(last_login_time_ms==this.last_login_time_ms) 
            return;
        this.last_login_time_ms = last_login_time_ms; 
        markField(_bm, FIELD_last_login_time_ms); 
    }
    public void saveLastLoginTimeMs(BM _bm, long last_login_time_ms) {
        if(last_login_time_ms==this.last_login_time_ms) 
            return;
        this.last_login_time_ms = last_login_time_ms;
        saveField(_bm, "last_login_time_ms", last_login_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `account_id` = '").append(account_id == null ? null : account_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `last_login_server_id` = '").append(last_login_server_id).append("',");
        sBuilder.append(" `last_login_time_ms` = '").append(last_login_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_account_id)) sBuilder.append(" `account_id` = '").append(account_id == null ? null : account_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_last_login_server_id)) sBuilder.append(" `last_login_server_id` = '").append(last_login_server_id).append("',");
        if(isFieldMarked(FIELD_last_login_time_ms)) sBuilder.append(" `last_login_time_ms` = '").append(last_login_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_login_server_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`account_id` varchar(50) NOT NULL DEFAULT '' COMMENT '玩家用户名',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家角色cid',"
                + "`last_login_server_id` int(11) NOT NULL DEFAULT '0' COMMENT '上一次的登录服务器id',"
                + "`last_login_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上一次登录时间戳',"
                + "KEY `account_id` (`account_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家登录到 US 记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.rcs_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(account_id);//account_id
        _size+=8;//cid
        _size+=4;//last_login_server_id
        _size+=8;//last_login_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, account_id);
        buff.putLong(cid);
        buff.putInt(last_login_server_id);
        buff.putLong(last_login_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        account_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cid=buff.getLong();
        last_login_server_id=buff.getInt();
        last_login_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
