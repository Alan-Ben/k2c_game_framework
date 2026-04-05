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
public class PlayerConsortChatDialogueBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_dialogue_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "dialogue_id", comment = "对话id")
    private long dialogue_id;

    public static final int FIELD_trigger_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "trigger_time_ms", comment = "触发时间ms")
    private long trigger_time_ms;

    public static final int FIELD_had_draw_reward =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_draw_reward", comment = "是否领取奖励")
    private boolean had_draw_reward;

    public static final int FIELD_detail_info =4;
    @DataBaseField(type = "blob", fieldname = "detail_info", comment = "聊天详情")
    private byte[] detail_info;

    public static final int FIELD_draw_reward_time_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "draw_reward_time_ms", comment = "领取奖励时间ms")
    private long draw_reward_time_ms;

    public PlayerConsortChatDialogueBO() {
        id = 0;
        cid = 0L;
        dialogue_id = 0L;
        trigger_time_ms = 0L;
        had_draw_reward = false;
        detail_info = null;
        draw_reward_time_ms = 0L;
    }

    public PlayerConsortChatDialogueBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        dialogue_id = rs.getLong(3);
        trigger_time_ms = rs.getLong(4);
        had_draw_reward = rs.getBoolean(5);
        detail_info = rs.getBytes(6);
        draw_reward_time_ms = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortChatDialogueBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `dialogue_id`, `trigger_time_ms`, `had_draw_reward`, `detail_info`, `draw_reward_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_consort_chat_dialogue`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(dialogue_id).append("', ");
        strBuf.append("'").append(trigger_time_ms).append("', ");
        strBuf.append("'").append(had_draw_reward ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(draw_reward_time_ms).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(detail_info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_detail_info)) ret.add(detail_info);         return ret;
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

    // 对话id
    public long getDialogueId() { return this.dialogue_id; }
    public void setDialogueId(BM _bm, long dialogue_id) {
        if(dialogue_id==this.dialogue_id) 
            return;
        this.dialogue_id = dialogue_id; 
        markField(_bm, FIELD_dialogue_id); 
    }
    public void saveDialogueId(BM _bm, long dialogue_id) {
        if(dialogue_id==this.dialogue_id) 
            return;
        this.dialogue_id = dialogue_id;
        saveField(_bm, "dialogue_id", dialogue_id);
    }

    // 触发时间ms
    public long getTriggerTimeMs() { return this.trigger_time_ms; }
    public void setTriggerTimeMs(BM _bm, long trigger_time_ms) {
        if(trigger_time_ms==this.trigger_time_ms) 
            return;
        this.trigger_time_ms = trigger_time_ms; 
        markField(_bm, FIELD_trigger_time_ms); 
    }
    public void saveTriggerTimeMs(BM _bm, long trigger_time_ms) {
        if(trigger_time_ms==this.trigger_time_ms) 
            return;
        this.trigger_time_ms = trigger_time_ms;
        saveField(_bm, "trigger_time_ms", trigger_time_ms);
    }

    // 是否领取奖励
    public boolean getHadDrawReward() { return this.had_draw_reward; }
    public void setHadDrawReward(BM _bm, boolean had_draw_reward) {
        if(had_draw_reward==this.had_draw_reward) 
            return;
        this.had_draw_reward = had_draw_reward; 
        markField(_bm, FIELD_had_draw_reward); 
    }
    public void saveHadDrawReward(BM _bm, boolean had_draw_reward) {
        if(had_draw_reward==this.had_draw_reward) 
            return;
        this.had_draw_reward = had_draw_reward;
        saveField(_bm, "had_draw_reward", had_draw_reward ? 1 : 0);
    }

    // 聊天详情
    public byte[] getDetailInfo() { return this.detail_info; }
    public void setDetailInfo(BM _bm, byte[] detail_info) {
        if(detail_info==this.detail_info) 
            return;
        this.detail_info = detail_info; 
        markField(_bm, FIELD_detail_info); 
    }
    public void saveDetailInfo(BM _bm, byte[] detail_info) {
        if(detail_info==this.detail_info) 
            return;
        this.detail_info = detail_info;
        saveFieldBytes(_bm, "detail_info", detail_info);
    }

    // 领取奖励时间ms
    public long getDrawRewardTimeMs() { return this.draw_reward_time_ms; }
    public void setDrawRewardTimeMs(BM _bm, long draw_reward_time_ms) {
        if(draw_reward_time_ms==this.draw_reward_time_ms) 
            return;
        this.draw_reward_time_ms = draw_reward_time_ms; 
        markField(_bm, FIELD_draw_reward_time_ms); 
    }
    public void saveDrawRewardTimeMs(BM _bm, long draw_reward_time_ms) {
        if(draw_reward_time_ms==this.draw_reward_time_ms) 
            return;
        this.draw_reward_time_ms = draw_reward_time_ms;
        saveField(_bm, "draw_reward_time_ms", draw_reward_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `dialogue_id` = '").append(dialogue_id).append("',");
        sBuilder.append(" `trigger_time_ms` = '").append(trigger_time_ms).append("',");
        sBuilder.append(" `had_draw_reward` = '").append(had_draw_reward ? 1 : 0).append("',");
        sBuilder.append(" `detail_info` = ?,");
        sBuilder.append(" `draw_reward_time_ms` = '").append(draw_reward_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_dialogue_id)) sBuilder.append(" `dialogue_id` = '").append(dialogue_id).append("',");
        if(isFieldMarked(FIELD_trigger_time_ms)) sBuilder.append(" `trigger_time_ms` = '").append(trigger_time_ms).append("',");
        if(isFieldMarked(FIELD_had_draw_reward)) sBuilder.append(" `had_draw_reward` = '").append(had_draw_reward ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_detail_info)) sBuilder.append(" `detail_info` = ?,");
        if(isFieldMarked(FIELD_draw_reward_time_ms)) sBuilder.append(" `draw_reward_time_ms` = '").append(draw_reward_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort_chat_dialogue` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`dialogue_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '对话id',"
                + "`trigger_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '触发时间ms',"
                + "`had_draw_reward` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否领取奖励',"
                + "`detail_info` blob NULL COMMENT '聊天详情',"
                + "`draw_reward_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '领取奖励时间ms',"
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
        _size+=8;//dialogue_id
        _size+=8;//trigger_time_ms
        _size+=1;//had_draw_reward
        _size+=2;_size+=detail_info.length;//detail_info
        _size+=8;//draw_reward_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(dialogue_id);
        buff.putLong(trigger_time_ms);
        buff.put((byte)(had_draw_reward?1:0));
        buff.putShort((short)(detail_info == null ? 0 : detail_info.length));if(null != detail_info){buff.put(detail_info);}
        buff.putLong(draw_reward_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        dialogue_id=buff.getLong();
        trigger_time_ms=buff.getLong();
        had_draw_reward=(buff.get()==1);
        int detail_info_count = buff.getShort();if(detail_info_count>0){detail_info = new byte[detail_info_count];buff.get(detail_info);}
        draw_reward_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
