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
public class MiddayDungeonBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_round_preview_time_ms =0;
    @DataBaseField(type = "bigint(20)", fieldname = "round_preview_time_ms", comment = "本轮预告时间")
    private long round_preview_time_ms;

    public static final int FIELD_round_start_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "round_start_time_ms", comment = "本轮开始时间")
    private long round_start_time_ms;

    public static final int FIELD_round_end_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "round_end_time_ms", comment = "本轮结束时间")
    private long round_end_time_ms;

    public static final int FIELD_round_drop_box_num =3;
    @DataBaseField(type = "int(11)", fieldname = "round_drop_box_num", comment = "本轮掉落宝箱数量")
    private int round_drop_box_num;

    public static final int FIELD_had_send_open_notice =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_send_open_notice", comment = "是否已发送开启通知")
    private boolean had_send_open_notice;

    public MiddayDungeonBO() {
        id = 0;
        round_preview_time_ms = 0L;
        round_start_time_ms = 0L;
        round_end_time_ms = 0L;
        round_drop_box_num = 0;
        had_send_open_notice = false;
    }

    public MiddayDungeonBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        round_preview_time_ms = rs.getLong(2);
        round_start_time_ms = rs.getLong(3);
        round_end_time_ms = rs.getLong(4);
        round_drop_box_num = rs.getInt(5);
        had_send_open_notice = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MiddayDungeonBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `round_preview_time_ms`, `round_start_time_ms`, `round_end_time_ms`, `round_drop_box_num`, `had_send_open_notice`";
    }

    @Override
    public String getTableName() {
        return "`midday_dungeon`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(round_preview_time_ms).append("', ");
        strBuf.append("'").append(round_start_time_ms).append("', ");
        strBuf.append("'").append(round_end_time_ms).append("', ");
        strBuf.append("'").append(round_drop_box_num).append("', ");
        strBuf.append("'").append(had_send_open_notice ? 1 : 0).append("', ");
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

    // 本轮预告时间
    public long getRoundPreviewTimeMs() { return this.round_preview_time_ms; }
    public void setRoundPreviewTimeMs(BM _bm, long round_preview_time_ms) {
        if(round_preview_time_ms==this.round_preview_time_ms) 
            return;
        this.round_preview_time_ms = round_preview_time_ms; 
        markField(_bm, FIELD_round_preview_time_ms); 
    }
    public void saveRoundPreviewTimeMs(BM _bm, long round_preview_time_ms) {
        if(round_preview_time_ms==this.round_preview_time_ms) 
            return;
        this.round_preview_time_ms = round_preview_time_ms;
        saveField(_bm, "round_preview_time_ms", round_preview_time_ms);
    }

    // 本轮开始时间
    public long getRoundStartTimeMs() { return this.round_start_time_ms; }
    public void setRoundStartTimeMs(BM _bm, long round_start_time_ms) {
        if(round_start_time_ms==this.round_start_time_ms) 
            return;
        this.round_start_time_ms = round_start_time_ms; 
        markField(_bm, FIELD_round_start_time_ms); 
    }
    public void saveRoundStartTimeMs(BM _bm, long round_start_time_ms) {
        if(round_start_time_ms==this.round_start_time_ms) 
            return;
        this.round_start_time_ms = round_start_time_ms;
        saveField(_bm, "round_start_time_ms", round_start_time_ms);
    }

    // 本轮结束时间
    public long getRoundEndTimeMs() { return this.round_end_time_ms; }
    public void setRoundEndTimeMs(BM _bm, long round_end_time_ms) {
        if(round_end_time_ms==this.round_end_time_ms) 
            return;
        this.round_end_time_ms = round_end_time_ms; 
        markField(_bm, FIELD_round_end_time_ms); 
    }
    public void saveRoundEndTimeMs(BM _bm, long round_end_time_ms) {
        if(round_end_time_ms==this.round_end_time_ms) 
            return;
        this.round_end_time_ms = round_end_time_ms;
        saveField(_bm, "round_end_time_ms", round_end_time_ms);
    }

    // 本轮掉落宝箱数量
    public int getRoundDropBoxNum() { return this.round_drop_box_num; }
    public void setRoundDropBoxNum(BM _bm, int round_drop_box_num) {
        if(round_drop_box_num==this.round_drop_box_num) 
            return;
        this.round_drop_box_num = round_drop_box_num; 
        markField(_bm, FIELD_round_drop_box_num); 
    }
    public void saveRoundDropBoxNum(BM _bm, int round_drop_box_num) {
        if(round_drop_box_num==this.round_drop_box_num) 
            return;
        this.round_drop_box_num = round_drop_box_num;
        saveField(_bm, "round_drop_box_num", round_drop_box_num);
    }

    // 是否已发送开启通知
    public boolean getHadSendOpenNotice() { return this.had_send_open_notice; }
    public void setHadSendOpenNotice(BM _bm, boolean had_send_open_notice) {
        if(had_send_open_notice==this.had_send_open_notice) 
            return;
        this.had_send_open_notice = had_send_open_notice; 
        markField(_bm, FIELD_had_send_open_notice); 
    }
    public void saveHadSendOpenNotice(BM _bm, boolean had_send_open_notice) {
        if(had_send_open_notice==this.had_send_open_notice) 
            return;
        this.had_send_open_notice = had_send_open_notice;
        saveField(_bm, "had_send_open_notice", had_send_open_notice ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `round_preview_time_ms` = '").append(round_preview_time_ms).append("',");
        sBuilder.append(" `round_start_time_ms` = '").append(round_start_time_ms).append("',");
        sBuilder.append(" `round_end_time_ms` = '").append(round_end_time_ms).append("',");
        sBuilder.append(" `round_drop_box_num` = '").append(round_drop_box_num).append("',");
        sBuilder.append(" `had_send_open_notice` = '").append(had_send_open_notice ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_round_preview_time_ms)) sBuilder.append(" `round_preview_time_ms` = '").append(round_preview_time_ms).append("',");
        if(isFieldMarked(FIELD_round_start_time_ms)) sBuilder.append(" `round_start_time_ms` = '").append(round_start_time_ms).append("',");
        if(isFieldMarked(FIELD_round_end_time_ms)) sBuilder.append(" `round_end_time_ms` = '").append(round_end_time_ms).append("',");
        if(isFieldMarked(FIELD_round_drop_box_num)) sBuilder.append(" `round_drop_box_num` = '").append(round_drop_box_num).append("',");
        if(isFieldMarked(FIELD_had_send_open_notice)) sBuilder.append(" `had_send_open_notice` = '").append(had_send_open_notice ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `midday_dungeon` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`round_preview_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮预告时间',"
                + "`round_start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮开始时间',"
                + "`round_end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮结束时间',"
                + "`round_drop_box_num` int(11) NOT NULL DEFAULT '0' COMMENT '本轮掉落宝箱数量',"
                + "`had_send_open_notice` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已发送开启通知',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='午间副本' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//round_preview_time_ms
        _size+=8;//round_start_time_ms
        _size+=8;//round_end_time_ms
        _size+=4;//round_drop_box_num
        _size+=1;//had_send_open_notice
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(round_preview_time_ms);
        buff.putLong(round_start_time_ms);
        buff.putLong(round_end_time_ms);
        buff.putInt(round_drop_box_num);
        buff.put((byte)(had_send_open_notice?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        round_preview_time_ms=buff.getLong();
        round_start_time_ms=buff.getLong();
        round_end_time_ms=buff.getLong();
        round_drop_box_num=buff.getInt();
        had_send_open_notice=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
