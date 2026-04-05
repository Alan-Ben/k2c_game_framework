using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPLS2GC.p001_BasicOp
{

public class NPLS2GC_001_006_RetEnterSNCode : ALBasicProtocolPack._IALProtocolStructure {
private int errCode;


public NPLS2GC_001_006_RetEnterSNCode() {
	errCode = 0;
}

public NPLS2GC_001_006_RetEnterSNCode(
	int _errCode
) {	errCode = _errCode;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)6; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }


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
	errCode = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

