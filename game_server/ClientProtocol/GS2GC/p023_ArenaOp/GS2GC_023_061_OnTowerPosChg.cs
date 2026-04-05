using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_061_OnTowerPosChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo posInfo;


public GS2GC_023_061_OnTowerPosChg() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
}

public GS2GC_023_061_OnTowerPosChg(
	Common.TowerObj.Tower_PosInfo _posInfo
) {	posInfo = _posInfo;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
/// <summary>
/// 位置信息
/// </summary>
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }


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
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.getCurPos();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.setPosition(_posInfoCurPos + _posInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)61);
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
	builder.Append("posInfo").Append(":").Append(posInfo == null ? "null" : posInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

