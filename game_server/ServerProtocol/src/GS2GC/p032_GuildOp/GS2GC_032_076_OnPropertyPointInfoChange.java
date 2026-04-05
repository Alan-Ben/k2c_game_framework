package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 属性据点信息变更推送
 **/
public class GS2GC_032_076_OnPropertyPointInfoChange implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励据点位置 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/** 属性据点信息 */
private Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo propertyPointInfo;


public GS2GC_032_076_OnPropertyPointInfoChange() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	propertyPointInfo = new Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo();
}

public GS2GC_032_076_OnPropertyPointInfoChange(
	 Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointInfo
) {	pos = _pos;
	propertyPointInfo = _propertyPointInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)76; }

/** 奖励据点位置 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/** 奖励据点位置 */
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/** 属性据点信息 */
public Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo getPropertyPointInfo() { return propertyPointInfo; }
/** 属性据点信息 */
public void setPropertyPointInfo(Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointInfo) { propertyPointInfo = _propertyPointInfo; }


public final int GetBufSize() {
	int _size = 44;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _propertyPointInfoCustLen = _buf.getInt();
	int _propertyPointInfoCurPos = _buf.position();
	propertyPointInfo.ReadUnzipBuf(_buf, _propertyPointInfoCurPos + _propertyPointInfoCustLen);
	_buf.position(_propertyPointInfoCurPos + _propertyPointInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putInt(propertyPointInfo.GetBufSize());
	propertyPointInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)76);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)76);
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

