package HSDB.Bo;
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
public class HsAllServerMailServerIdBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_mail_db_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "mail_db_id", comment = "邮件唯一id")
    private long mail_db_id;

    public static final int FIELD_us_server_type_id =1;
    @DataBaseField(type = "int(11)", fieldname = "us_server_type_id", comment = "服务器typeId")
    private int us_server_type_id;

    public HsAllServerMailServerIdBO() {
        id = 0;
        mail_db_id = 0L;
        us_server_type_id = 0;
    }

    public HsAllServerMailServerIdBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        mail_db_id = rs.getLong(2);
        us_server_type_id = rs.getInt(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new HsAllServerMailServerIdBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `mail_db_id`, `us_server_type_id`";
    }

    @Override
    public String getTableName() {
        return "`hs_all_server_mail_server_id`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(mail_db_id).append("', ");
        strBuf.append("'").append(us_server_type_id).append("', ");
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

    // 邮件唯一id
    public long getMailDbId() { return this.mail_db_id; }
    public void setMailDbId(BM _bm, long mail_db_id) {
        if(mail_db_id==this.mail_db_id) 
            return;
        this.mail_db_id = mail_db_id; 
        markField(_bm, FIELD_mail_db_id); 
    }
    public void saveMailDbId(BM _bm, long mail_db_id) {
        if(mail_db_id==this.mail_db_id) 
            return;
        this.mail_db_id = mail_db_id;
        saveField(_bm, "mail_db_id", mail_db_id);
    }

    // 服务器typeId
    public int getUsServerTypeId() { return this.us_server_type_id; }
    public void setUsServerTypeId(BM _bm, int us_server_type_id) {
        if(us_server_type_id==this.us_server_type_id) 
            return;
        this.us_server_type_id = us_server_type_id; 
        markField(_bm, FIELD_us_server_type_id); 
    }
    public void saveUsServerTypeId(BM _bm, int us_server_type_id) {
        if(us_server_type_id==this.us_server_type_id) 
            return;
        this.us_server_type_id = us_server_type_id;
        saveField(_bm, "us_server_type_id", us_server_type_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `mail_db_id` = '").append(mail_db_id).append("',");
        sBuilder.append(" `us_server_type_id` = '").append(us_server_type_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_mail_db_id)) sBuilder.append(" `mail_db_id` = '").append(mail_db_id).append("',");
        if(isFieldMarked(FIELD_us_server_type_id)) sBuilder.append(" `us_server_type_id` = '").append(us_server_type_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `hs_all_server_mail_server_id` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`mail_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '邮件唯一id',"
                + "`us_server_type_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器typeId',"
                + "KEY `mail_db_id` (`mail_db_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='http 全服邮件-服务器列表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.hs_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//mail_db_id
        _size+=4;//us_server_type_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(mail_db_id);
        buff.putInt(us_server_type_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        mail_db_id=buff.getLong();
        us_server_type_id=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
