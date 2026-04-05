package Common.LevyObj;

import java.nio.ByteBuffer;
/*********
 * 征收信息
 **/
public class Levy_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 征收类型 */
private Common.LevyEnum.ELevy_Type levyType;
private byte[] levyInfo;
/** 征收的数量累计总和 */
private long levySum;


public Levy_Info() {
	levyType = Common.LevyEnum.ELevy_Type.values()[0];
	levyInfo = null;
	levySum = (long)0;
}

public Levy_Info(
	 Common.LevyEnum.ELevy_Type _levyType
	, byte[] _levyInfo
	, long _levySum
) {	levyType = _levyType;
	levyInfo = _levyInfo;
	levySum = _levySum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 征收类型 */
public Common.LevyEnum.ELevy_Type getLevyType() { return levyType; }
/** 征收类型 */
public void setLevyType(Common.LevyEnum.ELevy_Type _levyType) { levyType = _levyType; }
public byte[] getLevyInfo() { return levyInfo; }
public java.nio.ByteBuffer get_buffer_LevyInfo() { if(null == levyInfo)return null; else return ByteBuffer.wrap(levyInfo); }

public void setLevyInfo(byte[] _levyInfo) { levyInfo = _levyInfo; }
public void setLevyInfo(java.nio.ByteBuffer _levyInfo) 
{
	if(null == _levyInfo){return;}
	int _oldPos = _levyInfo.position();
	int _bufLength = _levyInfo.remaining();
	levyInfo = new byte[_bufLength];
	_levyInfo.get(levyInfo);
	_levyInfo.position(_oldPos);
}

/** 征收的数量累计总和 */
public long getLevySum() { return levySum; }
/** 征收的数量累计总和 */
public void setLevySum(long _levySum) { levySum = _levySum; }


public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (levyInfo == null ? 0 : levyInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (levyInfo == null ? 0 : levyInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) levyType = Common.LevyEnum.ELevy_Type.ELevy_Type_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _levyInfoCount = _buf.getInt();
	if(0 < _levyInfoCount){
		levyInfo = new byte[_levyInfoCount];
		_buf.get(levyInfo);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) levySum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(levyType.ordinal());

	_buf.putInt((levyInfo == null ? 0 : levyInfo.length));
	if(null != levyInfo){_buf.put(levyInfo);}

	_buf.putLong(levySum);
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

