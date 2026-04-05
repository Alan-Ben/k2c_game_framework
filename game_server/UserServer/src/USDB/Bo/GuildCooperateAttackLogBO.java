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
public class GuildCooperateAttackLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "公会ID")
    private long guild_id;

    public static final int FIELD_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "time_ms", comment = "攻击时间毫秒")
    private long time_ms;

    public static final int FIELD_player_name =2;
    @DataBaseField(type = "varchar(256)", fieldname = "player_name", comment = "攻击者姓名")
    private String player_name;

    public static final int FIELD_pos_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "pos_id", comment = "奖励据点ID（配表ID）")
    private long pos_id;

    public static final int FIELD_attr_type =4;
    @DataBaseField(type = "int(11)", fieldname = "attr_type", comment = "攻击的属性类型")
    private int attr_type;

    public static final int FIELD_attack_hp =5;
    @DataBaseField(type = "bigint(20)", fieldname = "attack_hp", comment = "攻击造成的伤害")
    private long attack_hp;

    public GuildCooperateAttackLogBO() {
        id = 0;
        guild_id = 0L;
        time_ms = 0L;
        player_name = "";
        pos_id = 0L;
        attr_type = 0;
        attack_hp = 0L;
    }

    public GuildCooperateAttackLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        time_ms = rs.getLong(3);
        player_name = rs.getString(4);
        pos_id = rs.getLong(5);
        attr_type = rs.getInt(6);
        attack_hp = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildCooperateAttackLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `time_ms`, `player_name`, `pos_id`, `attr_type`, `attack_hp`";
    }

    @Override
    public String getTableName() {
        return "`guild_cooperate_attack_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(time_ms).append("', ");
        strBuf.append("'").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pos_id).append("', ");
        strBuf.append("'").append(attr_type).append("', ");
        strBuf.append("'").append(attack_hp).append("', ");
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

    // 公会ID
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

    // 攻击时间毫秒
    public long getTimeMs() { return this.time_ms; }
    public void setTimeMs(BM _bm, long time_ms) {
        if(time_ms==this.time_ms) 
            return;
        this.time_ms = time_ms; 
        markField(_bm, FIELD_time_ms); 
    }
    public void saveTimeMs(BM _bm, long time_ms) {
        if(time_ms==this.time_ms) 
            return;
        this.time_ms = time_ms;
        saveField(_bm, "time_ms", time_ms);
    }

    // 攻击者姓名
    public String getPlayerName() { return this.player_name; }
    public void setPlayerName(BM _bm, String player_name) {
        if(player_name.equals(this.player_name)) 
            return;
        this.player_name = player_name; 
        markField(_bm, FIELD_player_name); 
    }
    public void savePlayerName(BM _bm, String player_name) {
        if(player_name.equals(this.player_name)) 
            return;
        this.player_name = player_name;
        saveField(_bm, "player_name", player_name);
    }

    // 奖励据点ID（配表ID）
    public long getPosId() { return this.pos_id; }
    public void setPosId(BM _bm, long pos_id) {
        if(pos_id==this.pos_id) 
            return;
        this.pos_id = pos_id; 
        markField(_bm, FIELD_pos_id); 
    }
    public void savePosId(BM _bm, long pos_id) {
        if(pos_id==this.pos_id) 
            return;
        this.pos_id = pos_id;
        saveField(_bm, "pos_id", pos_id);
    }

    // 攻击的属性类型
    public int getAttrType() { return this.attr_type; }
    public void setAttrType(BM _bm, int attr_type) {
        if(attr_type==this.attr_type) 
            return;
        this.attr_type = attr_type; 
        markField(_bm, FIELD_attr_type); 
    }
    public void saveAttrType(BM _bm, int attr_type) {
        if(attr_type==this.attr_type) 
            return;
        this.attr_type = attr_type;
        saveField(_bm, "attr_type", attr_type);
    }

    // 攻击造成的伤害
    public long getAttackHp() { return this.attack_hp; }
    public void setAttackHp(BM _bm, long attack_hp) {
        if(attack_hp==this.attack_hp) 
            return;
        this.attack_hp = attack_hp; 
        markField(_bm, FIELD_attack_hp); 
    }
    public void saveAttackHp(BM _bm, long attack_hp) {
        if(attack_hp==this.attack_hp) 
            return;
        this.attack_hp = attack_hp;
        saveField(_bm, "attack_hp", attack_hp);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `time_ms` = '").append(time_ms).append("',");
        sBuilder.append(" `player_name` = '").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pos_id` = '").append(pos_id).append("',");
        sBuilder.append(" `attr_type` = '").append(attr_type).append("',");
        sBuilder.append(" `attack_hp` = '").append(attack_hp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_time_ms)) sBuilder.append(" `time_ms` = '").append(time_ms).append("',");
        if(isFieldMarked(FIELD_player_name)) sBuilder.append(" `player_name` = '").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pos_id)) sBuilder.append(" `pos_id` = '").append(pos_id).append("',");
        if(isFieldMarked(FIELD_attr_type)) sBuilder.append(" `attr_type` = '").append(attr_type).append("',");
        if(isFieldMarked(FIELD_attack_hp)) sBuilder.append(" `attack_hp` = '").append(attack_hp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_cooperate_attack_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会ID',"
                + "`time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击时间毫秒',"
                + "`player_name` varchar(256) NOT NULL DEFAULT '' COMMENT '攻击者姓名',"
                + "`pos_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奖励据点ID（配表ID）',"
                + "`attr_type` int(11) NOT NULL DEFAULT '0' COMMENT '攻击的属性类型',"
                + "`attack_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击造成的伤害',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟协作攻击日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//time_ms
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(player_name);//player_name
        _size+=8;//pos_id
        _size+=4;//attr_type
        _size+=8;//attack_hp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(time_ms);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, player_name);
        buff.putLong(pos_id);
        buff.putInt(attr_type);
        buff.putLong(attack_hp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        time_ms=buff.getLong();
        player_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pos_id=buff.getLong();
        attr_type=buff.getInt();
        attack_hp=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
