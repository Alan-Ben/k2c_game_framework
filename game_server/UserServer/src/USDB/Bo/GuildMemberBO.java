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
public class GuildMemberBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public static final int FIELD_position =2;
    @DataBaseField(type = "int(11)", fieldname = "position", comment = "职位")
    private int position;

    public static final int FIELD_total_contribute =3;
    @DataBaseField(type = "bigint(20)", fieldname = "total_contribute", comment = "总贡献度")
    private long total_contribute;

    public static final int FIELD_total_deal_entrust_num =4;
    @DataBaseField(type = "int(11)", fieldname = "total_deal_entrust_num", comment = "总处理委托数")
    private int total_deal_entrust_num;

    public static final int FIELD_is_online =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_online", comment = "上次在线时间")
    private boolean is_online;

    public static final int FIELD_last_report_online_timestamp =6;
    @DataBaseField(type = "bigint(20)", fieldname = "last_report_online_timestamp", comment = "上次上报在线时间")
    private long last_report_online_timestamp;

    public static final int FIELD_last_reset_data_timestamp =7;
    @DataBaseField(type = "bigint(20)", fieldname = "last_reset_data_timestamp", comment = "上次重置数据时间")
    private long last_reset_data_timestamp;

    public static final int FIELD_past_day_contribute =8;
    @DataBaseField(type = "blob", fieldname = "past_day_contribute", comment = "历史贡献度记录")
    private byte[] past_day_contribute;

    public static final int FIELD_day_deal_entrust_num =9;
    @DataBaseField(type = "int(11)", fieldname = "day_deal_entrust_num", comment = "当日处理委托数")
    private int day_deal_entrust_num;

    public static final int FIELD_last_active_guild_box =10;
    @DataBaseField(type = "bigint(20)", fieldname = "last_active_guild_box", comment = "最后一个被标记的宝箱实例ID")
    private long last_active_guild_box;

    public static final int FIELD_last_free_guild_box =11;
    @DataBaseField(type = "bigint(20)", fieldname = "last_free_guild_box", comment = "最后一个被标记的宝箱实例ID")
    private long last_free_guild_box;

    public static final int FIELD_last_gift_guild_box =12;
    @DataBaseField(type = "bigint(20)", fieldname = "last_gift_guild_box", comment = "最后一个被标记的宝箱实例ID")
    private long last_gift_guild_box;

    public static final int FIELD_is_guild_box_share_anonymous =13;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_guild_box_share_anonymous", comment = "分享宝箱匿名")
    private boolean is_guild_box_share_anonymous;

    public GuildMemberBO() {
        id = 0;
        guild_id = 0L;
        cid = 0L;
        position = 0;
        total_contribute = 0L;
        total_deal_entrust_num = 0;
        is_online = false;
        last_report_online_timestamp = 0L;
        last_reset_data_timestamp = 0L;
        past_day_contribute = null;
        day_deal_entrust_num = 0;
        last_active_guild_box = 0L;
        last_free_guild_box = 0L;
        last_gift_guild_box = 0L;
        is_guild_box_share_anonymous = false;
    }

    public GuildMemberBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        cid = rs.getLong(3);
        position = rs.getInt(4);
        total_contribute = rs.getLong(5);
        total_deal_entrust_num = rs.getInt(6);
        is_online = rs.getBoolean(7);
        last_report_online_timestamp = rs.getLong(8);
        last_reset_data_timestamp = rs.getLong(9);
        past_day_contribute = rs.getBytes(10);
        day_deal_entrust_num = rs.getInt(11);
        last_active_guild_box = rs.getLong(12);
        last_free_guild_box = rs.getLong(13);
        last_gift_guild_box = rs.getLong(14);
        is_guild_box_share_anonymous = rs.getBoolean(15);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildMemberBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `cid`, `position`, `total_contribute`, `total_deal_entrust_num`, `is_online`, `last_report_online_timestamp`, `last_reset_data_timestamp`, `past_day_contribute`, `day_deal_entrust_num`, `last_active_guild_box`, `last_free_guild_box`, `last_gift_guild_box`, `is_guild_box_share_anonymous`";
    }

    @Override
    public String getTableName() {
        return "`guild_member`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(position).append("', ");
        strBuf.append("'").append(total_contribute).append("', ");
        strBuf.append("'").append(total_deal_entrust_num).append("', ");
        strBuf.append("'").append(is_online ? 1 : 0).append("', ");
        strBuf.append("'").append(last_report_online_timestamp).append("', ");
        strBuf.append("'").append(last_reset_data_timestamp).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(day_deal_entrust_num).append("', ");
        strBuf.append("'").append(last_active_guild_box).append("', ");
        strBuf.append("'").append(last_free_guild_box).append("', ");
        strBuf.append("'").append(last_gift_guild_box).append("', ");
        strBuf.append("'").append(is_guild_box_share_anonymous ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(past_day_contribute);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_past_day_contribute)) ret.add(past_day_contribute);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 联盟id
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
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

    // 职位
    public int getPosition() { return this.position; }
    public void setPosition(BM _bm, int position) {
        if(position==this.position) 
            return;
        this.position = position; 
        markField(_bm, FIELD_position); 
    }
    public void savePosition(BM _bm, int position) {
        if(position==this.position) 
            return;
        this.position = position;
        saveField(_bm, "position", position);
    }

    // 总贡献度
    public long getTotalContribute() { return this.total_contribute; }
    public void setTotalContribute(BM _bm, long total_contribute) {
        if(total_contribute==this.total_contribute) 
            return;
        this.total_contribute = total_contribute; 
        markField(_bm, FIELD_total_contribute); 
    }
    public void saveTotalContribute(BM _bm, long total_contribute) {
        if(total_contribute==this.total_contribute) 
            return;
        this.total_contribute = total_contribute;
        saveField(_bm, "total_contribute", total_contribute);
    }

    // 总处理委托数
    public int getTotalDealEntrustNum() { return this.total_deal_entrust_num; }
    public void setTotalDealEntrustNum(BM _bm, int total_deal_entrust_num) {
        if(total_deal_entrust_num==this.total_deal_entrust_num) 
            return;
        this.total_deal_entrust_num = total_deal_entrust_num; 
        markField(_bm, FIELD_total_deal_entrust_num); 
    }
    public void saveTotalDealEntrustNum(BM _bm, int total_deal_entrust_num) {
        if(total_deal_entrust_num==this.total_deal_entrust_num) 
            return;
        this.total_deal_entrust_num = total_deal_entrust_num;
        saveField(_bm, "total_deal_entrust_num", total_deal_entrust_num);
    }

    // 上次在线时间
    public boolean getIsOnline() { return this.is_online; }
    public void setIsOnline(BM _bm, boolean is_online) {
        if(is_online==this.is_online) 
            return;
        this.is_online = is_online; 
        markField(_bm, FIELD_is_online); 
    }
    public void saveIsOnline(BM _bm, boolean is_online) {
        if(is_online==this.is_online) 
            return;
        this.is_online = is_online;
        saveField(_bm, "is_online", is_online ? 1 : 0);
    }

    // 上次上报在线时间
    public long getLastReportOnlineTimestamp() { return this.last_report_online_timestamp; }
    public void setLastReportOnlineTimestamp(BM _bm, long last_report_online_timestamp) {
        if(last_report_online_timestamp==this.last_report_online_timestamp) 
            return;
        this.last_report_online_timestamp = last_report_online_timestamp; 
        markField(_bm, FIELD_last_report_online_timestamp); 
    }
    public void saveLastReportOnlineTimestamp(BM _bm, long last_report_online_timestamp) {
        if(last_report_online_timestamp==this.last_report_online_timestamp) 
            return;
        this.last_report_online_timestamp = last_report_online_timestamp;
        saveField(_bm, "last_report_online_timestamp", last_report_online_timestamp);
    }

    // 上次重置数据时间
    public long getLastResetDataTimestamp() { return this.last_reset_data_timestamp; }
    public void setLastResetDataTimestamp(BM _bm, long last_reset_data_timestamp) {
        if(last_reset_data_timestamp==this.last_reset_data_timestamp) 
            return;
        this.last_reset_data_timestamp = last_reset_data_timestamp; 
        markField(_bm, FIELD_last_reset_data_timestamp); 
    }
    public void saveLastResetDataTimestamp(BM _bm, long last_reset_data_timestamp) {
        if(last_reset_data_timestamp==this.last_reset_data_timestamp) 
            return;
        this.last_reset_data_timestamp = last_reset_data_timestamp;
        saveField(_bm, "last_reset_data_timestamp", last_reset_data_timestamp);
    }

    // 历史贡献度记录
    public byte[] getPastDayContribute() { return this.past_day_contribute; }
    public void setPastDayContribute(BM _bm, byte[] past_day_contribute) {
        if(past_day_contribute==this.past_day_contribute) 
            return;
        this.past_day_contribute = past_day_contribute; 
        markField(_bm, FIELD_past_day_contribute); 
    }
    public void savePastDayContribute(BM _bm, byte[] past_day_contribute) {
        if(past_day_contribute==this.past_day_contribute) 
            return;
        this.past_day_contribute = past_day_contribute;
        saveFieldBytes(_bm, "past_day_contribute", past_day_contribute);
    }

    // 当日处理委托数
    public int getDayDealEntrustNum() { return this.day_deal_entrust_num; }
    public void setDayDealEntrustNum(BM _bm, int day_deal_entrust_num) {
        if(day_deal_entrust_num==this.day_deal_entrust_num) 
            return;
        this.day_deal_entrust_num = day_deal_entrust_num; 
        markField(_bm, FIELD_day_deal_entrust_num); 
    }
    public void saveDayDealEntrustNum(BM _bm, int day_deal_entrust_num) {
        if(day_deal_entrust_num==this.day_deal_entrust_num) 
            return;
        this.day_deal_entrust_num = day_deal_entrust_num;
        saveField(_bm, "day_deal_entrust_num", day_deal_entrust_num);
    }

    // 最后一个被标记的宝箱实例ID
    public long getLastActiveGuildBox() { return this.last_active_guild_box; }
    public void setLastActiveGuildBox(BM _bm, long last_active_guild_box) {
        if(last_active_guild_box==this.last_active_guild_box) 
            return;
        this.last_active_guild_box = last_active_guild_box; 
        markField(_bm, FIELD_last_active_guild_box); 
    }
    public void saveLastActiveGuildBox(BM _bm, long last_active_guild_box) {
        if(last_active_guild_box==this.last_active_guild_box) 
            return;
        this.last_active_guild_box = last_active_guild_box;
        saveField(_bm, "last_active_guild_box", last_active_guild_box);
    }

    // 最后一个被标记的宝箱实例ID
    public long getLastFreeGuildBox() { return this.last_free_guild_box; }
    public void setLastFreeGuildBox(BM _bm, long last_free_guild_box) {
        if(last_free_guild_box==this.last_free_guild_box) 
            return;
        this.last_free_guild_box = last_free_guild_box; 
        markField(_bm, FIELD_last_free_guild_box); 
    }
    public void saveLastFreeGuildBox(BM _bm, long last_free_guild_box) {
        if(last_free_guild_box==this.last_free_guild_box) 
            return;
        this.last_free_guild_box = last_free_guild_box;
        saveField(_bm, "last_free_guild_box", last_free_guild_box);
    }

    // 最后一个被标记的宝箱实例ID
    public long getLastGiftGuildBox() { return this.last_gift_guild_box; }
    public void setLastGiftGuildBox(BM _bm, long last_gift_guild_box) {
        if(last_gift_guild_box==this.last_gift_guild_box) 
            return;
        this.last_gift_guild_box = last_gift_guild_box; 
        markField(_bm, FIELD_last_gift_guild_box); 
    }
    public void saveLastGiftGuildBox(BM _bm, long last_gift_guild_box) {
        if(last_gift_guild_box==this.last_gift_guild_box) 
            return;
        this.last_gift_guild_box = last_gift_guild_box;
        saveField(_bm, "last_gift_guild_box", last_gift_guild_box);
    }

    // 分享宝箱匿名
    public boolean getIsGuildBoxShareAnonymous() { return this.is_guild_box_share_anonymous; }
    public void setIsGuildBoxShareAnonymous(BM _bm, boolean is_guild_box_share_anonymous) {
        if(is_guild_box_share_anonymous==this.is_guild_box_share_anonymous) 
            return;
        this.is_guild_box_share_anonymous = is_guild_box_share_anonymous; 
        markField(_bm, FIELD_is_guild_box_share_anonymous); 
    }
    public void saveIsGuildBoxShareAnonymous(BM _bm, boolean is_guild_box_share_anonymous) {
        if(is_guild_box_share_anonymous==this.is_guild_box_share_anonymous) 
            return;
        this.is_guild_box_share_anonymous = is_guild_box_share_anonymous;
        saveField(_bm, "is_guild_box_share_anonymous", is_guild_box_share_anonymous ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `position` = '").append(position).append("',");
        sBuilder.append(" `total_contribute` = '").append(total_contribute).append("',");
        sBuilder.append(" `total_deal_entrust_num` = '").append(total_deal_entrust_num).append("',");
        sBuilder.append(" `is_online` = '").append(is_online ? 1 : 0).append("',");
        sBuilder.append(" `last_report_online_timestamp` = '").append(last_report_online_timestamp).append("',");
        sBuilder.append(" `last_reset_data_timestamp` = '").append(last_reset_data_timestamp).append("',");
        sBuilder.append(" `past_day_contribute` = ?,");
        sBuilder.append(" `day_deal_entrust_num` = '").append(day_deal_entrust_num).append("',");
        sBuilder.append(" `last_active_guild_box` = '").append(last_active_guild_box).append("',");
        sBuilder.append(" `last_free_guild_box` = '").append(last_free_guild_box).append("',");
        sBuilder.append(" `last_gift_guild_box` = '").append(last_gift_guild_box).append("',");
        sBuilder.append(" `is_guild_box_share_anonymous` = '").append(is_guild_box_share_anonymous ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_position)) sBuilder.append(" `position` = '").append(position).append("',");
        if(isFieldMarked(FIELD_total_contribute)) sBuilder.append(" `total_contribute` = '").append(total_contribute).append("',");
        if(isFieldMarked(FIELD_total_deal_entrust_num)) sBuilder.append(" `total_deal_entrust_num` = '").append(total_deal_entrust_num).append("',");
        if(isFieldMarked(FIELD_is_online)) sBuilder.append(" `is_online` = '").append(is_online ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_last_report_online_timestamp)) sBuilder.append(" `last_report_online_timestamp` = '").append(last_report_online_timestamp).append("',");
        if(isFieldMarked(FIELD_last_reset_data_timestamp)) sBuilder.append(" `last_reset_data_timestamp` = '").append(last_reset_data_timestamp).append("',");
        if(isFieldMarked(FIELD_past_day_contribute)) sBuilder.append(" `past_day_contribute` = ?,");
        if(isFieldMarked(FIELD_day_deal_entrust_num)) sBuilder.append(" `day_deal_entrust_num` = '").append(day_deal_entrust_num).append("',");
        if(isFieldMarked(FIELD_last_active_guild_box)) sBuilder.append(" `last_active_guild_box` = '").append(last_active_guild_box).append("',");
        if(isFieldMarked(FIELD_last_free_guild_box)) sBuilder.append(" `last_free_guild_box` = '").append(last_free_guild_box).append("',");
        if(isFieldMarked(FIELD_last_gift_guild_box)) sBuilder.append(" `last_gift_guild_box` = '").append(last_gift_guild_box).append("',");
        if(isFieldMarked(FIELD_is_guild_box_share_anonymous)) sBuilder.append(" `is_guild_box_share_anonymous` = '").append(is_guild_box_share_anonymous ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_member` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`position` int(11) NOT NULL DEFAULT '0' COMMENT '职位',"
                + "`total_contribute` bigint(20) NOT NULL DEFAULT '0' COMMENT '总贡献度',"
                + "`total_deal_entrust_num` int(11) NOT NULL DEFAULT '0' COMMENT '总处理委托数',"
                + "`is_online` tinyint(1) NOT NULL DEFAULT '0' COMMENT '上次在线时间',"
                + "`last_report_online_timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次上报在线时间',"
                + "`last_reset_data_timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次重置数据时间',"
                + "`past_day_contribute` blob NULL COMMENT '历史贡献度记录',"
                + "`day_deal_entrust_num` int(11) NOT NULL DEFAULT '0' COMMENT '当日处理委托数',"
                + "`last_active_guild_box` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一个被标记的宝箱实例ID',"
                + "`last_free_guild_box` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一个被标记的宝箱实例ID',"
                + "`last_gift_guild_box` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一个被标记的宝箱实例ID',"
                + "`is_guild_box_share_anonymous` tinyint(1) NOT NULL DEFAULT '0' COMMENT '分享宝箱匿名',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟成员数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=8;//cid
        _size+=4;//position
        _size+=8;//total_contribute
        _size+=4;//total_deal_entrust_num
        _size+=1;//is_online
        _size+=8;//last_report_online_timestamp
        _size+=8;//last_reset_data_timestamp
        _size+=2;_size+=past_day_contribute.length;//past_day_contribute
        _size+=4;//day_deal_entrust_num
        _size+=8;//last_active_guild_box
        _size+=8;//last_free_guild_box
        _size+=8;//last_gift_guild_box
        _size+=1;//is_guild_box_share_anonymous
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(cid);
        buff.putInt(position);
        buff.putLong(total_contribute);
        buff.putInt(total_deal_entrust_num);
        buff.put((byte)(is_online?1:0));
        buff.putLong(last_report_online_timestamp);
        buff.putLong(last_reset_data_timestamp);
        buff.putShort((short)(past_day_contribute == null ? 0 : past_day_contribute.length));if(null != past_day_contribute){buff.put(past_day_contribute);}
        buff.putInt(day_deal_entrust_num);
        buff.putLong(last_active_guild_box);
        buff.putLong(last_free_guild_box);
        buff.putLong(last_gift_guild_box);
        buff.put((byte)(is_guild_box_share_anonymous?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        cid=buff.getLong();
        position=buff.getInt();
        total_contribute=buff.getLong();
        total_deal_entrust_num=buff.getInt();
        is_online=(buff.get()==1);
        last_report_online_timestamp=buff.getLong();
        last_reset_data_timestamp=buff.getLong();
        int past_day_contribute_count = buff.getShort();if(past_day_contribute_count>0){past_day_contribute = new byte[past_day_contribute_count];buff.get(past_day_contribute);}
        day_deal_entrust_num=buff.getInt();
        last_active_guild_box=buff.getLong();
        last_free_guild_box=buff.getLong();
        last_gift_guild_box=buff.getLong();
        is_guild_box_share_anonymous=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
