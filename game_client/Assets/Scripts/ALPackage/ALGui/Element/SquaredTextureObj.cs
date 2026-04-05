using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /*************************
     * 2D图片九宫格绘制对象
     *************************/
    public class SquaredTextureObj
    {
        /********************
         * 根据图片中间区域大小，使用九宫格方式绘制图片到指定位置
         **/
        public static void GUISquaredDraw(Rect _drawRect, Texture2D _pic, Rect _midScaleRect)
        {
            if (null == _pic)
                return;

            float midX = _midScaleRect.x * _pic.width;
            float midWidth = _midScaleRect.width * _pic.width;
            float midY = _midScaleRect.y * _pic.height;
            float midHeight = _midScaleRect.height * _pic.height;

            //绘制的区域对象
            Rect drawRect = new Rect(_drawRect.x, _drawRect.y, midX, midY);
            //绘制左上部分
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(0, _midScaleRect.y, _midScaleRect.x, -_midScaleRect.y));

            //计算左中部分区域
            drawRect.y = _drawRect.y + midY;
            drawRect.height = _drawRect.height - (_pic.height - midHeight);
            //绘制
            float midScaleY = _midScaleRect.y + _midScaleRect.height;
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(0, midScaleY, _midScaleRect.x, -_midScaleRect.height));

            //计算左下部分区域
            drawRect.height = _pic.height - midY - midHeight;
            drawRect.y = _drawRect.y + _drawRect.height - drawRect.height;
            //绘制
            float bottomHeightScale = _midScaleRect.y + _midScaleRect.height - 1f;
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(0, 1f, _midScaleRect.x, bottomHeightScale));

            //计算中上部区域
            drawRect.x = _drawRect.x + midX;
            drawRect.y = _drawRect.y;
            drawRect.width = _drawRect.width - (_pic.width - midWidth);
            drawRect.height = midY;
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(_midScaleRect.x, _midScaleRect.y, _midScaleRect.width, -_midScaleRect.y));

            //计算中中部区域
            drawRect.y = _drawRect.y + midY;
            drawRect.height = _drawRect.height - (_pic.height - midHeight);
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(_midScaleRect.x, midScaleY, _midScaleRect.width, -_midScaleRect.height));

            //计算中下部区域
            drawRect.height = _pic.height - midY - midHeight;
            drawRect.y = _drawRect.y + _drawRect.height - drawRect.height;
            //绘制
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(_midScaleRect.x, 1f, _midScaleRect.width, bottomHeightScale));

            float srcX = _midScaleRect.x + _midScaleRect.width;
            float rightWidth = 1f - srcX;
            //计算右上部区域
            drawRect.width = _pic.width - midX - midWidth;
            drawRect.x = _drawRect.x + _drawRect.width - drawRect.width;
            drawRect.y = _drawRect.y;
            drawRect.height = midY;
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(srcX, _midScaleRect.y, rightWidth, -_midScaleRect.y));

            //计算右中部区域
            drawRect.y = _drawRect.y + midY;
            drawRect.height = _drawRect.height - (_pic.height - midHeight);
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(srcX, midScaleY, rightWidth, -_midScaleRect.height));

            //计算右下部区域
            drawRect.height = _pic.height - midY - midHeight;
            drawRect.y = _drawRect.y + _drawRect.height - drawRect.height;
            //绘制
            GUI.DrawTextureWithTexCoords(drawRect, _pic, new Rect(srcX, 1f, rightWidth, bottomHeightScale));
        }
        public static void GraphicsSquaredDraw(Rect _drawRect, Texture2D _pic, int _leftBoard, int _rightBoard, int _topBoard, int _bottomBoard)
        {
            if (null == _pic)
                return;

            //使用九宫格绘制
            if (Event.current.type.Equals(EventType.Repaint))
                Graphics.DrawTexture(_drawRect, _pic, _leftBoard, _rightBoard, _topBoard, _bottomBoard);
        }

        protected Texture2D texture;

        protected bool useSquared;

        protected Rect squaredBoardRect;
        protected int leftBoard;
        protected int rightBoard;
        protected int topBoard;
        protected int bottomBoard;

        public SquaredTextureObj(Texture2D _texture)
        {
            if (null == _texture)
                return;

            texture = _texture;

            squaredBoardRect = new Rect();
            squaredBoardRect.x = 0;
            squaredBoardRect.width = 1f;
            squaredBoardRect.y = 0;
            squaredBoardRect.height = 1f;

            leftBoard = 0;
            rightBoard = 0;
            topBoard = 0;
            bottomBoard = 0;

            useSquared = false;
        }
        public SquaredTextureObj(Texture2D _texture, float _boardScale)
        {
            if (null == _texture)
                return;

            texture = _texture;

            squaredBoardRect = new Rect();
            squaredBoardRect.x = _boardScale;
            squaredBoardRect.width = 1f - _boardScale - _boardScale;
            squaredBoardRect.y = _boardScale;
            squaredBoardRect.height = squaredBoardRect.width;

            leftBoard = (int)(_texture.width * _boardScale);
            rightBoard = leftBoard;
            topBoard = (int)(_texture.height * _boardScale);
            bottomBoard = topBoard;

            if (1 == squaredBoardRect.width
                && 1 == squaredBoardRect.height)
                useSquared = false;
            else
                useSquared = true;
        }
        public SquaredTextureObj(Texture2D _texture, int _boardSize)
        {
            if (null == _texture)
                return;

            texture = _texture;

            squaredBoardRect = new Rect();
            squaredBoardRect.x = (float)_boardSize / (float)_texture.width;
            squaredBoardRect.width = (float)(_texture.width - _boardSize - _boardSize) / (float)_texture.width;
            squaredBoardRect.y = (float)_boardSize / (float)_texture.height;
            squaredBoardRect.height = (float)(_texture.height - _boardSize - _boardSize) / (float)_texture.height;

            leftBoard = _boardSize;
            rightBoard = _boardSize;
            topBoard = _boardSize;
            bottomBoard = _boardSize;

            if (1 == squaredBoardRect.width
                && 1 == squaredBoardRect.height)
                useSquared = false;
            else
                useSquared = true;
        }
        public SquaredTextureObj(Texture2D _texture, int _leftBoard, int _rightBoard, int _topBoard, int _bottomBoard)
        {
            if (null == _texture)
                return;

            texture = _texture;

            squaredBoardRect = new Rect();
            squaredBoardRect.x = (float)_leftBoard / (float)_texture.width;
            squaredBoardRect.width = (float)(_texture.width - _leftBoard - _rightBoard) / (float)_texture.width;
            squaredBoardRect.y = (float)_topBoard / (float)_texture.height;
            squaredBoardRect.height = (float)(_texture.height - _topBoard - _bottomBoard) / (float)_texture.height;

            leftBoard = _leftBoard;
            rightBoard = _rightBoard;
            topBoard = _topBoard;
            bottomBoard = _bottomBoard;

            if (1 == squaredBoardRect.width
                && 1 == squaredBoardRect.height)
                useSquared = false;
            else
                useSquared = true;
        }
        public SquaredTextureObj(Texture2D _texture, float _leftBoardScale, float _rightBoardScale, float _topBoardScale, float _bottomBoardScale)
        {
            if (null == _texture)
                return;

            texture = _texture;

            squaredBoardRect = new Rect();
            squaredBoardRect.x = _leftBoardScale;
            squaredBoardRect.width = 1f - _leftBoardScale - _rightBoardScale;
            squaredBoardRect.y = _topBoardScale;
            squaredBoardRect.height = 1f - _topBoardScale - _bottomBoardScale;

            leftBoard = (int)(_leftBoardScale * _texture.width);
            rightBoard = (int)(_rightBoardScale * _texture.width);
            topBoard = (int)(_topBoardScale * _texture.height);
            bottomBoard = (int)(_bottomBoardScale * _texture.height);

            if (1 == squaredBoardRect.width
                && 1 == squaredBoardRect.height)
                useSquared = false;
            else
                useSquared = true;
        }
        public SquaredTextureObj(ALSquaredTextureObj _obj)
        {
            if (null == _obj || null == _obj.texture)
            {
                Debug.LogError("Init SquaredTextureObj use a Null ALSOSquaredTextureObj");
                return;
            }

            texture = _obj.texture;

            squaredBoardRect = new Rect();
            squaredBoardRect.x = (float)_obj.leftBoard / (float)texture.width;
            squaredBoardRect.width = (float)(texture.width - _obj.leftBoard - _obj.rightBoard) / (float)texture.width;
            squaredBoardRect.y = (float)_obj.topBoard / (float)texture.height;
            squaredBoardRect.height = (float)(texture.height - _obj.topBoard - _obj.bottomBoard) / (float)texture.height;

            leftBoard = _obj.leftBoard;
            rightBoard = _obj.rightBoard;
            topBoard = _obj.topBoard;
            bottomBoard = _obj.bottomBoard;

            if (1 == squaredBoardRect.width
                && 1 == squaredBoardRect.height)
                useSquared = false;
            else
                useSquared = true;
        }

        /********************
         * 将九宫格图片绘制到GUI上
         ********************/
        public void GUIDraw(Rect _drawRect)
        {
            if (null == texture)
                return;

            if (useSquared)
                GUISquaredDraw(_drawRect, texture, squaredBoardRect);
            else
                GUI.DrawTexture(_drawRect, texture);
        }
        public void GraphicsDraw(Rect _drawRect)
        {
            if (null == texture)
                return;

            if (Event.current.type.Equals(EventType.Repaint))
            {
                if (useSquared)
                    Graphics.DrawTexture(_drawRect, texture, leftBoard, rightBoard, topBoard, bottomBoard);
                else
                    Graphics.DrawTexture(_drawRect, texture);
            }
        }
    }
}

#endif
