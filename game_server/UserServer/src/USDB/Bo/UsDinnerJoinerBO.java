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
public class UsDinnerJoinerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "宴会实例ID")
    private long instance_id;

    public static final int FIELD_joiner_type =1;
    @DataBaseField(type = "int(11)", fieldname = "joiner_type", comment = "赴宴对象类型")
    private int joiner_type;

    public static final int FIELD_joiner_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "joiner_id", comment = "赴宴对象ID")
    private long joiner_id;

    public static final int FIELD_cost_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "cost_id", comment = "赴宴消耗配置")
    private long cost_id;

    public static final int FIELD_join_ts =4;
    @DataBaseField(type = "int(11)", fieldname = "join_ts", comment = "赴宴时间戳（秒）")
    private int join_ts;

    public static final int FIELD_gain_coin =5;
    @DataBaseField(type = "bigint(20)", fieldname = "gain_coin", comment = "结算的宴会币")
    private long gain_coin;

    public static final int FIELD_gain_score =6;
    @DataBaseField(type = "bigint(20)", fieldname = "gain_score", comment = "结算的宴会人气")
    private long gain_score;

    public UsDinnerJoinerBO() {
        id = 0;
        instance_id = 0L;
        joiner_type = 0;
        joiner_id = 0L;
        cost_id = 0L;
        join_ts = 0;
        gain_coin = 0L;
        gain_score = 0L;
    }

    public UsDinnerJoinerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instance_id = rs.getLong(2);
        joiner_type = rs.getInt(3);
        joiner_id = rs.getLong(4);
        cost_id = rs.getLong(5);
        join_ts = rs.getInt(6);
        gain_coin = rs.getLong(7);
        gain_score = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsDinnerJoinerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instance_id`, `joiner_type`, `joiner_id`, `cost_id`, `join_ts`, `gain_coin`, `gain_score`";
    }

    @Override
    public String getTableName() {
        return "`us_dinner_joiner`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(joiner_type).append("', ");
        strBuf.append("'").append(joiner_id).append("', ");
        strBuf.append("'").append(cost_id).append("', ");
        strBuf.append("'").append(join_ts).append("', ");
        strBuf.append("'").append(gain_coin).append("', ");
        strBuf.append("'").append(gain_score).append("', ");
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

    // 宴会实例ID
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

    // 赴宴对象类型
    public int getJoinerType() { return this.joiner_type; }
    public void setJoinerType(BM _bm, int joiner_type) {
        if(joiner_type==this.joiner_type) 
            return;
        this.joiner_type = joiner_type; 
        markField(_bm, FIELD_joiner_type); 
    }
    public void saveJoinerType(BM _bm, int joiner_type) {
        if(joiner_type==this.joiner_type) 
            return;
        this.joiner_type = joiner_type;
        saveField(_bm, "joiner_type", joiner_type);
    }

    // 赴宴对象ID
    public long getJoinerId() { return this.joiner_id; }
    public void setJoinerId(BM _bm, long joiner_id) {
        if(joiner_id==this.joiner_id) 
            return;
        this.joiner_id = joiner_id; 
        markField(_bm, FIELD_joiner_id); 
    }
    public void saveJoinerId(BM _bm, long joiner_id) {
        if(joiner_id==this.joiner_id) 
            return;
        this.joiner_id = joiner_id;
        saveField(_bm, "joiner_id", joiner_id);
    }

    // 赴宴消耗配置
    public long getCostId() { return this.cost_id; }
    public void setCostId(BM _bm, long cost_id) {
        if(cost_id==this.cost_id) 
            return;
        this.cost_id = cost_id; 
        markField(_bm, FIELD_cost_id); 
    }
    public void saveCostId(BM _bm, long cost_id) {
        if(cost_id==this.cost_id) 
            return;
        this.cost_id = cost_id;
        saveField(_bm, "cost_id", cost_id);
    }

    // 赴宴时间戳（秒）
    public int getJoinTs() { return this.join_ts; }
    public void setJoinTs(BM _bm, int join_ts) {
        if(join_ts==this.join_ts) 
            return;
        this.join_ts = join_ts; 
        markField(_bm, FIELD_join_ts); 
    }
    public void saveJoinTs(BM _bm, int join_ts) {
        if(join_ts==this.join_ts) 
            return;
        this.join_ts = join_ts;
        saveField(_bm, "join_ts", join_ts);
    }

    // 结算的宴会币
    public long getGainCoin() { return this.gain_coin; }
    public void setGainCoin(BM _bm, long gain_coin) {
        if(gain_coin==this.gain_coin) 
            return;
        this.gain_coin = gain_coin; 
        markField(_bm, FIELD_gain_coin); 
    }
    public void saveGainCoin(BM _bm, long gain_coin) {
        if(gain_coin==this.gain_coin) 
            return;
        this.gain_coin = gain_coin;
        saveField(_bm, "gain_coin", gain_coin);
    }

    // 结算的宴会人气
    public long getGainScore() { return this.gain_score; }
    public void setGainScore(BM _bm, long gain_score) {
        if(gain_score==this.gain_score) 
            return;
        this.gain_score = gain_score; 
        markField(_bm, FIELD_gain_score); 
    }
    public void saveGainScore(BM _bm, long gain_score) {
        if(gain_score==this.gain_score) 
            return;
        this.gain_score = gain_score;
        saveField(_bm, "gain_score", gain_score);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `joiner_type` = '").append(joiner_type).append("',");
        sBuilder.append(" `joiner_id` = '").append(joiner_id).append("',");
        sBuilder.append(" `cost_id` = '").append(cost_id).append("',");
        sBuilder.append(" `join_ts` = '").append(join_ts).append("',");
        sBuilder.append(" `gain_coin` = '").append(gain_coin).append("',");
        sBuilder.append(" `gain_score` = '").append(gain_score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_joiner_type)) sBuilder.append(" `joiner_type` = '").append(joiner_type).append("',");
        if(isFieldMarked(FIELD_joiner_id)) sBuilder.append(" `joiner_id` = '").append(joiner_id).append("',");
        if(isFieldMarked(FIELD_cost_id)) sBuilder.append(" `cost_id` = '").append(cost_id).append("',");
        if(isFieldMarked(FIELD_join_ts)) sBuilder.append(" `join_ts` = '").append(join_ts).append("',");
        if(isFieldMarked(FIELD_gain_coin)) sBuilder.append(" `gain_coin` = '").append(gain_coin).append("',");
        if(isFieldMarked(FIELD_gain_score)) sBuilder.append(" `gain_score` = '").append(gain_score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_dinner_joiner` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宴会实例ID',"
                + "`joiner_type` int(11) NOT NULL DEFAULT '0' COMMENT '赴宴对象类型',"
                + "`joiner_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '赴宴对象ID',"
                + "`cost_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '赴宴消耗配置',"
                + "`join_ts` int(11) NOT NULL DEFAULT '0' COMMENT '赴宴时间戳（秒）',"
                + "`gain_coin` bigint(20) NOT NULL DEFAULT '0' COMMENT '结算的宴会币',"
                + "`gain_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '结算的宴会人气',"
                + "KEY `instance_id` (`instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='宴会参与玩家数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//joiner_type
        _size+=8;//joiner_id
        _size+=8;//cost_id
        _size+=4;//join_ts
        _size+=8;//gain_coin
        _size+=8;//gain_score
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instance_id);
        buff.putInt(joiner_type);
        buff.putLong(joiner_id);
        buff.putLong(cost_id);
        buff.putInt(join_ts);
        buff.putLong(gain_coin);
        buff.putLong(gain_score);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instance_id=buff.getLong();
        joiner_type=buff.getInt();
        joiner_id=buff.getLong();
        cost_id=buff.getLong();
        join_ts=buff.getInt();
        gain_coin=buff.getLong();
        gain_score=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
