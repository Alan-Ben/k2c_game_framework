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
public class PlayerAchieveBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_achieve_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "achieve_id", comment = "成就ID")
    private long achieve_id;

    public static final int FIELD_counter =2;
    @DataBaseField(type = "bigint(20)", fieldname = "counter", comment = "成就计数")
    private long counter;

    public static final int FIELD_had_draw_step_list =3;
    @DataBaseField(type = "varchar(512)", fieldname = "had_draw_step_list", comment = "已领取步骤列表")
    private String had_draw_step_list;

    public PlayerAchieveBO() {
        id = 0;
        cid = 0L;
        achieve_id = 0L;
        counter = 0L;
        had_draw_step_list = "";
    }

    public PlayerAchieveBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        achieve_id = rs.getLong(3);
        counter = rs.getLong(4);
        had_draw_step_list = rs.getString(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerAchieveBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `achieve_id`, `counter`, `had_draw_step_list`";
    }

    @Override
    public String getTableName() {
        return "`player_achieve`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(achieve_id).append("', ");
        strBuf.append("'").append(counter).append("', ");
        strBuf.append("'").append(had_draw_step_list == null ? null : had_draw_step_list.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 成就ID
    public long getAchieveId() { return this.achieve_id; }
    public void setAchieveId(BM _bm, long achieve_id) {
        if(achieve_id==this.achieve_id) 
            return;
        this.achieve_id = achieve_id; 
        markField(_bm, FIELD_achieve_id); 
    }
    public void saveAchieveId(BM _bm, long achieve_id) {
        if(achieve_id==this.achieve_id) 
            return;
        this.achieve_id = achieve_id;
        saveField(_bm, "achieve_id", achieve_id);
    }

    // 成就计数
    public long getCounter() { return this.counter; }
    public void setCounter(BM _bm, long counter) {
        if(counter==this.counter) 
            return;
        this.counter = counter; 
        markField(_bm, FIELD_counter); 
    }
    public void saveCounter(BM _bm, long counter) {
        if(counter==this.counter) 
            return;
        this.counter = counter;
        saveField(_bm, "counter", counter);
    }

    // 已领取步骤列表
    public String getHadDrawStepList() { return this.had_draw_step_list; }
    public void setHadDrawStepList(BM _bm, String had_draw_step_list) {
        if(had_draw_step_list.equals(this.had_draw_step_list)) 
            return;
        this.had_draw_step_list = had_draw_step_list; 
        markField(_bm, FIELD_had_draw_step_list); 
    }
    public void saveHadDrawStepList(BM _bm, String had_draw_step_list) {
        if(had_draw_step_list.equals(this.had_draw_step_list)) 
            return;
        this.had_draw_step_list = had_draw_step_list;
        saveField(_bm, "had_draw_step_list", had_draw_step_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `achieve_id` = '").append(achieve_id).append("',");
        sBuilder.append(" `counter` = '").append(counter).append("',");
        sBuilder.append(" `had_draw_step_list` = '").append(had_draw_step_list == null ? null : had_draw_step_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_achieve_id)) sBuilder.append(" `achieve_id` = '").append(achieve_id).append("',");
        if(isFieldMarked(FIELD_counter)) sBuilder.append(" `counter` = '").append(counter).append("',");
        if(isFieldMarked(FIELD_had_draw_step_list)) sBuilder.append(" `had_draw_step_list` = '").append(had_draw_step_list == null ? null : had_draw_step_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_achieve` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`achieve_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '成就ID',"
                + "`counter` bigint(20) NOT NULL DEFAULT '0' COMMENT '成就计数',"
                + "`had_draw_step_list` varchar(512) NOT NULL DEFAULT '' COMMENT '已领取步骤列表',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家成就数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//achieve_id
        _size+=8;//counter
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(had_draw_step_list);//had_draw_step_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(achieve_id);
        buff.putLong(counter);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, had_draw_step_list);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        achieve_id=buff.getLong();
        counter=buff.getLong();
        had_draw_step_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
