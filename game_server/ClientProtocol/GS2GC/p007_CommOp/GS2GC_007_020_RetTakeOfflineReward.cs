using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_020_RetTakeOfflineReward : ALBasicProtocolPack._IALProtocolStructure {
private byte[] extData;


public GS2GC_007_020_RetTakeOfflineReward() {
	extData = null;
}

public GS2GC_007_020_RetTakeOfflineReward(
	byte[] _extData
) {	extData = _extData;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)20; }

public byte[] getExtData() { return extData; }

public void setExtData(byte[] _extData) { extData = _extData; }



public int GetBufSize() {
	int _size = 0;
	_size += 4 + (extData == null ? 0 : extData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (extData == null ? 0 : extData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putByteBuffer(extData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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
	builder.Append("extData").Append(":").Append(extData == null ? "null" : extData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

