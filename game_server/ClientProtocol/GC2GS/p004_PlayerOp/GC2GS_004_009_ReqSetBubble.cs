using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

public class GC2GS_004_009_ReqSetBubble : ALBasicProtocolPack._IALProtocolStructure {
private long bubbleId;


public GC2GS_004_009_ReqSetBubble() {
	bubbleId = (long)0;
}

public GC2GS_004_009_ReqSetBubble(
	long _bubbleId
) {	bubbleId = _bubbleId;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)9; }

public long getBubbleId() { return bubbleId; }
public void setBubbleId(long _bubbleId) { bubbleId = _bubbleId; }


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
	bubbleId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(bubbleId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)9);
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
	builder.Append("bubbleId").Append(":").Append(bubbleId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

