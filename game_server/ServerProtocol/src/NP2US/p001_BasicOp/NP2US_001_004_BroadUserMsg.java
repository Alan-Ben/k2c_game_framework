package NP2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_001_004_BroadUserMsg implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> cidList;
private byte[] msgBuffer;


public NP2US_001_004_BroadUserMsg() {
	cidList = new java.util.ArrayList<Long>();
	msgBuffer = null;
}

public NP2US_001_004_BroadUserMsg(
	 java.util.ArrayList<Long> _cidList
	, byte[] _msgBuffer
) {	cidList = _cidList;
	msgBuffer = _msgBuffer;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

public java.util.ArrayList<Long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.add(_cidList); }
public byte[] getMsgBuffer() { return msgBuffer; }
public java.nio.ByteBuffer get_buffer_MsgBuffer() { if(null == msgBuffer)return null; else return ByteBuffer.wrap(msgBuffer); }

public void setMsgBuffer(byte[] _msgBuffer) { msgBuffer = _msgBuffer; }
public void setMsgBuffer(java.nio.ByteBuffer _msgBuffer) 
{
	if(null == _msgBuffer){return;}
	int _oldPos = _msgBuffer.position();
	int _bufLength = _msgBuffer.remaining();
	msgBuffer = new byte[_bufLength];
	_msgBuffer.get(msgBuffer);
	_msgBuffer.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cidList.size() * 8);
	_size += 4 + (msgBuffer == null ? 0 : msgBuffer.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cidList.size() * 8);
	_size += 4 + (msgBuffer == null ? 0 : msgBuffer.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		if(_buf.remaining() > 0) _cidList = _buf.getLong();
		cidList.add(_cidList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgBufferCount = _buf.getInt();
	if(0 < _msgBufferCount){
		msgBuffer = new byte[_msgBufferCount];
		_buf.get(msgBuffer);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
	_buf.putInt((msgBuffer == null ? 0 : msgBuffer.length));
	if(null != msgBuffer){_buf.put(msgBuffer);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)4);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

