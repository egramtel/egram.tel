using System;
using Tel.Egram.Gui.Views.Messenger.Editor;

namespace Tel.Egram.Components.Messenger.Editor
{
    public interface IEditorController : IDisposable
    {
        EditorControlModel Model { get; }
    }
}