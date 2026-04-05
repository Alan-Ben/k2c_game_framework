package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_UniformPack implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.WCGGS2GC_UniformItem> items;


public WCGGS2GC_UniformPack() {
	items = new java.util.ArrayList<Common.WCGGS2GC_UniformItem>();
}

public WCGGS2GC_UniformPack(
	 java.util.ArrayList<Common.WCGGS2GC_UniformItem> _items
) {	items = _items;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.WCGGS2GC_UniformItem> getItems() { return items; }
public void addItems(Common.WCGGS2GC_UniformItem _items) { items.add(_items); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (items.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (items.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemsCount = _buf.getShort();
	for(int _i = 0; _i < _itemsCount; _i++) { 
		Common.WCGGS2GC_UniformItem _items = new Common.WCGGS2GC_UniformItem();
		if(_buf.remaining() <= 0) return;
	int __itemsCustLen = _buf.getInt();
	int __itemsCurPos = _buf.position();
	_items.ReadUnzipBuf(_buf, __itemsCurPos + __itemsCustLen);
	_buf.position(__itemsCurPos + __itemsCustLen);

		items.add(_items);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)items.size());
	for(int _i = 0; _i < items.size(); _i++) { 
		_buf.putInt(items.get(_i).GetBufSize());
	items.get(_i).PutUnzipBuf(_buf);
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

