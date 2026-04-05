package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 联盟协作初始化响应
 **/
public class GS2GC_002_075_RetGuildCooperateInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟协作信息 */
private Common.GuildCooperateObj.GuildCooperate_Info info;
/** 大臣使用信息列表 */
private java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo> heroUseInfoList;
/** 已领取奖励点列表 */
private Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList hadDrawRewardPointList;


public GS2GC_002_075_RetGuildCooperateInit() {
	info = new Common.GuildCooperateObj.GuildCooperate_Info();
	heroUseInfoList = new java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo>();
	hadDrawRewardPointList = new Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList();
}

public GS2GC_002_075_RetGuildCooperateInit(
	 Common.GuildCooperateObj.GuildCooperate_Info _info
	, java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo> _heroUseInfoList
	, Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _hadDrawRewardPointList
) {	info = _info;
	heroUseInfoList = _heroUseInfoList;
	hadDrawRewardPointList = _hadDrawRewardPointList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)75; }

/** 联盟协作信息 */
public Common.GuildCooperateObj.GuildCooperate_Info getInfo() { return info; }
/** 联盟协作信息 */
public void setInfo(Common.GuildCooperateObj.GuildCooperate_Info _info) { info = _info; }
/** 大臣使用信息列表 */
public java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_HeroUseInfo> getHeroUseInfoList() { return heroUseInfoList; }
/** 大臣使用信息列表 */
public void addHeroUseInfoList(Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfoList) { heroUseInfoList.add(_heroUseInfoList); }
/** 已领取奖励点列表 */
public Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList getHadDrawRewardPointList() { return hadDrawRewardPointList; }
/** 已领取奖励点列表 */
public void setHadDrawRewardPointList(Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _hadDrawRewardPointList) { hadDrawRewardPointList = _hadDrawRewardPointList; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + info.GetBufSize();
	_size += 2 + (heroUseInfoList.size() * 28);
	_size += 4 + hadDrawRewardPointList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + info.GetBufSize();
	_size += 2 + (heroUseInfoList.size() * 28);
	_size += 4 + hadDrawRewardPointList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroUseInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _heroUseInfoListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_HeroUseInfo _heroUseInfoList = new Common.GuildCooperateObj.GuildCooperate_HeroUseInfo();
		if(_buf.remaining() <= 0) return;
	int __heroUseInfoListCustLen = _buf.getInt();
	int __heroUseInfoListCurPos = _buf.position();
	_heroUseInfoList.ReadUnzipBuf(_buf, __heroUseInfoListCurPos + __heroUseInfoListCustLen);
	_buf.position(__heroUseInfoListCurPos + __heroUseInfoListCustLen);

		heroUseInfoList.add(_heroUseInfoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _hadDrawRewardPointListCustLen = _buf.getInt();
	int _hadDrawRewardPointListCurPos = _buf.position();
	hadDrawRewardPointList.ReadUnzipBuf(_buf, _hadDrawRewardPointListCurPos + _hadDrawRewardPointListCustLen);
	_buf.position(_hadDrawRewardPointListCurPos + _hadDrawRewardPointListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putShort((short)heroUseInfoList.size());
	for(int _i = 0; _i < heroUseInfoList.size(); _i++) { 
		_buf.putInt(heroUseInfoList.get(_i).GetBufSize());
	heroUseInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(hadDrawRewardPointList.GetBufSize());
	hadDrawRewardPointList.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)75);
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

