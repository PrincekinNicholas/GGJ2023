using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic C# Event System
public static class EventHandler
{
    public static event Action<string> E_OnAfterSceneUnload;
    public static void Call_OnAfterSceneUnload(string sceneName) { E_OnAfterSceneUnload(sceneName);}
    public static event Action E_OnBeforeSceneUnload;
    public static void Call_OnBeforeSceneUnload() => E_OnBeforeSceneUnload?.Invoke();
    public static event Action E_OnAfterSceneLoaded;
    public static void Call_OnAfterSceneLoaded() => E_OnAfterSceneLoaded?.Invoke();
#region UI
    public static event Action<int> E_UI_RefreshRootCount;
    public static void Call_UI_RefreshRootCount(int rootCount) => E_UI_RefreshRootCount?.Invoke(rootCount);
    public static event Action<string, bool> E_UI_OnSubtitle;
    public static void Call_UI_OnSubtitle(string content, bool isMiddle=false) => E_UI_OnSubtitle?.Invoke(content, isMiddle);
    public static Action<DialogueCommand> E_UI_OnShowDialogueBubble;
    public static void Call_UI_OnShowDialogueBubble(DialogueCommand dialogueCommand) => E_UI_OnShowDialogueBubble?.Invoke(dialogueCommand);
    public static Action<DialogueCommand> E_UI_OnHideDialogueBubble;
    public static void Call_UI_OnHideDialougeBubble(DialogueCommand dialogueCommand) => E_UI_OnHideDialogueBubble?.Invoke(dialogueCommand);
    #endregion
    public static Action E_OnEnterCutscene;
    public static void Call_OnEnterCutscene()=>E_OnEnterCutscene?.Invoke();
    public static Action E_OnExitCutscene;
    public static void Call_OnExitCutscene() => E_OnExitCutscene?.Invoke();
    public static event Action E_OnBreakRoot;
    public static void Call_OnBreakRoot() => E_OnBreakRoot?.Invoke();
}

//A More Strict Event System
namespace SimpleEventSystem{
    public abstract class Event{
        public delegate void Handler(Event e);
    }
    public class E_OnTestEvent:Event{
        public float value;
        public E_OnTestEvent(float data){value = data;}
    }

    public class EventManager{
        private static  EventManager instance;
        public static EventManager Instance{
            get{
                if(instance == null) instance = new EventManager();
                return instance;
            }
        }

        private Dictionary<Type, Event.Handler> RegisteredHandlers = new Dictionary<Type, Event.Handler>();
        public void Register<T>(Event.Handler handler) where T: Event{
            Type type = typeof(T);

            if(RegisteredHandlers.ContainsKey(type)){
                RegisteredHandlers[type] += handler;
            }
            else{
                RegisteredHandlers[type] = handler;
            }
        }
        public void UnRegister<T>(Event.Handler handler) where T: Event{
            Type type = typeof(T);
            Event.Handler handlers;

            if(RegisteredHandlers.TryGetValue(type, out handlers)){
                handlers -= handler;
                if(handlers == null){
                    RegisteredHandlers.Remove(type);
                }
                else{
                    RegisteredHandlers[type] = handlers;
                }
            }
        }
        public void FireEvent<T>(T e) where T:Event {
            Type type = e.GetType();
            Event.Handler handlers;

            if(RegisteredHandlers.TryGetValue(type, out handlers)){
                handlers?.Invoke(e);
            }
        }
        public void ClearList(){RegisteredHandlers.Clear();}
    }
}
