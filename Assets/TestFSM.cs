using UnityEngine;
using System.Collections;

public class TestFSM : MonoBehaviour
{
	void Start () 
    {
        CreateFSMMachine();
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            FSM.Instance.HandleEvent("touch_down"); //runState -> jumpState
        }
        else if(Input.GetKeyDown(KeyCode.S))  
        {
            FSM.Instance.HandleEvent("touch_down"); //jumpState -> doubleJumpState
        }
        else if (Input.GetKeyDown(KeyCode.D))     
        {
            FSM.Instance.HandleEvent("land");  //jumpState/doubleJumpState -> runState
        }
    }


    private void CreateFSMMachine()
    {
        //创建状态 
        FSM.FSMState idleState = new FSM.FSMState("idle");
        FSM.FSMState runState = new FSM.FSMState("run");
        FSM.FSMState jumpState = new FSM.FSMState("jump");
        FSM.FSMState doubleJumpState = new FSM.FSMState("double_jump");
        FSM.FSMState dieState = new FSM.FSMState("die");

        //创建跳转
        FSM.FSMTranslation touchTranslation1 = new FSM.FSMTranslation(runState, "touch_down", jumpState, Jump);
        FSM.FSMTranslation touchTranslation2 = new FSM.FSMTranslation(jumpState, "touch_down", doubleJumpState, DoubleJump);

        FSM.FSMTranslation landTranslation1 = new FSM.FSMTranslation(jumpState, "land", runState, Run);
        FSM.FSMTranslation landTranslation2 = new FSM.FSMTranslation(doubleJumpState, "land", runState, Run);

        //为状态机添加状态
        FSM.Instance.AddState(idleState);
        FSM.Instance.AddState(runState);
        FSM.Instance.AddState(jumpState);
        FSM.Instance.AddState(doubleJumpState);
        FSM.Instance.AddState(dieState);

        //添加切换
        FSM.Instance.AddTranslation(touchTranslation1);
        FSM.Instance.AddTranslation(touchTranslation2);
        FSM.Instance.AddTranslation(landTranslation1);
        FSM.Instance.AddTranslation(landTranslation2);

        //启动FSM状态机
        FSM.Instance.Init(runState);
    }

    private void Jump()
    {
        Debug.Log("切换到跳跃状态");
    }

    private void DoubleJump()
    {
        Debug.Log("切换到双重跳跃状态");
    }
	
    private void Run()
    {
        Debug.Log("切换到奔跑状态");
    }
	
};
