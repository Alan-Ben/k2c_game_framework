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
public class PlayerQuestTargetBO extends BaseBO {
    
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

    public static final int FIELD_quest_target =3;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_target", comment = "任务目标ID")
    private long quest_target;

    public static final int FIELD_quest_target_count =4;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_target_count", comment = "任务步骤目标计数")
    private long quest_target_count;

    public PlayerQuestTargetBO() {
        id = 0;
        cid = 0L;
        quest_id = 0L;
        quest_step = 0L;
        quest_target = 0L;
        quest_target_count = 0L;
    }

    public PlayerQuestTargetBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        quest_id = rs.getLong(3);
        quest_step = rs.getLong(4);
        quest_target = rs.getLong(5);
        quest_target_count = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerQuestTargetBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `quest_id`, `quest_step`, `quest_target`, `quest_target_count`";
    }

    @Override
    public String getTableName() {
        return "`player_quest_target`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(quest_id).append("', ");
        strBuf.append("'").append(quest_step).append("', ");
        strBuf.append("'").append(quest_target).append("', ");
        strBuf.append("'").append(quest_target_count).append("', ");
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

    // 任务目标ID
    public long getQuestTarget() { return this.quest_target; }
    public void setQuestTarget(BM _bm, long quest_target) {
        if(quest_target==this.quest_target) 
            return;
        this.quest_target = quest_target; 
        markField(_bm, FIELD_quest_target); 
    }
    public void saveQuestTarget(BM _bm, long quest_target) {
        if(quest_target==this.quest_target) 
            return;
        this.quest_target = quest_target;
        saveField(_bm, "quest_target", quest_target);
    }

    // 任务步骤目标计数
    public long getQuestTargetCount() { return this.quest_target_count; }
    public void setQuestTargetCount(BM _bm, long quest_target_count) {
        if(quest_target_count==this.quest_target_count) 
            return;
        this.quest_target_count = quest_target_count; 
        markField(_bm, FIELD_quest_target_count); 
    }
    public void saveQuestTargetCount(BM _bm, long quest_target_count) {
        if(quest_target_count==this.quest_target_count) 
            return;
        this.quest_target_count = quest_target_count;
        saveField(_bm, "quest_target_count", quest_target_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        sBuilder.append(" `quest_step` = '").append(quest_step).append("',");
        sBuilder.append(" `quest_target` = '").append(quest_target).append("',");
        sBuilder.append(" `quest_target_count` = '").append(quest_target_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_quest_id)) sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        if(isFieldMarked(FIELD_quest_step)) sBuilder.append(" `quest_step` = '").append(quest_step).append("',");
        if(isFieldMarked(FIELD_quest_target)) sBuilder.append(" `quest_target` = '").append(quest_target).append("',");
        if(isFieldMarked(FIELD_quest_target_count)) sBuilder.append(" `quest_target_count` = '").append(quest_target_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_quest_target` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`quest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`quest_step` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务步骤',"
                + "`quest_target` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务目标ID',"
                + "`quest_target_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务步骤目标计数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家任务步骤目标数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//quest_target
        _size+=8;//quest_target_count
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
        buff.putLong(quest_target);
        buff.putLong(quest_target_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        quest_id=buff.getLong();
        quest_step=buff.getLong();
        quest_target=buff.getLong();
        quest_target_count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
