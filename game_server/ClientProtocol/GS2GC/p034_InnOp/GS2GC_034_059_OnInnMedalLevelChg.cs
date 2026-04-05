using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店奖牌等级变更
/// </summary>
public class GS2GC_034_059_OnInnMedalLevelChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 新奖牌等级
/// </summary>
private int medalLevel;


public GS2GC_034_059_OnInnMedalLevelChg() {
	medalLevel = 0;
}

public GS2GC_034_059_OnInnMedalLevelChg(
	int _medalLevel
) {	medalLevel = _medalLevel;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 新奖牌等级
/// </summary>
public int getMedalLevel() { return medalLevel; }
/// <summary>
/// 新奖牌等级
/// </summary>
public void setMedalLevel(int _medalLevel) { medalLevel = _medalLevel; }


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
	medalLevel = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(medalLevel);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)59);
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
	builder.Append("medalLevel").Append(":").Append(medalLevel.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

