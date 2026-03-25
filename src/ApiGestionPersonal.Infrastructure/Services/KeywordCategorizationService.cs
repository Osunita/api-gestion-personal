using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Infrastructure.Services;

public class KeywordCategorizationService : ICategorizationService
{
    private static readonly Dictionary<string, string[]> Keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        { "trabajo", new[] { "reunión", "meeting", "conference", "trabajar", "work", "job", "oficina", "office", "proyecto", "project", "deadline", "fecha límite", "cliente", "client", "email", "correo" } },
        { "compras", new[] { "comprar", "buy", "shop", "tienda", "store", "supermercado", "grocery", "amazon", "pedido", "order" } },
        { "prioridad-alta", new[] { "urgente", "urgent", "important", "crítico", "critical", "asap", "inmediato", "immediate" } },
        { "comunicación", new[] { "llamar", "call", "phone", "teléfono", "whatsapp", "mensaje", "message", "sms", "email", "correo" } },
        { "personal", new[] { "casa", "home", "familia", "family", "amigo", "friend", "fiesta", "party", "cumpleaños", "birthday" } }
    };

    public string Categorize(string contenido)
    {
        if (string.IsNullOrWhiteSpace(contenido))
        {
            return "General";
        }

        foreach (var (category, keywords) in Keywords)
        {
            foreach (var keyword in keywords)
            {
                if (contenido.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return category;
                }
            }
        }

        return "General";
    }
}