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
public class PlayerTowerDefenceReportBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家cid")
    private long cid;

    public static final int FIELD_attackerCid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "attackerCid", comment = "攻击玩家CID")
    private long attackerCid;

    public static final int FIELD_chapterId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "chapterId", comment = "楼层ID")
    private long chapterId;

    public static final int FIELD_chapterLevel =3;
    @DataBaseField(type = "int(11)", fieldname = "chapterLevel", comment = "楼层等级")
    private int chapterLevel;

    public static final int FIELD_isSucc =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "isSucc", comment = "攻击是否成功")
    private boolean isSucc;

    public static final int FIELD_curChapterLevel =5;
    @DataBaseField(type = "int(11)", fieldname = "curChapterLevel", comment = "当前楼层等级")
    private int curChapterLevel;

    public static final int FIELD_timestamp =6;
    @DataBaseField(type = "bigint(20)", fieldname = "timestamp", comment = "时间戳 毫秒")
    private long timestamp;

    public PlayerTowerDefenceReportBO() {
        id = 0;
        cid = 0L;
        attackerCid = 0L;
        chapterId = 0L;
        chapterLevel = 0;
        isSucc = false;
        curChapterLevel = 0;
        timestamp = 0L;
    }

    public PlayerTowerDefenceReportBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        attackerCid = rs.getLong(3);
        chapterId = rs.getLong(4);
        chapterLevel = rs.getInt(5);
        isSucc = rs.getBoolean(6);
        curChapterLevel = rs.getInt(7);
        timestamp = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTowerDefenceReportBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `attackerCid`, `chapterId`, `chapterLevel`, `isSucc`, `curChapterLevel`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`player_tower_defence_report`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(attackerCid).append("', ");
        strBuf.append("'").append(chapterId).append("', ");
        strBuf.append("'").append(chapterLevel).append("', ");
        strBuf.append("'").append(isSucc ? 1 : 0).append("', ");
        strBuf.append("'").append(curChapterLevel).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
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

    // 玩家cid
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

    // 攻击玩家CID
    public long getAttackerCid() { return this.attackerCid; }
    public void setAttackerCid(BM _bm, long attackerCid) {
        if(attackerCid==this.attackerCid) 
            return;
        this.attackerCid = attackerCid; 
        markField(_bm, FIELD_attackerCid); 
    }
    public void saveAttackerCid(BM _bm, long attackerCid) {
        if(attackerCid==this.attackerCid) 
            return;
        this.attackerCid = attackerCid;
        saveField(_bm, "attackerCid", attackerCid);
    }

    // 楼层ID
    public long getChapterId() { return this.chapterId; }
    public void setChapterId(BM _bm, long chapterId) {
        if(chapterId==this.chapterId) 
            return;
        this.chapterId = chapterId; 
        markField(_bm, FIELD_chapterId); 
    }
    public void saveChapterId(BM _bm, long chapterId) {
        if(chapterId==this.chapterId) 
            return;
        this.chapterId = chapterId;
        saveField(_bm, "chapterId", chapterId);
    }

    // 楼层等级
    public int getChapterLevel() { return this.chapterLevel; }
    public void setChapterLevel(BM _bm, int chapterLevel) {
        if(chapterLevel==this.chapterLevel) 
            return;
        this.chapterLevel = chapterLevel; 
        markField(_bm, FIELD_chapterLevel); 
    }
    public void saveChapterLevel(BM _bm, int chapterLevel) {
        if(chapterLevel==this.chapterLevel) 
            return;
        this.chapterLevel = chapterLevel;
        saveField(_bm, "chapterLevel", chapterLevel);
    }

    // 攻击是否成功
    public boolean getIsSucc() { return this.isSucc; }
    public void setIsSucc(BM _bm, boolean isSucc) {
        if(isSucc==this.isSucc) 
            return;
        this.isSucc = isSucc; 
        markField(_bm, FIELD_isSucc); 
    }
    public void saveIsSucc(BM _bm, boolean isSucc) {
        if(isSucc==this.isSucc) 
            return;
        this.isSucc = isSucc;
        saveField(_bm, "isSucc", isSucc ? 1 : 0);
    }

    // 当前楼层等级
    public int getCurChapterLevel() { return this.curChapterLevel; }
    public void setCurChapterLevel(BM _bm, int curChapterLevel) {
        if(curChapterLevel==this.curChapterLevel) 
            return;
        this.curChapterLevel = curChapterLevel; 
        markField(_bm, FIELD_curChapterLevel); 
    }
    public void saveCurChapterLevel(BM _bm, int curChapterLevel) {
        if(curChapterLevel==this.curChapterLevel) 
            return;
        this.curChapterLevel = curChapterLevel;
        saveField(_bm, "curChapterLevel", curChapterLevel);
    }

    // 时间戳 毫秒
    public long getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `attackerCid` = '").append(attackerCid).append("',");
        sBuilder.append(" `chapterId` = '").append(chapterId).append("',");
        sBuilder.append(" `chapterLevel` = '").append(chapterLevel).append("',");
        sBuilder.append(" `isSucc` = '").append(isSucc ? 1 : 0).append("',");
        sBuilder.append(" `curChapterLevel` = '").append(curChapterLevel).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_attackerCid)) sBuilder.append(" `attackerCid` = '").append(attackerCid).append("',");
        if(isFieldMarked(FIELD_chapterId)) sBuilder.append(" `chapterId` = '").append(chapterId).append("',");
        if(isFieldMarked(FIELD_chapterLevel)) sBuilder.append(" `chapterLevel` = '").append(chapterLevel).append("',");
        if(isFieldMarked(FIELD_isSucc)) sBuilder.append(" `isSucc` = '").append(isSucc ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_curChapterLevel)) sBuilder.append(" `curChapterLevel` = '").append(curChapterLevel).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_tower_defence_report` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家cid',"
                + "`attackerCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击玩家CID',"
                + "`chapterId` bigint(20) NOT NULL DEFAULT '0' COMMENT '楼层ID',"
                + "`chapterLevel` int(11) NOT NULL DEFAULT '0' COMMENT '楼层等级',"
                + "`isSucc` tinyint(1) NOT NULL DEFAULT '0' COMMENT '攻击是否成功',"
                + "`curChapterLevel` int(11) NOT NULL DEFAULT '0' COMMENT '当前楼层等级',"
                + "`timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '时间戳 毫秒',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='防守玩家战报数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//attackerCid
        _size+=8;//chapterId
        _size+=4;//chapterLevel
        _size+=1;//isSucc
        _size+=4;//curChapterLevel
        _size+=8;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(attackerCid);
        buff.putLong(chapterId);
        buff.putInt(chapterLevel);
        buff.put((byte)(isSucc?1:0));
        buff.putInt(curChapterLevel);
        buff.putLong(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        attackerCid=buff.getLong();
        chapterId=buff.getLong();
        chapterLevel=buff.getInt();
        isSucc=(buff.get()==1);
        curChapterLevel=buff.getInt();
        timestamp=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
