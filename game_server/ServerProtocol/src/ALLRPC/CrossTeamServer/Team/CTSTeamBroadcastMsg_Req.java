package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSTeamBroadcastMsg_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
private byte[] protocol;


public CTSTeamBroadcastMsg_Req() {
	teamId = (long)0;
	protocol = null;
}

public CTSTeamBroadcastMsg_Req(
	 long _teamId
	, byte[] _protocol
) {	teamId = _teamId;
	protocol = _protocol;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
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
	int _size = 8;
	_size += 4 + (protocol == null ? 0 : protocol.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (protocol == null ? 0 : protocol.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _protocolCount = _buf.getInt();
	if(0 < _protocolCount){
		protocol = new byte[_protocolCount];
		_buf.get(protocol);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putInt((protocol == null ? 0 : protocol.length));
	if(null != protocol){_buf.put(protocol);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

