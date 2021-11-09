// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.Linq;
using UnityEditor;
using UnityEngine;
using VRBuilder.Core;
using VRBuilder.Editor.UI.Windows;
using VRBuilder.Editor.Configuration;

namespace VRBuilder.Editor
{
    /// <summary>
    /// This strategy is used by default and it handles interaction between course assets and various Builder windows.
    /// </summary>
    internal class DefaultEditingStrategy : IEditingStrategy
    {
        private ProcessWindow courseWindow;
        private StepWindow stepWindow;

        public IProcess CurrentCourse { get; protected set; }
        public IChapter CurrentChapter { get; protected set; }

        /// <inheritdoc/>
        public void HandleNewCourseWindow(ProcessWindow window)
        {
            courseWindow = window;
            courseWindow.SetCourse(CurrentCourse);
        }

        /// <inheritdoc/>
        public void HandleNewStepWindow(StepWindow window)
        {
            stepWindow = window;
            if (courseWindow == null || courseWindow.Equals(null))
            {
                HandleCurrentStepChanged(null);
            }
            else
            {
                HandleCurrentStepChanged(courseWindow.GetChapter().ChapterMetadata.LastSelectedStep);
            }
        }

        /// <inheritdoc/>
        public void HandleCurrentCourseModified()
        {
        }

        /// <inheritdoc/>
        public void HandleCourseWindowClosed(ProcessWindow window)
        {
            if (courseWindow != window)
            {
                return;
            }

            if (CurrentCourse != null)
            {
                ProcessAssetManager.Save(CurrentCourse);
            }
        }

        /// <inheritdoc/>
        public void HandleStepWindowClosed(StepWindow window)
        {
            if (CurrentCourse != null)
            {
                ProcessAssetManager.Save(CurrentCourse);
            }

            stepWindow = null;
        }

        /// <inheritdoc/>
        public void HandleStartEditingCourse()
        {
            if (courseWindow == null)
            {
                courseWindow = EditorWindow.GetWindow<ProcessWindow>();
                courseWindow.minSize = new Vector2(400f, 100f);
            }
            else
            {
                courseWindow.Focus();
            }
        }

        /// <inheritdoc/>
        public void HandleCurrentCourseChanged(string courseName)
        {
            if (CurrentCourse != null)
            {
                ProcessAssetManager.Save(CurrentCourse);
            }

            EditorPrefs.SetString(GlobalEditorHandler.LastEditedCourseNameKey, courseName);
            LoadCourse(ProcessAssetManager.Load(courseName));
        }

        private void LoadCourse(IProcess newCourse)
        {
            CurrentCourse = newCourse;
            CurrentChapter = null;

            if (newCourse != null && EditorConfigurator.Instance.Validation.IsAllowedToValidate())
            {
                EditorConfigurator.Instance.Validation.Validate(newCourse.Data, newCourse);
            }

            if (courseWindow != null)
            {
                courseWindow.SetCourse(CurrentCourse);
                if (stepWindow != null)
                {
                    stepWindow.SetStep(courseWindow.GetChapter()?.ChapterMetadata.LastSelectedStep);
                }
            }
            else if (stepWindow != null)
            {
                stepWindow.SetStep(null);
            }
        }

        /// <inheritdoc/>
        public void HandleCurrentStepModified(IStep step)
        {
            courseWindow.GetChapter().ChapterMetadata.LastSelectedStep = step;

            if (EditorConfigurator.Instance.Validation.IsAllowedToValidate())
            {
                EditorConfigurator.Instance.Validation.Validate(step.Data, CurrentCourse);
            }

            courseWindow.RefreshChapterRepresentation();
        }

        /// <inheritdoc/>
        public void HandleCurrentStepChanged(IStep step)
        {
            if (stepWindow != null)
            {
                if (step != null && EditorConfigurator.Instance.Validation.IsAllowedToValidate())
                {
                    EditorConfigurator.Instance.Validation.Validate(step.Data, CurrentCourse);
                }
                stepWindow.SetStep(step);
            }
        }

        /// <inheritdoc/>
        public void HandleStartEditingStep()
        {
            if (stepWindow == null)
            {
                StepWindow.ShowInspector();
            }
        }

        public void HandleCurrentChapterChanged(IChapter chapter)
        {
            CurrentChapter = chapter;
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToUnload()
        {
            if (CurrentCourse != null)
            {
                ProcessAssetManager.Save(CurrentCourse);
            }
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToSave()
        {
            if (CurrentCourse != null)
            {
                ProcessAssetManager.Save(CurrentCourse);
            }
        }

        /// <inheritdoc/>
        public void HandleExitingPlayMode()
        {
            if (stepWindow != null)
            {
                stepWindow.ResetStepView();
            }
        }

        /// <inheritdoc/>
        public void HandleEnterPlayMode()
        {
        }
    }
}
