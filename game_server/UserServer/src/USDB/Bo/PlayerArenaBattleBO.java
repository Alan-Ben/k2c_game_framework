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
public class PlayerArenaBattleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_round =1;
    @DataBaseField(type = "int(11)", fieldname = "round", comment = "当前回合数")
    private int round;

    public static final int FIELD_had_buy_buff =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_buy_buff", comment = "本轮是否已购买buff")
    private boolean had_buy_buff;

    public static final int FIELD_buff_list =3;
    @DataBaseField(type = "blob", fieldname = "buff_list", comment = "临时增益列表")
    private byte[] buff_list;

    public static final int FIELD_hero_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_id", comment = "我方大臣id")
    private long hero_id;

    public static final int FIELD_base_power =5;
    @DataBaseField(type = "bigint(20)", fieldname = "base_power", comment = "我方基础实力")
    private long base_power;

    public static final int FIELD_deducted_hp =6;
    @DataBaseField(type = "bigint(20)", fieldname = "deducted_hp", comment = "我方被扣除血量")
    private long deducted_hp;

    public static final int FIELD_select_attack_item_id =7;
    @DataBaseField(type = "bigint(20)", fieldname = "select_attack_item_id", comment = "选择攻击道具id")
    private long select_attack_item_id;

    public static final int FIELD_attack_type =8;
    @DataBaseField(type = "int(11)", fieldname = "attack_type", comment = "攻击类型")
    private int attack_type;

    public static final int FIELD_opponent_cid =9;
    @DataBaseField(type = "bigint(20)", fieldname = "opponent_cid", comment = "对手CID")
    private long opponent_cid;

    public static final int FIELD_had_defeat_hero_list =10;
    @DataBaseField(type = "blob", fieldname = "had_defeat_hero_list", comment = "已经击败的大臣列表")
    private byte[] had_defeat_hero_list;

    public static final int FIELD_opponent_hero_list =11;
    @DataBaseField(type = "blob", fieldname = "opponent_hero_list", comment = "对手大臣列表")
    private byte[] opponent_hero_list;

    public static final int FIELD_can_attack_hero_list =12;
    @DataBaseField(type = "blob", fieldname = "can_attack_hero_list", comment = "本回合可攻击英雄列表")
    private byte[] can_attack_hero_list;

    public static final int FIELD_opponent_power =13;
    @DataBaseField(type = "bigint(20)", fieldname = "opponent_power", comment = "对手战力")
    private long opponent_power;

    public static final int FIELD_bot_name =14;
    @DataBaseField(type = "varchar(256)", fieldname = "bot_name", comment = "机器人名字")
    private String bot_name;

    public static final int FIELD_is_bot =15;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_bot", comment = "是否是机器人")
    private boolean is_bot;

    public PlayerArenaBattleBO() {
        id = 0;
        cid = 0L;
        round = 0;
        had_buy_buff = false;
        buff_list = null;
        hero_id = 0L;
        base_power = 0L;
        deducted_hp = 0L;
        select_attack_item_id = 0L;
        attack_type = 0;
        opponent_cid = 0L;
        had_defeat_hero_list = null;
        opponent_hero_list = null;
        can_attack_hero_list = null;
        opponent_power = 0L;
        bot_name = "";
        is_bot = false;
    }

    public PlayerArenaBattleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        round = rs.getInt(3);
        had_buy_buff = rs.getBoolean(4);
        buff_list = rs.getBytes(5);
        hero_id = rs.getLong(6);
        base_power = rs.getLong(7);
        deducted_hp = rs.getLong(8);
        select_attack_item_id = rs.getLong(9);
        attack_type = rs.getInt(10);
        opponent_cid = rs.getLong(11);
        had_defeat_hero_list = rs.getBytes(12);
        opponent_hero_list = rs.getBytes(13);
        can_attack_hero_list = rs.getBytes(14);
        opponent_power = rs.getLong(15);
        bot_name = rs.getString(16);
        is_bot = rs.getBoolean(17);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerArenaBattleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `round`, `had_buy_buff`, `buff_list`, `hero_id`, `base_power`, `deducted_hp`, `select_attack_item_id`, `attack_type`, `opponent_cid`, `had_defeat_hero_list`, `opponent_hero_list`, `can_attack_hero_list`, `opponent_power`, `bot_name`, `is_bot`";
    }

    @Override
    public String getTableName() {
        return "`player_arena_battle`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(round).append("', ");
        strBuf.append("'").append(had_buy_buff ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(base_power).append("', ");
        strBuf.append("'").append(deducted_hp).append("', ");
        strBuf.append("'").append(select_attack_item_id).append("', ");
        strBuf.append("'").append(attack_type).append("', ");
        strBuf.append("'").append(opponent_cid).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(opponent_power).append("', ");
        strBuf.append("'").append(bot_name == null ? null : bot_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(is_bot ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(buff_list); 
        ret.add(had_defeat_hero_list); 
        ret.add(opponent_hero_list); 
        ret.add(can_attack_hero_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_buff_list)) ret.add(buff_list); 
        if(isFieldMarked(FIELD_had_defeat_hero_list)) ret.add(had_defeat_hero_list); 
        if(isFieldMarked(FIELD_opponent_hero_list)) ret.add(opponent_hero_list); 
        if(isFieldMarked(FIELD_can_attack_hero_list)) ret.add(can_attack_hero_list);         return ret;
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

    // 当前回合数
    public int getRound() { return this.round; }
    public void setRound(BM _bm, int round) {
        if(round==this.round) 
            return;
        this.round = round; 
        markField(_bm, FIELD_round); 
    }
    public void saveRound(BM _bm, int round) {
        if(round==this.round) 
            return;
        this.round = round;
        saveField(_bm, "round", round);
    }

    // 本轮是否已购买buff
    public boolean getHadBuyBuff() { return this.had_buy_buff; }
    public void setHadBuyBuff(BM _bm, boolean had_buy_buff) {
        if(had_buy_buff==this.had_buy_buff) 
            return;
        this.had_buy_buff = had_buy_buff; 
        markField(_bm, FIELD_had_buy_buff); 
    }
    public void saveHadBuyBuff(BM _bm, boolean had_buy_buff) {
        if(had_buy_buff==this.had_buy_buff) 
            return;
        this.had_buy_buff = had_buy_buff;
        saveField(_bm, "had_buy_buff", had_buy_buff ? 1 : 0);
    }

    // 临时增益列表
    public byte[] getBuffList() { return this.buff_list; }
    public void setBuffList(BM _bm, byte[] buff_list) {
        if(buff_list==this.buff_list) 
            return;
        this.buff_list = buff_list; 
        markField(_bm, FIELD_buff_list); 
    }
    public void saveBuffList(BM _bm, byte[] buff_list) {
        if(buff_list==this.buff_list) 
            return;
        this.buff_list = buff_list;
        saveFieldBytes(_bm, "buff_list", buff_list);
    }

    // 我方大臣id
    public long getHeroId() { return this.hero_id; }
    public void setHeroId(BM _bm, long hero_id) {
        if(hero_id==this.hero_id) 
            return;
        this.hero_id = hero_id; 
        markField(_bm, FIELD_hero_id); 
    }
    public void saveHeroId(BM _bm, long hero_id) {
        if(hero_id==this.hero_id) 
            return;
        this.hero_id = hero_id;
        saveField(_bm, "hero_id", hero_id);
    }

    // 我方基础实力
    public long getBasePower() { return this.base_power; }
    public void setBasePower(BM _bm, long base_power) {
        if(base_power==this.base_power) 
            return;
        this.base_power = base_power; 
        markField(_bm, FIELD_base_power); 
    }
    public void saveBasePower(BM _bm, long base_power) {
        if(base_power==this.base_power) 
            return;
        this.base_power = base_power;
        saveField(_bm, "base_power", base_power);
    }

    // 我方被扣除血量
    public long getDeductedHp() { return this.deducted_hp; }
    public void setDeductedHp(BM _bm, long deducted_hp) {
        if(deducted_hp==this.deducted_hp) 
            return;
        this.deducted_hp = deducted_hp; 
        markField(_bm, FIELD_deducted_hp); 
    }
    public void saveDeductedHp(BM _bm, long deducted_hp) {
        if(deducted_hp==this.deducted_hp) 
            return;
        this.deducted_hp = deducted_hp;
        saveField(_bm, "deducted_hp", deducted_hp);
    }

    // 选择攻击道具id
    public long getSelectAttackItemId() { return this.select_attack_item_id; }
    public void setSelectAttackItemId(BM _bm, long select_attack_item_id) {
        if(select_attack_item_id==this.select_attack_item_id) 
            return;
        this.select_attack_item_id = select_attack_item_id; 
        markField(_bm, FIELD_select_attack_item_id); 
    }
    public void saveSelectAttackItemId(BM _bm, long select_attack_item_id) {
        if(select_attack_item_id==this.select_attack_item_id) 
            return;
        this.select_attack_item_id = select_attack_item_id;
        saveField(_bm, "select_attack_item_id", select_attack_item_id);
    }

    // 攻击类型
    public int getAttackType() { return this.attack_type; }
    public void setAttackType(BM _bm, int attack_type) {
        if(attack_type==this.attack_type) 
            return;
        this.attack_type = attack_type; 
        markField(_bm, FIELD_attack_type); 
    }
    public void saveAttackType(BM _bm, int attack_type) {
        if(attack_type==this.attack_type) 
            return;
        this.attack_type = attack_type;
        saveField(_bm, "attack_type", attack_type);
    }

    // 对手CID
    public long getOpponentCid() { return this.opponent_cid; }
    public void setOpponentCid(BM _bm, long opponent_cid) {
        if(opponent_cid==this.opponent_cid) 
            return;
        this.opponent_cid = opponent_cid; 
        markField(_bm, FIELD_opponent_cid); 
    }
    public void saveOpponentCid(BM _bm, long opponent_cid) {
        if(opponent_cid==this.opponent_cid) 
            return;
        this.opponent_cid = opponent_cid;
        saveField(_bm, "opponent_cid", opponent_cid);
    }

    // 已经击败的大臣列表
    public byte[] getHadDefeatHeroList() { return this.had_defeat_hero_list; }
    public void setHadDefeatHeroList(BM _bm, byte[] had_defeat_hero_list) {
        if(had_defeat_hero_list==this.had_defeat_hero_list) 
            return;
        this.had_defeat_hero_list = had_defeat_hero_list; 
        markField(_bm, FIELD_had_defeat_hero_list); 
    }
    public void saveHadDefeatHeroList(BM _bm, byte[] had_defeat_hero_list) {
        if(had_defeat_hero_list==this.had_defeat_hero_list) 
            return;
        this.had_defeat_hero_list = had_defeat_hero_list;
        saveFieldBytes(_bm, "had_defeat_hero_list", had_defeat_hero_list);
    }

    // 对手大臣列表
    public byte[] getOpponentHeroList() { return this.opponent_hero_list; }
    public void setOpponentHeroList(BM _bm, byte[] opponent_hero_list) {
        if(opponent_hero_list==this.opponent_hero_list) 
            return;
        this.opponent_hero_list = opponent_hero_list; 
        markField(_bm, FIELD_opponent_hero_list); 
    }
    public void saveOpponentHeroList(BM _bm, byte[] opponent_hero_list) {
        if(opponent_hero_list==this.opponent_hero_list) 
            return;
        this.opponent_hero_list = opponent_hero_list;
        saveFieldBytes(_bm, "opponent_hero_list", opponent_hero_list);
    }

    // 本回合可攻击英雄列表
    public byte[] getCanAttackHeroList() { return this.can_attack_hero_list; }
    public void setCanAttackHeroList(BM _bm, byte[] can_attack_hero_list) {
        if(can_attack_hero_list==this.can_attack_hero_list) 
            return;
        this.can_attack_hero_list = can_attack_hero_list; 
        markField(_bm, FIELD_can_attack_hero_list); 
    }
    public void saveCanAttackHeroList(BM _bm, byte[] can_attack_hero_list) {
        if(can_attack_hero_list==this.can_attack_hero_list) 
            return;
        this.can_attack_hero_list = can_attack_hero_list;
        saveFieldBytes(_bm, "can_attack_hero_list", can_attack_hero_list);
    }

    // 对手战力
    public long getOpponentPower() { return this.opponent_power; }
    public void setOpponentPower(BM _bm, long opponent_power) {
        if(opponent_power==this.opponent_power) 
            return;
        this.opponent_power = opponent_power; 
        markField(_bm, FIELD_opponent_power); 
    }
    public void saveOpponentPower(BM _bm, long opponent_power) {
        if(opponent_power==this.opponent_power) 
            return;
        this.opponent_power = opponent_power;
        saveField(_bm, "opponent_power", opponent_power);
    }

    // 机器人名字
    public String getBotName() { return this.bot_name; }
    public void setBotName(BM _bm, String bot_name) {
        if(bot_name.equals(this.bot_name)) 
            return;
        this.bot_name = bot_name; 
        markField(_bm, FIELD_bot_name); 
    }
    public void saveBotName(BM _bm, String bot_name) {
        if(bot_name.equals(this.bot_name)) 
            return;
        this.bot_name = bot_name;
        saveField(_bm, "bot_name", bot_name);
    }

    // 是否是机器人
    public boolean getIsBot() { return this.is_bot; }
    public void setIsBot(BM _bm, boolean is_bot) {
        if(is_bot==this.is_bot) 
            return;
        this.is_bot = is_bot; 
        markField(_bm, FIELD_is_bot); 
    }
    public void saveIsBot(BM _bm, boolean is_bot) {
        if(is_bot==this.is_bot) 
            return;
        this.is_bot = is_bot;
        saveField(_bm, "is_bot", is_bot ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `round` = '").append(round).append("',");
        sBuilder.append(" `had_buy_buff` = '").append(had_buy_buff ? 1 : 0).append("',");
        sBuilder.append(" `buff_list` = ?,");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `base_power` = '").append(base_power).append("',");
        sBuilder.append(" `deducted_hp` = '").append(deducted_hp).append("',");
        sBuilder.append(" `select_attack_item_id` = '").append(select_attack_item_id).append("',");
        sBuilder.append(" `attack_type` = '").append(attack_type).append("',");
        sBuilder.append(" `opponent_cid` = '").append(opponent_cid).append("',");
        sBuilder.append(" `had_defeat_hero_list` = ?,");
        sBuilder.append(" `opponent_hero_list` = ?,");
        sBuilder.append(" `can_attack_hero_list` = ?,");
        sBuilder.append(" `opponent_power` = '").append(opponent_power).append("',");
        sBuilder.append(" `bot_name` = '").append(bot_name == null ? null : bot_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `is_bot` = '").append(is_bot ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_round)) sBuilder.append(" `round` = '").append(round).append("',");
        if(isFieldMarked(FIELD_had_buy_buff)) sBuilder.append(" `had_buy_buff` = '").append(had_buy_buff ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_buff_list)) sBuilder.append(" `buff_list` = ?,");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_base_power)) sBuilder.append(" `base_power` = '").append(base_power).append("',");
        if(isFieldMarked(FIELD_deducted_hp)) sBuilder.append(" `deducted_hp` = '").append(deducted_hp).append("',");
        if(isFieldMarked(FIELD_select_attack_item_id)) sBuilder.append(" `select_attack_item_id` = '").append(select_attack_item_id).append("',");
        if(isFieldMarked(FIELD_attack_type)) sBuilder.append(" `attack_type` = '").append(attack_type).append("',");
        if(isFieldMarked(FIELD_opponent_cid)) sBuilder.append(" `opponent_cid` = '").append(opponent_cid).append("',");
        if(isFieldMarked(FIELD_had_defeat_hero_list)) sBuilder.append(" `had_defeat_hero_list` = ?,");
        if(isFieldMarked(FIELD_opponent_hero_list)) sBuilder.append(" `opponent_hero_list` = ?,");
        if(isFieldMarked(FIELD_can_attack_hero_list)) sBuilder.append(" `can_attack_hero_list` = ?,");
        if(isFieldMarked(FIELD_opponent_power)) sBuilder.append(" `opponent_power` = '").append(opponent_power).append("',");
        if(isFieldMarked(FIELD_bot_name)) sBuilder.append(" `bot_name` = '").append(bot_name == null ? null : bot_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_is_bot)) sBuilder.append(" `is_bot` = '").append(is_bot ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_arena_battle` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`round` int(11) NOT NULL DEFAULT '0' COMMENT '当前回合数',"
                + "`had_buy_buff` tinyint(1) NOT NULL DEFAULT '0' COMMENT '本轮是否已购买buff',"
                + "`buff_list` blob NULL COMMENT '临时增益列表',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '我方大臣id',"
                + "`base_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '我方基础实力',"
                + "`deducted_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '我方被扣除血量',"
                + "`select_attack_item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '选择攻击道具id',"
                + "`attack_type` int(11) NOT NULL DEFAULT '0' COMMENT '攻击类型',"
                + "`opponent_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '对手CID',"
                + "`had_defeat_hero_list` blob NULL COMMENT '已经击败的大臣列表',"
                + "`opponent_hero_list` blob NULL COMMENT '对手大臣列表',"
                + "`can_attack_hero_list` blob NULL COMMENT '本回合可攻击英雄列表',"
                + "`opponent_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '对手战力',"
                + "`bot_name` varchar(256) NOT NULL DEFAULT '' COMMENT '机器人名字',"
                + "`is_bot` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否是机器人',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家竞技场战斗数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//round
        _size+=1;//had_buy_buff
        _size+=2;_size+=buff_list.length;//buff_list
        _size+=8;//hero_id
        _size+=8;//base_power
        _size+=8;//deducted_hp
        _size+=8;//select_attack_item_id
        _size+=4;//attack_type
        _size+=8;//opponent_cid
        _size+=2;_size+=had_defeat_hero_list.length;//had_defeat_hero_list
        _size+=2;_size+=opponent_hero_list.length;//opponent_hero_list
        _size+=2;_size+=can_attack_hero_list.length;//can_attack_hero_list
        _size+=8;//opponent_power
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(bot_name);//bot_name
        _size+=1;//is_bot
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(round);
        buff.put((byte)(had_buy_buff?1:0));
        buff.putShort((short)(buff_list == null ? 0 : buff_list.length));if(null != buff_list){buff.put(buff_list);}
        buff.putLong(hero_id);
        buff.putLong(base_power);
        buff.putLong(deducted_hp);
        buff.putLong(select_attack_item_id);
        buff.putInt(attack_type);
        buff.putLong(opponent_cid);
        buff.putShort((short)(had_defeat_hero_list == null ? 0 : had_defeat_hero_list.length));if(null != had_defeat_hero_list){buff.put(had_defeat_hero_list);}
        buff.putShort((short)(opponent_hero_list == null ? 0 : opponent_hero_list.length));if(null != opponent_hero_list){buff.put(opponent_hero_list);}
        buff.putShort((short)(can_attack_hero_list == null ? 0 : can_attack_hero_list.length));if(null != can_attack_hero_list){buff.put(can_attack_hero_list);}
        buff.putLong(opponent_power);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, bot_name);
        buff.put((byte)(is_bot?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        round=buff.getInt();
        had_buy_buff=(buff.get()==1);
        int buff_list_count = buff.getShort();if(buff_list_count>0){buff_list = new byte[buff_list_count];buff.get(buff_list);}
        hero_id=buff.getLong();
        base_power=buff.getLong();
        deducted_hp=buff.getLong();
        select_attack_item_id=buff.getLong();
        attack_type=buff.getInt();
        opponent_cid=buff.getLong();
        int had_defeat_hero_list_count = buff.getShort();if(had_defeat_hero_list_count>0){had_defeat_hero_list = new byte[had_defeat_hero_list_count];buff.get(had_defeat_hero_list);}
        int opponent_hero_list_count = buff.getShort();if(opponent_hero_list_count>0){opponent_hero_list = new byte[opponent_hero_list_count];buff.get(opponent_hero_list);}
        int can_attack_hero_list_count = buff.getShort();if(can_attack_hero_list_count>0){can_attack_hero_list = new byte[can_attack_hero_list_count];buff.get(can_attack_hero_list);}
        opponent_power=buff.getLong();
        bot_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        is_bot=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
