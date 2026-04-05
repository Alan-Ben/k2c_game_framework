package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 大臣使用信息变更推送
 **/
public class GS2GC_032_079_OnHeroUseInfoChange implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣使用信息 */
private Common.GuildCooperateObj.GuildCooperate_HeroUseInfo heroUseInfo;


public GS2GC_032_079_OnHeroUseInfoChange() {
	heroUseInfo = new Common.GuildCooperateObj.GuildCooperate_HeroUseInfo();
}

public GS2GC_032_079_OnHeroUseInfoChange(
	 Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfo
) {	heroUseInfo = _heroUseInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)79; }

/** 大臣使用信息 */
public Common.GuildCooperateObj.GuildCooperate_HeroUseInfo getHeroUseInfo() { return heroUseInfo; }
/** 大臣使用信息 */
public void setHeroUseInfo(Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfo) { heroUseInfo = _heroUseInfo; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _heroUseInfoCustLen = _buf.getInt();
	int _heroUseInfoCurPos = _buf.position();
	heroUseInfo.ReadUnzipBuf(_buf, _heroUseInfoCurPos + _heroUseInfoCustLen);
	_buf.position(_heroUseInfoCurPos + _heroUseInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(heroUseInfo.GetBufSize());
	heroUseInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)79);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)79);
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

