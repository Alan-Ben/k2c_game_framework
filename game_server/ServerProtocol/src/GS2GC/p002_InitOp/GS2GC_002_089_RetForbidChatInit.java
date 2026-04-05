package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_089_RetForbidChatInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<NPCommon.NPCommon_ForbidChatInfo> forbidChatList;


public GS2GC_002_089_RetForbidChatInit() {
	forbidChatList = new java.util.ArrayList<NPCommon.NPCommon_ForbidChatInfo>();
}

public GS2GC_002_089_RetForbidChatInit(
	 java.util.ArrayList<NPCommon.NPCommon_ForbidChatInfo> _forbidChatList
) {	forbidChatList = _forbidChatList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)89; }

public java.util.ArrayList<NPCommon.NPCommon_ForbidChatInfo> getForbidChatList() { return forbidChatList; }
public void addForbidChatList(NPCommon.NPCommon_ForbidChatInfo _forbidChatList) { forbidChatList.add(_forbidChatList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (forbidChatList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (forbidChatList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _forbidChatListCount = _buf.getShort();
	for(int _i = 0; _i < _forbidChatListCount; _i++) { 
		NPCommon.NPCommon_ForbidChatInfo _forbidChatList = new NPCommon.NPCommon_ForbidChatInfo();
		if(_buf.remaining() <= 0) return;
	int __forbidChatListCustLen = _buf.getInt();
	int __forbidChatListCurPos = _buf.position();
	_forbidChatList.ReadUnzipBuf(_buf, __forbidChatListCurPos + __forbidChatListCustLen);
	_buf.position(__forbidChatListCurPos + __forbidChatListCustLen);

		forbidChatList.add(_forbidChatList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)forbidChatList.size());
	for(int _i = 0; _i < forbidChatList.size(); _i++) { 
		_buf.putInt(forbidChatList.get(_i).GetBufSize());
	forbidChatList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)89);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)89);
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

