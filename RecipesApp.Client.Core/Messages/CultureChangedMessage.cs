using System.Globalization;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RecipesApp.Client.Core.Messages;

public class CultureChangedMessage : ValueChangedMessage<CultureInfo>
{
    public CultureChangedMessage(CultureInfo value) :
        base(value)
    {
    }
}