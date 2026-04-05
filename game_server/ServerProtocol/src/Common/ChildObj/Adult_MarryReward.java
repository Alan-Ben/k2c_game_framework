package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣数据奖励
 **/
public class Adult_MarryReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 已婚子嗣数据 */
private Common.ChildObj.Adult_MarriedInfo marriedInfo;
/** 奖励物品 */
private NPCommon.NPCommon_ItemInfo item;


public Adult_MarryReward() {
	marriedInfo = new Common.ChildObj.Adult_MarriedInfo();
	item = new NPCommon.NPCommon_ItemInfo();
}

public Adult_MarryReward(
	 Common.ChildObj.Adult_MarriedInfo _marriedInfo
	, NPCommon.NPCommon_ItemInfo _item
) {	marriedInfo = _marriedInfo;
	item = _item;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已婚子嗣数据 */
public Common.ChildObj.Adult_MarriedInfo getMarriedInfo() { return marriedInfo; }
/** 已婚子嗣数据 */
public void setMarriedInfo(Common.ChildObj.Adult_MarriedInfo _marriedInfo) { marriedInfo = _marriedInfo; }
/** 奖励物品 */
public NPCommon.NPCommon_ItemInfo getItem() { return item; }
/** 奖励物品 */
public void setItem(NPCommon.NPCommon_ItemInfo _item) { item = _item; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + marriedInfo.GetBufSize();
	_size += 4 + item.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + marriedInfo.GetBufSize();
	_size += 4 + item.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marriedInfoCustLen = _buf.getInt();
	int _marriedInfoCurPos = _buf.position();
	marriedInfo.ReadUnzipBuf(_buf, _marriedInfoCurPos + _marriedInfoCustLen);
	_buf.position(_marriedInfoCurPos + _marriedInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _itemCustLen = _buf.getInt();
	int _itemCurPos = _buf.position();
	item.ReadUnzipBuf(_buf, _itemCurPos + _itemCustLen);
	_buf.position(_itemCurPos + _itemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(marriedInfo.GetBufSize());
	marriedInfo.PutUnzipBuf(_buf);
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

