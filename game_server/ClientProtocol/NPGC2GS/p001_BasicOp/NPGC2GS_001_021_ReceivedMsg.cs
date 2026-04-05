using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGC2GS.p001_BasicOp
{

public class NPGC2GS_001_021_ReceivedMsg : ALBasicProtocolPack._IALProtocolStructure {
private int curRecMesCount;


public NPGC2GS_001_021_ReceivedMsg() {
	curRecMesCount = 0;
}

public NPGC2GS_001_021_ReceivedMsg(
	int _curRecMesCount
) {	curRecMesCount = _curRecMesCount;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)21; }

public int getCurRecMesCount() { return curRecMesCount; }
public void setCurRecMesCount(int _curRecMesCount) { curRecMesCount = _curRecMesCount; }


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
	curRecMesCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(curRecMesCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)21);
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
	builder.Append("curRecMesCount").Append(":").Append(curRecMesCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

