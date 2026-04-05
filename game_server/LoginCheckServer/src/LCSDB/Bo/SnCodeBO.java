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
public class SnCodeBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_sn =0;
    @DataBaseField(type = "varchar(128)", fieldname = "sn", comment = "激活码")
    private String sn;

    public static final int FIELD_pid =1;
    @DataBaseField(type = "varchar(32)", fieldname = "pid", comment = "使用的账号")
    private String pid;

    public SnCodeBO() {
        id = 0;
        sn = "";
        pid = "";
    }

    public SnCodeBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sn = rs.getString(2);
        pid = rs.getString(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new SnCodeBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sn`, `pid`";
    }

    @Override
    public String getTableName() {
        return "`sn_code`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sn == null ? null : sn.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pid == null ? null : pid.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 激活码
    public String getSn() { return this.sn; }
    public void setSn(BM _bm, String sn) {
        if(sn.equals(this.sn)) 
            return;
        this.sn = sn; 
        markField(_bm, FIELD_sn); 
    }
    public void saveSn(BM _bm, String sn) {
        if(sn.equals(this.sn)) 
            return;
        this.sn = sn;
        saveField(_bm, "sn", sn);
    }

    // 使用的账号
    public String getPid() { return this.pid; }
    public void setPid(BM _bm, String pid) {
        if(pid.equals(this.pid)) 
            return;
        this.pid = pid; 
        markField(_bm, FIELD_pid); 
    }
    public void savePid(BM _bm, String pid) {
        if(pid.equals(this.pid)) 
            return;
        this.pid = pid;
        saveField(_bm, "pid", pid);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `sn` = '").append(sn == null ? null : sn.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pid` = '").append(pid == null ? null : pid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_sn)) sBuilder.append(" `sn` = '").append(sn == null ? null : sn.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pid)) sBuilder.append(" `pid` = '").append(pid == null ? null : pid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `sn_code` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sn` varchar(128) NOT NULL DEFAULT '' COMMENT '激活码',"
                + "`pid` varchar(32) NOT NULL DEFAULT '' COMMENT '使用的账号',"
                + "UNIQUE INDEX `sn` (`sn`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='激活码' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sn);//sn
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pid);//pid
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sn);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, pid);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        sn=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
