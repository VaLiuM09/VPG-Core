﻿using VRBuilder.Core.Behaviors;
using VRBuilder.TextToSpeech.Audio;
using VRBuilder.Core.Internationalization;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.TextToSpeech.UI.Behaviors
{
    /// <inheritdoc />
    public class TextToSpeechMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Audio/Play TextToSpeech Audio";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new PlayAudioBehavior(new TextToSpeechAudio(new LocalizedString()), BehaviorExecutionStages.Activation, true);
        }
    }
}
