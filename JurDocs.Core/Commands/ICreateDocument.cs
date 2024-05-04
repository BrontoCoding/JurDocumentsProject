﻿using JurDocs.Core.Views;

namespace JurDocs.Core.Commands
{
    /// <summary>
    /// Создание документа
    /// </summary>
    public interface ICreateDocument
    {
        Task ExecuteAsync(IMainView mainView);
    }
}