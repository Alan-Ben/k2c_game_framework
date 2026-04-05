package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 火星-火星居民数据初始化
 **/
public class GS2GC_002_074_RetMarsPeopleInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 居民数量 */
private Common.MarsObj.Mars_PeopleNum peopleNum;
/** 决策数据列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Intelligent> intelligentList;
/** 满意度万分比 */
private int satisfaction;
/** 信件数据列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Letter> letterList;
/** 求助列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Help> helpList;
/** 移民数据 */
private Common.MarsObj.Mars_PeopleImmigrant immigrant;
/** 移民次数 */
private Common.MarsObj.Mars_PeopleImmigrantCount immigrantCount;


public GS2GC_002_074_RetMarsPeopleInit() {
	peopleNum = new Common.MarsObj.Mars_PeopleNum();
	intelligentList = new java.util.ArrayList<Common.MarsObj.Mars_Intelligent>();
	satisfaction = 0;
	letterList = new java.util.ArrayList<Common.MarsObj.Mars_Letter>();
	helpList = new java.util.ArrayList<Common.MarsObj.Mars_Help>();
	immigrant = new Common.MarsObj.Mars_PeopleImmigrant();
	immigrantCount = new Common.MarsObj.Mars_PeopleImmigrantCount();
}

public GS2GC_002_074_RetMarsPeopleInit(
	 Common.MarsObj.Mars_PeopleNum _peopleNum
	, java.util.ArrayList<Common.MarsObj.Mars_Intelligent> _intelligentList
	, int _satisfaction
	, java.util.ArrayList<Common.MarsObj.Mars_Letter> _letterList
	, java.util.ArrayList<Common.MarsObj.Mars_Help> _helpList
	, Common.MarsObj.Mars_PeopleImmigrant _immigrant
	, Common.MarsObj.Mars_PeopleImmigrantCount _immigrantCount
) {	peopleNum = _peopleNum;
	intelligentList = _intelligentList;
	satisfaction = _satisfaction;
	letterList = _letterList;
	helpList = _helpList;
	immigrant = _immigrant;
	immigrantCount = _immigrantCount;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)74; }

/** 居民数量 */
public Common.MarsObj.Mars_PeopleNum getPeopleNum() { return peopleNum; }
/** 居民数量 */
public void setPeopleNum(Common.MarsObj.Mars_PeopleNum _peopleNum) { peopleNum = _peopleNum; }
/** 决策数据列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Intelligent> getIntelligentList() { return intelligentList; }
/** 决策数据列表 */
public void addIntelligentList(Common.MarsObj.Mars_Intelligent _intelligentList) { intelligentList.add(_intelligentList); }
/** 满意度万分比 */
public int getSatisfaction() { return satisfaction; }
/** 满意度万分比 */
public void setSatisfaction(int _satisfaction) { satisfaction = _satisfaction; }
/** 信件数据列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Letter> getLetterList() { return letterList; }
/** 信件数据列表 */
public void addLetterList(Common.MarsObj.Mars_Letter _letterList) { letterList.add(_letterList); }
/** 求助列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Help> getHelpList() { return helpList; }
/** 求助列表 */
public void addHelpList(Common.MarsObj.Mars_Help _helpList) { helpList.add(_helpList); }
/** 移民数据 */
public Common.MarsObj.Mars_PeopleImmigrant getImmigrant() { return immigrant; }
/** 移民数据 */
public void setImmigrant(Common.MarsObj.Mars_PeopleImmigrant _immigrant) { immigrant = _immigrant; }
/** 移民次数 */
public Common.MarsObj.Mars_PeopleImmigrantCount getImmigrantCount() { return immigrantCount; }
/** 移民次数 */
public void setImmigrantCount(Common.MarsObj.Mars_PeopleImmigrantCount _immigrantCount) { immigrantCount = _immigrantCount; }


public final int GetBufSize() {
	int _size = 56;
	_size += 2 + (intelligentList.size() * 20);
	_size += 2 + (letterList.size() * 29);
	_size += 2 + (helpList.size() * 33);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 58;
	_size += 2 + (intelligentList.size() * 20);
	_size += 2 + (letterList.size() * 29);
	_size += 2 + (helpList.size() * 33);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _peopleNumCustLen = _buf.getInt();
	int _peopleNumCurPos = _buf.position();
	peopleNum.ReadUnzipBuf(_buf, _peopleNumCurPos + _peopleNumCustLen);
	_buf.position(_peopleNumCurPos + _peopleNumCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _intelligentListCount = _buf.getShort();
	for(int _i = 0; _i < _intelligentListCount; _i++) { 
		Common.MarsObj.Mars_Intelligent _intelligentList = new Common.MarsObj.Mars_Intelligent();
		if(_buf.remaining() <= 0) return;
	int __intelligentListCustLen = _buf.getInt();
	int __intelligentListCurPos = _buf.position();
	_intelligentList.ReadUnzipBuf(_buf, __intelligentListCurPos + __intelligentListCustLen);
	_buf.position(__intelligentListCurPos + __intelligentListCustLen);

		intelligentList.add(_intelligentList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) satisfaction = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _letterListCount = _buf.getShort();
	for(int _i = 0; _i < _letterListCount; _i++) { 
		Common.MarsObj.Mars_Letter _letterList = new Common.MarsObj.Mars_Letter();
		if(_buf.remaining() <= 0) return;
	int __letterListCustLen = _buf.getInt();
	int __letterListCurPos = _buf.position();
	_letterList.ReadUnzipBuf(_buf, __letterListCurPos + __letterListCustLen);
	_buf.position(__letterListCurPos + __letterListCustLen);

		letterList.add(_letterList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _helpListCount = _buf.getShort();
	for(int _i = 0; _i < _helpListCount; _i++) { 
		Common.MarsObj.Mars_Help _helpList = new Common.MarsObj.Mars_Help();
		if(_buf.remaining() <= 0) return;
	int __helpListCustLen = _buf.getInt();
	int __helpListCurPos = _buf.position();
	_helpList.ReadUnzipBuf(_buf, __helpListCurPos + __helpListCustLen);
	_buf.position(__helpListCurPos + __helpListCustLen);

		helpList.add(_helpList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _immigrantCustLen = _buf.getInt();
	int _immigrantCurPos = _buf.position();
	immigrant.ReadUnzipBuf(_buf, _immigrantCurPos + _immigrantCustLen);
	_buf.position(_immigrantCurPos + _immigrantCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _immigrantCountCustLen = _buf.getInt();
	int _immigrantCountCurPos = _buf.position();
	immigrantCount.ReadUnzipBuf(_buf, _immigrantCountCurPos + _immigrantCountCustLen);
	_buf.position(_immigrantCountCurPos + _immigrantCountCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(peopleNum.GetBufSize());
	peopleNum.PutUnzipBuf(_buf);
	_buf.putShort((short)intelligentList.size());
	for(int _i = 0; _i < intelligentList.size(); _i++) { 
		_buf.putInt(intelligentList.get(_i).GetBufSize());
	intelligentList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(satisfaction);
	_buf.putShort((short)letterList.size());
	for(int _i = 0; _i < letterList.size(); _i++) { 
		_buf.putInt(letterList.get(_i).GetBufSize());
	letterList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)helpList.size());
	for(int _i = 0; _i < helpList.size(); _i++) { 
		_buf.putInt(helpList.get(_i).GetBufSize());
	helpList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(immigrant.GetBufSize());
	immigrant.PutUnzipBuf(_buf);
	_buf.putInt(immigrantCount.GetBufSize());
	immigrantCount.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

