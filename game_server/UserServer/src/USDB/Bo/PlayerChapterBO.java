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
public class PlayerChapterBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_chapter_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "chapter_id", comment = "章节ID")
    private long chapter_id;

    public static final int FIELD_point =2;
    @DataBaseField(type = "int(11)", fieldname = "point", comment = "位置")
    private int point;

    public static final int FIELD_gold_inspire_times =3;
    @DataBaseField(type = "int(11)", fieldname = "gold_inspire_times", comment = "金币鼓舞次数")
    private int gold_inspire_times;

    public static final int FIELD_item_inspire_times =4;
    @DataBaseField(type = "int(11)", fieldname = "item_inspire_times", comment = "道具鼓舞次数")
    private int item_inspire_times;

    public static final int FIELD_crystal_inspire_times =5;
    @DataBaseField(type = "int(11)", fieldname = "crystal_inspire_times", comment = "钻石鼓舞次数")
    private int crystal_inspire_times;

    public PlayerChapterBO() {
        id = 0;
        cid = 0L;
        chapter_id = 0L;
        point = 0;
        gold_inspire_times = 0;
        item_inspire_times = 0;
        crystal_inspire_times = 0;
    }

    public PlayerChapterBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        chapter_id = rs.getLong(3);
        point = rs.getInt(4);
        gold_inspire_times = rs.getInt(5);
        item_inspire_times = rs.getInt(6);
        crystal_inspire_times = rs.getInt(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerChapterBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `chapter_id`, `point`, `gold_inspire_times`, `item_inspire_times`, `crystal_inspire_times`";
    }

    @Override
    public String getTableName() {
        return "`player_chapter`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(chapter_id).append("', ");
        strBuf.append("'").append(point).append("', ");
        strBuf.append("'").append(gold_inspire_times).append("', ");
        strBuf.append("'").append(item_inspire_times).append("', ");
        strBuf.append("'").append(crystal_inspire_times).append("', ");
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

    // 位置
    public int getPoint() { return this.point; }
    public void setPoint(BM _bm, int point) {
        if(point==this.point) 
            return;
        this.point = point; 
        markField(_bm, FIELD_point); 
    }
    public void savePoint(BM _bm, int point) {
        if(point==this.point) 
            return;
        this.point = point;
        saveField(_bm, "point", point);
    }

    // 金币鼓舞次数
    public int getGoldInspireTimes() { return this.gold_inspire_times; }
    public void setGoldInspireTimes(BM _bm, int gold_inspire_times) {
        if(gold_inspire_times==this.gold_inspire_times) 
            return;
        this.gold_inspire_times = gold_inspire_times; 
        markField(_bm, FIELD_gold_inspire_times); 
    }
    public void saveGoldInspireTimes(BM _bm, int gold_inspire_times) {
        if(gold_inspire_times==this.gold_inspire_times) 
            return;
        this.gold_inspire_times = gold_inspire_times;
        saveField(_bm, "gold_inspire_times", gold_inspire_times);
    }

    // 道具鼓舞次数
    public int getItemInspireTimes() { return this.item_inspire_times; }
    public void setItemInspireTimes(BM _bm, int item_inspire_times) {
        if(item_inspire_times==this.item_inspire_times) 
            return;
        this.item_inspire_times = item_inspire_times; 
        markField(_bm, FIELD_item_inspire_times); 
    }
    public void saveItemInspireTimes(BM _bm, int item_inspire_times) {
        if(item_inspire_times==this.item_inspire_times) 
            return;
        this.item_inspire_times = item_inspire_times;
        saveField(_bm, "item_inspire_times", item_inspire_times);
    }

    // 钻石鼓舞次数
    public int getCrystalInspireTimes() { return this.crystal_inspire_times; }
    public void setCrystalInspireTimes(BM _bm, int crystal_inspire_times) {
        if(crystal_inspire_times==this.crystal_inspire_times) 
            return;
        this.crystal_inspire_times = crystal_inspire_times; 
        markField(_bm, FIELD_crystal_inspire_times); 
    }
    public void saveCrystalInspireTimes(BM _bm, int crystal_inspire_times) {
        if(crystal_inspire_times==this.crystal_inspire_times) 
            return;
        this.crystal_inspire_times = crystal_inspire_times;
        saveField(_bm, "crystal_inspire_times", crystal_inspire_times);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        sBuilder.append(" `point` = '").append(point).append("',");
        sBuilder.append(" `gold_inspire_times` = '").append(gold_inspire_times).append("',");
        sBuilder.append(" `item_inspire_times` = '").append(item_inspire_times).append("',");
        sBuilder.append(" `crystal_inspire_times` = '").append(crystal_inspire_times).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_chapter_id)) sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        if(isFieldMarked(FIELD_point)) sBuilder.append(" `point` = '").append(point).append("',");
        if(isFieldMarked(FIELD_gold_inspire_times)) sBuilder.append(" `gold_inspire_times` = '").append(gold_inspire_times).append("',");
        if(isFieldMarked(FIELD_item_inspire_times)) sBuilder.append(" `item_inspire_times` = '").append(item_inspire_times).append("',");
        if(isFieldMarked(FIELD_crystal_inspire_times)) sBuilder.append(" `crystal_inspire_times` = '").append(crystal_inspire_times).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_chapter` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '章节ID',"
                + "`point` int(11) NOT NULL DEFAULT '0' COMMENT '位置',"
                + "`gold_inspire_times` int(11) NOT NULL DEFAULT '0' COMMENT '金币鼓舞次数',"
                + "`item_inspire_times` int(11) NOT NULL DEFAULT '0' COMMENT '道具鼓舞次数',"
                + "`crystal_inspire_times` int(11) NOT NULL DEFAULT '0' COMMENT '钻石鼓舞次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家关卡数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//point
        _size+=4;//gold_inspire_times
        _size+=4;//item_inspire_times
        _size+=4;//crystal_inspire_times
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
        buff.putInt(point);
        buff.putInt(gold_inspire_times);
        buff.putInt(item_inspire_times);
        buff.putInt(crystal_inspire_times);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        chapter_id=buff.getLong();
        point=buff.getInt();
        gold_inspire_times=buff.getInt();
        item_inspire_times=buff.getInt();
        crystal_inspire_times=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
