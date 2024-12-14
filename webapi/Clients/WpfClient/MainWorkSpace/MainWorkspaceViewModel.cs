using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;

public class MainWorkspaceViewModel : ViewModelBase
{
    DocumentViewModel lastOpenedItem;
    ObservableCollection<WorkspaceViewModel> workspaces;
    public MainWorkspaceViewModel()
    {
    }

    public ObservableCollection<WorkspaceViewModel> Workspaces
    {
        get
        {
            if (workspaces == null)
            {
                workspaces = new ObservableCollection<WorkspaceViewModel>();
                workspaces.CollectionChanged += OnWorkspacesChanged;
            }
            return workspaces;
        }
    }

    protected void OpenOrCloseWorkspace(DocumentViewModel workspace, bool activateOnOpen = true)
    {
        if (Workspaces.Contains(workspace))
        {
            workspace.IsClosed = !workspace.IsClosed;
        }
        else
        {
            Workspaces.Add(workspace);
            workspace.IsClosed = false;
        }
        if (activateOnOpen && workspace.IsOpened)
            SetActiveWorkspace(workspace);
    }

    DocumentViewModel CreateDocumentViewModel()
    {
        return ViewModelSource<DocumentViewModel>.Create();
    }

    void OnWorkspaceRequestClose(object sender, EventArgs e)
    {
        var workspace = sender as DocumentViewModel;
        if (workspace != null)
        {
            workspace.IsClosed = true;
            if (workspace is DocumentViewModel)
            {
                workspace.Dispose();
                Workspaces.Remove(workspace);
            }
        }
    }

    void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null && e.NewItems.Count != 0)
        {
            foreach (WorkspaceViewModel workspace in e.NewItems)
            {
                workspace.RequestClose += OnWorkspaceRequestClose;
            }
        }

        if (e.OldItems != null && e.OldItems.Count != 0)
        {
            foreach (WorkspaceViewModel workspace in e.OldItems)
            {
                workspace.RequestClose -= OnWorkspaceRequestClose;
            }
        }
    }

    private void CreateItem(object content, string displayName, string iconPath)
    {
        var documentView = CreateDocumentViewModel();
        lastOpenedItem = documentView;
        lastOpenedItem.SetContent(content, displayName);
        lastOpenedItem.Glyph = String.IsNullOrWhiteSpace(iconPath) ? "" : iconPath;
        OpenOrCloseWorkspace(lastOpenedItem);
    }

    DocumentViewModel GetDocument(string identity)
    {
        return Workspaces.OfType<DocumentViewModel>().FirstOrDefault(x => x.Identity == identity);
    }

    public bool ActivateDocument(string identity)
    {
        var document = GetDocument(identity);
        bool isFound = document != null;
        if (isFound) document.IsActive = true;
        return isFound;
    }

    void SetActiveWorkspace(WorkspaceViewModel workspace)
    {
        workspace.IsActive = true;
    }

    public void OpenItem(object content, string displayName, string iconPath)
    {
        if (ActivateDocument(DocumentViewModel.GetIdentity(content, displayName))) return;
        CreateItem(content, displayName, iconPath);
    }

}
public class DocumentViewModel : WorkspaceViewModel
{
    public object Content { get; protected set; }
    public string Identity { get { return GetIdentity(Content, DisplayName); } }

    internal void SetContent(object content, string displayName)
    {
        this.Content = content;
        this.DisplayName = displayName;
    }

    public static string GetIdentity(Type content, string displayName)
    {
        return content.FullName + "^" + displayName;
    }

    public static string GetIdentity(object content, string displayName)
    {
        return GetIdentity(content.GetType(), displayName);
    }
}
