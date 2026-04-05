using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 晚间副本信息
/// </summary>
public class EveningDungeon_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 本轮开始时间戳
/// </summary>
private long roundStartTimeMS;
/// <summary>
/// 已战斗过的英雄列表
/// </summary>
private List<long> hadFightHeroList;


public EveningDungeon_Info() {
	roundStartTimeMS = (long)0;
	hadFightHeroList = new List<long>();
}

public EveningDungeon_Info(
	long _roundStartTimeMS
	, List<long> _hadFightHeroList
) {	roundStartTimeMS = _roundStartTimeMS;
	hadFightHeroList = _hadFightHeroList;
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
/// 已战斗过的英雄列表
/// </summary>
public List<long> getHadFightHeroList() { return hadFightHeroList; }
/// <summary>
/// 已战斗过的英雄列表
/// </summary>
public void addHadFightHeroList(long _hadFightHeroList) { hadFightHeroList.Add(_hadFightHeroList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (hadFightHeroList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (hadFightHeroList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roundStartTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadFightHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadFightHeroListCount; _i++) { 
		long _hadFightHeroList = (long)0;
		_hadFightHeroList = _buf.getLong();
		hadFightHeroList.Add(_hadFightHeroList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(roundStartTimeMS);
	_buf.putShort((short)hadFightHeroList.Count);
	for(int _i = 0; _i < hadFightHeroList.Count; _i++) { 
		_buf.putLong(hadFightHeroList[_i]);
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
	builder.Append("hadFightHeroList").Append(":").Append(hadFightHeroList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

