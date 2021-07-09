﻿using System;
using VRBuilder.Core.Utils.Logging;
using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.UI
{
    internal class LoggingSettingsSection : IProjectSettingsSection
    {
        public string Title { get; } = "Course LifeCycle Logging";
        public Type TargetPageProvider { get; } = typeof(BuilderSettingsProvider);
        public int Priority { get; } = 1000;

        public void OnGUI(string searchContext)
        {
            LifeCycleLoggingConfig config = LifeCycleLoggingConfig.Instance;

            EditorGUI.BeginChangeCheck();

            config.LogChapters = GUILayout.Toggle(config.LogChapters, "Log Chapter output", BuilderEditorStyles.Toggle);
            config.LogSteps = GUILayout.Toggle(config.LogSteps, "Log Step output", BuilderEditorStyles.Toggle);
            config.LogBehaviors = GUILayout.Toggle(config.LogBehaviors, "Log Behaviors output", BuilderEditorStyles.Toggle);
            config.LogTransitions = GUILayout.Toggle(config.LogTransitions, "Log Transition output", BuilderEditorStyles.Toggle);
            config.LogConditions = GUILayout.Toggle(config.LogConditions, "Log Condition output", BuilderEditorStyles.Toggle);
        }

        ~LoggingSettingsSection()
        {
            if (EditorUtility.IsDirty(LifeCycleLoggingConfig.Instance))
            {
                LifeCycleLoggingConfig.Instance.Save();
            }
        }
    }
}
