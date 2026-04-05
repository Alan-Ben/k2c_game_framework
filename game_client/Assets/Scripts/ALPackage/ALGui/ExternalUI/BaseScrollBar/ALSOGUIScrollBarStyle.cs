using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /** Scroll Bar Direction Style */
    public enum EALGUIScrollBarDirection
    {
        VERTICAL,
        HORIZONTAL,
    }

    /**********************
     * scroll bar style info
     **/
    public class ALSOGUIScrollBarStyle : ScriptableObject
    {
        /** the scroll bar's direction */
        public EALGUIScrollBarDirection scrollBarDirection;
        /** back ground picture */
        public ALSquaredTextureObj scrollBarBakTexture;
        /** the texture picture */
        public Texture2D scrollBarTexture;
        /** the size of each state of the scroll bar picture */
        public int scrollBarTexturePageSize;
        /** the buttons' picture size */
        public int topBtnSize;
        public int dragBtnSize;
        public int bottomBtnSize;
        /** drag button picture variable */
        public int dragBtnTopSize;
        public int dragBtnBottomSize;

        /** the index of the part of different states in the Scroll Bar texture. index start from 0 */
        public int normalPartIdx;
        public int mouseOverPartIdx;
        public int mouseDownPartIdx;
        public int disablePartIdx;

        /*********************
         * draw top button for the part index
         **/
        public void drawTopBtn(int _partIdx, ALGUIBaseScrollBarBtnItem _btnItem)
        {
            if (null == scrollBarTexture)
                return;

            //calculate start x position
            int startX = _partIdx * scrollBarTexturePageSize;

            //draw the top button
            if (EALGUIScrollBarDirection.VERTICAL == scrollBarDirection)
            {
                if (startX >= scrollBarTexture.width)
                    startX = 0;

                GUI.DrawTextureWithTexCoords(new Rect(0, 0, _btnItem.width, _btnItem.height), scrollBarTexture
                    , new Rect((float)startX / scrollBarTexture.width, (float)topBtnSize / scrollBarTexture.height, (float)scrollBarTexturePageSize / scrollBarTexture.width, -(float)topBtnSize / scrollBarTexture.height));
            }
            else
            {
                if (startX >= scrollBarTexture.height)
                    startX = 0;

                GUI.DrawTextureWithTexCoords(new Rect(0, 0, _btnItem.width, _btnItem.height), scrollBarTexture
                    , new Rect(0, (float)(startX + scrollBarTexturePageSize) / scrollBarTexture.height, (float)topBtnSize / scrollBarTexture.width, -(float)scrollBarTexturePageSize / scrollBarTexture.height));
            }
        }

        /*********************
         * draw bottom button for the part index
         **/
        public void drawBottomBtn(int _partIdx, ALGUIBaseScrollBarBtnItem _btnItem)
        {
            if (null == scrollBarTexture)
                return;

            //calculate start x position
            int startX = _partIdx * scrollBarTexturePageSize;

            //draw the bottom button
            if (EALGUIScrollBarDirection.VERTICAL == scrollBarDirection)
            {
                int drawStartYPos = scrollBarTexture.height - bottomBtnSize;

                if (startX >= scrollBarTexture.width)
                    startX = 0;

                GUI.DrawTextureWithTexCoords(new Rect(0, 0, _btnItem.width, _btnItem.height), scrollBarTexture
                    , new Rect((float)startX / scrollBarTexture.width, (float)(drawStartYPos + bottomBtnSize) / scrollBarTexture.height, (float)scrollBarTexturePageSize / scrollBarTexture.width, -(float)bottomBtnSize / scrollBarTexture.height));
            }
            else
            {
                int drawStartYPos = scrollBarTexture.width - bottomBtnSize;

                if (startX >= scrollBarTexture.height)
                    startX = 0;

                GUI.DrawTextureWithTexCoords(new Rect(0, 0, _btnItem.width, _btnItem.height), scrollBarTexture
                    , new Rect((float)drawStartYPos / scrollBarTexture.width, (float)(startX + scrollBarTexturePageSize) / scrollBarTexture.height, (float)bottomBtnSize / scrollBarTexture.width, -(float)scrollBarTexturePageSize / scrollBarTexture.height));
            }
        }

        /*********************
         * draw drag button for the part index
         **/
        public void drawDragBtn(int _partIdx, ALGUIBaseScrollBarDragBtn _btnItem)
        {
            if (null == scrollBarTexture)
                return;

            //calculate start x position
            int startX = _partIdx * scrollBarTexturePageSize;

            //draw the drag button
            if (EALGUIScrollBarDirection.VERTICAL == scrollBarDirection)
            {
                if (startX >= scrollBarTexture.width)
                    startX = 0;

                //top part
                if (dragBtnTopSize > 0)
                    GUI.DrawTextureWithTexCoords(new Rect(0, 0, _btnItem.width, dragBtnTopSize), scrollBarTexture
                        , new Rect((float)startX / scrollBarTexture.width, (float)(topBtnSize + dragBtnTopSize) / scrollBarTexture.height, (float)scrollBarTexturePageSize / scrollBarTexture.width, -(float)dragBtnTopSize / scrollBarTexture.height));
                //mid part
                GUI.DrawTextureWithTexCoords(new Rect(0, dragBtnTopSize, _btnItem.width, _btnItem.height - dragBtnTopSize - dragBtnBottomSize), scrollBarTexture
                                             , new Rect((float)startX / scrollBarTexture.width, (float)(topBtnSize + dragBtnSize - dragBtnBottomSize) / scrollBarTexture.height, (float)scrollBarTexturePageSize / scrollBarTexture.width, -(float)(dragBtnSize - dragBtnTopSize - dragBtnBottomSize) / scrollBarTexture.height));
                //bottom part
                if (dragBtnBottomSize > 0)
                    GUI.DrawTextureWithTexCoords(new Rect(0, _btnItem.height - dragBtnBottomSize, _btnItem.width, dragBtnBottomSize), scrollBarTexture
                                                 , new Rect((float)startX / scrollBarTexture.width, (float)(topBtnSize + dragBtnSize) / scrollBarTexture.height, (float)scrollBarTexturePageSize / scrollBarTexture.width, -(float)dragBtnBottomSize / scrollBarTexture.height));
            }
            else
            {
                if (startX >= scrollBarTexture.height)
                    startX = 0;

                //left part
                if (dragBtnTopSize > 0)
                    GUI.DrawTextureWithTexCoords(new Rect(0, 0, dragBtnTopSize, _btnItem.height), scrollBarTexture
                        , new Rect((float)topBtnSize / scrollBarTexture.width, (float)(startX + scrollBarTexturePageSize) / scrollBarTexture.height, (float)dragBtnTopSize / scrollBarTexture.width, -(float)scrollBarTexturePageSize / scrollBarTexture.height));
                //mid part
                GUI.DrawTextureWithTexCoords(new Rect(dragBtnTopSize, 0, _btnItem.width - dragBtnTopSize - dragBtnBottomSize, _btnItem.height), scrollBarTexture
                                             , new Rect((float)(topBtnSize + dragBtnTopSize) / scrollBarTexture.width, (float)(startX + scrollBarTexturePageSize) / scrollBarTexture.height, (float)(dragBtnSize - dragBtnTopSize - dragBtnBottomSize) / scrollBarTexture.width, -(float)scrollBarTexturePageSize / scrollBarTexture.height));
                //right part
                if (dragBtnBottomSize > 0)
                    GUI.DrawTextureWithTexCoords(new Rect(_btnItem.width - dragBtnBottomSize, 0, dragBtnBottomSize, _btnItem.height), scrollBarTexture
                                                 , new Rect((float)(topBtnSize + dragBtnSize - dragBtnBottomSize) / scrollBarTexture.width, (float)(startX + scrollBarTexturePageSize) / scrollBarTexture.height, (float)dragBtnBottomSize / scrollBarTexture.width, -(float)scrollBarTexturePageSize / scrollBarTexture.height));
            }
        }
    }
}

#endif
