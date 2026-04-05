using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-矿采集日志
/// </summary>
public class Mars_MineCollectLog : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍ID
/// </summary>
private long teamId;
/// <summary>
/// 矿点配置ID
/// </summary>
private long mineRefId;
/// <summary>
/// 采集数量
/// </summary>
private long collectNum;


public Mars_MineCollectLog() {
	teamId = (long)0;
	mineRefId = (long)0;
	collectNum = (long)0;
}

public Mars_MineCollectLog(
	long _teamId
	, long _mineRefId
	, long _collectNum
) {	teamId = _teamId;
	mineRefId = _mineRefId;
	collectNum = _collectNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 队伍ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 矿点配置ID
/// </summary>
public long getMineRefId() { return mineRefId; }
/// <summary>
/// 矿点配置ID
/// </summary>
public void setMineRefId(long _mineRefId) { mineRefId = _mineRefId; }
/// <summary>
/// 采集数量
/// </summary>
public long getCollectNum() { return collectNum; }
/// <summary>
/// 采集数量
/// </summary>
public void setCollectNum(long _collectNum) { collectNum = _collectNum; }


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
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	collectNum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putLong(mineRefId);
	_buf.putLong(collectNum);
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
	builder.Append("teamId").Append(":").Append(teamId.ToString()).Append(", ");
	builder.Append("mineRefId").Append(":").Append(mineRefId.ToString()).Append(", ");
	builder.Append("collectNum").Append(":").Append(collectNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

