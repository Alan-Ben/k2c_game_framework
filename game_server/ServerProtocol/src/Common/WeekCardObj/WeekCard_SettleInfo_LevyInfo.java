package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-征收结算信息
 **/
public class WeekCard_SettleInfo_LevyInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.LevyEnum.ELevy_Type type;
/** 次数 */
private int num;


public WeekCard_SettleInfo_LevyInfo() {
	type = Common.LevyEnum.ELevy_Type.values()[0];
	num = 0;
}

public WeekCard_SettleInfo_LevyInfo(
	 Common.LevyEnum.ELevy_Type _type
	, int _num
) {	type = _type;
	num = _num;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.LevyEnum.ELevy_Type getType() { return type; }
public void setType(Common.LevyEnum.ELevy_Type _type) { type = _type; }
/** 次数 */
public int getNum() { return num; }
/** 次数 */
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.LevyEnum.ELevy_Type.ELevy_Type_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt(num);
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

