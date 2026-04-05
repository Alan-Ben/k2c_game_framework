package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_017_007_RetActivityRankBaseSubInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础子数据列表 */
private java.util.ArrayList<Common.RankObj.Rank_BaseSubItem> subList;


public GS2GC_017_007_RetActivityRankBaseSubInfoList() {
	subList = new java.util.ArrayList<Common.RankObj.Rank_BaseSubItem>();
}

public GS2GC_017_007_RetActivityRankBaseSubInfoList(
	 java.util.ArrayList<Common.RankObj.Rank_BaseSubItem> _subList
) {	subList = _subList;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)7; }

/** 基础子数据列表 */
public java.util.ArrayList<Common.RankObj.Rank_BaseSubItem> getSubList() { return subList; }
/** 基础子数据列表 */
public void addSubList(Common.RankObj.Rank_BaseSubItem _subList) { subList.add(_subList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (subList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (subList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _subListCount = _buf.getShort();
	for(int _i = 0; _i < _subListCount; _i++) { 
		Common.RankObj.Rank_BaseSubItem _subList = new Common.RankObj.Rank_BaseSubItem();
		if(_buf.remaining() <= 0) return;
	int __subListCustLen = _buf.getInt();
	int __subListCurPos = _buf.position();
	_subList.ReadUnzipBuf(_buf, __subListCurPos + __subListCustLen);
	_buf.position(__subListCurPos + __subListCustLen);

		subList.add(_subList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)subList.size());
	for(int _i = 0; _i < subList.size(); _i++) { 
		_buf.putInt(subList.get(_i).GetBufSize());
	subList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)7);
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

