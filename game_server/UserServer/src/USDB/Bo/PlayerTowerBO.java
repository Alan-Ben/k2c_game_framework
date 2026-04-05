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
public class PlayerTowerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_chapter_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "chapter_id", comment = "章节ID")
    private long chapter_id;

    public static final int FIELD_chapter_level =2;
    @DataBaseField(type = "int(11)", fieldname = "chapter_level", comment = "关卡内层数 从1开始")
    private int chapter_level;

    public static final int FIELD_last_draw_tower_coin_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "last_draw_tower_coin_time_ms", comment = "上次领取每日迷宫币的时间戳（毫秒）")
    private long last_draw_tower_coin_time_ms;

    public static final int FIELD_had_active_research_chapter_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "had_active_research_chapter_id", comment = "已激活研究章节ID")
    private long had_active_research_chapter_id;

    public static final int FIELD_had_active_research_chapter_level =5;
    @DataBaseField(type = "int(11)", fieldname = "had_active_research_chapter_level", comment = "已激活研究章节内层数")
    private int had_active_research_chapter_level;

    public static final int FIELD_highest_had_reach_chapter_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "highest_had_reach_chapter_id", comment = "到达最高章节ID")
    private long highest_had_reach_chapter_id;

    public static final int FIELD_highest_had_reach_chapter_level =7;
    @DataBaseField(type = "int(11)", fieldname = "highest_had_reach_chapter_level", comment = "到达最高章节内层数")
    private int highest_had_reach_chapter_level;

    public PlayerTowerBO() {
        id = 0;
        cid = 0L;
        chapter_id = 0L;
        chapter_level = 0;
        last_draw_tower_coin_time_ms = 0L;
        had_active_research_chapter_id = 0L;
        had_active_research_chapter_level = 0;
        highest_had_reach_chapter_id = 0L;
        highest_had_reach_chapter_level = 0;
    }

    public PlayerTowerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        chapter_id = rs.getLong(3);
        chapter_level = rs.getInt(4);
        last_draw_tower_coin_time_ms = rs.getLong(5);
        had_active_research_chapter_id = rs.getLong(6);
        had_active_research_chapter_level = rs.getInt(7);
        highest_had_reach_chapter_id = rs.getLong(8);
        highest_had_reach_chapter_level = rs.getInt(9);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTowerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `chapter_id`, `chapter_level`, `last_draw_tower_coin_time_ms`, `had_active_research_chapter_id`, `had_active_research_chapter_level`, `highest_had_reach_chapter_id`, `highest_had_reach_chapter_level`";
    }

    @Override
    public String getTableName() {
        return "`player_tower`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(chapter_id).append("', ");
        strBuf.append("'").append(chapter_level).append("', ");
        strBuf.append("'").append(last_draw_tower_coin_time_ms).append("', ");
        strBuf.append("'").append(had_active_research_chapter_id).append("', ");
        strBuf.append("'").append(had_active_research_chapter_level).append("', ");
        strBuf.append("'").append(highest_had_reach_chapter_id).append("', ");
        strBuf.append("'").append(highest_had_reach_chapter_level).append("', ");
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

    // 章节ID
    public long getChapterId() { return this.chapter_id; }
    public void setChapterId(BM _bm, long chapter_id) {
        if(chapter_id==this.chapter_id) 
            return;
        this.chapter_id = chapter_id; 
        markField(_bm, FIELD_chapter_id); 
    }
    public void saveChapterId(BM _bm, long chapter_id) {
        if(chapter_id==this.chapter_id) 
            return;
        this.chapter_id = chapter_id;
        saveField(_bm, "chapter_id", chapter_id);
    }

    // 关卡内层数 从1开始
    public int getChapterLevel() { return this.chapter_level; }
    public void setChapterLevel(BM _bm, int chapter_level) {
        if(chapter_level==this.chapter_level) 
            return;
        this.chapter_level = chapter_level; 
        markField(_bm, FIELD_chapter_level); 
    }
    public void saveChapterLevel(BM _bm, int chapter_level) {
        if(chapter_level==this.chapter_level) 
            return;
        this.chapter_level = chapter_level;
        saveField(_bm, "chapter_level", chapter_level);
    }

    // 上次领取每日迷宫币的时间戳（毫秒）
    public long getLastDrawTowerCoinTimeMs() { return this.last_draw_tower_coin_time_ms; }
    public void setLastDrawTowerCoinTimeMs(BM _bm, long last_draw_tower_coin_time_ms) {
        if(last_draw_tower_coin_time_ms==this.last_draw_tower_coin_time_ms) 
            return;
        this.last_draw_tower_coin_time_ms = last_draw_tower_coin_time_ms; 
        markField(_bm, FIELD_last_draw_tower_coin_time_ms); 
    }
    public void saveLastDrawTowerCoinTimeMs(BM _bm, long last_draw_tower_coin_time_ms) {
        if(last_draw_tower_coin_time_ms==this.last_draw_tower_coin_time_ms) 
            return;
        this.last_draw_tower_coin_time_ms = last_draw_tower_coin_time_ms;
        saveField(_bm, "last_draw_tower_coin_time_ms", last_draw_tower_coin_time_ms);
    }

    // 已激活研究章节ID
    public long getHadActiveResearchChapterId() { return this.had_active_research_chapter_id; }
    public void setHadActiveResearchChapterId(BM _bm, long had_active_research_chapter_id) {
        if(had_active_research_chapter_id==this.had_active_research_chapter_id) 
            return;
        this.had_active_research_chapter_id = had_active_research_chapter_id; 
        markField(_bm, FIELD_had_active_research_chapter_id); 
    }
    public void saveHadActiveResearchChapterId(BM _bm, long had_active_research_chapter_id) {
        if(had_active_research_chapter_id==this.had_active_research_chapter_id) 
            return;
        this.had_active_research_chapter_id = had_active_research_chapter_id;
        saveField(_bm, "had_active_research_chapter_id", had_active_research_chapter_id);
    }

    // 已激活研究章节内层数
    public int getHadActiveResearchChapterLevel() { return this.had_active_research_chapter_level; }
    public void setHadActiveResearchChapterLevel(BM _bm, int had_active_research_chapter_level) {
        if(had_active_research_chapter_level==this.had_active_research_chapter_level) 
            return;
        this.had_active_research_chapter_level = had_active_research_chapter_level; 
        markField(_bm, FIELD_had_active_research_chapter_level); 
    }
    public void saveHadActiveResearchChapterLevel(BM _bm, int had_active_research_chapter_level) {
        if(had_active_research_chapter_level==this.had_active_research_chapter_level) 
            return;
        this.had_active_research_chapter_level = had_active_research_chapter_level;
        saveField(_bm, "had_active_research_chapter_level", had_active_research_chapter_level);
    }

    // 到达最高章节ID
    public long getHighestHadReachChapterId() { return this.highest_had_reach_chapter_id; }
    public void setHighestHadReachChapterId(BM _bm, long highest_had_reach_chapter_id) {
        if(highest_had_reach_chapter_id==this.highest_had_reach_chapter_id) 
            return;
        this.highest_had_reach_chapter_id = highest_had_reach_chapter_id; 
        markField(_bm, FIELD_highest_had_reach_chapter_id); 
    }
    public void saveHighestHadReachChapterId(BM _bm, long highest_had_reach_chapter_id) {
        if(highest_had_reach_chapter_id==this.highest_had_reach_chapter_id) 
            return;
        this.highest_had_reach_chapter_id = highest_had_reach_chapter_id;
        saveField(_bm, "highest_had_reach_chapter_id", highest_had_reach_chapter_id);
    }

    // 到达最高章节内层数
    public int getHighestHadReachChapterLevel() { return this.highest_had_reach_chapter_level; }
    public void setHighestHadReachChapterLevel(BM _bm, int highest_had_reach_chapter_level) {
        if(highest_had_reach_chapter_level==this.highest_had_reach_chapter_level) 
            return;
        this.highest_had_reach_chapter_level = highest_had_reach_chapter_level; 
        markField(_bm, FIELD_highest_had_reach_chapter_level); 
    }
    public void saveHighestHadReachChapterLevel(BM _bm, int highest_had_reach_chapter_level) {
        if(highest_had_reach_chapter_level==this.highest_had_reach_chapter_level) 
            return;
        this.highest_had_reach_chapter_level = highest_had_reach_chapter_level;
        saveField(_bm, "highest_had_reach_chapter_level", highest_had_reach_chapter_level);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        sBuilder.append(" `chapter_level` = '").append(chapter_level).append("',");
        sBuilder.append(" `last_draw_tower_coin_time_ms` = '").append(last_draw_tower_coin_time_ms).append("',");
        sBuilder.append(" `had_active_research_chapter_id` = '").append(had_active_research_chapter_id).append("',");
        sBuilder.append(" `had_active_research_chapter_level` = '").append(had_active_research_chapter_level).append("',");
        sBuilder.append(" `highest_had_reach_chapter_id` = '").append(highest_had_reach_chapter_id).append("',");
        sBuilder.append(" `highest_had_reach_chapter_level` = '").append(highest_had_reach_chapter_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_chapter_id)) sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        if(isFieldMarked(FIELD_chapter_level)) sBuilder.append(" `chapter_level` = '").append(chapter_level).append("',");
        if(isFieldMarked(FIELD_last_draw_tower_coin_time_ms)) sBuilder.append(" `last_draw_tower_coin_time_ms` = '").append(last_draw_tower_coin_time_ms).append("',");
        if(isFieldMarked(FIELD_had_active_research_chapter_id)) sBuilder.append(" `had_active_research_chapter_id` = '").append(had_active_research_chapter_id).append("',");
        if(isFieldMarked(FIELD_had_active_research_chapter_level)) sBuilder.append(" `had_active_research_chapter_level` = '").append(had_active_research_chapter_level).append("',");
        if(isFieldMarked(FIELD_highest_had_reach_chapter_id)) sBuilder.append(" `highest_had_reach_chapter_id` = '").append(highest_had_reach_chapter_id).append("',");
        if(isFieldMarked(FIELD_highest_had_reach_chapter_level)) sBuilder.append(" `highest_had_reach_chapter_level` = '").append(highest_had_reach_chapter_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_tower` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '章节ID',"
                + "`chapter_level` int(11) NOT NULL DEFAULT '0' COMMENT '关卡内层数 从1开始',"
                + "`last_draw_tower_coin_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次领取每日迷宫币的时间戳（毫秒）',"
                + "`had_active_research_chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '已激活研究章节ID',"
                + "`had_active_research_chapter_level` int(11) NOT NULL DEFAULT '0' COMMENT '已激活研究章节内层数',"
                + "`highest_had_reach_chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '到达最高章节ID',"
                + "`highest_had_reach_chapter_level` int(11) NOT NULL DEFAULT '0' COMMENT '到达最高章节内层数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家爬塔数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//chapter_id
        _size+=4;//chapter_level
        _size+=8;//last_draw_tower_coin_time_ms
        _size+=8;//had_active_research_chapter_id
        _size+=4;//had_active_research_chapter_level
        _size+=8;//highest_had_reach_chapter_id
        _size+=4;//highest_had_reach_chapter_level
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(chapter_id);
        buff.putInt(chapter_level);
        buff.putLong(last_draw_tower_coin_time_ms);
        buff.putLong(had_active_research_chapter_id);
        buff.putInt(had_active_research_chapter_level);
        buff.putLong(highest_had_reach_chapter_id);
        buff.putInt(highest_had_reach_chapter_level);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        chapter_id=buff.getLong();
        chapter_level=buff.getInt();
        last_draw_tower_coin_time_ms=buff.getLong();
        had_active_research_chapter_id=buff.getLong();
        had_active_research_chapter_level=buff.getInt();
        highest_had_reach_chapter_id=buff.getLong();
        highest_had_reach_chapter_level=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
