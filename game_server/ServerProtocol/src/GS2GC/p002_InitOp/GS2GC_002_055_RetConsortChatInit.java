package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_055_RetConsortChatInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ConsortObj.Consort_ChatInfo> chatList;


public GS2GC_002_055_RetConsortChatInit() {
	chatList = new java.util.ArrayList<Common.ConsortObj.Consort_ChatInfo>();
}

public GS2GC_002_055_RetConsortChatInit(
	 java.util.ArrayList<Common.ConsortObj.Consort_ChatInfo> _chatList
) {	chatList = _chatList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)55; }

public java.util.ArrayList<Common.ConsortObj.Consort_ChatInfo> getChatList() { return chatList; }
public void addChatList(Common.ConsortObj.Consort_ChatInfo _chatList) { chatList.add(_chatList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < chatList.size(); _i++) {
	_size += 4 + chatList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < chatList.size(); _i++) {
	_size += 4 + chatList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _chatListCount = _buf.getShort();
	for(int _i = 0; _i < _chatListCount; _i++) { 
		Common.ConsortObj.Consort_ChatInfo _chatList = new Common.ConsortObj.Consort_ChatInfo();
		if(_buf.remaining() <= 0) return;
	int __chatListCustLen = _buf.getInt();
	int __chatListCurPos = _buf.position();
	_chatList.ReadUnzipBuf(_buf, __chatListCurPos + __chatListCustLen);
	_buf.position(__chatListCurPos + __chatListCustLen);

		chatList.add(_chatList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)chatList.size());
	for(int _i = 0; _i < chatList.size(); _i++) { 
		_buf.putInt(chatList.get(_i).GetBufSize());
	chatList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)55);
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

