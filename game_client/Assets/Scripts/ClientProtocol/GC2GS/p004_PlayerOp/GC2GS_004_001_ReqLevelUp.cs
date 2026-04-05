using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

public class GC2GS_004_001_ReqLevelUp : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前等级，为了防止多次发送错误，这里用于校验
/// </summary>
private int curLvl;


public GC2GS_004_001_ReqLevelUp() {
	curLvl = 0;
}

public GC2GS_004_001_ReqLevelUp(
	int _curLvl
) {	curLvl = _curLvl;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 当前等级，为了防止多次发送错误，这里用于校验
/// </summary>
public int getCurLvl() { return curLvl; }
/// <summary>
/// 当前等级，为了防止多次发送错误，这里用于校验
/// </summary>
public void setCurLvl(int _curLvl) { curLvl = _curLvl; }


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
	curLvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(curLvl);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)1);
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
	builder.Append("curLvl").Append(":").Append(curLvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

