using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p006_BagItemOp
{

public class GC2GS_006_002_ReqBagUseItem : ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long count;
private List<int> selectedIdx;


public GC2GS_006_002_ReqBagUseItem() {
	itemId = (long)0;
	count = (long)0;
	selectedIdx = new List<int>();
}

public GC2GS_006_002_ReqBagUseItem(
	long _itemId
	, long _count
	, List<int> _selectedIdx
) {	itemId = _itemId;
	count = _count;
	selectedIdx = _selectedIdx;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)2; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
public List<int> getSelectedIdx() { return selectedIdx; }
public void addSelectedIdx(int _selectedIdx) { selectedIdx.Add(_selectedIdx); }


public int GetBufSize() {
	int _size = 16;
	_size += 2 + (selectedIdx.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (selectedIdx.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _selectedIdxCount = _buf.getShort();
	for(int _i = 0; _i < _selectedIdxCount; _i++) { 
		int _selectedIdx = 0;
		_selectedIdx = _buf.getInt();
		selectedIdx.Add(_selectedIdx);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.putLong(count);
	_buf.putShort((short)selectedIdx.Count);
	for(int _i = 0; _i < selectedIdx.Count; _i++) { 
		_buf.putInt(selectedIdx[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)2);
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
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("selectedIdx").Append(":").Append(selectedIdx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

