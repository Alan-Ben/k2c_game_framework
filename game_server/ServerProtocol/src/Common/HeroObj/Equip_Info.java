package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 藏品信息
 **/
public class Equip_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础信息 */
private Common.HeroObj.Equip_BaseInfo baseInfo;
/** 技能列表 */
private java.util.ArrayList<Common.HeroObj.Equip_SkillInfo> skillList;


public Equip_Info() {
	baseInfo = new Common.HeroObj.Equip_BaseInfo();
	skillList = new java.util.ArrayList<Common.HeroObj.Equip_SkillInfo>();
}

public Equip_Info(
	 Common.HeroObj.Equip_BaseInfo _baseInfo
	, java.util.ArrayList<Common.HeroObj.Equip_SkillInfo> _skillList
) {	baseInfo = _baseInfo;
	skillList = _skillList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 基础信息 */
public Common.HeroObj.Equip_BaseInfo getBaseInfo() { return baseInfo; }
/** 基础信息 */
public void setBaseInfo(Common.HeroObj.Equip_BaseInfo _baseInfo) { baseInfo = _baseInfo; }
/** 技能列表 */
public java.util.ArrayList<Common.HeroObj.Equip_SkillInfo> getSkillList() { return skillList; }
/** 技能列表 */
public void addSkillList(Common.HeroObj.Equip_SkillInfo _skillList) { skillList.add(_skillList); }


public final int GetBufSize() {
	int _size = 41;
	_size += 2 + (skillList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 43;
	_size += 2 + (skillList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.position();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.position(_baseInfoCurPos + _baseInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _skillListCount = _buf.getShort();
	for(int _i = 0; _i < _skillListCount; _i++) { 
		Common.HeroObj.Equip_SkillInfo _skillList = new Common.HeroObj.Equip_SkillInfo();
		if(_buf.remaining() <= 0) return;
	int __skillListCustLen = _buf.getInt();
	int __skillListCurPos = _buf.position();
	_skillList.ReadUnzipBuf(_buf, __skillListCurPos + __skillListCustLen);
	_buf.position(__skillListCurPos + __skillListCustLen);

		skillList.add(_skillList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)skillList.size());
	for(int _i = 0; _i < skillList.size(); _i++) { 
		_buf.putInt(skillList.get(_i).GetBufSize());
	skillList.get(_i).PutUnzipBuf(_buf);
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

