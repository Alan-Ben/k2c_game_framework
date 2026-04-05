using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 所有子嗣（成年/未成年）总收益变动推送
/// </summary>
public class GS2GC_014_061_OnChildBonusSumChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 所有子嗣总收益
/// </summary>
private long bonus;


public GS2GC_014_061_OnChildBonusSumChg() {
	bonus = (long)0;
}

public GS2GC_014_061_OnChildBonusSumChg(
	long _bonus
) {	bonus = _bonus;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 所有子嗣总收益
/// </summary>
public long getBonus() { return bonus; }
/// <summary>
/// 所有子嗣总收益
/// </summary>
public void setBonus(long _bonus) { bonus = _bonus; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bonus = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bonus);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
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
	builder.Append("bonus").Append(":").Append(bonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

