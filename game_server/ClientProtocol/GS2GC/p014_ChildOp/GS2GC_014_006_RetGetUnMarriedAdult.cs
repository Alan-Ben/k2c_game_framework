using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 获取子嗣（成年未婚）数据
/// </summary>
public class GS2GC_014_006_RetGetUnMarriedAdult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.ChildObj.Adult_UnmarriedInfo adult;


public GS2GC_014_006_RetGetUnMarriedAdult() {
	adult = new Common.ChildObj.Adult_UnmarriedInfo();
}

public GS2GC_014_006_RetGetUnMarriedAdult(
	Common.ChildObj.Adult_UnmarriedInfo _adult
) {	adult = _adult;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 空
/// </summary>
public Common.ChildObj.Adult_UnmarriedInfo getAdult() { return adult; }
/// <summary>
/// 空
/// </summary>
public void setAdult(Common.ChildObj.Adult_UnmarriedInfo _adult) { adult = _adult; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + adult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + adult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.getCurPos();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.setPosition(_adultCurPos + _adultCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)6);
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
	builder.Append("adult").Append(":").Append(adult == null ? "null" : adult.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

