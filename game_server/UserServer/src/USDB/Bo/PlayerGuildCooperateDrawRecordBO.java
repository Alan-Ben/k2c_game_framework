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
public class PlayerGuildCooperateDrawRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_area_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "area_id", comment = "区域id")
    private long area_id;

    public static final int FIELD_reward_point_index =2;
    @DataBaseField(type = "int(11)", fieldname = "reward_point_index", comment = "奖励据点索引")
    private int reward_point_index;

    public PlayerGuildCooperateDrawRecordBO() {
        id = 0;
        cid = 0L;
        area_id = 0L;
        reward_point_index = 0;
    }

    public PlayerGuildCooperateDrawRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        area_id = rs.getLong(3);
        reward_point_index = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGuildCooperateDrawRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `area_id`, `reward_point_index`";
    }

    @Override
    public String getTableName() {
        return "`player_guild_cooperate_draw_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(area_id).append("', ");
        strBuf.append("'").append(reward_point_index).append("', ");
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

    // 区域id
    public long getAreaId() { return this.area_id; }
    public void setAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id; 
        markField(_bm, FIELD_area_id); 
    }
    public void saveAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id;
        saveField(_bm, "area_id", area_id);
    }

    // 奖励据点索引
    public int getRewardPointIndex() { return this.reward_point_index; }
    public void setRewardPointIndex(BM _bm, int reward_point_index) {
        if(reward_point_index==this.reward_point_index) 
            return;
        this.reward_point_index = reward_point_index; 
        markField(_bm, FIELD_reward_point_index); 
    }
    public void saveRewardPointIndex(BM _bm, int reward_point_index) {
        if(reward_point_index==this.reward_point_index) 
            return;
        this.reward_point_index = reward_point_index;
        saveField(_bm, "reward_point_index", reward_point_index);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `area_id` = '").append(area_id).append("',");
        sBuilder.append(" `reward_point_index` = '").append(reward_point_index).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_area_id)) sBuilder.append(" `area_id` = '").append(area_id).append("',");
        if(isFieldMarked(FIELD_reward_point_index)) sBuilder.append(" `reward_point_index` = '").append(reward_point_index).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_guild_cooperate_draw_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`area_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '区域id',"
                + "`reward_point_index` int(11) NOT NULL DEFAULT '0' COMMENT '奖励据点索引',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家联盟协作奖励据点领取记录数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//area_id
        _size+=4;//reward_point_index
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(area_id);
        buff.putInt(reward_point_index);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        area_id=buff.getLong();
        reward_point_index=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
