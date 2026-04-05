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
public class PlayerActivityFundTaskBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_fund_record_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "fund_record_id", comment = "基金记录ID（player_activity_fund表的主键id）")
    private long fund_record_id;

    public static final int FIELD_task_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "task_id", comment = "任务ID")
    private long task_id;

    public static final int FIELD_current_count =3;
    @DataBaseField(type = "bigint(20)", fieldname = "current_count", comment = "当前计数")
    private long current_count;

    public static final int FIELD_finished_times =4;
    @DataBaseField(type = "int(11)", fieldname = "finished_times", comment = "已完成次数")
    private int finished_times;

    public PlayerActivityFundTaskBO() {
        id = 0;
        cid = 0L;
        fund_record_id = 0L;
        task_id = 0L;
        current_count = 0L;
        finished_times = 0;
    }

    public PlayerActivityFundTaskBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        fund_record_id = rs.getLong(3);
        task_id = rs.getLong(4);
        current_count = rs.getLong(5);
        finished_times = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerActivityFundTaskBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `fund_record_id`, `task_id`, `current_count`, `finished_times`";
    }

    @Override
    public String getTableName() {
        return "`player_activity_fund_task`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(fund_record_id).append("', ");
        strBuf.append("'").append(task_id).append("', ");
        strBuf.append("'").append(current_count).append("', ");
        strBuf.append("'").append(finished_times).append("', ");
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

    // 基金记录ID（player_activity_fund表的主键id）
    public long getFundRecordId() { return this.fund_record_id; }
    public void setFundRecordId(BM _bm, long fund_record_id) {
        if(fund_record_id==this.fund_record_id) 
            return;
        this.fund_record_id = fund_record_id; 
        markField(_bm, FIELD_fund_record_id); 
    }
    public void saveFundRecordId(BM _bm, long fund_record_id) {
        if(fund_record_id==this.fund_record_id) 
            return;
        this.fund_record_id = fund_record_id;
        saveField(_bm, "fund_record_id", fund_record_id);
    }

    // 任务ID
    public long getTaskId() { return this.task_id; }
    public void setTaskId(BM _bm, long task_id) {
        if(task_id==this.task_id) 
            return;
        this.task_id = task_id; 
        markField(_bm, FIELD_task_id); 
    }
    public void saveTaskId(BM _bm, long task_id) {
        if(task_id==this.task_id) 
            return;
        this.task_id = task_id;
        saveField(_bm, "task_id", task_id);
    }

    // 当前计数
    public long getCurrentCount() { return this.current_count; }
    public void setCurrentCount(BM _bm, long current_count) {
        if(current_count==this.current_count) 
            return;
        this.current_count = current_count; 
        markField(_bm, FIELD_current_count); 
    }
    public void saveCurrentCount(BM _bm, long current_count) {
        if(current_count==this.current_count) 
            return;
        this.current_count = current_count;
        saveField(_bm, "current_count", current_count);
    }

    // 已完成次数
    public int getFinishedTimes() { return this.finished_times; }
    public void setFinishedTimes(BM _bm, int finished_times) {
        if(finished_times==this.finished_times) 
            return;
        this.finished_times = finished_times; 
        markField(_bm, FIELD_finished_times); 
    }
    public void saveFinishedTimes(BM _bm, int finished_times) {
        if(finished_times==this.finished_times) 
            return;
        this.finished_times = finished_times;
        saveField(_bm, "finished_times", finished_times);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `fund_record_id` = '").append(fund_record_id).append("',");
        sBuilder.append(" `task_id` = '").append(task_id).append("',");
        sBuilder.append(" `current_count` = '").append(current_count).append("',");
        sBuilder.append(" `finished_times` = '").append(finished_times).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_fund_record_id)) sBuilder.append(" `fund_record_id` = '").append(fund_record_id).append("',");
        if(isFieldMarked(FIELD_task_id)) sBuilder.append(" `task_id` = '").append(task_id).append("',");
        if(isFieldMarked(FIELD_current_count)) sBuilder.append(" `current_count` = '").append(current_count).append("',");
        if(isFieldMarked(FIELD_finished_times)) sBuilder.append(" `finished_times` = '").append(finished_times).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_activity_fund_task` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`fund_record_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '基金记录ID（player_activity_fund表的主键id）',"
                + "`task_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`current_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前计数',"
                + "`finished_times` int(11) NOT NULL DEFAULT '0' COMMENT '已完成次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家活动基金任务数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//fund_record_id
        _size+=8;//task_id
        _size+=8;//current_count
        _size+=4;//finished_times
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(fund_record_id);
        buff.putLong(task_id);
        buff.putLong(current_count);
        buff.putInt(finished_times);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        fund_record_id=buff.getLong();
        task_id=buff.getLong();
        current_count=buff.getLong();
        finished_times=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
