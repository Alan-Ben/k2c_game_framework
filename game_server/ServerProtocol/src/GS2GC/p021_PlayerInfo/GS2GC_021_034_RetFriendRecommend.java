package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GS2GC_021_034_RetFriendRecommend implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家信息列表 */
private java.util.ArrayList<NPCommon.PlayerInfo_IconShow> playerList;


public GS2GC_021_034_RetFriendRecommend() {
	playerList = new java.util.ArrayList<NPCommon.PlayerInfo_IconShow>();
}

public GS2GC_021_034_RetFriendRecommend(
	 java.util.ArrayList<NPCommon.PlayerInfo_IconShow> _playerList
) {	playerList = _playerList;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)34; }

/** 玩家信息列表 */
public java.util.ArrayList<NPCommon.PlayerInfo_IconShow> getPlayerList() { return playerList; }
/** 玩家信息列表 */
public void addPlayerList(NPCommon.PlayerInfo_IconShow _playerList) { playerList.add(_playerList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < playerList.size(); _i++) {
	_size += 4 + playerList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < playerList.size(); _i++) {
	_size += 4 + playerList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _playerListCount = _buf.getShort();
	for(int _i = 0; _i < _playerListCount; _i++) { 
		NPCommon.PlayerInfo_IconShow _playerList = new NPCommon.PlayerInfo_IconShow();
		if(_buf.remaining() <= 0) return;
	int __playerListCustLen = _buf.getInt();
	int __playerListCurPos = _buf.position();
	_playerList.ReadUnzipBuf(_buf, __playerListCurPos + __playerListCustLen);
	_buf.position(__playerListCurPos + __playerListCustLen);

		playerList.add(_playerList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)playerList.size());
	for(int _i = 0; _i < playerList.size(); _i++) { 
		_buf.putInt(playerList.get(_i).GetBufSize());
	playerList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)34);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)34);
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

