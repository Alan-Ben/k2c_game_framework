package ALLRPC.MarryMatchServer.MarryMatch;

import java.nio.ByteBuffer;
public class MmsAddMatchItem_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组ID */
private long groupId;
/** 请求数据 */
private Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo item;


public MmsAddMatchItem_Req() {
	groupId = (long)0;
	item = new Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo();
}

public MmsAddMatchItem_Req(
	 long _groupId
	, Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo _item
) {	groupId = _groupId;
	item = _item;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组ID */
public long getGroupId() { return groupId; }
/** 分组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 请求数据 */
public Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo getItem() { return item; }
/** 请求数据 */
public void setItem(Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo _item) { item = _item; }


public final int GetBufSize() {
	int _size = 48;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.position();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.position(_itemCurPos + _itemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putInt(item.GetBufSize());
	item.PutUnzipBuf(_buf);
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

