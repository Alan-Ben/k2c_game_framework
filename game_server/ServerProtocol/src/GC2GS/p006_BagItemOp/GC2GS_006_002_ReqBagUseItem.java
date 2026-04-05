package GC2GS.p006_BagItemOp;

import java.nio.ByteBuffer;
public class GC2GS_006_002_ReqBagUseItem implements ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long count;
private java.util.ArrayList<Integer> selectedIdx;


public GC2GS_006_002_ReqBagUseItem() {
	itemId = (long)0;
	count = (long)0;
	selectedIdx = new java.util.ArrayList<Integer>();
}

public GC2GS_006_002_ReqBagUseItem(
	 long _itemId
	, long _count
	, java.util.ArrayList<Integer> _selectedIdx
) {	itemId = _itemId;
	count = _count;
	selectedIdx = _selectedIdx;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)2; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
public java.util.ArrayList<Integer> getSelectedIdx() { return selectedIdx; }
public void addSelectedIdx(int _selectedIdx) { selectedIdx.add(_selectedIdx); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (selectedIdx.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (selectedIdx.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _selectedIdxCount = _buf.getShort();
	for(int _i = 0; _i < _selectedIdxCount; _i++) { 
		int _selectedIdx = 0;
		if(_buf.remaining() > 0) _selectedIdx = _buf.getInt();
		selectedIdx.add(_selectedIdx);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
	_buf.putLong(count);
	_buf.putShort((short)selectedIdx.size());
	for(int _i = 0; _i < selectedIdx.size(); _i++) { 
		_buf.putInt(selectedIdx.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)2);
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

