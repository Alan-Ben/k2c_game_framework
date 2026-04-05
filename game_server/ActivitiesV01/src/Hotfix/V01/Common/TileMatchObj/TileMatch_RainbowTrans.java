package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-彩虹转换逻辑
 **/
public class TileMatch_RainbowTrans implements ALBasicProtocolPack._IALProtocolStructure {
/** 触发方块索引 */
private int triggerBlockIndex;
/** 新方块id */
private int newBlockId;
/** 被转换方块索引列表 */
private java.util.ArrayList<Integer> beTransIndexList;


public TileMatch_RainbowTrans() {
	triggerBlockIndex = 0;
	newBlockId = 0;
	beTransIndexList = new java.util.ArrayList<Integer>();
}

public TileMatch_RainbowTrans(
	 int _triggerBlockIndex
	, int _newBlockId
	, java.util.ArrayList<Integer> _beTransIndexList
) {	triggerBlockIndex = _triggerBlockIndex;
	newBlockId = _newBlockId;
	beTransIndexList = _beTransIndexList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 触发方块索引 */
public int getTriggerBlockIndex() { return triggerBlockIndex; }
/** 触发方块索引 */
public void setTriggerBlockIndex(int _triggerBlockIndex) { triggerBlockIndex = _triggerBlockIndex; }
/** 新方块id */
public int getNewBlockId() { return newBlockId; }
/** 新方块id */
public void setNewBlockId(int _newBlockId) { newBlockId = _newBlockId; }
/** 被转换方块索引列表 */
public java.util.ArrayList<Integer> getBeTransIndexList() { return beTransIndexList; }
/** 被转换方块索引列表 */
public void addBeTransIndexList(int _beTransIndexList) { beTransIndexList.add(_beTransIndexList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (beTransIndexList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (beTransIndexList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) triggerBlockIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) newBlockId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _beTransIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _beTransIndexListCount; _i++) { 
		int _beTransIndexList = 0;
		if(_buf.remaining() > 0) _beTransIndexList = _buf.getInt();
		beTransIndexList.add(_beTransIndexList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(triggerBlockIndex);
	_buf.putInt(newBlockId);
	_buf.putShort((short)beTransIndexList.size());
	for(int _i = 0; _i < beTransIndexList.size(); _i++) { 
		_buf.putInt(beTransIndexList.get(_i));
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

