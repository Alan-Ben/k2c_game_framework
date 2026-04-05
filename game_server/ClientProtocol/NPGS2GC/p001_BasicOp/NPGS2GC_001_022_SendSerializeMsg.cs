using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_022_SendSerializeMsg : ALBasicProtocolPack._IALProtocolStructure {
private int msgSerialize;
private byte[] msg;


public NPGS2GC_001_022_SendSerializeMsg() {
	msgSerialize = 0;
	msg = null;
}

public NPGS2GC_001_022_SendSerializeMsg(
	int _msgSerialize
	, byte[] _msg
) {	msgSerialize = _msgSerialize;
	msg = _msg;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)22; }

public int getMsgSerialize() { return msgSerialize; }
public void setMsgSerialize(int _msgSerialize) { msgSerialize = _msgSerialize; }
public byte[] getMsg() { return msg; }

public void setMsg(byte[] _msg) { msg = _msg; }



public int GetBufSize() {
	int _size = 4;
	_size += 4 + (msg == null ? 0 : msg.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (msg == null ? 0 : msg.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msg = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(msgSerialize);
	_buf.putByteBuffer(msg);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)22);
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
	builder.Append("msgSerialize").Append(":").Append(msgSerialize.ToString()).Append(", ");
	builder.Append("msg").Append(":").Append(msg == null ? "null" : msg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

