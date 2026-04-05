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
public class PlayerCollegeSeatBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_index =1;
    @DataBaseField(type = "int(11)", fieldname = "index", comment = "位置索引")
    private int index;

    public static final int FIELD_hero_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_id", comment = "大臣id")
    private long hero_id;

    public static final int FIELD_start_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "start_time_ms", comment = "开始学习时间")
    private long start_time_ms;

    public PlayerCollegeSeatBO() {
        id = 0;
        cid = 0L;
        index = 0;
        hero_id = 0L;
        start_time_ms = 0L;
    }

    public PlayerCollegeSeatBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        index = rs.getInt(3);
        hero_id = rs.getLong(4);
        start_time_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerCollegeSeatBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `index`, `hero_id`, `start_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_college_seat`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(index).append("', ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(start_time_ms).append("', ");
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

    // 位置索引
    public int getIndex() { return this.index; }
    public void setIndex(BM _bm, int index) {
        if(index==this.index) 
            return;
        this.index = index; 
        markField(_bm, FIELD_index); 
    }
    public void saveIndex(BM _bm, int index) {
        if(index==this.index) 
            return;
        this.index = index;
        saveField(_bm, "index", index);
    }

    // 大臣id
    public long getHeroId() { return this.hero_id; }
    public void setHeroId(BM _bm, long hero_id) {
        if(hero_id==this.hero_id) 
            return;
        this.hero_id = hero_id; 
        markField(_bm, FIELD_hero_id); 
    }
    public void saveHeroId(BM _bm, long hero_id) {
        if(hero_id==this.hero_id) 
            return;
        this.hero_id = hero_id;
        saveField(_bm, "hero_id", hero_id);
    }

    // 开始学习时间
    public long getStartTimeMs() { return this.start_time_ms; }
    public void setStartTimeMs(BM _bm, long start_time_ms) {
        if(start_time_ms==this.start_time_ms) 
            return;
        this.start_time_ms = start_time_ms; 
        markField(_bm, FIELD_start_time_ms); 
    }
    public void saveStartTimeMs(BM _bm, long start_time_ms) {
        if(start_time_ms==this.start_time_ms) 
            return;
        this.start_time_ms = start_time_ms;
        saveField(_bm, "start_time_ms", start_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `index` = '").append(index).append("',");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_index)) sBuilder.append(" `index` = '").append(index).append("',");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_start_time_ms)) sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_college_seat` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`index` int(11) NOT NULL DEFAULT '0' COMMENT '位置索引',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣id',"
                + "`start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始学习时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大学座位数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//index
        _size+=8;//hero_id
        _size+=8;//start_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(index);
        buff.putLong(hero_id);
        buff.putLong(start_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        index=buff.getInt();
        hero_id=buff.getLong();
        start_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
