using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_002_RetGmCommand : ALBasicProtocolPack._IALProtocolStructure {
private bool isSucc;
private string result;
private byte[] zipedResult;


public GS2GC_004_002_RetGmCommand() {
	isSucc = false;
	result = "";
	zipedResult = null;
}

public GS2GC_004_002_RetGmCommand(
	bool _isSucc
	, string _result
	, byte[] _zipedResult
) {	isSucc = _isSucc;
	result = _result;
	zipedResult = _zipedResult;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)2; }

public bool getIsSucc() { return isSucc; }
public void setIsSucc(bool _isSucc) { isSucc = _isSucc; }
public string getResult() { return result; }
public void setResult(string _result) { result = _result; }
public byte[] getZipedResult() { return zipedResult; }

public void setZipedResult(byte[] _zipedResult) { zipedResult = _zipedResult; }



public int GetBufSize() {
	int _size = 1;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result);
	_size += 4 + (zipedResult == null ? 0 : zipedResult.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result);
	_size += 4 + (zipedResult == null ? 0 : zipedResult.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	result = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	zipedResult = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putString(result);
	_buf.putByteBuffer(zipedResult);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)2);
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
	builder.Append("isSucc").Append(":").Append(isSucc.ToString()).Append(", ");
	builder.Append("result").Append(":").Append(result.ToString()).Append(", ");
	builder.Append("zipedResult").Append(":").Append(zipedResult == null ? "null" : zipedResult.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

