using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 出战大臣数据
/// </summary>
public class GuildDungeon_FightHero : ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
/// <summary>
/// 出战次数
/// </summary>
private int fightedCount;
/// <summary>
/// 已恢复次数
/// </summary>
private int recoveredCount;
/// <summary>
/// 最后一次出战时间（毫秒）
/// </summary>
private long lastFightedMs;


public GuildDungeon_FightHero() {
	heroId = (long)0;
	fightedCount = 0;
	recoveredCount = 0;
	lastFightedMs = (long)0;
}

public GuildDungeon_FightHero(
	long _heroId
	, int _fightedCount
	, int _recoveredCount
	, long _lastFightedMs
) {	heroId = _heroId;
	fightedCount = _fightedCount;
	recoveredCount = _recoveredCount;
	lastFightedMs = _lastFightedMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 出战次数
/// </summary>
public int getFightedCount() { return fightedCount; }
/// <summary>
/// 出战次数
/// </summary>
public void setFightedCount(int _fightedCount) { fightedCount = _fightedCount; }
/// <summary>
/// 已恢复次数
/// </summary>
public int getRecoveredCount() { return recoveredCount; }
/// <summary>
/// 已恢复次数
/// </summary>
public void setRecoveredCount(int _recoveredCount) { recoveredCount = _recoveredCount; }
/// <summary>
/// 最后一次出战时间（毫秒）
/// </summary>
public long getLastFightedMs() { return lastFightedMs; }
/// <summary>
/// 最后一次出战时间（毫秒）
/// </summary>
public void setLastFightedMs(long _lastFightedMs) { lastFightedMs = _lastFightedMs; }


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
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fightedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	recoveredCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastFightedMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putInt(fightedCount);
	_buf.putInt(recoveredCount);
	_buf.putLong(lastFightedMs);
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
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("fightedCount").Append(":").Append(fightedCount.ToString()).Append(", ");
	builder.Append("recoveredCount").Append(":").Append(recoveredCount.ToString()).Append(", ");
	builder.Append("lastFightedMs").Append(":").Append(lastFightedMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

