using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_UniformPack : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.WCGGS2GC_UniformItem> items;


public WCGGS2GC_UniformPack() {
	items = new List<Common.WCGGS2GC_UniformItem>();
}

public WCGGS2GC_UniformPack(
	List<Common.WCGGS2GC_UniformItem> _items
) {	items = _items;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public List<Common.WCGGS2GC_UniformItem> getItems() { return items; }
public void addItems(Common.WCGGS2GC_UniformItem _items) { items.Add(_items); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (items.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (items.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemsCount = _buf.getShort();
	for(int _i = 0; _i < _itemsCount; _i++) { 
		Common.WCGGS2GC_UniformItem _items = new Common.WCGGS2GC_UniformItem();
		int __itemsCustLen = _buf.getInt();
	int __itemsCurPos = _buf.getCurPos();
	_items.ReadUnzipBuf(_buf, __itemsCurPos + __itemsCustLen);
	_buf.setPosition(__itemsCurPos + __itemsCustLen);

		items.Add(_items);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)items.Count);
	for(int _i = 0; _i < items.Count; _i++) { 
		_buf.putInt(items[_i].GetBufSize());
	items[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("items").Append(":").Append(items.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

