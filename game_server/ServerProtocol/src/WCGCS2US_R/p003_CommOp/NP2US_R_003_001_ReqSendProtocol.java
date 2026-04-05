package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 玩家列表推送协议
 **/
public class NP2US_R_003_001_ReqSendProtocol implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> cidList;
private byte[] protocol;


public NP2US_R_003_001_ReqSendProtocol() {
	cidList = new java.util.ArrayList<Long>();
	protocol = null;
}

public NP2US_R_003_001_ReqSendProtocol(
	 java.util.ArrayList<Long> _cidList
	, byte[] _protocol
) {	cidList = _cidList;
	protocol = _protocol;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

public java.util.ArrayList<Long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.add(_cidList); }
public byte[] getProtocol() { return protocol; }
public java.nio.ByteBuffer get_buffer_Protocol() { if(null == protocol)return null; else return ByteBuffer.wrap(protocol); }

public void setProtocol(byte[] _protocol) { protocol = _protocol; }
public void setProtocol(java.nio.ByteBuffer _protocol) 
{
	if(null == _protocol){return;}
	int _oldPos = _protocol.position();
	int _bufLength = _protocol.remaining();
	protocol = new byte[_bufLength];
	_protocol.get(protocol);
	_protocol.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cidList.size() * 8);
	_size += 4 + (protocol == null ? 0 : protocol.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cidList.size() * 8);
	_size += 4 + (protocol == null ? 0 : protocol.length);

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
	int _protocolCount = _buf.getInt();
	if(0 < _protocolCount){
		protocol = new byte[_protocolCount];
		_buf.get(protocol);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
	_buf.putInt((protocol == null ? 0 : protocol.length));
	if(null != protocol){_buf.put(protocol);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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

