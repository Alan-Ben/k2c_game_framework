using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGC2LS.p001_BasicOp
{

public class NPGC2LS_001_006_ReqEnterSNCode : ALBasicProtocolPack._IALProtocolStructure {
private string snCode;


public NPGC2LS_001_006_ReqEnterSNCode() {
	snCode = "";
}

public NPGC2LS_001_006_ReqEnterSNCode(
	string _snCode
) {	snCode = _snCode;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)6; }

public string getSnCode() { return snCode; }
public void setSnCode(string _snCode) { snCode = _snCode; }


public int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(snCode);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(snCode);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	snCode = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(snCode);
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
	builder.Append("snCode").Append(":").Append(snCode.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

