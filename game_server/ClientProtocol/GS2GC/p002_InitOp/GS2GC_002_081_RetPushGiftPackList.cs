using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 返回推送礼包列表
/// </summary>
public class GS2GC_002_081_RetPushGiftPackList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包组列表
/// </summary>
private List<Common.PushGiftObj.PushGift_GroupInfo> groupList;


public GS2GC_002_081_RetPushGiftPackList() {
	groupList = new List<Common.PushGiftObj.PushGift_GroupInfo>();
}

public GS2GC_002_081_RetPushGiftPackList(
	List<Common.PushGiftObj.PushGift_GroupInfo> _groupList
) {	groupList = _groupList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)81; }

/// <summary>
/// 礼包组列表
/// </summary>
public List<Common.PushGiftObj.PushGift_GroupInfo> getGroupList() { return groupList; }
/// <summary>
/// 礼包组列表
/// </summary>
public void addGroupList(Common.PushGiftObj.PushGift_GroupInfo _groupList) { groupList.Add(_groupList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (groupList.Count * 42);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (groupList.Count * 42);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _groupListCount = _buf.getShort();
	for(int _i = 0; _i < _groupListCount; _i++) { 
		Common.PushGiftObj.PushGift_GroupInfo _groupList = new Common.PushGiftObj.PushGift_GroupInfo();
		int __groupListCustLen = _buf.getInt();
	int __groupListCurPos = _buf.getCurPos();
	_groupList.ReadUnzipBuf(_buf, __groupListCurPos + __groupListCustLen);
	_buf.setPosition(__groupListCurPos + __groupListCustLen);

		groupList.Add(_groupList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)groupList.Count);
	for(int _i = 0; _i < groupList.Count; _i++) { 
		_buf.putInt(groupList[_i].GetBufSize());
	groupList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)81);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)81);
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
	builder.Append("groupList").Append(":").Append(groupList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

