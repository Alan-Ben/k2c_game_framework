using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p008_TravelOp
{

/// <summary>
/// 妃子数据变化推送
/// </summary>
public class GS2GC_008_052_OnConsortChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 妃子数据
/// </summary>
private Common.TravelObj.Travel_Consort consort;


public GS2GC_008_052_OnConsortChg() {
	consort = new Common.TravelObj.Travel_Consort();
}

public GS2GC_008_052_OnConsortChg(
	Common.TravelObj.Travel_Consort _consort
) {	consort = _consort;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 妃子数据
/// </summary>
public Common.TravelObj.Travel_Consort getConsort() { return consort; }
/// <summary>
/// 妃子数据
/// </summary>
public void setConsort(Common.TravelObj.Travel_Consort _consort) { consort = _consort; }


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
	int _consortCustLen = _buf.getInt();
	int _consortCurPos = _buf.getCurPos();
	consort.ReadUnzipBuf(_buf, _consortCurPos + _consortCustLen);
	_buf.setPosition(_consortCurPos + _consortCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(consort.GetBufSize());
	consort.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)52);
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
	builder.Append("consort").Append(":").Append(consort == null ? "null" : consort.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

