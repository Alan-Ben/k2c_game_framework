package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作属性据点信息
 **/
public class GuildCooperate_PropertyPointInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 属性据点索引，从0开始 */
private int index;
/** 所属属性 */
private CommonEnum.ESpecAttrType attr;
/** 已攻击血量 */
private long hadAttackHp;
/** 总血量 */
private long totalHp;


public GuildCooperate_PropertyPointInfo() {
	index = 0;
	attr = CommonEnum.ESpecAttrType.values()[0];
	hadAttackHp = (long)0;
	totalHp = (long)0;
}

public GuildCooperate_PropertyPointInfo(
	 int _index
	, CommonEnum.ESpecAttrType _attr
	, long _hadAttackHp
	, long _totalHp
) {	index = _index;
	attr = _attr;
	hadAttackHp = _hadAttackHp;
	totalHp = _totalHp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 属性据点索引，从0开始 */
public int getIndex() { return index; }
/** 属性据点索引，从0开始 */
public void setIndex(int _index) { index = _index; }
/** 所属属性 */
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/** 所属属性 */
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/** 已攻击血量 */
public long getHadAttackHp() { return hadAttackHp; }
/** 已攻击血量 */
public void setHadAttackHp(long _hadAttackHp) { hadAttackHp = _hadAttackHp; }
/** 总血量 */
public long getTotalHp() { return totalHp; }
/** 总血量 */
public void setTotalHp(long _totalHp) { totalHp = _totalHp; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attr = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadAttackHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalHp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
	_buf.putInt(attr.ordinal());

	_buf.putLong(hadAttackHp);
	_buf.putLong(totalHp);
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

