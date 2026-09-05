using System;

namespace Sirenix.OdinInspector {
    [AttributeUsage(AttributeTargets.All)] public class LabelTextAttribute : Attribute { public LabelTextAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class BoxGroupAttribute : Attribute { public BoxGroupAttribute(string t) {} public BoxGroupAttribute(string t, bool b) {} }
    [AttributeUsage(AttributeTargets.All)] public class FoldoutGroupAttribute : Attribute { public FoldoutGroupAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class TabGroupAttribute : Attribute { public TabGroupAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class ShowIfAttribute : Attribute { public ShowIfAttribute(string t) {} public ShowIfAttribute(string t, object v) {} }
    [AttributeUsage(AttributeTargets.All)] public class HideIfAttribute : Attribute { public HideIfAttribute(string t) {} public HideIfAttribute(string t, object v) {} public HideIfAttribute(string t, bool v) {} }
    [AttributeUsage(AttributeTargets.All)] public class SuffixLabelAttribute : Attribute { public SuffixLabelAttribute(string t, bool b=false) {} }
    [AttributeUsage(AttributeTargets.All)] public class PropertyOrderAttribute : Attribute { public PropertyOrderAttribute(float o) {} }
    [AttributeUsage(AttributeTargets.All)] public class TitleGroupAttribute : Attribute { public TitleGroupAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class DetailedInfoBoxAttribute : Attribute { public DetailedInfoBoxAttribute(string t, string d) {} }
    [AttributeUsage(AttributeTargets.All)] public class PaddingRightAttribute : Attribute { public PaddingRightAttribute(float p) {} }
    [AttributeUsage(AttributeTargets.All)] public class DisableIfAttribute : Attribute { public DisableIfAttribute(string t) {} public DisableIfAttribute(string t, object v) {} }
    [AttributeUsage(AttributeTargets.All)] public class EnableIfAttribute : Attribute { public EnableIfAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class PreviewFieldAttribute : Attribute { public PreviewFieldAttribute() {} }
    [AttributeUsage(AttributeTargets.All)] public class ReadOnlyAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.All)] public class ButtonAttribute : Attribute { public ButtonAttribute(string t=null) {} }
    [AttributeUsage(AttributeTargets.All)] public class GUIColorAttribute : Attribute { public GUIColorAttribute(float r,float g,float b) {} }
    [AttributeUsage(AttributeTargets.All)] public class PropertySpaceAttribute : Attribute { public PropertySpaceAttribute(float s=0) {} }
    [AttributeUsage(AttributeTargets.All)] public class InlineEditorAttribute : Attribute { public InlineEditorAttribute() {} public InlineEditorAttribute(int i) {} }
    [AttributeUsage(AttributeTargets.All)] public class ValueDropdownAttribute : Attribute { public ValueDropdownAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class OnValueChangedAttribute : Attribute { public OnValueChangedAttribute(string t) {} }
    [AttributeUsage(AttributeTargets.All)] public class RequiredAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.All)] public class InfoBoxAttribute : Attribute { public InfoBoxAttribute(string t) {} public InfoBoxAttribute(string t, InfoMessageType m) {} }
    [AttributeUsage(AttributeTargets.All)] public class HideReferenceObjectPickerAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.All)] public class HideLabelAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.All)] public class HorizontalGroupAttribute : Attribute { public HorizontalGroupAttribute(string t) {} public HorizontalGroupAttribute() {} public float PaddingLeft; public float PaddingRight; public float Width; }
    [AttributeUsage(AttributeTargets.All)] public class ToggleGroupAttribute : Attribute { public ToggleGroupAttribute(string t) {} public ToggleGroupAttribute(string t, string s) {} }
    [AttributeUsage(AttributeTargets.All)] public class ListDrawerSettingsAttribute : Attribute { public bool DraggableItems; public bool ShowItemCount; public string CustomAddFunction; public bool HideAddButton; public bool Expanded; public bool ShowFoldout; }
    [AttributeUsage(AttributeTargets.All)] public class OnInspectorGUIAttribute : Attribute { public OnInspectorGUIAttribute() {} public OnInspectorGUIAttribute(string t) {} public OnInspectorGUIAttribute(string t, bool append) {} public string PrependMethodName; public string AppendMethodName; public bool append; }
    [AttributeUsage(AttributeTargets.All)] public class DelayedPropertyAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.All)] public class EffectDecorateAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.All)] public class TypeDrawerAttribute : Attribute {}
    public enum InfoMessageType { None, Info, Warning, Error }
    public class SerializedScriptableObject : UnityEngine.ScriptableObject {}
    public class SerializedMonoBehaviour : UnityEngine.MonoBehaviour {}
}
namespace Sirenix.OdinInspector.Editor {
    public class OdinEditor : UnityEditor.Editor {}
    public class OdinEditorWindow : UnityEditor.EditorWindow { protected virtual void OnEnable() {} protected virtual void OnGUI() {} protected virtual void OnDisable() {} protected virtual void OnDestroy() {} }
    public static class SirenixEditorFields { public static Enum EnumDropdown(string l, Enum v) => v; }
}
namespace Sirenix.Utilities { 
    public static class GUIHelper { public static void RequestRepaint() {} public static UnityEngine.Rect GetEditorWindowRect() => new UnityEngine.Rect(0,0,700,700); public static UnityEngine.Rect AlignCenter(this UnityEngine.Rect r, float w, float h) => r; }
    public static class ListExtensions { public static void SetLength<T>(this System.Collections.Generic.List<T> list, int len) { while(list.Count < len) list.Add(default(T)); while(list.Count > len) list.RemoveAt(list.Count-1); } }
    public static class RectExtensions { public static UnityEngine.Rect Padding(this UnityEngine.Rect r, float p) => new UnityEngine.Rect(r.x+p,r.y+p,r.width-p*2,r.height-p*2); }
}
namespace Sirenix.Utilities.Editor {
    public static class SirenixEditorGUI {
        public static void DrawThickHorizontalSeparator(int a=0,int b=0) {}
        public static void DrawThickHorizontalSeparator() {}
        public static void BeginBox(string t) {}
        public static void EndBox() {}
        public static void BeginBox() {}
    }
}
public static class SirenixEditorGUI {
    public static void DrawThickHorizontalSeparator() {}
    public static void BeginBox(string t) {}
    public static void EndBox() {}
}

