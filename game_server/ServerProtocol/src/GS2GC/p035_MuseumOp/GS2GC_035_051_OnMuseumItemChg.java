package GS2GC.p035_MuseumOp;

import java.nio.ByteBuffer;
/*********
 * 博物馆物品新增推送
 **/
public class GS2GC_035_051_OnMuseumItemChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 物品信息 */
private Common.MuseumObj.Museum_ItemInfo itemInfo;


public GS2GC_035_051_OnMuseumItemChg() {
	itemInfo = new Common.MuseumObj.Museum_ItemInfo();
}

public GS2GC_035_051_OnMuseumItemChg(
	 Common.MuseumObj.Museum_ItemInfo _itemInfo
) {	itemInfo = _itemInfo;
}

public final byte getMainOrder() { return (byte)35; }

public final byte getSubOrder() { return (byte)51; }

/** 物品信息 */
public Common.MuseumObj.Museum_ItemInfo getItemInfo() { return itemInfo; }
/** 物品信息 */
public void setItemInfo(Common.MuseumObj.Museum_ItemInfo _itemInfo) { itemInfo = _itemInfo; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _itemInfoCustLen = _buf.getInt();
	int _itemInfoCurPos = _buf.position();
	itemInfo.ReadUnzipBuf(_buf, _itemInfoCurPos + _itemInfoCustLen);
	_buf.position(_itemInfoCurPos + _itemInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(itemInfo.GetBufSize());
	itemInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)35);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)35);
	_recBuf.put((byte)51);
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

