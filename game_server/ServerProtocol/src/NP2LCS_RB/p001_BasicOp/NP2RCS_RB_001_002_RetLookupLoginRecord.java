package NP2LCS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2RCS_RB_001_002_RetLookupLoginRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 登录记录 */
private java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> joinedUSList;


public NP2RCS_RB_001_002_RetLookupLoginRecord() {
	joinedUSList = new java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo>();
}

public NP2RCS_RB_001_002_RetLookupLoginRecord(
	 java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> _joinedUSList
) {	joinedUSList = _joinedUSList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

/** 登录记录 */
public java.util.ArrayList<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> getJoinedUSList() { return joinedUSList; }
/** 登录记录 */
public void addJoinedUSList(Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinedUSList) { joinedUSList.add(_joinedUSList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < joinedUSList.size(); _i++) {
	_size += 4 + joinedUSList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < joinedUSList.size(); _i++) {
	_size += 4 + joinedUSList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
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
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)joinedUSList.size());
	for(int _i = 0; _i < joinedUSList.size(); _i++) { 
		_buf.putInt(joinedUSList.get(_i).GetBufSize());
	joinedUSList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

