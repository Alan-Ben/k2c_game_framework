package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 礼包组变更通知
 **/
public class GS2GC_004_074_OnPushGiftPackGroupChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包组信息 */
private Common.PushGiftObj.PushGift_GroupInfo groupInfo;


public GS2GC_004_074_OnPushGiftPackGroupChg() {
	groupInfo = new Common.PushGiftObj.PushGift_GroupInfo();
}

public GS2GC_004_074_OnPushGiftPackGroupChg(
	 Common.PushGiftObj.PushGift_GroupInfo _groupInfo
) {	groupInfo = _groupInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)74; }

/** 礼包组信息 */
public Common.PushGiftObj.PushGift_GroupInfo getGroupInfo() { return groupInfo; }
/** 礼包组信息 */
public void setGroupInfo(Common.PushGiftObj.PushGift_GroupInfo _groupInfo) { groupInfo = _groupInfo; }


public final int GetBufSize() {
	int _size = 42;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 44;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _groupInfoCustLen = _buf.getInt();
	int _groupInfoCurPos = _buf.position();
	groupInfo.ReadUnzipBuf(_buf, _groupInfoCurPos + _groupInfoCustLen);
	_buf.position(_groupInfoCurPos + _groupInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(groupInfo.GetBufSize());
	groupInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)74);
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

