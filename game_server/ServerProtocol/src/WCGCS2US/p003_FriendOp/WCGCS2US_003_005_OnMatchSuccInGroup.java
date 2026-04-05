package WCGCS2US.p003_FriendOp;

import java.nio.ByteBuffer;
public class WCGCS2US_003_005_OnMatchSuccInGroup implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private java.util.ArrayList<WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer> members;


public WCGCS2US_003_005_OnMatchSuccInGroup() {
	uid = (long)0;
	members = new java.util.ArrayList<WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer>();
}

public WCGCS2US_003_005_OnMatchSuccInGroup(
	 long _uid
	, java.util.ArrayList<WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer> _members
) {	uid = _uid;
	members = _members;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)5; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public java.util.ArrayList<WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer> getMembers() { return members; }
public void addMembers(WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer _members) { members.add(_members); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < members.size(); _i++) {
	_size += 4 + members.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < members.size(); _i++) {
	_size += 4 + members.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _membersCount = _buf.getShort();
	for(int _i = 0; _i < _membersCount; _i++) { 
		WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer _members = new WCGCS2US.p003_FriendOp.WCGCS2US_003_005_Inner_GroupPlayer();
		if(_buf.remaining() <= 0) return;
	int __membersCustLen = _buf.getInt();
	int __membersCurPos = _buf.position();
	_members.ReadUnzipBuf(_buf, __membersCurPos + __membersCustLen);
	_buf.position(__membersCurPos + __membersCustLen);

		members.add(_members);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putShort((short)members.size());
	for(int _i = 0; _i < members.size(); _i++) { 
		_buf.putInt(members.get(_i).GetBufSize());
	members.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)5);
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

