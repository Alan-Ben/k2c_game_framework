package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟相性派遣信息
 **/
public class Guild_AttrDispatchInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 相性 */
private CommonEnum.ESpecAttrType attr;
/** 大臣列表 */
private java.util.ArrayList<Common.GuildObj.Guild_DispatchHeroInfo> heroList;


public Guild_AttrDispatchInfo() {
	attr = CommonEnum.ESpecAttrType.values()[0];
	heroList = new java.util.ArrayList<Common.GuildObj.Guild_DispatchHeroInfo>();
}

public Guild_AttrDispatchInfo(
	 CommonEnum.ESpecAttrType _attr
	, java.util.ArrayList<Common.GuildObj.Guild_DispatchHeroInfo> _heroList
) {	attr = _attr;
	heroList = _heroList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 相性 */
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/** 相性 */
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/** 大臣列表 */
public java.util.ArrayList<Common.GuildObj.Guild_DispatchHeroInfo> getHeroList() { return heroList; }
/** 大臣列表 */
public void addHeroList(Common.GuildObj.Guild_DispatchHeroInfo _heroList) { heroList.add(_heroList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (heroList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (heroList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attr = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		Common.GuildObj.Guild_DispatchHeroInfo _heroList = new Common.GuildObj.Guild_DispatchHeroInfo();
		if(_buf.remaining() <= 0) return;
	int __heroListCustLen = _buf.getInt();
	int __heroListCurPos = _buf.position();
	_heroList.ReadUnzipBuf(_buf, __heroListCurPos + __heroListCustLen);
	_buf.position(__heroListCurPos + __heroListCustLen);

		heroList.add(_heroList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(attr.ordinal());

	_buf.putShort((short)heroList.size());
	for(int _i = 0; _i < heroList.size(); _i++) { 
		_buf.putInt(heroList.get(_i).GetBufSize());
	heroList.get(_i).PutUnzipBuf(_buf);
	}
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

