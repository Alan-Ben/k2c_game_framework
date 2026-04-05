package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 返回推送礼包列表
 **/
public class GS2GC_002_081_RetPushGiftPackList implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包组列表 */
private java.util.ArrayList<Common.PushGiftObj.PushGift_GroupInfo> groupList;


public GS2GC_002_081_RetPushGiftPackList() {
	groupList = new java.util.ArrayList<Common.PushGiftObj.PushGift_GroupInfo>();
}

public GS2GC_002_081_RetPushGiftPackList(
	 java.util.ArrayList<Common.PushGiftObj.PushGift_GroupInfo> _groupList
) {	groupList = _groupList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)81; }

/** 礼包组列表 */
public java.util.ArrayList<Common.PushGiftObj.PushGift_GroupInfo> getGroupList() { return groupList; }
/** 礼包组列表 */
public void addGroupList(Common.PushGiftObj.PushGift_GroupInfo _groupList) { groupList.add(_groupList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (groupList.size() * 42);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (groupList.size() * 42);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupListCount = _buf.getShort();
	for(int _i = 0; _i < _groupListCount; _i++) { 
		Common.PushGiftObj.PushGift_GroupInfo _groupList = new Common.PushGiftObj.PushGift_GroupInfo();
		if(_buf.remaining() <= 0) return;
	int __groupListCustLen = _buf.getInt();
	int __groupListCurPos = _buf.position();
	_groupList.ReadUnzipBuf(_buf, __groupListCurPos + __groupListCustLen);
	_buf.position(__groupListCurPos + __groupListCustLen);

		groupList.add(_groupList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)groupList.size());
	for(int _i = 0; _i < groupList.size(); _i++) { 
		_buf.putInt(groupList.get(_i).GetBufSize());
	groupList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)81);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)81);
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

