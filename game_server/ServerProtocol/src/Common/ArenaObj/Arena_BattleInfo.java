package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场战斗数据
 **/
public class Arena_BattleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 对手CID */
private long opponentCid;
/** 已击败对手数量 */
private int hadDefeatNum;
/** 对手大臣数量 */
private int opponentHeroNum;
/** 对手战力 */
private long opponentPower;
/** 本回合可攻击英雄列表 */
private java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo> canAttackHeroList;
/** 本轮是否已购买buff */
private boolean hadBuyBuff;
/** 临时增益列表 */
private java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo> buffList;
/** 我方大臣id */
private long heroId;
/** 我方基础实力 */
private long basePower;
/** 我方被扣除血量 */
private long deductedHp;
/** 是否npc */
private boolean isNpc;
/** npc名字 */
private String npcName;


public Arena_BattleInfo() {
	opponentCid = (long)0;
	hadDefeatNum = 0;
	opponentHeroNum = 0;
	opponentPower = (long)0;
	canAttackHeroList = new java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo>();
	hadBuyBuff = false;
	buffList = new java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo>();
	heroId = (long)0;
	basePower = (long)0;
	deductedHp = (long)0;
	isNpc = false;
	npcName = "";
}

public Arena_BattleInfo(
	 long _opponentCid
	, int _hadDefeatNum
	, int _opponentHeroNum
	, long _opponentPower
	, java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo> _canAttackHeroList
	, boolean _hadBuyBuff
	, java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo> _buffList
	, long _heroId
	, long _basePower
	, long _deductedHp
	, boolean _isNpc
	, String _npcName
) {	opponentCid = _opponentCid;
	hadDefeatNum = _hadDefeatNum;
	opponentHeroNum = _opponentHeroNum;
	opponentPower = _opponentPower;
	canAttackHeroList = _canAttackHeroList;
	hadBuyBuff = _hadBuyBuff;
	buffList = _buffList;
	heroId = _heroId;
	basePower = _basePower;
	deductedHp = _deductedHp;
	isNpc = _isNpc;
	npcName = _npcName;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 对手CID */
public long getOpponentCid() { return opponentCid; }
/** 对手CID */
public void setOpponentCid(long _opponentCid) { opponentCid = _opponentCid; }
/** 已击败对手数量 */
public int getHadDefeatNum() { return hadDefeatNum; }
/** 已击败对手数量 */
public void setHadDefeatNum(int _hadDefeatNum) { hadDefeatNum = _hadDefeatNum; }
/** 对手大臣数量 */
public int getOpponentHeroNum() { return opponentHeroNum; }
/** 对手大臣数量 */
public void setOpponentHeroNum(int _opponentHeroNum) { opponentHeroNum = _opponentHeroNum; }
/** 对手战力 */
public long getOpponentPower() { return opponentPower; }
/** 对手战力 */
public void setOpponentPower(long _opponentPower) { opponentPower = _opponentPower; }
/** 本回合可攻击英雄列表 */
public java.util.ArrayList<Common.HeroObj.Hero_ArenaShowInfo> getCanAttackHeroList() { return canAttackHeroList; }
/** 本回合可攻击英雄列表 */
public void addCanAttackHeroList(Common.HeroObj.Hero_ArenaShowInfo _canAttackHeroList) { canAttackHeroList.add(_canAttackHeroList); }
/** 本轮是否已购买buff */
public boolean getHadBuyBuff() { return hadBuyBuff; }
/** 本轮是否已购买buff */
public void setHadBuyBuff(boolean _hadBuyBuff) { hadBuyBuff = _hadBuyBuff; }
/** 临时增益列表 */
public java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo> getBuffList() { return buffList; }
/** 临时增益列表 */
public void addBuffList(Common.ArenaObj.Arena_SingleBuffInfo _buffList) { buffList.add(_buffList); }
/** 我方大臣id */
public long getHeroId() { return heroId; }
/** 我方大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 我方基础实力 */
public long getBasePower() { return basePower; }
/** 我方基础实力 */
public void setBasePower(long _basePower) { basePower = _basePower; }
/** 我方被扣除血量 */
public long getDeductedHp() { return deductedHp; }
/** 我方被扣除血量 */
public void setDeductedHp(long _deductedHp) { deductedHp = _deductedHp; }
/** 是否npc */
public boolean getIsNpc() { return isNpc; }
/** 是否npc */
public void setIsNpc(boolean _isNpc) { isNpc = _isNpc; }
/** npc名字 */
public String getNpcName() { return npcName; }
/** npc名字 */
public void setNpcName(String _npcName) { npcName = _npcName; }


public final int GetBufSize() {
	int _size = 50;
	_size += 2 + (canAttackHeroList.size() * 32);
	_size += 2 + (buffList.size() * 16);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(npcName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 52;
	_size += 2 + (canAttackHeroList.size() * 32);
	_size += 2 + (buffList.size() * 16);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(npcName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDefeatNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _canAttackHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _canAttackHeroListCount; _i++) { 
		Common.HeroObj.Hero_ArenaShowInfo _canAttackHeroList = new Common.HeroObj.Hero_ArenaShowInfo();
		if(_buf.remaining() <= 0) return;
	int __canAttackHeroListCustLen = _buf.getInt();
	int __canAttackHeroListCurPos = _buf.position();
	_canAttackHeroList.ReadUnzipBuf(_buf, __canAttackHeroListCurPos + __canAttackHeroListCustLen);
	_buf.position(__canAttackHeroListCurPos + __canAttackHeroListCustLen);

		canAttackHeroList.add(_canAttackHeroList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBuyBuff = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buffListCount = _buf.getShort();
	for(int _i = 0; _i < _buffListCount; _i++) { 
		Common.ArenaObj.Arena_SingleBuffInfo _buffList = new Common.ArenaObj.Arena_SingleBuffInfo();
		if(_buf.remaining() <= 0) return;
	int __buffListCustLen = _buf.getInt();
	int __buffListCurPos = _buf.position();
	_buffList.ReadUnzipBuf(_buf, __buffListCurPos + __buffListCustLen);
	_buf.position(__buffListCurPos + __buffListCustLen);

		buffList.add(_buffList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) basePower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) deductedHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNpc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) npcName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(opponentCid);
	_buf.putInt(hadDefeatNum);
	_buf.putInt(opponentHeroNum);
	_buf.putLong(opponentPower);
	_buf.putShort((short)canAttackHeroList.size());
	for(int _i = 0; _i < canAttackHeroList.size(); _i++) { 
		_buf.putInt(canAttackHeroList.get(_i).GetBufSize());
	canAttackHeroList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(hadBuyBuff?(byte)1:(byte)0);
	_buf.putShort((short)buffList.size());
	for(int _i = 0; _i < buffList.size(); _i++) { 
		_buf.putInt(buffList.get(_i).GetBufSize());
	buffList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(heroId);
	_buf.putLong(basePower);
	_buf.putLong(deductedHp);
	_buf.put(isNpc?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, npcName);
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

