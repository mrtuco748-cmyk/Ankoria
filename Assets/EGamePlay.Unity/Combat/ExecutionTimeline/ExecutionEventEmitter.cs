#if !SERVER
#if UNITY_EDITOR
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine.Timeline;
#endif

public enum ColliderShape
{
    Box,
    Circle,
    Sector,
}

public enum ColliderType
{
    FixedPosition,
    FixedDirection,
    TargetFly,
    ForwardFly,
}

public enum EffectApplyType
{
    AllEffects,
    Effect1,
    Effect2,
    Effect3,
    Other = 100,
}

public enum ExecutionEventType
{
    TriggerApplyEffect,
    TriggerSpawnCollider,
}

#if !SERVER
public class ExecutionEventEmitter : SignalEmitter
{
    public ExecutionEventType ExecutionEventType;
    public string ColliderName;
    public ColliderType ColliderType;
    public float ExistTime;
    public EffectApplyType EffectApplyType;


    public override void OnInitialize(TrackAsset aPent)
    {
        base.OnInitialize(aPent);
        retroactive = true;
        emitOnce = true;
    }
}

#if UNITY_EDITOR
#if ODIN_INSPECTOR
[CustomEditor(typeof(ExecutionEventEmitter))]
public class ExecutionEventEmitterInspector : OdinEditor
{
    protected override void OnEnable()
    {
        base.OnEnable();
        var emitter = target as ExecutionEventEmitter;
        if (emitter.asset == null)
        {
            SignalAsset signalAsset = null;
            var arr = AssetDatabase.FindAssets("t:SignalAsset", new string[] { "Assets" });
            foreach (var item in arr)
            {
                signalAsset = AssetDatabase.LoadAssetAtPath<SignalAsset>(AssetDatabase.GUIDToAssetPath(item));
                if (signalAsset != null) break;
            }
            emitter.asset = signalAsset;
            serializedObject.ApplyModifiedProperties();
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var emitter = target as ExecutionEventEmitter;
        emitter.time = EditorGUILayout.FloatField("Time", (float)emitter.time);
        emitter.retroactive = EditorGUILayout.Toggle("Retroactive", emitter.retroactive);
        emitter.emitOnce = EditorGUILayout.Toggle("EmitOnce", emitter.emitOnce);
        EditorGUILayout.Space(20);
        emitter.ExecutionEventType = (ExecutionEventType)SirenixEditorFields.EnumDropdown("事件类型", emitter.ExecutionEventType);
        if (emitter.ExecutionEventType == ExecutionEventType.TriggerSpawnCollider)
        {
            emitter.ColliderName = EditorGUILayout.TextField("碰撞体名称", emitter.ColliderName);
            emitter.ColliderType = (ColliderType)SirenixEditorFields.EnumDropdown("碰撞体类型", emitter.ColliderType);
            if (emitter.ColliderType == ColliderType.FixedDirection
                || emitter.ColliderType == ColliderType.FixedPosition
                || emitter.ColliderType == ColliderType.ForwardFly)
            {
                emitter.ExistTime = EditorGUILayout.FloatField("存活时间", emitter.ExistTime);
            }
            emitter.EffectApplyType = (EffectApplyType)EditorGUILayout.EnumPopup("应用效果", emitter.EffectApplyType);
        }
        if (emitter.ExecutionEventType == ExecutionEventType.TriggerApplyEffect)
        {
            emitter.EffectApplyType = (EffectApplyType)EditorGUILayout.EnumPopup("应用效果", emitter.EffectApplyType);
        }
        serializedObject.ApplyModifiedProperties();
        if (!EditorUtility.IsDirty(emitter)) EditorUtility.SetDirty(emitter);
    }
}
#else
[CustomEditor(typeof(ExecutionEventEmitter))]
public class ExecutionEventEmitterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var emitter = target as ExecutionEventEmitter;
        emitter.time = EditorGUILayout.FloatField("Time", (float)emitter.time);
        emitter.retroactive = EditorGUILayout.Toggle("Retroactive", emitter.retroactive);
        emitter.emitOnce = EditorGUILayout.Toggle("EmitOnce", emitter.emitOnce);
        EditorGUILayout.Space(10);
        emitter.ExecutionEventType = (ExecutionEventType)EditorGUILayout.EnumPopup("事件类型", emitter.ExecutionEventType);
        if (emitter.ExecutionEventType == ExecutionEventType.TriggerSpawnCollider)
        {
            emitter.ColliderName = EditorGUILayout.TextField("碰撞体名称", emitter.ColliderName);
            emitter.ColliderType = (ColliderType)EditorGUILayout.EnumPopup("碰撞体类型", emitter.ColliderType);
            if (emitter.ColliderType == ColliderType.FixedDirection || emitter.ColliderType == ColliderType.FixedPosition || emitter.ColliderType == ColliderType.ForwardFly)
                emitter.ExistTime = EditorGUILayout.FloatField("存活时间", emitter.ExistTime);
            emitter.EffectApplyType = (EffectApplyType)EditorGUILayout.EnumPopup("应用效果", emitter.EffectApplyType);
        }
        if (emitter.ExecutionEventType == ExecutionEventType.TriggerApplyEffect)
            emitter.EffectApplyType = (EffectApplyType)EditorGUILayout.EnumPopup("应用效果", emitter.EffectApplyType);
        serializedObject.ApplyModifiedProperties();
        if (!EditorUtility.IsDirty(emitter)) EditorUtility.SetDirty(emitter);
    }
}
#endif
#endif
#endif