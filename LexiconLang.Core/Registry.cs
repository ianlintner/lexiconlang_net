namespace LexiconLang.Core;

public interface IRegistry
{
    void Register<T, TContextData>(IGenerator<T, TContextData> generator);
    IGenerator<T, TContextData> Get<T, TContextData>(string id);
    bool Has(string id);
    IGenerator<T, TContextData> Resolve<T, TContextData>(object reference);
    IReadOnlyList<string> List();
}

public sealed class Registry : IRegistry
{
    private readonly Dictionary<string, object> _map = new();

    public void Register<T, TContextData>(IGenerator<T, TContextData> generator)
    {
        if (_map.ContainsKey(generator.Id))
        {
            throw new InvalidOperationException($"Registry: duplicate generator id '{generator.Id}'");
        }

        _map.Add(generator.Id, generator);
    }

    public IGenerator<T, TContextData> Get<T, TContextData>(string id)
    {
        if (!_map.TryGetValue(id, out var generator))
        {
            throw new InvalidOperationException($"Registry: no generator registered for id '{id}'");
        }

        if (generator is IGenerator<T, TContextData> typedGenerator)
        {
            return typedGenerator;
        }

        throw new InvalidOperationException($"Registry: generator '{id}' has incompatible types");
    }

    public bool Has(string id) => _map.ContainsKey(id);

    public IGenerator<T, TContextData> Resolve<T, TContextData>(object reference)
    {
        return reference switch
        {
            string id => Get<T, TContextData>(id),
            IGenerator<T, TContextData> generator => generator,
            _ => throw new ArgumentException("Reference must be generator id or generator instance", nameof(reference))
        };
    }

    public IReadOnlyList<string> List() => _map.Keys.OrderBy(x => x).ToArray();
}
