package Common.TravelObj;

import java.nio.ByteBuffer;
/*********
 * 游历事件结果
 **/
public class Travel_EventResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件ID */
private long eventId;
/** 奖励物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** 额外数据 */
private byte[] ext;


public Travel_EventResult() {
	eventId = (long)0;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	ext = null;
}

public Travel_EventResult(
	 long _eventId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, byte[] _ext
) {	eventId = _eventId;
	itemList = _itemList;
	ext = _ext;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 事件ID */
public long getEventId() { return eventId; }
/** 事件ID */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 奖励物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 奖励物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
/** 额外数据 */
public byte[] getExt() { return ext; }
public java.nio.ByteBuffer get_buffer_Ext() { if(null == ext)return null; else return ByteBuffer.wrap(ext); }

/** 额外数据 */
public void setExt(byte[] _ext) { ext = _ext; }
public void setExt(java.nio.ByteBuffer _ext) 
{
	if(null == _ext){return;}
	int _oldPos = _ext.position();
	int _bufLength = _ext.remaining();
	ext = new byte[_bufLength];
	_ext.get(ext);
	_ext.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 4 + (ext == null ? 0 : ext.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 4 + (ext == null ? 0 : ext.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extCount = _buf.getInt();
	if(0 < _extCount){
		ext = new byte[_extCount];
		_buf.get(ext);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(eventId);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt((ext == null ? 0 : ext.length));
	if(null != ext){_buf.put(ext);}

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

