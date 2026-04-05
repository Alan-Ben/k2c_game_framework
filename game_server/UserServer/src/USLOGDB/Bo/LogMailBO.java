package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogMailBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_logType =1;
    @DataBaseField(type = "int(11)", fieldname = "logType", comment = "日志类型，增删改")
    private int logType;

    public static final int FIELD_mail_ref_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "mail_ref_id", comment = "配置邮件ID")
    private long mail_ref_id;

    public static final int FIELD_phpMailId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "phpMailId", comment = "运营邮件ID")
    private long phpMailId;

    public static final int FIELD_expiredTimeSec =4;
    @DataBaseField(type = "bigint(20)", fieldname = "expiredTimeSec", comment = "过期时间戳（秒）")
    private long expiredTimeSec;

    public static final int FIELD_curMaxGMailId =5;
    @DataBaseField(type = "bigint(20)", fieldname = "curMaxGMailId", comment = "此时邮件分界线")
    private long curMaxGMailId;

    public static final int FIELD_playerLanguage =6;
    @DataBaseField(type = "varchar(20)", fieldname = "playerLanguage", comment = "玩家语言")
    private String playerLanguage;

    public static final int FIELD_event_id =7;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =8;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =9;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =10;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public LogMailBO() {
        id = 0;
        cid = 0L;
        logType = 0;
        mail_ref_id = 0L;
        phpMailId = 0L;
        expiredTimeSec = 0L;
        curMaxGMailId = 0L;
        playerLanguage = "";
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
    }

    public LogMailBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        logType = rs.getInt(3);
        mail_ref_id = rs.getLong(4);
        phpMailId = rs.getLong(5);
        expiredTimeSec = rs.getLong(6);
        curMaxGMailId = rs.getLong(7);
        playerLanguage = rs.getString(8);
        event_id = rs.getInt(9);
        guid = rs.getLong(10);
        date_time = rs.getInt(11);
        timestamp = rs.getInt(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogMailBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `logType`, `mail_ref_id`, `phpMailId`, `expiredTimeSec`, `curMaxGMailId`, `playerLanguage`, `event_id`, `guid`, `date_time`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`log_mail`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(logType).append("', ");
        strBuf.append("'").append(mail_ref_id).append("', ");
        strBuf.append("'").append(phpMailId).append("', ");
        strBuf.append("'").append(expiredTimeSec).append("', ");
        strBuf.append("'").append(curMaxGMailId).append("', ");
        strBuf.append("'").append(playerLanguage == null ? null : playerLanguage.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
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

    // 日志类型，增删改
    public int getLogType() { return this.logType; }
    public void setLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType; 
        markField(_bm, FIELD_logType); 
    }
    public void saveLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType;
        saveField(_bm, "logType", logType);
    }

    // 配置邮件ID
    public long getMailRefId() { return this.mail_ref_id; }
    public void setMailRefId(BM _bm, long mail_ref_id) {
        if(mail_ref_id==this.mail_ref_id) 
            return;
        this.mail_ref_id = mail_ref_id; 
        markField(_bm, FIELD_mail_ref_id); 
    }
    public void saveMailRefId(BM _bm, long mail_ref_id) {
        if(mail_ref_id==this.mail_ref_id) 
            return;
        this.mail_ref_id = mail_ref_id;
        saveField(_bm, "mail_ref_id", mail_ref_id);
    }

    // 运营邮件ID
    public long getPhpMailId() { return this.phpMailId; }
    public void setPhpMailId(BM _bm, long phpMailId) {
        if(phpMailId==this.phpMailId) 
            return;
        this.phpMailId = phpMailId; 
        markField(_bm, FIELD_phpMailId); 
    }
    public void savePhpMailId(BM _bm, long phpMailId) {
        if(phpMailId==this.phpMailId) 
            return;
        this.phpMailId = phpMailId;
        saveField(_bm, "phpMailId", phpMailId);
    }

    // 过期时间戳（秒）
    public long getExpiredTimeSec() { return this.expiredTimeSec; }
    public void setExpiredTimeSec(BM _bm, long expiredTimeSec) {
        if(expiredTimeSec==this.expiredTimeSec) 
            return;
        this.expiredTimeSec = expiredTimeSec; 
        markField(_bm, FIELD_expiredTimeSec); 
    }
    public void saveExpiredTimeSec(BM _bm, long expiredTimeSec) {
        if(expiredTimeSec==this.expiredTimeSec) 
            return;
        this.expiredTimeSec = expiredTimeSec;
        saveField(_bm, "expiredTimeSec", expiredTimeSec);
    }

    // 此时邮件分界线
    public long getCurMaxGMailId() { return this.curMaxGMailId; }
    public void setCurMaxGMailId(BM _bm, long curMaxGMailId) {
        if(curMaxGMailId==this.curMaxGMailId) 
            return;
        this.curMaxGMailId = curMaxGMailId; 
        markField(_bm, FIELD_curMaxGMailId); 
    }
    public void saveCurMaxGMailId(BM _bm, long curMaxGMailId) {
        if(curMaxGMailId==this.curMaxGMailId) 
            return;
        this.curMaxGMailId = curMaxGMailId;
        saveField(_bm, "curMaxGMailId", curMaxGMailId);
    }

    // 玩家语言
    public String getPlayerLanguage() { return this.playerLanguage; }
    public void setPlayerLanguage(BM _bm, String playerLanguage) {
        if(playerLanguage.equals(this.playerLanguage)) 
            return;
        this.playerLanguage = playerLanguage; 
        markField(_bm, FIELD_playerLanguage); 
    }
    public void savePlayerLanguage(BM _bm, String playerLanguage) {
        if(playerLanguage.equals(this.playerLanguage)) 
            return;
        this.playerLanguage = playerLanguage;
        saveField(_bm, "playerLanguage", playerLanguage);
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



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `logType` = '").append(logType).append("',");
        sBuilder.append(" `mail_ref_id` = '").append(mail_ref_id).append("',");
        sBuilder.append(" `phpMailId` = '").append(phpMailId).append("',");
        sBuilder.append(" `expiredTimeSec` = '").append(expiredTimeSec).append("',");
        sBuilder.append(" `curMaxGMailId` = '").append(curMaxGMailId).append("',");
        sBuilder.append(" `playerLanguage` = '").append(playerLanguage == null ? null : playerLanguage.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_logType)) sBuilder.append(" `logType` = '").append(logType).append("',");
        if(isFieldMarked(FIELD_mail_ref_id)) sBuilder.append(" `mail_ref_id` = '").append(mail_ref_id).append("',");
        if(isFieldMarked(FIELD_phpMailId)) sBuilder.append(" `phpMailId` = '").append(phpMailId).append("',");
        if(isFieldMarked(FIELD_expiredTimeSec)) sBuilder.append(" `expiredTimeSec` = '").append(expiredTimeSec).append("',");
        if(isFieldMarked(FIELD_curMaxGMailId)) sBuilder.append(" `curMaxGMailId` = '").append(curMaxGMailId).append("',");
        if(isFieldMarked(FIELD_playerLanguage)) sBuilder.append(" `playerLanguage` = '").append(playerLanguage == null ? null : playerLanguage.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_mail` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型，增删改',"
                + "`mail_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置邮件ID',"
                + "`phpMailId` bigint(20) NOT NULL DEFAULT '0' COMMENT '运营邮件ID',"
                + "`expiredTimeSec` bigint(20) NOT NULL DEFAULT '0' COMMENT '过期时间戳（秒）',"
                + "`curMaxGMailId` bigint(20) NOT NULL DEFAULT '0' COMMENT '此时邮件分界线',"
                + "`playerLanguage` varchar(20) NOT NULL DEFAULT '' COMMENT '玩家语言',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Mail Log 邮件数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//logType
        _size+=8;//mail_ref_id
        _size+=8;//phpMailId
        _size+=8;//expiredTimeSec
        _size+=8;//curMaxGMailId
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerLanguage);//playerLanguage
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(logType);
        buff.putLong(mail_ref_id);
        buff.putLong(phpMailId);
        buff.putLong(expiredTimeSec);
        buff.putLong(curMaxGMailId);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, playerLanguage);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        logType=buff.getInt();
        mail_ref_id=buff.getLong();
        phpMailId=buff.getLong();
        expiredTimeSec=buff.getLong();
        curMaxGMailId=buff.getLong();
        playerLanguage=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
