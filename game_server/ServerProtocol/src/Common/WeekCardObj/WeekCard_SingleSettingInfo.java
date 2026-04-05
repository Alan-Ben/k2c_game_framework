package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-设置信息
 **/
public class WeekCard_SingleSettingInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 类型 */
private CommonEnum.EWeekCardSettleType type;
/** 额外设置 */
private byte[] extraInfo;


public WeekCard_SingleSettingInfo() {
	type = CommonEnum.EWeekCardSettleType.values()[0];
	extraInfo = null;
}

public WeekCard_SingleSettingInfo(
	 CommonEnum.EWeekCardSettleType _type
	, byte[] _extraInfo
) {	type = _type;
	extraInfo = _extraInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 类型 */
public CommonEnum.EWeekCardSettleType getType() { return type; }
/** 类型 */
public void setType(CommonEnum.EWeekCardSettleType _type) { type = _type; }
/** 额外设置 */
public byte[] getExtraInfo() { return extraInfo; }
public java.nio.ByteBuffer get_buffer_ExtraInfo() { if(null == extraInfo)return null; else return ByteBuffer.wrap(extraInfo); }

/** 额外设置 */
public void setExtraInfo(byte[] _extraInfo) { extraInfo = _extraInfo; }
public void setExtraInfo(java.nio.ByteBuffer _extraInfo) 
{
	if(null == _extraInfo){return;}
	int _oldPos = _extraInfo.position();
	int _bufLength = _extraInfo.remaining();
	extraInfo = new byte[_bufLength];
	_extraInfo.get(extraInfo);
	_extraInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 4;
	_size += 4 + (extraInfo == null ? 0 : extraInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (extraInfo == null ? 0 : extraInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = CommonEnum.EWeekCardSettleType.EWeekCardSettleType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extraInfoCount = _buf.getInt();
	if(0 < _extraInfoCount){
		extraInfo = new byte[_extraInfoCount];
		_buf.get(extraInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt((extraInfo == null ? 0 : extraInfo.length));
	if(null != extraInfo){_buf.put(extraInfo);}

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

