package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人经营技能属性汇总
 **/
public class Consort_BusinessSkillPropertySum implements ALBasicProtocolPack._IALProtocolStructure {
/** 相性 */
private CommonEnum.ESpecAttrType attr;
/** 概率加成 */
private long proAddSum;


public Consort_BusinessSkillPropertySum() {
	attr = CommonEnum.ESpecAttrType.values()[0];
	proAddSum = (long)0;
}

public Consort_BusinessSkillPropertySum(
	 CommonEnum.ESpecAttrType _attr
	, long _proAddSum
) {	attr = _attr;
	proAddSum = _proAddSum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 相性 */
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/** 相性 */
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/** 概率加成 */
public long getProAddSum() { return proAddSum; }
/** 概率加成 */
public void setProAddSum(long _proAddSum) { proAddSum = _proAddSum; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attr = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) proAddSum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(attr.ordinal());

	_buf.putLong(proAddSum);
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

