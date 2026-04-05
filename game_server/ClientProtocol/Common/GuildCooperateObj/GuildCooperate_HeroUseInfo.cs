using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作大臣使用信息
/// </summary>
public class GuildCooperate_HeroUseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次刷新时间 ms
/// </summary>
private long lastRefreshTimeMs;
/// <summary>
/// 大臣ID
/// </summary>
private long heroId;
/// <summary>
/// 已使用次数
/// </summary>
private int useCount;
/// <summary>
/// 已恢复次数
/// </summary>
private int recoveredCount;


public GuildCooperate_HeroUseInfo() {
	lastRefreshTimeMs = (long)0;
	heroId = (long)0;
	useCount = 0;
	recoveredCount = 0;
}

public GuildCooperate_HeroUseInfo(
	long _lastRefreshTimeMs
	, long _heroId
	, int _useCount
	, int _recoveredCount
) {	lastRefreshTimeMs = _lastRefreshTimeMs;
	heroId = _heroId;
	useCount = _useCount;
	recoveredCount = _recoveredCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上次刷新时间 ms
/// </summary>
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/// <summary>
/// 上次刷新时间 ms
/// </summary>
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }
/// <summary>
/// 大臣ID
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣ID
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 已使用次数
/// </summary>
public int getUseCount() { return useCount; }
/// <summary>
/// 已使用次数
/// </summary>
public void setUseCount(int _useCount) { useCount = _useCount; }
/// <summary>
/// 已恢复次数
/// </summary>
public int getRecoveredCount() { return recoveredCount; }
/// <summary>
/// 已恢复次数
/// </summary>
public void setRecoveredCount(int _recoveredCount) { recoveredCount = _recoveredCount; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	useCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	recoveredCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastRefreshTimeMs);
	_buf.putLong(heroId);
	_buf.putInt(useCount);
	_buf.putInt(recoveredCount);
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
	builder.Append("lastRefreshTimeMs").Append(":").Append(lastRefreshTimeMs.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("useCount").Append(":").Append(useCount.ToString()).Append(", ");
	builder.Append("recoveredCount").Append(":").Append(recoveredCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

