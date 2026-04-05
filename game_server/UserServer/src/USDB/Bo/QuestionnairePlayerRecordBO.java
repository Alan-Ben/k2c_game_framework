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
public class QuestionnairePlayerRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_questionnaire_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "questionnaire_id", comment = "问卷实例id")
    private long questionnaire_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "cid")
    private long cid;

    public static final int FIELD_has_draw =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_draw", comment = "是否领取")
    private boolean has_draw;

    public QuestionnairePlayerRecordBO() {
        id = 0;
        questionnaire_id = 0L;
        cid = 0L;
        has_draw = false;
    }

    public QuestionnairePlayerRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        questionnaire_id = rs.getLong(2);
        cid = rs.getLong(3);
        has_draw = rs.getBoolean(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new QuestionnairePlayerRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `questionnaire_id`, `cid`, `has_draw`";
    }

    @Override
    public String getTableName() {
        return "`questionnaire_player_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(questionnaire_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(has_draw ? 1 : 0).append("', ");
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

    // 问卷实例id
    public long getQuestionnaireId() { return this.questionnaire_id; }
    public void setQuestionnaireId(BM _bm, long questionnaire_id) {
        if(questionnaire_id==this.questionnaire_id) 
            return;
        this.questionnaire_id = questionnaire_id; 
        markField(_bm, FIELD_questionnaire_id); 
    }
    public void saveQuestionnaireId(BM _bm, long questionnaire_id) {
        if(questionnaire_id==this.questionnaire_id) 
            return;
        this.questionnaire_id = questionnaire_id;
        saveField(_bm, "questionnaire_id", questionnaire_id);
    }

    // cid
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

    // 是否领取
    public boolean getHasDraw() { return this.has_draw; }
    public void setHasDraw(BM _bm, boolean has_draw) {
        if(has_draw==this.has_draw) 
            return;
        this.has_draw = has_draw; 
        markField(_bm, FIELD_has_draw); 
    }
    public void saveHasDraw(BM _bm, boolean has_draw) {
        if(has_draw==this.has_draw) 
            return;
        this.has_draw = has_draw;
        saveField(_bm, "has_draw", has_draw ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `questionnaire_id` = '").append(questionnaire_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `has_draw` = '").append(has_draw ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_questionnaire_id)) sBuilder.append(" `questionnaire_id` = '").append(questionnaire_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_has_draw)) sBuilder.append(" `has_draw` = '").append(has_draw ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `questionnaire_player_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`questionnaire_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '问卷实例id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT 'cid',"
                + "`has_draw` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否领取',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='问卷玩家记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//questionnaire_id
        _size+=8;//cid
        _size+=1;//has_draw
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(questionnaire_id);
        buff.putLong(cid);
        buff.put((byte)(has_draw?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        questionnaire_id=buff.getLong();
        cid=buff.getLong();
        has_draw=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
