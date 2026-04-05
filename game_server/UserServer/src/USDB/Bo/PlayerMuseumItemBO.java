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
public class PlayerMuseumItemBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_item_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "item_id", comment = "物品ID")
    private long item_id;

    public static final int FIELD_level =2;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_is_active =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_active", comment = "是否激活")
    private boolean is_active;

    public static final int FIELD_gain_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "gain_time_ms", comment = "获得时间毫秒")
    private long gain_time_ms;

    public PlayerMuseumItemBO() {
        id = 0;
        cid = 0L;
        item_id = 0L;
        level = 0;
        is_active = false;
        gain_time_ms = 0L;
    }

    public PlayerMuseumItemBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        item_id = rs.getLong(3);
        level = rs.getInt(4);
        is_active = rs.getBoolean(5);
        gain_time_ms = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMuseumItemBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `item_id`, `level`, `is_active`, `gain_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_museum_item`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(item_id).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(is_active ? 1 : 0).append("', ");
        strBuf.append("'").append(gain_time_ms).append("', ");
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

    // 物品ID
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

    // 等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 是否激活
    public boolean getIsActive() { return this.is_active; }
    public void setIsActive(BM _bm, boolean is_active) {
        if(is_active==this.is_active) 
            return;
        this.is_active = is_active; 
        markField(_bm, FIELD_is_active); 
    }
    public void saveIsActive(BM _bm, boolean is_active) {
        if(is_active==this.is_active) 
            return;
        this.is_active = is_active;
        saveField(_bm, "is_active", is_active ? 1 : 0);
    }

    // 获得时间毫秒
    public long getGainTimeMs() { return this.gain_time_ms; }
    public void setGainTimeMs(BM _bm, long gain_time_ms) {
        if(gain_time_ms==this.gain_time_ms) 
            return;
        this.gain_time_ms = gain_time_ms; 
        markField(_bm, FIELD_gain_time_ms); 
    }
    public void saveGainTimeMs(BM _bm, long gain_time_ms) {
        if(gain_time_ms==this.gain_time_ms) 
            return;
        this.gain_time_ms = gain_time_ms;
        saveField(_bm, "gain_time_ms", gain_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `item_id` = '").append(item_id).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `is_active` = '").append(is_active ? 1 : 0).append("',");
        sBuilder.append(" `gain_time_ms` = '").append(gain_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_item_id)) sBuilder.append(" `item_id` = '").append(item_id).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_is_active)) sBuilder.append(" `is_active` = '").append(is_active ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_gain_time_ms)) sBuilder.append(" `gain_time_ms` = '").append(gain_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_museum_item` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '物品ID',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`is_active` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否激活',"
                + "`gain_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '获得时间毫秒',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家博物馆 物品数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//item_id
        _size+=4;//level
        _size+=1;//is_active
        _size+=8;//gain_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(item_id);
        buff.putInt(level);
        buff.put((byte)(is_active?1:0));
        buff.putLong(gain_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        item_id=buff.getLong();
        level=buff.getInt();
        is_active=(buff.get()==1);
        gain_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
