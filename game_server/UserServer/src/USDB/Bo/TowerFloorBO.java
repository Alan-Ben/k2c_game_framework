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
public class TowerFloorBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_chapter_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "chapter_id", comment = "章节id")
    private long chapter_id;

    public static final int FIELD_chapter_level =1;
    @DataBaseField(type = "int(11)", fieldname = "chapter_level", comment = "章节内楼层数")
    private int chapter_level;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public TowerFloorBO() {
        id = 0;
        chapter_id = 0L;
        chapter_level = 0;
        cid = 0L;
    }

    public TowerFloorBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        chapter_id = rs.getLong(2);
        chapter_level = rs.getInt(3);
        cid = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new TowerFloorBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `chapter_id`, `chapter_level`, `cid`";
    }

    @Override
    public String getTableName() {
        return "`tower_floor`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(chapter_id).append("', ");
        strBuf.append("'").append(chapter_level).append("', ");
        strBuf.append("'").append(cid).append("', ");
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

    // 章节id
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

    // 章节内楼层数
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

    // 玩家id
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        sBuilder.append(" `chapter_level` = '").append(chapter_level).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_chapter_id)) sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        if(isFieldMarked(FIELD_chapter_level)) sBuilder.append(" `chapter_level` = '").append(chapter_level).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `tower_floor` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '章节id',"
                + "`chapter_level` int(11) NOT NULL DEFAULT '0' COMMENT '章节内楼层数',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='爬塔玩家数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//chapter_id
        _size+=4;//chapter_level
        _size+=8;//cid
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(chapter_id);
        buff.putInt(chapter_level);
        buff.putLong(cid);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        chapter_id=buff.getLong();
        chapter_level=buff.getInt();
        cid=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
