using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_053_OnPlayerBuffChg : ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_PlayerBuffInfo buff;


public GS2GC_004_053_OnPlayerBuffChg() {
	buff = new NPCommon.NPCommon_PlayerBuffInfo();
}

public GS2GC_004_053_OnPlayerBuffChg(
	NPCommon.NPCommon_PlayerBuffInfo _buff
) {	buff = _buff;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)53; }

public NPCommon.NPCommon_PlayerBuffInfo getBuff() { return buff; }
public void setBuff(NPCommon.NPCommon_PlayerBuffInfo _buff) { buff = _buff; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _buffCustLen = _buf.getInt();
	int _buffCurPos = _buf.getCurPos();
	buff.ReadUnzipBuf(_buf, _buffCurPos + _buffCustLen);
	_buf.setPosition(_buffCurPos + _buffCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(buff.GetBufSize());
	buff.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)53);
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
	builder.Append("buff").Append(":").Append(buff == null ? "null" : buff.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

