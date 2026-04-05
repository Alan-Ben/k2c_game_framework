package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 成就点变更推送
 **/
public class GS2GC_021_067_OnAchievePointChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 成就点信息 */
private Common.AchieveObj.Achieve_AchievePointInfo achievePointInfo;


public GS2GC_021_067_OnAchievePointChg() {
	achievePointInfo = new Common.AchieveObj.Achieve_AchievePointInfo();
}

public GS2GC_021_067_OnAchievePointChg(
	 Common.AchieveObj.Achieve_AchievePointInfo _achievePointInfo
) {	achievePointInfo = _achievePointInfo;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)67; }

/** 成就点信息 */
public Common.AchieveObj.Achieve_AchievePointInfo getAchievePointInfo() { return achievePointInfo; }
/** 成就点信息 */
public void setAchievePointInfo(Common.AchieveObj.Achieve_AchievePointInfo _achievePointInfo) { achievePointInfo = _achievePointInfo; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _achievePointInfoCustLen = _buf.getInt();
	int _achievePointInfoCurPos = _buf.position();
	achievePointInfo.ReadUnzipBuf(_buf, _achievePointInfoCurPos + _achievePointInfoCustLen);
	_buf.position(_achievePointInfoCurPos + _achievePointInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(achievePointInfo.GetBufSize());
	achievePointInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)67);
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

