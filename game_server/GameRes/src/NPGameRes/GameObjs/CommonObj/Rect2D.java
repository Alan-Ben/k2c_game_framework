package NPGameRes.GameObjs.CommonObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.Pair.WCGPairInteger;
import NPCommon.Util.Random;
import NPGameRes.GameObjs.Battle.Vector2;

/******************
 * 进行区域相关处理时的2D区域数据对象
 * @author mj
 *
 */
public class Rect2D implements _IParseFromStringable
{
    private float _m_fX;
    private float _m_fY;
    private float _m_fWidth;
    private float _m_fHeight;

    public Rect2D()
    {
    }

    public Rect2D(float _x, float _y, float _width, float _height)
    {
        _m_fX = _x;
        _m_fY = _y;
        _m_fWidth = _width;
        _m_fHeight = _height;
    }

    public float getX()
    {
        return _m_fX;
    }

    public float getY()
    {
        return _m_fY;
    }

    public float getWidth()
    {
        return _m_fWidth;
    }

    public float getHeight()
    {
        return _m_fHeight;
    }

    public void setX(float _x)
    {
        _m_fX = _x;
    }

    public void setY(float _y)
    {
        _m_fY = _y;
    }

    public void setWidth(float _width)
    {
        _m_fWidth = _width;
    }

    public void setHeight(float _height)
    {
        _m_fHeight = _height;
    }

    /**************
     * 设置区域值
     * @param _rect
     */
    public void setValue(Rect2D _rect)
    {
        _m_fX = _rect._m_fX;
        _m_fY = _rect._m_fY;
        _m_fWidth = _rect._m_fWidth;
        _m_fHeight = _rect._m_fHeight;
    }

    /************
     * 增加边界宽度和高度
     * @param _edgeX
     * @param _edgeY
     */
    public void addEdge(float _edgeX, float _edgeY)
    {
        //根据原宽度和高度是否反向进行新的参考点计算
        _m_fX = _m_fWidth > 0 ? _m_fX - _edgeX : _m_fX + _edgeX;
        _m_fY = _m_fHeight > 0 ? _m_fY - _edgeY : _m_fY + _edgeY;
        _m_fWidth = _m_fWidth > 0 ? _m_fWidth + _edgeX : _m_fWidth - _edgeX;
        _m_fHeight = _m_fHeight > 0 ? _m_fHeight + _edgeY : _m_fHeight - _edgeY;
    }

    /****************
     * 判断坐标是否在区域内
     * @param _pos
     * @return
     */
    public boolean isInRect(Vector2 _pos)
    {
        return isInRect(_pos.x, _pos.y);
    }

    public boolean isInRect(float _x, float _y)
    {
        //宽度和高度是否大于0分别做不同的上下限处理
        if (_m_fWidth > 0)
        {
            //宽度大于0时，边界为x和x + width
            if (_x < _m_fX)
                return false;

            if (_x > _m_fX + _m_fWidth)
                return false;
        } else
        {
            //宽度小于等于0时，边界为x + width和x
            if (_x < _m_fX + _m_fWidth)
                return false;

            if (_x > _m_fX)
                return false;
        }

        if (_m_fHeight > 0)
        {
            //高度大于0时，边界为y和y + height
            if (_y < _m_fY)
                return false;

            if (_y > _m_fY + _m_fHeight)
                return false;
        } else
        {
            //高度小于等于0时，边界为y + height和y
            if (_y < _m_fY + _m_fHeight)
                return false;

            if (_y > _m_fY)
                return false;
        }

        //没有超出边界的都是范围内的
        return true;
    }

    /****************
     * 增加额外边界范围后，判断坐标是否在区域内
     * 边界如果大于0则外扩计算
     * 边界如果小于0则缩小计算
     * @param _pos
     * @return
     */
    public boolean isInRect(Vector2 _pos, Vector2 _edgeSize)
    {
        return isInRect(_pos.x, _pos.y, _edgeSize.x, _edgeSize.y);
    }

    public boolean isInRect(float _x, float _y, float _xEdgeWidth, float _yEdgeHeight)
    {
        //根据原宽度和高度是否反向进行新的参考点计算
        float tmpX = _m_fWidth > 0 ? _m_fX - _xEdgeWidth : _m_fX + _xEdgeWidth;
        float tmpY = _m_fHeight > 0 ? _m_fY - _yEdgeHeight : _m_fY + _yEdgeHeight;
        float tmpWidth = _m_fWidth > 0 ? _m_fWidth + _xEdgeWidth : _m_fWidth - _xEdgeWidth;
        float tmpHeight = _m_fHeight > 0 ? _m_fHeight + _yEdgeHeight : _m_fHeight - _yEdgeHeight;

        //宽度和高度是否大于0分别做不同的上下限处理
        if (tmpWidth > 0)
        {
            //宽度大于0时，边界为x和x + width
            if (_x < tmpX)
                return false;

            if (_x > tmpX + tmpWidth)
                return false;
        } else
        {
            //宽度小于等于0时，边界为x + width和x
            if (_x < tmpX + tmpWidth)
                return false;

            if (_x > tmpX)
                return false;
        }

        if (tmpHeight > 0)
        {
            //高度大于0时，边界为y和y + height
            if (_y < tmpY)
                return false;

            if (_y > tmpY + tmpHeight)
                return false;
        } else
        {
            //高度小于等于0时，边界为y + height和y
            if (_y < tmpY + tmpHeight)
                return false;

            if (_y > tmpY)
                return false;
        }

        //没有超出边界的都是范围内的
        return true;
    }

    @Override
    public boolean parseFromString(String _sValue)
    {
        NPStringReader sr = new NPStringReader(_sValue);
        try
        {
            String x = sr.readItem(';');
            _m_fX = Float.parseFloat(x);

            String y = sr.readItem(';');
            _m_fY = Float.parseFloat(y);

            String weight = sr.readItem(';');
            _m_fWidth = Float.parseFloat(weight);

            String high = sr.readItem(';');
            _m_fHeight = Float.parseFloat(high);

            return true;
        } catch (Exception e)
        {
            e.printStackTrace();
        }
        return false;
    }

    /**
     * 获取这个范围内的一个随机位置
     * 位置坐标放大了10000倍，规避浮点型数值
     * @return WCGPairLong
     */
    public WCGPairInteger randomXY()
    {
        //float转int，
        long maxWidth = (long) (_m_fWidth * 10000);
        long maxHigh = (long) (_m_fHeight * 10000);
        //随机x偏移，y偏移
        long randomWidth = Random.nextLong(maxWidth);
        long randomHigh = Random.nextLong(maxHigh);

        WCGPairInteger randomXY = new WCGPairInteger((int) (_m_fX * 10000L + randomWidth), (int) (_m_fY * 10000L + randomHigh));
        return randomXY;
    }
}
