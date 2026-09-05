using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

namespace EGamePlay.Combat
{
    [CreateAssetMenu(fileName = "????", menuName = "??|??/????")]
    [LabelText("????")]
    public class SkillConfigObject
#if !NOT_UNITY
        : SerializedScriptableObject
#endif
    {
        [LabelText("??ID"), DelayedProperty]
        public int Id;
        [LabelText("????"), DelayedProperty]
        public string Name = "??1";
        public SkillSpellType SkillSpellType;
        [LabelText("????????"), ShowIf("SkillSpellType", SkillSpellType.Initiative)]
        public SkillTargetSelectType TargetSelectType;
        [LabelText("?????"), ShowIf("TargetSelectType", SkillTargetSelectType.AreaSelect)]
        public SkillAffectAreaType AffectAreaType;
        [LabelText("???????"), ShowIf("ShowCircleAreaRadius")]
        public float CircleAreaRadius;
        public bool ShowCircleAreaRadius { get { return AffectAreaType == SkillAffectAreaType.Circle && TargetSelectType == SkillTargetSelectType.AreaSelect; } }
        [LabelText("?????"), ShowIf("TargetSelectType", SkillTargetSelectType.AreaSelect)]
        public GameObject AreaCollider;
        [LabelText("??????"), ShowIf("SkillSpellType", SkillSpellType.Initiative)]
        public SkillAffectTargetType AffectTargetType;
        [LabelText("????"), SuffixLabel("??", true), ShowIf("SkillSpellType", SkillSpellType.Initiative)]
        public uint ColdTime;

        [LabelText("??????")]
        public bool EnableChildrenStatuses;
        [OnInspectorGUI("DrawSpace", append: true)]
        [HideReferenceObjectPicker]
        [LabelText("????????"), ShowIf("EnableChildrenStatuses"), ListDrawerSettings(DraggableItems = false, ShowItemCount = false, CustomAddFunction = "AddChildStatus")]
        public List<ChildStatus> ChildrenStatuses = new List<ChildStatus>();
        private void AddChildStatus()
        {
            ChildrenStatuses.Add(new ChildStatus());
        }

        [LabelText("????"), Space(30)]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowItemCount = false, HideAddButton = true)]
        [HideReferenceObjectPicker]
        public List<Effect> Effects = new List<Effect>();

        //[ShowInInspector]
        //public static bool Draggable;

        [HorizontalGroup(PaddingLeft = 40, PaddingRight = 40)]
        [HideLabel, OnValueChanged("AddEffect"), ValueDropdown("EffectTypeSelect")]
        public string EffectTypeName = "(????)";

        public IEnumerable<string> EffectTypeSelect()
        {
            var types = typeof(Effect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(Effect).IsAssignableFrom(x))
                .Where(x => x.GetCustomAttribute<EffectAttribute>() != null && x != typeof(ActionControlEffect) && x != typeof(AttributeModifyEffect))
                .OrderBy(x => x.GetCustomAttribute<EffectAttribute>().Order)
                .Select(x => x.GetCustomAttribute<EffectAttribute>().EffectType);
            var results = types.ToList();
            results.Insert(0, "(????)");
            return results;
        }

        private void AddEffect()
        {
            if (EffectTypeName != "(????)")
            {
                var effectType = typeof(Effect).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => typeof(Effect).IsAssignableFrom(x))
                    .Where(x => x.GetCustomAttribute<EffectAttribute>() != null)
                    .Where(x => x.GetCustomAttribute<EffectAttribute>().EffectType == EffectTypeName)
                    .FirstOrDefault();

                var effect = Activator.CreateInstance(effectType) as Effect;
                effect.Enabled = true;
                effect.IsSkillEffect = true;
                Effects.Add(effect);

                EffectTypeName = "(????)";
            }
        }

#if !NOT_UNITY
        [OnInspectorGUI("BeginBox", append: false)]
        [LabelText("????")]
        public GameObject SkillExecutionAsset;
        [LabelText("????")]
        [OnInspectorGUI("EndBox", append: true)]
        public AudioClip SkillAudio;

        [TextArea, LabelText("????")]
        public string SkillDescription;
#endif

#if UNITY_EDITOR
        [LabelText("?????")]
        public bool AutoRename { get { return StatusConfigObject.AutoRenameStatic; } set { StatusConfigObject.AutoRenameStatic = value; } }

        private void OnEnable()
        {
            StatusConfigObject.AutoRenameStatic = UnityEditor.EditorPrefs.GetBool("AutoRename", true);
        }

        private void OnDisable()
        {
            UnityEditor.EditorPrefs.SetBool("AutoRename", StatusConfigObject.AutoRenameStatic);
        }

        private void DrawSpace()
        {
            GUILayout.Space(20);
        }

        private void BeginBox()
        {
            GUILayout.Space(30);
            Sirenix.Utilities.Editor.SirenixEditorGUI.DrawThickHorizontalSeparator();
            GUILayout.Space(10);
            Sirenix.Utilities.Editor.SirenixEditorGUI.BeginBox("????");
        }

        private void EndBox()
        {
            Sirenix.Utilities.Editor.SirenixEditorGUI.EndBox();
            GUILayout.Space(30);
            Sirenix.Utilities.Editor.SirenixEditorGUI.DrawThickHorizontalSeparator();
            GUILayout.Space(10);
        }

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            //foreach (var item in this.Effects)
            //{
            //    item.IsSkillEffect = true;
            //}

            if (!AutoRename)
            {
                return;
            }

            RenameFile();
        }

        [Button("???????"), HideIf("AutoRename")]
        private void RenameFile()
        {
            string[] guids = UnityEditor.Selection.assetGUIDs;
            int i = guids.Length;
            if (i == 1)
            {
                string guid = guids[0];
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<SkillConfigObject>(assetPath);
                if (so != this)
                {
                    return;
                }
                var fileName = Path.GetFileName(assetPath);
                var newName = $"Skill_{this.Id}_{this.Name}";
                if (!fileName.StartsWith(newName))
                {
                    //Debug.Log(assetPath);
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
        }
#endif
    }

    [LabelText("????")]
    public enum ShieldType
    {
        [LabelText("????")]
        Shield,
        [LabelText("????")]
        PhysicShield,
        [LabelText("????")]
        MagicShield,
        [LabelText("????")]
        SkillShield,
    }

    [LabelText("????")]
    public enum TagType
    {
        [LabelText("????")]
        Power,
    }
}