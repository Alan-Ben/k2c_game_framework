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
public class UsRankGiftPackBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_ui_res_path_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "ui_res_path_id", comment = "礼包页面加载资源id")
    private long ui_res_path_id;

    public static final int FIELD_sale =1;
    @DataBaseField(type = "int(11)", fieldname = "sale", comment = "折扣(万分比)")
    private int sale;

    public static final int FIELD_buy_limit =2;
    @DataBaseField(type = "int(11)", fieldname = "buy_limit", comment = "购买次数限制")
    private int buy_limit;

    public static final int FIELD_name =3;
    @DataBaseField(type = "varchar(200)", fieldname = "name", comment = "礼包名称")
    private String name;

    public static final int FIELD_cost =4;
    @DataBaseField(type = "varchar(200)", fieldname = "cost", comment = "消耗道具")
    private String cost;

    public static final int FIELD_ori_cost =5;
    @DataBaseField(type = "varchar(200)", fieldname = "ori_cost", comment = "原价消耗道具")
    private String ori_cost;

    public static final int FIELD_reward_item_list =6;
    @DataBaseField(type = "varchar(2000)", fieldname = "reward_item_list", comment = "奖励列表")
    private String reward_item_list;

    public static final int FIELD_end_time_ms =7;
    @DataBaseField(type = "bigint(20)", fieldname = "end_time_ms", comment = "截止时间（时间戳毫秒）")
    private long end_time_ms;

    public static final int FIELD_activate_time_ms =8;
    @DataBaseField(type = "bigint(20)", fieldname = "activate_time_ms", comment = "激活时间（时间戳毫秒，>0表示已激活）")
    private long activate_time_ms;

    public UsRankGiftPackBO() {
        id = 0;
        ui_res_path_id = 0L;
        sale = 0;
        buy_limit = 0;
        name = "";
        cost = "";
        ori_cost = "";
        reward_item_list = "";
        end_time_ms = 0L;
        activate_time_ms = 0L;
    }

    public UsRankGiftPackBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        ui_res_path_id = rs.getLong(2);
        sale = rs.getInt(3);
        buy_limit = rs.getInt(4);
        name = rs.getString(5);
        cost = rs.getString(6);
        ori_cost = rs.getString(7);
        reward_item_list = rs.getString(8);
        end_time_ms = rs.getLong(9);
        activate_time_ms = rs.getLong(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsRankGiftPackBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `ui_res_path_id`, `sale`, `buy_limit`, `name`, `cost`, `ori_cost`, `reward_item_list`, `end_time_ms`, `activate_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`us_rank_gift_pack`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(ui_res_path_id).append("', ");
        strBuf.append("'").append(sale).append("', ");
        strBuf.append("'").append(buy_limit).append("', ");
        strBuf.append("'").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cost == null ? null : cost.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(ori_cost == null ? null : ori_cost.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(reward_item_list == null ? null : reward_item_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(end_time_ms).append("', ");
        strBuf.append("'").append(activate_time_ms).append("', ");
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

    // 礼包页面加载资源id
    public long getUiResPathId() { return this.ui_res_path_id; }
    public void setUiResPathId(BM _bm, long ui_res_path_id) {
        if(ui_res_path_id==this.ui_res_path_id) 
            return;
        this.ui_res_path_id = ui_res_path_id; 
        markField(_bm, FIELD_ui_res_path_id); 
    }
    public void saveUiResPathId(BM _bm, long ui_res_path_id) {
        if(ui_res_path_id==this.ui_res_path_id) 
            return;
        this.ui_res_path_id = ui_res_path_id;
        saveField(_bm, "ui_res_path_id", ui_res_path_id);
    }

    // 折扣(万分比)
    public int getSale() { return this.sale; }
    public void setSale(BM _bm, int sale) {
        if(sale==this.sale) 
            return;
        this.sale = sale; 
        markField(_bm, FIELD_sale); 
    }
    public void saveSale(BM _bm, int sale) {
        if(sale==this.sale) 
            return;
        this.sale = sale;
        saveField(_bm, "sale", sale);
    }

    // 购买次数限制
    public int getBuyLimit() { return this.buy_limit; }
    public void setBuyLimit(BM _bm, int buy_limit) {
        if(buy_limit==this.buy_limit) 
            return;
        this.buy_limit = buy_limit; 
        markField(_bm, FIELD_buy_limit); 
    }
    public void saveBuyLimit(BM _bm, int buy_limit) {
        if(buy_limit==this.buy_limit) 
            return;
        this.buy_limit = buy_limit;
        saveField(_bm, "buy_limit", buy_limit);
    }

    // 礼包名称
    public String getName() { return this.name; }
    public void setName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name; 
        markField(_bm, FIELD_name); 
    }
    public void saveName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name;
        saveField(_bm, "name", name);
    }

    // 消耗道具
    public String getCost() { return this.cost; }
    public void setCost(BM _bm, String cost) {
        if(cost.equals(this.cost)) 
            return;
        this.cost = cost; 
        markField(_bm, FIELD_cost); 
    }
    public void saveCost(BM _bm, String cost) {
        if(cost.equals(this.cost)) 
            return;
        this.cost = cost;
        saveField(_bm, "cost", cost);
    }

    // 原价消耗道具
    public String getOriCost() { return this.ori_cost; }
    public void setOriCost(BM _bm, String ori_cost) {
        if(ori_cost.equals(this.ori_cost)) 
            return;
        this.ori_cost = ori_cost; 
        markField(_bm, FIELD_ori_cost); 
    }
    public void saveOriCost(BM _bm, String ori_cost) {
        if(ori_cost.equals(this.ori_cost)) 
            return;
        this.ori_cost = ori_cost;
        saveField(_bm, "ori_cost", ori_cost);
    }

    // 奖励列表
    public String getRewardItemList() { return this.reward_item_list; }
    public void setRewardItemList(BM _bm, String reward_item_list) {
        if(reward_item_list.equals(this.reward_item_list)) 
            return;
        this.reward_item_list = reward_item_list; 
        markField(_bm, FIELD_reward_item_list); 
    }
    public void saveRewardItemList(BM _bm, String reward_item_list) {
        if(reward_item_list.equals(this.reward_item_list)) 
            return;
        this.reward_item_list = reward_item_list;
        saveField(_bm, "reward_item_list", reward_item_list);
    }

    // 截止时间（时间戳毫秒）
    public long getEndTimeMs() { return this.end_time_ms; }
    public void setEndTimeMs(BM _bm, long end_time_ms) {
        if(end_time_ms==this.end_time_ms) 
            return;
        this.end_time_ms = end_time_ms; 
        markField(_bm, FIELD_end_time_ms); 
    }
    public void saveEndTimeMs(BM _bm, long end_time_ms) {
        if(end_time_ms==this.end_time_ms) 
            return;
        this.end_time_ms = end_time_ms;
        saveField(_bm, "end_time_ms", end_time_ms);
    }

    // 激活时间（时间戳毫秒，>0表示已激活）
    public long getActivateTimeMs() { return this.activate_time_ms; }
    public void setActivateTimeMs(BM _bm, long activate_time_ms) {
        if(activate_time_ms==this.activate_time_ms) 
            return;
        this.activate_time_ms = activate_time_ms; 
        markField(_bm, FIELD_activate_time_ms); 
    }
    public void saveActivateTimeMs(BM _bm, long activate_time_ms) {
        if(activate_time_ms==this.activate_time_ms) 
            return;
        this.activate_time_ms = activate_time_ms;
        saveField(_bm, "activate_time_ms", activate_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `ui_res_path_id` = '").append(ui_res_path_id).append("',");
        sBuilder.append(" `sale` = '").append(sale).append("',");
        sBuilder.append(" `buy_limit` = '").append(buy_limit).append("',");
        sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cost` = '").append(cost == null ? null : cost.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `ori_cost` = '").append(ori_cost == null ? null : ori_cost.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `reward_item_list` = '").append(reward_item_list == null ? null : reward_item_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        sBuilder.append(" `activate_time_ms` = '").append(activate_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_ui_res_path_id)) sBuilder.append(" `ui_res_path_id` = '").append(ui_res_path_id).append("',");
        if(isFieldMarked(FIELD_sale)) sBuilder.append(" `sale` = '").append(sale).append("',");
        if(isFieldMarked(FIELD_buy_limit)) sBuilder.append(" `buy_limit` = '").append(buy_limit).append("',");
        if(isFieldMarked(FIELD_name)) sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cost)) sBuilder.append(" `cost` = '").append(cost == null ? null : cost.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_ori_cost)) sBuilder.append(" `ori_cost` = '").append(ori_cost == null ? null : ori_cost.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_reward_item_list)) sBuilder.append(" `reward_item_list` = '").append(reward_item_list == null ? null : reward_item_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_end_time_ms)) sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        if(isFieldMarked(FIELD_activate_time_ms)) sBuilder.append(" `activate_time_ms` = '").append(activate_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_rank_gift_pack` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`ui_res_path_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包页面加载资源id',"
                + "`sale` int(11) NOT NULL DEFAULT '0' COMMENT '折扣(万分比)',"
                + "`buy_limit` int(11) NOT NULL DEFAULT '0' COMMENT '购买次数限制',"
                + "`name` varchar(200) NOT NULL DEFAULT '' COMMENT '礼包名称',"
                + "`cost` varchar(200) NOT NULL DEFAULT '' COMMENT '消耗道具',"
                + "`ori_cost` varchar(200) NOT NULL DEFAULT '' COMMENT '原价消耗道具',"
                + "`reward_item_list` varchar(2000) NOT NULL DEFAULT '' COMMENT '奖励列表',"
                + "`end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '截止时间（时间戳毫秒）',"
                + "`activate_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '激活时间（时间戳毫秒，>0表示已激活）',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='冲榜礼包服务器数据（缓存配表字段+激活状态）' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//ui_res_path_id
        _size+=4;//sale
        _size+=4;//buy_limit
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);//name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cost);//cost
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ori_cost);//ori_cost
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward_item_list);//reward_item_list
        _size+=8;//end_time_ms
        _size+=8;//activate_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(ui_res_path_id);
        buff.putInt(sale);
        buff.putInt(buy_limit);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, cost);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ori_cost);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, reward_item_list);
        buff.putLong(end_time_ms);
        buff.putLong(activate_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        ui_res_path_id=buff.getLong();
        sale=buff.getInt();
        buy_limit=buff.getInt();
        name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cost=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        ori_cost=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        reward_item_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        end_time_ms=buff.getLong();
        activate_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
