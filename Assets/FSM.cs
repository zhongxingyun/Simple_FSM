using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 简易版 FSM
/// </summary>
public class FSM
{
    //定义状态切换回调函数
    public delegate void FSMTranslationCallfunc();

    /// <summary>
    /// 状态类
    /// </summary>
    public class FSMState
    {
        public string name;

        public FSMState(string name)
        {
            this.name = name;
        }

        //存储该状态的所有 状态切换条件
        public Dictionary<string, FSMTranslation> TranslationDict = new Dictionary<string, FSMTranslation>();
    };

    /// <summary>
    /// 状态切换条件类
    /// </summary>
    public class FSMTranslation
    {
        public string name;
        public FSMState fromState;
        public FSMState toState;
        public FSMTranslationCallfunc callfunc;  //回调函数

        public FSMTranslation(FSMState fromState,string name,FSMState toState,FSMTranslationCallfunc callfunc)
        {
            this.fromState = fromState;
            this.name = name;
            this.toState = toState;
            this.callfunc = callfunc;
        }
    };

    private static FSM instance;
    private static object _lock = new object();
    public static FSM Instance
    {
        get
        { 
            if(instance == null)
            {
                lock(_lock)
                {
                    if(instance == null)
                    {
                        instance = new FSM();
                    }
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private FSMState mCurrentState;
    Dictionary<string, FSMState> stateDict = new Dictionary<string, FSMState>();

    public void AddState(FSMState state)
    {
        stateDict[state.name] = state;
    }

    public void AddTranslation(FSMTranslation translation)
    {
        stateDict[translation.fromState.name].TranslationDict[translation.name] = translation;
    }

    //1,启动FSM状态机  2,调用FSM.AddState(),FSM.AddTranslation()添加状态和切换条件  3,调用FSM.HandleEvent()执行状态切换和行为操作
    public void Init(FSMState state)
    {
        mCurrentState = state;
    }

    //执行状态跳转，并执行行为操作
    public void HandleEvent(string name)
    {
        if(mCurrentState != null && mCurrentState.TranslationDict.ContainsKey(name))
        {
            Debug.LogWarning("fromState : " + mCurrentState.name);
        }

        mCurrentState.TranslationDict[name].callfunc();  //行为操作
        mCurrentState = mCurrentState.TranslationDict[name].toState; //状态跳转

        Debug.LogWarning("toState : " + mCurrentState.name);
    }

    ~FSM()
    {
        Instance = null;
    }
};
