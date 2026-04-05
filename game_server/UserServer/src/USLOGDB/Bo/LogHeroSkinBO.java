package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogHeroSkinBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_event_id =0;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =2;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =3;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_cid =4;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_skin_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "skin_id", comment = "大臣皮肤ID")
    private long skin_id;

    public static final int FIELD_ori_lvl =6;
    @DataBaseField(type = "int(11)", fieldname = "ori_lvl", comment = "原皮肤等级")
    private int ori_lvl;

    public static final int FIELD_cur_lvl =7;
    @DataBaseField(type = "int(11)", fieldname = "cur_lvl", comment = "现皮肤等级")
    private int cur_lvl;

    public LogHeroSkinBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        skin_id = 0L;
        ori_lvl = 0;
        cur_lvl = 0;
    }

    public LogHeroSkinBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        cid = rs.getLong(6);
        skin_id = rs.getLong(7);
        ori_lvl = rs.getInt(8);
        cur_lvl = rs.getInt(9);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogHeroSkinBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `skin_id`, `ori_lvl`, `cur_lvl`";
    }

    @Override
    public String getTableName() {
        return "`log_hero_skin`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(skin_id).append("', ");
        strBuf.append("'").append(ori_lvl).append("', ");
        strBuf.append("'").append(cur_lvl).append("', ");
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

    // 事件类型
    public int getEventId() { return this.event_id; }
    public void setEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 事件唯一id
    public long getGuid() { return this.guid; }
    public void setGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid; 
        markField(_bm, FIELD_guid); 
    }
    public void saveGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid;
        saveField(_bm, "guid", guid);
    }

    // 日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }

    // 时间戳
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
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

    // 大臣皮肤ID
    public long getSkinId() { return this.skin_id; }
    public void setSkinId(BM _bm, long skin_id) {
        if(skin_id==this.skin_id) 
            return;
        this.skin_id = skin_id; 
        markField(_bm, FIELD_skin_id); 
    }
    public void saveSkinId(BM _bm, long skin_id) {
        if(skin_id==this.skin_id) 
            return;
        this.skin_id = skin_id;
        saveField(_bm, "skin_id", skin_id);
    }

    // 原皮肤等级
    public int getOriLvl() { return this.ori_lvl; }
    public void setOriLvl(BM _bm, int ori_lvl) {
        if(ori_lvl==this.ori_lvl) 
            return;
        this.ori_lvl = ori_lvl; 
        markField(_bm, FIELD_ori_lvl); 
    }
    public void saveOriLvl(BM _bm, int ori_lvl) {
        if(ori_lvl==this.ori_lvl) 
            return;
        this.ori_lvl = ori_lvl;
        saveField(_bm, "ori_lvl", ori_lvl);
    }

    // 现皮肤等级
    public int getCurLvl() { return this.cur_lvl; }
    public void setCurLvl(BM _bm, int cur_lvl) {
        if(cur_lvl==this.cur_lvl) 
            return;
        this.cur_lvl = cur_lvl; 
        markField(_bm, FIELD_cur_lvl); 
    }
    public void saveCurLvl(BM _bm, int cur_lvl) {
        if(cur_lvl==this.cur_lvl) 
            return;
        this.cur_lvl = cur_lvl;
        saveField(_bm, "cur_lvl", cur_lvl);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        sBuilder.append(" `ori_lvl` = '").append(ori_lvl).append("',");
        sBuilder.append(" `cur_lvl` = '").append(cur_lvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_skin_id)) sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        if(isFieldMarked(FIELD_ori_lvl)) sBuilder.append(" `ori_lvl` = '").append(ori_lvl).append("',");
        if(isFieldMarked(FIELD_cur_lvl)) sBuilder.append(" `cur_lvl` = '").append(cur_lvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_hero_skin` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`skin_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣皮肤ID',"
                + "`ori_lvl` int(11) NOT NULL DEFAULT '0' COMMENT '原皮肤等级',"
                + "`cur_lvl` int(11) NOT NULL DEFAULT '0' COMMENT '现皮肤等级',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大臣皮肤数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//cid
        _size+=8;//skin_id
        _size+=4;//ori_lvl
        _size+=4;//cur_lvl
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(cid);
        buff.putLong(skin_id);
        buff.putInt(ori_lvl);
        buff.putInt(cur_lvl);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        cid=buff.getLong();
        skin_id=buff.getLong();
        ori_lvl=buff.getInt();
        cur_lvl=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
