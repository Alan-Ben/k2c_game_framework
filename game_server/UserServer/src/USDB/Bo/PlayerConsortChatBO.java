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
public class PlayerConsortChatBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_last_refresh_count_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "last_refresh_count_time_ms", comment = "上次刷新次数时间ms")
    private long last_refresh_count_time_ms;

    public static final int FIELD_day_had_send_pay_ai_times =2;
    @DataBaseField(type = "int(11)", fieldname = "day_had_send_pay_ai_times", comment = "今日已发送付费AI次数")
    private int day_had_send_pay_ai_times;

    public static final int FIELD_day_had_send_circle_ai_times =3;
    @DataBaseField(type = "int(11)", fieldname = "day_had_send_circle_ai_times", comment = "今日已发送圈子AI次数")
    private int day_had_send_circle_ai_times;

    public static final int FIELD_day_had_send_circle_ai_reply_times =4;
    @DataBaseField(type = "int(11)", fieldname = "day_had_send_circle_ai_reply_times", comment = "今日已发送圈子AI回复次数")
    private int day_had_send_circle_ai_reply_times;

    public static final int FIELD_day_had_send_consort_initiative_times =5;
    @DataBaseField(type = "int(11)", fieldname = "day_had_send_consort_initiative_times", comment = "今日妃子主动发送消息次数")
    private int day_had_send_consort_initiative_times;

    public static final int FIELD_day_had_evaluate_reply_times =6;
    @DataBaseField(type = "int(11)", fieldname = "day_had_evaluate_reply_times", comment = "今日已评价回复次数")
    private int day_had_evaluate_reply_times;

    public PlayerConsortChatBO() {
        id = 0;
        cid = 0L;
        last_refresh_count_time_ms = 0L;
        day_had_send_pay_ai_times = 0;
        day_had_send_circle_ai_times = 0;
        day_had_send_circle_ai_reply_times = 0;
        day_had_send_consort_initiative_times = 0;
        day_had_evaluate_reply_times = 0;
    }

    public PlayerConsortChatBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        last_refresh_count_time_ms = rs.getLong(3);
        day_had_send_pay_ai_times = rs.getInt(4);
        day_had_send_circle_ai_times = rs.getInt(5);
        day_had_send_circle_ai_reply_times = rs.getInt(6);
        day_had_send_consort_initiative_times = rs.getInt(7);
        day_had_evaluate_reply_times = rs.getInt(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortChatBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `last_refresh_count_time_ms`, `day_had_send_pay_ai_times`, `day_had_send_circle_ai_times`, `day_had_send_circle_ai_reply_times`, `day_had_send_consort_initiative_times`, `day_had_evaluate_reply_times`";
    }

    @Override
    public String getTableName() {
        return "`player_consort_chat`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(last_refresh_count_time_ms).append("', ");
        strBuf.append("'").append(day_had_send_pay_ai_times).append("', ");
        strBuf.append("'").append(day_had_send_circle_ai_times).append("', ");
        strBuf.append("'").append(day_had_send_circle_ai_reply_times).append("', ");
        strBuf.append("'").append(day_had_send_consort_initiative_times).append("', ");
        strBuf.append("'").append(day_had_evaluate_reply_times).append("', ");
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

    // 上次刷新次数时间ms
    public long getLastRefreshCountTimeMs() { return this.last_refresh_count_time_ms; }
    public void setLastRefreshCountTimeMs(BM _bm, long last_refresh_count_time_ms) {
        if(last_refresh_count_time_ms==this.last_refresh_count_time_ms) 
            return;
        this.last_refresh_count_time_ms = last_refresh_count_time_ms; 
        markField(_bm, FIELD_last_refresh_count_time_ms); 
    }
    public void saveLastRefreshCountTimeMs(BM _bm, long last_refresh_count_time_ms) {
        if(last_refresh_count_time_ms==this.last_refresh_count_time_ms) 
            return;
        this.last_refresh_count_time_ms = last_refresh_count_time_ms;
        saveField(_bm, "last_refresh_count_time_ms", last_refresh_count_time_ms);
    }

    // 今日已发送付费AI次数
    public int getDayHadSendPayAiTimes() { return this.day_had_send_pay_ai_times; }
    public void setDayHadSendPayAiTimes(BM _bm, int day_had_send_pay_ai_times) {
        if(day_had_send_pay_ai_times==this.day_had_send_pay_ai_times) 
            return;
        this.day_had_send_pay_ai_times = day_had_send_pay_ai_times; 
        markField(_bm, FIELD_day_had_send_pay_ai_times); 
    }
    public void saveDayHadSendPayAiTimes(BM _bm, int day_had_send_pay_ai_times) {
        if(day_had_send_pay_ai_times==this.day_had_send_pay_ai_times) 
            return;
        this.day_had_send_pay_ai_times = day_had_send_pay_ai_times;
        saveField(_bm, "day_had_send_pay_ai_times", day_had_send_pay_ai_times);
    }

    // 今日已发送圈子AI次数
    public int getDayHadSendCircleAiTimes() { return this.day_had_send_circle_ai_times; }
    public void setDayHadSendCircleAiTimes(BM _bm, int day_had_send_circle_ai_times) {
        if(day_had_send_circle_ai_times==this.day_had_send_circle_ai_times) 
            return;
        this.day_had_send_circle_ai_times = day_had_send_circle_ai_times; 
        markField(_bm, FIELD_day_had_send_circle_ai_times); 
    }
    public void saveDayHadSendCircleAiTimes(BM _bm, int day_had_send_circle_ai_times) {
        if(day_had_send_circle_ai_times==this.day_had_send_circle_ai_times) 
            return;
        this.day_had_send_circle_ai_times = day_had_send_circle_ai_times;
        saveField(_bm, "day_had_send_circle_ai_times", day_had_send_circle_ai_times);
    }

    // 今日已发送圈子AI回复次数
    public int getDayHadSendCircleAiReplyTimes() { return this.day_had_send_circle_ai_reply_times; }
    public void setDayHadSendCircleAiReplyTimes(BM _bm, int day_had_send_circle_ai_reply_times) {
        if(day_had_send_circle_ai_reply_times==this.day_had_send_circle_ai_reply_times) 
            return;
        this.day_had_send_circle_ai_reply_times = day_had_send_circle_ai_reply_times; 
        markField(_bm, FIELD_day_had_send_circle_ai_reply_times); 
    }
    public void saveDayHadSendCircleAiReplyTimes(BM _bm, int day_had_send_circle_ai_reply_times) {
        if(day_had_send_circle_ai_reply_times==this.day_had_send_circle_ai_reply_times) 
            return;
        this.day_had_send_circle_ai_reply_times = day_had_send_circle_ai_reply_times;
        saveField(_bm, "day_had_send_circle_ai_reply_times", day_had_send_circle_ai_reply_times);
    }

    // 今日妃子主动发送消息次数
    public int getDayHadSendConsortInitiativeTimes() { return this.day_had_send_consort_initiative_times; }
    public void setDayHadSendConsortInitiativeTimes(BM _bm, int day_had_send_consort_initiative_times) {
        if(day_had_send_consort_initiative_times==this.day_had_send_consort_initiative_times) 
            return;
        this.day_had_send_consort_initiative_times = day_had_send_consort_initiative_times; 
        markField(_bm, FIELD_day_had_send_consort_initiative_times); 
    }
    public void saveDayHadSendConsortInitiativeTimes(BM _bm, int day_had_send_consort_initiative_times) {
        if(day_had_send_consort_initiative_times==this.day_had_send_consort_initiative_times) 
            return;
        this.day_had_send_consort_initiative_times = day_had_send_consort_initiative_times;
        saveField(_bm, "day_had_send_consort_initiative_times", day_had_send_consort_initiative_times);
    }

    // 今日已评价回复次数
    public int getDayHadEvaluateReplyTimes() { return this.day_had_evaluate_reply_times; }
    public void setDayHadEvaluateReplyTimes(BM _bm, int day_had_evaluate_reply_times) {
        if(day_had_evaluate_reply_times==this.day_had_evaluate_reply_times) 
            return;
        this.day_had_evaluate_reply_times = day_had_evaluate_reply_times; 
        markField(_bm, FIELD_day_had_evaluate_reply_times); 
    }
    public void saveDayHadEvaluateReplyTimes(BM _bm, int day_had_evaluate_reply_times) {
        if(day_had_evaluate_reply_times==this.day_had_evaluate_reply_times) 
            return;
        this.day_had_evaluate_reply_times = day_had_evaluate_reply_times;
        saveField(_bm, "day_had_evaluate_reply_times", day_had_evaluate_reply_times);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `last_refresh_count_time_ms` = '").append(last_refresh_count_time_ms).append("',");
        sBuilder.append(" `day_had_send_pay_ai_times` = '").append(day_had_send_pay_ai_times).append("',");
        sBuilder.append(" `day_had_send_circle_ai_times` = '").append(day_had_send_circle_ai_times).append("',");
        sBuilder.append(" `day_had_send_circle_ai_reply_times` = '").append(day_had_send_circle_ai_reply_times).append("',");
        sBuilder.append(" `day_had_send_consort_initiative_times` = '").append(day_had_send_consort_initiative_times).append("',");
        sBuilder.append(" `day_had_evaluate_reply_times` = '").append(day_had_evaluate_reply_times).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_last_refresh_count_time_ms)) sBuilder.append(" `last_refresh_count_time_ms` = '").append(last_refresh_count_time_ms).append("',");
        if(isFieldMarked(FIELD_day_had_send_pay_ai_times)) sBuilder.append(" `day_had_send_pay_ai_times` = '").append(day_had_send_pay_ai_times).append("',");
        if(isFieldMarked(FIELD_day_had_send_circle_ai_times)) sBuilder.append(" `day_had_send_circle_ai_times` = '").append(day_had_send_circle_ai_times).append("',");
        if(isFieldMarked(FIELD_day_had_send_circle_ai_reply_times)) sBuilder.append(" `day_had_send_circle_ai_reply_times` = '").append(day_had_send_circle_ai_reply_times).append("',");
        if(isFieldMarked(FIELD_day_had_send_consort_initiative_times)) sBuilder.append(" `day_had_send_consort_initiative_times` = '").append(day_had_send_consort_initiative_times).append("',");
        if(isFieldMarked(FIELD_day_had_evaluate_reply_times)) sBuilder.append(" `day_had_evaluate_reply_times` = '").append(day_had_evaluate_reply_times).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort_chat` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`last_refresh_count_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次刷新次数时间ms',"
                + "`day_had_send_pay_ai_times` int(11) NOT NULL DEFAULT '0' COMMENT '今日已发送付费AI次数',"
                + "`day_had_send_circle_ai_times` int(11) NOT NULL DEFAULT '0' COMMENT '今日已发送圈子AI次数',"
                + "`day_had_send_circle_ai_reply_times` int(11) NOT NULL DEFAULT '0' COMMENT '今日已发送圈子AI回复次数',"
                + "`day_had_send_consort_initiative_times` int(11) NOT NULL DEFAULT '0' COMMENT '今日妃子主动发送消息次数',"
                + "`day_had_evaluate_reply_times` int(11) NOT NULL DEFAULT '0' COMMENT '今日已评价回复次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家妃子聊天数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//last_refresh_count_time_ms
        _size+=4;//day_had_send_pay_ai_times
        _size+=4;//day_had_send_circle_ai_times
        _size+=4;//day_had_send_circle_ai_reply_times
        _size+=4;//day_had_send_consort_initiative_times
        _size+=4;//day_had_evaluate_reply_times
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(last_refresh_count_time_ms);
        buff.putInt(day_had_send_pay_ai_times);
        buff.putInt(day_had_send_circle_ai_times);
        buff.putInt(day_had_send_circle_ai_reply_times);
        buff.putInt(day_had_send_consort_initiative_times);
        buff.putInt(day_had_evaluate_reply_times);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        last_refresh_count_time_ms=buff.getLong();
        day_had_send_pay_ai_times=buff.getInt();
        day_had_send_circle_ai_times=buff.getInt();
        day_had_send_circle_ai_reply_times=buff.getInt();
        day_had_send_consort_initiative_times=buff.getInt();
        day_had_evaluate_reply_times=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
