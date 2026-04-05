package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟宝箱奖励数据
 **/
public class Guild_BoxReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱实例ID */
private long id;
/** 物品 */
private NPCommon.NPCommon_ItemInfo item;


public Guild_BoxReward() {
	id = (long)0;
	item = new NPCommon.NPCommon_ItemInfo();
}

public Guild_BoxReward(
	 long _id
	, NPCommon.NPCommon_ItemInfo _item
) {	id = _id;
	item = _item;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宝箱实例ID */
public long getId() { return id; }
/** 宝箱实例ID */
public void setId(long _id) { id = _id; }
/** 物品 */
public NPCommon.NPCommon_ItemInfo getItem() { return item; }
/** 物品 */
public void setItem(NPCommon.NPCommon_ItemInfo _item) { item = _item; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + item.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + item.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.position();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.position(_itemCurPos + _itemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
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

