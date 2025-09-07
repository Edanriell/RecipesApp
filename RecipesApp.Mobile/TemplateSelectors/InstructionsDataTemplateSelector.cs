using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile.TemplateSelectors;

public class InstructionsDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate InstructionTemplate { get; set; }
    public DataTemplate NoteTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is InstructionViewModel)
            return InstructionTemplate;
        if (item is NoteViewModel)
            return NoteTemplate;

        return null;
    }
}