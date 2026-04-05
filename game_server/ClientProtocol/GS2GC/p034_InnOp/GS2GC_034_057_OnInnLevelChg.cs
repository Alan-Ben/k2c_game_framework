using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店等级变更
/// </summary>
public class GS2GC_034_057_OnInnLevelChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 新等级
/// </summary>
private int newLevel;


public GS2GC_034_057_OnInnLevelChg() {
	newLevel = 0;
}

public GS2GC_034_057_OnInnLevelChg(
	int _newLevel
) {	newLevel = _newLevel;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 新等级
/// </summary>
public int getNewLevel() { return newLevel; }
/// <summary>
/// 新等级
/// </summary>
public void setNewLevel(int _newLevel) { newLevel = _newLevel; }


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
	newLevel = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(newLevel);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)57);
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
	builder.Append("newLevel").Append(":").Append(newLevel.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

