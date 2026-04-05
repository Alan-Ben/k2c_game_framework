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
public class PlayerQuestCountBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_quest_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_id", comment = "任务ID")
    private long quest_id;

    public static final int FIELD_done_count =2;
    @DataBaseField(type = "int(11)", fieldname = "done_count", comment = "完成次数")
    private int done_count;

    public PlayerQuestCountBO() {
        id = 0;
        cid = 0L;
        quest_id = 0L;
        done_count = 0;
    }

    public PlayerQuestCountBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        quest_id = rs.getLong(3);
        done_count = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerQuestCountBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `quest_id`, `done_count`";
    }

    @Override
    public String getTableName() {
        return "`player_quest_count`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(quest_id).append("', ");
        strBuf.append("'").append(done_count).append("', ");
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

    // 完成次数
    public int getDoneCount() { return this.done_count; }
    public void setDoneCount(BM _bm, int done_count) {
        if(done_count==this.done_count) 
            return;
        this.done_count = done_count; 
        markField(_bm, FIELD_done_count); 
    }
    public void saveDoneCount(BM _bm, int done_count) {
        if(done_count==this.done_count) 
            return;
        this.done_count = done_count;
        saveField(_bm, "done_count", done_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        sBuilder.append(" `done_count` = '").append(done_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_quest_id)) sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        if(isFieldMarked(FIELD_done_count)) sBuilder.append(" `done_count` = '").append(done_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_quest_count` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`quest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`done_count` int(11) NOT NULL DEFAULT '0' COMMENT '完成次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家任务完成记录数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//done_count
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
        buff.putInt(done_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        quest_id=buff.getLong();
        done_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
