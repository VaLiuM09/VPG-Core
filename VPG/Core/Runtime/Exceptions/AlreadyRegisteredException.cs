﻿using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Exceptions
{
    public class AlreadyRegisteredException : TrainingException
    {
        public AlreadyRegisteredException(ISceneObject obj) : base(string.Format("Could not register SceneObject {0}, it's already registered!", obj))
        {
        }
    }
}
