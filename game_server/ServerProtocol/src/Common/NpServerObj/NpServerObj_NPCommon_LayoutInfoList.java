package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 战斗服务端存储用阵容数据
 **/
public class NpServerObj_NPCommon_LayoutInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 使用阵型id */
private long layoutRefId;
/** 阵型数据 */
private java.util.ArrayList<NPCommon.NPCommon_RetLayoutInfo> layoutInfoList;


public NpServerObj_NPCommon_LayoutInfoList() {
	layoutRefId = (long)0;
	layoutInfoList = new java.util.ArrayList<NPCommon.NPCommon_RetLayoutInfo>();
}

public NpServerObj_NPCommon_LayoutInfoList(
	 long _layoutRefId
	, java.util.ArrayList<NPCommon.NPCommon_RetLayoutInfo> _layoutInfoList
) {	layoutRefId = _layoutRefId;
	layoutInfoList = _layoutInfoList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 使用阵型id */
public long getLayoutRefId() { return layoutRefId; }
/** 使用阵型id */
public void setLayoutRefId(long _layoutRefId) { layoutRefId = _layoutRefId; }
/** 阵型数据 */
public java.util.ArrayList<NPCommon.NPCommon_RetLayoutInfo> getLayoutInfoList() { return layoutInfoList; }
/** 阵型数据 */
public void addLayoutInfoList(NPCommon.NPCommon_RetLayoutInfo _layoutInfoList) { layoutInfoList.add(_layoutInfoList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (layoutInfoList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (layoutInfoList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) layoutRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _layoutInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _layoutInfoListCount; _i++) { 
		NPCommon.NPCommon_RetLayoutInfo _layoutInfoList = new NPCommon.NPCommon_RetLayoutInfo();
		if(_buf.remaining() <= 0) return;
	int __layoutInfoListCustLen = _buf.getInt();
	int __layoutInfoListCurPos = _buf.position();
	_layoutInfoList.ReadUnzipBuf(_buf, __layoutInfoListCurPos + __layoutInfoListCustLen);
	_buf.position(__layoutInfoListCurPos + __layoutInfoListCustLen);

		layoutInfoList.add(_layoutInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(layoutRefId);
	_buf.putShort((short)layoutInfoList.size());
	for(int _i = 0; _i < layoutInfoList.size(); _i++) { 
		_buf.putInt(layoutInfoList.get(_i).GetBufSize());
	layoutInfoList.get(_i).PutUnzipBuf(_buf);
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

