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
public class PlayerForeverAddBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_itemId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "itemId", comment = "配置ID")
    private long itemId;

    public static final int FIELD_itemCount =2;
    @DataBaseField(type = "int(11)", fieldname = "itemCount", comment = "物品数量")
    private int itemCount;

    public PlayerForeverAddBO() {
        id = 0;
        cid = 0L;
        itemId = 0L;
        itemCount = 0;
    }

    public PlayerForeverAddBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        itemId = rs.getLong(3);
        itemCount = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerForeverAddBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `itemId`, `itemCount`";
    }

    @Override
    public String getTableName() {
        return "`player_forever_add`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(itemId).append("', ");
        strBuf.append("'").append(itemCount).append("', ");
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

    // 玩家账号ID
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

    // 配置ID
    public long getItemId() { return this.itemId; }
    public void setItemId(BM _bm, long itemId) {
        if(itemId==this.itemId) 
            return;
        this.itemId = itemId; 
        markField(_bm, FIELD_itemId); 
    }
    public void saveItemId(BM _bm, long itemId) {
        if(itemId==this.itemId) 
            return;
        this.itemId = itemId;
        saveField(_bm, "itemId", itemId);
    }

    // 物品数量
    public int getItemCount() { return this.itemCount; }
    public void setItemCount(BM _bm, int itemCount) {
        if(itemCount==this.itemCount) 
            return;
        this.itemCount = itemCount; 
        markField(_bm, FIELD_itemCount); 
    }
    public void saveItemCount(BM _bm, int itemCount) {
        if(itemCount==this.itemCount) 
            return;
        this.itemCount = itemCount;
        saveField(_bm, "itemCount", itemCount);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `itemId` = '").append(itemId).append("',");
        sBuilder.append(" `itemCount` = '").append(itemCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_itemId)) sBuilder.append(" `itemId` = '").append(itemId).append("',");
        if(isFieldMarked(FIELD_itemCount)) sBuilder.append(" `itemCount` = '").append(itemCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_forever_add` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`itemId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`itemCount` int(11) NOT NULL DEFAULT '0' COMMENT '物品数量',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家永久加成数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//itemId
        _size+=4;//itemCount
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(itemId);
        buff.putInt(itemCount);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        itemId=buff.getLong();
        itemCount=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
