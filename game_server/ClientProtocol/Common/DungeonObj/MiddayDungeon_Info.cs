using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 午间副本信息
/// </summary>
public class MiddayDungeon_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 本轮开始时间戳
/// </summary>
private long roundStartTimeMS;
/// <summary>
/// Boss信息
/// </summary>
private Common.DungeonObj.MiddayDungeon_BossInfo bossInfo;
/// <summary>
/// 已战斗过的英雄列表
/// </summary>
private List<Common.DungeonObj.MiddayDungeon_FightHero> hadFightHeroList;
/// <summary>
/// 借用过大臣的玩家列表
/// </summary>
private List<long> borrowCidList;


public MiddayDungeon_Info() {
	roundStartTimeMS = (long)0;
	bossInfo = new Common.DungeonObj.MiddayDungeon_BossInfo();
	hadFightHeroList = new List<Common.DungeonObj.MiddayDungeon_FightHero>();
	borrowCidList = new List<long>();
}

public MiddayDungeon_Info(
	long _roundStartTimeMS
	, Common.DungeonObj.MiddayDungeon_BossInfo _bossInfo
	, List<Common.DungeonObj.MiddayDungeon_FightHero> _hadFightHeroList
	, List<long> _borrowCidList
) {	roundStartTimeMS = _roundStartTimeMS;
	bossInfo = _bossInfo;
	hadFightHeroList = _hadFightHeroList;
	borrowCidList = _borrowCidList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 本轮开始时间戳
/// </summary>
public long getRoundStartTimeMS() { return roundStartTimeMS; }
/// <summary>
/// 本轮开始时间戳
/// </summary>
public void setRoundStartTimeMS(long _roundStartTimeMS) { roundStartTimeMS = _roundStartTimeMS; }
/// <summary>
/// Boss信息
/// </summary>
public Common.DungeonObj.MiddayDungeon_BossInfo getBossInfo() { return bossInfo; }
/// <summary>
/// Boss信息
/// </summary>
public void setBossInfo(Common.DungeonObj.MiddayDungeon_BossInfo _bossInfo) { bossInfo = _bossInfo; }
/// <summary>
/// 已战斗过的英雄列表
/// </summary>
public List<Common.DungeonObj.MiddayDungeon_FightHero> getHadFightHeroList() { return hadFightHeroList; }
/// <summary>
/// 已战斗过的英雄列表
/// </summary>
public void addHadFightHeroList(Common.DungeonObj.MiddayDungeon_FightHero _hadFightHeroList) { hadFightHeroList.Add(_hadFightHeroList); }
/// <summary>
/// 借用过大臣的玩家列表
/// </summary>
public List<long> getBorrowCidList() { return borrowCidList; }
/// <summary>
/// 借用过大臣的玩家列表
/// </summary>
public void addBorrowCidList(long _borrowCidList) { borrowCidList.Add(_borrowCidList); }


public int GetBufSize() {
	int _size = 24;
	_size += 2 + (hadFightHeroList.Count * 14);
	_size += 2 + (borrowCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (hadFightHeroList.Count * 14);
	_size += 2 + (borrowCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roundStartTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _bossInfoCustLen = _buf.getInt();
	int _bossInfoCurPos = _buf.getCurPos();
	bossInfo.ReadUnzipBuf(_buf, _bossInfoCurPos + _bossInfoCustLen);
	_buf.setPosition(_bossInfoCurPos + _bossInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadFightHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadFightHeroListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_FightHero _hadFightHeroList = new Common.DungeonObj.MiddayDungeon_FightHero();
		int __hadFightHeroListCustLen = _buf.getInt();
	int __hadFightHeroListCurPos = _buf.getCurPos();
	_hadFightHeroList.ReadUnzipBuf(_buf, __hadFightHeroListCurPos + __hadFightHeroListCustLen);
	_buf.setPosition(__hadFightHeroListCurPos + __hadFightHeroListCustLen);

		hadFightHeroList.Add(_hadFightHeroList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _borrowCidListCount = _buf.getShort();
	for(int _i = 0; _i < _borrowCidListCount; _i++) { 
		long _borrowCidList = (long)0;
		_borrowCidList = _buf.getLong();
		borrowCidList.Add(_borrowCidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(roundStartTimeMS);
	_buf.putInt(bossInfo.GetBufSize());
	bossInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)hadFightHeroList.Count);
	for(int _i = 0; _i < hadFightHeroList.Count; _i++) { 
		_buf.putInt(hadFightHeroList[_i].GetBufSize());
	hadFightHeroList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)borrowCidList.Count);
	for(int _i = 0; _i < borrowCidList.Count; _i++) { 
		_buf.putLong(borrowCidList[_i]);
	}
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
	builder.Append("roundStartTimeMS").Append(":").Append(roundStartTimeMS.ToString()).Append(", ");
	builder.Append("bossInfo").Append(":").Append(bossInfo == null ? "null" : bossInfo.ToString()).Append(", ");
	builder.Append("hadFightHeroList").Append(":").Append(hadFightHeroList.ToString()).Append(", ");
	builder.Append("borrowCidList").Append(":").Append(borrowCidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

