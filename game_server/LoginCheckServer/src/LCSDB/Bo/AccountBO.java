package LCSDB.Bo;
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
public class AccountBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_accName =0;
    @DataBaseField(type = "varchar(32)", fieldname = "accName", comment = "账号名")
    private String accName;

    public static final int FIELD_chkKey =1;
    @DataBaseField(type = "varchar(32)", fieldname = "chkKey", comment = "验证串")
    private String chkKey;

    public static final int FIELD_lastLoginTime =2;
    @DataBaseField(type = "int(11)", fieldname = "lastLoginTime", comment = "最后登录时间")
    private int lastLoginTime;

    public AccountBO() {
        id = 0;
        accName = "";
        chkKey = "";
        lastLoginTime = 0;
    }

    public AccountBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        accName = rs.getString(2);
        chkKey = rs.getString(3);
        lastLoginTime = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new AccountBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `accName`, `chkKey`, `lastLoginTime`";
    }

    @Override
    public String getTableName() {
        return "`account`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(accName == null ? null : accName.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(chkKey == null ? null : chkKey.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(lastLoginTime).append("', ");
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

    // 账号名
    public String getAccName() { return this.accName; }
    public void setAccName(BM _bm, String accName) {
        if(accName.equals(this.accName)) 
            return;
        this.accName = accName; 
        markField(_bm, FIELD_accName); 
    }
    public void saveAccName(BM _bm, String accName) {
        if(accName.equals(this.accName)) 
            return;
        this.accName = accName;
        saveField(_bm, "accName", accName);
    }

    // 验证串
    public String getChkKey() { return this.chkKey; }
    public void setChkKey(BM _bm, String chkKey) {
        if(chkKey.equals(this.chkKey)) 
            return;
        this.chkKey = chkKey; 
        markField(_bm, FIELD_chkKey); 
    }
    public void saveChkKey(BM _bm, String chkKey) {
        if(chkKey.equals(this.chkKey)) 
            return;
        this.chkKey = chkKey;
        saveField(_bm, "chkKey", chkKey);
    }

    // 最后登录时间
    public int getLastLoginTime() { return this.lastLoginTime; }
    public void setLastLoginTime(BM _bm, int lastLoginTime) {
        if(lastLoginTime==this.lastLoginTime) 
            return;
        this.lastLoginTime = lastLoginTime; 
        markField(_bm, FIELD_lastLoginTime); 
    }
    public void saveLastLoginTime(BM _bm, int lastLoginTime) {
        if(lastLoginTime==this.lastLoginTime) 
            return;
        this.lastLoginTime = lastLoginTime;
        saveField(_bm, "lastLoginTime", lastLoginTime);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `accName` = '").append(accName == null ? null : accName.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `chkKey` = '").append(chkKey == null ? null : chkKey.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `lastLoginTime` = '").append(lastLoginTime).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_accName)) sBuilder.append(" `accName` = '").append(accName == null ? null : accName.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_chkKey)) sBuilder.append(" `chkKey` = '").append(chkKey == null ? null : chkKey.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_lastLoginTime)) sBuilder.append(" `lastLoginTime` = '").append(lastLoginTime).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `account` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`accName` varchar(32) NOT NULL DEFAULT '' COMMENT '账号名',"
                + "`chkKey` varchar(32) NOT NULL DEFAULT '' COMMENT '验证串',"
                + "`lastLoginTime` int(11) NOT NULL DEFAULT '0' COMMENT '最后登录时间',"
                + "UNIQUE INDEX `accName` (`accName`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='账号信息表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.account_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accName);//accName
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chkKey);//chkKey
        _size+=4;//lastLoginTime
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, accName);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, chkKey);
        buff.putInt(lastLoginTime);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        accName=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        chkKey=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        lastLoginTime=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
