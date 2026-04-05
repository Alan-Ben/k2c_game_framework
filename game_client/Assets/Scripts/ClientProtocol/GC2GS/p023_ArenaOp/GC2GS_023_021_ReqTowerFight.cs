using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 爬塔攻击
/// </summary>
public class GC2GS_023_021_ReqTowerFight : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo targetPos;


public GC2GS_023_021_ReqTowerFight() {
	targetPos = new Common.TowerObj.Tower_PosInfo();
}

public GC2GS_023_021_ReqTowerFight(
	Common.TowerObj.Tower_PosInfo _targetPos
) {	targetPos = _targetPos;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 目标位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getTargetPos() { return targetPos; }
/// <summary>
/// 目标位置信息
/// </summary>
public void setTargetPos(Common.TowerObj.Tower_PosInfo _targetPos) { targetPos = _targetPos; }


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
	int _targetPosCustLen = _buf.getInt();
	int _targetPosCurPos = _buf.getCurPos();
	targetPos.ReadUnzipBuf(_buf, _targetPosCurPos + _targetPosCustLen);
	_buf.setPosition(_targetPosCurPos + _targetPosCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(targetPos.GetBufSize());
	targetPos.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)21);
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
	builder.Append("targetPos").Append(":").Append(targetPos == null ? "null" : targetPos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

