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
public class QuestionnaireInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_url_link =0;
    @DataBaseField(type = "varchar(500)", fieldname = "url_link", comment = "链接")
    private String url_link;

    public static final int FIELD_questionnaire_code =1;
    @DataBaseField(type = "varchar(500)", fieldname = "questionnaire_code", comment = "问卷码")
    private String questionnaire_code;

    public static final int FIELD_start_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "start_time_ms", comment = "开始时间")
    private long start_time_ms;

    public static final int FIELD_close_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "close_time_ms", comment = "关闭时间")
    private long close_time_ms;

    public static final int FIELD_reward_list =4;
    @DataBaseField(type = "blob", fieldname = "reward_list", comment = "奖励列表")
    private byte[] reward_list;

    public static final int FIELD_is_close =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_close", comment = "是否关闭")
    private boolean is_close;

    public QuestionnaireInfoBO() {
        id = 0;
        url_link = "";
        questionnaire_code = "";
        start_time_ms = 0L;
        close_time_ms = 0L;
        reward_list = null;
        is_close = false;
    }

    public QuestionnaireInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        url_link = rs.getString(2);
        questionnaire_code = rs.getString(3);
        start_time_ms = rs.getLong(4);
        close_time_ms = rs.getLong(5);
        reward_list = rs.getBytes(6);
        is_close = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new QuestionnaireInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `url_link`, `questionnaire_code`, `start_time_ms`, `close_time_ms`, `reward_list`, `is_close`";
    }

    @Override
    public String getTableName() {
        return "`questionnaire_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(url_link == null ? null : url_link.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(questionnaire_code == null ? null : questionnaire_code.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(start_time_ms).append("', ");
        strBuf.append("'").append(close_time_ms).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(is_close ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(reward_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_reward_list)) ret.add(reward_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 链接
    public String getUrlLink() { return this.url_link; }
    public void setUrlLink(BM _bm, String url_link) {
        if(url_link.equals(this.url_link)) 
            return;
        this.url_link = url_link; 
        markField(_bm, FIELD_url_link); 
    }
    public void saveUrlLink(BM _bm, String url_link) {
        if(url_link.equals(this.url_link)) 
            return;
        this.url_link = url_link;
        saveField(_bm, "url_link", url_link);
    }

    // 问卷码
    public String getQuestionnaireCode() { return this.questionnaire_code; }
    public void setQuestionnaireCode(BM _bm, String questionnaire_code) {
        if(questionnaire_code.equals(this.questionnaire_code)) 
            return;
        this.questionnaire_code = questionnaire_code; 
        markField(_bm, FIELD_questionnaire_code); 
    }
    public void saveQuestionnaireCode(BM _bm, String questionnaire_code) {
        if(questionnaire_code.equals(this.questionnaire_code)) 
            return;
        this.questionnaire_code = questionnaire_code;
        saveField(_bm, "questionnaire_code", questionnaire_code);
    }

    // 开始时间
    public long getStartTimeMs() { return this.start_time_ms; }
    public void setStartTimeMs(BM _bm, long start_time_ms) {
        if(start_time_ms==this.start_time_ms) 
            return;
        this.start_time_ms = start_time_ms; 
        markField(_bm, FIELD_start_time_ms); 
    }
    public void saveStartTimeMs(BM _bm, long start_time_ms) {
        if(start_time_ms==this.start_time_ms) 
            return;
        this.start_time_ms = start_time_ms;
        saveField(_bm, "start_time_ms", start_time_ms);
    }

    // 关闭时间
    public long getCloseTimeMs() { return this.close_time_ms; }
    public void setCloseTimeMs(BM _bm, long close_time_ms) {
        if(close_time_ms==this.close_time_ms) 
            return;
        this.close_time_ms = close_time_ms; 
        markField(_bm, FIELD_close_time_ms); 
    }
    public void saveCloseTimeMs(BM _bm, long close_time_ms) {
        if(close_time_ms==this.close_time_ms) 
            return;
        this.close_time_ms = close_time_ms;
        saveField(_bm, "close_time_ms", close_time_ms);
    }

    // 奖励列表
    public byte[] getRewardList() { return this.reward_list; }
    public void setRewardList(BM _bm, byte[] reward_list) {
        if(reward_list==this.reward_list) 
            return;
        this.reward_list = reward_list; 
        markField(_bm, FIELD_reward_list); 
    }
    public void saveRewardList(BM _bm, byte[] reward_list) {
        if(reward_list==this.reward_list) 
            return;
        this.reward_list = reward_list;
        saveFieldBytes(_bm, "reward_list", reward_list);
    }

    // 是否关闭
    public boolean getIsClose() { return this.is_close; }
    public void setIsClose(BM _bm, boolean is_close) {
        if(is_close==this.is_close) 
            return;
        this.is_close = is_close; 
        markField(_bm, FIELD_is_close); 
    }
    public void saveIsClose(BM _bm, boolean is_close) {
        if(is_close==this.is_close) 
            return;
        this.is_close = is_close;
        saveField(_bm, "is_close", is_close ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `url_link` = '").append(url_link == null ? null : url_link.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `questionnaire_code` = '").append(questionnaire_code == null ? null : questionnaire_code.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        sBuilder.append(" `close_time_ms` = '").append(close_time_ms).append("',");
        sBuilder.append(" `reward_list` = ?,");
        sBuilder.append(" `is_close` = '").append(is_close ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_url_link)) sBuilder.append(" `url_link` = '").append(url_link == null ? null : url_link.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_questionnaire_code)) sBuilder.append(" `questionnaire_code` = '").append(questionnaire_code == null ? null : questionnaire_code.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_start_time_ms)) sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        if(isFieldMarked(FIELD_close_time_ms)) sBuilder.append(" `close_time_ms` = '").append(close_time_ms).append("',");
        if(isFieldMarked(FIELD_reward_list)) sBuilder.append(" `reward_list` = ?,");
        if(isFieldMarked(FIELD_is_close)) sBuilder.append(" `is_close` = '").append(is_close ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `questionnaire_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`url_link` varchar(500) NOT NULL DEFAULT '' COMMENT '链接',"
                + "`questionnaire_code` varchar(500) NOT NULL DEFAULT '' COMMENT '问卷码',"
                + "`start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始时间',"
                + "`close_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '关闭时间',"
                + "`reward_list` blob NULL COMMENT '奖励列表',"
                + "`is_close` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否关闭',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='问卷信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(url_link);//url_link
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(questionnaire_code);//questionnaire_code
        _size+=8;//start_time_ms
        _size+=8;//close_time_ms
        _size+=2;_size+=reward_list.length;//reward_list
        _size+=1;//is_close
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, url_link);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, questionnaire_code);
        buff.putLong(start_time_ms);
        buff.putLong(close_time_ms);
        buff.putShort((short)(reward_list == null ? 0 : reward_list.length));if(null != reward_list){buff.put(reward_list);}
        buff.put((byte)(is_close?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        url_link=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        questionnaire_code=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        start_time_ms=buff.getLong();
        close_time_ms=buff.getLong();
        int reward_list_count = buff.getShort();if(reward_list_count>0){reward_list = new byte[reward_list_count];buff.get(reward_list);}
        is_close=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
