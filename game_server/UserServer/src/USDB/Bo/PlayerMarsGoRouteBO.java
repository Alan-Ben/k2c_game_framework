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
public class PlayerMarsGoRouteBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_stage =1;
    @DataBaseField(type = "int(11)", fieldname = "stage", comment = "阶段")
    private int stage;

    public static final int FIELD_arrivedMs =2;
    @DataBaseField(type = "bigint(20)", fieldname = "arrivedMs", comment = "第一阶段到达时间（毫秒）")
    private long arrivedMs;

    public static final int FIELD_stageStartMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "stageStartMs", comment = "当前阶段开始时间（毫秒）")
    private long stageStartMs;

    public static final int FIELD_sendDoneMarquee =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "sendDoneMarquee", comment = "发送登录成功后的跑马灯")
    private boolean sendDoneMarquee;

    public static final int FIELD_hasSentStageMsg =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "hasSentStageMsg", comment = "当前阶段是否已发送过留言（阶段变更时清空）")
    private boolean hasSentStageMsg;

    public PlayerMarsGoRouteBO() {
        id = 0;
        cid = 0L;
        stage = 0;
        arrivedMs = 0L;
        stageStartMs = 0L;
        sendDoneMarquee = false;
        hasSentStageMsg = false;
    }

    public PlayerMarsGoRouteBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        stage = rs.getInt(3);
        arrivedMs = rs.getLong(4);
        stageStartMs = rs.getLong(5);
        sendDoneMarquee = rs.getBoolean(6);
        hasSentStageMsg = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsGoRouteBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `stage`, `arrivedMs`, `stageStartMs`, `sendDoneMarquee`, `hasSentStageMsg`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_go_route`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(stage).append("', ");
        strBuf.append("'").append(arrivedMs).append("', ");
        strBuf.append("'").append(stageStartMs).append("', ");
        strBuf.append("'").append(sendDoneMarquee ? 1 : 0).append("', ");
        strBuf.append("'").append(hasSentStageMsg ? 1 : 0).append("', ");
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

    // 阶段
    public int getStage() { return this.stage; }
    public void setStage(BM _bm, int stage) {
        if(stage==this.stage) 
            return;
        this.stage = stage; 
        markField(_bm, FIELD_stage); 
    }
    public void saveStage(BM _bm, int stage) {
        if(stage==this.stage) 
            return;
        this.stage = stage;
        saveField(_bm, "stage", stage);
    }

    // 第一阶段到达时间（毫秒）
    public long getArrivedMs() { return this.arrivedMs; }
    public void setArrivedMs(BM _bm, long arrivedMs) {
        if(arrivedMs==this.arrivedMs) 
            return;
        this.arrivedMs = arrivedMs; 
        markField(_bm, FIELD_arrivedMs); 
    }
    public void saveArrivedMs(BM _bm, long arrivedMs) {
        if(arrivedMs==this.arrivedMs) 
            return;
        this.arrivedMs = arrivedMs;
        saveField(_bm, "arrivedMs", arrivedMs);
    }

    // 当前阶段开始时间（毫秒）
    public long getStageStartMs() { return this.stageStartMs; }
    public void setStageStartMs(BM _bm, long stageStartMs) {
        if(stageStartMs==this.stageStartMs) 
            return;
        this.stageStartMs = stageStartMs; 
        markField(_bm, FIELD_stageStartMs); 
    }
    public void saveStageStartMs(BM _bm, long stageStartMs) {
        if(stageStartMs==this.stageStartMs) 
            return;
        this.stageStartMs = stageStartMs;
        saveField(_bm, "stageStartMs", stageStartMs);
    }

    // 发送登录成功后的跑马灯
    public boolean getSendDoneMarquee() { return this.sendDoneMarquee; }
    public void setSendDoneMarquee(BM _bm, boolean sendDoneMarquee) {
        if(sendDoneMarquee==this.sendDoneMarquee) 
            return;
        this.sendDoneMarquee = sendDoneMarquee; 
        markField(_bm, FIELD_sendDoneMarquee); 
    }
    public void saveSendDoneMarquee(BM _bm, boolean sendDoneMarquee) {
        if(sendDoneMarquee==this.sendDoneMarquee) 
            return;
        this.sendDoneMarquee = sendDoneMarquee;
        saveField(_bm, "sendDoneMarquee", sendDoneMarquee ? 1 : 0);
    }

    // 当前阶段是否已发送过留言（阶段变更时清空）
    public boolean getHasSentStageMsg() { return this.hasSentStageMsg; }
    public void setHasSentStageMsg(BM _bm, boolean hasSentStageMsg) {
        if(hasSentStageMsg==this.hasSentStageMsg) 
            return;
        this.hasSentStageMsg = hasSentStageMsg; 
        markField(_bm, FIELD_hasSentStageMsg); 
    }
    public void saveHasSentStageMsg(BM _bm, boolean hasSentStageMsg) {
        if(hasSentStageMsg==this.hasSentStageMsg) 
            return;
        this.hasSentStageMsg = hasSentStageMsg;
        saveField(_bm, "hasSentStageMsg", hasSentStageMsg ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `stage` = '").append(stage).append("',");
        sBuilder.append(" `arrivedMs` = '").append(arrivedMs).append("',");
        sBuilder.append(" `stageStartMs` = '").append(stageStartMs).append("',");
        sBuilder.append(" `sendDoneMarquee` = '").append(sendDoneMarquee ? 1 : 0).append("',");
        sBuilder.append(" `hasSentStageMsg` = '").append(hasSentStageMsg ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_stage)) sBuilder.append(" `stage` = '").append(stage).append("',");
        if(isFieldMarked(FIELD_arrivedMs)) sBuilder.append(" `arrivedMs` = '").append(arrivedMs).append("',");
        if(isFieldMarked(FIELD_stageStartMs)) sBuilder.append(" `stageStartMs` = '").append(stageStartMs).append("',");
        if(isFieldMarked(FIELD_sendDoneMarquee)) sBuilder.append(" `sendDoneMarquee` = '").append(sendDoneMarquee ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_hasSentStageMsg)) sBuilder.append(" `hasSentStageMsg` = '").append(hasSentStageMsg ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_go_route` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`stage` int(11) NOT NULL DEFAULT '0' COMMENT '阶段',"
                + "`arrivedMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '第一阶段到达时间（毫秒）',"
                + "`stageStartMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前阶段开始时间（毫秒）',"
                + "`sendDoneMarquee` tinyint(1) NOT NULL DEFAULT '0' COMMENT '发送登录成功后的跑马灯',"
                + "`hasSentStageMsg` tinyint(1) NOT NULL DEFAULT '0' COMMENT '当前阶段是否已发送过留言（阶段变更时清空）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-玩家前往火星数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//stage
        _size+=8;//arrivedMs
        _size+=8;//stageStartMs
        _size+=1;//sendDoneMarquee
        _size+=1;//hasSentStageMsg
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(stage);
        buff.putLong(arrivedMs);
        buff.putLong(stageStartMs);
        buff.put((byte)(sendDoneMarquee?1:0));
        buff.put((byte)(hasSentStageMsg?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        stage=buff.getInt();
        arrivedMs=buff.getLong();
        stageStartMs=buff.getLong();
        sendDoneMarquee=(buff.get()==1);
        hasSentStageMsg=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
