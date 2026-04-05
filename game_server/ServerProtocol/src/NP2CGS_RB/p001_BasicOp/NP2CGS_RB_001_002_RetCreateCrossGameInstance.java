package NP2CGS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2CGS_RB_001_002_RetCreateCrossGameInstance implements ALBasicProtocolPack._IALProtocolStructure {
/** 消息 */
private byte[] retMsg;


public NP2CGS_RB_001_002_RetCreateCrossGameInstance() {
	retMsg = null;
}

public NP2CGS_RB_001_002_RetCreateCrossGameInstance(
	 byte[] _retMsg
) {	retMsg = _retMsg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

/** 消息 */
public byte[] getRetMsg() { return retMsg; }
public java.nio.ByteBuffer get_buffer_RetMsg() { if(null == retMsg)return null; else return ByteBuffer.wrap(retMsg); }

/** 消息 */
public void setRetMsg(byte[] _retMsg) { retMsg = _retMsg; }
public void setRetMsg(java.nio.ByteBuffer _retMsg) 
{
	if(null == _retMsg){return;}
	int _oldPos = _retMsg.position();
	int _bufLength = _retMsg.remaining();
	retMsg = new byte[_bufLength];
	_retMsg.get(retMsg);
	_retMsg.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 4 + (retMsg == null ? 0 : retMsg.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (retMsg == null ? 0 : retMsg.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _retMsgCount = _buf.getInt();
	if(0 < _retMsgCount){
		retMsg = new byte[_retMsgCount];
		_buf.get(retMsg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt((retMsg == null ? 0 : retMsg.length));
	if(null != retMsg){_buf.put(retMsg);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

