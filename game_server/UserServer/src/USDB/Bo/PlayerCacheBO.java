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
public class PlayerCacheBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_player_name =1;
    @DataBaseField(type = "varchar(100)", fieldname = "player_name", comment = "玩家名")
    private String player_name;

    public static final int FIELD_vip_lvl =2;
    @DataBaseField(type = "int(11)", fieldname = "vip_lvl", comment = "VIP等级")
    private int vip_lvl;

    public static final int FIELD_player_lvl =3;
    @DataBaseField(type = "int(11)", fieldname = "player_lvl", comment = "玩家等级")
    private int player_lvl;

    public static final int FIELD_memory_update_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "memory_update_time_ms", comment = "内存数据写入时间")
    private long memory_update_time_ms;

    public static final int FIELD_guild_info =5;
    @DataBaseField(type = "blob", fieldname = "guild_info", comment = "玩家归属联盟信息")
    private byte[] guild_info;

    public static final int FIELD_icon_info =6;
    @DataBaseField(type = "blob", fieldname = "icon_info", comment = "头像信息")
    private byte[] icon_info;

    public static final int FIELD_icon_bgk_info =7;
    @DataBaseField(type = "blob", fieldname = "icon_bgk_info", comment = "头像框信息")
    private byte[] icon_bgk_info;

    public static final int FIELD_bubble_info =8;
    @DataBaseField(type = "blob", fieldname = "bubble_info", comment = "气泡框信息")
    private byte[] bubble_info;

    public static final int FIELD_cute_actor_info =9;
    @DataBaseField(type = "blob", fieldname = "cute_actor_info", comment = "Q版形象信息")
    private byte[] cute_actor_info;

    public static final int FIELD_last_offline_time_ms =10;
    @DataBaseField(type = "bigint(20)", fieldname = "last_offline_time_ms", comment = "最近一次离线时间")
    private long last_offline_time_ms;

    public static final int FIELD_last_online_time_ms =11;
    @DataBaseField(type = "bigint(20)", fieldname = "last_online_time_ms", comment = "最近一次上线时间")
    private long last_online_time_ms;

    public static final int FIELD_freeze_time_ms =12;
    @DataBaseField(type = "bigint(20)", fieldname = "freeze_time_ms", comment = "冻结结束时间")
    private long freeze_time_ms;

    public static final int FIELD_language =13;
    @DataBaseField(type = "varchar(20)", fieldname = "language", comment = "玩家语言")
    private String language;

    public static final int FIELD_total_power =14;
    @DataBaseField(type = "bigint(20)", fieldname = "total_power", comment = "总实力(延迟更新)")
    private long total_power;

    public static final int FIELD_max_power =15;
    @DataBaseField(type = "bigint(20)", fieldname = "max_power", comment = "历史最高实力(延迟更新)")
    private long max_power;

    public static final int FIELD_earnings =16;
    @DataBaseField(type = "bigint(20)", fieldname = "earnings", comment = "总赚速(延迟更新)")
    private long earnings;

    public static final int FIELD_max_earnings =17;
    @DataBaseField(type = "bigint(20)", fieldname = "max_earnings", comment = "历史最大总赚速(延迟更新)")
    private long max_earnings;

    public static final int FIELD_exp =18;
    @DataBaseField(type = "bigint(20)", fieldname = "exp", comment = "经验值")
    private long exp;

    public static final int FIELD_title_obj_v2 =19;
    @DataBaseField(type = "blob", fieldname = "title_obj_v2", comment = "称号相关")
    private byte[] title_obj_v2;

    public static final int FIELD_player_skin =20;
    @DataBaseField(type = "bigint(20)", fieldname = "player_skin", comment = "玩家皮肤")
    private long player_skin;

    public static final int FIELD_vipExp =21;
    @DataBaseField(type = "bigint(20)", fieldname = "vipExp", comment = "VIP经验值")
    private long vipExp;

    public PlayerCacheBO() {
        id = 0;
        cid = 0L;
        player_name = "";
        vip_lvl = 0;
        player_lvl = 0;
        memory_update_time_ms = 0L;
        guild_info = null;
        icon_info = null;
        icon_bgk_info = null;
        bubble_info = null;
        cute_actor_info = null;
        last_offline_time_ms = 0L;
        last_online_time_ms = 0L;
        freeze_time_ms = 0L;
        language = "";
        total_power = 0L;
        max_power = 0L;
        earnings = 0L;
        max_earnings = 0L;
        exp = 0L;
        title_obj_v2 = null;
        player_skin = 0L;
        vipExp = 0L;
    }

    public PlayerCacheBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        player_name = rs.getString(3);
        vip_lvl = rs.getInt(4);
        player_lvl = rs.getInt(5);
        memory_update_time_ms = rs.getLong(6);
        guild_info = rs.getBytes(7);
        icon_info = rs.getBytes(8);
        icon_bgk_info = rs.getBytes(9);
        bubble_info = rs.getBytes(10);
        cute_actor_info = rs.getBytes(11);
        last_offline_time_ms = rs.getLong(12);
        last_online_time_ms = rs.getLong(13);
        freeze_time_ms = rs.getLong(14);
        language = rs.getString(15);
        total_power = rs.getLong(16);
        max_power = rs.getLong(17);
        earnings = rs.getLong(18);
        max_earnings = rs.getLong(19);
        exp = rs.getLong(20);
        title_obj_v2 = rs.getBytes(21);
        player_skin = rs.getLong(22);
        vipExp = rs.getLong(23);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerCacheBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `player_name`, `vip_lvl`, `player_lvl`, `memory_update_time_ms`, `guild_info`, `icon_info`, `icon_bgk_info`, `bubble_info`, `cute_actor_info`, `last_offline_time_ms`, `last_online_time_ms`, `freeze_time_ms`, `language`, `total_power`, `max_power`, `earnings`, `max_earnings`, `exp`, `title_obj_v2`, `player_skin`, `vipExp`";
    }

    @Override
    public String getTableName() {
        return "`player_cache`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(vip_lvl).append("', ");
        strBuf.append("'").append(player_lvl).append("', ");
        strBuf.append("'").append(memory_update_time_ms).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(last_offline_time_ms).append("', ");
        strBuf.append("'").append(last_online_time_ms).append("', ");
        strBuf.append("'").append(freeze_time_ms).append("', ");
        strBuf.append("'").append(language == null ? null : language.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(total_power).append("', ");
        strBuf.append("'").append(max_power).append("', ");
        strBuf.append("'").append(earnings).append("', ");
        strBuf.append("'").append(max_earnings).append("', ");
        strBuf.append("'").append(exp).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(player_skin).append("', ");
        strBuf.append("'").append(vipExp).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(guild_info); 
        ret.add(icon_info); 
        ret.add(icon_bgk_info); 
        ret.add(bubble_info); 
        ret.add(cute_actor_info); 
        ret.add(title_obj_v2);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_guild_info)) ret.add(guild_info); 
        if(isFieldMarked(FIELD_icon_info)) ret.add(icon_info); 
        if(isFieldMarked(FIELD_icon_bgk_info)) ret.add(icon_bgk_info); 
        if(isFieldMarked(FIELD_bubble_info)) ret.add(bubble_info); 
        if(isFieldMarked(FIELD_cute_actor_info)) ret.add(cute_actor_info); 
        if(isFieldMarked(FIELD_title_obj_v2)) ret.add(title_obj_v2);         return ret;
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

    // 玩家名
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

    // VIP等级
    public int getVipLvl() { return this.vip_lvl; }
    public void setVipLvl(BM _bm, int vip_lvl) {
        if(vip_lvl==this.vip_lvl) 
            return;
        this.vip_lvl = vip_lvl; 
        markField(_bm, FIELD_vip_lvl); 
    }
    public void saveVipLvl(BM _bm, int vip_lvl) {
        if(vip_lvl==this.vip_lvl) 
            return;
        this.vip_lvl = vip_lvl;
        saveField(_bm, "vip_lvl", vip_lvl);
    }

    // 玩家等级
    public int getPlayerLvl() { return this.player_lvl; }
    public void setPlayerLvl(BM _bm, int player_lvl) {
        if(player_lvl==this.player_lvl) 
            return;
        this.player_lvl = player_lvl; 
        markField(_bm, FIELD_player_lvl); 
    }
    public void savePlayerLvl(BM _bm, int player_lvl) {
        if(player_lvl==this.player_lvl) 
            return;
        this.player_lvl = player_lvl;
        saveField(_bm, "player_lvl", player_lvl);
    }

    // 内存数据写入时间
    public long getMemoryUpdateTimeMs() { return this.memory_update_time_ms; }
    public void setMemoryUpdateTimeMs(BM _bm, long memory_update_time_ms) {
        if(memory_update_time_ms==this.memory_update_time_ms) 
            return;
        this.memory_update_time_ms = memory_update_time_ms; 
        markField(_bm, FIELD_memory_update_time_ms); 
    }
    public void saveMemoryUpdateTimeMs(BM _bm, long memory_update_time_ms) {
        if(memory_update_time_ms==this.memory_update_time_ms) 
            return;
        this.memory_update_time_ms = memory_update_time_ms;
        saveField(_bm, "memory_update_time_ms", memory_update_time_ms);
    }

    // 玩家归属联盟信息
    public byte[] getGuildInfo() { return this.guild_info; }
    public void setGuildInfo(BM _bm, byte[] guild_info) {
        if(guild_info==this.guild_info) 
            return;
        this.guild_info = guild_info; 
        markField(_bm, FIELD_guild_info); 
    }
    public void saveGuildInfo(BM _bm, byte[] guild_info) {
        if(guild_info==this.guild_info) 
            return;
        this.guild_info = guild_info;
        saveFieldBytes(_bm, "guild_info", guild_info);
    }

    // 头像信息
    public byte[] getIconInfo() { return this.icon_info; }
    public void setIconInfo(BM _bm, byte[] icon_info) {
        if(icon_info==this.icon_info) 
            return;
        this.icon_info = icon_info; 
        markField(_bm, FIELD_icon_info); 
    }
    public void saveIconInfo(BM _bm, byte[] icon_info) {
        if(icon_info==this.icon_info) 
            return;
        this.icon_info = icon_info;
        saveFieldBytes(_bm, "icon_info", icon_info);
    }

    // 头像框信息
    public byte[] getIconBgkInfo() { return this.icon_bgk_info; }
    public void setIconBgkInfo(BM _bm, byte[] icon_bgk_info) {
        if(icon_bgk_info==this.icon_bgk_info) 
            return;
        this.icon_bgk_info = icon_bgk_info; 
        markField(_bm, FIELD_icon_bgk_info); 
    }
    public void saveIconBgkInfo(BM _bm, byte[] icon_bgk_info) {
        if(icon_bgk_info==this.icon_bgk_info) 
            return;
        this.icon_bgk_info = icon_bgk_info;
        saveFieldBytes(_bm, "icon_bgk_info", icon_bgk_info);
    }

    // 气泡框信息
    public byte[] getBubbleInfo() { return this.bubble_info; }
    public void setBubbleInfo(BM _bm, byte[] bubble_info) {
        if(bubble_info==this.bubble_info) 
            return;
        this.bubble_info = bubble_info; 
        markField(_bm, FIELD_bubble_info); 
    }
    public void saveBubbleInfo(BM _bm, byte[] bubble_info) {
        if(bubble_info==this.bubble_info) 
            return;
        this.bubble_info = bubble_info;
        saveFieldBytes(_bm, "bubble_info", bubble_info);
    }

    // Q版形象信息
    public byte[] getCuteActorInfo() { return this.cute_actor_info; }
    public void setCuteActorInfo(BM _bm, byte[] cute_actor_info) {
        if(cute_actor_info==this.cute_actor_info) 
            return;
        this.cute_actor_info = cute_actor_info; 
        markField(_bm, FIELD_cute_actor_info); 
    }
    public void saveCuteActorInfo(BM _bm, byte[] cute_actor_info) {
        if(cute_actor_info==this.cute_actor_info) 
            return;
        this.cute_actor_info = cute_actor_info;
        saveFieldBytes(_bm, "cute_actor_info", cute_actor_info);
    }

    // 最近一次离线时间
    public long getLastOfflineTimeMs() { return this.last_offline_time_ms; }
    public void setLastOfflineTimeMs(BM _bm, long last_offline_time_ms) {
        if(last_offline_time_ms==this.last_offline_time_ms) 
            return;
        this.last_offline_time_ms = last_offline_time_ms; 
        markField(_bm, FIELD_last_offline_time_ms); 
    }
    public void saveLastOfflineTimeMs(BM _bm, long last_offline_time_ms) {
        if(last_offline_time_ms==this.last_offline_time_ms) 
            return;
        this.last_offline_time_ms = last_offline_time_ms;
        saveField(_bm, "last_offline_time_ms", last_offline_time_ms);
    }

    // 最近一次上线时间
    public long getLastOnlineTimeMs() { return this.last_online_time_ms; }
    public void setLastOnlineTimeMs(BM _bm, long last_online_time_ms) {
        if(last_online_time_ms==this.last_online_time_ms) 
            return;
        this.last_online_time_ms = last_online_time_ms; 
        markField(_bm, FIELD_last_online_time_ms); 
    }
    public void saveLastOnlineTimeMs(BM _bm, long last_online_time_ms) {
        if(last_online_time_ms==this.last_online_time_ms) 
            return;
        this.last_online_time_ms = last_online_time_ms;
        saveField(_bm, "last_online_time_ms", last_online_time_ms);
    }

    // 冻结结束时间
    public long getFreezeTimeMs() { return this.freeze_time_ms; }
    public void setFreezeTimeMs(BM _bm, long freeze_time_ms) {
        if(freeze_time_ms==this.freeze_time_ms) 
            return;
        this.freeze_time_ms = freeze_time_ms; 
        markField(_bm, FIELD_freeze_time_ms); 
    }
    public void saveFreezeTimeMs(BM _bm, long freeze_time_ms) {
        if(freeze_time_ms==this.freeze_time_ms) 
            return;
        this.freeze_time_ms = freeze_time_ms;
        saveField(_bm, "freeze_time_ms", freeze_time_ms);
    }

    // 玩家语言
    public String getLanguage() { return this.language; }
    public void setLanguage(BM _bm, String language) {
        if(language.equals(this.language)) 
            return;
        this.language = language; 
        markField(_bm, FIELD_language); 
    }
    public void saveLanguage(BM _bm, String language) {
        if(language.equals(this.language)) 
            return;
        this.language = language;
        saveField(_bm, "language", language);
    }

    // 总实力(延迟更新)
    public long getTotalPower() { return this.total_power; }
    public void setTotalPower(BM _bm, long total_power) {
        if(total_power==this.total_power) 
            return;
        this.total_power = total_power; 
        markField(_bm, FIELD_total_power); 
    }
    public void saveTotalPower(BM _bm, long total_power) {
        if(total_power==this.total_power) 
            return;
        this.total_power = total_power;
        saveField(_bm, "total_power", total_power);
    }

    // 历史最高实力(延迟更新)
    public long getMaxPower() { return this.max_power; }
    public void setMaxPower(BM _bm, long max_power) {
        if(max_power==this.max_power) 
            return;
        this.max_power = max_power; 
        markField(_bm, FIELD_max_power); 
    }
    public void saveMaxPower(BM _bm, long max_power) {
        if(max_power==this.max_power) 
            return;
        this.max_power = max_power;
        saveField(_bm, "max_power", max_power);
    }

    // 总赚速(延迟更新)
    public long getEarnings() { return this.earnings; }
    public void setEarnings(BM _bm, long earnings) {
        if(earnings==this.earnings) 
            return;
        this.earnings = earnings; 
        markField(_bm, FIELD_earnings); 
    }
    public void saveEarnings(BM _bm, long earnings) {
        if(earnings==this.earnings) 
            return;
        this.earnings = earnings;
        saveField(_bm, "earnings", earnings);
    }

    // 历史最大总赚速(延迟更新)
    public long getMaxEarnings() { return this.max_earnings; }
    public void setMaxEarnings(BM _bm, long max_earnings) {
        if(max_earnings==this.max_earnings) 
            return;
        this.max_earnings = max_earnings; 
        markField(_bm, FIELD_max_earnings); 
    }
    public void saveMaxEarnings(BM _bm, long max_earnings) {
        if(max_earnings==this.max_earnings) 
            return;
        this.max_earnings = max_earnings;
        saveField(_bm, "max_earnings", max_earnings);
    }

    // 经验值
    public long getExp() { return this.exp; }
    public void setExp(BM _bm, long exp) {
        if(exp==this.exp) 
            return;
        this.exp = exp; 
        markField(_bm, FIELD_exp); 
    }
    public void saveExp(BM _bm, long exp) {
        if(exp==this.exp) 
            return;
        this.exp = exp;
        saveField(_bm, "exp", exp);
    }

    // 称号相关
    public byte[] getTitleObjV2() { return this.title_obj_v2; }
    public void setTitleObjV2(BM _bm, byte[] title_obj_v2) {
        if(title_obj_v2==this.title_obj_v2) 
            return;
        this.title_obj_v2 = title_obj_v2; 
        markField(_bm, FIELD_title_obj_v2); 
    }
    public void saveTitleObjV2(BM _bm, byte[] title_obj_v2) {
        if(title_obj_v2==this.title_obj_v2) 
            return;
        this.title_obj_v2 = title_obj_v2;
        saveFieldBytes(_bm, "title_obj_v2", title_obj_v2);
    }

    // 玩家皮肤
    public long getPlayerSkin() { return this.player_skin; }
    public void setPlayerSkin(BM _bm, long player_skin) {
        if(player_skin==this.player_skin) 
            return;
        this.player_skin = player_skin; 
        markField(_bm, FIELD_player_skin); 
    }
    public void savePlayerSkin(BM _bm, long player_skin) {
        if(player_skin==this.player_skin) 
            return;
        this.player_skin = player_skin;
        saveField(_bm, "player_skin", player_skin);
    }

    // VIP经验值
    public long getVipExp() { return this.vipExp; }
    public void setVipExp(BM _bm, long vipExp) {
        if(vipExp==this.vipExp) 
            return;
        this.vipExp = vipExp; 
        markField(_bm, FIELD_vipExp); 
    }
    public void saveVipExp(BM _bm, long vipExp) {
        if(vipExp==this.vipExp) 
            return;
        this.vipExp = vipExp;
        saveField(_bm, "vipExp", vipExp);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `player_name` = '").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `vip_lvl` = '").append(vip_lvl).append("',");
        sBuilder.append(" `player_lvl` = '").append(player_lvl).append("',");
        sBuilder.append(" `memory_update_time_ms` = '").append(memory_update_time_ms).append("',");
        sBuilder.append(" `guild_info` = ?,");
        sBuilder.append(" `icon_info` = ?,");
        sBuilder.append(" `icon_bgk_info` = ?,");
        sBuilder.append(" `bubble_info` = ?,");
        sBuilder.append(" `cute_actor_info` = ?,");
        sBuilder.append(" `last_offline_time_ms` = '").append(last_offline_time_ms).append("',");
        sBuilder.append(" `last_online_time_ms` = '").append(last_online_time_ms).append("',");
        sBuilder.append(" `freeze_time_ms` = '").append(freeze_time_ms).append("',");
        sBuilder.append(" `language` = '").append(language == null ? null : language.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `total_power` = '").append(total_power).append("',");
        sBuilder.append(" `max_power` = '").append(max_power).append("',");
        sBuilder.append(" `earnings` = '").append(earnings).append("',");
        sBuilder.append(" `max_earnings` = '").append(max_earnings).append("',");
        sBuilder.append(" `exp` = '").append(exp).append("',");
        sBuilder.append(" `title_obj_v2` = ?,");
        sBuilder.append(" `player_skin` = '").append(player_skin).append("',");
        sBuilder.append(" `vipExp` = '").append(vipExp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_player_name)) sBuilder.append(" `player_name` = '").append(player_name == null ? null : player_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_vip_lvl)) sBuilder.append(" `vip_lvl` = '").append(vip_lvl).append("',");
        if(isFieldMarked(FIELD_player_lvl)) sBuilder.append(" `player_lvl` = '").append(player_lvl).append("',");
        if(isFieldMarked(FIELD_memory_update_time_ms)) sBuilder.append(" `memory_update_time_ms` = '").append(memory_update_time_ms).append("',");
        if(isFieldMarked(FIELD_guild_info)) sBuilder.append(" `guild_info` = ?,");
        if(isFieldMarked(FIELD_icon_info)) sBuilder.append(" `icon_info` = ?,");
        if(isFieldMarked(FIELD_icon_bgk_info)) sBuilder.append(" `icon_bgk_info` = ?,");
        if(isFieldMarked(FIELD_bubble_info)) sBuilder.append(" `bubble_info` = ?,");
        if(isFieldMarked(FIELD_cute_actor_info)) sBuilder.append(" `cute_actor_info` = ?,");
        if(isFieldMarked(FIELD_last_offline_time_ms)) sBuilder.append(" `last_offline_time_ms` = '").append(last_offline_time_ms).append("',");
        if(isFieldMarked(FIELD_last_online_time_ms)) sBuilder.append(" `last_online_time_ms` = '").append(last_online_time_ms).append("',");
        if(isFieldMarked(FIELD_freeze_time_ms)) sBuilder.append(" `freeze_time_ms` = '").append(freeze_time_ms).append("',");
        if(isFieldMarked(FIELD_language)) sBuilder.append(" `language` = '").append(language == null ? null : language.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_total_power)) sBuilder.append(" `total_power` = '").append(total_power).append("',");
        if(isFieldMarked(FIELD_max_power)) sBuilder.append(" `max_power` = '").append(max_power).append("',");
        if(isFieldMarked(FIELD_earnings)) sBuilder.append(" `earnings` = '").append(earnings).append("',");
        if(isFieldMarked(FIELD_max_earnings)) sBuilder.append(" `max_earnings` = '").append(max_earnings).append("',");
        if(isFieldMarked(FIELD_exp)) sBuilder.append(" `exp` = '").append(exp).append("',");
        if(isFieldMarked(FIELD_title_obj_v2)) sBuilder.append(" `title_obj_v2` = ?,");
        if(isFieldMarked(FIELD_player_skin)) sBuilder.append(" `player_skin` = '").append(player_skin).append("',");
        if(isFieldMarked(FIELD_vipExp)) sBuilder.append(" `vipExp` = '").append(vipExp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_cache` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`player_name` varchar(100) NOT NULL DEFAULT '' COMMENT '玩家名',"
                + "`vip_lvl` int(11) NOT NULL DEFAULT '0' COMMENT 'VIP等级',"
                + "`player_lvl` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`memory_update_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '内存数据写入时间',"
                + "`guild_info` blob NULL COMMENT '玩家归属联盟信息',"
                + "`icon_info` blob NULL COMMENT '头像信息',"
                + "`icon_bgk_info` blob NULL COMMENT '头像框信息',"
                + "`bubble_info` blob NULL COMMENT '气泡框信息',"
                + "`cute_actor_info` blob NULL COMMENT 'Q版形象信息',"
                + "`last_offline_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '最近一次离线时间',"
                + "`last_online_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '最近一次上线时间',"
                + "`freeze_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '冻结结束时间',"
                + "`language` varchar(20) NOT NULL DEFAULT '' COMMENT '玩家语言',"
                + "`total_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '总实力(延迟更新)',"
                + "`max_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '历史最高实力(延迟更新)',"
                + "`earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '总赚速(延迟更新)',"
                + "`max_earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '历史最大总赚速(延迟更新)',"
                + "`exp` bigint(20) NOT NULL DEFAULT '0' COMMENT '经验值',"
                + "`title_obj_v2` blob NULL COMMENT '称号相关',"
                + "`player_skin` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家皮肤',"
                + "`vipExp` bigint(20) NOT NULL DEFAULT '0' COMMENT 'VIP经验值',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家缓存数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(player_name);//player_name
        _size+=4;//vip_lvl
        _size+=4;//player_lvl
        _size+=8;//memory_update_time_ms
        _size+=2;_size+=guild_info.length;//guild_info
        _size+=2;_size+=icon_info.length;//icon_info
        _size+=2;_size+=icon_bgk_info.length;//icon_bgk_info
        _size+=2;_size+=bubble_info.length;//bubble_info
        _size+=2;_size+=cute_actor_info.length;//cute_actor_info
        _size+=8;//last_offline_time_ms
        _size+=8;//last_online_time_ms
        _size+=8;//freeze_time_ms
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(language);//language
        _size+=8;//total_power
        _size+=8;//max_power
        _size+=8;//earnings
        _size+=8;//max_earnings
        _size+=8;//exp
        _size+=2;_size+=title_obj_v2.length;//title_obj_v2
        _size+=8;//player_skin
        _size+=8;//vipExp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, player_name);
        buff.putInt(vip_lvl);
        buff.putInt(player_lvl);
        buff.putLong(memory_update_time_ms);
        buff.putShort((short)(guild_info == null ? 0 : guild_info.length));if(null != guild_info){buff.put(guild_info);}
        buff.putShort((short)(icon_info == null ? 0 : icon_info.length));if(null != icon_info){buff.put(icon_info);}
        buff.putShort((short)(icon_bgk_info == null ? 0 : icon_bgk_info.length));if(null != icon_bgk_info){buff.put(icon_bgk_info);}
        buff.putShort((short)(bubble_info == null ? 0 : bubble_info.length));if(null != bubble_info){buff.put(bubble_info);}
        buff.putShort((short)(cute_actor_info == null ? 0 : cute_actor_info.length));if(null != cute_actor_info){buff.put(cute_actor_info);}
        buff.putLong(last_offline_time_ms);
        buff.putLong(last_online_time_ms);
        buff.putLong(freeze_time_ms);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, language);
        buff.putLong(total_power);
        buff.putLong(max_power);
        buff.putLong(earnings);
        buff.putLong(max_earnings);
        buff.putLong(exp);
        buff.putShort((short)(title_obj_v2 == null ? 0 : title_obj_v2.length));if(null != title_obj_v2){buff.put(title_obj_v2);}
        buff.putLong(player_skin);
        buff.putLong(vipExp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        player_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        vip_lvl=buff.getInt();
        player_lvl=buff.getInt();
        memory_update_time_ms=buff.getLong();
        int guild_info_count = buff.getShort();if(guild_info_count>0){guild_info = new byte[guild_info_count];buff.get(guild_info);}
        int icon_info_count = buff.getShort();if(icon_info_count>0){icon_info = new byte[icon_info_count];buff.get(icon_info);}
        int icon_bgk_info_count = buff.getShort();if(icon_bgk_info_count>0){icon_bgk_info = new byte[icon_bgk_info_count];buff.get(icon_bgk_info);}
        int bubble_info_count = buff.getShort();if(bubble_info_count>0){bubble_info = new byte[bubble_info_count];buff.get(bubble_info);}
        int cute_actor_info_count = buff.getShort();if(cute_actor_info_count>0){cute_actor_info = new byte[cute_actor_info_count];buff.get(cute_actor_info);}
        last_offline_time_ms=buff.getLong();
        last_online_time_ms=buff.getLong();
        freeze_time_ms=buff.getLong();
        language=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        total_power=buff.getLong();
        max_power=buff.getLong();
        earnings=buff.getLong();
        max_earnings=buff.getLong();
        exp=buff.getLong();
        int title_obj_v2_count = buff.getShort();if(title_obj_v2_count>0){title_obj_v2 = new byte[title_obj_v2_count];buff.get(title_obj_v2);}
        player_skin=buff.getLong();
        vipExp=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
