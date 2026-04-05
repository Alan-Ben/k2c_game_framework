package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 已领取奖励点列表变更推送
 **/
public class GS2GC_032_078_OnHadDrawRewardPointListChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 信息 */
private Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList data;


public GS2GC_032_078_OnHadDrawRewardPointListChg() {
	data = new Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList();
}

public GS2GC_032_078_OnHadDrawRewardPointListChg(
	 Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _data
) {	data = _data;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)78; }

/** 信息 */
public Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList getData() { return data; }
/** 信息 */
public void setData(Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _data) { data = _data; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + data.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + data.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dataCustLen = _buf.getInt();
	int _dataCurPos = _buf.position();
	data.ReadUnzipBuf(_buf, _dataCurPos + _dataCustLen);
	_buf.position(_dataCurPos + _dataCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(data.GetBufSize());
	data.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)78);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)78);
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

