using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_020_RetReconnectInfo : ALBasicProtocolPack._IALProtocolStructure {
private int receivedMesCount;
private int curServerResVersion;


public NPGS2GC_001_020_RetReconnectInfo() {
	receivedMesCount = 0;
	curServerResVersion = 0;
}

public NPGS2GC_001_020_RetReconnectInfo(
	int _receivedMesCount
	, int _curServerResVersion
) {	receivedMesCount = _receivedMesCount;
	curServerResVersion = _curServerResVersion;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)20; }

public int getReceivedMesCount() { return receivedMesCount; }
public void setReceivedMesCount(int _receivedMesCount) { receivedMesCount = _receivedMesCount; }
public int getCurServerResVersion() { return curServerResVersion; }
public void setCurServerResVersion(int _curServerResVersion) { curServerResVersion = _curServerResVersion; }


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
	receivedMesCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curServerResVersion = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(receivedMesCount);
	_buf.putInt(curServerResVersion);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)20);
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
	builder.Append("receivedMesCount").Append(":").Append(receivedMesCount.ToString()).Append(", ");
	builder.Append("curServerResVersion").Append(":").Append(curServerResVersion.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

