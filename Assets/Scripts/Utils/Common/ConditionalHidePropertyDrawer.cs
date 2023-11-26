using UnityEngine;
using UnityEditor;
namespace ProjectSRG.Utils.Common
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            if (enabled)
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            if (enabled)
                return EditorGUI.GetPropertyHeight(property, label);
            return -EditorGUIUtility.standardVerticalSpacing;

        }

        private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
        {
            SerializedProperty sourcePropertyValue = null;

            if (!property.isArray)
            {
                string propertyPath = property.propertyPath;
                string conditionPath = propertyPath.Replace(property.name, condHAtt.conditionalSourceField);
                sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
                if (sourcePropertyValue == null)
                    sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.conditionalSourceField);
            }
            else
                sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.conditionalSourceField);

            if (sourcePropertyValue != null)
                return CheckPropertyType(condHAtt, sourcePropertyValue);

            return true;
        }

        private bool CheckPropertyType(ConditionalHideAttribute condHAtt, SerializedProperty sourcePropertyValue)
            => sourcePropertyValue.propertyType switch
            {
                SerializedPropertyType.Boolean => sourcePropertyValue.boolValue,
                SerializedPropertyType.Enum => sourcePropertyValue.enumValueIndex == condHAtt.enumIndex,
                _ => LogThatDataTypeOfPropertyIsNotSupported(sourcePropertyValue)
            };

        private bool LogThatDataTypeOfPropertyIsNotSupported(SerializedProperty sourcePropertyValue)
        {
            Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
            return true;
        }
    }
}