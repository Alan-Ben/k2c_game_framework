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
public class ActivityRankBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "活动实例ID")
    private long instance_id;

    public static final int FIELD_rank_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_instance_id", comment = "排行榜实例ID")
    private long rank_instance_id;

    public static final int FIELD_rank_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_id", comment = "排行榜配置ID")
    private long rank_id;

    public static final int FIELD_can_draw_reward =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "can_draw_reward", comment = "是否可以领取奖励")
    private boolean can_draw_reward;

    public static final int FIELD_had_deal_first =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_deal_first", comment = "是否已处理第一名")
    private boolean had_deal_first;

    public static final int FIELD_rewarded_cid_set =5;
    @DataBaseField(type = "blob", fieldname = "rewarded_cid_set", comment = "已领取奖励的玩家CID集合")
    private byte[] rewarded_cid_set;

    public ActivityRankBO() {
        id = 0;
        instance_id = 0L;
        rank_instance_id = 0L;
        rank_id = 0L;
        can_draw_reward = false;
        had_deal_first = false;
        rewarded_cid_set = null;
    }

    public ActivityRankBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instance_id = rs.getLong(2);
        rank_instance_id = rs.getLong(3);
        rank_id = rs.getLong(4);
        can_draw_reward = rs.getBoolean(5);
        had_deal_first = rs.getBoolean(6);
        rewarded_cid_set = rs.getBytes(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityRankBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instance_id`, `rank_instance_id`, `rank_id`, `can_draw_reward`, `had_deal_first`, `rewarded_cid_set`";
    }

    @Override
    public String getTableName() {
        return "`activity_rank`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(rank_instance_id).append("', ");
        strBuf.append("'").append(rank_id).append("', ");
        strBuf.append("'").append(can_draw_reward ? 1 : 0).append("', ");
        strBuf.append("'").append(had_deal_first ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(rewarded_cid_set);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_rewarded_cid_set)) ret.add(rewarded_cid_set);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 活动实例ID
    public long getInstanceId() { return this.instance_id; }
    public void setInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id; 
        markField(_bm, FIELD_instance_id); 
    }
    public void saveInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id;
        saveField(_bm, "instance_id", instance_id);
    }

    // 排行榜实例ID
    public long getRankInstanceId() { return this.rank_instance_id; }
    public void setRankInstanceId(BM _bm, long rank_instance_id) {
        if(rank_instance_id==this.rank_instance_id) 
            return;
        this.rank_instance_id = rank_instance_id; 
        markField(_bm, FIELD_rank_instance_id); 
    }
    public void saveRankInstanceId(BM _bm, long rank_instance_id) {
        if(rank_instance_id==this.rank_instance_id) 
            return;
        this.rank_instance_id = rank_instance_id;
        saveField(_bm, "rank_instance_id", rank_instance_id);
    }

    // 排行榜配置ID
    public long getRankId() { return this.rank_id; }
    public void setRankId(BM _bm, long rank_id) {
        if(rank_id==this.rank_id) 
            return;
        this.rank_id = rank_id; 
        markField(_bm, FIELD_rank_id); 
    }
    public void saveRankId(BM _bm, long rank_id) {
        if(rank_id==this.rank_id) 
            return;
        this.rank_id = rank_id;
        saveField(_bm, "rank_id", rank_id);
    }

    // 是否可以领取奖励
    public boolean getCanDrawReward() { return this.can_draw_reward; }
    public void setCanDrawReward(BM _bm, boolean can_draw_reward) {
        if(can_draw_reward==this.can_draw_reward) 
            return;
        this.can_draw_reward = can_draw_reward; 
        markField(_bm, FIELD_can_draw_reward); 
    }
    public void saveCanDrawReward(BM _bm, boolean can_draw_reward) {
        if(can_draw_reward==this.can_draw_reward) 
            return;
        this.can_draw_reward = can_draw_reward;
        saveField(_bm, "can_draw_reward", can_draw_reward ? 1 : 0);
    }

    // 是否已处理第一名
    public boolean getHadDealFirst() { return this.had_deal_first; }
    public void setHadDealFirst(BM _bm, boolean had_deal_first) {
        if(had_deal_first==this.had_deal_first) 
            return;
        this.had_deal_first = had_deal_first; 
        markField(_bm, FIELD_had_deal_first); 
    }
    public void saveHadDealFirst(BM _bm, boolean had_deal_first) {
        if(had_deal_first==this.had_deal_first) 
            return;
        this.had_deal_first = had_deal_first;
        saveField(_bm, "had_deal_first", had_deal_first ? 1 : 0);
    }

    // 已领取奖励的玩家CID集合
    public byte[] getRewardedCidSet() { return this.rewarded_cid_set; }
    public void setRewardedCidSet(BM _bm, byte[] rewarded_cid_set) {
        if(rewarded_cid_set==this.rewarded_cid_set) 
            return;
        this.rewarded_cid_set = rewarded_cid_set; 
        markField(_bm, FIELD_rewarded_cid_set); 
    }
    public void saveRewardedCidSet(BM _bm, byte[] rewarded_cid_set) {
        if(rewarded_cid_set==this.rewarded_cid_set) 
            return;
        this.rewarded_cid_set = rewarded_cid_set;
        saveFieldBytes(_bm, "rewarded_cid_set", rewarded_cid_set);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `rank_instance_id` = '").append(rank_instance_id).append("',");
        sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        sBuilder.append(" `can_draw_reward` = '").append(can_draw_reward ? 1 : 0).append("',");
        sBuilder.append(" `had_deal_first` = '").append(had_deal_first ? 1 : 0).append("',");
        sBuilder.append(" `rewarded_cid_set` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_rank_instance_id)) sBuilder.append(" `rank_instance_id` = '").append(rank_instance_id).append("',");
        if(isFieldMarked(FIELD_rank_id)) sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        if(isFieldMarked(FIELD_can_draw_reward)) sBuilder.append(" `can_draw_reward` = '").append(can_draw_reward ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_deal_first)) sBuilder.append(" `had_deal_first` = '").append(had_deal_first ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_rewarded_cid_set)) sBuilder.append(" `rewarded_cid_set` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_rank` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`rank_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜实例ID',"
                + "`rank_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜配置ID',"
                + "`can_draw_reward` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否可以领取奖励',"
                + "`had_deal_first` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已处理第一名',"
                + "`rewarded_cid_set` blob NULL COMMENT '已领取奖励的玩家CID集合',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排行榜数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instance_id
        _size+=8;//rank_instance_id
        _size+=8;//rank_id
        _size+=1;//can_draw_reward
        _size+=1;//had_deal_first
        _size+=2;_size+=rewarded_cid_set.length;//rewarded_cid_set
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instance_id);
        buff.putLong(rank_instance_id);
        buff.putLong(rank_id);
        buff.put((byte)(can_draw_reward?1:0));
        buff.put((byte)(had_deal_first?1:0));
        buff.putShort((short)(rewarded_cid_set == null ? 0 : rewarded_cid_set.length));if(null != rewarded_cid_set){buff.put(rewarded_cid_set);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instance_id=buff.getLong();
        rank_instance_id=buff.getLong();
        rank_id=buff.getLong();
        can_draw_reward=(buff.get()==1);
        had_deal_first=(buff.get()==1);
        int rewarded_cid_set_count = buff.getShort();if(rewarded_cid_set_count>0){rewarded_cid_set = new byte[rewarded_cid_set_count];buff.get(rewarded_cid_set);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
