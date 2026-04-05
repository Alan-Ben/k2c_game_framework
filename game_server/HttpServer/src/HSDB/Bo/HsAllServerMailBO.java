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
public class HsAllServerMailBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_mail_ref_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "mail_ref_id", comment = "邮件配表配置id")
    private long mail_ref_id;

    public static final int FIELD_php_mail_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "php_mail_id", comment = "邮件配表配置id")
    private long php_mail_id;

    public static final int FIELD_send_time =2;
    @DataBaseField(type = "varchar(32)", fieldname = "send_time", comment = "玩家看到的邮件接收时间")
    private String send_time;

    public static final int FIELD_expired_time =3;
    @DataBaseField(type = "varchar(32)", fieldname = "expired_time", comment = "失效时间")
    private String expired_time;

    public static final int FIELD_default_lang =4;
    @DataBaseField(type = "varchar(32)", fieldname = "default_lang", comment = "默认语言")
    private String default_lang;

    public static final int FIELD_passed_time_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "passed_time_ms", comment = "邮件审核时间，用于邮件组标识位处理")
    private long passed_time_ms;

    public static final int FIELD_content_replace =6;
    @DataBaseField(type = "blob", fieldname = "content_replace", comment = "内容替换数据")
    private byte[] content_replace;

    public HsAllServerMailBO() {
        id = 0;
        mail_ref_id = 0L;
        php_mail_id = 0L;
        send_time = "";
        expired_time = "";
        default_lang = "";
        passed_time_ms = 0L;
        content_replace = null;
    }

    public HsAllServerMailBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        mail_ref_id = rs.getLong(2);
        php_mail_id = rs.getLong(3);
        send_time = rs.getString(4);
        expired_time = rs.getString(5);
        default_lang = rs.getString(6);
        passed_time_ms = rs.getLong(7);
        content_replace = rs.getBytes(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new HsAllServerMailBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `mail_ref_id`, `php_mail_id`, `send_time`, `expired_time`, `default_lang`, `passed_time_ms`, `content_replace`";
    }

    @Override
    public String getTableName() {
        return "`hs_all_server_mail`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(mail_ref_id).append("', ");
        strBuf.append("'").append(php_mail_id).append("', ");
        strBuf.append("'").append(send_time == null ? null : send_time.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(expired_time == null ? null : expired_time.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(default_lang == null ? null : default_lang.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(passed_time_ms).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(content_replace);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_content_replace)) ret.add(content_replace);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 邮件配表配置id
    public long getMailRefId() { return this.mail_ref_id; }
    public void setMailRefId(BM _bm, long mail_ref_id) {
        if(mail_ref_id==this.mail_ref_id) 
            return;
        this.mail_ref_id = mail_ref_id; 
        markField(_bm, FIELD_mail_ref_id); 
    }
    public void saveMailRefId(BM _bm, long mail_ref_id) {
        if(mail_ref_id==this.mail_ref_id) 
            return;
        this.mail_ref_id = mail_ref_id;
        saveField(_bm, "mail_ref_id", mail_ref_id);
    }

    // 邮件配表配置id
    public long getPhpMailId() { return this.php_mail_id; }
    public void setPhpMailId(BM _bm, long php_mail_id) {
        if(php_mail_id==this.php_mail_id) 
            return;
        this.php_mail_id = php_mail_id; 
        markField(_bm, FIELD_php_mail_id); 
    }
    public void savePhpMailId(BM _bm, long php_mail_id) {
        if(php_mail_id==this.php_mail_id) 
            return;
        this.php_mail_id = php_mail_id;
        saveField(_bm, "php_mail_id", php_mail_id);
    }

    // 玩家看到的邮件接收时间
    public String getSendTime() { return this.send_time; }
    public void setSendTime(BM _bm, String send_time) {
        if(send_time.equals(this.send_time)) 
            return;
        this.send_time = send_time; 
        markField(_bm, FIELD_send_time); 
    }
    public void saveSendTime(BM _bm, String send_time) {
        if(send_time.equals(this.send_time)) 
            return;
        this.send_time = send_time;
        saveField(_bm, "send_time", send_time);
    }

    // 失效时间
    public String getExpiredTime() { return this.expired_time; }
    public void setExpiredTime(BM _bm, String expired_time) {
        if(expired_time.equals(this.expired_time)) 
            return;
        this.expired_time = expired_time; 
        markField(_bm, FIELD_expired_time); 
    }
    public void saveExpiredTime(BM _bm, String expired_time) {
        if(expired_time.equals(this.expired_time)) 
            return;
        this.expired_time = expired_time;
        saveField(_bm, "expired_time", expired_time);
    }

    // 默认语言
    public String getDefaultLang() { return this.default_lang; }
    public void setDefaultLang(BM _bm, String default_lang) {
        if(default_lang.equals(this.default_lang)) 
            return;
        this.default_lang = default_lang; 
        markField(_bm, FIELD_default_lang); 
    }
    public void saveDefaultLang(BM _bm, String default_lang) {
        if(default_lang.equals(this.default_lang)) 
            return;
        this.default_lang = default_lang;
        saveField(_bm, "default_lang", default_lang);
    }

    // 邮件审核时间，用于邮件组标识位处理
    public long getPassedTimeMs() { return this.passed_time_ms; }
    public void setPassedTimeMs(BM _bm, long passed_time_ms) {
        if(passed_time_ms==this.passed_time_ms) 
            return;
        this.passed_time_ms = passed_time_ms; 
        markField(_bm, FIELD_passed_time_ms); 
    }
    public void savePassedTimeMs(BM _bm, long passed_time_ms) {
        if(passed_time_ms==this.passed_time_ms) 
            return;
        this.passed_time_ms = passed_time_ms;
        saveField(_bm, "passed_time_ms", passed_time_ms);
    }

    // 内容替换数据
    public byte[] getContentReplace() { return this.content_replace; }
    public void setContentReplace(BM _bm, byte[] content_replace) {
        if(content_replace==this.content_replace) 
            return;
        this.content_replace = content_replace; 
        markField(_bm, FIELD_content_replace); 
    }
    public void saveContentReplace(BM _bm, byte[] content_replace) {
        if(content_replace==this.content_replace) 
            return;
        this.content_replace = content_replace;
        saveFieldBytes(_bm, "content_replace", content_replace);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `mail_ref_id` = '").append(mail_ref_id).append("',");
        sBuilder.append(" `php_mail_id` = '").append(php_mail_id).append("',");
        sBuilder.append(" `send_time` = '").append(send_time == null ? null : send_time.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `expired_time` = '").append(expired_time == null ? null : expired_time.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `default_lang` = '").append(default_lang == null ? null : default_lang.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `passed_time_ms` = '").append(passed_time_ms).append("',");
        sBuilder.append(" `content_replace` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_mail_ref_id)) sBuilder.append(" `mail_ref_id` = '").append(mail_ref_id).append("',");
        if(isFieldMarked(FIELD_php_mail_id)) sBuilder.append(" `php_mail_id` = '").append(php_mail_id).append("',");
        if(isFieldMarked(FIELD_send_time)) sBuilder.append(" `send_time` = '").append(send_time == null ? null : send_time.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_expired_time)) sBuilder.append(" `expired_time` = '").append(expired_time == null ? null : expired_time.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_default_lang)) sBuilder.append(" `default_lang` = '").append(default_lang == null ? null : default_lang.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_passed_time_ms)) sBuilder.append(" `passed_time_ms` = '").append(passed_time_ms).append("',");
        if(isFieldMarked(FIELD_content_replace)) sBuilder.append(" `content_replace` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `hs_all_server_mail` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`mail_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '邮件配表配置id',"
                + "`php_mail_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '邮件配表配置id',"
                + "`send_time` varchar(32) NOT NULL DEFAULT '' COMMENT '玩家看到的邮件接收时间',"
                + "`expired_time` varchar(32) NOT NULL DEFAULT '' COMMENT '失效时间',"
                + "`default_lang` varchar(32) NOT NULL DEFAULT '' COMMENT '默认语言',"
                + "`passed_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '邮件审核时间，用于邮件组标识位处理',"
                + "`content_replace` blob NULL COMMENT '内容替换数据',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='http 全服邮件' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//mail_ref_id
        _size+=8;//php_mail_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(send_time);//send_time
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(expired_time);//expired_time
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(default_lang);//default_lang
        _size+=8;//passed_time_ms
        _size+=2;_size+=content_replace.length;//content_replace
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(mail_ref_id);
        buff.putLong(php_mail_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, send_time);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, expired_time);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, default_lang);
        buff.putLong(passed_time_ms);
        buff.putShort((short)(content_replace == null ? 0 : content_replace.length));if(null != content_replace){buff.put(content_replace);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        mail_ref_id=buff.getLong();
        php_mail_id=buff.getLong();
        send_time=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        expired_time=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        default_lang=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        passed_time_ms=buff.getLong();
        int content_replace_count = buff.getShort();if(content_replace_count>0){content_replace = new byte[content_replace_count];buff.get(content_replace);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
