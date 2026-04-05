package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-消除逻辑
 **/
public class TileMatch_Remove implements ALBasicProtocolPack._IALProtocolStructure {
/** 触发方块索引 */
private int triggerBlockIndex;
/** 消除方块索引列表 */
private java.util.ArrayList<Integer> removeBlockIndexList;


public TileMatch_Remove() {
	triggerBlockIndex = 0;
	removeBlockIndexList = new java.util.ArrayList<Integer>();
}

public TileMatch_Remove(
	 int _triggerBlockIndex
	, java.util.ArrayList<Integer> _removeBlockIndexList
) {	triggerBlockIndex = _triggerBlockIndex;
	removeBlockIndexList = _removeBlockIndexList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 触发方块索引 */
public int getTriggerBlockIndex() { return triggerBlockIndex; }
/** 触发方块索引 */
public void setTriggerBlockIndex(int _triggerBlockIndex) { triggerBlockIndex = _triggerBlockIndex; }
/** 消除方块索引列表 */
public java.util.ArrayList<Integer> getRemoveBlockIndexList() { return removeBlockIndexList; }
/** 消除方块索引列表 */
public void addRemoveBlockIndexList(int _removeBlockIndexList) { removeBlockIndexList.add(_removeBlockIndexList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (removeBlockIndexList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (removeBlockIndexList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) triggerBlockIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _removeBlockIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _removeBlockIndexListCount; _i++) { 
		int _removeBlockIndexList = 0;
		if(_buf.remaining() > 0) _removeBlockIndexList = _buf.getInt();
		removeBlockIndexList.add(_removeBlockIndexList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(triggerBlockIndex);
	_buf.putShort((short)removeBlockIndexList.size());
	for(int _i = 0; _i < removeBlockIndexList.size(); _i++) { 
		_buf.putInt(removeBlockIndexList.get(_i));
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

