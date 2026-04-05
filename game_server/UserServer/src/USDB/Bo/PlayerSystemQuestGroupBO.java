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
public class PlayerSystemQuestGroupBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_group_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "组id")
    private long group_id;

    public static final int FIELD_step =2;
    @DataBaseField(type = "int(11)", fieldname = "step", comment = "阶段")
    private int step;

    public static final int FIELD_count =3;
    @DataBaseField(type = "bigint(20)", fieldname = "count", comment = "计数")
    private long count;

    public PlayerSystemQuestGroupBO() {
        id = 0;
        cid = 0L;
        group_id = 0L;
        step = 0;
        count = 0L;
    }

    public PlayerSystemQuestGroupBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        group_id = rs.getLong(3);
        step = rs.getInt(4);
        count = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerSystemQuestGroupBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `group_id`, `step`, `count`";
    }

    @Override
    public String getTableName() {
        return "`player_system_quest_group`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(count).append("', ");
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

    // 组id
    public long getGroupId() { return this.group_id; }
    public void setGroupId(BM _bm, long group_id) {
        if(group_id==this.group_id) 
            return;
        this.group_id = group_id; 
        markField(_bm, FIELD_group_id); 
    }
    public void saveGroupId(BM _bm, long group_id) {
        if(group_id==this.group_id) 
            return;
        this.group_id = group_id;
        saveField(_bm, "group_id", group_id);
    }

    // 阶段
    public int getStep() { return this.step; }
    public void setStep(BM _bm, int step) {
        if(step==this.step) 
            return;
        this.step = step; 
        markField(_bm, FIELD_step); 
    }
    public void saveStep(BM _bm, int step) {
        if(step==this.step) 
            return;
        this.step = step;
        saveField(_bm, "step", step);
    }

    // 计数
    public long getCount() { return this.count; }
    public void setCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_system_quest_group` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '组id',"
                + "`step` int(11) NOT NULL DEFAULT '0' COMMENT '阶段',"
                + "`count` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家系统任务组数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//group_id
        _size+=4;//step
        _size+=8;//count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(group_id);
        buff.putInt(step);
        buff.putLong(count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        group_id=buff.getLong();
        step=buff.getInt();
        count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
