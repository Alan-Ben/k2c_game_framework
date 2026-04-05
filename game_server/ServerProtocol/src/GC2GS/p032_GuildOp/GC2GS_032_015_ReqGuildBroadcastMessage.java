package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求群发消息
 **/
public class GC2GS_032_015_ReqGuildBroadcastMessage implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> cidList;
private String message;


public GC2GS_032_015_ReqGuildBroadcastMessage() {
	cidList = new java.util.ArrayList<Long>();
	message = "";
}

public GC2GS_032_015_ReqGuildBroadcastMessage(
	 java.util.ArrayList<Long> _cidList
	, String _message
) {	cidList = _cidList;
	message = _message;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)15; }

public java.util.ArrayList<Long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.add(_cidList); }
public String getMessage() { return message; }
public void setMessage(String _message) { message = _message; }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cidList.size() * 8);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(message);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cidList.size() * 8);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(message);

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
	if(_buf.remaining() > 0) message = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, message);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)15);
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

