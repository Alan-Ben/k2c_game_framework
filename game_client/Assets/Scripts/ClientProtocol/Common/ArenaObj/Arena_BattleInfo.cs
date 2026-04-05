using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场战斗数据
/// </summary>
public class Arena_BattleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对手CID
/// </summary>
private long opponentCid;
/// <summary>
/// 已击败对手数量
/// </summary>
private int hadDefeatNum;
/// <summary>
/// 对手大臣数量
/// </summary>
private int opponentHeroNum;
/// <summary>
/// 对手战力
/// </summary>
private long opponentPower;
/// <summary>
/// 本回合可攻击英雄列表
/// </summary>
private List<Common.HeroObj.Hero_ArenaShowInfo> canAttackHeroList;
/// <summary>
/// 本轮是否已购买buff
/// </summary>
private bool hadBuyBuff;
/// <summary>
/// 临时增益列表
/// </summary>
private List<Common.ArenaObj.Arena_SingleBuffInfo> buffList;
/// <summary>
/// 我方大臣id
/// </summary>
private long heroId;
/// <summary>
/// 我方基础实力
/// </summary>
private long basePower;
/// <summary>
/// 我方被扣除血量
/// </summary>
private long deductedHp;
/// <summary>
/// 是否npc
/// </summary>
private bool isNpc;
/// <summary>
/// npc名字
/// </summary>
private string npcName;


