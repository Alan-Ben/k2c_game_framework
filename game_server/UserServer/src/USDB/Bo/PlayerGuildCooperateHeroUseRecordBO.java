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
public class PlayerGuildCooperateHeroUseRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_hero_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_id", comment = "大臣id")
    private long hero_id;

    public static final int FIELD_use_count =2;
    @DataBaseField(type = "int(11)", fieldname = "use_count", comment = "使用次数")
    private int use_count;

    public static final int FIELD_recover_count =3;
    @DataBaseField(type = "int(11)", fieldname = "recover_count", comment = "恢复次数")
    private int recover_count;

    public static final int FIELD_last_refresh_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "last_refresh_time_ms", comment = "上次刷新时间")
    private long last_refresh_time_ms;

    public PlayerGuildCooperateHeroUseRecordBO() {
        id = 0;
        cid = 0L;
        hero_id = 0L;
        use_count = 0;
        recover_count = 0;
        last_refresh_time_ms = 0L;
    }

    public PlayerGuildCooperateHeroUseRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        hero_id = rs.getLong(3);
        use_count = rs.getInt(4);
        recover_count = rs.getInt(5);
        last_refresh_time_ms = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGuildCooperateHeroUseRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `hero_id`, `use_count`, `recover_count`, `last_refresh_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_guild_cooperate_hero_use_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(use_count).append("', ");
        strBuf.append("'").append(recover_count).append("', ");
        strBuf.append("'").append(last_refresh_time_ms).append("', ");
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

    // 使用次数
    public int getUseCount() { return this.use_count; }
    public void setUseCount(BM _bm, int use_count) {
        if(use_count==this.use_count) 
            return;
        this.use_count = use_count; 
        markField(_bm, FIELD_use_count); 
    }
    public void saveUseCount(BM _bm, int use_count) {
        if(use_count==this.use_count) 
            return;
        this.use_count = use_count;
        saveField(_bm, "use_count", use_count);
    }

    // 恢复次数
    public int getRecoverCount() { return this.recover_count; }
    public void setRecoverCount(BM _bm, int recover_count) {
        if(recover_count==this.recover_count) 
            return;
        this.recover_count = recover_count; 
        markField(_bm, FIELD_recover_count); 
    }
    public void saveRecoverCount(BM _bm, int recover_count) {
        if(recover_count==this.recover_count) 
            return;
        this.recover_count = recover_count;
        saveField(_bm, "recover_count", recover_count);
    }

    // 上次刷新时间
    public long getLastRefreshTimeMs() { return this.last_refresh_time_ms; }
    public void setLastRefreshTimeMs(BM _bm, long last_refresh_time_ms) {
        if(last_refresh_time_ms==this.last_refresh_time_ms) 
            return;
        this.last_refresh_time_ms = last_refresh_time_ms; 
        markField(_bm, FIELD_last_refresh_time_ms); 
    }
    public void saveLastRefreshTimeMs(BM _bm, long last_refresh_time_ms) {
        if(last_refresh_time_ms==this.last_refresh_time_ms) 
            return;
        this.last_refresh_time_ms = last_refresh_time_ms;
        saveField(_bm, "last_refresh_time_ms", last_refresh_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `use_count` = '").append(use_count).append("',");
        sBuilder.append(" `recover_count` = '").append(recover_count).append("',");
        sBuilder.append(" `last_refresh_time_ms` = '").append(last_refresh_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_use_count)) sBuilder.append(" `use_count` = '").append(use_count).append("',");
        if(isFieldMarked(FIELD_recover_count)) sBuilder.append(" `recover_count` = '").append(recover_count).append("',");
        if(isFieldMarked(FIELD_last_refresh_time_ms)) sBuilder.append(" `last_refresh_time_ms` = '").append(last_refresh_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_guild_cooperate_hero_use_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣id',"
                + "`use_count` int(11) NOT NULL DEFAULT '0' COMMENT '使用次数',"
                + "`recover_count` int(11) NOT NULL DEFAULT '0' COMMENT '恢复次数',"
                + "`last_refresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次刷新时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家联盟协作大臣使用记录数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//hero_id
        _size+=4;//use_count
        _size+=4;//recover_count
        _size+=8;//last_refresh_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(hero_id);
        buff.putInt(use_count);
        buff.putInt(recover_count);
        buff.putLong(last_refresh_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        hero_id=buff.getLong();
        use_count=buff.getInt();
        recover_count=buff.getInt();
        last_refresh_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
