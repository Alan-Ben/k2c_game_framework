using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 公会副本日志-开启
/// </summary>
public class GuildDungeon_LogStart : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/// <summary>
/// 副本ID
/// </summary>
private long dungeonId;
/// <summary>
/// 开启方式
/// </summary>
private Common.GuildDungeonEnum.EGuildDungeon_StartType startType;
/// <summary>
/// 开启消耗数值
/// </summary>
private long startCost;


public GuildDungeon_LogStart() {
	cid = (long)0;
	dungeonId = (long)0;
	startType = 0;
	startCost = (long)0;
}

public GuildDungeon_LogStart(
	long _cid
	, long _dungeonId
	, Common.GuildDungeonEnum.EGuildDungeon_StartType _startType
	, long _startCost
) {	cid = _cid;
	dungeonId = _dungeonId;
	startType = _startType;
	startCost = _startCost;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 副本ID
/// </summary>
public long getDungeonId() { return dungeonId; }
/// <summary>
/// 副本ID
/// </summary>
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/// <summary>
/// 开启方式
/// </summary>
public Common.GuildDungeonEnum.EGuildDungeon_StartType getStartType() { return startType; }
/// <summary>
/// 开启方式
/// </summary>
public void setStartType(Common.GuildDungeonEnum.EGuildDungeon_StartType _startType) { startType = _startType; }
/// <summary>
/// 开启消耗数值
/// </summary>
public long getStartCost() { return startCost; }
/// <summary>
/// 开启消耗数值
/// </summary>
public void setStartCost(long _startCost) { startCost = _startCost; }


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
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startType = (Common.GuildDungeonEnum.EGuildDungeon_StartType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startCost = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(dungeonId);
	_buf.putInt((int)startType);

	_buf.putLong(startCost);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("dungeonId").Append(":").Append(dungeonId.ToString()).Append(", ");
	builder.Append("startType").Append(":").Append(startType.ToString()).Append(", ");
	builder.Append("startCost").Append(":").Append(startCost.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

