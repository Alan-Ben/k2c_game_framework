using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_064_OnTowerHighestPosHadReachChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 最高位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo pos;


public GS2GC_023_064_OnTowerHighestPosHadReachChg() {
	pos = new Common.TowerObj.Tower_PosInfo();
}

public GS2GC_023_064_OnTowerHighestPosHadReachChg(
	Common.TowerObj.Tower_PosInfo _pos
) {	pos = _pos;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)64; }

/// <summary>
/// 最高位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getPos() { return pos; }
/// <summary>
/// 最高位置信息
/// </summary>
public void setPos(Common.TowerObj.Tower_PosInfo _pos) { pos = _pos; }


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
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)64);
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
	builder.Append("pos").Append(":").Append(pos == null ? "null" : pos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

