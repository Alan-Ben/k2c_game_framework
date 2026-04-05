using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 初始化好友
/// </summary>
public class GS2GC_002_049_RetFriendInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 申请数据列表
/// </summary>
private List<Common.FriendObj.Friend_ApplyInfo> applyList;
/// <summary>
/// 好友数据列表
/// </summary>
private List<Common.FriendObj.Friend_Info> friendList;
/// <summary>
/// 分组数据列表
/// </summary>
private List<Common.FriendObj.Friend_CustomGroupInfo> groupList;
/// <summary>
/// 分组排序列表
/// </summary>
private List<long> groupOrderList;


public GS2GC_002_049_RetFriendInit() {
	applyList = new List<Common.FriendObj.Friend_ApplyInfo>();
	friendList = new List<Common.FriendObj.Friend_Info>();
	groupList = new List<Common.FriendObj.Friend_CustomGroupInfo>();
	groupOrderList = new List<long>();
}

public GS2GC_002_049_RetFriendInit(
	List<Common.FriendObj.Friend_ApplyInfo> _applyList
	, List<Common.FriendObj.Friend_Info> _friendList
	, List<Common.FriendObj.Friend_CustomGroupInfo> _groupList
	, List<long> _groupOrderList
) {	applyList = _applyList;
	friendList = _friendList;
	groupList = _groupList;
	groupOrderList = _groupOrderList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)49; }

/// <summary>
/// 申请数据列表
/// </summary>
public List<Common.FriendObj.Friend_ApplyInfo> getApplyList() { return applyList; }
/// <summary>
/// 申请数据列表
/// </summary>
public void addApplyList(Common.FriendObj.Friend_ApplyInfo _applyList) { applyList.Add(_applyList); }
/// <summary>
/// 好友数据列表
/// </summary>
public List<Common.FriendObj.Friend_Info> getFriendList() { return friendList; }
/// <summary>
/// 好友数据列表
/// </summary>
public void addFriendList(Common.FriendObj.Friend_Info _friendList) { friendList.Add(_friendList); }
/// <summary>
/// 分组数据列表
/// </summary>
public List<Common.FriendObj.Friend_CustomGroupInfo> getGroupList() { return groupList; }
/// <summary>
/// 分组数据列表
/// </summary>
public void addGroupList(Common.FriendObj.Friend_CustomGroupInfo _groupList) { groupList.Add(_groupList); }
/// <summary>
/// 分组排序列表
/// </summary>
public List<long> getGroupOrderList() { return groupOrderList; }
/// <summary>
/// 分组排序列表
/// </summary>
public void addGroupOrderList(long _groupOrderList) { groupOrderList.Add(_groupOrderList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (applyList.Count * 16);
	_size += 2 + (friendList.Count * 12);
	_size += 2;
for(int _i = 0; _i < groupList.Count; _i++) {
	_size += 4 + groupList[_i].GetBufSize();
	}

	_size += 2 + (groupOrderList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (applyList.Count * 16);
	_size += 2 + (friendList.Count * 12);
	_size += 2;
for(int _i = 0; _i < groupList.Count; _i++) {
	_size += 4 + groupList[_i].GetBufSize();
	}

	_size += 2 + (groupOrderList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _applyListCount = _buf.getShort();
	for(int _i = 0; _i < _applyListCount; _i++) { 
		Common.FriendObj.Friend_ApplyInfo _applyList = new Common.FriendObj.Friend_ApplyInfo();
		int __applyListCustLen = _buf.getInt();
	int __applyListCurPos = _buf.getCurPos();
	_applyList.ReadUnzipBuf(_buf, __applyListCurPos + __applyListCustLen);
	_buf.setPosition(__applyListCurPos + __applyListCustLen);

		applyList.Add(_applyList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _friendListCount = _buf.getShort();
	for(int _i = 0; _i < _friendListCount; _i++) { 
		Common.FriendObj.Friend_Info _friendList = new Common.FriendObj.Friend_Info();
		int __friendListCustLen = _buf.getInt();
	int __friendListCurPos = _buf.getCurPos();
	_friendList.ReadUnzipBuf(_buf, __friendListCurPos + __friendListCustLen);
	_buf.setPosition(__friendListCurPos + __friendListCustLen);

		friendList.Add(_friendList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _groupListCount = _buf.getShort();
	for(int _i = 0; _i < _groupListCount; _i++) { 
		Common.FriendObj.Friend_CustomGroupInfo _groupList = new Common.FriendObj.Friend_CustomGroupInfo();
		int __groupListCustLen = _buf.getInt();
	int __groupListCurPos = _buf.getCurPos();
	_groupList.ReadUnzipBuf(_buf, __groupListCurPos + __groupListCustLen);
	_buf.setPosition(__groupListCurPos + __groupListCustLen);

		groupList.Add(_groupList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _groupOrderListCount = _buf.getShort();
	for(int _i = 0; _i < _groupOrderListCount; _i++) { 
		long _groupOrderList = (long)0;
		_groupOrderList = _buf.getLong();
		groupOrderList.Add(_groupOrderList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)applyList.Count);
	for(int _i = 0; _i < applyList.Count; _i++) { 
		_buf.putInt(applyList[_i].GetBufSize());
	applyList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)friendList.Count);
	for(int _i = 0; _i < friendList.Count; _i++) { 
		_buf.putInt(friendList[_i].GetBufSize());
	friendList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)groupList.Count);
	for(int _i = 0; _i < groupList.Count; _i++) { 
		_buf.putInt(groupList[_i].GetBufSize());
	groupList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)groupOrderList.Count);
	for(int _i = 0; _i < groupOrderList.Count; _i++) { 
		_buf.putLong(groupOrderList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)49);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)49);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("applyList").Append(":").Append(applyList.ToString()).Append(", ");
	builder.Append("friendList").Append(":").Append(friendList.ToString()).Append(", ");
	builder.Append("groupList").Append(":").Append(groupList.ToString()).Append(", ");
	builder.Append("groupOrderList").Append(":").Append(groupOrderList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

