using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;

public static class FakeSatchel
{
    public static void InsertCustomAction(this PlayMakerFSM fsm, string state, System.Action action, int index)
    {
        SFCore.Utils.FsmUtil.InsertMethod(fsm, state, action, index);
    }
    public static void AddCustomAction(this PlayMakerFSM fsm, string state, System.Action action)
    {
        fsm.GetState(state).AddMethod(action);
    }
    public static void AddAction(this PlayMakerFSM fsm, string state, FsmStateAction action)
    {
        fsm.GetState(state).AddAction(action);
    }
    public static FsmState AddState(this PlayMakerFSM fsm, FsmState state)
    {
        var currStates = fsm.Fsm.States;
        var states = new FsmState[currStates.Length + 1];
        var i = 0;
        for (; i < currStates.Length; i++)
        {
            states[i] = currStates[i];
        }
        states[i] = state;
        fsm.Fsm.States = states;
        return states[i];
    }
    public static FsmState AddState(this PlayMakerFSM fsm, string stateName)
    {
        return fsm.AddState(new FsmState(fsm.Fsm) { Name = stateName });
    }
    public static void RemoveAction(this PlayMakerFSM fsm, string state, int index)
    {
        fsm.GetState(state).RemoveAction(index);
    }
    public static void AddTransition(this PlayMakerFSM fsm, string from, string e, string to)
    {
        fsm.GetState(from).AddTransition(e, to);
    }
}