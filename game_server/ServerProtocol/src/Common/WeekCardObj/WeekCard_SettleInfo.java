package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-详细结算列表
 **/
public class WeekCard_SettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 详细信息列表 */
private java.util.ArrayList<Common.WeekCardObj.WeekCard_SettleDetailInfo> detailList;
/** 事件奖励物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;


public WeekCard_SettleInfo() {
	detailList = new java.util.ArrayList<Common.WeekCardObj.WeekCard_SettleDetailInfo>();
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public WeekCard_SettleInfo(
	 java.util.ArrayList<Common.WeekCardObj.WeekCard_SettleDetailInfo> _detailList
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
) {	detailList = _detailList;
	itemList = _itemList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 详细信息列表 */
public java.util.ArrayList<Common.WeekCardObj.WeekCard_SettleDetailInfo> getDetailList() { return detailList; }
/** 详细信息列表 */
public void addDetailList(Common.WeekCardObj.WeekCard_SettleDetailInfo _detailList) { detailList.add(_detailList); }
/** 事件奖励物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 事件奖励物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < detailList.size(); _i++) {
	_size += 4 + detailList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < detailList.size(); _i++) {
	_size += 4 + detailList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _detailListCount = _buf.getShort();
	for(int _i = 0; _i < _detailListCount; _i++) { 
		Common.WeekCardObj.WeekCard_SettleDetailInfo _detailList = new Common.WeekCardObj.WeekCard_SettleDetailInfo();
		if(_buf.remaining() <= 0) return;
	int __detailListCustLen = _buf.getInt();
	int __detailListCurPos = _buf.position();
	_detailList.ReadUnzipBuf(_buf, __detailListCurPos + __detailListCustLen);
	_buf.position(__detailListCurPos + __detailListCustLen);

		detailList.add(_detailList);
	}
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
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)detailList.size());
	for(int _i = 0; _i < detailList.size(); _i++) { 
		_buf.putInt(detailList.get(_i).GetBufSize());
	detailList.get(_i).PutUnzipBuf(_buf);
	}
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

