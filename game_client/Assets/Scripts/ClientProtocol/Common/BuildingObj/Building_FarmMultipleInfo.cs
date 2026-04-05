using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.BuildingObj
{

/// <summary>
/// 农田暴击信息
/// </summary>
public class Building_FarmMultipleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 暴击次数
/// </summary>
private int critCount;
/// <summary>
/// 上次刷新暴击次数时间 毫秒
/// </summary>
private long lastRefreshTimeMs;
/// <summary>
/// 点击次数
/// </summary>
private long clickNum;
/// <summary>
/// 随机种子
/// </summary>
private long randomSeed;


public Building_FarmMultipleInfo() {
	critCount = 0;
	lastRefreshTimeMs = (long)0;
	clickNum = (long)0;
	randomSeed = (long)0;
}

public Building_FarmMultipleInfo(
	int _critCount
	, long _lastRefreshTimeMs
	, long _clickNum
	, long _randomSeed
) {	critCount = _critCount;
	lastRefreshTimeMs = _lastRefreshTimeMs;
	clickNum = _clickNum;
	randomSeed = _randomSeed;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 暴击次数
/// </summary>
public int getCritCount() { return critCount; }
/// <summary>
/// 暴击次数
/// </summary>
public void setCritCount(int _critCount) { critCount = _critCount; }
/// <summary>
/// 上次刷新暴击次数时间 毫秒
/// </summary>
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/// <summary>
/// 上次刷新暴击次数时间 毫秒
/// </summary>
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }
/// <summary>
/// 点击次数
/// </summary>
public long getClickNum() { return clickNum; }
/// <summary>
/// 点击次数
/// </summary>
public void setClickNum(long _clickNum) { clickNum = _clickNum; }
/// <summary>
/// 随机种子
/// </summary>
public long getRandomSeed() { return randomSeed; }
/// <summary>
/// 随机种子
/// </summary>
public void setRandomSeed(long _randomSeed) { randomSeed = _randomSeed; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	critCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clickNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	randomSeed = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(critCount);
	_buf.putLong(lastRefreshTimeMs);
	_buf.putLong(clickNum);
	_buf.putLong(randomSeed);
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
	builder.Append("critCount").Append(":").Append(critCount.ToString()).Append(", ");
	builder.Append("lastRefreshTimeMs").Append(":").Append(lastRefreshTimeMs.ToString()).Append(", ");
	builder.Append("clickNum").Append(":").Append(clickNum.ToString()).Append(", ");
	builder.Append("randomSeed").Append(":").Append(randomSeed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

