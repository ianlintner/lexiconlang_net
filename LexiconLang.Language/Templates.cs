using LexiconLang.Core;

namespace LexiconLang.Language;

public static class Templates
{
    public static TranslatedName GenerateName<TContextData>(Culture culture, string nameKind, Context<TContextData> ctx)
    {
        var template = nameKind switch
        {
            "given" => culture.Templates.Given,
            "surname" => culture.Templates.Surname,
            "settlement" => culture.Templates.Settlement,
            "mountain" => culture.Templates.Mountain,
            "river" => culture.Templates.River,
            "forest" => culture.Templates.Forest,
            _ => null
        };

        if (template == null)
            throw new InvalidOperationException($"No template found for name kind '{nameKind}' in culture '{culture.Id}'");

        var lexicon = LexiconBuilder.BuildLexicon(culture, ctx);

        return template.Kind switch
        {
            NameTemplateKind.Compose => RenderCompose(template, lexicon, culture, ctx),
            NameTemplateKind.Literal => new TranslatedName(
                template.Form ?? "",
                template.Translation ?? template.Form ?? "",
                culture.Id),
            _ => throw new InvalidOperationException($"Unknown template kind: {template.Kind}")
        };
    }

    private static TranslatedName RenderCompose<TContextData>(
        NameTemplate template, Lexicon lexicon, Culture culture, Context<TContextData> ctx)
    {
        var parts = template.Parts ?? Array.Empty<NameTemplatePart>();
        var forms = new List<string>();
        var translations = new List<string>();

        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i];
            var partCtx = ctx.Child($"part:{i}");

            if (part.Literal != null)
            {
                forms.Add(part.Literal);
                translations.Add(part.Translation ?? part.Literal);
            }
            else if (part.Pick.HasValue)
            {
                var candidates = lexicon.ByClass(part.Pick.Value, part.Tag);
                if (candidates.Length == 0)
                {
                    forms.Add("?");
                    translations.Add("?");
                    continue;
                }

                var meaning = candidates[partCtx.Rng.NextInt(0, candidates.Length)];
                var form = lexicon.FormOf(meaning.Id);

                if (part.Capitalize && form.Length > 0)
                    form = char.ToUpperInvariant(form[0]) + form[1..];

                forms.Add(form);
                translations.Add(meaning.Label ?? meaning.Id);
            }
        }

        var sep = template.Sep ?? " ";
        var transSep = template.TransSep ?? sep;

        return new TranslatedName(
            string.Join(sep, forms),
            string.Join(transSep, translations),
            culture.Id,
            forms.ToArray());
    }
}