using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人-获取周期出游次数
/// </summary>
public class GS2GC_015_008_RetGetRoundTravelCount : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 出游次数
/// </summary>
private long counter;


public GS2GC_015_008_RetGetRoundTravelCount() {
	counter = (long)0;
}

public GS2GC_015_008_RetGetRoundTravelCount(
	long _counter
) {	counter = _counter;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 出游次数
/// </summary>
public long getCounter() { return counter; }
/// <summary>
/// 出游次数
/// </summary>
public void setCounter(long _counter) { counter = _counter; }


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
	counter = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(counter);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)8);
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
	builder.Append("counter").Append(":").Append(counter.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

