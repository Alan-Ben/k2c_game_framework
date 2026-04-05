package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 攻击属性据点响应
 **/
public class GS2GC_032_044_RetAttackPropertyPoint implements ALBasicProtocolPack._IALProtocolStructure {
/** 本次攻击血量 */
private long attackHp;
/** 获得物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;


public GS2GC_032_044_RetAttackPropertyPoint() {
	attackHp = (long)0;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_032_044_RetAttackPropertyPoint(
	 long _attackHp
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
) {	attackHp = _attackHp;
	itemList = _itemList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)44; }

/** 本次攻击血量 */
public long getAttackHp() { return attackHp; }
/** 本次攻击血量 */
public void setAttackHp(long _attackHp) { attackHp = _attackHp; }
/** 获得物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 获得物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(attackHp);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)44);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)44);
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

