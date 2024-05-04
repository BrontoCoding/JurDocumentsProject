﻿using JurDocs.Client;
using JurDocs.Core.Model;

namespace JurDocs.Core.Commands
{
    /// <summary>
    /// Сохранить проект
    /// </summary>
    public interface ISaveProject
    {
        Task ExecuteAsync(JurDocProject project);
        Task ExecuteAsync(EditedProjectData project);
    }
}