using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.UI;

public class CharacterControllerMove0 : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    public float speed = 5;
    CharacterController controller;// 角色控制器
    private Vector3 direction = new Vector3(0, 0, 0);
    private MirDirection mirDirection = MirDirection.Up;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        var animator = GetComponent<Animator>();
        joystick.OnValueChanged.AddListener(v =>
        {
            if (v.magnitude != 0)
            {

                direction.x = v.x;
                direction.y = v.y;
                direction = getDirection(direction);
                if (animator.GetInteger("MirDirection") != (int)mirDirection)
                    animator.SetInteger("MirDirection", (int)mirDirection);
                if (animator.GetInteger("MirAction") != 1)
                    animator.SetInteger("MirAction", 1);
                controller.Move(direction * speed * Time.deltaTime);// 角色控制器移动
                //transform.rotation = Quaternion.LookRotation(new Vector3(v.x, 0, v.y));

            }
        });
    }



    private Vector3 getDirection(Vector3 direction)
    {
        var rad = Math.Atan2(direction.y, direction.x);// [-PI, PI]

        if ((rad >= -Math.PI / 8 && rad < 0) || (rad >= 0 && rad < Math.PI / 8))
        {
            // 右
            direction.x = 1;
            direction.y = 0;
            mirDirection = MirDirection.Right;
        }
        else if (rad >= Math.PI / 8 && rad < 3 * Math.PI / 8)
        {
            //右上
            direction.x = 1;
            direction.y = 1;
            mirDirection = MirDirection.UpRight;
        }
        else if (rad >= 3 * Math.PI / 8 && rad < 5 * Math.PI / 8)
        {
            //上
            direction.x = 0;
            direction.y = 1;
            mirDirection = MirDirection.Up;
        }
        else if (rad >= 5 * Math.PI / 8 && rad < 7 * Math.PI / 8)
        {
            // 左上
            direction.x = -1;
            direction.y = 1;
            mirDirection = MirDirection.UpLeft;
        }
        else if ((rad >= 7 * Math.PI / 8 && rad < Math.PI) || (rad >= -Math.PI && rad < -7 * Math.PI / 8))
        {
            // 左
            direction.x = -1;
            direction.y = 0;
            mirDirection = MirDirection.Left;
        }
        else if (rad >= -7 * Math.PI / 8 && rad < -5 * Math.PI / 8)
        {
            // 左下
            direction.x = -1;
            direction.y = -1;
            mirDirection = MirDirection.DownLeft;
        }
        else if (rad >= -5 * Math.PI / 8 && rad < -3 * Math.PI / 8)
        {
            // 下
            direction.x = 0;
            direction.y = -1;
            mirDirection = MirDirection.Down;
        }
        else
        {
            // 右下
            direction.x = 1;
            direction.y = -1;
            mirDirection = MirDirection.DownRight;
        }
        return direction;
    }
}
