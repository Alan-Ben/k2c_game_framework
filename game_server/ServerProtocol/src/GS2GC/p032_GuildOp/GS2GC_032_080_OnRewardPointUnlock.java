package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 联盟协作奖励据点解锁推送
 **/
public class GS2GC_032_080_OnRewardPointUnlock implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励据点位置列表 */
private java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> posList;


public GS2GC_032_080_OnRewardPointUnlock() {
	posList = new java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos>();
}

public GS2GC_032_080_OnRewardPointUnlock(
	 java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> _posList
) {	posList = _posList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)80; }

/** 奖励据点位置列表 */
public java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> getPosList() { return posList; }
/** 奖励据点位置列表 */
public void addPosList(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _posList) { posList.add(_posList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (posList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (posList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _posListCount = _buf.getShort();
	for(int _i = 0; _i < _posListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_RewardPointPos _posList = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
		if(_buf.remaining() <= 0) return;
	int __posListCustLen = _buf.getInt();
	int __posListCurPos = _buf.position();
	_posList.ReadUnzipBuf(_buf, __posListCurPos + __posListCustLen);
	_buf.position(__posListCurPos + __posListCustLen);

		posList.add(_posList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)posList.size());
	for(int _i = 0; _i < posList.size(); _i++) { 
		_buf.putInt(posList.get(_i).GetBufSize());
	posList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)80);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)80);
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

