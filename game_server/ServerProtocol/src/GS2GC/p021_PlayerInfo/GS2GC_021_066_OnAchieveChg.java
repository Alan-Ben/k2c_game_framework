package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 成就变更推送
 **/
public class GS2GC_021_066_OnAchieveChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 成就数据 */
private Common.AchieveObj.Achieve_Info achieve;


public GS2GC_021_066_OnAchieveChg() {
	achieve = new Common.AchieveObj.Achieve_Info();
}

public GS2GC_021_066_OnAchieveChg(
	 Common.AchieveObj.Achieve_Info _achieve
) {	achieve = _achieve;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)66; }

/** 成就数据 */
public Common.AchieveObj.Achieve_Info getAchieve() { return achieve; }
/** 成就数据 */
public void setAchieve(Common.AchieveObj.Achieve_Info _achieve) { achieve = _achieve; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + achieve.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + achieve.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _achieveCustLen = _buf.getInt();
	int _achieveCurPos = _buf.position();
	achieve.ReadUnzipBuf(_buf, _achieveCurPos + _achieveCustLen);
	_buf.position(_achieveCurPos + _achieveCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(achieve.GetBufSize());
	achieve.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)66);
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

