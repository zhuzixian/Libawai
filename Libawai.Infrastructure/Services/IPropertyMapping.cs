using System.Collections.Generic;

namespace Libawai.Infrastructure.Services
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get;}
    }
}
