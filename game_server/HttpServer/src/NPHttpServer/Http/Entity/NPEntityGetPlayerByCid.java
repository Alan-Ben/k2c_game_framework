package NPHttpServer.Http.Entity;

import Common.ServerObj.ServerObj_PHPPlayer;
import com.google.gson.JsonObject;

/**
 * 根据CID查询角色明细信息
 * 
 * 传递参数
us_type_id	服务器号	是	int	1
cid	角色CID	否：CID&Name 二选1	string	精确查找
name	用户名	否：CID&Name 二选1	string	精确查找

 * 返回数据
参数	说明	是否必填	类型	备注
uid	账号UID	是	string	
cid	角色CID	是	string	
name	用户名	是	string	
lvl	角色等级	是	int	
vip_lvl	VIP等级	是	int	
ar_time	注册时间	是	int	时间戳
create_role_time	创角时间	是	int	时间戳
last_login_time	最后登录时间	是	int	时间戳
recharged_money	累计充值金额	是	double	100
game_coin	剩余游戏币	是	double	100
game_diamond	剩余钻石（充值转换币种）	是	double	100
union_id	所属联盟ID	否		
union_position	所属联盟职位	否		
其他	其他运营要求返回的字段	是		根据运营需求进行返回

 */
public class NPEntityGetPlayerByCid
{
    private int _m_iUsTypeId;
    private long _m_lCid;
    private String _m_sName;
    
    public NPEntityGetPlayerByCid()
    {
    	_m_sName = "";
    }

    public int getUsId() {return _m_iUsTypeId;}
    public long getCid() {return _m_lCid;}
    public String getName() {return _m_sName;}
    
    public void setUsId(int _usId) {_m_iUsTypeId = _usId;}
    public void setCid(long _cid) {_m_lCid = _cid;}
    public void setName(String _name) {_m_sName = _name;}
    
    /**
     * 输出后台需要的玩家数据结构
     * @param _playerObj
     * @return
     */
    public static JsonObject toJson(ServerObj_PHPPlayer _playerObj)
    {
    	JsonObject jsonObj = new JsonObject();
    	
    	jsonObj.addProperty("uid", _playerObj.getUid());
    	jsonObj.addProperty("cid", _playerObj.getCid());
    	jsonObj.addProperty("name", _playerObj.getName());
    	jsonObj.addProperty("lvl", _playerObj.getLvl());
    	jsonObj.addProperty("vip_lvl", _playerObj.getVip_lvl());
    	jsonObj.addProperty("ar_time", _playerObj.getAr_time());
    	jsonObj.addProperty("create_role_time", _playerObj.getCreate_role_time());
    	jsonObj.addProperty("last_login_time", _playerObj.getLast_login_time());
    	jsonObj.addProperty("recharged_money", _playerObj.getRecharged_money());
    	jsonObj.addProperty("game_coin", _playerObj.getGame_coin());
    	jsonObj.addProperty("union_id", _playerObj.getUnion_id());
    	jsonObj.addProperty("union_position", _playerObj.getUnion_position());
    	
    	return jsonObj;
    }
}
