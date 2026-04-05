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
public class GachaPublicRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_pool_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "pool_id", comment = "卡池id")
    private long pool_id;

    public static final int FIELD_item_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "item_id", comment = "奖品id")
    private long item_id;

    public static final int FIELD_player_name =2;
    @DataBaseField(type = "varchar(128)", fieldname = "player_name", comment = "玩家名字")
    private String player_name;

    public GachaPublicRecordBO() {
        id = 0;
        pool_id = 0L;
        item_id = 0L;
        player_name = "";
    }

    public GachaPublicRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        pool_id = rs.getLong(2);
        item_id = rs.getLong(3);
        player_name = rs.getString(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GachaPublicRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `pool_id`, `item_id`, `player_name`";
    }

    @Override
    public String getTableName() {
        return "`gacha_public_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(pool_id).append("', ");
        strBuf.append("'").append(item_id).append("', ");
        strBuf.append("'").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 卡池id
    public long getPoolId() { return this.pool_id; }
    public void setPoolId(BM _bm, long pool_id) {
        if(pool_id==this.pool_id) 
            return;
        this.pool_id = pool_id; 
        markField(_bm, FIELD_pool_id); 
    }
    public void savePoolId(BM _bm, long pool_id) {
        if(pool_id==this.pool_id) 
            return;
        this.pool_id = pool_id;
        saveField(_bm, "pool_id", pool_id);
    }

    // 奖品id
    public long getItemId() { return this.item_id; }
    public void setItemId(BM _bm, long item_id) {
        if(item_id==this.item_id) 
            return;
        this.item_id = item_id; 
        markField(_bm, FIELD_item_id); 
    }
    public void saveItemId(BM _bm, long item_id) {
        if(item_id==this.item_id) 
            return;
        this.item_id = item_id;
        saveField(_bm, "item_id", item_id);
    }

    // 玩家名字
    public String getPlayerName() { return this.player_name; }
    public void setPlayerName(BM _bm, String player_name) {
        if(player_name.equals(this.player_name)) 
            return;
        this.player_name = player_name; 
        markField(_bm, FIELD_player_name); 
    }
    public void savePlayerName(BM _bm, String player_name) {
        if(player_name.equals(this.player_name)) 
            return;
        this.player_name = player_name;
        saveField(_bm, "player_name", player_name);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `pool_id` = '").append(pool_id).append("',");
        sBuilder.append(" `item_id` = '").append(item_id).append("',");
        sBuilder.append(" `player_name` = '").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_pool_id)) sBuilder.append(" `pool_id` = '").append(pool_id).append("',");
        if(isFieldMarked(FIELD_item_id)) sBuilder.append(" `item_id` = '").append(item_id).append("',");
        if(isFieldMarked(FIELD_player_name)) sBuilder.append(" `player_name` = '").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `gacha_public_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`pool_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '卡池id',"
                + "`item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奖品id',"
                + "`player_name` varchar(128) NOT NULL DEFAULT '' COMMENT '玩家名字',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='抽卡公屏记录信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//pool_id
        _size+=8;//item_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(player_name);//player_name
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(pool_id);
        buff.putLong(item_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, player_name);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        pool_id=buff.getLong();
        item_id=buff.getLong();
        player_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
