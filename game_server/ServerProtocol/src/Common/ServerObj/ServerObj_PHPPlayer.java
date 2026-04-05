package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 平台获取的玩家明细信息
 **/
public class ServerObj_PHPPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 账号UID */
private String uid;
/** 角色CID */
private long cid;
/** 用户名 */
private String name;
/** 角色等级 */
private int lvl;
/** VIP等级 */
private int vip_lvl;
/** 注册时间-时间戳 */
private int ar_time;
/** 创角时间-时间戳 */
private int create_role_time;
/** 最后登录时间-时间戳 */
private int last_login_time;
/** 累计充值金额 */
private double recharged_money;
/** 剩余游戏币 */
private double game_coin;
/** 剩余钻石（充值转换币种） */
private double game_diamond;
/** 所属联盟ID */
private long union_id;
/** 所属联盟职位 */
private int union_position;


public ServerObj_PHPPlayer() {
	uid = "";
	cid = (long)0;
	name = "";
	lvl = 0;
	vip_lvl = 0;
	ar_time = 0;
	create_role_time = 0;
	last_login_time = 0;
	recharged_money = 0d;
	game_coin = 0d;
	game_diamond = 0d;
	union_id = (long)0;
	union_position = 0;
}

public ServerObj_PHPPlayer(
	 String _uid
	, long _cid
	, String _name
	, int _lvl
	, int _vip_lvl
	, int _ar_time
	, int _create_role_time
	, int _last_login_time
	, double _recharged_money
	, double _game_coin
	, double _game_diamond
	, long _union_id
	, int _union_position
) {	uid = _uid;
	cid = _cid;
	name = _name;
	lvl = _lvl;
	vip_lvl = _vip_lvl;
	ar_time = _ar_time;
	create_role_time = _create_role_time;
	last_login_time = _last_login_time;
	recharged_money = _recharged_money;
	game_coin = _game_coin;
	game_diamond = _game_diamond;
	union_id = _union_id;
	union_position = _union_position;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 账号UID */
public String getUid() { return uid; }
/** 账号UID */
public void setUid(String _uid) { uid = _uid; }
/** 角色CID */
public long getCid() { return cid; }
/** 角色CID */
public void setCid(long _cid) { cid = _cid; }
/** 用户名 */
public String getName() { return name; }
/** 用户名 */
public void setName(String _name) { name = _name; }
/** 角色等级 */
public int getLvl() { return lvl; }
/** 角色等级 */
public void setLvl(int _lvl) { lvl = _lvl; }
/** VIP等级 */
public int getVip_lvl() { return vip_lvl; }
/** VIP等级 */
public void setVip_lvl(int _vip_lvl) { vip_lvl = _vip_lvl; }
/** 注册时间-时间戳 */
public int getAr_time() { return ar_time; }
/** 注册时间-时间戳 */
public void setAr_time(int _ar_time) { ar_time = _ar_time; }
/** 创角时间-时间戳 */
public int getCreate_role_time() { return create_role_time; }
/** 创角时间-时间戳 */
public void setCreate_role_time(int _create_role_time) { create_role_time = _create_role_time; }
/** 最后登录时间-时间戳 */
public int getLast_login_time() { return last_login_time; }
/** 最后登录时间-时间戳 */
public void setLast_login_time(int _last_login_time) { last_login_time = _last_login_time; }
/** 累计充值金额 */
public double getRecharged_money() { return recharged_money; }
/** 累计充值金额 */
public void setRecharged_money(double _recharged_money) { recharged_money = _recharged_money; }
/** 剩余游戏币 */
public double getGame_coin() { return game_coin; }
/** 剩余游戏币 */
public void setGame_coin(double _game_coin) { game_coin = _game_coin; }
/** 剩余钻石（充值转换币种） */
public double getGame_diamond() { return game_diamond; }
/** 剩余钻石（充值转换币种） */
public void setGame_diamond(double _game_diamond) { game_diamond = _game_diamond; }
/** 所属联盟ID */
public long getUnion_id() { return union_id; }
/** 所属联盟ID */
public void setUnion_id(long _union_id) { union_id = _union_id; }
/** 所属联盟职位 */
public int getUnion_position() { return union_position; }
/** 所属联盟职位 */
public void setUnion_position(int _union_position) { union_position = _union_position; }


public final int GetBufSize() {
	int _size = 64;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 66;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vip_lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ar_time = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) create_role_time = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) last_login_time = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) recharged_money = _buf.getDouble();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) game_coin = _buf.getDouble();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) game_diamond = _buf.getDouble();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) union_id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) union_position = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putInt(lvl);
	_buf.putInt(vip_lvl);
	_buf.putInt(ar_time);
	_buf.putInt(create_role_time);
	_buf.putInt(last_login_time);
	_buf.putDouble(recharged_money);
	_buf.putDouble(game_coin);
	_buf.putDouble(game_diamond);
	_buf.putLong(union_id);
	_buf.putInt(union_position);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

