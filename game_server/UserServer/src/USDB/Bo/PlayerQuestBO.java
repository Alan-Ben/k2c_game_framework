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
public class PlayerQuestBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_quest_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_id", comment = "任务ID")
    private long quest_id;

    public static final int FIELD_quest_step =2;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_step", comment = "任务步骤")
    private long quest_step;

    public static final int FIELD_step_expire_time_tag =3;
    @DataBaseField(type = "int(11)", fieldname = "step_expire_time_tag", comment = "步骤截至时间戳（秒）")
    private int step_expire_time_tag;

    public PlayerQuestBO() {
        id = 0;
        cid = 0L;
        quest_id = 0L;
        quest_step = 0L;
        step_expire_time_tag = 0;
    }

    public PlayerQuestBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        quest_id = rs.getLong(3);
        quest_step = rs.getLong(4);
        step_expire_time_tag = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerQuestBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `quest_id`, `quest_step`, `step_expire_time_tag`";
    }

    @Override
    public String getTableName() {
        return "`player_quest`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(quest_id).append("', ");
        strBuf.append("'").append(quest_step).append("', ");
        strBuf.append("'").append(step_expire_time_tag).append("', ");
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

    // 玩家CID
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

    // 任务ID
    public long getQuestId() { return this.quest_id; }
    public void setQuestId(BM _bm, long quest_id) {
        if(quest_id==this.quest_id) 
            return;
        this.quest_id = quest_id; 
        markField(_bm, FIELD_quest_id); 
    }
    public void saveQuestId(BM _bm, long quest_id) {
        if(quest_id==this.quest_id) 
            return;
        this.quest_id = quest_id;
        saveField(_bm, "quest_id", quest_id);
    }

    // 任务步骤
    public long getQuestStep() { return this.quest_step; }
    public void setQuestStep(BM _bm, long quest_step) {
        if(quest_step==this.quest_step) 
            return;
        this.quest_step = quest_step; 
        markField(_bm, FIELD_quest_step); 
    }
    public void saveQuestStep(BM _bm, long quest_step) {
        if(quest_step==this.quest_step) 
            return;
        this.quest_step = quest_step;
        saveField(_bm, "quest_step", quest_step);
    }

    // 步骤截至时间戳（秒）
    public int getStepExpireTimeTag() { return this.step_expire_time_tag; }
    public void setStepExpireTimeTag(BM _bm, int step_expire_time_tag) {
        if(step_expire_time_tag==this.step_expire_time_tag) 
            return;
        this.step_expire_time_tag = step_expire_time_tag; 
        markField(_bm, FIELD_step_expire_time_tag); 
    }
    public void saveStepExpireTimeTag(BM _bm, int step_expire_time_tag) {
        if(step_expire_time_tag==this.step_expire_time_tag) 
            return;
        this.step_expire_time_tag = step_expire_time_tag;
        saveField(_bm, "step_expire_time_tag", step_expire_time_tag);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        sBuilder.append(" `quest_step` = '").append(quest_step).append("',");
        sBuilder.append(" `step_expire_time_tag` = '").append(step_expire_time_tag).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_quest_id)) sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        if(isFieldMarked(FIELD_quest_step)) sBuilder.append(" `quest_step` = '").append(quest_step).append("',");
        if(isFieldMarked(FIELD_step_expire_time_tag)) sBuilder.append(" `step_expire_time_tag` = '").append(step_expire_time_tag).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_quest` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`quest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`quest_step` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务步骤',"
                + "`step_expire_time_tag` int(11) NOT NULL DEFAULT '0' COMMENT '步骤截至时间戳（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家任务数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//quest_id
        _size+=8;//quest_step
        _size+=4;//step_expire_time_tag
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(quest_id);
        buff.putLong(quest_step);
        buff.putInt(step_expire_time_tag);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        quest_id=buff.getLong();
        quest_step=buff.getLong();
        step_expire_time_tag=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
