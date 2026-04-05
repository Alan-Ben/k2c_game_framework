package ALLRPC.MarryMatchServer.MarryMatch;

import java.nio.ByteBuffer;
public class MmsUploadMatchItemList_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组ID */
private long groupId;
/** 请求数据列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo> itemList;


public MmsUploadMatchItemList_Req() {
	groupId = (long)0;
	itemList = new java.util.ArrayList<Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo>();
}

public MmsUploadMatchItemList_Req(
	 long _groupId
	, java.util.ArrayList<Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo> _itemList
) {	groupId = _groupId;
	itemList = _itemList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组ID */
public long getGroupId() { return groupId; }
/** 分组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 请求数据列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo> getItemList() { return itemList; }
/** 请求数据列表 */
public void addItemList(Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo _itemList) { itemList.add(_itemList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (itemList.size() * 40);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (itemList.size() * 40);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo _itemList = new Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

