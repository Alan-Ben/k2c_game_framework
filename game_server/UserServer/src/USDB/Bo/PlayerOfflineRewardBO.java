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
public class PlayerOfflineRewardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_rewardType =1;
    @DataBaseField(type = "int(11)", fieldname = "rewardType", comment = "类型ID")
    private int rewardType;

    public static final int FIELD_offline_data =2;
    @DataBaseField(type = "blob", fieldname = "offline_data", comment = "离线数据")
    private byte[] offline_data;

    public static final int FIELD_item_list =3;
    @DataBaseField(type = "blob", fieldname = "item_list", comment = "物品列表")
    private byte[] item_list;

    public static final int FIELD_reward_show =4;
    @DataBaseField(type = "blob", fieldname = "reward_show", comment = "奖励展示")
    private byte[] reward_show;

    public static final int FIELD_has_pre_deal =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_pre_deal", comment = "是否已经预处理")
    private boolean has_pre_deal;

    public PlayerOfflineRewardBO() {
        id = 0;
        cid = 0L;
        rewardType = 0;
        offline_data = null;
        item_list = null;
        reward_show = null;
        has_pre_deal = false;
    }

    public PlayerOfflineRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        rewardType = rs.getInt(3);
        offline_data = rs.getBytes(4);
        item_list = rs.getBytes(5);
        reward_show = rs.getBytes(6);
        has_pre_deal = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerOfflineRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `rewardType`, `offline_data`, `item_list`, `reward_show`, `has_pre_deal`";
    }

    @Override
    public String getTableName() {
        return "`player_offline_reward`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(rewardType).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(has_pre_deal ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(offline_data); 
        ret.add(item_list); 
        ret.add(reward_show);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_offline_data)) ret.add(offline_data); 
        if(isFieldMarked(FIELD_item_list)) ret.add(item_list); 
        if(isFieldMarked(FIELD_reward_show)) ret.add(reward_show);         return ret;
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

    // 类型ID
    public int getRewardType() { return this.rewardType; }
    public void setRewardType(BM _bm, int rewardType) {
        if(rewardType==this.rewardType) 
            return;
        this.rewardType = rewardType; 
        markField(_bm, FIELD_rewardType); 
    }
    public void saveRewardType(BM _bm, int rewardType) {
        if(rewardType==this.rewardType) 
            return;
        this.rewardType = rewardType;
        saveField(_bm, "rewardType", rewardType);
    }

    // 离线数据
    public byte[] getOfflineData() { return this.offline_data; }
    public void setOfflineData(BM _bm, byte[] offline_data) {
        if(offline_data==this.offline_data) 
            return;
        this.offline_data = offline_data; 
        markField(_bm, FIELD_offline_data); 
    }
    public void saveOfflineData(BM _bm, byte[] offline_data) {
        if(offline_data==this.offline_data) 
            return;
        this.offline_data = offline_data;
        saveFieldBytes(_bm, "offline_data", offline_data);
    }

    // 物品列表
    public byte[] getItemList() { return this.item_list; }
    public void setItemList(BM _bm, byte[] item_list) {
        if(item_list==this.item_list) 
            return;
        this.item_list = item_list; 
        markField(_bm, FIELD_item_list); 
    }
    public void saveItemList(BM _bm, byte[] item_list) {
        if(item_list==this.item_list) 
            return;
        this.item_list = item_list;
        saveFieldBytes(_bm, "item_list", item_list);
    }

    // 奖励展示
    public byte[] getRewardShow() { return this.reward_show; }
    public void setRewardShow(BM _bm, byte[] reward_show) {
        if(reward_show==this.reward_show) 
            return;
        this.reward_show = reward_show; 
        markField(_bm, FIELD_reward_show); 
    }
    public void saveRewardShow(BM _bm, byte[] reward_show) {
        if(reward_show==this.reward_show) 
            return;
        this.reward_show = reward_show;
        saveFieldBytes(_bm, "reward_show", reward_show);
    }

    // 是否已经预处理
    public boolean getHasPreDeal() { return this.has_pre_deal; }
    public void setHasPreDeal(BM _bm, boolean has_pre_deal) {
        if(has_pre_deal==this.has_pre_deal) 
            return;
        this.has_pre_deal = has_pre_deal; 
        markField(_bm, FIELD_has_pre_deal); 
    }
    public void saveHasPreDeal(BM _bm, boolean has_pre_deal) {
        if(has_pre_deal==this.has_pre_deal) 
            return;
        this.has_pre_deal = has_pre_deal;
        saveField(_bm, "has_pre_deal", has_pre_deal ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `rewardType` = '").append(rewardType).append("',");
        sBuilder.append(" `offline_data` = ?,");
        sBuilder.append(" `item_list` = ?,");
        sBuilder.append(" `reward_show` = ?,");
        sBuilder.append(" `has_pre_deal` = '").append(has_pre_deal ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_rewardType)) sBuilder.append(" `rewardType` = '").append(rewardType).append("',");
        if(isFieldMarked(FIELD_offline_data)) sBuilder.append(" `offline_data` = ?,");
        if(isFieldMarked(FIELD_item_list)) sBuilder.append(" `item_list` = ?,");
        if(isFieldMarked(FIELD_reward_show)) sBuilder.append(" `reward_show` = ?,");
        if(isFieldMarked(FIELD_has_pre_deal)) sBuilder.append(" `has_pre_deal` = '").append(has_pre_deal ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_offline_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`rewardType` int(11) NOT NULL DEFAULT '0' COMMENT '类型ID',"
                + "`offline_data` blob NULL COMMENT '离线数据',"
                + "`item_list` blob NULL COMMENT '物品列表',"
                + "`reward_show` blob NULL COMMENT '奖励展示',"
                + "`has_pre_deal` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已经预处理',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Offline Reward 玩家离线奖励数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//rewardType
        _size+=2;_size+=offline_data.length;//offline_data
        _size+=2;_size+=item_list.length;//item_list
        _size+=2;_size+=reward_show.length;//reward_show
        _size+=1;//has_pre_deal
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(rewardType);
        buff.putShort((short)(offline_data == null ? 0 : offline_data.length));if(null != offline_data){buff.put(offline_data);}
        buff.putShort((short)(item_list == null ? 0 : item_list.length));if(null != item_list){buff.put(item_list);}
        buff.putShort((short)(reward_show == null ? 0 : reward_show.length));if(null != reward_show){buff.put(reward_show);}
        buff.put((byte)(has_pre_deal?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        rewardType=buff.getInt();
        int offline_data_count = buff.getShort();if(offline_data_count>0){offline_data = new byte[offline_data_count];buff.get(offline_data);}
        int item_list_count = buff.getShort();if(item_list_count>0){item_list = new byte[item_list_count];buff.get(item_list);}
        int reward_show_count = buff.getShort();if(reward_show_count>0){reward_show = new byte[reward_show_count];buff.get(reward_show);}
        has_pre_deal=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
