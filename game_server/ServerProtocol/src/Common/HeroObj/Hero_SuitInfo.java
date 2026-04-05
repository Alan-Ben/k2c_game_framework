package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 大臣套系信息
 **/
public class Hero_SuitInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 套系id */
private long suitId;
/** 套系技能列表 */
private java.util.ArrayList<Common.HeroObj.Hero_SuitSkillInfo> suitSkillList;


public Hero_SuitInfo() {
	suitId = (long)0;
	suitSkillList = new java.util.ArrayList<Common.HeroObj.Hero_SuitSkillInfo>();
}

public Hero_SuitInfo(
	 long _suitId
	, java.util.ArrayList<Common.HeroObj.Hero_SuitSkillInfo> _suitSkillList
) {	suitId = _suitId;
	suitSkillList = _suitSkillList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 套系id */
public long getSuitId() { return suitId; }
/** 套系id */
public void setSuitId(long _suitId) { suitId = _suitId; }
/** 套系技能列表 */
public java.util.ArrayList<Common.HeroObj.Hero_SuitSkillInfo> getSuitSkillList() { return suitSkillList; }
/** 套系技能列表 */
public void addSuitSkillList(Common.HeroObj.Hero_SuitSkillInfo _suitSkillList) { suitSkillList.add(_suitSkillList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (suitSkillList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (suitSkillList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) suitId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _suitSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _suitSkillListCount; _i++) { 
		Common.HeroObj.Hero_SuitSkillInfo _suitSkillList = new Common.HeroObj.Hero_SuitSkillInfo();
		if(_buf.remaining() <= 0) return;
	int __suitSkillListCustLen = _buf.getInt();
	int __suitSkillListCurPos = _buf.position();
	_suitSkillList.ReadUnzipBuf(_buf, __suitSkillListCurPos + __suitSkillListCustLen);
	_buf.position(__suitSkillListCurPos + __suitSkillListCustLen);

		suitSkillList.add(_suitSkillList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(suitId);
	_buf.putShort((short)suitSkillList.size());
	for(int _i = 0; _i < suitSkillList.size(); _i++) { 
		_buf.putInt(suitSkillList.get(_i).GetBufSize());
	suitSkillList.get(_i).PutUnzipBuf(_buf);
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

