package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_057_OnSelfRequestJoinGuildListChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 自己请求加入联盟列表 */
private java.util.ArrayList<Long> selfRequestJoinGuildList;


public GS2GC_032_057_OnSelfRequestJoinGuildListChg() {
	selfRequestJoinGuildList = new java.util.ArrayList<Long>();
}

public GS2GC_032_057_OnSelfRequestJoinGuildListChg(
	 java.util.ArrayList<Long> _selfRequestJoinGuildList
) {	selfRequestJoinGuildList = _selfRequestJoinGuildList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)57; }

/** 自己请求加入联盟列表 */
public java.util.ArrayList<Long> getSelfRequestJoinGuildList() { return selfRequestJoinGuildList; }
/** 自己请求加入联盟列表 */
public void addSelfRequestJoinGuildList(long _selfRequestJoinGuildList) { selfRequestJoinGuildList.add(_selfRequestJoinGuildList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (selfRequestJoinGuildList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (selfRequestJoinGuildList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _selfRequestJoinGuildListCount = _buf.getShort();
	for(int _i = 0; _i < _selfRequestJoinGuildListCount; _i++) { 
		long _selfRequestJoinGuildList = (long)0;
		if(_buf.remaining() > 0) _selfRequestJoinGuildList = _buf.getLong();
		selfRequestJoinGuildList.add(_selfRequestJoinGuildList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)selfRequestJoinGuildList.size());
	for(int _i = 0; _i < selfRequestJoinGuildList.size(); _i++) { 
		_buf.putLong(selfRequestJoinGuildList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)57);
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

