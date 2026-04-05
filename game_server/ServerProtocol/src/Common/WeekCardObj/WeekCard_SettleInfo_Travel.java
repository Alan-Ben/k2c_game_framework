package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-游历结算信息
 **/
public class WeekCard_SettleInfo_Travel implements ALBasicProtocolPack._IALProtocolStructure {
/** 次数 */
private int num;
/** 已获得妃子列表 */
private java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo> gotConsrtlist;
/** 未获得妃子列表 */
private java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo> ungetConsortlist;


public WeekCard_SettleInfo_Travel() {
	num = 0;
	gotConsrtlist = new java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo>();
	ungetConsortlist = new java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo>();
}

public WeekCard_SettleInfo_Travel(
	 int _num
	, java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo> _gotConsrtlist
	, java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo> _ungetConsortlist
) {	num = _num;
	gotConsrtlist = _gotConsrtlist;
	ungetConsortlist = _ungetConsortlist;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 次数 */
public int getNum() { return num; }
/** 次数 */
public void setNum(int _num) { num = _num; }
/** 已获得妃子列表 */
public java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo> getGotConsrtlist() { return gotConsrtlist; }
/** 已获得妃子列表 */
public void addGotConsrtlist(Common.WeekCardObj.WeekCard_TravelGotConsrtInfo _gotConsrtlist) { gotConsrtlist.add(_gotConsrtlist); }
/** 未获得妃子列表 */
public java.util.ArrayList<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo> getUngetConsortlist() { return ungetConsortlist; }
/** 未获得妃子列表 */
public void addUngetConsortlist(Common.WeekCardObj.WeekCard_TravelUngetConsortInfo _ungetConsortlist) { ungetConsortlist.add(_ungetConsortlist); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (gotConsrtlist.size() * 20);
	_size += 2 + (ungetConsortlist.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (gotConsrtlist.size() * 20);
	_size += 2 + (ungetConsortlist.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gotConsrtlistCount = _buf.getShort();
	for(int _i = 0; _i < _gotConsrtlistCount; _i++) { 
		Common.WeekCardObj.WeekCard_TravelGotConsrtInfo _gotConsrtlist = new Common.WeekCardObj.WeekCard_TravelGotConsrtInfo();
		if(_buf.remaining() <= 0) return;
	int __gotConsrtlistCustLen = _buf.getInt();
	int __gotConsrtlistCurPos = _buf.position();
	_gotConsrtlist.ReadUnzipBuf(_buf, __gotConsrtlistCurPos + __gotConsrtlistCustLen);
	_buf.position(__gotConsrtlistCurPos + __gotConsrtlistCustLen);

		gotConsrtlist.add(_gotConsrtlist);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _ungetConsortlistCount = _buf.getShort();
	for(int _i = 0; _i < _ungetConsortlistCount; _i++) { 
		Common.WeekCardObj.WeekCard_TravelUngetConsortInfo _ungetConsortlist = new Common.WeekCardObj.WeekCard_TravelUngetConsortInfo();
		if(_buf.remaining() <= 0) return;
	int __ungetConsortlistCustLen = _buf.getInt();
	int __ungetConsortlistCurPos = _buf.position();
	_ungetConsortlist.ReadUnzipBuf(_buf, __ungetConsortlistCurPos + __ungetConsortlistCustLen);
	_buf.position(__ungetConsortlistCurPos + __ungetConsortlistCustLen);

		ungetConsortlist.add(_ungetConsortlist);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(num);
	_buf.putShort((short)gotConsrtlist.size());
	for(int _i = 0; _i < gotConsrtlist.size(); _i++) { 
		_buf.putInt(gotConsrtlist.get(_i).GetBufSize());
	gotConsrtlist.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)ungetConsortlist.size());
	for(int _i = 0; _i < ungetConsortlist.size(); _i++) { 
		_buf.putInt(ungetConsortlist.get(_i).GetBufSize());
	ungetConsortlist.get(_i).PutUnzipBuf(_buf);
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

