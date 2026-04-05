package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_017_003_RetActivityRankBaseList implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础数据列表 */
private java.util.ArrayList<Common.RankObj.Rank_BaseItem> baseItemlist;


public GS2GC_017_003_RetActivityRankBaseList() {
	baseItemlist = new java.util.ArrayList<Common.RankObj.Rank_BaseItem>();
}

public GS2GC_017_003_RetActivityRankBaseList(
	 java.util.ArrayList<Common.RankObj.Rank_BaseItem> _baseItemlist
) {	baseItemlist = _baseItemlist;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)3; }

/** 基础数据列表 */
public java.util.ArrayList<Common.RankObj.Rank_BaseItem> getBaseItemlist() { return baseItemlist; }
/** 基础数据列表 */
public void addBaseItemlist(Common.RankObj.Rank_BaseItem _baseItemlist) { baseItemlist.add(_baseItemlist); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (baseItemlist.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (baseItemlist.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _baseItemlistCount = _buf.getShort();
	for(int _i = 0; _i < _baseItemlistCount; _i++) { 
		Common.RankObj.Rank_BaseItem _baseItemlist = new Common.RankObj.Rank_BaseItem();
		if(_buf.remaining() <= 0) return;
	int __baseItemlistCustLen = _buf.getInt();
	int __baseItemlistCurPos = _buf.position();
	_baseItemlist.ReadUnzipBuf(_buf, __baseItemlistCurPos + __baseItemlistCustLen);
	_buf.position(__baseItemlistCurPos + __baseItemlistCustLen);

		baseItemlist.add(_baseItemlist);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)baseItemlist.size());
	for(int _i = 0; _i < baseItemlist.size(); _i++) { 
		_buf.putInt(baseItemlist.get(_i).GetBufSize());
	baseItemlist.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)3);
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

