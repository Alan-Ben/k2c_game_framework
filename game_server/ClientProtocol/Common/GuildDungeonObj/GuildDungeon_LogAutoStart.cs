using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 公会副本日志-自动开启
/// </summary>
public class GuildDungeon_LogAutoStart : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 副本ID
/// </summary>
private long dungeonId;
/// <summary>
/// 联盟财富消耗数值
/// </summary>
private long costValue;


public GuildDungeon_LogAutoStart() {
	dungeonId = (long)0;
	costValue = (long)0;
}

public GuildDungeon_LogAutoStart(
	long _dungeonId
	, long _costValue
) {	dungeonId = _dungeonId;
	costValue = _costValue;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 副本ID
/// </summary>
public long getDungeonId() { return dungeonId; }
/// <summary>
/// 副本ID
/// </summary>
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/// <summary>
/// 联盟财富消耗数值
/// </summary>
public long getCostValue() { return costValue; }
/// <summary>
/// 联盟财富消耗数值
/// </summary>
public void setCostValue(long _costValue) { costValue = _costValue; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costValue = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dungeonId);
	_buf.putLong(costValue);
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
	builder.Append("dungeonId").Append(":").Append(dungeonId.ToString()).Append(", ");
	builder.Append("costValue").Append(":").Append(costValue.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

