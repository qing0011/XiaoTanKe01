using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]

public class Rotate : MonoBehaviour 
{

	#region ROTATE
	public float _sensitivity = 5f;//旋转灵敏度，数值越大旋转越快
    private Vector3 _mouseReference;//记录上一帧的鼠标位置
    private Vector3 _mouseOffset;//鼠标移动的偏移量
    private Vector3 _rotation = Vector3.zero;//要应用的旋转值
    private bool _isRotating;//是否正在旋转的状态标志

    //开始旋转模式（_isRotating = true）

    //记录当前鼠标位置作为参考点
    #endregion

    void Update()
	{
        
        if (_isRotating)
		{
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

            // 绕Y轴旋转杆子 更新鼠标参考位置
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            // 

            gameObject.transform.Rotate(_rotation);

			// store new mouse position得到一个新的位置
			_mouseReference = Input.mousePosition;
		}
	}

	void OnMouseDown()//按下开始旋转
	{
		// rotating flag
		_isRotating = true;

		// store mouse position
		_mouseReference = Input.mousePosition;
	}

	void OnMouseUp()//松开结束旋转
    {
		// rotating flag
		_isRotating = false;
	}

}