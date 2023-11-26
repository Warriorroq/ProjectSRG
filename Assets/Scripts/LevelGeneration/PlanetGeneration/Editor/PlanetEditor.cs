using UnityEditor;
using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : Editor
    {
        private Planet _planet;
        private Editor shapeEditor;
        private Editor colorEditor;
        private void OnEnable()
        {
            _planet = (Planet)target;
        }

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if(check.changed)
                    _planet.GeneratePlanet();
            }

            if(GUILayout.Button("Generate Planet"))
                _planet.GeneratePlanet();

            DrawSettingsEditor(_planet.shapeSettings, _planet.OnShapeSettignsUpdated, ref _planet.shapeSettingsFoldout, ref shapeEditor);
            DrawSettingsEditor(_planet.colorSettings, _planet.OnColorSettingsUpdated, ref _planet.colorSettingsFoldout, ref colorEditor);
        }

        private void DrawSettingsEditor(Object settings, System.Action onSettignsUpdated,ref bool foldout, ref UnityEditor.Editor editor)
        {
            if (settings is null)
                return;
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                CreateCachedEditor(settings, null, ref editor);
                if (!foldout)
                    return;
                editor.OnInspectorGUI();
                if (check.changed && onSettignsUpdated != null)
                    onSettignsUpdated();
            }
        }
    }
}