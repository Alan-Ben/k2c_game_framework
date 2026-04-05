using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 一键攻击
/// </summary>
public class GC2GS_023_013_ReqArenaAKeyAttack : ALBasicProtocolPack._IALProtocolStructure {
private Common.ArenaEnum.EArenaBuffType buffType;


public GC2GS_023_013_ReqArenaAKeyAttack() {
	buffType = 0;
}

public GC2GS_023_013_ReqArenaAKeyAttack(
	Common.ArenaEnum.EArenaBuffType _buffType
) {	buffType = _buffType;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)13; }

public Common.ArenaEnum.EArenaBuffType getBuffType() { return buffType; }
public void setBuffType(Common.ArenaEnum.EArenaBuffType _buffType) { buffType = _buffType; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buffType = (Common.ArenaEnum.EArenaBuffType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)buffType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)13);
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
	builder.Append("buffType").Append(":").Append(buffType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

