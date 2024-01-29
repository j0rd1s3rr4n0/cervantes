using Cervantes.CORE.Entities;
using Cervantes.Web.Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using Task = System.Threading.Tasks.Task;

namespace Cervantes.Web.Components.Pages.KnowledgeBase;

public partial class KnowledgeCategoryDialog: ComponentBase
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private KnowledgeBaseController _KnowledgeBaseController { get; set; }
    private string searchString = "";
    private List<KnowledgeBaseCategories> Categories = new List<KnowledgeBaseCategories>();
    DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };
    DialogOptions medium = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Update();
        StateHasChanged();
    }
    protected async Task  Update()
    {
        Categories = _KnowledgeBaseController.GetCategories().ToList();

    }
    
    async Task RowClicked(DataGridRowClickEventArgs<CORE.Entities.KnowledgeBaseCategories> args)
    {
        var parameters = new DialogParameters { ["category"]=args.Item };

        var dialog =  Dialog.Show<EditKnowledgeCategoryDialog>(@localizer["deleteTarget"], parameters, maxWidth);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await Update();
            StateHasChanged();
        }
    }
    
    private Func<CORE.Entities.KnowledgeBaseCategories, bool> _quickFilter => element =>
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.User.FullName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    };
    
    async Task AddCategory(DialogOptions options)
    {

        var dialog =  Dialog.Show<CreateKnowledgeCategoryDialog>("Edit", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await Update();
            StateHasChanged();
        }
    }
    
    
}