using System.Collections.Generic;

namespace Analog_Tamigo_API.Mappers
{
    public interface IMapper<in TIn, out TOut>
    {
        IEnumerable<TOut> Map(IEnumerable<TIn> t);
    }
}
