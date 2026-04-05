using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场基础数据
/// </summary>
public class Arena_BaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次重置时间戳 毫秒
/// </summary>
private long lastResetTimeMs;
/// <summary>
/// 已指定攻击次数
/// </summary>
private int hadSelectAttackNum;
/// <summary>
/// 已随机攻击次数
/// </summary>
private int hadRandomAttackNum;
/// <summary>
/// 已购买随机攻击次数
/// </summary>
private int hadBuyRandomAttackNum;
/// <summary>
/// 已指定攻击大臣id列表
/// </summary>
private List<long> hadSelectAttackHeroList;
/// <summary>
/// 已随机攻击大臣id列表
/// </summary>
private List<long> hadRandomAttackHeroList;


public Arena_BaseInfo() {
	lastResetTimeMs = (long)0;
	hadSelectAttackNum = 0;
	hadRandomAttackNum = 0;
	hadBuyRandomAttackNum = 0;
	hadSelectAttackHeroList = new List<long>();
	hadRandomAttackHeroList = new List<long>();
}

public Arena_BaseInfo(
	long _lastResetTimeMs
	, int _hadSelectAttackNum
	, int _hadRandomAttackNum
	, int _hadBuyRandomAttackNum
	, List<long> _hadSelectAttackHeroList
	, List<long> _hadRandomAttackHeroList
) {	lastResetTimeMs = _lastResetTimeMs;
	hadSelectAttackNum = _hadSelectAttackNum;
	hadRandomAttackNum = _hadRandomAttackNum;
	hadBuyRandomAttackNum = _hadBuyRandomAttackNum;
	hadSelectAttackHeroList = _hadSelectAttackHeroList;
	hadRandomAttackHeroList = _hadRandomAttackHeroList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上次重置时间戳 毫秒
/// </summary>
public long getLastResetTimeMs() { return lastResetTimeMs; }
/// <summary>
/// 上次重置时间戳 毫秒
/// </summary>
public void setLastResetTimeMs(long _lastResetTimeMs) { lastResetTimeMs = _lastResetTimeMs; }
/// <summary>
/// 已指定攻击次数
/// </summary>
public int getHadSelectAttackNum() { return hadSelectAttackNum; }
/// <summary>
/// 已指定攻击次数
/// </summary>
public void setHadSelectAttackNum(int _hadSelectAttackNum) { hadSelectAttackNum = _hadSelectAttackNum; }
/// <summary>
/// 已随机攻击次数
/// </summary>
public int getHadRandomAttackNum() { return hadRandomAttackNum; }
/// <summary>
/// 已随机攻击次数
/// </summary>
public void setHadRandomAttackNum(int _hadRandomAttackNum) { hadRandomAttackNum = _hadRandomAttackNum; }
/// <summary>
/// 已购买随机攻击次数
/// </summary>
public int getHadBuyRandomAttackNum() { return hadBuyRandomAttackNum; }
/// <summary>
/// 已购买随机攻击次数
/// </summary>
public void setHadBuyRandomAttackNum(int _hadBuyRandomAttackNum) { hadBuyRandomAttackNum = _hadBuyRandomAttackNum; }
/// <summary>
/// 已指定攻击大臣id列表
/// </summary>
public List<long> getHadSelectAttackHeroList() { return hadSelectAttackHeroList; }
/// <summary>
/// 已指定攻击大臣id列表
/// </summary>
public void addHadSelectAttackHeroList(long _hadSelectAttackHeroList) { hadSelectAttackHeroList.Add(_hadSelectAttackHeroList); }
/// <summary>
/// 已随机攻击大臣id列表
/// </summary>
public List<long> getHadRandomAttackHeroList() { return hadRandomAttackHeroList; }
/// <summary>
/// 已随机攻击大臣id列表
/// </summary>
public void addHadRandomAttackHeroList(long _hadRandomAttackHeroList) { hadRandomAttackHeroList.Add(_hadRandomAttackHeroList); }


public int GetBufSize() {
	int _size = 20;
	_size += 2 + (hadSelectAttackHeroList.Count * 8);
	_size += 2 + (hadRandomAttackHeroList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (hadSelectAttackHeroList.Count * 8);
	_size += 2 + (hadRandomAttackHeroList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastResetTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadSelectAttackNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadRandomAttackNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadBuyRandomAttackNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadSelectAttackHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadSelectAttackHeroListCount; _i++) { 
		long _hadSelectAttackHeroList = (long)0;
		_hadSelectAttackHeroList = _buf.getLong();
		hadSelectAttackHeroList.Add(_hadSelectAttackHeroList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadRandomAttackHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadRandomAttackHeroListCount; _i++) { 
		long _hadRandomAttackHeroList = (long)0;
		_hadRandomAttackHeroList = _buf.getLong();
		hadRandomAttackHeroList.Add(_hadRandomAttackHeroList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastResetTimeMs);
	_buf.putInt(hadSelectAttackNum);
	_buf.putInt(hadRandomAttackNum);
	_buf.putInt(hadBuyRandomAttackNum);
	_buf.putShort((short)hadSelectAttackHeroList.Count);
	for(int _i = 0; _i < hadSelectAttackHeroList.Count; _i++) { 
		_buf.putLong(hadSelectAttackHeroList[_i]);
	}
	_buf.putShort((short)hadRandomAttackHeroList.Count);
	for(int _i = 0; _i < hadRandomAttackHeroList.Count; _i++) { 
		_buf.putLong(hadRandomAttackHeroList[_i]);
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
	builder.Append("lastResetTimeMs").Append(":").Append(lastResetTimeMs.ToString()).Append(", ");
	builder.Append("hadSelectAttackNum").Append(":").Append(hadSelectAttackNum.ToString()).Append(", ");
	builder.Append("hadRandomAttackNum").Append(":").Append(hadRandomAttackNum.ToString()).Append(", ");
	builder.Append("hadBuyRandomAttackNum").Append(":").Append(hadBuyRandomAttackNum.ToString()).Append(", ");
	builder.Append("hadSelectAttackHeroList").Append(":").Append(hadSelectAttackHeroList.ToString()).Append(", ");
	builder.Append("hadRandomAttackHeroList").Append(":").Append(hadRandomAttackHeroList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

