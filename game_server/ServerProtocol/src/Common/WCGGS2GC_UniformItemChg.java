package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_UniformItemChg implements ALBasicProtocolPack._IALProtocolStructure {
private long uniformId;
private long count;
private long srcCount;
private java.util.ArrayList<Common.WCGGS2GC_UniformItemFix> fixList;


public WCGGS2GC_UniformItemChg() {
	uniformId = (long)0;
	count = (long)0;
	srcCount = (long)0;
	fixList = new java.util.ArrayList<Common.WCGGS2GC_UniformItemFix>();
}

public WCGGS2GC_UniformItemChg(
	 long _uniformId
	, long _count
	, long _srcCount
	, java.util.ArrayList<Common.WCGGS2GC_UniformItemFix> _fixList
) {	uniformId = _uniformId;
	count = _count;
	srcCount = _srcCount;
	fixList = _fixList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUniformId() { return uniformId; }
public void setUniformId(long _uniformId) { uniformId = _uniformId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
public long getSrcCount() { return srcCount; }
public void setSrcCount(long _srcCount) { srcCount = _srcCount; }
public java.util.ArrayList<Common.WCGGS2GC_UniformItemFix> getFixList() { return fixList; }
public void addFixList(Common.WCGGS2GC_UniformItemFix _fixList) { fixList.add(_fixList); }


public final int GetBufSize() {
	int _size = 24;
	_size += 2 + (fixList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (fixList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uniformId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) srcCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _fixListCount = _buf.getShort();
	for(int _i = 0; _i < _fixListCount; _i++) { 
		Common.WCGGS2GC_UniformItemFix _fixList = new Common.WCGGS2GC_UniformItemFix();
		if(_buf.remaining() <= 0) return;
	int __fixListCustLen = _buf.getInt();
	int __fixListCurPos = _buf.position();
	_fixList.ReadUnzipBuf(_buf, __fixListCurPos + __fixListCustLen);
	_buf.position(__fixListCurPos + __fixListCustLen);

		fixList.add(_fixList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uniformId);
	_buf.putLong(count);
	_buf.putLong(srcCount);
	_buf.putShort((short)fixList.size());
	for(int _i = 0; _i < fixList.size(); _i++) { 
		_buf.putInt(fixList.get(_i).GetBufSize());
	fixList.get(_i).PutUnzipBuf(_buf);
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

