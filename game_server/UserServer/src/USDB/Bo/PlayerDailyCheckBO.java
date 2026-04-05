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
public class PlayerDailyCheckBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_last_check_round_start_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "last_check_round_start_time_ms", comment = "上一次签到轮的开始时间ms")
    private long last_check_round_start_time_ms;

    public static final int FIELD_cur_round_start_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cur_round_start_time_ms", comment = "本轮签到的开始时间ms")
    private long cur_round_start_time_ms;

    public static final int FIELD_not_check_days =3;
    @DataBaseField(type = "int(11)", fieldname = "not_check_days", comment = "没签到的天数")
    private int not_check_days;

    public static final int FIELD_consort_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "consort_id", comment = "情人id")
    private long consort_id;

    public static final int FIELD_dessert_list =5;
    @DataBaseField(type = "varchar(256)", fieldname = "dessert_list", comment = "甜品列表")
    private String dessert_list;

    public static final int FIELD_has_check =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_check", comment = "今天是否已签到")
    private boolean has_check;

    public static final int FIELD_choose_dessert_id =7;
    @DataBaseField(type = "bigint(20)", fieldname = "choose_dessert_id", comment = "选择的甜品id")
    private long choose_dessert_id;

    public static final int FIELD_reward_list =8;
    @DataBaseField(type = "blob", fieldname = "reward_list", comment = "奖励列表")
    private byte[] reward_list;

    public static final int FIELD_next_refresh_time_ms =9;
    @DataBaseField(type = "bigint(20)", fieldname = "next_refresh_time_ms", comment = "下一次刷新时间")
    private long next_refresh_time_ms;

    public static final int FIELD_total_check_days =10;
    @DataBaseField(type = "int(11)", fieldname = "total_check_days", comment = "累计签到天数")
    private int total_check_days;

    public static final int FIELD_rewarded_check_days =11;
    @DataBaseField(type = "int(11)", fieldname = "rewarded_check_days", comment = "已领奖的签到天数")
    private int rewarded_check_days;

    public PlayerDailyCheckBO() {
        id = 0;
        cid = 0L;
        last_check_round_start_time_ms = 0L;
        cur_round_start_time_ms = 0L;
        not_check_days = 0;
        consort_id = 0L;
        dessert_list = "";
        has_check = false;
        choose_dessert_id = 0L;
        reward_list = null;
        next_refresh_time_ms = 0L;
        total_check_days = 0;
        rewarded_check_days = 0;
    }

    public PlayerDailyCheckBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        last_check_round_start_time_ms = rs.getLong(3);
        cur_round_start_time_ms = rs.getLong(4);
        not_check_days = rs.getInt(5);
        consort_id = rs.getLong(6);
        dessert_list = rs.getString(7);
        has_check = rs.getBoolean(8);
        choose_dessert_id = rs.getLong(9);
        reward_list = rs.getBytes(10);
        next_refresh_time_ms = rs.getLong(11);
        total_check_days = rs.getInt(12);
        rewarded_check_days = rs.getInt(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDailyCheckBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `last_check_round_start_time_ms`, `cur_round_start_time_ms`, `not_check_days`, `consort_id`, `dessert_list`, `has_check`, `choose_dessert_id`, `reward_list`, `next_refresh_time_ms`, `total_check_days`, `rewarded_check_days`";
    }

    @Override
    public String getTableName() {
        return "`player_daily_check`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(last_check_round_start_time_ms).append("', ");
        strBuf.append("'").append(cur_round_start_time_ms).append("', ");
        strBuf.append("'").append(not_check_days).append("', ");
        strBuf.append("'").append(consort_id).append("', ");
        strBuf.append("'").append(dessert_list == null ? null : dessert_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(has_check ? 1 : 0).append("', ");
        strBuf.append("'").append(choose_dessert_id).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(next_refresh_time_ms).append("', ");
        strBuf.append("'").append(total_check_days).append("', ");
        strBuf.append("'").append(rewarded_check_days).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(reward_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_reward_list)) ret.add(reward_list);         return ret;
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

    // 上一次签到轮的开始时间ms
    public long getLastCheckRoundStartTimeMs() { return this.last_check_round_start_time_ms; }
    public void setLastCheckRoundStartTimeMs(BM _bm, long last_check_round_start_time_ms) {
        if(last_check_round_start_time_ms==this.last_check_round_start_time_ms) 
            return;
        this.last_check_round_start_time_ms = last_check_round_start_time_ms; 
        markField(_bm, FIELD_last_check_round_start_time_ms); 
    }
    public void saveLastCheckRoundStartTimeMs(BM _bm, long last_check_round_start_time_ms) {
        if(last_check_round_start_time_ms==this.last_check_round_start_time_ms) 
            return;
        this.last_check_round_start_time_ms = last_check_round_start_time_ms;
        saveField(_bm, "last_check_round_start_time_ms", last_check_round_start_time_ms);
    }

    // 本轮签到的开始时间ms
    public long getCurRoundStartTimeMs() { return this.cur_round_start_time_ms; }
    public void setCurRoundStartTimeMs(BM _bm, long cur_round_start_time_ms) {
        if(cur_round_start_time_ms==this.cur_round_start_time_ms) 
            return;
        this.cur_round_start_time_ms = cur_round_start_time_ms; 
        markField(_bm, FIELD_cur_round_start_time_ms); 
    }
    public void saveCurRoundStartTimeMs(BM _bm, long cur_round_start_time_ms) {
        if(cur_round_start_time_ms==this.cur_round_start_time_ms) 
            return;
        this.cur_round_start_time_ms = cur_round_start_time_ms;
        saveField(_bm, "cur_round_start_time_ms", cur_round_start_time_ms);
    }

    // 没签到的天数
    public int getNotCheckDays() { return this.not_check_days; }
    public void setNotCheckDays(BM _bm, int not_check_days) {
        if(not_check_days==this.not_check_days) 
            return;
        this.not_check_days = not_check_days; 
        markField(_bm, FIELD_not_check_days); 
    }
    public void saveNotCheckDays(BM _bm, int not_check_days) {
        if(not_check_days==this.not_check_days) 
            return;
        this.not_check_days = not_check_days;
        saveField(_bm, "not_check_days", not_check_days);
    }

    // 情人id
    public long getConsortId() { return this.consort_id; }
    public void setConsortId(BM _bm, long consort_id) {
        if(consort_id==this.consort_id) 
            return;
        this.consort_id = consort_id; 
        markField(_bm, FIELD_consort_id); 
    }
    public void saveConsortId(BM _bm, long consort_id) {
        if(consort_id==this.consort_id) 
            return;
        this.consort_id = consort_id;
        saveField(_bm, "consort_id", consort_id);
    }

    // 甜品列表
    public String getDessertList() { return this.dessert_list; }
    public void setDessertList(BM _bm, String dessert_list) {
        if(dessert_list.equals(this.dessert_list)) 
            return;
        this.dessert_list = dessert_list; 
        markField(_bm, FIELD_dessert_list); 
    }
    public void saveDessertList(BM _bm, String dessert_list) {
        if(dessert_list.equals(this.dessert_list)) 
            return;
        this.dessert_list = dessert_list;
        saveField(_bm, "dessert_list", dessert_list);
    }

    // 今天是否已签到
    public boolean getHasCheck() { return this.has_check; }
    public void setHasCheck(BM _bm, boolean has_check) {
        if(has_check==this.has_check) 
            return;
        this.has_check = has_check; 
        markField(_bm, FIELD_has_check); 
    }
    public void saveHasCheck(BM _bm, boolean has_check) {
        if(has_check==this.has_check) 
            return;
        this.has_check = has_check;
        saveField(_bm, "has_check", has_check ? 1 : 0);
    }

    // 选择的甜品id
    public long getChooseDessertId() { return this.choose_dessert_id; }
    public void setChooseDessertId(BM _bm, long choose_dessert_id) {
        if(choose_dessert_id==this.choose_dessert_id) 
            return;
        this.choose_dessert_id = choose_dessert_id; 
        markField(_bm, FIELD_choose_dessert_id); 
    }
    public void saveChooseDessertId(BM _bm, long choose_dessert_id) {
        if(choose_dessert_id==this.choose_dessert_id) 
            return;
        this.choose_dessert_id = choose_dessert_id;
        saveField(_bm, "choose_dessert_id", choose_dessert_id);
    }

    // 奖励列表
    public byte[] getRewardList() { return this.reward_list; }
    public void setRewardList(BM _bm, byte[] reward_list) {
        if(reward_list==this.reward_list) 
            return;
        this.reward_list = reward_list; 
        markField(_bm, FIELD_reward_list); 
    }
    public void saveRewardList(BM _bm, byte[] reward_list) {
        if(reward_list==this.reward_list) 
            return;
        this.reward_list = reward_list;
        saveFieldBytes(_bm, "reward_list", reward_list);
    }

    // 下一次刷新时间
    public long getNextRefreshTimeMs() { return this.next_refresh_time_ms; }
    public void setNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms; 
        markField(_bm, FIELD_next_refresh_time_ms); 
    }
    public void saveNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms;
        saveField(_bm, "next_refresh_time_ms", next_refresh_time_ms);
    }

    // 累计签到天数
    public int getTotalCheckDays() { return this.total_check_days; }
    public void setTotalCheckDays(BM _bm, int total_check_days) {
        if(total_check_days==this.total_check_days) 
            return;
        this.total_check_days = total_check_days; 
        markField(_bm, FIELD_total_check_days); 
    }
    public void saveTotalCheckDays(BM _bm, int total_check_days) {
        if(total_check_days==this.total_check_days) 
            return;
        this.total_check_days = total_check_days;
        saveField(_bm, "total_check_days", total_check_days);
    }

    // 已领奖的签到天数
    public int getRewardedCheckDays() { return this.rewarded_check_days; }
    public void setRewardedCheckDays(BM _bm, int rewarded_check_days) {
        if(rewarded_check_days==this.rewarded_check_days) 
            return;
        this.rewarded_check_days = rewarded_check_days; 
        markField(_bm, FIELD_rewarded_check_days); 
    }
    public void saveRewardedCheckDays(BM _bm, int rewarded_check_days) {
        if(rewarded_check_days==this.rewarded_check_days) 
            return;
        this.rewarded_check_days = rewarded_check_days;
        saveField(_bm, "rewarded_check_days", rewarded_check_days);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `last_check_round_start_time_ms` = '").append(last_check_round_start_time_ms).append("',");
        sBuilder.append(" `cur_round_start_time_ms` = '").append(cur_round_start_time_ms).append("',");
        sBuilder.append(" `not_check_days` = '").append(not_check_days).append("',");
        sBuilder.append(" `consort_id` = '").append(consort_id).append("',");
        sBuilder.append(" `dessert_list` = '").append(dessert_list == null ? null : dessert_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `has_check` = '").append(has_check ? 1 : 0).append("',");
        sBuilder.append(" `choose_dessert_id` = '").append(choose_dessert_id).append("',");
        sBuilder.append(" `reward_list` = ?,");
        sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        sBuilder.append(" `total_check_days` = '").append(total_check_days).append("',");
        sBuilder.append(" `rewarded_check_days` = '").append(rewarded_check_days).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_last_check_round_start_time_ms)) sBuilder.append(" `last_check_round_start_time_ms` = '").append(last_check_round_start_time_ms).append("',");
        if(isFieldMarked(FIELD_cur_round_start_time_ms)) sBuilder.append(" `cur_round_start_time_ms` = '").append(cur_round_start_time_ms).append("',");
        if(isFieldMarked(FIELD_not_check_days)) sBuilder.append(" `not_check_days` = '").append(not_check_days).append("',");
        if(isFieldMarked(FIELD_consort_id)) sBuilder.append(" `consort_id` = '").append(consort_id).append("',");
        if(isFieldMarked(FIELD_dessert_list)) sBuilder.append(" `dessert_list` = '").append(dessert_list == null ? null : dessert_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_has_check)) sBuilder.append(" `has_check` = '").append(has_check ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_choose_dessert_id)) sBuilder.append(" `choose_dessert_id` = '").append(choose_dessert_id).append("',");
        if(isFieldMarked(FIELD_reward_list)) sBuilder.append(" `reward_list` = ?,");
        if(isFieldMarked(FIELD_next_refresh_time_ms)) sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        if(isFieldMarked(FIELD_total_check_days)) sBuilder.append(" `total_check_days` = '").append(total_check_days).append("',");
        if(isFieldMarked(FIELD_rewarded_check_days)) sBuilder.append(" `rewarded_check_days` = '").append(rewarded_check_days).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_daily_check` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`last_check_round_start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上一次签到轮的开始时间ms',"
                + "`cur_round_start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本轮签到的开始时间ms',"
                + "`not_check_days` int(11) NOT NULL DEFAULT '0' COMMENT '没签到的天数',"
                + "`consort_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '情人id',"
                + "`dessert_list` varchar(256) NOT NULL DEFAULT '' COMMENT '甜品列表',"
                + "`has_check` tinyint(1) NOT NULL DEFAULT '0' COMMENT '今天是否已签到',"
                + "`choose_dessert_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '选择的甜品id',"
                + "`reward_list` blob NULL COMMENT '奖励列表',"
                + "`next_refresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下一次刷新时间',"
                + "`total_check_days` int(11) NOT NULL DEFAULT '0' COMMENT '累计签到天数',"
                + "`rewarded_check_days` int(11) NOT NULL DEFAULT '0' COMMENT '已领奖的签到天数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家每日签到' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//last_check_round_start_time_ms
        _size+=8;//cur_round_start_time_ms
        _size+=4;//not_check_days
        _size+=8;//consort_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(dessert_list);//dessert_list
        _size+=1;//has_check
        _size+=8;//choose_dessert_id
        _size+=2;_size+=reward_list.length;//reward_list
        _size+=8;//next_refresh_time_ms
        _size+=4;//total_check_days
        _size+=4;//rewarded_check_days
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(last_check_round_start_time_ms);
        buff.putLong(cur_round_start_time_ms);
        buff.putInt(not_check_days);
        buff.putLong(consort_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, dessert_list);
        buff.put((byte)(has_check?1:0));
        buff.putLong(choose_dessert_id);
        buff.putShort((short)(reward_list == null ? 0 : reward_list.length));if(null != reward_list){buff.put(reward_list);}
        buff.putLong(next_refresh_time_ms);
        buff.putInt(total_check_days);
        buff.putInt(rewarded_check_days);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        last_check_round_start_time_ms=buff.getLong();
        cur_round_start_time_ms=buff.getLong();
        not_check_days=buff.getInt();
        consort_id=buff.getLong();
        dessert_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        has_check=(buff.get()==1);
        choose_dessert_id=buff.getLong();
        int reward_list_count = buff.getShort();if(reward_list_count>0){reward_list = new byte[reward_list_count];buff.get(reward_list);}
        next_refresh_time_ms=buff.getLong();
        total_check_days=buff.getInt();
        rewarded_check_days=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
