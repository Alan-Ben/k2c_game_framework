package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 初始化好友
 **/
public class GS2GC_002_049_RetFriendInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请数据列表 */
private java.util.ArrayList<Common.FriendObj.Friend_ApplyInfo> applyList;
/** 好友数据列表 */
private java.util.ArrayList<Common.FriendObj.Friend_Info> friendList;
/** 分组数据列表 */
private java.util.ArrayList<Common.FriendObj.Friend_CustomGroupInfo> groupList;
/** 分组排序列表 */
private java.util.ArrayList<Long> groupOrderList;


public GS2GC_002_049_RetFriendInit() {
	applyList = new java.util.ArrayList<Common.FriendObj.Friend_ApplyInfo>();
	friendList = new java.util.ArrayList<Common.FriendObj.Friend_Info>();
	groupList = new java.util.ArrayList<Common.FriendObj.Friend_CustomGroupInfo>();
	groupOrderList = new java.util.ArrayList<Long>();
}

public GS2GC_002_049_RetFriendInit(
	 java.util.ArrayList<Common.FriendObj.Friend_ApplyInfo> _applyList
	, java.util.ArrayList<Common.FriendObj.Friend_Info> _friendList
	, java.util.ArrayList<Common.FriendObj.Friend_CustomGroupInfo> _groupList
	, java.util.ArrayList<Long> _groupOrderList
) {	applyList = _applyList;
	friendList = _friendList;
	groupList = _groupList;
	groupOrderList = _groupOrderList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)49; }

/** 申请数据列表 */
public java.util.ArrayList<Common.FriendObj.Friend_ApplyInfo> getApplyList() { return applyList; }
/** 申请数据列表 */
public void addApplyList(Common.FriendObj.Friend_ApplyInfo _applyList) { applyList.add(_applyList); }
/** 好友数据列表 */
public java.util.ArrayList<Common.FriendObj.Friend_Info> getFriendList() { return friendList; }
/** 好友数据列表 */
public void addFriendList(Common.FriendObj.Friend_Info _friendList) { friendList.add(_friendList); }
/** 分组数据列表 */
public java.util.ArrayList<Common.FriendObj.Friend_CustomGroupInfo> getGroupList() { return groupList; }
/** 分组数据列表 */
public void addGroupList(Common.FriendObj.Friend_CustomGroupInfo _groupList) { groupList.add(_groupList); }
/** 分组排序列表 */
public java.util.ArrayList<Long> getGroupOrderList() { return groupOrderList; }
/** 分组排序列表 */
public void addGroupOrderList(long _groupOrderList) { groupOrderList.add(_groupOrderList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (applyList.size() * 16);
	_size += 2 + (friendList.size() * 12);
	_size += 2;
	for(int _i = 0; _i < groupList.size(); _i++) {
	_size += 4 + groupList.get(_i).GetBufSize();
	}

	_size += 2 + (groupOrderList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (applyList.size() * 16);
	_size += 2 + (friendList.size() * 12);
	_size += 2;
	for(int _i = 0; _i < groupList.size(); _i++) {
	_size += 4 + groupList.get(_i).GetBufSize();
	}

	_size += 2 + (groupOrderList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _applyListCount = _buf.getShort();
	for(int _i = 0; _i < _applyListCount; _i++) { 
		Common.FriendObj.Friend_ApplyInfo _applyList = new Common.FriendObj.Friend_ApplyInfo();
		if(_buf.remaining() <= 0) return;
	int __applyListCustLen = _buf.getInt();
	int __applyListCurPos = _buf.position();
	_applyList.ReadUnzipBuf(_buf, __applyListCurPos + __applyListCustLen);
	_buf.position(__applyListCurPos + __applyListCustLen);

		applyList.add(_applyList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _friendListCount = _buf.getShort();
	for(int _i = 0; _i < _friendListCount; _i++) { 
		Common.FriendObj.Friend_Info _friendList = new Common.FriendObj.Friend_Info();
		if(_buf.remaining() <= 0) return;
	int __friendListCustLen = _buf.getInt();
	int __friendListCurPos = _buf.position();
	_friendList.ReadUnzipBuf(_buf, __friendListCurPos + __friendListCustLen);
	_buf.position(__friendListCurPos + __friendListCustLen);

		friendList.add(_friendList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupListCount = _buf.getShort();
	for(int _i = 0; _i < _groupListCount; _i++) { 
		Common.FriendObj.Friend_CustomGroupInfo _groupList = new Common.FriendObj.Friend_CustomGroupInfo();
		if(_buf.remaining() <= 0) return;
	int __groupListCustLen = _buf.getInt();
	int __groupListCurPos = _buf.position();
	_groupList.ReadUnzipBuf(_buf, __groupListCurPos + __groupListCustLen);
	_buf.position(__groupListCurPos + __groupListCustLen);

		groupList.add(_groupList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupOrderListCount = _buf.getShort();
	for(int _i = 0; _i < _groupOrderListCount; _i++) { 
		long _groupOrderList = (long)0;
		if(_buf.remaining() > 0) _groupOrderList = _buf.getLong();
		groupOrderList.add(_groupOrderList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)applyList.size());
	for(int _i = 0; _i < applyList.size(); _i++) { 
		_buf.putInt(applyList.get(_i).GetBufSize());
	applyList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)friendList.size());
	for(int _i = 0; _i < friendList.size(); _i++) { 
		_buf.putInt(friendList.get(_i).GetBufSize());
	friendList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)groupList.size());
	for(int _i = 0; _i < groupList.size(); _i++) { 
		_buf.putInt(groupList.get(_i).GetBufSize());
	groupList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)groupOrderList.size());
	for(int _i = 0; _i < groupOrderList.size(); _i++) { 
		_buf.putLong(groupOrderList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)49);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)49);
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

