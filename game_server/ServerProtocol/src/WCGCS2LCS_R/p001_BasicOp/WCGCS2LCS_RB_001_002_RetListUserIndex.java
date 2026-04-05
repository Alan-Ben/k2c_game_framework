package WCGCS2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2LCS_RB_001_002_RetListUserIndex implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex> userServerID;


public WCGCS2LCS_RB_001_002_RetListUserIndex() {
	userServerID = new java.util.ArrayList<WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex>();
}

public WCGCS2LCS_RB_001_002_RetListUserIndex(
	 java.util.ArrayList<WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex> _userServerID
) {	userServerID = _userServerID;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public java.util.ArrayList<WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex> getUserServerID() { return userServerID; }
public void addUserServerID(WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex _userServerID) { userServerID.add(_userServerID); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (userServerID.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (userServerID.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _userServerIDCount = _buf.getShort();
	for(int _i = 0; _i < _userServerIDCount; _i++) { 
		WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex _userServerID = new WCGCS2LCS_R.p001_BasicOp.WCGCS2LCS_RB_001_002_Inter_UserIndex();
		if(_buf.remaining() <= 0) return;
	int __userServerIDCustLen = _buf.getInt();
	int __userServerIDCurPos = _buf.position();
	_userServerID.ReadUnzipBuf(_buf, __userServerIDCurPos + __userServerIDCustLen);
	_buf.position(__userServerIDCurPos + __userServerIDCustLen);

		userServerID.add(_userServerID);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)userServerID.size());
	for(int _i = 0; _i < userServerID.size(); _i++) { 
		_buf.putInt(userServerID.get(_i).GetBufSize());
	userServerID.get(_i).PutUnzipBuf(_buf);
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

