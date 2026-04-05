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
public class PlayerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_cname =1;
    @DataBaseField(type = "varchar(64)", fieldname = "cname", comment = "玩家名称")
    private String cname;

    public static final int FIELD_icon =2;
    @DataBaseField(type = "bigint(20)", fieldname = "icon", comment = "玩家头像")
    private long icon;

    public static final int FIELD_iconBgk =3;
    @DataBaseField(type = "bigint(20)", fieldname = "iconBgk", comment = "玩家头像框")
    private long iconBgk;

    public static final int FIELD_title =4;
    @DataBaseField(type = "bigint(20)", fieldname = "title", comment = "玩家当前使用的称号")
    private long title;

    public static final int FIELD_bubble =5;
    @DataBaseField(type = "bigint(20)", fieldname = "bubble", comment = "玩家当前使用的气泡框")
    private long bubble;

    public static final int FIELD_createTime =6;
    @DataBaseField(type = "int(11)", fieldname = "createTime", comment = "账号创建时间(秒) 同ArTime")
    private int createTime;

    public static final int FIELD_create_date =7;
    @DataBaseField(type = "int(11)", fieldname = "create_date", comment = "账号创建日期 同ArTime")
    private int create_date;

    public static final int FIELD_createRoleTime =8;
    @DataBaseField(type = "int(11)", fieldname = "createRoleTime", comment = "创角时间(秒)")
    private int createRoleTime;

    public static final int FIELD_create_role_date =9;
    @DataBaseField(type = "int(11)", fieldname = "create_role_date", comment = "创角日期")
    private int create_role_date;

    public static final int FIELD_lvl =10;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "玩家等级")
    private int lvl;

    public static final int FIELD_vipLvl =11;
    @DataBaseField(type = "int(11)", fieldname = "vipLvl", comment = "VIP等级")
    private int vipLvl;

    public static final int FIELD_gmLevel =12;
    @DataBaseField(type = "int(11)", fieldname = "gmLevel", comment = "gm权限")
    private int gmLevel;

    public static final int FIELD_player_lang =13;
    @DataBaseField(type = "varchar(20)", fieldname = "player_lang", comment = "玩家语言")
    private String player_lang;

    public static final int FIELD_last_login_date =14;
    @DataBaseField(type = "int(11)", fieldname = "last_login_date", comment = "账号最后日期")
    private int last_login_date;

    public static final int FIELD_login_day_count =15;
    @DataBaseField(type = "int(11)", fieldname = "login_day_count", comment = "登录次数")
    private int login_day_count;

    public static final int FIELD_last_take_version =16;
    @DataBaseField(type = "bigint(20)", fieldname = "last_take_version", comment = "最后领取奖励的客户端版本")
    private long last_take_version;

    public static final int FIELD_recharged_gem =17;
    @DataBaseField(type = "bigint(20)", fieldname = "recharged_gem", comment = "充值钻石数量")
    private long recharged_gem;

    public static final int FIELD_last_left_bag_time_s =18;
    @DataBaseField(type = "int(11)", fieldname = "last_left_bag_time_s", comment = "最后一次查看背包物品的时间")
    private int last_left_bag_time_s;

    public static final int FIELD_lastOfflineMs =19;
    @DataBaseField(type = "bigint(20)", fieldname = "lastOfflineMs", comment = "最后一次下线时间（毫秒）")
    private long lastOfflineMs;

    public static final int FIELD_prefab =20;
    @DataBaseField(type = "bigint(20)", fieldname = "prefab", comment = "玩家预制形象配置ID")
    private long prefab;

    public static final int FIELD_is_set_default =21;
    @DataBaseField(type = "bigint(20)", fieldname = "is_set_default", comment = "是否创角 1-已设置")
    private long is_set_default;

    public static final int FIELD_player_stage =22;
    @DataBaseField(type = "bigint(20)", fieldname = "player_stage", comment = "玩家段位")
    private long player_stage;

    public static final int FIELD_happy_value_cur_step_value =23;
    @DataBaseField(type = "bigint(20)", fieldname = "happy_value_cur_step_value", comment = "当前阶段数值")
    private long happy_value_cur_step_value;

    public static final int FIELD_last_login_week_tag =24;
    @DataBaseField(type = "bigint(20)", fieldname = "last_login_week_tag", comment = "最后一次登录周标记")
    private long last_login_week_tag;

    public static final int FIELD_week_login_day_count =25;
    @DataBaseField(type = "bigint(20)", fieldname = "week_login_day_count", comment = "本周登录天数")
    private long week_login_day_count;

    public static final int FIELD_latest_login_time_ms =26;
    @DataBaseField(type = "bigint(20)", fieldname = "latest_login_time_ms", comment = "最近一次登陆时间（毫秒）")
    private long latest_login_time_ms;

    public static final int FIELD_uid =27;
    @DataBaseField(type = "varchar(32)", fieldname = "uid", comment = "平台账号")
    private String uid;

    public static final int FIELD_adfrom =28;
    @DataBaseField(type = "varchar(128)", fieldname = "adfrom", comment = "渠道")
    private String adfrom;

    public static final int FIELD_adfrom2 =29;
    @DataBaseField(type = "varchar(128)", fieldname = "adfrom2", comment = "二级渠道")
    private String adfrom2;

    public static final int FIELD_adid =30;
    @DataBaseField(type = "varchar(128)", fieldname = "adid", comment = "设备id")
    private String adid;

    public static final int FIELD_clientPackageName =31;
    @DataBaseField(type = "varchar(64)", fieldname = "clientPackageName", comment = "客户端包名")
    private String clientPackageName;

    public static final int FIELD_clientVerion =32;
    @DataBaseField(type = "varchar(32)", fieldname = "clientVerion", comment = "客户端版本")
    private String clientVerion;

    public static final int FIELD_nation =33;
    @DataBaseField(type = "varchar(32)", fieldname = "nation", comment = "国家")
    private String nation;

    public static final int FIELD_ar_ip =34;
    @DataBaseField(type = "varchar(32)", fieldname = "ar_ip", comment = "注册IP")
    private String ar_ip;

    public static final int FIELD_sdkid =35;
    @DataBaseField(type = "varchar(128)", fieldname = "sdkid", comment = "渠道账号")
    private String sdkid;

    public static final int FIELD_recharged_money =36;
    @DataBaseField(type = "bigint(20)", fieldname = "recharged_money", comment = "充值货币金额")
    private long recharged_money;

    public static final int FIELD_power_max_record =37;
    @DataBaseField(type = "bigint(20)", fieldname = "power_max_record", comment = "玩家实力历史记录最高值")
    private long power_max_record;

    public static final int FIELD_earnings_max_record =38;
    @DataBaseField(type = "bigint(20)", fieldname = "earnings_max_record", comment = "玩家赚速历史记录最高值")
    private long earnings_max_record;

    public static final int FIELD_market_next_refresh_ms =39;
    @DataBaseField(type = "bigint(20)", fieldname = "market_next_refresh_ms", comment = "集市下次刷新时间")
    private long market_next_refresh_ms;

    public static final int FIELD_last_log_section_day =40;
    @DataBaseField(type = "int(11)", fieldname = "last_log_section_day", comment = "上次截面日志记录日期")
    private int last_log_section_day;

    public static final int FIELD_last_gain_visit_reward_day =41;
    @DataBaseField(type = "int(11)", fieldname = "last_gain_visit_reward_day", comment = "上次领取拜访其他玩家奖励日期")
    private int last_gain_visit_reward_day;

    public static final int FIELD_cute_actor_id =42;
    @DataBaseField(type = "bigint(20)", fieldname = "cute_actor_id", comment = "Q版形象ID")
    private long cute_actor_id;

    public static final int FIELD_consort_call_next_refresh_ms =43;
    @DataBaseField(type = "bigint(20)", fieldname = "consort_call_next_refresh_ms", comment = "家人指定邀约下次刷新时间（针对需要计数的数据）")
    private long consort_call_next_refresh_ms;

    public static final int FIELD_birth_giftde_coum =44;
    @DataBaseField(type = "int(11)", fieldname = "birth_giftde_coum", comment = "下次出生卷王次数")
    private int birth_giftde_coum;

    public static final int FIELD_adult_record_bonus =45;
    @DataBaseField(type = "bigint(20)", fieldname = "adult_record_bonus", comment = "子嗣记录收益总值（因为移除记录在玩家身上）")
    private long adult_record_bonus;

    public static final int FIELD_last_draw_daily_reward_date =46;
    @DataBaseField(type = "int(11)", fieldname = "last_draw_daily_reward_date", comment = "最后一次领取每日奖励的日期标记")
    private int last_draw_daily_reward_date;

    public static final int FIELD_extra_earnings =47;
    @DataBaseField(type = "bigint(20)", fieldname = "extra_earnings", comment = "额外赚速")
    private long extra_earnings;

    public static final int FIELD_building_earnings_max_record =48;
    @DataBaseField(type = "bigint(20)", fieldname = "building_earnings_max_record", comment = "建筑赚速历史记录最高值")
    private long building_earnings_max_record;

    public static final int FIELD_child_earnings_max_record =49;
    @DataBaseField(type = "bigint(20)", fieldname = "child_earnings_max_record", comment = "子嗣赚速历史记录最高值")
    private long child_earnings_max_record;

    public static final int FIELD_seven_days_login_cal_offset =50;
    @DataBaseField(type = "int(11)", fieldname = "seven_days_login_cal_offset", comment = "七天登录计算偏移量")
    private int seven_days_login_cal_offset;

    public static final int FIELD_curTitle =51;
    @DataBaseField(type = "blob", fieldname = "curTitle", comment = "当前称号")
    private byte[] curTitle;

    public static final int FIELD_curTitleShow =52;
    @DataBaseField(type = "tinyint(1)", fieldname = "curTitleShow", comment = "当前称号是否对外展示")
    private boolean curTitleShow;

    public static final int FIELD_curPlayerSkin =53;
    @DataBaseField(type = "bigint(20)", fieldname = "curPlayerSkin", comment = "当前玩家皮肤")
    private long curPlayerSkin;

    public static final int FIELD_is_set_prefab =54;
    @DataBaseField(type = "bigint(20)", fieldname = "is_set_prefab", comment = "是否设置预制形象 1-已设置")
    private long is_set_prefab;

    public static final int FIELD_pending_order_db_id =55;
    @DataBaseField(type = "bigint(20)", fieldname = "pending_order_db_id", comment = "待处理的订单数据ID")
    private long pending_order_db_id;

    public static final int FIELD_isMarsGoRouteDone =56;
    @DataBaseField(type = "tinyint(1)", fieldname = "isMarsGoRouteDone", comment = "火星-前往火星全部阶段完成")
    private boolean isMarsGoRouteDone;

    public static final int FIELD_had_draw_vip_level_reward =57;
    @DataBaseField(type = "bigint(20)", fieldname = "had_draw_vip_level_reward", comment = "VIP等级奖励领取记录")
    private long had_draw_vip_level_reward;

    public static final int FIELD_had_draw_vip_level_recharge_reward =58;
    @DataBaseField(type = "bigint(20)", fieldname = "had_draw_vip_level_recharge_reward", comment = "VIP等级充值奖励领取记录")
    private long had_draw_vip_level_recharge_reward;

    public static final int FIELD_isStoreReviews =59;
    @DataBaseField(type = "tinyint(1)", fieldname = "isStoreReviews", comment = "是否商店评价")
    private boolean isStoreReviews;

    public static final int FIELD_room_skin =60;
    @DataBaseField(type = "bigint(20)", fieldname = "room_skin", comment = "房间皮肤ID")
    private long room_skin;

    public static final int FIELD_giftde_child_graduate_count =61;
    @DataBaseField(type = "int(11)", fieldname = "giftde_child_graduate_count", comment = "卷王子嗣毕业历史数量")
    private int giftde_child_graduate_count;

    public PlayerBO() {
        id = 0;
        cid = 0L;
        cname = "";
        icon = 0L;
        iconBgk = 0L;
        title = 0L;
        bubble = 0L;
        createTime = 0;
        create_date = 0;
        createRoleTime = 0;
        create_role_date = 0;
        lvl = 0;
        vipLvl = 0;
        gmLevel = 0;
        player_lang = "";
        last_login_date = 0;
        login_day_count = 0;
        last_take_version = 0L;
        recharged_gem = 0L;
        last_left_bag_time_s = 0;
        lastOfflineMs = 0L;
        prefab = 0L;
        is_set_default = 0L;
        player_stage = 0L;
        happy_value_cur_step_value = 0L;
        last_login_week_tag = 0L;
        week_login_day_count = 0L;
        latest_login_time_ms = 0L;
        uid = "";
        adfrom = "";
        adfrom2 = "";
        adid = "";
        clientPackageName = "";
        clientVerion = "";
        nation = "";
        ar_ip = "";
        sdkid = "";
        recharged_money = 0L;
        power_max_record = 0L;
        earnings_max_record = 0L;
        market_next_refresh_ms = 0L;
        last_log_section_day = 0;
        last_gain_visit_reward_day = 0;
        cute_actor_id = 0L;
        consort_call_next_refresh_ms = 0L;
        birth_giftde_coum = 0;
        adult_record_bonus = 0L;
        last_draw_daily_reward_date = 0;
        extra_earnings = 0L;
        building_earnings_max_record = 0L;
        child_earnings_max_record = 0L;
        seven_days_login_cal_offset = 0;
        curTitle = null;
        curTitleShow = false;
        curPlayerSkin = 0L;
        is_set_prefab = 0L;
        pending_order_db_id = 0L;
        isMarsGoRouteDone = false;
        had_draw_vip_level_reward = 0L;
        had_draw_vip_level_recharge_reward = 0L;
        isStoreReviews = false;
        room_skin = 0L;
        giftde_child_graduate_count = 0;
    }

    public PlayerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        cname = rs.getString(3);
        icon = rs.getLong(4);
        iconBgk = rs.getLong(5);
        title = rs.getLong(6);
        bubble = rs.getLong(7);
        createTime = rs.getInt(8);
        create_date = rs.getInt(9);
        createRoleTime = rs.getInt(10);
        create_role_date = rs.getInt(11);
        lvl = rs.getInt(12);
        vipLvl = rs.getInt(13);
        gmLevel = rs.getInt(14);
        player_lang = rs.getString(15);
        last_login_date = rs.getInt(16);
        login_day_count = rs.getInt(17);
        last_take_version = rs.getLong(18);
        recharged_gem = rs.getLong(19);
        last_left_bag_time_s = rs.getInt(20);
        lastOfflineMs = rs.getLong(21);
        prefab = rs.getLong(22);
        is_set_default = rs.getLong(23);
        player_stage = rs.getLong(24);
        happy_value_cur_step_value = rs.getLong(25);
        last_login_week_tag = rs.getLong(26);
        week_login_day_count = rs.getLong(27);
        latest_login_time_ms = rs.getLong(28);
        uid = rs.getString(29);
        adfrom = rs.getString(30);
        adfrom2 = rs.getString(31);
        adid = rs.getString(32);
        clientPackageName = rs.getString(33);
        clientVerion = rs.getString(34);
        nation = rs.getString(35);
        ar_ip = rs.getString(36);
        sdkid = rs.getString(37);
        recharged_money = rs.getLong(38);
        power_max_record = rs.getLong(39);
        earnings_max_record = rs.getLong(40);
        market_next_refresh_ms = rs.getLong(41);
        last_log_section_day = rs.getInt(42);
        last_gain_visit_reward_day = rs.getInt(43);
        cute_actor_id = rs.getLong(44);
        consort_call_next_refresh_ms = rs.getLong(45);
        birth_giftde_coum = rs.getInt(46);
        adult_record_bonus = rs.getLong(47);
        last_draw_daily_reward_date = rs.getInt(48);
        extra_earnings = rs.getLong(49);
        building_earnings_max_record = rs.getLong(50);
        child_earnings_max_record = rs.getLong(51);
        seven_days_login_cal_offset = rs.getInt(52);
        curTitle = rs.getBytes(53);
        curTitleShow = rs.getBoolean(54);
        curPlayerSkin = rs.getLong(55);
        is_set_prefab = rs.getLong(56);
        pending_order_db_id = rs.getLong(57);
        isMarsGoRouteDone = rs.getBoolean(58);
        had_draw_vip_level_reward = rs.getLong(59);
        had_draw_vip_level_recharge_reward = rs.getLong(60);
        isStoreReviews = rs.getBoolean(61);
        room_skin = rs.getLong(62);
        giftde_child_graduate_count = rs.getInt(63);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `cname`, `icon`, `iconBgk`, `title`, `bubble`, `createTime`, `create_date`, `createRoleTime`, `create_role_date`, `lvl`, `vipLvl`, `gmLevel`, `player_lang`, `last_login_date`, `login_day_count`, `last_take_version`, `recharged_gem`, `last_left_bag_time_s`, `lastOfflineMs`, `prefab`, `is_set_default`, `player_stage`, `happy_value_cur_step_value`, `last_login_week_tag`, `week_login_day_count`, `latest_login_time_ms`, `uid`, `adfrom`, `adfrom2`, `adid`, `clientPackageName`, `clientVerion`, `nation`, `ar_ip`, `sdkid`, `recharged_money`, `power_max_record`, `earnings_max_record`, `market_next_refresh_ms`, `last_log_section_day`, `last_gain_visit_reward_day`, `cute_actor_id`, `consort_call_next_refresh_ms`, `birth_giftde_coum`, `adult_record_bonus`, `last_draw_daily_reward_date`, `extra_earnings`, `building_earnings_max_record`, `child_earnings_max_record`, `seven_days_login_cal_offset`, `curTitle`, `curTitleShow`, `curPlayerSkin`, `is_set_prefab`, `pending_order_db_id`, `isMarsGoRouteDone`, `had_draw_vip_level_reward`, `had_draw_vip_level_recharge_reward`, `isStoreReviews`, `room_skin`, `giftde_child_graduate_count`";
    }

    @Override
    public String getTableName() {
        return "`player`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(cname == null ? null : cname.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(icon).append("', ");
        strBuf.append("'").append(iconBgk).append("', ");
        strBuf.append("'").append(title).append("', ");
        strBuf.append("'").append(bubble).append("', ");
        strBuf.append("'").append(createTime).append("', ");
        strBuf.append("'").append(create_date).append("', ");
        strBuf.append("'").append(createRoleTime).append("', ");
        strBuf.append("'").append(create_role_date).append("', ");
        strBuf.append("'").append(lvl).append("', ");
        strBuf.append("'").append(vipLvl).append("', ");
        strBuf.append("'").append(gmLevel).append("', ");
        strBuf.append("'").append(player_lang == null ? null : player_lang.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(last_login_date).append("', ");
        strBuf.append("'").append(login_day_count).append("', ");
        strBuf.append("'").append(last_take_version).append("', ");
        strBuf.append("'").append(recharged_gem).append("', ");
        strBuf.append("'").append(last_left_bag_time_s).append("', ");
        strBuf.append("'").append(lastOfflineMs).append("', ");
        strBuf.append("'").append(prefab).append("', ");
        strBuf.append("'").append(is_set_default).append("', ");
        strBuf.append("'").append(player_stage).append("', ");
        strBuf.append("'").append(happy_value_cur_step_value).append("', ");
        strBuf.append("'").append(last_login_week_tag).append("', ");
        strBuf.append("'").append(week_login_day_count).append("', ");
        strBuf.append("'").append(latest_login_time_ms).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(clientPackageName == null ? null : clientPackageName.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(clientVerion == null ? null : clientVerion.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(ar_ip == null ? null : ar_ip.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdkid == null ? null : sdkid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(recharged_money).append("', ");
        strBuf.append("'").append(power_max_record).append("', ");
        strBuf.append("'").append(earnings_max_record).append("', ");
        strBuf.append("'").append(market_next_refresh_ms).append("', ");
        strBuf.append("'").append(last_log_section_day).append("', ");
        strBuf.append("'").append(last_gain_visit_reward_day).append("', ");
        strBuf.append("'").append(cute_actor_id).append("', ");
        strBuf.append("'").append(consort_call_next_refresh_ms).append("', ");
        strBuf.append("'").append(birth_giftde_coum).append("', ");
        strBuf.append("'").append(adult_record_bonus).append("', ");
        strBuf.append("'").append(last_draw_daily_reward_date).append("', ");
        strBuf.append("'").append(extra_earnings).append("', ");
        strBuf.append("'").append(building_earnings_max_record).append("', ");
        strBuf.append("'").append(child_earnings_max_record).append("', ");
        strBuf.append("'").append(seven_days_login_cal_offset).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(curTitleShow ? 1 : 0).append("', ");
        strBuf.append("'").append(curPlayerSkin).append("', ");
        strBuf.append("'").append(is_set_prefab).append("', ");
        strBuf.append("'").append(pending_order_db_id).append("', ");
        strBuf.append("'").append(isMarsGoRouteDone ? 1 : 0).append("', ");
        strBuf.append("'").append(had_draw_vip_level_reward).append("', ");
        strBuf.append("'").append(had_draw_vip_level_recharge_reward).append("', ");
        strBuf.append("'").append(isStoreReviews ? 1 : 0).append("', ");
        strBuf.append("'").append(room_skin).append("', ");
        strBuf.append("'").append(giftde_child_graduate_count).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(curTitle);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_curTitle)) ret.add(curTitle);         return ret;
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

    // 玩家名称
    public String getCname() { return this.cname; }
    public void setCname(BM _bm, String cname) {
        if(cname.equals(this.cname)) 
            return;
        this.cname = cname; 
        markField(_bm, FIELD_cname); 
    }
    public void saveCname(BM _bm, String cname) {
        if(cname.equals(this.cname)) 
            return;
        this.cname = cname;
        saveField(_bm, "cname", cname);
    }

    // 玩家头像
    public long getIcon() { return this.icon; }
    public void setIcon(BM _bm, long icon) {
        if(icon==this.icon) 
            return;
        this.icon = icon; 
        markField(_bm, FIELD_icon); 
    }
    public void saveIcon(BM _bm, long icon) {
        if(icon==this.icon) 
            return;
        this.icon = icon;
        saveField(_bm, "icon", icon);
    }

    // 玩家头像框
    public long getIconBgk() { return this.iconBgk; }
    public void setIconBgk(BM _bm, long iconBgk) {
        if(iconBgk==this.iconBgk) 
            return;
        this.iconBgk = iconBgk; 
        markField(_bm, FIELD_iconBgk); 
    }
    public void saveIconBgk(BM _bm, long iconBgk) {
        if(iconBgk==this.iconBgk) 
            return;
        this.iconBgk = iconBgk;
        saveField(_bm, "iconBgk", iconBgk);
    }

    // 玩家当前使用的称号
    public long getTitle() { return this.title; }
    public void setTitle(BM _bm, long title) {
        if(title==this.title) 
            return;
        this.title = title; 
        markField(_bm, FIELD_title); 
    }
    public void saveTitle(BM _bm, long title) {
        if(title==this.title) 
            return;
        this.title = title;
        saveField(_bm, "title", title);
    }

    // 玩家当前使用的气泡框
    public long getBubble() { return this.bubble; }
    public void setBubble(BM _bm, long bubble) {
        if(bubble==this.bubble) 
            return;
        this.bubble = bubble; 
        markField(_bm, FIELD_bubble); 
    }
    public void saveBubble(BM _bm, long bubble) {
        if(bubble==this.bubble) 
            return;
        this.bubble = bubble;
        saveField(_bm, "bubble", bubble);
    }

    // 账号创建时间(秒) 同ArTime
    public int getCreateTime() { return this.createTime; }
    public void setCreateTime(BM _bm, int createTime) {
        if(createTime==this.createTime) 
            return;
        this.createTime = createTime; 
        markField(_bm, FIELD_createTime); 
    }
    public void saveCreateTime(BM _bm, int createTime) {
        if(createTime==this.createTime) 
            return;
        this.createTime = createTime;
        saveField(_bm, "createTime", createTime);
    }

    // 账号创建日期 同ArTime
    public int getCreateDate() { return this.create_date; }
    public void setCreateDate(BM _bm, int create_date) {
        if(create_date==this.create_date) 
            return;
        this.create_date = create_date; 
        markField(_bm, FIELD_create_date); 
    }
    public void saveCreateDate(BM _bm, int create_date) {
        if(create_date==this.create_date) 
            return;
        this.create_date = create_date;
        saveField(_bm, "create_date", create_date);
    }

    // 创角时间(秒)
    public int getCreateRoleTime() { return this.createRoleTime; }
    public void setCreateRoleTime(BM _bm, int createRoleTime) {
        if(createRoleTime==this.createRoleTime) 
            return;
        this.createRoleTime = createRoleTime; 
        markField(_bm, FIELD_createRoleTime); 
    }
    public void saveCreateRoleTime(BM _bm, int createRoleTime) {
        if(createRoleTime==this.createRoleTime) 
            return;
        this.createRoleTime = createRoleTime;
        saveField(_bm, "createRoleTime", createRoleTime);
    }

    // 创角日期
    public int getCreateRoleDate() { return this.create_role_date; }
    public void setCreateRoleDate(BM _bm, int create_role_date) {
        if(create_role_date==this.create_role_date) 
            return;
        this.create_role_date = create_role_date; 
        markField(_bm, FIELD_create_role_date); 
    }
    public void saveCreateRoleDate(BM _bm, int create_role_date) {
        if(create_role_date==this.create_role_date) 
            return;
        this.create_role_date = create_role_date;
        saveField(_bm, "create_role_date", create_role_date);
    }

    // 玩家等级
    public int getLvl() { return this.lvl; }
    public void setLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl; 
        markField(_bm, FIELD_lvl); 
    }
    public void saveLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl;
        saveField(_bm, "lvl", lvl);
    }

    // VIP等级
    public int getVipLvl() { return this.vipLvl; }
    public void setVipLvl(BM _bm, int vipLvl) {
        if(vipLvl==this.vipLvl) 
            return;
        this.vipLvl = vipLvl; 
        markField(_bm, FIELD_vipLvl); 
    }
    public void saveVipLvl(BM _bm, int vipLvl) {
        if(vipLvl==this.vipLvl) 
            return;
        this.vipLvl = vipLvl;
        saveField(_bm, "vipLvl", vipLvl);
    }

    // gm权限
    public int getGmLevel() { return this.gmLevel; }
    public void setGmLevel(BM _bm, int gmLevel) {
        if(gmLevel==this.gmLevel) 
            return;
        this.gmLevel = gmLevel; 
        markField(_bm, FIELD_gmLevel); 
    }
    public void saveGmLevel(BM _bm, int gmLevel) {
        if(gmLevel==this.gmLevel) 
            return;
        this.gmLevel = gmLevel;
        saveField(_bm, "gmLevel", gmLevel);
    }

    // 玩家语言
    public String getPlayerLang() { return this.player_lang; }
    public void setPlayerLang(BM _bm, String player_lang) {
        if(player_lang.equals(this.player_lang)) 
            return;
        this.player_lang = player_lang; 
        markField(_bm, FIELD_player_lang); 
    }
    public void savePlayerLang(BM _bm, String player_lang) {
        if(player_lang.equals(this.player_lang)) 
            return;
        this.player_lang = player_lang;
        saveField(_bm, "player_lang", player_lang);
    }

    // 账号最后日期
    public int getLastLoginDate() { return this.last_login_date; }
    public void setLastLoginDate(BM _bm, int last_login_date) {
        if(last_login_date==this.last_login_date) 
            return;
        this.last_login_date = last_login_date; 
        markField(_bm, FIELD_last_login_date); 
    }
    public void saveLastLoginDate(BM _bm, int last_login_date) {
        if(last_login_date==this.last_login_date) 
            return;
        this.last_login_date = last_login_date;
        saveField(_bm, "last_login_date", last_login_date);
    }

    // 登录次数
    public int getLoginDayCount() { return this.login_day_count; }
    public void setLoginDayCount(BM _bm, int login_day_count) {
        if(login_day_count==this.login_day_count) 
            return;
        this.login_day_count = login_day_count; 
        markField(_bm, FIELD_login_day_count); 
    }
    public void saveLoginDayCount(BM _bm, int login_day_count) {
        if(login_day_count==this.login_day_count) 
            return;
        this.login_day_count = login_day_count;
        saveField(_bm, "login_day_count", login_day_count);
    }

    // 最后领取奖励的客户端版本
    public long getLastTakeVersion() { return this.last_take_version; }
    public void setLastTakeVersion(BM _bm, long last_take_version) {
        if(last_take_version==this.last_take_version) 
            return;
        this.last_take_version = last_take_version; 
        markField(_bm, FIELD_last_take_version); 
    }
    public void saveLastTakeVersion(BM _bm, long last_take_version) {
        if(last_take_version==this.last_take_version) 
            return;
        this.last_take_version = last_take_version;
        saveField(_bm, "last_take_version", last_take_version);
    }

    // 充值钻石数量
    public long getRechargedGem() { return this.recharged_gem; }
    public void setRechargedGem(BM _bm, long recharged_gem) {
        if(recharged_gem==this.recharged_gem) 
            return;
        this.recharged_gem = recharged_gem; 
        markField(_bm, FIELD_recharged_gem); 
    }
    public void saveRechargedGem(BM _bm, long recharged_gem) {
        if(recharged_gem==this.recharged_gem) 
            return;
        this.recharged_gem = recharged_gem;
        saveField(_bm, "recharged_gem", recharged_gem);
    }

    // 最后一次查看背包物品的时间
    public int getLastLeftBagTimeS() { return this.last_left_bag_time_s; }
    public void setLastLeftBagTimeS(BM _bm, int last_left_bag_time_s) {
        if(last_left_bag_time_s==this.last_left_bag_time_s) 
            return;
        this.last_left_bag_time_s = last_left_bag_time_s; 
        markField(_bm, FIELD_last_left_bag_time_s); 
    }
    public void saveLastLeftBagTimeS(BM _bm, int last_left_bag_time_s) {
        if(last_left_bag_time_s==this.last_left_bag_time_s) 
            return;
        this.last_left_bag_time_s = last_left_bag_time_s;
        saveField(_bm, "last_left_bag_time_s", last_left_bag_time_s);
    }

    // 最后一次下线时间（毫秒）
    public long getLastOfflineMs() { return this.lastOfflineMs; }
    public void setLastOfflineMs(BM _bm, long lastOfflineMs) {
        if(lastOfflineMs==this.lastOfflineMs) 
            return;
        this.lastOfflineMs = lastOfflineMs; 
        markField(_bm, FIELD_lastOfflineMs); 
    }
    public void saveLastOfflineMs(BM _bm, long lastOfflineMs) {
        if(lastOfflineMs==this.lastOfflineMs) 
            return;
        this.lastOfflineMs = lastOfflineMs;
        saveField(_bm, "lastOfflineMs", lastOfflineMs);
    }

    // 玩家预制形象配置ID
    public long getPrefab() { return this.prefab; }
    public void setPrefab(BM _bm, long prefab) {
        if(prefab==this.prefab) 
            return;
        this.prefab = prefab; 
        markField(_bm, FIELD_prefab); 
    }
    public void savePrefab(BM _bm, long prefab) {
        if(prefab==this.prefab) 
            return;
        this.prefab = prefab;
        saveField(_bm, "prefab", prefab);
    }

    // 是否创角 1-已设置
    public long getIsSetDefault() { return this.is_set_default; }
    public void setIsSetDefault(BM _bm, long is_set_default) {
        if(is_set_default==this.is_set_default) 
            return;
        this.is_set_default = is_set_default; 
        markField(_bm, FIELD_is_set_default); 
    }
    public void saveIsSetDefault(BM _bm, long is_set_default) {
        if(is_set_default==this.is_set_default) 
            return;
        this.is_set_default = is_set_default;
        saveField(_bm, "is_set_default", is_set_default);
    }

    // 玩家段位
    public long getPlayerStage() { return this.player_stage; }
    public void setPlayerStage(BM _bm, long player_stage) {
        if(player_stage==this.player_stage) 
            return;
        this.player_stage = player_stage; 
        markField(_bm, FIELD_player_stage); 
    }
    public void savePlayerStage(BM _bm, long player_stage) {
        if(player_stage==this.player_stage) 
            return;
        this.player_stage = player_stage;
        saveField(_bm, "player_stage", player_stage);
    }

    // 当前阶段数值
    public long getHappyValueCurStepValue() { return this.happy_value_cur_step_value; }
    public void setHappyValueCurStepValue(BM _bm, long happy_value_cur_step_value) {
        if(happy_value_cur_step_value==this.happy_value_cur_step_value) 
            return;
        this.happy_value_cur_step_value = happy_value_cur_step_value; 
        markField(_bm, FIELD_happy_value_cur_step_value); 
    }
    public void saveHappyValueCurStepValue(BM _bm, long happy_value_cur_step_value) {
        if(happy_value_cur_step_value==this.happy_value_cur_step_value) 
            return;
        this.happy_value_cur_step_value = happy_value_cur_step_value;
        saveField(_bm, "happy_value_cur_step_value", happy_value_cur_step_value);
    }

    // 最后一次登录周标记
    public long getLastLoginWeekTag() { return this.last_login_week_tag; }
    public void setLastLoginWeekTag(BM _bm, long last_login_week_tag) {
        if(last_login_week_tag==this.last_login_week_tag) 
            return;
        this.last_login_week_tag = last_login_week_tag; 
        markField(_bm, FIELD_last_login_week_tag); 
    }
    public void saveLastLoginWeekTag(BM _bm, long last_login_week_tag) {
        if(last_login_week_tag==this.last_login_week_tag) 
            return;
        this.last_login_week_tag = last_login_week_tag;
        saveField(_bm, "last_login_week_tag", last_login_week_tag);
    }

    // 本周登录天数
    public long getWeekLoginDayCount() { return this.week_login_day_count; }
    public void setWeekLoginDayCount(BM _bm, long week_login_day_count) {
        if(week_login_day_count==this.week_login_day_count) 
            return;
        this.week_login_day_count = week_login_day_count; 
        markField(_bm, FIELD_week_login_day_count); 
    }
    public void saveWeekLoginDayCount(BM _bm, long week_login_day_count) {
        if(week_login_day_count==this.week_login_day_count) 
            return;
        this.week_login_day_count = week_login_day_count;
        saveField(_bm, "week_login_day_count", week_login_day_count);
    }

    // 最近一次登陆时间（毫秒）
    public long getLatestLoginTimeMs() { return this.latest_login_time_ms; }
    public void setLatestLoginTimeMs(BM _bm, long latest_login_time_ms) {
        if(latest_login_time_ms==this.latest_login_time_ms) 
            return;
        this.latest_login_time_ms = latest_login_time_ms; 
        markField(_bm, FIELD_latest_login_time_ms); 
    }
    public void saveLatestLoginTimeMs(BM _bm, long latest_login_time_ms) {
        if(latest_login_time_ms==this.latest_login_time_ms) 
            return;
        this.latest_login_time_ms = latest_login_time_ms;
        saveField(_bm, "latest_login_time_ms", latest_login_time_ms);
    }

    // 平台账号
    public String getUid() { return this.uid; }
    public void setUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 渠道
    public String getAdfrom() { return this.adfrom; }
    public void setAdfrom(BM _bm, String adfrom) {
        if(adfrom.equals(this.adfrom)) 
            return;
        this.adfrom = adfrom; 
        markField(_bm, FIELD_adfrom); 
    }
    public void saveAdfrom(BM _bm, String adfrom) {
        if(adfrom.equals(this.adfrom)) 
            return;
        this.adfrom = adfrom;
        saveField(_bm, "adfrom", adfrom);
    }

    // 二级渠道
    public String getAdfrom2() { return this.adfrom2; }
    public void setAdfrom2(BM _bm, String adfrom2) {
        if(adfrom2.equals(this.adfrom2)) 
            return;
        this.adfrom2 = adfrom2; 
        markField(_bm, FIELD_adfrom2); 
    }
    public void saveAdfrom2(BM _bm, String adfrom2) {
        if(adfrom2.equals(this.adfrom2)) 
            return;
        this.adfrom2 = adfrom2;
        saveField(_bm, "adfrom2", adfrom2);
    }

    // 设备id
    public String getAdid() { return this.adid; }
    public void setAdid(BM _bm, String adid) {
        if(adid.equals(this.adid)) 
            return;
        this.adid = adid; 
        markField(_bm, FIELD_adid); 
    }
    public void saveAdid(BM _bm, String adid) {
        if(adid.equals(this.adid)) 
            return;
        this.adid = adid;
        saveField(_bm, "adid", adid);
    }

    // 客户端包名
    public String getClientPackageName() { return this.clientPackageName; }
    public void setClientPackageName(BM _bm, String clientPackageName) {
        if(clientPackageName.equals(this.clientPackageName)) 
            return;
        this.clientPackageName = clientPackageName; 
        markField(_bm, FIELD_clientPackageName); 
    }
    public void saveClientPackageName(BM _bm, String clientPackageName) {
        if(clientPackageName.equals(this.clientPackageName)) 
            return;
        this.clientPackageName = clientPackageName;
        saveField(_bm, "clientPackageName", clientPackageName);
    }

    // 客户端版本
    public String getClientVerion() { return this.clientVerion; }
    public void setClientVerion(BM _bm, String clientVerion) {
        if(clientVerion.equals(this.clientVerion)) 
            return;
        this.clientVerion = clientVerion; 
        markField(_bm, FIELD_clientVerion); 
    }
    public void saveClientVerion(BM _bm, String clientVerion) {
        if(clientVerion.equals(this.clientVerion)) 
            return;
        this.clientVerion = clientVerion;
        saveField(_bm, "clientVerion", clientVerion);
    }

    // 国家
    public String getNation() { return this.nation; }
    public void setNation(BM _bm, String nation) {
        if(nation.equals(this.nation)) 
            return;
        this.nation = nation; 
        markField(_bm, FIELD_nation); 
    }
    public void saveNation(BM _bm, String nation) {
        if(nation.equals(this.nation)) 
            return;
        this.nation = nation;
        saveField(_bm, "nation", nation);
    }

    // 注册IP
    public String getArIp() { return this.ar_ip; }
    public void setArIp(BM _bm, String ar_ip) {
        if(ar_ip.equals(this.ar_ip)) 
            return;
        this.ar_ip = ar_ip; 
        markField(_bm, FIELD_ar_ip); 
    }
    public void saveArIp(BM _bm, String ar_ip) {
        if(ar_ip.equals(this.ar_ip)) 
            return;
        this.ar_ip = ar_ip;
        saveField(_bm, "ar_ip", ar_ip);
    }

    // 渠道账号
    public String getSdkid() { return this.sdkid; }
    public void setSdkid(BM _bm, String sdkid) {
        if(sdkid.equals(this.sdkid)) 
            return;
        this.sdkid = sdkid; 
        markField(_bm, FIELD_sdkid); 
    }
    public void saveSdkid(BM _bm, String sdkid) {
        if(sdkid.equals(this.sdkid)) 
            return;
        this.sdkid = sdkid;
        saveField(_bm, "sdkid", sdkid);
    }

    // 充值货币金额
    public long getRechargedMoney() { return this.recharged_money; }
    public void setRechargedMoney(BM _bm, long recharged_money) {
        if(recharged_money==this.recharged_money) 
            return;
        this.recharged_money = recharged_money; 
        markField(_bm, FIELD_recharged_money); 
    }
    public void saveRechargedMoney(BM _bm, long recharged_money) {
        if(recharged_money==this.recharged_money) 
            return;
        this.recharged_money = recharged_money;
        saveField(_bm, "recharged_money", recharged_money);
    }

    // 玩家实力历史记录最高值
    public long getPowerMaxRecord() { return this.power_max_record; }
    public void setPowerMaxRecord(BM _bm, long power_max_record) {
        if(power_max_record==this.power_max_record) 
            return;
        this.power_max_record = power_max_record; 
        markField(_bm, FIELD_power_max_record); 
    }
    public void savePowerMaxRecord(BM _bm, long power_max_record) {
        if(power_max_record==this.power_max_record) 
            return;
        this.power_max_record = power_max_record;
        saveField(_bm, "power_max_record", power_max_record);
    }

    // 玩家赚速历史记录最高值
    public long getEarningsMaxRecord() { return this.earnings_max_record; }
    public void setEarningsMaxRecord(BM _bm, long earnings_max_record) {
        if(earnings_max_record==this.earnings_max_record) 
            return;
        this.earnings_max_record = earnings_max_record; 
        markField(_bm, FIELD_earnings_max_record); 
    }
    public void saveEarningsMaxRecord(BM _bm, long earnings_max_record) {
        if(earnings_max_record==this.earnings_max_record) 
            return;
        this.earnings_max_record = earnings_max_record;
        saveField(_bm, "earnings_max_record", earnings_max_record);
    }

    // 集市下次刷新时间
    public long getMarketNextRefreshMs() { return this.market_next_refresh_ms; }
    public void setMarketNextRefreshMs(BM _bm, long market_next_refresh_ms) {
        if(market_next_refresh_ms==this.market_next_refresh_ms) 
            return;
        this.market_next_refresh_ms = market_next_refresh_ms; 
        markField(_bm, FIELD_market_next_refresh_ms); 
    }
    public void saveMarketNextRefreshMs(BM _bm, long market_next_refresh_ms) {
        if(market_next_refresh_ms==this.market_next_refresh_ms) 
            return;
        this.market_next_refresh_ms = market_next_refresh_ms;
        saveField(_bm, "market_next_refresh_ms", market_next_refresh_ms);
    }

    // 上次截面日志记录日期
    public int getLastLogSectionDay() { return this.last_log_section_day; }
    public void setLastLogSectionDay(BM _bm, int last_log_section_day) {
        if(last_log_section_day==this.last_log_section_day) 
            return;
        this.last_log_section_day = last_log_section_day; 
        markField(_bm, FIELD_last_log_section_day); 
    }
    public void saveLastLogSectionDay(BM _bm, int last_log_section_day) {
        if(last_log_section_day==this.last_log_section_day) 
            return;
        this.last_log_section_day = last_log_section_day;
        saveField(_bm, "last_log_section_day", last_log_section_day);
    }

    // 上次领取拜访其他玩家奖励日期
    public int getLastGainVisitRewardDay() { return this.last_gain_visit_reward_day; }
    public void setLastGainVisitRewardDay(BM _bm, int last_gain_visit_reward_day) {
        if(last_gain_visit_reward_day==this.last_gain_visit_reward_day) 
            return;
        this.last_gain_visit_reward_day = last_gain_visit_reward_day; 
        markField(_bm, FIELD_last_gain_visit_reward_day); 
    }
    public void saveLastGainVisitRewardDay(BM _bm, int last_gain_visit_reward_day) {
        if(last_gain_visit_reward_day==this.last_gain_visit_reward_day) 
            return;
        this.last_gain_visit_reward_day = last_gain_visit_reward_day;
        saveField(_bm, "last_gain_visit_reward_day", last_gain_visit_reward_day);
    }

    // Q版形象ID
    public long getCuteActorId() { return this.cute_actor_id; }
    public void setCuteActorId(BM _bm, long cute_actor_id) {
        if(cute_actor_id==this.cute_actor_id) 
            return;
        this.cute_actor_id = cute_actor_id; 
        markField(_bm, FIELD_cute_actor_id); 
    }
    public void saveCuteActorId(BM _bm, long cute_actor_id) {
        if(cute_actor_id==this.cute_actor_id) 
            return;
        this.cute_actor_id = cute_actor_id;
        saveField(_bm, "cute_actor_id", cute_actor_id);
    }

    // 家人指定邀约下次刷新时间（针对需要计数的数据）
    public long getConsortCallNextRefreshMs() { return this.consort_call_next_refresh_ms; }
    public void setConsortCallNextRefreshMs(BM _bm, long consort_call_next_refresh_ms) {
        if(consort_call_next_refresh_ms==this.consort_call_next_refresh_ms) 
            return;
        this.consort_call_next_refresh_ms = consort_call_next_refresh_ms; 
        markField(_bm, FIELD_consort_call_next_refresh_ms); 
    }
    public void saveConsortCallNextRefreshMs(BM _bm, long consort_call_next_refresh_ms) {
        if(consort_call_next_refresh_ms==this.consort_call_next_refresh_ms) 
            return;
        this.consort_call_next_refresh_ms = consort_call_next_refresh_ms;
        saveField(_bm, "consort_call_next_refresh_ms", consort_call_next_refresh_ms);
    }

    // 下次出生卷王次数
    public int getBirthGiftdeCoum() { return this.birth_giftde_coum; }
    public void setBirthGiftdeCoum(BM _bm, int birth_giftde_coum) {
        if(birth_giftde_coum==this.birth_giftde_coum) 
            return;
        this.birth_giftde_coum = birth_giftde_coum; 
        markField(_bm, FIELD_birth_giftde_coum); 
    }
    public void saveBirthGiftdeCoum(BM _bm, int birth_giftde_coum) {
        if(birth_giftde_coum==this.birth_giftde_coum) 
            return;
        this.birth_giftde_coum = birth_giftde_coum;
        saveField(_bm, "birth_giftde_coum", birth_giftde_coum);
    }

    // 子嗣记录收益总值（因为移除记录在玩家身上）
    public long getAdultRecordBonus() { return this.adult_record_bonus; }
    public void setAdultRecordBonus(BM _bm, long adult_record_bonus) {
        if(adult_record_bonus==this.adult_record_bonus) 
            return;
        this.adult_record_bonus = adult_record_bonus; 
        markField(_bm, FIELD_adult_record_bonus); 
    }
    public void saveAdultRecordBonus(BM _bm, long adult_record_bonus) {
        if(adult_record_bonus==this.adult_record_bonus) 
            return;
        this.adult_record_bonus = adult_record_bonus;
        saveField(_bm, "adult_record_bonus", adult_record_bonus);
    }

    // 最后一次领取每日奖励的日期标记
    public int getLastDrawDailyRewardDate() { return this.last_draw_daily_reward_date; }
    public void setLastDrawDailyRewardDate(BM _bm, int last_draw_daily_reward_date) {
        if(last_draw_daily_reward_date==this.last_draw_daily_reward_date) 
            return;
        this.last_draw_daily_reward_date = last_draw_daily_reward_date; 
        markField(_bm, FIELD_last_draw_daily_reward_date); 
    }
    public void saveLastDrawDailyRewardDate(BM _bm, int last_draw_daily_reward_date) {
        if(last_draw_daily_reward_date==this.last_draw_daily_reward_date) 
            return;
        this.last_draw_daily_reward_date = last_draw_daily_reward_date;
        saveField(_bm, "last_draw_daily_reward_date", last_draw_daily_reward_date);
    }

    // 额外赚速
    public long getExtraEarnings() { return this.extra_earnings; }
    public void setExtraEarnings(BM _bm, long extra_earnings) {
        if(extra_earnings==this.extra_earnings) 
            return;
        this.extra_earnings = extra_earnings; 
        markField(_bm, FIELD_extra_earnings); 
    }
    public void saveExtraEarnings(BM _bm, long extra_earnings) {
        if(extra_earnings==this.extra_earnings) 
            return;
        this.extra_earnings = extra_earnings;
        saveField(_bm, "extra_earnings", extra_earnings);
    }

    // 建筑赚速历史记录最高值
    public long getBuildingEarningsMaxRecord() { return this.building_earnings_max_record; }
    public void setBuildingEarningsMaxRecord(BM _bm, long building_earnings_max_record) {
        if(building_earnings_max_record==this.building_earnings_max_record) 
            return;
        this.building_earnings_max_record = building_earnings_max_record; 
        markField(_bm, FIELD_building_earnings_max_record); 
    }
    public void saveBuildingEarningsMaxRecord(BM _bm, long building_earnings_max_record) {
        if(building_earnings_max_record==this.building_earnings_max_record) 
            return;
        this.building_earnings_max_record = building_earnings_max_record;
        saveField(_bm, "building_earnings_max_record", building_earnings_max_record);
    }

    // 子嗣赚速历史记录最高值
    public long getChildEarningsMaxRecord() { return this.child_earnings_max_record; }
    public void setChildEarningsMaxRecord(BM _bm, long child_earnings_max_record) {
        if(child_earnings_max_record==this.child_earnings_max_record) 
            return;
        this.child_earnings_max_record = child_earnings_max_record; 
        markField(_bm, FIELD_child_earnings_max_record); 
    }
    public void saveChildEarningsMaxRecord(BM _bm, long child_earnings_max_record) {
        if(child_earnings_max_record==this.child_earnings_max_record) 
            return;
        this.child_earnings_max_record = child_earnings_max_record;
        saveField(_bm, "child_earnings_max_record", child_earnings_max_record);
    }

    // 七天登录计算偏移量
    public int getSevenDaysLoginCalOffset() { return this.seven_days_login_cal_offset; }
    public void setSevenDaysLoginCalOffset(BM _bm, int seven_days_login_cal_offset) {
        if(seven_days_login_cal_offset==this.seven_days_login_cal_offset) 
            return;
        this.seven_days_login_cal_offset = seven_days_login_cal_offset; 
        markField(_bm, FIELD_seven_days_login_cal_offset); 
    }
    public void saveSevenDaysLoginCalOffset(BM _bm, int seven_days_login_cal_offset) {
        if(seven_days_login_cal_offset==this.seven_days_login_cal_offset) 
            return;
        this.seven_days_login_cal_offset = seven_days_login_cal_offset;
        saveField(_bm, "seven_days_login_cal_offset", seven_days_login_cal_offset);
    }

    // 当前称号
    public byte[] getCurTitle() { return this.curTitle; }
    public void setCurTitle(BM _bm, byte[] curTitle) {
        if(curTitle==this.curTitle) 
            return;
        this.curTitle = curTitle; 
        markField(_bm, FIELD_curTitle); 
    }
    public void saveCurTitle(BM _bm, byte[] curTitle) {
        if(curTitle==this.curTitle) 
            return;
        this.curTitle = curTitle;
        saveFieldBytes(_bm, "curTitle", curTitle);
    }

    // 当前称号是否对外展示
    public boolean getCurTitleShow() { return this.curTitleShow; }
    public void setCurTitleShow(BM _bm, boolean curTitleShow) {
        if(curTitleShow==this.curTitleShow) 
            return;
        this.curTitleShow = curTitleShow; 
        markField(_bm, FIELD_curTitleShow); 
    }
    public void saveCurTitleShow(BM _bm, boolean curTitleShow) {
        if(curTitleShow==this.curTitleShow) 
            return;
        this.curTitleShow = curTitleShow;
        saveField(_bm, "curTitleShow", curTitleShow ? 1 : 0);
    }

    // 当前玩家皮肤
    public long getCurPlayerSkin() { return this.curPlayerSkin; }
    public void setCurPlayerSkin(BM _bm, long curPlayerSkin) {
        if(curPlayerSkin==this.curPlayerSkin) 
            return;
        this.curPlayerSkin = curPlayerSkin; 
        markField(_bm, FIELD_curPlayerSkin); 
    }
    public void saveCurPlayerSkin(BM _bm, long curPlayerSkin) {
        if(curPlayerSkin==this.curPlayerSkin) 
            return;
        this.curPlayerSkin = curPlayerSkin;
        saveField(_bm, "curPlayerSkin", curPlayerSkin);
    }

    // 是否设置预制形象 1-已设置
    public long getIsSetPrefab() { return this.is_set_prefab; }
    public void setIsSetPrefab(BM _bm, long is_set_prefab) {
        if(is_set_prefab==this.is_set_prefab) 
            return;
        this.is_set_prefab = is_set_prefab; 
        markField(_bm, FIELD_is_set_prefab); 
    }
    public void saveIsSetPrefab(BM _bm, long is_set_prefab) {
        if(is_set_prefab==this.is_set_prefab) 
            return;
        this.is_set_prefab = is_set_prefab;
        saveField(_bm, "is_set_prefab", is_set_prefab);
    }

    // 待处理的订单数据ID
    public long getPendingOrderDbId() { return this.pending_order_db_id; }
    public void setPendingOrderDbId(BM _bm, long pending_order_db_id) {
        if(pending_order_db_id==this.pending_order_db_id) 
            return;
        this.pending_order_db_id = pending_order_db_id; 
        markField(_bm, FIELD_pending_order_db_id); 
    }
    public void savePendingOrderDbId(BM _bm, long pending_order_db_id) {
        if(pending_order_db_id==this.pending_order_db_id) 
            return;
        this.pending_order_db_id = pending_order_db_id;
        saveField(_bm, "pending_order_db_id", pending_order_db_id);
    }

    // 火星-前往火星全部阶段完成
    public boolean getIsMarsGoRouteDone() { return this.isMarsGoRouteDone; }
    public void setIsMarsGoRouteDone(BM _bm, boolean isMarsGoRouteDone) {
        if(isMarsGoRouteDone==this.isMarsGoRouteDone) 
            return;
        this.isMarsGoRouteDone = isMarsGoRouteDone; 
        markField(_bm, FIELD_isMarsGoRouteDone); 
    }
    public void saveIsMarsGoRouteDone(BM _bm, boolean isMarsGoRouteDone) {
        if(isMarsGoRouteDone==this.isMarsGoRouteDone) 
            return;
        this.isMarsGoRouteDone = isMarsGoRouteDone;
        saveField(_bm, "isMarsGoRouteDone", isMarsGoRouteDone ? 1 : 0);
    }

    // VIP等级奖励领取记录
    public long getHadDrawVipLevelReward() { return this.had_draw_vip_level_reward; }
    public void setHadDrawVipLevelReward(BM _bm, long had_draw_vip_level_reward) {
        if(had_draw_vip_level_reward==this.had_draw_vip_level_reward) 
            return;
        this.had_draw_vip_level_reward = had_draw_vip_level_reward; 
        markField(_bm, FIELD_had_draw_vip_level_reward); 
    }
    public void saveHadDrawVipLevelReward(BM _bm, long had_draw_vip_level_reward) {
        if(had_draw_vip_level_reward==this.had_draw_vip_level_reward) 
            return;
        this.had_draw_vip_level_reward = had_draw_vip_level_reward;
        saveField(_bm, "had_draw_vip_level_reward", had_draw_vip_level_reward);
    }

    // VIP等级充值奖励领取记录
    public long getHadDrawVipLevelRechargeReward() { return this.had_draw_vip_level_recharge_reward; }
    public void setHadDrawVipLevelRechargeReward(BM _bm, long had_draw_vip_level_recharge_reward) {
        if(had_draw_vip_level_recharge_reward==this.had_draw_vip_level_recharge_reward) 
            return;
        this.had_draw_vip_level_recharge_reward = had_draw_vip_level_recharge_reward; 
        markField(_bm, FIELD_had_draw_vip_level_recharge_reward); 
    }
    public void saveHadDrawVipLevelRechargeReward(BM _bm, long had_draw_vip_level_recharge_reward) {
        if(had_draw_vip_level_recharge_reward==this.had_draw_vip_level_recharge_reward) 
            return;
        this.had_draw_vip_level_recharge_reward = had_draw_vip_level_recharge_reward;
        saveField(_bm, "had_draw_vip_level_recharge_reward", had_draw_vip_level_recharge_reward);
    }

    // 是否商店评价
    public boolean getIsStoreReviews() { return this.isStoreReviews; }
    public void setIsStoreReviews(BM _bm, boolean isStoreReviews) {
        if(isStoreReviews==this.isStoreReviews) 
            return;
        this.isStoreReviews = isStoreReviews; 
        markField(_bm, FIELD_isStoreReviews); 
    }
    public void saveIsStoreReviews(BM _bm, boolean isStoreReviews) {
        if(isStoreReviews==this.isStoreReviews) 
            return;
        this.isStoreReviews = isStoreReviews;
        saveField(_bm, "isStoreReviews", isStoreReviews ? 1 : 0);
    }

    // 房间皮肤ID
    public long getRoomSkin() { return this.room_skin; }
    public void setRoomSkin(BM _bm, long room_skin) {
        if(room_skin==this.room_skin) 
            return;
        this.room_skin = room_skin; 
        markField(_bm, FIELD_room_skin); 
    }
    public void saveRoomSkin(BM _bm, long room_skin) {
        if(room_skin==this.room_skin) 
            return;
        this.room_skin = room_skin;
        saveField(_bm, "room_skin", room_skin);
    }

    // 卷王子嗣毕业历史数量
    public int getGiftdeChildGraduateCount() { return this.giftde_child_graduate_count; }
    public void setGiftdeChildGraduateCount(BM _bm, int giftde_child_graduate_count) {
        if(giftde_child_graduate_count==this.giftde_child_graduate_count) 
            return;
        this.giftde_child_graduate_count = giftde_child_graduate_count; 
        markField(_bm, FIELD_giftde_child_graduate_count); 
    }
    public void saveGiftdeChildGraduateCount(BM _bm, int giftde_child_graduate_count) {
        if(giftde_child_graduate_count==this.giftde_child_graduate_count) 
            return;
        this.giftde_child_graduate_count = giftde_child_graduate_count;
        saveField(_bm, "giftde_child_graduate_count", giftde_child_graduate_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `cname` = '").append(cname == null ? null : cname.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `icon` = '").append(icon).append("',");
        sBuilder.append(" `iconBgk` = '").append(iconBgk).append("',");
        sBuilder.append(" `title` = '").append(title).append("',");
        sBuilder.append(" `bubble` = '").append(bubble).append("',");
        sBuilder.append(" `createTime` = '").append(createTime).append("',");
        sBuilder.append(" `create_date` = '").append(create_date).append("',");
        sBuilder.append(" `createRoleTime` = '").append(createRoleTime).append("',");
        sBuilder.append(" `create_role_date` = '").append(create_role_date).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.append(" `vipLvl` = '").append(vipLvl).append("',");
        sBuilder.append(" `gmLevel` = '").append(gmLevel).append("',");
        sBuilder.append(" `player_lang` = '").append(player_lang == null ? null : player_lang.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `last_login_date` = '").append(last_login_date).append("',");
        sBuilder.append(" `login_day_count` = '").append(login_day_count).append("',");
        sBuilder.append(" `last_take_version` = '").append(last_take_version).append("',");
        sBuilder.append(" `recharged_gem` = '").append(recharged_gem).append("',");
        sBuilder.append(" `last_left_bag_time_s` = '").append(last_left_bag_time_s).append("',");
        sBuilder.append(" `lastOfflineMs` = '").append(lastOfflineMs).append("',");
        sBuilder.append(" `prefab` = '").append(prefab).append("',");
        sBuilder.append(" `is_set_default` = '").append(is_set_default).append("',");
        sBuilder.append(" `player_stage` = '").append(player_stage).append("',");
        sBuilder.append(" `happy_value_cur_step_value` = '").append(happy_value_cur_step_value).append("',");
        sBuilder.append(" `last_login_week_tag` = '").append(last_login_week_tag).append("',");
        sBuilder.append(" `week_login_day_count` = '").append(week_login_day_count).append("',");
        sBuilder.append(" `latest_login_time_ms` = '").append(latest_login_time_ms).append("',");
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `clientPackageName` = '").append(clientPackageName == null ? null : clientPackageName.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `clientVerion` = '").append(clientVerion == null ? null : clientVerion.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `ar_ip` = '").append(ar_ip == null ? null : ar_ip.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdkid` = '").append(sdkid == null ? null : sdkid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `recharged_money` = '").append(recharged_money).append("',");
        sBuilder.append(" `power_max_record` = '").append(power_max_record).append("',");
        sBuilder.append(" `earnings_max_record` = '").append(earnings_max_record).append("',");
        sBuilder.append(" `market_next_refresh_ms` = '").append(market_next_refresh_ms).append("',");
        sBuilder.append(" `last_log_section_day` = '").append(last_log_section_day).append("',");
        sBuilder.append(" `last_gain_visit_reward_day` = '").append(last_gain_visit_reward_day).append("',");
        sBuilder.append(" `cute_actor_id` = '").append(cute_actor_id).append("',");
        sBuilder.append(" `consort_call_next_refresh_ms` = '").append(consort_call_next_refresh_ms).append("',");
        sBuilder.append(" `birth_giftde_coum` = '").append(birth_giftde_coum).append("',");
        sBuilder.append(" `adult_record_bonus` = '").append(adult_record_bonus).append("',");
        sBuilder.append(" `last_draw_daily_reward_date` = '").append(last_draw_daily_reward_date).append("',");
        sBuilder.append(" `extra_earnings` = '").append(extra_earnings).append("',");
        sBuilder.append(" `building_earnings_max_record` = '").append(building_earnings_max_record).append("',");
        sBuilder.append(" `child_earnings_max_record` = '").append(child_earnings_max_record).append("',");
        sBuilder.append(" `seven_days_login_cal_offset` = '").append(seven_days_login_cal_offset).append("',");
        sBuilder.append(" `curTitle` = ?,");
        sBuilder.append(" `curTitleShow` = '").append(curTitleShow ? 1 : 0).append("',");
        sBuilder.append(" `curPlayerSkin` = '").append(curPlayerSkin).append("',");
        sBuilder.append(" `is_set_prefab` = '").append(is_set_prefab).append("',");
        sBuilder.append(" `pending_order_db_id` = '").append(pending_order_db_id).append("',");
        sBuilder.append(" `isMarsGoRouteDone` = '").append(isMarsGoRouteDone ? 1 : 0).append("',");
        sBuilder.append(" `had_draw_vip_level_reward` = '").append(had_draw_vip_level_reward).append("',");
        sBuilder.append(" `had_draw_vip_level_recharge_reward` = '").append(had_draw_vip_level_recharge_reward).append("',");
        sBuilder.append(" `isStoreReviews` = '").append(isStoreReviews ? 1 : 0).append("',");
        sBuilder.append(" `room_skin` = '").append(room_skin).append("',");
        sBuilder.append(" `giftde_child_graduate_count` = '").append(giftde_child_graduate_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_cname)) sBuilder.append(" `cname` = '").append(cname == null ? null : cname.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_icon)) sBuilder.append(" `icon` = '").append(icon).append("',");
        if(isFieldMarked(FIELD_iconBgk)) sBuilder.append(" `iconBgk` = '").append(iconBgk).append("',");
        if(isFieldMarked(FIELD_title)) sBuilder.append(" `title` = '").append(title).append("',");
        if(isFieldMarked(FIELD_bubble)) sBuilder.append(" `bubble` = '").append(bubble).append("',");
        if(isFieldMarked(FIELD_createTime)) sBuilder.append(" `createTime` = '").append(createTime).append("',");
        if(isFieldMarked(FIELD_create_date)) sBuilder.append(" `create_date` = '").append(create_date).append("',");
        if(isFieldMarked(FIELD_createRoleTime)) sBuilder.append(" `createRoleTime` = '").append(createRoleTime).append("',");
        if(isFieldMarked(FIELD_create_role_date)) sBuilder.append(" `create_role_date` = '").append(create_role_date).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        if(isFieldMarked(FIELD_vipLvl)) sBuilder.append(" `vipLvl` = '").append(vipLvl).append("',");
        if(isFieldMarked(FIELD_gmLevel)) sBuilder.append(" `gmLevel` = '").append(gmLevel).append("',");
        if(isFieldMarked(FIELD_player_lang)) sBuilder.append(" `player_lang` = '").append(player_lang == null ? null : player_lang.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_last_login_date)) sBuilder.append(" `last_login_date` = '").append(last_login_date).append("',");
        if(isFieldMarked(FIELD_login_day_count)) sBuilder.append(" `login_day_count` = '").append(login_day_count).append("',");
        if(isFieldMarked(FIELD_last_take_version)) sBuilder.append(" `last_take_version` = '").append(last_take_version).append("',");
        if(isFieldMarked(FIELD_recharged_gem)) sBuilder.append(" `recharged_gem` = '").append(recharged_gem).append("',");
        if(isFieldMarked(FIELD_last_left_bag_time_s)) sBuilder.append(" `last_left_bag_time_s` = '").append(last_left_bag_time_s).append("',");
        if(isFieldMarked(FIELD_lastOfflineMs)) sBuilder.append(" `lastOfflineMs` = '").append(lastOfflineMs).append("',");
        if(isFieldMarked(FIELD_prefab)) sBuilder.append(" `prefab` = '").append(prefab).append("',");
        if(isFieldMarked(FIELD_is_set_default)) sBuilder.append(" `is_set_default` = '").append(is_set_default).append("',");
        if(isFieldMarked(FIELD_player_stage)) sBuilder.append(" `player_stage` = '").append(player_stage).append("',");
        if(isFieldMarked(FIELD_happy_value_cur_step_value)) sBuilder.append(" `happy_value_cur_step_value` = '").append(happy_value_cur_step_value).append("',");
        if(isFieldMarked(FIELD_last_login_week_tag)) sBuilder.append(" `last_login_week_tag` = '").append(last_login_week_tag).append("',");
        if(isFieldMarked(FIELD_week_login_day_count)) sBuilder.append(" `week_login_day_count` = '").append(week_login_day_count).append("',");
        if(isFieldMarked(FIELD_latest_login_time_ms)) sBuilder.append(" `latest_login_time_ms` = '").append(latest_login_time_ms).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom)) sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom2)) sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adid)) sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_clientPackageName)) sBuilder.append(" `clientPackageName` = '").append(clientPackageName == null ? null : clientPackageName.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_clientVerion)) sBuilder.append(" `clientVerion` = '").append(clientVerion == null ? null : clientVerion.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_nation)) sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_ar_ip)) sBuilder.append(" `ar_ip` = '").append(ar_ip == null ? null : ar_ip.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdkid)) sBuilder.append(" `sdkid` = '").append(sdkid == null ? null : sdkid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_recharged_money)) sBuilder.append(" `recharged_money` = '").append(recharged_money).append("',");
        if(isFieldMarked(FIELD_power_max_record)) sBuilder.append(" `power_max_record` = '").append(power_max_record).append("',");
        if(isFieldMarked(FIELD_earnings_max_record)) sBuilder.append(" `earnings_max_record` = '").append(earnings_max_record).append("',");
        if(isFieldMarked(FIELD_market_next_refresh_ms)) sBuilder.append(" `market_next_refresh_ms` = '").append(market_next_refresh_ms).append("',");
        if(isFieldMarked(FIELD_last_log_section_day)) sBuilder.append(" `last_log_section_day` = '").append(last_log_section_day).append("',");
        if(isFieldMarked(FIELD_last_gain_visit_reward_day)) sBuilder.append(" `last_gain_visit_reward_day` = '").append(last_gain_visit_reward_day).append("',");
        if(isFieldMarked(FIELD_cute_actor_id)) sBuilder.append(" `cute_actor_id` = '").append(cute_actor_id).append("',");
        if(isFieldMarked(FIELD_consort_call_next_refresh_ms)) sBuilder.append(" `consort_call_next_refresh_ms` = '").append(consort_call_next_refresh_ms).append("',");
        if(isFieldMarked(FIELD_birth_giftde_coum)) sBuilder.append(" `birth_giftde_coum` = '").append(birth_giftde_coum).append("',");
        if(isFieldMarked(FIELD_adult_record_bonus)) sBuilder.append(" `adult_record_bonus` = '").append(adult_record_bonus).append("',");
        if(isFieldMarked(FIELD_last_draw_daily_reward_date)) sBuilder.append(" `last_draw_daily_reward_date` = '").append(last_draw_daily_reward_date).append("',");
        if(isFieldMarked(FIELD_extra_earnings)) sBuilder.append(" `extra_earnings` = '").append(extra_earnings).append("',");
        if(isFieldMarked(FIELD_building_earnings_max_record)) sBuilder.append(" `building_earnings_max_record` = '").append(building_earnings_max_record).append("',");
        if(isFieldMarked(FIELD_child_earnings_max_record)) sBuilder.append(" `child_earnings_max_record` = '").append(child_earnings_max_record).append("',");
        if(isFieldMarked(FIELD_seven_days_login_cal_offset)) sBuilder.append(" `seven_days_login_cal_offset` = '").append(seven_days_login_cal_offset).append("',");
        if(isFieldMarked(FIELD_curTitle)) sBuilder.append(" `curTitle` = ?,");
        if(isFieldMarked(FIELD_curTitleShow)) sBuilder.append(" `curTitleShow` = '").append(curTitleShow ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_curPlayerSkin)) sBuilder.append(" `curPlayerSkin` = '").append(curPlayerSkin).append("',");
        if(isFieldMarked(FIELD_is_set_prefab)) sBuilder.append(" `is_set_prefab` = '").append(is_set_prefab).append("',");
        if(isFieldMarked(FIELD_pending_order_db_id)) sBuilder.append(" `pending_order_db_id` = '").append(pending_order_db_id).append("',");
        if(isFieldMarked(FIELD_isMarsGoRouteDone)) sBuilder.append(" `isMarsGoRouteDone` = '").append(isMarsGoRouteDone ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_draw_vip_level_reward)) sBuilder.append(" `had_draw_vip_level_reward` = '").append(had_draw_vip_level_reward).append("',");
        if(isFieldMarked(FIELD_had_draw_vip_level_recharge_reward)) sBuilder.append(" `had_draw_vip_level_recharge_reward` = '").append(had_draw_vip_level_recharge_reward).append("',");
        if(isFieldMarked(FIELD_isStoreReviews)) sBuilder.append(" `isStoreReviews` = '").append(isStoreReviews ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_room_skin)) sBuilder.append(" `room_skin` = '").append(room_skin).append("',");
        if(isFieldMarked(FIELD_giftde_child_graduate_count)) sBuilder.append(" `giftde_child_graduate_count` = '").append(giftde_child_graduate_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`cname` varchar(64) NOT NULL DEFAULT '' COMMENT '玩家名称',"
                + "`icon` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家头像',"
                + "`iconBgk` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家头像框',"
                + "`title` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家当前使用的称号',"
                + "`bubble` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家当前使用的气泡框',"
                + "`createTime` int(11) NOT NULL DEFAULT '0' COMMENT '账号创建时间(秒) 同ArTime',"
                + "`create_date` int(11) NOT NULL DEFAULT '0' COMMENT '账号创建日期 同ArTime',"
                + "`createRoleTime` int(11) NOT NULL DEFAULT '0' COMMENT '创角时间(秒)',"
                + "`create_role_date` int(11) NOT NULL DEFAULT '0' COMMENT '创角日期',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`vipLvl` int(11) NOT NULL DEFAULT '0' COMMENT 'VIP等级',"
                + "`gmLevel` int(11) NOT NULL DEFAULT '0' COMMENT 'gm权限',"
                + "`player_lang` varchar(20) NOT NULL DEFAULT '' COMMENT '玩家语言',"
                + "`last_login_date` int(11) NOT NULL DEFAULT '0' COMMENT '账号最后日期',"
                + "`login_day_count` int(11) NOT NULL DEFAULT '0' COMMENT '登录次数',"
                + "`last_take_version` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后领取奖励的客户端版本',"
                + "`recharged_gem` bigint(20) NOT NULL DEFAULT '0' COMMENT '充值钻石数量',"
                + "`last_left_bag_time_s` int(11) NOT NULL DEFAULT '0' COMMENT '最后一次查看背包物品的时间',"
                + "`lastOfflineMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一次下线时间（毫秒）',"
                + "`prefab` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家预制形象配置ID',"
                + "`is_set_default` bigint(20) NOT NULL DEFAULT '0' COMMENT '是否创角 1-已设置',"
                + "`player_stage` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家段位',"
                + "`happy_value_cur_step_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前阶段数值',"
                + "`last_login_week_tag` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一次登录周标记',"
                + "`week_login_day_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '本周登录天数',"
                + "`latest_login_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '最近一次登陆时间（毫秒）',"
                + "`uid` varchar(32) NOT NULL DEFAULT '' COMMENT '平台账号',"
                + "`adfrom` varchar(128) NOT NULL DEFAULT '' COMMENT '渠道',"
                + "`adfrom2` varchar(128) NOT NULL DEFAULT '' COMMENT '二级渠道',"
                + "`adid` varchar(128) NOT NULL DEFAULT '' COMMENT '设备id',"
                + "`clientPackageName` varchar(64) NOT NULL DEFAULT '' COMMENT '客户端包名',"
                + "`clientVerion` varchar(32) NOT NULL DEFAULT '' COMMENT '客户端版本',"
                + "`nation` varchar(32) NOT NULL DEFAULT '' COMMENT '国家',"
                + "`ar_ip` varchar(32) NOT NULL DEFAULT '' COMMENT '注册IP',"
                + "`sdkid` varchar(128) NOT NULL DEFAULT '' COMMENT '渠道账号',"
                + "`recharged_money` bigint(20) NOT NULL DEFAULT '0' COMMENT '充值货币金额',"
                + "`power_max_record` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家实力历史记录最高值',"
                + "`earnings_max_record` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家赚速历史记录最高值',"
                + "`market_next_refresh_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '集市下次刷新时间',"
                + "`last_log_section_day` int(11) NOT NULL DEFAULT '0' COMMENT '上次截面日志记录日期',"
                + "`last_gain_visit_reward_day` int(11) NOT NULL DEFAULT '0' COMMENT '上次领取拜访其他玩家奖励日期',"
                + "`cute_actor_id` bigint(20) NOT NULL DEFAULT '0' COMMENT 'Q版形象ID',"
                + "`consort_call_next_refresh_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '家人指定邀约下次刷新时间（针对需要计数的数据）',"
                + "`birth_giftde_coum` int(11) NOT NULL DEFAULT '0' COMMENT '下次出生卷王次数',"
                + "`adult_record_bonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣记录收益总值（因为移除记录在玩家身上）',"
                + "`last_draw_daily_reward_date` int(11) NOT NULL DEFAULT '0' COMMENT '最后一次领取每日奖励的日期标记',"
                + "`extra_earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '额外赚速',"
                + "`building_earnings_max_record` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑赚速历史记录最高值',"
                + "`child_earnings_max_record` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣赚速历史记录最高值',"
                + "`seven_days_login_cal_offset` int(11) NOT NULL DEFAULT '0' COMMENT '七天登录计算偏移量',"
                + "`curTitle` blob NULL COMMENT '当前称号',"
                + "`curTitleShow` tinyint(1) NOT NULL DEFAULT '0' COMMENT '当前称号是否对外展示',"
                + "`curPlayerSkin` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前玩家皮肤',"
                + "`is_set_prefab` bigint(20) NOT NULL DEFAULT '0' COMMENT '是否设置预制形象 1-已设置',"
                + "`pending_order_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '待处理的订单数据ID',"
                + "`isMarsGoRouteDone` tinyint(1) NOT NULL DEFAULT '0' COMMENT '火星-前往火星全部阶段完成',"
                + "`had_draw_vip_level_reward` bigint(20) NOT NULL DEFAULT '0' COMMENT 'VIP等级奖励领取记录',"
                + "`had_draw_vip_level_recharge_reward` bigint(20) NOT NULL DEFAULT '0' COMMENT 'VIP等级充值奖励领取记录',"
                + "`isStoreReviews` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否商店评价',"
                + "`room_skin` bigint(20) NOT NULL DEFAULT '0' COMMENT '房间皮肤ID',"
                + "`giftde_child_graduate_count` int(11) NOT NULL DEFAULT '0' COMMENT '卷王子嗣毕业历史数量',"
                + "KEY `cid` (`cid`),"
                + "KEY `lastOfflineMs` (`lastOfflineMs`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家基本数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);//cname
        _size+=8;//icon
        _size+=8;//iconBgk
        _size+=8;//title
        _size+=8;//bubble
        _size+=4;//createTime
        _size+=4;//create_date
        _size+=4;//createRoleTime
        _size+=4;//create_role_date
        _size+=4;//lvl
        _size+=4;//vipLvl
        _size+=4;//gmLevel
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(player_lang);//player_lang
        _size+=4;//last_login_date
        _size+=4;//login_day_count
        _size+=8;//last_take_version
        _size+=8;//recharged_gem
        _size+=4;//last_left_bag_time_s
        _size+=8;//lastOfflineMs
        _size+=8;//prefab
        _size+=8;//is_set_default
        _size+=8;//player_stage
        _size+=8;//happy_value_cur_step_value
        _size+=8;//last_login_week_tag
        _size+=8;//week_login_day_count
        _size+=8;//latest_login_time_ms
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom);//adfrom
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom2);//adfrom2
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adid);//adid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientPackageName);//clientPackageName
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientVerion);//clientVerion
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(nation);//nation
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ar_ip);//ar_ip
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkid);//sdkid
        _size+=8;//recharged_money
        _size+=8;//power_max_record
        _size+=8;//earnings_max_record
        _size+=8;//market_next_refresh_ms
        _size+=4;//last_log_section_day
        _size+=4;//last_gain_visit_reward_day
        _size+=8;//cute_actor_id
        _size+=8;//consort_call_next_refresh_ms
        _size+=4;//birth_giftde_coum
        _size+=8;//adult_record_bonus
        _size+=4;//last_draw_daily_reward_date
        _size+=8;//extra_earnings
        _size+=8;//building_earnings_max_record
        _size+=8;//child_earnings_max_record
        _size+=4;//seven_days_login_cal_offset
        _size+=2;_size+=curTitle.length;//curTitle
        _size+=1;//curTitleShow
        _size+=8;//curPlayerSkin
        _size+=8;//is_set_prefab
        _size+=8;//pending_order_db_id
        _size+=1;//isMarsGoRouteDone
        _size+=8;//had_draw_vip_level_reward
        _size+=8;//had_draw_vip_level_recharge_reward
        _size+=1;//isStoreReviews
        _size+=8;//room_skin
        _size+=4;//giftde_child_graduate_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, cname);
        buff.putLong(icon);
        buff.putLong(iconBgk);
        buff.putLong(title);
        buff.putLong(bubble);
        buff.putInt(createTime);
        buff.putInt(create_date);
        buff.putInt(createRoleTime);
        buff.putInt(create_role_date);
        buff.putInt(lvl);
        buff.putInt(vipLvl);
        buff.putInt(gmLevel);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, player_lang);
        buff.putInt(last_login_date);
        buff.putInt(login_day_count);
        buff.putLong(last_take_version);
        buff.putLong(recharged_gem);
        buff.putInt(last_left_bag_time_s);
        buff.putLong(lastOfflineMs);
        buff.putLong(prefab);
        buff.putLong(is_set_default);
        buff.putLong(player_stage);
        buff.putLong(happy_value_cur_step_value);
        buff.putLong(last_login_week_tag);
        buff.putLong(week_login_day_count);
        buff.putLong(latest_login_time_ms);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom2);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, clientPackageName);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, clientVerion);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, nation);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ar_ip);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sdkid);
        buff.putLong(recharged_money);
        buff.putLong(power_max_record);
        buff.putLong(earnings_max_record);
        buff.putLong(market_next_refresh_ms);
        buff.putInt(last_log_section_day);
        buff.putInt(last_gain_visit_reward_day);
        buff.putLong(cute_actor_id);
        buff.putLong(consort_call_next_refresh_ms);
        buff.putInt(birth_giftde_coum);
        buff.putLong(adult_record_bonus);
        buff.putInt(last_draw_daily_reward_date);
        buff.putLong(extra_earnings);
        buff.putLong(building_earnings_max_record);
        buff.putLong(child_earnings_max_record);
        buff.putInt(seven_days_login_cal_offset);
        buff.putShort((short)(curTitle == null ? 0 : curTitle.length));if(null != curTitle){buff.put(curTitle);}
        buff.put((byte)(curTitleShow?1:0));
        buff.putLong(curPlayerSkin);
        buff.putLong(is_set_prefab);
        buff.putLong(pending_order_db_id);
        buff.put((byte)(isMarsGoRouteDone?1:0));
        buff.putLong(had_draw_vip_level_reward);
        buff.putLong(had_draw_vip_level_recharge_reward);
        buff.put((byte)(isStoreReviews?1:0));
        buff.putLong(room_skin);
        buff.putInt(giftde_child_graduate_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        cname=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        icon=buff.getLong();
        iconBgk=buff.getLong();
        title=buff.getLong();
        bubble=buff.getLong();
        createTime=buff.getInt();
        create_date=buff.getInt();
        createRoleTime=buff.getInt();
        create_role_date=buff.getInt();
        lvl=buff.getInt();
        vipLvl=buff.getInt();
        gmLevel=buff.getInt();
        player_lang=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        last_login_date=buff.getInt();
        login_day_count=buff.getInt();
        last_take_version=buff.getLong();
        recharged_gem=buff.getLong();
        last_left_bag_time_s=buff.getInt();
        lastOfflineMs=buff.getLong();
        prefab=buff.getLong();
        is_set_default=buff.getLong();
        player_stage=buff.getLong();
        happy_value_cur_step_value=buff.getLong();
        last_login_week_tag=buff.getLong();
        week_login_day_count=buff.getLong();
        latest_login_time_ms=buff.getLong();
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom2=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        clientPackageName=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        clientVerion=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        nation=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        ar_ip=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdkid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        recharged_money=buff.getLong();
        power_max_record=buff.getLong();
        earnings_max_record=buff.getLong();
        market_next_refresh_ms=buff.getLong();
        last_log_section_day=buff.getInt();
        last_gain_visit_reward_day=buff.getInt();
        cute_actor_id=buff.getLong();
        consort_call_next_refresh_ms=buff.getLong();
        birth_giftde_coum=buff.getInt();
        adult_record_bonus=buff.getLong();
        last_draw_daily_reward_date=buff.getInt();
        extra_earnings=buff.getLong();
        building_earnings_max_record=buff.getLong();
        child_earnings_max_record=buff.getLong();
        seven_days_login_cal_offset=buff.getInt();
        int curTitle_count = buff.getShort();if(curTitle_count>0){curTitle = new byte[curTitle_count];buff.get(curTitle);}
        curTitleShow=(buff.get()==1);
        curPlayerSkin=buff.getLong();
        is_set_prefab=buff.getLong();
        pending_order_db_id=buff.getLong();
        isMarsGoRouteDone=(buff.get()==1);
        had_draw_vip_level_reward=buff.getLong();
        had_draw_vip_level_recharge_reward=buff.getLong();
        isStoreReviews=(buff.get()==1);
        room_skin=buff.getLong();
        giftde_child_graduate_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
