package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-详细结算信息
 **/
public class WeekCard_SettleDetailInfo implements ALBasicProtocolPack._IALProtocolStructure {
private CommonEnum.EWeekCardSettleType type;
private byte[] detailInfo;


public WeekCard_SettleDetailInfo() {
	type = CommonEnum.EWeekCardSettleType.values()[0];
	detailInfo = null;
}

public WeekCard_SettleDetailInfo(
	 CommonEnum.EWeekCardSettleType _type
	, byte[] _detailInfo
) {	type = _type;
	detailInfo = _detailInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public CommonEnum.EWeekCardSettleType getType() { return type; }
public void setType(CommonEnum.EWeekCardSettleType _type) { type = _type; }
public byte[] getDetailInfo() { return detailInfo; }
public java.nio.ByteBuffer get_buffer_DetailInfo() { if(null == detailInfo)return null; else return ByteBuffer.wrap(detailInfo); }

public void setDetailInfo(byte[] _detailInfo) { detailInfo = _detailInfo; }
public void setDetailInfo(java.nio.ByteBuffer _detailInfo) 
{
	if(null == _detailInfo){return;}
	int _oldPos = _detailInfo.position();
	int _bufLength = _detailInfo.remaining();
	detailInfo = new byte[_bufLength];
	_detailInfo.get(detailInfo);
	_detailInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 4;
	_size += 4 + (detailInfo == null ? 0 : detailInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (detailInfo == null ? 0 : detailInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = CommonEnum.EWeekCardSettleType.EWeekCardSettleType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _detailInfoCount = _buf.getInt();
	if(0 < _detailInfoCount){
		detailInfo = new byte[_detailInfoCount];
		_buf.get(detailInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt((detailInfo == null ? 0 : detailInfo.length));
	if(null != detailInfo){_buf.put(detailInfo);}

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

