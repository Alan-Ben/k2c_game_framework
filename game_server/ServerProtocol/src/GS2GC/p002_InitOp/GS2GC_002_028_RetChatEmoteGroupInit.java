package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_028_RetChatEmoteGroupInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup> emoteGroupList;


public GS2GC_002_028_RetChatEmoteGroupInit() {
	emoteGroupList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup>();
}

public GS2GC_002_028_RetChatEmoteGroupInit(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup> _emoteGroupList
) {	emoteGroupList = _emoteGroupList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)28; }

public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup> getEmoteGroupList() { return emoteGroupList; }
public void addEmoteGroupList(Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup _emoteGroupList) { emoteGroupList.add(_emoteGroupList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (emoteGroupList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (emoteGroupList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _emoteGroupListCount = _buf.getShort();
	for(int _i = 0; _i < _emoteGroupListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup _emoteGroupList = new Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup();
		if(_buf.remaining() <= 0) return;
	int __emoteGroupListCustLen = _buf.getInt();
	int __emoteGroupListCurPos = _buf.position();
	_emoteGroupList.ReadUnzipBuf(_buf, __emoteGroupListCurPos + __emoteGroupListCustLen);
	_buf.position(__emoteGroupListCurPos + __emoteGroupListCustLen);

		emoteGroupList.add(_emoteGroupList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)emoteGroupList.size());
	for(int _i = 0; _i < emoteGroupList.size(); _i++) { 
		_buf.putInt(emoteGroupList.get(_i).GetBufSize());
	emoteGroupList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)28);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)28);
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

