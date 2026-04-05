using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class TestCameraOrthographicSize : MonoBehaviour
    {

        public float speed = 1.0f;
        public Vector2 moveSpeed = Vector2.one;
        private bool isAdd;
        private bool isReduce;
        private float sizeAdd = 0;

        private bool _m_bShowPanel = false;

        private string tempstring_s;
        private string tempstring_x;
        private string tempstring_z;

        // Use this for initialization
        void Start()
        {

        }

        public void OnGUI()
        {
            //处理是否展示
            if(_m_bShowPanel)
            {
                GUI.Label(new Rect(100, 60, 100, 40), "scale speed:");
                tempstring_s = GUI.TextField(new Rect(100, 100, 100, 40), speed.ToString());
                float.TryParse(tempstring_s, out speed);
                if(speed < 0)
                    speed = 0;

                GUI.Label(new Rect(100, 160, 100, 40), "move speed:");

                tempstring_x = GUI.TextField(new Rect(100, 200, 100, 40), moveSpeed.x.ToString());
                float.TryParse(tempstring_x, out moveSpeed.x);
                if(moveSpeed.x < 0)
                    moveSpeed.x = 0;

                tempstring_z = GUI.TextField(new Rect(100, 250, 100, 40), moveSpeed.y.ToString());
                float.TryParse(tempstring_z, out moveSpeed.y);
                if(moveSpeed.y < 0)
                    moveSpeed.y = 0;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.End))
            {
                _m_bShowPanel = !_m_bShowPanel;
            }

            if(Input.GetKey(KeyCode.O))
            {
                isAdd = true;
            }
            else
            {
                isAdd = false;
            }

            if(Input.GetKey(KeyCode.P))
            {
                isReduce = true;
            }
            else
            {
                isReduce = false;
            }

            if(!Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.A))
            {
                CameraController.instance.cameraPos += Vector3.right * moveSpeed.x * Time.deltaTime;
                CameraController.instance.cameraFocusPos += Vector3.right * moveSpeed.x * Time.deltaTime;
            }
            if(!Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.S))
            {
                CameraController.instance.cameraPos += Vector3.forward * moveSpeed.y * Time.deltaTime;
                CameraController.instance.cameraFocusPos += Vector3.forward * moveSpeed.y * Time.deltaTime;
            }
            if(!Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.D))
            {
                CameraController.instance.cameraPos += Vector3.left * moveSpeed.x * Time.deltaTime;
                CameraController.instance.cameraFocusPos += Vector3.left * moveSpeed.x * Time.deltaTime;
            }
            if(!Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.W))
            {
                CameraController.instance.cameraPos += Vector3.back * moveSpeed.y * Time.deltaTime;
                CameraController.instance.cameraFocusPos += Vector3.back * moveSpeed.y * Time.deltaTime;
            }

            if(Input.GetKeyDown(KeyCode.U))
            {
                //切换UI有效性
                for(int i = 0; i < Game.instance.mainCamera.uiGraphicRaycasterList.Count; i++)
                {
                    Game.instance.mainCamera.uiGraphicRaycasterList[i].gameObject.SetActive(!Game.instance.mainCamera.uiGraphicRaycasterList[i].gameObject.activeSelf);
                }
            }
        }
    }
}
