package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_008_RetPlayerBuffList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<NPCommon.NPCommon_PlayerBuffInfo> playerBuffList;


public GS2GC_002_008_RetPlayerBuffList() {
	playerBuffList = new java.util.ArrayList<NPCommon.NPCommon_PlayerBuffInfo>();
}

public GS2GC_002_008_RetPlayerBuffList(
	 java.util.ArrayList<NPCommon.NPCommon_PlayerBuffInfo> _playerBuffList
) {	playerBuffList = _playerBuffList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)8; }

public java.util.ArrayList<NPCommon.NPCommon_PlayerBuffInfo> getPlayerBuffList() { return playerBuffList; }
public void addPlayerBuffList(NPCommon.NPCommon_PlayerBuffInfo _playerBuffList) { playerBuffList.add(_playerBuffList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (playerBuffList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (playerBuffList.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _playerBuffListCount = _buf.getShort();
	for(int _i = 0; _i < _playerBuffListCount; _i++) { 
		NPCommon.NPCommon_PlayerBuffInfo _playerBuffList = new NPCommon.NPCommon_PlayerBuffInfo();
		if(_buf.remaining() <= 0) return;
	int __playerBuffListCustLen = _buf.getInt();
	int __playerBuffListCurPos = _buf.position();
	_playerBuffList.ReadUnzipBuf(_buf, __playerBuffListCurPos + __playerBuffListCustLen);
	_buf.position(__playerBuffListCurPos + __playerBuffListCustLen);

		playerBuffList.add(_playerBuffList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)playerBuffList.size());
	for(int _i = 0; _i < playerBuffList.size(); _i++) { 
		_buf.putInt(playerBuffList.get(_i).GetBufSize());
	playerBuffList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)8);
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

