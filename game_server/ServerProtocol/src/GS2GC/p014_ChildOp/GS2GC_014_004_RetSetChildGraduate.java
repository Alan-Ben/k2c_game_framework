package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 设置子嗣（未成年）毕业
 **/
public class GS2GC_014_004_RetSetChildGraduate implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItemList;
/** 空 */
private Common.ChildObj.Adult_UnmarriedInfo adult;


public GS2GC_014_004_RetSetChildGraduate() {
	gainItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	adult = new Common.ChildObj.Adult_UnmarriedInfo();
}

public GS2GC_014_004_RetSetChildGraduate(
	 java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItemList
	, Common.ChildObj.Adult_UnmarriedInfo _adult
) {	gainItemList = _gainItemList;
	adult = _adult;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)4; }

/** 奖励物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
/** 奖励物品列表 */
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.add(_gainItemList); }
/** 空 */
public Common.ChildObj.Adult_UnmarriedInfo getAdult() { return adult; }
/** 空 */
public void setAdult(Common.ChildObj.Adult_UnmarriedInfo _adult) { adult = _adult; }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}

	_size += 4 + adult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}

	_size += 4 + adult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainItemListCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __gainItemListCustLen = _buf.getInt();
	int __gainItemListCurPos = _buf.position();
	_gainItemList.ReadUnzipBuf(_buf, __gainItemListCurPos + __gainItemListCustLen);
	_buf.position(__gainItemListCurPos + __gainItemListCustLen);

		gainItemList.add(_gainItemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.position();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.position(_adultCurPos + _adultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)gainItemList.size());
	for(int _i = 0; _i < gainItemList.size(); _i++) { 
		_buf.putInt(gainItemList.get(_i).GetBufSize());
	gainItemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)4);
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

