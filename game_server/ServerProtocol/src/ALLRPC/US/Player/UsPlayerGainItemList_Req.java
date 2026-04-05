package ALLRPC.US.Player;

import java.nio.ByteBuffer;
public class UsPlayerGainItemList_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/** 上下文信息 */
private Common.Common_Context context;
private NPCommon.NPCommon_ItemList itemList;
/** 是否弹窗提示 */
private boolean isPush;


public UsPlayerGainItemList_Req() {
	cid = (long)0;
	context = new Common.Common_Context();
	itemList = new NPCommon.NPCommon_ItemList();
	isPush = false;
}

public UsPlayerGainItemList_Req(
	 long _cid
	, Common.Common_Context _context
	, NPCommon.NPCommon_ItemList _itemList
	, boolean _isPush
) {	cid = _cid;
	context = _context;
	itemList = _itemList;
	isPush = _isPush;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 上下文信息 */
public Common.Common_Context getContext() { return context; }
/** 上下文信息 */
public void setContext(Common.Common_Context _context) { context = _context; }
public NPCommon.NPCommon_ItemList getItemList() { return itemList; }
public void setItemList(NPCommon.NPCommon_ItemList _itemList) { itemList = _itemList; }
/** 是否弹窗提示 */
public boolean getIsPush() { return isPush; }
/** 是否弹窗提示 */
public void setIsPush(boolean _isPush) { isPush = _isPush; }


public final int GetBufSize() {
	int _size = 25;
	_size += 4 + itemList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += 4 + itemList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _contextCustLen = _buf.getInt();
	int _contextCurPos = _buf.position();
	context.ReadUnzipBuf(_buf, _contextCurPos + _contextCustLen);
	_buf.position(_contextCurPos + _contextCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _itemListCustLen = _buf.getInt();
	int _itemListCurPos = _buf.position();
	itemList.ReadUnzipBuf(_buf, _itemListCurPos + _itemListCustLen);
	_buf.position(_itemListCurPos + _itemListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isPush = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(context.GetBufSize());
	context.PutUnzipBuf(_buf);
	_buf.putInt(itemList.GetBufSize());
	itemList.PutUnzipBuf(_buf);
	_buf.put(isPush?(byte)1:(byte)0);
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

