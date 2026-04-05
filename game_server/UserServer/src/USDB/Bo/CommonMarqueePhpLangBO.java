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
public class CommonMarqueePhpLangBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_marquee_dbid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "marquee_dbid", comment = "跑马灯实例id")
    private long marquee_dbid;

    public static final int FIELD_lang =1;
    @DataBaseField(type = "varchar(500)", fieldname = "lang", comment = "语言ID")
    private String lang;

    public static final int FIELD_param_list =2;
    @DataBaseField(type = "blob", fieldname = "param_list", comment = "参数列表")
    private byte[] param_list;

    public static final int FIELD_content =3;
    @DataBaseField(type = "varchar(2048)", fieldname = "content", comment = "内容")
    private String content;

    public CommonMarqueePhpLangBO() {
        id = 0;
        marquee_dbid = 0L;
        lang = "";
        param_list = null;
        content = "";
    }

    public CommonMarqueePhpLangBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        marquee_dbid = rs.getLong(2);
        lang = rs.getString(3);
        param_list = rs.getBytes(4);
        content = rs.getString(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CommonMarqueePhpLangBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `marquee_dbid`, `lang`, `param_list`, `content`";
    }

    @Override
    public String getTableName() {
        return "`common_marquee_php_lang`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(marquee_dbid).append("', ");
        strBuf.append("'").append(lang == null ? null : lang.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(param_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_param_list)) ret.add(param_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 跑马灯实例id
    public long getMarqueeDbid() { return this.marquee_dbid; }
    public void setMarqueeDbid(BM _bm, long marquee_dbid) {
        if(marquee_dbid==this.marquee_dbid) 
            return;
        this.marquee_dbid = marquee_dbid; 
        markField(_bm, FIELD_marquee_dbid); 
    }
    public void saveMarqueeDbid(BM _bm, long marquee_dbid) {
        if(marquee_dbid==this.marquee_dbid) 
            return;
        this.marquee_dbid = marquee_dbid;
        saveField(_bm, "marquee_dbid", marquee_dbid);
    }

    // 语言ID
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

    // 参数列表
    public byte[] getParamList() { return this.param_list; }
    public void setParamList(BM _bm, byte[] param_list) {
        if(param_list==this.param_list) 
            return;
        this.param_list = param_list; 
        markField(_bm, FIELD_param_list); 
    }
    public void saveParamList(BM _bm, byte[] param_list) {
        if(param_list==this.param_list) 
            return;
        this.param_list = param_list;
        saveFieldBytes(_bm, "param_list", param_list);
    }

    // 内容
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
        sBuilder.append(" `marquee_dbid` = '").append(marquee_dbid).append("',");
        sBuilder.append(" `lang` = '").append(lang == null ? null : lang.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `param_list` = ?,");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_marquee_dbid)) sBuilder.append(" `marquee_dbid` = '").append(marquee_dbid).append("',");
        if(isFieldMarked(FIELD_lang)) sBuilder.append(" `lang` = '").append(lang == null ? null : lang.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_param_list)) sBuilder.append(" `param_list` = ?,");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `common_marquee_php_lang` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`marquee_dbid` bigint(20) NOT NULL DEFAULT '0' COMMENT '跑马灯实例id',"
                + "`lang` varchar(500) NOT NULL DEFAULT '' COMMENT '语言ID',"
                + "`param_list` blob NULL COMMENT '参数列表',"
                + "`content` varchar(2048) NOT NULL DEFAULT '' COMMENT '内容',"
                + "KEY `marquee_dbid` (`marquee_dbid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跑马灯信息-后台多语言内容' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//marquee_dbid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lang);//lang
        _size+=2;_size+=param_list.length;//param_list
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(marquee_dbid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, lang);
        buff.putShort((short)(param_list == null ? 0 : param_list.length));if(null != param_list){buff.put(param_list);}
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        marquee_dbid=buff.getLong();
        lang=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        int param_list_count = buff.getShort();if(param_list_count>0){param_list = new byte[param_list_count];buff.get(param_list);}
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