public Arena_BattleInfo() {
	opponentCid = (long)0;
	hadDefeatNum = 0;
	opponentHeroNum = 0;
	opponentPower = (long)0;
	canAttackHeroList = new List<Common.HeroObj.Hero_ArenaShowInfo>();
	hadBuyBuff = false;
	buffList = new List<Common.ArenaObj.Arena_SingleBuffInfo>();
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
	, List<Common.HeroObj.Hero_ArenaShowInfo> _canAttackHeroList
	, bool _hadBuyBuff
	, List<Common.ArenaObj.Arena_SingleBuffInfo> _buffList
	, long _heroId
	, long _basePower
	, long _deductedHp
	, bool _isNpc
	, string _npcName
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 对手CID
/// </summary>
public long getOpponentCid() { return opponentCid; }
/// <summary>
/// 对手CID
/// </summary>
public void setOpponentCid(long _opponentCid) { opponentCid = _opponentCid; }
/// <summary>
/// 已击败对手数量
/// </summary>
public int getHadDefeatNum() { return hadDefeatNum; }
/// <summary>
/// 已击败对手数量
/// </summary>
public void setHadDefeatNum(int _hadDefeatNum) { hadDefeatNum = _hadDefeatNum; }
/// <summary>
/// 对手大臣数量
/// </summary>
public int getOpponentHeroNum() { return opponentHeroNum; }
/// <summary>
/// 对手大臣数量
/// </summary>
public void setOpponentHeroNum(int _opponentHeroNum) { opponentHeroNum = _opponentHeroNum; }
/// <summary>
/// 对手战力
/// </summary>
public long getOpponentPower() { return opponentPower; }
/// <summary>
/// 对手战力
/// </summary>
public void setOpponentPower(long _opponentPower) { opponentPower = _opponentPower; }
/// <summary>
/// 本回合可攻击英雄列表
/// </summary>
public List<Common.HeroObj.Hero_ArenaShowInfo> getCanAttackHeroList() { return canAttackHeroList; }
/// <summary>
/// 本回合可攻击英雄列表
/// </summary>
public void addCanAttackHeroList(Common.HeroObj.Hero_ArenaShowInfo _canAttackHeroList) { canAttackHeroList.Add(_canAttackHeroList); }
/// <summary>
/// 本轮是否已购买buff
/// </summary>
public bool getHadBuyBuff() { return hadBuyBuff; }
/// <summary>
/// 本轮是否已购买buff
/// </summary>
public void setHadBuyBuff(bool _hadBuyBuff) { hadBuyBuff = _hadBuyBuff; }
/// <summary>
/// 临时增益列表
/// </summary>
public List<Common.ArenaObj.Arena_SingleBuffInfo> getBuffList() { return buffList; }
/// <summary>
/// 临时增益列表
/// </summary>
public void addBuffList(Common.ArenaObj.Arena_SingleBuffInfo _buffList) { buffList.Add(_buffList); }
/// <summary>
/// 我方大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 我方大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 我方基础实力
/// </summary>
public long getBasePower() { return basePower; }
/// <summary>
/// 我方基础实力
/// </summary>
public void setBasePower(long _basePower) { basePower = _basePower; }
/// <summary>
/// 我方被扣除血量
/// </summary>
public long getDeductedHp() { return deductedHp; }
/// <summary>
/// 我方被扣除血量
/// </summary>
public void setDeductedHp(long _deductedHp) { deductedHp = _deductedHp; }
/// <summary>
/// 是否npc
/// </summary>
public bool getIsNpc() { return isNpc; }
/// <summary>
/// 是否npc
/// </summary>
public void setIsNpc(bool _isNpc) { isNpc = _isNpc; }
/// <summary>
/// npc名字
/// </summary>
public string getNpcName() { return npcName; }
/// <summary>
/// npc名字
/// </summary>
public void setNpcName(string _npcName) { npcName = _npcName; }


public int GetBufSize() {
	int _size = 50;
	_size += 2 + (canAttackHeroList.Count * 32);
	_size += 2 + (buffList.Count * 16);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(npcName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 52;
	_size += 2 + (canAttackHeroList.Count * 32);
	_size += 2 + (buffList.Count * 16);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(npcName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDefeatNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _canAttackHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _canAttackHeroListCount; _i++) { 
		Common.HeroObj.Hero_ArenaShowInfo _canAttackHeroList = new Common.HeroObj.Hero_ArenaShowInfo();
		int __canAttackHeroListCustLen = _buf.getInt();
	int __canAttackHeroListCurPos = _buf.getCurPos();
	_canAttackHeroList.ReadUnzipBuf(_buf, __canAttackHeroListCurPos + __canAttackHeroListCustLen);
	_buf.setPosition(__canAttackHeroListCurPos + __canAttackHeroListCustLen);

		canAttackHeroList.Add(_canAttackHeroList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadBuyBuff = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buffListCount = _buf.getShort();
	for(int _i = 0; _i < _buffListCount; _i++) { 
		Common.ArenaObj.Arena_SingleBuffInfo _buffList = new Common.ArenaObj.Arena_SingleBuffInfo();
		int __buffListCustLen = _buf.getInt();
	int __buffListCurPos = _buf.getCurPos();
	_buffList.ReadUnzipBuf(_buf, __buffListCurPos + __buffListCustLen);
	_buf.setPosition(__buffListCurPos + __buffListCustLen);

		buffList.Add(_buffList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	basePower = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	deductedHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNpc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcName = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(opponentCid);
	_buf.putInt(hadDefeatNum);
	_buf.putInt(opponentHeroNum);
	_buf.putLong(opponentPower);
	_buf.putShort((short)canAttackHeroList.Count);
	for(int _i = 0; _i < canAttackHeroList.Count; _i++) { 
		_buf.putInt(canAttackHeroList[_i].GetBufSize());
	canAttackHeroList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(hadBuyBuff?(byte)1:(byte)0);
	_buf.putShort((short)buffList.Count);
	for(int _i = 0; _i < buffList.Count; _i++) { 
		_buf.putInt(buffList[_i].GetBufSize());
	buffList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(heroId);
	_buf.putLong(basePower);
	_buf.putLong(deductedHp);
	_buf.put(isNpc?(byte)1:(byte)0);
	_buf.putString(npcName);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("opponentCid").Append(":").Append(opponentCid.ToString()).Append(", ");
	builder.Append("hadDefeatNum").Append(":").Append(hadDefeatNum.ToString()).Append(", ");
	builder.Append("opponentHeroNum").Append(":").Append(opponentHeroNum.ToString()).Append(", ");
	builder.Append("opponentPower").Append(":").Append(opponentPower.ToString()).Append(", ");
	builder.Append("canAttackHeroList").Append(":").Append(canAttackHeroList.ToString()).Append(", ");
	builder.Append("hadBuyBuff").Append(":").Append(hadBuyBuff.ToString()).Append(", ");
	builder.Append("buffList").Append(":").Append(buffList.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("basePower").Append(":").Append(basePower.ToString()).Append(", ");
	builder.Append("deductedHp").Append(":").Append(deductedHp.ToString()).Append(", ");
	builder.Append("isNpc").Append(":").Append(isNpc.ToString()).Append(", ");
	builder.Append("npcName").Append(":").Append(npcName.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

