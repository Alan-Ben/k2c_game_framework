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
public class HsAllServerMailTextBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_mail_db_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "mail_db_id", comment = "邮件唯一id")
    private long mail_db_id;

    public static final int FIELD_lang =1;
    @DataBaseField(type = "varchar(32)", fieldname = "lang", comment = "语言编号(查看公共参数中的语言ID对应表)")
    private String lang;

    public static final int FIELD_title =2;
    @DataBaseField(type = "varchar(128)", fieldname = "title", comment = "对应语种的邮件标题")
    private String title;

    public static final int FIELD_content =3;
    @DataBaseField(type = "varchar(1024)", fieldname = "content", comment = "对应语种的邮件内容")
    private String content;

    public HsAllServerMailTextBO() {
        id = 0;
        mail_db_id = 0L;
        lang = "";
        title = "";
        content = "";
    }

    public HsAllServerMailTextBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        mail_db_id = rs.getLong(2);
        lang = rs.getString(3);
        title = rs.getString(4);
        content = rs.getString(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new HsAllServerMailTextBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `mail_db_id`, `lang`, `title`, `content`";
    }

    @Override
    public String getTableName() {
        return "`hs_all_server_mail_text`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(mail_db_id).append("', ");
        strBuf.append("'").append(lang == null ? null : lang.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 语言编号(查看公共参数中的语言ID对应表)
    public String getLang() { return this.lang; }
    public void setLang(BM _bm, String lang) {
        if(lang.equals(this.lang)) 
            return;
        this.lang = lang; 
        markField(_bm, FIELD_lang); 
    }
    public void saveLang(BM _bm, String lang) {
        if(lang.equals(this.lang)) 
            return;
        this.lang = lang;
        saveField(_bm, "lang", lang);
    }

    // 对应语种的邮件标题
    public String getTitle() { return this.title; }
    public void setTitle(BM _bm, String title) {
        if(title.equals(this.title)) 
            return;
        this.title = title; 
        markField(_bm, FIELD_title); 
    }
    public void saveTitle(BM _bm, String title) {
        if(title.equals(this.title)) 
            return;
        this.title = title;
        saveField(_bm, "title", title);
    }

    // 对应语种的邮件内容
    public String getContent() { return this.content; }
    public void setContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content; 
        markField(_bm, FIELD_content); 
    }
    public void saveContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content;
        saveField(_bm, "content", content);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `mail_db_id` = '").append(mail_db_id).append("',");
        sBuilder.append(" `lang` = '").append(lang == null ? null : lang.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_mail_db_id)) sBuilder.append(" `mail_db_id` = '").append(mail_db_id).append("',");
        if(isFieldMarked(FIELD_lang)) sBuilder.append(" `lang` = '").append(lang == null ? null : lang.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_title)) sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `hs_all_server_mail_text` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`mail_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '邮件唯一id',"
                + "`lang` varchar(32) NOT NULL DEFAULT '' COMMENT '语言编号(查看公共参数中的语言ID对应表)',"
                + "`title` varchar(128) NOT NULL DEFAULT '' COMMENT '对应语种的邮件标题',"
                + "`content` varchar(1024) NOT NULL DEFAULT '' COMMENT '对应语种的邮件内容',"
                + "KEY `mail_db_id` (`mail_db_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='http 全服邮件-文本数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lang);//lang
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);//title
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(mail_db_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, lang);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, title);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        mail_db_id=buff.getLong();
        lang=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        title=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
