package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_011_RetPlayerJoinedUSList implements ALBasicProtocolPack._IALProtocolStructure {
/** 最后一次登录的US服务器逻辑id */
private int lastJoinUSLogicId;
/** 玩家登录过的服务及服务器上的角色信息 */
private java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> joinedUSList;
/** 是否账号被封禁 */
private boolean isFreeze;
/** 封禁结束时间 */
private long freezeTimeMs;


public NPGS2GC_001_011_RetPlayerJoinedUSList() {
	lastJoinUSLogicId = 0;
	joinedUSList = new java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo>();
	isFreeze = false;
	freezeTimeMs = (long)0;
}

public NPGS2GC_001_011_RetPlayerJoinedUSList(
	 int _lastJoinUSLogicId
	, java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> _joinedUSList
	, boolean _isFreeze
	, long _freezeTimeMs
) {	lastJoinUSLogicId = _lastJoinUSLogicId;
	joinedUSList = _joinedUSList;
	isFreeze = _isFreeze;
	freezeTimeMs = _freezeTimeMs;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)11; }

/** 最后一次登录的US服务器逻辑id */
public int getLastJoinUSLogicId() { return lastJoinUSLogicId; }
/** 最后一次登录的US服务器逻辑id */
public void setLastJoinUSLogicId(int _lastJoinUSLogicId) { lastJoinUSLogicId = _lastJoinUSLogicId; }
/** 玩家登录过的服务及服务器上的角色信息 */
public java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> getJoinedUSList() { return joinedUSList; }
/** 玩家登录过的服务及服务器上的角色信息 */
public void addJoinedUSList(Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinedUSList) { joinedUSList.add(_joinedUSList); }
/** 是否账号被封禁 */
public boolean getIsFreeze() { return isFreeze; }
/** 是否账号被封禁 */
public void setIsFreeze(boolean _isFreeze) { isFreeze = _isFreeze; }
/** 封禁结束时间 */
public long getFreezeTimeMs() { return freezeTimeMs; }
/** 封禁结束时间 */
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


public final int GetBufSize() {
	int _size = 13;
	_size += 2;
	for(int _i = 0; _i < joinedUSList.size(); _i++) {
	_size += 4 + joinedUSList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;
	_size += 2;
	for(int _i = 0; _i < joinedUSList.size(); _i++) {
	_size += 4 + joinedUSList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastJoinUSLogicId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinedUSListCount = _buf.getShort();
	for(int _i = 0; _i < _joinedUSListCount; _i++) { 
		Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinedUSList = new Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo();
		if(_buf.remaining() <= 0) return;
	int __joinedUSListCustLen = _buf.getInt();
	int __joinedUSListCurPos = _buf.position();
	_joinedUSList.ReadUnzipBuf(_buf, __joinedUSListCurPos + __joinedUSListCustLen);
	_buf.position(__joinedUSListCurPos + __joinedUSListCustLen);

		joinedUSList.add(_joinedUSList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFreeze = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lastJoinUSLogicId);
	_buf.putShort((short)joinedUSList.size());
	for(int _i = 0; _i < joinedUSList.size(); _i++) { 
		_buf.putInt(joinedUSList.get(_i).GetBufSize());
	joinedUSList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(isFreeze?(byte)1:(byte)0);
	_buf.putLong(freezeTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)11);
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

