using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p037_GuildDungeonOp
{

/// <summary>
/// 副本升级
/// </summary>
public class GC2GS_037_004_ReqUpgradeDungeonLvl : ALBasicProtocolPack._IALProtocolStructure {
private long dungeonId;
/// <summary>
/// 当前等级
/// </summary>
private int curLvl;


public GC2GS_037_004_ReqUpgradeDungeonLvl() {
	dungeonId = (long)0;
	curLvl = 0;
}

public GC2GS_037_004_ReqUpgradeDungeonLvl(
	long _dungeonId
	, int _curLvl
) {	dungeonId = _dungeonId;
	curLvl = _curLvl;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)4; }

public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/// <summary>
/// 当前等级
/// </summary>
public int getCurLvl() { return curLvl; }
/// <summary>
/// 当前等级
/// </summary>
public void setCurLvl(int _curLvl) { curLvl = _curLvl; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curLvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dungeonId);
	_buf.putInt(curLvl);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)4);
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
	builder.Append("curLvl").Append(":").Append(curLvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

